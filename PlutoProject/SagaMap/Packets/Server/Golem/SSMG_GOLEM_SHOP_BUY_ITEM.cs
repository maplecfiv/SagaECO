using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_BUY_ITEM : Packet
    {
        public SSMG_GOLEM_SHOP_BUY_ITEM()
        {
            data = new byte[1413];
            offset = 2;
            ID = 0x1826;
            PutByte(100, 2);
            PutByte(100, (ushort)(3 + 4 * 100));
            PutByte(100, (ushort)(4 + 6 * 100));
        }

        public Dictionary<uint, GolemShopItem> Items
        {
            set
            {
                var j = 0;
                foreach (var i in value.Values)
                {
                    PutUInt(i.ItemID, (ushort)(3 + j * 4));
                    PutUShort(i.Count, (ushort)(4 + 100 * 4 + j * 2));
                    PutULong(i.Price, (ushort)(5 + 100 * 6 + j * 8));
                    j++;
                }
            }
        }

        public uint BuyLimit
        {
            set => PutULong(value, 1405);
        }
    }
}