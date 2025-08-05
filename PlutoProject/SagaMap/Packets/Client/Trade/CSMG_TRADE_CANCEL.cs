using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_TRADE_CANCEL : Packet
    {
        public CSMG_TRADE_CANCEL()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_TRADE_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTradeCancel(this);
        }
    }
}