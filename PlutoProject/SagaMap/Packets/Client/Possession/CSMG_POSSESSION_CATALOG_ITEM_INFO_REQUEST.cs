using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST : Packet
    {
        public CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public uint ItemID => GetUInt(6);

        public override Packet New()
        {
            return new CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPossessionCatalogItemInfoRequest(this);
        }
    }
}