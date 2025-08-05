using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_WAREHOUSE : Packet
    {
        public CSMG_GOLEM_WAREHOUSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_GOLEM_WAREHOUSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemWarehouse(this);
        }
    }
}