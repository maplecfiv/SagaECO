using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_EXCHANGE_CLOSE : Packet
    {
        public CSMG_ITEM_EXCHANGE_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ITEM_EXCHANGE_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemExchangeWindowClose(this);
        }
    }
}