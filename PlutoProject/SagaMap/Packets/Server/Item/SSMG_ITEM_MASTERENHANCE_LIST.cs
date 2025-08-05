using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_MASTERENHANCE_LIST : Packet
    {
        public SSMG_ITEM_MASTERENHANCE_LIST()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1F54;
        }

        public List<SagaDB.Item.Item> Items
        {
            set
            {
                var buf = new byte[4 + 4 * value.Count];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)value.Count, 2);
                var j = 0;
                foreach (var i in value)
                {
                    PutUInt(i.Slot, (ushort)(3 + 4 * j));
                    j++;
                }

                PutByte(0x0, 4 + 4 * value.Count);
            }
        }
    }
}