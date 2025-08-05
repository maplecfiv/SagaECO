using System;
using System.IO;
using System.IO.Compression;

namespace SagaLib.VirtualFileSytem.LPK
{
    public class LpkOutputStream : Stream
    {
        private readonly Stream baseStream;

        private readonly GZipStream gzip;
        private readonly LpkFileInfo info;

        public LpkOutputStream(Stream lpk, LpkFileInfo file)
        {
            baseStream = lpk;
            info = file;
            baseStream.Position = file.DataOffset;
            gzip = new GZipStream(baseStream, CompressionMode.Decompress, true);
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => info.UncompressedSize;

        public long CompressedLength => info.FileSize;

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Close()
        {
            base.Close();
            gzip.Close();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return gzip.Read(buffer, offset, count);
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
            throw new NotSupportedException();
        }
    }
}