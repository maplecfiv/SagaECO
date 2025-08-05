using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_FUSION_CANCEL : Packet
    {
        public CSMG_ITEM_FUSION_CANCEL()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_FUSION_CANCEL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemFusionCancel(this);
        }
    }
}