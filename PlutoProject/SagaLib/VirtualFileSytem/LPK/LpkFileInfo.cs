using System.IO;

namespace SagaLib.VirtualFileSytem.LPK
{
    public class LpkFileInfo
    {
        public LpkFileInfo()
        {
        }

        public LpkFileInfo(Stream stream)
        {
            var br = new BinaryReader(stream);
            HeaderOffset = (uint)stream.Position;
            DataOffset = br.ReadUInt32() ^ 265851106;
            FileSize = br.ReadUInt32() ^ 852870806;
            UncompressedSize = br.ReadUInt32() ^ 511060806;
            CRC = br.ReadUInt32() ^ 987654321;
        }

        public uint HeaderOffset { get; set; }

        public uint DataOffset { get; set; }

        public uint FileSize { get; set; }

        public uint UncompressedSize { get; set; }

        public uint CRC { get; set; }

        public string Name { get; set; }

        public static int Size => 16;

        public void WriteToStream(Stream stream)
        {
            var bw = new BinaryWriter(stream);
            stream.Position = HeaderOffset;
            bw.Write(DataOffset ^ 265851106);
            bw.Write(FileSize ^ 852870806);
            bw.Write(UncompressedSize ^ 511060806);
            bw.Write(CRC ^ 987654321);
        }
    }
}