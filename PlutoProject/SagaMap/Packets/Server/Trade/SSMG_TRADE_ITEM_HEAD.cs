using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_ITEM_HEAD : Packet
    {
        public SSMG_TRADE_ITEM_HEAD()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x0A20;
        }
    }
}