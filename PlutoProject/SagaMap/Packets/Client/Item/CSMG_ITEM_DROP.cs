using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ITEM_DROP : Packet
    {
        public CSMG_ITEM_DROP()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public ushort Count => GetUShort(6);

        public override Packet New()
        {
            return new CSMG_ITEM_DROP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemDrop(this);
        }
    }
}