using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ENHANCE_LIST : Packet
    {
        private int count = 0;

        public SSMG_ITEM_ENHANCE_LIST()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x13C4;
        }

        public List<SagaDB.Item.Item> Items
        {
            set
            {
                data = new byte[10 + 4 * value.Count];
                offset = 2;
                ID = 0x13C4;

                PutByte((byte)value.Count);

                foreach (var item in value) PutUInt(item.Slot);
                PutUInt(0u);
                PutByte(01);
                PutShort(100);
                PutByte(0);
            }
        }
    }
}