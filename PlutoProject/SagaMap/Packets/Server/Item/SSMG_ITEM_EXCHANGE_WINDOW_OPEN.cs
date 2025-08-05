using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_EXCHANGE_WINDOW_OPEN : Packet
    {
        public SSMG_ITEM_EXCHANGE_WINDOW_OPEN()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x060E;
        }

        public int SetWindowType
        {
            set => PutInt(value, 2);
        }
    }
}