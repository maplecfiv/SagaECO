using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_GOLD : Packet
    {
        public SSMG_TRADE_GOLD()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0A1F;
        }

        public long Gold
        {
            set => PutLong(value, 2);
        }
    }
}