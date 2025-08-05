using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_SKILL_JOINT_LIST : Packet
    {
        public SSMG_SKILL_JOINT_LIST()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x022F;
        }

        public List<SagaDB.Skill.Skill> Skills
        {
            set
            {
                data = new byte[3 + 2 * value.Count];
                ID = 0x022F;
                PutByte((byte)value.Count, 2);
                for (var i = 0; i < value.Count; i++) PutUShort((ushort)value[i].ID, (ushort)(3 + 2 * i));
            }
        }
    }
}