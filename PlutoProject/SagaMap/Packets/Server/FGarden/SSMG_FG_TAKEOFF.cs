using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_TAKEOFF : Packet
    {
        public SSMG_FG_TAKEOFF()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x18E3;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint MapID
        {
            set => PutUInt(value, 6);
        }
    }
}