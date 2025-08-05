using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_SELL_SETUP : Packet
    {
        public SSMG_GOLEM_SHOP_SELL_SETUP()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x17E9;
            MaxItemCount = 100;
        }

        public uint Unknown
        {
            set => PutUInt(value, 2);
        }

        public byte MaxItemCount
        {
            set => PutByte(value, 6);
        }

        public string Comment
        {
            set
            {
                var comment = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[10 + comment.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)comment.Length, 7);
                PutBytes(comment, 8);
            }
        }
    }
}