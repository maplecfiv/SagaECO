using System.Collections.Generic;
using SagaDB.DEMIC;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_CHIP_SHOP_CATEGORY : Packet
    {
        public SSMG_DEM_CHIP_SHOP_CATEGORY()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x0636;
        }

        public List<ChipShopCategory> Categories
        {
            set
            {
                var names = new byte[value.Count][];
                var j = 0;
                var count = 0;
                foreach (var i in value)
                {
                    names[j] = Global.Unicode.GetBytes(i.Name);
                    count += names[j].Length + 1;
                    j++;
                }

                var buf = new byte[4 + value.Count * 4 + count];
                data.CopyTo(buf, 0);
                data = buf;

                offset = 2;
                PutByte((byte)value.Count);
                foreach (var i in value) PutUInt(i.ID);
                PutByte((byte)value.Count);
                foreach (var i in names)
                {
                    PutByte((byte)i.Length);
                    PutBytes(i);
                }
            }
        }
    }
}