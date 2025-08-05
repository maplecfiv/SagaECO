using System.Collections.Generic;
using SagaDB.ECOShop;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_GSHOP_CATEGORY : Packet
    {
        public SSMG_GSHOP_CATEGORY()
        {
            data = new byte[27];
            offset = 2;
            ID = 0x062C;
        }

        public uint CurrentPoint
        {
            set => PutUInt(value, 11);
        }

        public byte type
        {
            set => PutByte(value, 6);
        }

        public Dictionary<uint, GShopCategory> Categories
        {
            set
            {
                var count = 0;
                var l = 0;
                var names = new List<byte[]>();
                PutByte((byte)value.Count, 23);
                var buf = new byte[24 + 4 * value.Count + 1 + 2];
                data.CopyTo(buf, 0);
                data = buf;
                foreach (var item in value.Values)
                {
                    PutInt(count, (ushort)(24 + 4 * count));
                    count++;
                    var d = Global.Unicode.GetBytes(item.Name);
                    names.Add(d);
                    l += 1 + d.Length;
                }

                var s = (ushort)(25 + 4 * count - 1);
                PutByte((byte)value.Count, s);
                s++;
                buf = new byte[s + l];
                data.CopyTo(buf, 0);
                data = buf;
                foreach (var item in names)
                {
                    PutByte((byte)item.Length, s);
                    s++;
                    PutBytes(item);
                    s += (ushort)item.Length;
                }
            }
        }
    }
}