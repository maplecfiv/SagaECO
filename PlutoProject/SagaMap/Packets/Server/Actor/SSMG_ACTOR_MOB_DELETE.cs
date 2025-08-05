using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_MOB_DELETE : Packet
    {
        public SSMG_ACTOR_MOB_DELETE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1225;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}