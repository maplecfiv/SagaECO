using System.Collections.Generic;
using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_EQUIP_REPAIR_LIST : Packet
    {
        public SSMG_ITEM_EQUIP_REPAIR_LIST()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x13BF;
        }


        public List<Item> Items
        {
            set
            {
                var buf = new byte[data.Length + value.Count * 4 + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, 2);
                for (var i = 0; i < value.Count; i++) PutUInt(value[i].Slot, (ushort)(3 + 4 * i));
            }
        }
    }
}