using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_VSHOP_CATEGORY_REQUEST : Packet
    {
        public CSMG_VSHOP_CATEGORY_REQUEST()
        {
            offset = 2;
        }

        public uint Page => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_VSHOP_CATEGORY_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnVShopCategoryRequest(this);
        }
    }
}