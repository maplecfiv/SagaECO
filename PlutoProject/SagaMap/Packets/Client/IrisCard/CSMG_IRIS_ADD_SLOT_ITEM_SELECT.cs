using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_ADD_SLOT_ITEM_SELECT : Packet
    {
        public CSMG_IRIS_ADD_SLOT_ITEM_SELECT()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_IRIS_ADD_SLOT_ITEM_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisAddSlotItemSelect(this);
        }
    }
}