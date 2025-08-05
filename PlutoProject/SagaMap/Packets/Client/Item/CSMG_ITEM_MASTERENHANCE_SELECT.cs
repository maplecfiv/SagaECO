using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_MASTERENHANCE_SELECT : Packet
    {
        public CSMG_ITEM_MASTERENHANCE_SELECT()
        {
            offset = 2;
            data = new byte[6];
        }

        public uint InventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        public override Packet New()
        {
            return new CSMG_ITEM_MASTERENHANCE_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemMasterEnhanceSelect(this);
        }
    }
}