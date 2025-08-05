using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_SHOP_BUY_CLOSE : Packet
    {
        public CSMG_GOLEM_SHOP_BUY_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_SHOP_BUY_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemShopBuyClose(this);
        }
    }
}