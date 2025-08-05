using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Golem
{
    public class CSMG_GOLEM_WAREHOUSE_SET : Packet
    {
        public CSMG_GOLEM_WAREHOUSE_SET()
        {
            offset = 2;
        }

        public string Title => Global.Unicode.GetString(GetBytes(GetByte(2), 3)).Replace("\0", "");

        public override Packet New()
        {
            return new CSMG_GOLEM_WAREHOUSE_SET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemWarehouseSet(this);
        }
    }
}