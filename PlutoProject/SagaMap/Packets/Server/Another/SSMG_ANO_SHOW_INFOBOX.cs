using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Another
{
    public class SSMG_ANO_SHOW_INFOBOX : Packet
    {
        public SSMG_ANO_SHOW_INFOBOX()
        {
            data = new byte[436];
            offset = 2;
            ID = 0x23A5;
            PutByte(0, 2); //unknown
            PutByte(1, 4); //Page
            PutByte(1, 5); //MaxPage
            PutByte(7, 14);
            PutByte(7, 29);
            PutByte(7, 86);
            PutByte(7, 94);
            PutByte(7, 151);
            PutByte(7, 208);
            PutByte(7, 265);
            PutByte(7, 322);
            PutByte(7, 379);
        }

        public byte index
        {
            set => PutByte(value, 3);
        }

        public ulong cexp
        {
            set => PutULong(value, 6);
        }

        public ushort usingPaperID
        {
            set => PutUShort(value, 15);
        }

        public List<ushort> papersID
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutUShort(value[i], 17 + i * 2);
            }
        }

        public ulong usingPaperValue
        {
            set => PutULong(value, 30);
        }

        public List<ulong> paperValues
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutULong(value[i], (ushort)(38 + i * 8));
            }
        }

        public byte usingLv
        {
            set => PutByte(value, 87);
        }

        public List<byte> papersLv
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutByte(value[i], 88 + i);
            }
        }

        public ulong usingSkillEXP_1
        {
            set => PutULong(value, 95);
        }

        public List<ulong> paperSkillsEXP_1
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutULong(value[i], (ushort)(103 + i * 8));
            }
        }

        public ulong usingSkillEXP_2
        {
            set => PutULong(value, 152);
        }

        public List<ulong> paperSkillsEXP_2
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutULong(value[i], (ushort)(160 + i * 8));
            }
        }

        public ulong usingSkillEXP_3
        {
            set => PutULong(value, 209);
        }

        public List<ulong> paperSkillsEXP_3
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutULong(value[i], (ushort)(217 + i * 8));
            }
        }

        public ulong usingSkillEXP_4
        {
            set => PutULong(value, 266);
        }

        public List<ulong> paperSkillsEXP_4
        {
            set
            {
                for (var i = 0; i < value.Count; i++) PutULong(value[i], (ushort)(274 + i * 8));
            }
        }
    }
}