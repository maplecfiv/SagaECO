using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_END : Packet
    {
        public SSMG_TRADE_END()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x0A1C;
        }
    }
}