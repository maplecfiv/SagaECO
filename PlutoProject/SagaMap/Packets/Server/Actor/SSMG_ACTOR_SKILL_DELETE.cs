using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_SKILL_DELETE : Packet
    {
        public SSMG_ACTOR_SKILL_DELETE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x13A6;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}