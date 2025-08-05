using SagaDB.Item;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_EQUIP_INFO : HasItemDetail
    {
        public SSMG_PLAYER_EQUIP_INFO()
        {
            data = new byte[215];
            offset = 2;
            ID = 0x0265;
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