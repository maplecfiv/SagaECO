using SagaDB.Item;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_INFO : HasItemDetail
    {
        public SSMG_ITEM_INFO()
        {
            data = new byte[216];
            offset = 2;
            ID = 0x0203;
        }

        public Item Item
        {
            set
            {
                offset = 8;
                ItemDetail = value;
                PutByte((byte)(data.Length - 4), 3);
            }
        }

        public byte Size
        {
            set => PutByte(value, 3);
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 4);
        }

        public uint ItemID
        {
            set => PutUInt(value, 8);
        }

        public ContainerType Container
        {
            set
            {
                if (value >= ContainerType.HEAD2)
                    PutByte((byte)(value - 200), 16);
                else
                    PutByte((byte)value, 16);
            }
        }
    }
}