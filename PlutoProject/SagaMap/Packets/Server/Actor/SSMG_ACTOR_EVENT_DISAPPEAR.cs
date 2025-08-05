using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_EVENT_DISAPPEAR : Packet
    {
        public SSMG_ACTOR_EVENT_DISAPPEAR()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0BB9;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}