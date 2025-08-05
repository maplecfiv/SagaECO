using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_SHOP_BUY_HEADER : Packet
    {
        public SSMG_GOLEM_SHOP_BUY_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1824;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}