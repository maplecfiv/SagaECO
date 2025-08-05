using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_WARE_CLOSE : Packet
    {
        public CSMG_ITEM_WARE_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_WARE_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemWareClose(this);
        }
    }
}