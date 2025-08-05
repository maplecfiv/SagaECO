using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_GOLEM_WAREHOUSE_GET : Packet
    {
        public CSMG_GOLEM_WAREHOUSE_GET()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public ushort Count => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_GOLEM_WAREHOUSE_GET();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnGolemWarehouseGet(this);
        }
    }
}