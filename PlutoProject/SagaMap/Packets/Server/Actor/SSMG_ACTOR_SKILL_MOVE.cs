using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_SKILL_MOVE : Packet
    {
        public SSMG_ACTOR_SKILL_MOVE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x13AB;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public short X
        {
            set => PutShort(value, 6);
        }

        public short Y
        {
            set => PutShort(value, 8);
        }
    }
}