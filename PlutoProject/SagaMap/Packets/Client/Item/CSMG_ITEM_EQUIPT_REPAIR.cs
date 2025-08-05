using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_EQUIPT_REPAIR : Packet
    {
        public CSMG_ITEM_EQUIPT_REPAIR()
        {
            offset = 2;
        }

        public uint InventoryID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_ITEM_EQUIPT_REPAIR();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemEquiptRepair(this);
        }
    }
}