using System.Collections.Generic;
using SagaDB.ECOShop;
using SagaLib;

namespace SagaMap.Packets.Server.VShop
{
    public class SSMG_VSHOP_CATEGORY : Packet
    {
        public SSMG_VSHOP_CATEGORY()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x0640;
        }

        public uint CurrentPoint
        {
            set => PutUInt(value, 2);
        }

        public Dictionary<uint, ShopCategory> Categories
        {
            set
            {
                var count = 0;
                var j = 0;
                var names = new byte[value.Count][];
                foreach (var i in value.Values)
                {
                    names[j] = Global.Unicode.GetBytes(i.Name);
                    count += names[j].Length + 1;
                    j++;
                }

                var buf = new byte[7 + count + 1 + 4 * names.Length];
                data.CopyTo(buf, 0);
                data = buf;
                offset = 6;
                PutByte((byte)names.Length);
                foreach (var i in names)
                {
                    PutByte((byte)i.Length);
                    PutBytes(i);
                }

                PutByte((byte)names.Length);
                for (var i = 1; i <= names.Length; i++) PutInt(i);
            }
        }
    }
}