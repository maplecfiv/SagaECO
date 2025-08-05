using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_SHOP_OPEN_OK : Packet
    {
        public SSMG_GOLEM_SHOP_OPEN_OK()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x17FE;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}