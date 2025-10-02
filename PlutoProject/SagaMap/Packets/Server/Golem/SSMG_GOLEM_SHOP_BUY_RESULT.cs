using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_BUY_RESULT : Packet
    {
        public SSMG_GOLEM_SHOP_BUY_RESULT()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x182E;
        }

        public Dictionary<uint, GolemShopItem> BoughtItems
        {
            set
            {
                data = new byte[12 + value.Count * 6];
                offset = 2;
                ID = 0x182E;
                uint gold = 0;
                PutByte((byte)value.Count, 10);
                PutByte((byte)value.Count, (ushort)(11 + value.Count * 4));
                var j = 0;
                foreach (var i in value.Keys)
                {
                    PutUInt(i, (ushort)(11 + j * 4));
                    PutUShort(value[i].Count, (ushort)(12 + value.Count * 4 + j * 2));
                    gold += value[i].Count * value[i].Price;
                    j++;
                }

                PutUInt(gold, 6);
            }
        }
    }
}