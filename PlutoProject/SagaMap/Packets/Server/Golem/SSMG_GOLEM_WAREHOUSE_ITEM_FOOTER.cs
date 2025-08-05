using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GOLEM_WAREHOUSE_ITEM_FOOTER : Packet
    {
        public SSMG_GOLEM_WAREHOUSE_ITEM_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x17F7;
        }
    }
}