using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_EXCHANGE_WINDOW_RESET : Packet
    {
        public SSMG_ITEM_EXCHANGE_WINDOW_RESET()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0610;
        }
    }
}