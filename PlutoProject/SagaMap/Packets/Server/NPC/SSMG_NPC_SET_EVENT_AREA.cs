using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SET_EVENT_AREA : Packet
    {
        public SSMG_NPC_SET_EVENT_AREA()
        {
            data = new byte[27];
            offset = 2;
            ID = 0x0C80;
        }

        public uint EventID
        {
            set => PutUInt(value, 2);
        }

        public uint StartX
        {
            set => PutUInt(value, 6);
        }

        public uint StartY
        {
            set => PutUInt(value, 10);
        }

        public uint EndX
        {
            set => PutUInt(value, 14);
        }

        public uint EndY
        {
            set => PutUInt(value, 18);
        }

        public uint EffectID
        {
            set => PutUInt(value, 22);
        }
    }
}