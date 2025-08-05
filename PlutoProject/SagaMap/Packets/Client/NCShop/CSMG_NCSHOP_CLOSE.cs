using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_NCSHOP_CLOSE : Packet
    {
        public CSMG_NCSHOP_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_NCSHOP_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNCShopClose(this);
        }
    }
}