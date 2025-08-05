using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.DualJob;
using SagaDB.Skill;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.Skill;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public bool changeDualJob;

        public void OnDualChangeRequest(CSMG_DUALJOB_CHANGE_CONFIRM p)
        {
            Character.DualJobID = p.DualJobID;

            if (!Character.PlayerDualJobList.ContainsKey(Character.DualJobID))
            {
                var dualjobinfo = new PlayerDualJobInfo();
                dualjobinfo.DualJobExp = 0;
                dualjobinfo.DualJobID = Character.DualJobID;
                dualjobinfo.DualJobLevel = 1;
                Character.PlayerDualJobList.Add(Character.DualJobID, dualjobinfo);
            }

            if (Character.PlayerDualJobList[Character.DualJobID].DualJobLevel <= 0)
            {
                Character.PlayerDualJobList[Character.DualJobID].DualJobLevel = 1;
                Character.PlayerDualJobList[Character.DualJobID].DualJobExp = 0;
            }

            var skills = new List<SagaDB.Skill.Skill>();
            var ids = p.DualJobSkillList;
            foreach (var item in ids)
                if (item != 0)
                {
                    var sks = DualJobSkillFactory.Instance.items[Character.DualJobID].Where(x =>
                        x.DualJobID == Character.DualJobID && x.SkillID == item &&
                        x.LearnSkillLevel.Where(y => y <= Character.DualJobLevel).Count() > 0).FirstOrDefault();
                    if (sks != null)
                    {
                        var sk = DualJobSkillFactory.Instance.items[Character.DualJobID]
                            .FirstOrDefault(x => x.SkillID == item);
                        var lv = sk.LearnSkillLevel.Count(x => x > 0 && x <= Character.DualJobLevel);
                        skills.Add(SkillFactory.Instance.GetSkill(item, (byte)lv));
                    }
                }

            Character.DualJobSkill = skills;
            Character.DualJobLevel = Character.PlayerDualJobList[Character.DualJobID].DualJobLevel;

            MapServer.charDB.SaveDualJobInfo(Character, true);

            SendPlayerInfo();
            SendPlayerDualJobSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);

            changeDualJob = false;

            var pi = new SSMG_DUALJOB_SET_DUALJOB_INFO();
            pi.Result = true;
            pi.RetType = 0x00;
            netIO.SendPacket(pi);
        }


        public void SendPlayerDualJobInfo()
        {
            var p2 = new SSMG_DUALJOB_INFO_SEND();
            p2.JobList = new byte[25]
            {
                0xC, 0x0, 0x1, 0x0, 0x2, 0x0, 0x3, 0x0, 0x4, 0x0, 0x5, 0x0, 0x6, 0x0, 0x7, 0x0, 0x8, 0x0, 0x9, 0x0, 0xa,
                0x0, 0xb, 0x0, 0xc
            };
            var levels = new byte[13];
            levels[0] = 0x0C;
            for (byte i = 1; i <= 0x0C; i++)
                if (Character.PlayerDualJobList.ContainsKey(i))
                    levels[i] = Character.PlayerDualJobList[i].DualJobLevel;
                else
                    levels[i] = 0;
            p2.JobLevel = levels;
            netIO.SendPacket(p2);
        }

        public void SendPlayerDualJobSkillList()
        {
            var p1 = new SSMG_DUALJOB_SKILL_SEND();
            p1.Skills = Character.DualJobSkill;
            p1.SkillLevels = Character.DualJobSkill;

            netIO.SendPacket(p1);
        }

        public void OnDualJobWindowClose()
        {
            changeDualJob = false;
        }

        /// <summary>
        ///     打开副职转职窗口
        /// </summary>
        /// <param name="pc">角色对象</param>
        /// <param name="ChangeDualJob">是否允许更改副职系统(是否为习得副职系统)</param>
        public void OpenDualJobChangeUI(ActorPC pc, bool ChangeDualJob)
        {
            var p = new SSMG_DUALJOB_WINDOW_OPEN();
            if (ChangeDualJob)
                p.CanChange = 0x01;
            else
                p.CanChange = 0x00;

            p.SetDualJobList(0x0C,
                new byte[]
                {
                    0x0, 0x1, 0x0, 0x2, 0x0, 0x3, 0x0, 0x4, 0x0, 0x5, 0x0, 0x6, 0x0, 0x7, 0x0, 0x8, 0x0, 0x9, 0x0, 0xa,
                    0x0, 0xb, 0x0, 0xc
                });

            var dualjoblevel = new byte[12];
            for (var i = 0; i < dualjoblevel.Length; i++)
                if (pc.PlayerDualJobList.ContainsKey(byte.Parse((i + 1).ToString())))
                    dualjoblevel[i] = pc.PlayerDualJobList[byte.Parse((i + 1).ToString())].DualJobLevel;
                else
                    dualjoblevel[i] = 0;
            p.DualJobLevel = dualjoblevel;
            p.CurrentDualJobSerial = pc.DualJobID;
            if (ChangeDualJob)
                p.CurrentSkillList = pc.DualJobSkill;
            else
                p.CurrentSkillList = new List<SagaDB.Skill.Skill>();

            netIO.SendPacket(p);
        }
    }
}