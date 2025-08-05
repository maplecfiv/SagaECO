using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_SPEED : Packet
    {
        public SSMG_ACTOR_SPEED()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x1239;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ushort Speed
        {
            set => PutUShort(value, 6);
        }
    }
}