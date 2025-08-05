using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_ITEM_INFO : HasItemDetail
    {
        public SSMG_TRADE_ITEM_INFO()
        {
            if (Configuration.Instance.Version < Version.Saga9_Iris)
                data = new byte[170];
            else
                data = new byte[217];
            offset = 2;
            ID = 0x0A1E;
        }

        public Item Item
        {
            set
            {
                if (Configuration.Instance.Version < Version.Saga9_Iris)
                    PutByte(0xa6, 2);
                else
                    PutByte(0xd6, 2);
                offset = 7;
                ItemDetail = value;
                //this.PutByte((byte)(this.data.Length - 3), 2);
            }
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 3);
        }

        public uint ItemID
        {
            set => PutUInt(value, 7);
        }

        public ContainerType Container
        {
            set => PutByte((byte)value, 15);
        }
    }
}