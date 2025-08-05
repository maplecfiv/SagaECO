using SagaLib;

namespace SagaMap.Packets.Server.Theater
{
    public class SSMG_THEATER_SCHEDULE_FOOTER : Packet
    {
        public SSMG_THEATER_SCHEDULE_FOOTER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1A9C;
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }
    }
}