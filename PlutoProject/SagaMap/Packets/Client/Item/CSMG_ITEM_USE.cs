using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_USE : Packet
    {
        public CSMG_ITEM_USE()
        {
            offset = 2;
        }

        public uint InventorySlot => GetUInt(2);

        public uint ActorID => GetUInt(6);

        public byte X => GetByte(10);

        public byte Y => GetByte(11);

        public override Packet New()
        {
            return new CSMG_ITEM_USE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemUse(this);
        }
    }
}