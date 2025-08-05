using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_COUNT_UPDATE : Packet
    {
        public SSMG_ITEM_COUNT_UPDATE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x09CF;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public ushort Stack
        {
            set => PutUShort(value, 6);
        }
    }
}