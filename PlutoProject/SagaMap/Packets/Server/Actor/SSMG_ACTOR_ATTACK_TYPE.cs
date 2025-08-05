using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_ATTACK_TYPE : Packet
    {
        public SSMG_ACTOR_ATTACK_TYPE()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x0FBF;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ATTACK_TYPE AttackType
        {
            set => PutByte((byte)value, 6);
        }
    }
}