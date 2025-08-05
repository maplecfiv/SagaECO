using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_CHANGEPAPER : Packet
    {
        public SSMG_ACTOR_CHANGEPAPER()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x23AE;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public ushort paperID
        {
            set => PutUShort(value, 6);
        }
    }
}