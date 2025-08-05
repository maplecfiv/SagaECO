using SagaLib;

namespace SagaMap.Packets.Server.Trade
{
    public class SSMG_TRADE_ITEM_FOOT : Packet
    {
        public SSMG_TRADE_ITEM_FOOT()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x0A21;
        }
    }
}