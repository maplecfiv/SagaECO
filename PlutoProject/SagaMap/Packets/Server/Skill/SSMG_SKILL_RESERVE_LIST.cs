using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_RESERVE_LIST : Packet
    {
        public SSMG_SKILL_RESERVE_LIST()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x022E;
        }

        public List<SagaDB.Skill.Skill> Skills
        {
            set
            {
                data = new byte[4 + 3 * value.Count];
                ID = 0x022E;
                PutByte((byte)value.Count, 2);
                PutByte((byte)value.Count, (ushort)(3 + 2 * value.Count));
                for (var i = 0; i < value.Count; i++)
                {
                    PutUShort((ushort)value[i].ID, (ushort)(3 + 2 * i));
                    PutByte(value[i].Level, (ushort)(4 + 2 * value.Count + i));
                }
            }
        }
    }
}