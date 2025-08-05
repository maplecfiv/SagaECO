using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_SHOP_BUY : Packet
    {
        public CSMG_GOLEM_SHOP_BUY()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_BUY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopBuy(this);
        }
    }
}