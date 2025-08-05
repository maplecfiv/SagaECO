using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DUALJOB_SKILL_SEND : Packet
    {
        public SSMG_DUALJOB_SKILL_SEND()
        {
            data = new byte[36];
            offset = 2;
            ID = 0x22D2;
        }

        public List<SagaDB.Skill.Skill> Skills
        {
            set
            {
                PutByte(byte.Parse(value.Count.ToString()), offset);

                foreach (var item in value) PutUShort(ushort.Parse(item.ID.ToString()));
            }
        }

        public List<SagaDB.Skill.Skill> SkillLevels
        {
            set
            {
                PutByte(byte.Parse(value.Count.ToString()), offset);

                foreach (var item in value) PutByte(item.Level);
            }
        }
    }
}