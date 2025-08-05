using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_SHOP_SELL : Packet
    {
        public CSMG_GOLEM_SHOP_SELL()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_SELL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopSell(this);
        }
    }
}