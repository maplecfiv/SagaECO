using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_ANOTHER_MOB_DELETE : Packet
    {
        public SSMG_ACTOR_ANOTHER_MOB_DELETE()
        {
            data = new byte[6];
            offset = 2;
            ID = 2329;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}