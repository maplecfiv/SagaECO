using System.Collections.Generic;
using SagaDB.MasterEnchance;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_MASTERENHANCE_DETAIL : Packet
    {
        public SSMG_ITEM_MASTERENHANCE_DETAIL()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x1F56;
        }

        public List<MasterEnhanceMaterial> Items
        {
            set
            {
                var buf = new byte[9 + 7 * value.Count];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)value.Count, 2);
                PutByte((byte)value.Count, (ushort)(3 + 4 * value.Count));
                PutByte((byte)value.Count, (ushort)(4 + 5 * value.Count));
                PutByte((byte)value.Count, (ushort)(5 + 6 * value.Count));
                PutByte(0x01, (ushort)(6 + 7 * value.Count));
                PutShort(0x64, (ushort)(6 + 8 * value.Count));

                var j = 0;
                foreach (var i in value)
                {
                    PutUInt(i.ID, (ushort)(3 + 4 * j));
                    PutByte((byte)i.Ability, (ushort)(4 + 4 * value.Count + j));
                    PutByte((byte)i.MinValue, (ushort)(5 + 5 * value.Count + j));
                    PutByte((byte)i.MaxValue, (ushort)(6 + 6 * value.Count + j));
                    j++;
                }
            }
        }
    }
}