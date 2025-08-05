using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_SKILL_RANGEA_RESULT : Packet
    {
        public SSMG_SKILL_RANGEA_RESULT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0FAB;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint Speed
        {
            set => PutUInt(value, 6);
        }
    }
}