using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Item
{
    public class CSMG_ITEM_CHANGE : Packet
    {
        public CSMG_ITEM_CHANGE()
        {
            offset = 2;
        }

        public uint ChangeID => GetUInt(2);

        public uint InventorySlot => GetUInt(7);

        /*
        public List<uint> SlotList()
        {
            List<uint> list = new List<uint>();
            byte count = GetByte(6);
            for (int i = 0; i < count; i++)
            {
                list.Add(GetUInt((ushort)(7 + (i * 4))));
            }
            return list;
        }
        */
        public override Packet New()
        {
            return new CSMG_ITEM_CHANGE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnItemChange(this);
        }
    }
}