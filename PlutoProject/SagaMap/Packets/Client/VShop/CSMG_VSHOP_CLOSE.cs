using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.VShop
{
    public class CSMG_VSHOP_CLOSE : Packet
    {
        public CSMG_VSHOP_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_VSHOP_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnVShopClose(this);
        }
    }
}