using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_MODE : Packet
    {
        public SSMG_ACTOR_MODE()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x0FA7;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public int Mode1
        {
            set => PutInt(value, 6);
        }

        public int Mode2
        {
            set => PutInt(value, 10);
        }
    }
}