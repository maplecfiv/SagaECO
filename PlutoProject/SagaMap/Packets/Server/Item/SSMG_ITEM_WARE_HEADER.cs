using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_WARE_HEADER : Packet
    {
        public SSMG_ITEM_WARE_HEADER()
        {
            data = new byte[30];
            offset = 2;
            ID = 0x09F6;
            PutInt(0x0F, 18);
        }

        public WarehousePlace Place
        {
            set => PutInt((int)value, 2);
        }

        public int CountCurrent
        {
            set => PutInt(value, 6);
        }

        public int CountAll
        {
            set => PutInt(value, 10);
        }

        public int CountMax
        {
            set => PutInt(value, 14);
        }

        public int Unknown
        {
            set => PutInt(value, 18);
        }

        public ulong Gold
        {
            set => PutULong(value, 22);
        }
    }
}