using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public enum EnhanceType
    {
        HP,
        MP,
        SP,
        Atk,
        MAtk,
        Def,
        MDef,
        Cri = 13,
        AvoidCri
    }

    public class EnhanceDetail
    {
        public byte exp1;
        public byte exp2;
        public uint material;
        public EnhanceType type;
        public short value;
    }

    public class SSMG_ITEM_ENHANCE_DETAIL : Packet
    {
        public SSMG_ITEM_ENHANCE_DETAIL()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x13C6;
        }

        public List<EnhanceDetail> Items
        {
            set
            {
                var buf = new byte[6 + 12 * value.Count];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)value.Count, 2);
                PutByte((byte)value.Count, (ushort)(3 + 4 * value.Count));
                PutByte((byte)value.Count, (ushort)(4 + 6 * value.Count));
                PutByte((byte)value.Count, (ushort)(5 + 8 * value.Count));
                var j = 0;
                foreach (var i in value)
                {
                    PutUInt(i.material, (ushort)(3 + 4 * j));
                    PutShort((short)i.type, (ushort)(4 + 4 * value.Count + 2 * j));
                    PutShort(i.value, (ushort)(5 + 6 * value.Count + 2 * j));
                    PutByte(0, 6 + 8 * value.Count + j);
                    PutByte(1, 6 + 9 * value.Count + j);
                    PutByte(i.exp1, 6 + 10 * value.Count + j);
                    PutByte(i.exp2, 6 + 11 * value.Count + j);
                    j++;
                }
            }
        }
    }
}