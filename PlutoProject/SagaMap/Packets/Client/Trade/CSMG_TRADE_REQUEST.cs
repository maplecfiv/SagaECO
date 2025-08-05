using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Trade
{
    public class CSMG_TRADE_REQUEST : Packet
    {
        public CSMG_TRADE_REQUEST()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_TRADE_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnTradeRequest(this);
        }
    }
}