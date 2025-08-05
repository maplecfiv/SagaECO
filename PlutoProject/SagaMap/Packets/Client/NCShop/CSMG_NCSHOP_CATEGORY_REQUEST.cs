using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_NCSHOP_CATEGORY_REQUEST : Packet
    {
        public CSMG_NCSHOP_CATEGORY_REQUEST()
        {
            offset = 2;
        }

        public uint Page => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_NCSHOP_CATEGORY_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNCShopCategoryRequest(this);
        }
    }
}