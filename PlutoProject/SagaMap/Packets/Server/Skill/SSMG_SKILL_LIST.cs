using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_SKILL_LIST : Packet
    {
        public SSMG_SKILL_LIST()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x0226;
        }

        /// <summary>
        ///     Set skills
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="job">0 for basic, 1 for expert, 2 for technical</param>
        public void Skills(List<SagaDB.Skill.Skill> list, byte job, PC_JOB job2, bool ifDominion, ActorPC pc)
        {
            if (Configuration.Instance.Version >= Version.Saga11)
                data = new byte[8 + list.Count * 2 + list.Count * 3];
            else
                data = new byte[7 + list.Count * 2 + list.Count * 3];
            ID = 0x0226;
            for (var i = 0; i < list.Count; i++)
            {
                var skill = list[i];
                byte jobLv = 1;
                if (SkillFactory.Instance.CheckSkillList(pc, SkillFactory.SkillPaga.none).ContainsKey(skill.ID))
                    jobLv = SkillFactory.Instance.CheckSkillList(pc, SkillFactory.SkillPaga.none)[skill.ID];
                PutByte((byte)list.Count, 2);
                PutUShort((ushort)skill.ID, (ushort)(3 + i * 2));
                PutByte((byte)list.Count, (ushort)(3 + list.Count * 2));
                PutByte(skill.Level, (ushort)(4 + list.Count * 2 + i));
                PutByte((byte)list.Count, (ushort)(4 + list.Count * 3));
                PutByte((byte)list.Count, (ushort)(5 + list.Count * 4));
                if (SkillFactory.Instance.CheckSkillList(pc, SkillFactory.SkillPaga.none).ContainsKey(skill.ID))
                {
                    jobLv = SkillFactory.Instance.CheckSkillList(pc, SkillFactory.SkillPaga.none)[skill.ID];
                    var skill2 = SkillFactory.Instance.GetSkill(skill.ID, skill.Level);
                    if (skill2.JobLv == 0)
                        PutByte(jobLv, (ushort)(6 + list.Count * 4 + i));
                    else
                        PutByte(skill2.JobLv, (ushort)(6 + list.Count * 4 + i));
                }
                else
                {
                    PutByte(1, (ushort)(6 + list.Count * 4 + i));
                }
            }

            PutByte(job, (ushort)(6 + list.Count * 5));
            /*
            this.data = new byte[43];
            byte[] s = { 0x02, 0x26, 0x07, 0x03, 0xD6, 0x04, 0x4C, 0x09, 0xB4, 0x09, 0xC2, 0x09, 0xC7, 0x0D, 0x16, 0x0D, 0x22, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x03, 0x0D, 0x14, 0x17, 0x19, 0x06, 0x0A, 0x03, 0x00 };
            this.data = s;//*/
        }
    }
}