using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_WRP_RANKING : Packet
    {
        public SSMG_ACTOR_WRP_RANKING()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0236;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint Ranking
        {
            set => PutUInt(value, 6);
        }
    }
}