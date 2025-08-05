using SagaLib;

namespace SagaMap.Packets.Server.Theater
{
    public class SSMG_THEATER_SCHEDULE : Packet
    {
        public SSMG_THEATER_SCHEDULE()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x1A9B;
        }

        public int Index
        {
            set => PutInt(value, 2);
        }

        public uint TicketItem
        {
            set => PutUInt(value, 6);
        }

        public string Time
        {
            set
            {
                var time = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[12 + time.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)time.Length, 10);
                PutBytes(time, 11);
            }
        }

        public string Title
        {
            set
            {
                var offset = GetByte(10);
                var title = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[12 + offset + title.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)title.Length, (ushort)(11 + offset));
                PutBytes(title, (ushort)(12 + offset));
            }
        }
    }
}