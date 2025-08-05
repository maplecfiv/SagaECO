using System;
using System.IO;
using System.IO.Compression;

namespace SagaLib.VirtualFileSytem.LPK
{
    public class LpkInputStream : Stream
    {
        private readonly Stream baseStream;

        private readonly GZipStream gzip;
        private readonly LpkFileInfo info;

        public LpkInputStream(Stream lpk, LpkFileInfo file)
        {
            baseStream = lpk;
            info = file;
            baseStream.Position = file.DataOffset;
            gzip = new GZipStream(baseStream, CompressionMode.Compress, true);
            file.FileSize = 0;
            file.UncompressedSize = 0;
        }

        public override bool CanRead => false;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => info.UncompressedSize;

        public long CompressedLength => baseStream.Position - info.DataOffset;

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Close()
        {
            base.Close();
            gzip.Close();
            info.FileSize = (uint)CompressedLength;
            info.WriteToStream(baseStream);
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            gzip.Write(buffer, offset, count);
            info.UncompressedSize += (uint)count;
        }
    }
}