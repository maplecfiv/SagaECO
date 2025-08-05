using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_WARE_FOOTER : Packet
    {
        public SSMG_ITEM_WARE_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x09F9;
        }
    }
}