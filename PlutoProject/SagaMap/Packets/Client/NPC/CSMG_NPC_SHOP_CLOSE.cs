using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_SHOP_CLOSE : Packet
    {
        public CSMG_NPC_SHOP_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_NPC_SHOP_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCShopClose(this);
        }
    }
}