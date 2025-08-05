using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_ACTOR_DISAPPEAR : Packet
    {
        public SSMG_FG_ACTOR_DISAPPEAR()
        {
            data = new byte[6];
            offset = 2;

            ID = 0x1C0D;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}