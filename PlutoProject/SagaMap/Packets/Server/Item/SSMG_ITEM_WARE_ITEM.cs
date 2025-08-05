using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_WARE_ITEM : HasItemDetail
    {
        public SSMG_ITEM_WARE_ITEM()
        {
            if (Configuration.Instance.Version < Version.Saga9_Iris)
                data = new byte[170];
            else
                data = new byte[217];
            offset = 2;
            ID = 0x09F8;
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

        public WarehousePlace Place
        {
            set => PutByte((byte)value, 15);
        }
    }
}