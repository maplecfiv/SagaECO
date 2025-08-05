using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_POSSESSION_CATALOG_REQUEST : Packet
    {
        public CSMG_POSSESSION_CATALOG_REQUEST()
        {
            offset = 2;
        }

        public PossessionPosition Position => (PossessionPosition)GetByte(2);

        public ushort Page => GetUShort(3);

        public override Packet New()
        {
            return new CSMG_POSSESSION_CATALOG_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPossessionCatalogRequest(this);
        }
    }
}