using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NCSHOP_INFO : Packet
    {
        public SSMG_NCSHOP_INFO()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x0630;
        }

        public uint Point
        {
            set => PutUInt(value, 2);
        }

        public uint ItemID
        {
            set => PutUInt(value, 6);
        }

        public string Comment
        {
            set
            {
                var comment = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[11 + comment.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)comment.Length, 10);
                PutBytes(comment, 11);
            }
        }
    }
}