using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_CANCEL_EVENT_AREA : Packet
    {
        public SSMG_NPC_CANCEL_EVENT_AREA()
        {
            data = new byte[18];
            offset = 2;
            ID = 0x0C80;
        }

        public uint StartX
        {
            set => PutUInt(value, 2);
        }

        public uint StartY
        {
            set => PutUInt(value, 6);
        }

        public uint EndX
        {
            set => PutUInt(value, 10);
        }

        public uint EndY
        {
            set => PutUInt(value, 14);
        }
    }
}