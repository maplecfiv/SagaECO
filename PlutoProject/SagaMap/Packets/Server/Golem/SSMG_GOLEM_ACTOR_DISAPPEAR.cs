using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_ACTOR_DISAPPEAR : Packet
    {
        public SSMG_GOLEM_ACTOR_DISAPPEAR()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x17D5;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}