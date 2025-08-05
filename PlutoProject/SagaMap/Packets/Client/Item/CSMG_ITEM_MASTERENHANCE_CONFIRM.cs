using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_MASTERENHANCE_CONFIRM : Packet
    {
        public CSMG_ITEM_MASTERENHANCE_CONFIRM()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public uint ItemID => GetUInt(6);

        public override Packet New()
        {
            return new CSMG_ITEM_MASTERENHANCE_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemMasterEnhanceConfirm(this);
        }
    }
}