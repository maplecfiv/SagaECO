using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Trade
{
    public class CSMG_TRADE_REQUEST_ANSWER : Packet
    {
        public CSMG_TRADE_REQUEST_ANSWER()
        {
            offset = 2;
        }

        public byte Answer => GetByte(2);

        public override Packet New()
        {
            return new CSMG_TRADE_REQUEST_ANSWER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTradeRequestAnswer(this);
        }
    }
}