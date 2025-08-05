using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_ENHANCE_CLOSE : Packet
    {
        public CSMG_ITEM_ENHANCE_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_ENHANCE_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemEnhanceClose(this);
        }
    }
}