using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Trade
{
    public class CSMG_TRADE_PERFORM : Packet
    {
        public CSMG_TRADE_PERFORM()
        {
            offset = 2;
        }

        public byte State => GetByte(2);

        public override Packet New()
        {
            return new CSMG_TRADE_PERFORM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTradePerform(this);
        }
    }
}