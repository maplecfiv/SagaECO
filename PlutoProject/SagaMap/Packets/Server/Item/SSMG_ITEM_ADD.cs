using SagaDB.Item;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_ADD : HasItemDetail
    {
        public SSMG_ITEM_ADD()
        {
            data = new byte[0xD7];
            offset = 2;
            ID = 0x09D4;
        }

        public Item Item
        {
            set
            {
                offset = 7;
                ItemDetail = value;
                PutByte((byte)(data.Length - 3), 2);
            }
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 3);
        }

        public ContainerType Container
        {
            set => PutByte((byte)value, 15);
        }
    }
}