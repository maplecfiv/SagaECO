using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_WAREHOUSE : Packet
    {
        public SSMG_GOLEM_WAREHOUSE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x17F3;
        }

        public uint ActorID
        {
            set => PutUInt(value, 3);
        }

        public string Title
        {
            set
            {
                var title = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[8 + title.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)title.Length, 7);
                PutBytes(title, 8);
            }
        }
    }
}