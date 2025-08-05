using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_BUY_SETUP : Packet
    {
        public SSMG_GOLEM_SHOP_BUY_SETUP()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x181B;
            MaxItemCount = 32;
        }

        public uint Unknown
        {
            set => PutUInt(value, 2);
        }

        public byte MaxItemCount
        {
            set => PutByte(value, 6);
        }

        public uint BuyLimit
        {
            set => PutUInt(value, 7);
        }

        public string Comment
        {
            set
            {
                var comment = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[14 + comment.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)comment.Length, 11);
                PutBytes(comment, 12);
            }
        }
    }
}