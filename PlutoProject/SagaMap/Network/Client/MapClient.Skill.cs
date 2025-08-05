using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaDB.Map;
using SagaDB.Skill;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.Skill;
using SagaMap.Skill.Additions.Global;
using SagaMap.Tasks.PC;
using SagaMap.Tasks.Skill;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        private readonly int hackCount = 0;

        //技能独立cd列表
        private readonly Dictionary<uint, DateTime> SingleCDLst = new Dictionary<uint, DateTime>();
        private DateTime assassinateStamp = DateTime.Now;
        private DateTime attackStamp = DateTime.Now;
        private bool AttactFinished;
        private int delay;
        private DateTime hackStamp = DateTime.Now;
#pragma warning disable CS0169 // 从不使用字段“MapClient.lastAttackRandom”
        private short lastAttackRandom;
#pragma warning restore CS0169 // 从不使用字段“MapClient.lastAttackRandom”
        private short lastCastRandom;

        private CSMG_SKILL_ATTACK Lastp;

        private Thread main;
        public List<uint> nextCombo = new List<uint>();
        private DateTime skillDelay = DateTime.Now;

        public DateTime SkillDelay
        {
            set => skillDelay = value;
        }

        public void OnSkillLvUP(CSMG_SKILL_LEVEL_UP p)
        {
            var p1 = new SSMG_SKILL_LEVEL_UP();
            var skillID = p.SkillID;
            byte type = 0;
            if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(skillID))
                type = 1;
            else if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(skillID))
                type = 2;
            else if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(skillID))
                type = 3;
            else if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(skillID))
                type = 4;
            if (type == 0)
            {
                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_EXIST;
            }
            else
            {
                if (type == 1)
                {
                    if (!Character.Skills.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills[skillID].Level);
                        if (Character.JobLevel1 < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills[skillID].Level == Character.Skills[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint -= 1;
                                Character.Skills[skillID] = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills[skillID].Level + 1));
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 2)
                {
                    if (!Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills2[skillID].Level);
                        if (Character.JobLevel2X < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint2X < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills2[skillID].Level == Character.Skills2[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint2X -= 1;
                                var data = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills2[skillID].Level + 1));
                                Character.Skills2[skillID] = data;
                                Character.Skills2_1[skillID] = data;
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 3)
                {
                    if (!Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills2[skillID].Level);
                        if (Character.JobLevel2T < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint2T < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills2[skillID].Level == Character.Skills2[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint2T -= 1;
                                var data = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills2[skillID].Level + 1));
                                Character.Skills2[skillID] = data;
                                Character.Skills2_2[skillID] = data;
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 4)
                {
                    if (!Character.Skills3.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills3[skillID].Level);
                        if (Character.JobLevel3 < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint3 < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills3[skillID].Level == Character.Skills3[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint3 -= 1;
                                Character.Skills3[skillID] = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills3[skillID].Level + 1));
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }
            }

            p1.SkillPoints = Character.SkillPoint;
            if (Character.Job == Character.Job2X)
            {
                p1.SkillPoints2 = Character.SkillPoint2X;
                p1.Job = 1;
            }
            else if (Character.Job == Character.Job2T)
            {
                p1.SkillPoints2 = Character.SkillPoint2T;
                p1.Job = 2;
            }
            else if (Character.Job == Character.Job3)
            {
                p1.SkillPoints2 = Character.SkillPoint3;
                p1.Job = 3;
            }
            else
            {
                p1.Job = 0;
            }

            netIO.SendPacket(p1);
            SendSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            MapServer.charDB.SaveChar(Character, true);
        }

        public void OnSkillLearn(CSMG_SKILL_LEARN p)
        {
            var p1 = new SSMG_SKILL_LEARN();
            var skillID = p.SkillID;
            byte type = 0;
            if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(skillID))
                type = 1;
            else if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(skillID))
                type = 2;
            else if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(skillID))
                type = 3;
            else if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(skillID))
                type = 4;
            if (type == 0)
            {
                p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_EXIST;
            }
            else
            {
                if (type == 1)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.JobBasic)[skillID];
                    if (Character.Skills.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;

                            if (Character.JobLevel1 < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }

                            else
                            {
                                Character.SkillPoint -= 3;
                                Character.Skills.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                if (Character.Job == Character.Job2X)
                                    p1.SkillPoints2 = Character.SkillPoint2X;
                                else if (Character.Job == Character.Job2T)
                                    p1.SkillPoints2 = Character.SkillPoint2T;
                            }
                        }
                    }
                }
                else if (type == 2)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job2X)[skillID];
                    if (Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint2X < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel2X < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint2X -= 3;
                                Character.Skills2.Add(skillID, skill);
                                Character.Skills2_1.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint2X;
                            }
                        }
                    }
                }
                else if (type == 3)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job2T)[skillID];

                    if (Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint2T < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel2T < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint2T -= 3;
                                Character.Skills2.Add(skillID, skill);
                                Character.Skills2_2.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint2T;
                            }
                        }
                    }
                }
                else if (type == 4)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job3)[skillID];

                    if (Character.Skills3.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint3 < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel3 < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint3 -= 3;
                                Character.Skills3.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint3;
                            }
                        }
                    }
                }
            }

            netIO.SendPacket(p1);
            SendSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            MapServer.charDB.SaveChar(Character, true);
        }

        public void OnSkillAttack(CSMG_SKILL_ATTACK p, bool auto)
        {
            var needthread = true;

            if (Character == null)
                return;
            if (!Character.Online || Character.HP == 0)
                return;

            var dActor = Map.GetActor(p.ActorID);
            SkillArg arg;

            var sActor = map.GetActor(Character.ActorID);
            if (sActor == null) return;
            if (dActor == null) return;
            if (sActor.MapID != dActor.MapID) return;
            if (sActor.TInt["targetID"] != dActor.ActorID)
            {
                sActor.TInt["targetID"] = (int)dActor.ActorID;
                //SendSystemMessage("锁定了【" + dActor.Name + "】作为目标");
                //Character.AutoAttack = true;

                Character.PartnerTartget = dActor; // Partner will follow the entity assigned to PartnerTarget.
            }

            if (needthread)
            {
                if (!auto && Character.AutoAttack) //客户端发来的攻击，但已开启自动
                {
                    Character.TInt["攻击检测"] += 1;
                    if (Character.TInt["攻击检测"] >= 3)
                        ScriptManager.Instance.VariableHolder.AInt[Character.Name + "攻击检测"] += Character.TInt["攻击检测"];
                    Lastp = p;
                    //return;
                }

                if (auto && !Character.AutoAttack) //自动攻击，但人物处于不能自动攻击状态
                    return;
            }

            byte s = 0;

            //射程判定
            if (Character == null || dActor == null)
                return;
            if (Character.Range + 1
                < Math.Max(Math.Abs(Character.X - dActor.X) / 100
                    , Math.Abs(Character.Y - dActor.Y) / 100))
            {
                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            Character.LastAttackActorID = 0;

            //this.lastAttackRandom = p.Random;
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (Character.Status.Additions.ContainsKey("Stun") || Character.Status.Additions.ContainsKey("Sleep") ||
                Character.Status.Additions.ContainsKey("Frosen") ||
                Character.Status.Additions.ContainsKey("Stone"))
                return;
            if (dActor == null || DateTime.Now < attackStamp)
            {
                if (s == 1)
                {
                    arg = new SkillArg();
                    arg.sActor = Character.ActorID;
                    arg.type = (ATTACK_TYPE)0xff;
                    arg.affectedActors.Add(Character);
                    arg.Init();
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                    Character.AutoAttack = false;
                    return;
                }

                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            if (dActor.HP == 0 || dActor.Buff.Dead)
            {
                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            arg = new SkillArg();

            delay = (int)(2000 * (1.0f - Character.Status.aspd / 1000.0f));
            delay = (int)(delay * arg.delayRate);
            if (Character.Status.aspd_skill_perc >= 1f)
                delay = (int)(delay / Character.Status.aspd_skill_perc);

            if (!needthread && Character.HP > 0)
                SkillHandler.Instance.Attack(Character, dActor, arg); //攻击

            if (Character.HP > 0 && !AttactFinished && needthread) //处于战斗状态
                SkillHandler.Instance.Attack(Character, dActor, arg); //攻击

            if (arg.affectedActors.Count > 0)
                attackStamp = DateTime.Now + new TimeSpan(0, 0, 0, 0, delay);

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);

            AttactFinished = false;
            PartnerTalking(Character.Partner, TALK_EVENT.BATTLE, 1, 20000);
            //新加
            if (needthread && s == 0)
            {
                Lastp = p;
                Character.LastAttackActorID = dActor.ActorID;
                delay = (int)(2000 * (1.0f - Character.Status.aspd / 1000.0f));
                delay = (int)(delay * arg.delayRate);
                if (Character.Status.aspd_skill_perc >= 1f)
                    delay = (int)(delay / Character.Status.aspd_skill_perc);

                //谜一样的弓,双枪延迟缩短,先注释掉
                //if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                //{
                //    ItemType it = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType;
                //    if (it == ItemType.DUALGUN || it == ItemType.BOW)
                //        delay = (int)(delay * 0.6f);
                //}

                try
                {
                    if (main != null)
                        ClientManager.RemoveThread(main.Name);
                    if (Character == null)
                        return;
                    if (this == null)
                        return;
                    main = new Thread(MainLoop);
                    main.Name = string.Format("ThreadPoolMainLoopAUTO({0})" + Character.Name, main.ManagedThreadId);
                    ClientManager.AddThread(main);
                    Character.AutoAttack = true;
                    main.Start();
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
            }
        }


        private void MainLoop()
        {
            try
            {
                if (Character == null)
                {
                    if (main != null)
                        ClientManager.RemoveThread(main.Name);
                    return;
                }

                if (this == null)
                    return;

                if (delay <= 0)
                    delay = 60;
                Thread.Sleep(delay);

                if (Character != null)
                {
                    OnSkillAttack(Lastp, true);
                    Character.TInt["攻击检测"] = 0;
                }
                else
                {
                    ClientManager.RemoveThread(main.Name);
                }
            }

            catch (Exception ex)
            {
                Logger.ShowError(main.Name + " Thread " + ex);
            }
        }


        public void OnSkillChangeBattleStatus(CSMG_SKILL_CHANGE_BATTLE_STATUS p)
        {
            if (p.Status == 0)
                Character.AutoAttack = false;

            if (Character.BattleStatus != p.Status)
            {
                Character.BattleStatus = p.Status;
                SendChangeStatus();
            }

            if (Character.Tasks.ContainsKey("RangeAttack") && Character.BattleStatus == 0)
            {
                Character.Tasks["RangeAttack"].Deactivate();
                Character.Tasks.Remove("RangeAttack");
                Character.TInt["RangeAttackMark"] = 0;
            }

            if (Character.Tasks.ContainsKey("SkillCast") && Character.BattleStatus == 0 &&
                (Character.Skills.ContainsKey(14000) || Character.Skills3.ContainsKey(14000)) &&
                (Character.Job == PC_JOB.CARDINAL || Character.Job == PC_JOB.ASTRALIST))
            {
                /*if (this.Character.Tasks["SkillCast"].getActivated())
                {
                    this.Character.Tasks["SkillCast"].Deactivate();
                    this.Character.Tasks.Remove("SkillCast");
                }*/

                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, Character, true);

                var p2 = new SSMG_SKILL_CAST_RESULT();
                p2.ActorID = Character.ActorID;
                p2.Result = 20;
                netIO.SendPacket(p2);
            }
        }

        public void OnSkillCast(CSMG_SKILL_CAST p)
        {
            OnSkillCast(p, true);
        }

        private bool checkSkill(uint skillID, byte skillLV)
        {
            var p = new SSMG_SKILL_LIST();
            Dictionary<uint, byte> skills;
            Dictionary<uint, byte> skills2X;
            Dictionary<uint, byte> skills2T;
            Dictionary<uint, byte> skills3;
            Dictionary<uint, byte> skillsHeat;
            var list = new List<SagaDB.Skill.Skill>();
            skills = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p1);
            skills2X = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p21);
            skills2T = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p22);
            skills3 = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p3);
            skillsHeat = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.none);

            if (Character.Skills.ContainsKey(skillID))
                if (Character.Skills[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2.ContainsKey(skillID))
                if (Character.Skills2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2_1.ContainsKey(skillID))
                if (Character.Skills2_1[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2_2.ContainsKey(skillID))
                if (Character.Skills2_2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2.ContainsKey(skillID))
                if (Character.Skills2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills3.ContainsKey(skillID))
                if (Character.Skills3[skillID].Level >= skillLV)
                    return true;
            if (Character.SkillsReserve.ContainsKey(skillID))
                if (Character.SkillsReserve[skillID].Level >= skillLV)
                    return true;
            if (Character.SkillsReserve.ContainsKey(skillID) && Character.DominionReserveSkill)
                if (Character.SkillsReserve[skillID].Level >= skillLV)
                    return true;
            if (Character.JobJoint != PC_JOB.NONE)
            {
                var skill =
                    from c in SkillFactory.Instance.SkillList(Character.JobJoint)
                    where c.Value <= Character.JointJobLevel
                    select c;
                foreach (var i in skill)
                    if (i.Key == skillID && Character.JointJobLevel >= i.Value)
                        return true;
            }

            return false;
        }

        public void OnSkillCast(CSMG_SKILL_CAST p, bool useMPSP)
        {
            OnSkillCast(p, useMPSP, false);
        }

        /// <summary>
        ///     检查技能是否符合使用条件
        /// </summary>
        /// <param name="skill">技能数据</param>
        /// <param name="arg">技能参数</param>
        /// <param name="mp">mp</param>
        /// <param name="sp">sp</param>
        /// <param name="ep">ep</param>
        /// <returns>结果</returns>
        private short CheckSkillUse(SagaDB.Skill.Skill skill, SkillArg arg, ushort mp, ushort sp, ushort ep)
        {
            if (SingleCDLst.ContainsKey(arg.skill.ID) && DateTime.Now < SingleCDLst[arg.skill.ID] &&
                !nextCombo.Contains(arg.skill.ID))
                return -30;
            if (arg.skill.ID == 3372)
            {
                SingleCDLst.Clear();
                return 0;
            }

            if (DateTime.Now < skillDelay && !nextCombo.Contains(arg.skill.ID))
                return -30;
            if (Character.SP < sp || Character.MP < mp || Character.EP < ep)
            {
                if (Character.SP < sp && Character.MP < mp)
                    return -1;
                if (Character.SP < sp)
                    return -16;
                if (Character.MP < mp)
                    return -15;
                return -62;
            }

            if (!SkillHandler.Instance.CheckSkillCanCastForWeapon(Character, arg))
                return -5;

            if (Character.Status.Additions.ContainsKey("Silence"))
                return -7;

            if (Character.Status.Additions.ContainsKey("居合模式"))
            {
                if (arg.skill.ID != 2129)
                    return -7;
                Character.Status.Additions["居合模式"].AdditionEnd();
                Character.Status.Additions.Remove("居合模式");
            }

            if (GetPossessionTarget() != null)
                if (GetPossessionTarget().Buff.Dead && arg.skill.ID != 3055)
                    return -27;
            if (scriptThread != null) return -59;
            if (skill.NoPossession)
                if (Character.Buff.GetReadyPossession || Character.PossessionTarget != 0)
                    return -25;

            if (skill.NotBeenPossessed)
                if (Character.PossesionedActors.Count > 0)
                    return -24;

            if (Character.Tasks.ContainsKey("SkillCast"))
            {
                if (arg.skill.ID == 3311)
                    return 0;
                return -8;
            }

            var res = (short)SkillHandler.Instance.TryCast(Character, Map.GetActor(arg.dActor), arg);
            if (res < 0)
                return res;
            return 0;
        }

        public void OnSkillCast(CSMG_SKILL_CAST p, bool useMPSP, bool nocheck)
        {
            if (((!checkSkill(p.SkillID, p.SkillLv) && Character.Account.GMLevel < 2) ||
                 (p.Random == lastCastRandom && Character.Account.GMLevel < 2)) && !nocheck)
            {
                SendHack();
                if (hackCount > 2)
                    return;
            }

            //断掉自动放技能
            Character.AutoAttack = false;
            if (main != null)
                ClientManager.RemoveThread(main.Name);


            lastCastRandom = p.Random;
            var skill = SkillFactory.Instance.GetSkill(p.SkillID, p.SkillLv);
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (Character.Tasks.ContainsKey("Regeneration"))
            {
                Character.Tasks["Regeneration"].Deactivate();
                Character.Tasks.Remove("Regeneration");
            }

            var arg = new SkillArg();
            arg.sActor = Character.ActorID;
            arg.dActor = p.ActorID;
            arg.skill = skill;
            arg.x = p.X;
            arg.y = p.Y;
            arg.argType = SkillArg.ArgType.Cast;
            ushort sp, mp, ep;
            //凭依时消耗加倍
            if (Character.PossessionTarget != 0)
            {
                sp = (ushort)(skill.SP * 2);
                mp = (ushort)(skill.MP * 2);
            }
            else
            {
                sp = skill.SP;
                mp = skill.MP;
            }

            if (Character.Status.Additions.ContainsKey("SwordEaseSp"))
                //sp = (ushort)(skill.SP * 2);
                //mp = (ushort)(skill.MP * 2);
                sp = (ushort)(skill.SP * 0.7);
            //mp = (ushort)(skill.MP * 0.7);
            if (Character.Status.Additions.ContainsKey("元素解放"))
            {
                sp = (ushort)(skill.SP * 2);
                mp = (ushort)(skill.MP * 2);
            }

            if (Character.Status.zenList.Contains((ushort)skill.ID) ||
                Character.Status.darkZenList.Contains((ushort)skill.ID))
                mp = (ushort)(mp * 2);

            if (Character.Status.Additions.ContainsKey("EnergyExcess")) //能量增幅耗蓝加深
            {
                float[] rate = { 0, 0.05f, 0.16f, 0.28f, 0.4f, 0.65f };
                mp += (ushort)(skill.MP * rate[Character.TInt["EnergyExcess"]]);
            }

            if (!useMPSP)
            {
                sp = 0;
                mp = 0;
            }

            ep = skill.EP;
            arg.useMPSP = useMPSP;
            //检查技能是否复合使用条件 0为符合, 其他为使用失败
            arg.result = CheckSkillUse(skill, arg, mp, sp, ep);

            if (arg.result == 0)
            {
                //使物理技能的读条时间受aspd影响,法系读条受cspd影响.
                //2018.07.13 现在魔法系职业的读条时间不可能小于0.5秒.
                if (skill.BaseData.flag.Test(SkillFlags.PHYSIC))
                    arg.delay = (uint)(skill.CastTime * (1.0f - Character.Status.aspd / 1000.0f));
                else
                    arg.delay = (uint)Math.Max(skill.CastTime * (1.0f - Character.Status.cspd / 1000.0f), 500);
                if (arg.skill.ID == 2559)
                {
                    if (Character.Gold >= 90000000)
                        arg.delay = (uint)(arg.delay * 0.5f);
                    else if (Character.Gold >= 50000000)
                        arg.delay = (uint)(arg.delay * 0.6f);
                    else if (Character.Gold >= 5000000)
                        arg.delay = (uint)(arg.delay * 0.7f);
                    else if (Character.Gold >= 500000)
                        arg.delay = (uint)(arg.delay * 0.8f);
                    else if (Character.Gold >= 50000)
                        arg.delay = (uint)(arg.delay * 0.9f);
                }

                if (Character.Status.delayCancelList.ContainsKey((ushort)arg.skill.ID))
                {
                    var rate = Character.Status.delayCancelList[(ushort)arg.skill.ID];
                    arg.delay = (uint)(arg.delay * (1f - rate / 100.0f));
                }

                //bool get = Character.Status.Additions.ContainsKey("EaseCt");
                if (Character.Status.Additions.ContainsKey("EaseCt") && arg.skill.ID != 2238) //杀界模块
                {
                    var eclv = new[] { 0f, 0.5f, 0.7f, 0.8f, 0.9f, 1.0f }[Character.Status.EaseCt_lv];
                    arg.delay = (uint)(arg.delay * (1.0f - eclv));
                    SkillHandler.RemoveAddition(Character, "EaseCt");
                }


                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);
                //if (this.Character.Status.Additions.ContainsKey("SwordEaseSp"))
                //{
                //    this.nextCombo.Clear();
                //    OnSkillCastComplete(arg);
                //}
                //else 
                if (skill.CastTime > 0 && !nextCombo.Contains(arg.skill.ID))
                {
                    var task = new SkillCast(this, arg);
                    Character.Tasks.Add("SkillCast", task);

                    task.Activate();
                    nextCombo.Clear();
                    ;
                }
                else
                {
                    nextCombo.Clear();
                    OnSkillCastComplete(arg);
                }

                if (Character.Status.Additions.ContainsKey("Parry"))
                    arg.delay = (uint)arg.skill.BaseData.delay;
            }
            else
            {
                Character.e.OnActorSkillUse(Character, arg);
            }
        }

        public void OnSkillCastComplete(SkillArg skill)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (skill.dActor != 0xFFFFFFFF)
            {
                var dActor = Map.GetActor(skill.dActor);
                if (dActor != null)
                {
                    skill.argType = SkillArg.ArgType.Active;
                    PartnerTalking(Character.Partner, TALK_EVENT.BATTLE, 1, 20000);
                    if (skill.useMPSP)
                    {
                        uint mpCost = skill.skill.MP;
                        uint spCost = skill.skill.SP;
                        uint epCost = skill.skill.EP;
                        if (Character.Status.sp_rate_down_iris < 100)
                            spCost = (uint)(spCost * (Character.Status.sp_rate_down_iris / 100.0f));
                        if (Character.Status.mp_rate_down_iris < 100)
                            mpCost = (uint)(mpCost * (Character.Status.mp_rate_down_iris / 100.0f));

                        if (Character.Status.doubleUpList.Contains((ushort)skill.skill.ID))
                            spCost = (ushort)(spCost * 2);

                        if (Character.Status.Additions.ContainsKey("SwordEaseSp"))
                            //mpCost = (ushort)(mpCost*0.7);
                            spCost = (ushort)(spCost * 0.7);
                        if (Character.Status.Additions.ContainsKey("HarvestMaster"))
                        {
                            mpCost = (ushort)(mpCost * (float)(1.0f - Character.Status.HarvestMaster_Lv * 0.05));
                            spCost = (ushort)(spCost * (float)(1.0f - Character.Status.HarvestMaster_Lv * 0.05));
                        }

                        if (skill.skill.ID == 2527 && (Character.Skills2_2.ContainsKey(2355) ||
                                                       Character.DualJobSkill.Exists(x => x.ID == 2355))) //当技能为神速斩
                        {
                            //这里取副职的拔刀斩等级
                            var duallv = 0;
                            if (Character.DualJobSkill.Exists(x => x.ID == 2355))
                                duallv = Character.DualJobSkill.FirstOrDefault(x => x.ID == 2355).Level;

                            //这里取主职的拔刀斩等级
                            var mainlv = 0;
                            if (Character.Skills2_2.ContainsKey(2355))
                                mainlv = Character.Skills2_2[2355].Level;
                            //获取最高的拔刀斩等级
                            var maxlevel = Math.Max(duallv, mainlv);
                            spCost = (ushort)(spCost - spCost * maxlevel * 0.04f);
                        }

                        if (Character.PossessionTarget != 0)
                        {
                            mpCost = (ushort)(mpCost * 2);
                            spCost = (ushort)(spCost * 2);
                        }

                        if (Character.Status.Additions.ContainsKey("Zensss"))
                            mpCost *= 2;

                        if (Character.Status.Additions.ContainsKey("EnergyExcess")) //能量增幅耗蓝加深
                        {
                            float[] rate = { 0, 0.05f, 0.16f, 0.28f, 0.4f, 0.65f };
                            mpCost += (ushort)(mpCost * rate[Character.TInt["EnergyExcess"]]);
                        }

                        if (mpCost > Character.MP && spCost > Character.SP)
                        {
                            skill.result = -1;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        if (mpCost > Character.MP)
                        {
                            skill.result = -15;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        if (spCost > Character.SP)
                        {
                            skill.result = -16;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        Character.MP -= mpCost;
                        if (Character.MP < 0)
                            Character.MP = 0;

                        Character.SP -= spCost;
                        if (Character.SP < 0)
                            Character.SP = 0;

                        Character.EP -= epCost;
                        if (Character.EP < 0)
                            Character.EP = 0;

                        SendActorHPMPSP(Character);
                    }

                    SkillHandler.Instance.SkillCast(Character, dActor, skill);
                }
                else
                {
                    skill.result = -11;
                    Character.e.OnActorSkillUse(Character, skill);
                }
            }
            else
            {
                skill.argType = SkillArg.ArgType.Active;
                if (skill.useMPSP)
                {
                    if (skill.skill.MP > Character.MP && skill.skill.SP > Character.SP)
                    {
                        skill.result = -1;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    if (skill.skill.MP > Character.MP)
                    {
                        skill.result = -15;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    if (skill.skill.SP > Character.SP)
                    {
                        skill.result = -16;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    Character.MP -= skill.skill.MP;
                    Character.SP -= skill.skill.SP;
                    SendActorHPMPSP(Character);
                }

                SkillHandler.Instance.SkillCast(Character, Character, skill);
            }

            if (Character.Pet != null)
                if (Character.Pet.Ride)
                    SkillHandler.Instance.ProcessPetGrowth(Character.Pet, PetGrowthReason.UseSkill);

            //技能延迟
            //if (this.Character.Status.Additions.ContainsKey("SwordEaseSp"))
            //{
            //    skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay * 0.2f));
            //}
            //else 
            if (Character.Status.delayCancelList.ContainsKey((ushort)skill.skill.ID))
                skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0,
                    (int)(skill.skill.Delay *
                          (1f - Character.Status.delayCancelList[(ushort)skill.skill.ID] / 100.0f)));
            else
                skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.Delay);

            //if (this.Character.Status.Additions.ContainsKey("DelayOut"))
            //    skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1);

            if (Character.Status.Additions.ContainsKey("OverWork") &&
                !skill.skill.BaseData.flag.Test(SkillFlags.PHYSIC)) //狂乱时间
            {
                var DelayTime = (Character.Status.Additions["OverWork"] as DefaultBuff).Variable["OverWork"];
                skillDelay = DateTime.Now +
                             new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay * (1f - DelayTime / 100.0f)));
            }

            if (Character.Status.aspd_skill_perc >= 1f)
                skillDelay = DateTime.Now +
                             new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay / Character.Status.aspd_skill_perc));

            //独立cd
            if (!SingleCDLst.ContainsKey(skill.skill.ID))
                SingleCDLst.Add(skill.skill.ID, DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.SinglgCD));
            else
                SingleCDLst[skill.skill.ID] = DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.SinglgCD);
            //if (!this.Character.Status.Additions.ContainsKey("DelayOut"))
            //{
            //    

            //}

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, Character, true);

            if (skill.skill.Effect != 0 &&
                (skill.skill.Target == 4 || (skill.skill.Target == 2 && skill.sActor == skill.dActor)))
            {
                var eff = new EffectArg();
                eff.actorID = skill.dActor;
                eff.effectID = skill.skill.Effect;
                eff.x = skill.x;
                eff.y = skill.y;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, eff, Character, true);
            }

            if (Character.Tasks.ContainsKey("AutoCast"))
            {
                Character.Tasks["AutoCast"].Activate();
            }
            else
            {
                if (skill.autoCast.Count != 0)
                {
                    Character.Buff.CannotMove = true;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                    var task = new AutoCast(Character, skill);
                    Character.Tasks.Add("AutoCast", task);
                    task.Activate();
                }
            }
        }

        public void SendChangeStatus()
        {
            if (Character.Tasks.ContainsKey("Regeneration"))
            {
                Character.Tasks["Regeneration"].Deactivate();
                Character.Tasks.Remove("Regeneration");
            }

            if (Character.Motion != MotionType.NONE && Character.Motion != MotionType.DEAD)
            {
                Character.Motion = MotionType.NONE;
                Character.MotionLoop = false;
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, Character, true);
        }

        public void SendRevive(byte level)
        {
            Character.Buff.Dead = false;
            Character.Buff.TurningPurple = false;
            Character.Motion = MotionType.STAND;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);

            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 0.1f;
                    break;
                case 2:
                    factor = 0.2f;
                    break;
                case 3:
                    factor = 0.45f;
                    break;
                case 4:
                    factor = 0.5f;
                    break;
                case 5:
                    factor = 0.75f;
                    break;
                case 6:
                    factor = 1f;
                    break;
            }

            Character.HP = (uint)(Character.MaxHP * factor);
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, Character, true);
            var arg = new SkillArg();
            arg.sActor = Character.ActorID;
            arg.dActor = 0;
            arg.skill = SkillFactory.Instance.GetSkill(10002, level);
            arg.x = 0;
            arg.y = 0;
            arg.hp = new List<int>();
            arg.sp = new List<int>();
            arg.mp = new List<int>();
            arg.hp.Add((int)(-Character.MaxHP * factor));
            arg.sp.Add(0);
            arg.mp.Add(0);
            arg.flag.Add(AttackFlag.HP_HEAL);
            arg.affectedActors.Add(Character);
            arg.argType = SkillArg.ArgType.Active;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);

            if (!Character.Tasks.ContainsKey("AutoSave"))
            {
                var task = new AutoSave(Character);
                Character.Tasks.Add("AutoSave", task);
                task.Activate();
            }

            if (!Character.Tasks.ContainsKey("Recover")) //自然恢复
            {
                var reg = new Recover(FromActorPC(Character));
                Character.Tasks.Add("Recover", reg);
                reg.Activate();
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();
        }

        public void SendSkillList()
        {
            var p = new SSMG_SKILL_LIST();
            Dictionary<uint, byte> skills;
            Dictionary<uint, byte> skills2X;
            Dictionary<uint, byte> skills2T;
            Dictionary<uint, byte> skills3;
            var list = new List<SagaDB.Skill.Skill>();
            var ifDominion = map.Info.Flag.Test(MapFlags.Dominion);
            if (ifDominion)
            {
                skills = new Dictionary<uint, byte>();
                skills2X = new Dictionary<uint, byte>();
                skills2T = new Dictionary<uint, byte>();
                skills3 = new Dictionary<uint, byte>();
            }
            else
            {
                skills = SkillFactory.Instance.SkillList(Character.JobBasic);
                skills2X = SkillFactory.Instance.SkillList(Character.Job2X);
                skills2T = SkillFactory.Instance.SkillList(Character.Job2T);
                skills3 = SkillFactory.Instance.SkillList(Character.Job3);
            }

            {
                var skill =
                    from c in skills.Keys
                    where !Character.Skills.ContainsKey(c)
                    select c;
                foreach (var i in skill)
                {
                    var sk = SkillFactory.Instance.GetSkill(i, 0);
                    list.Add(sk);
                }

                foreach (var i in Character.Skills.Values) list.Add(i);
            }
            p.Skills(list, 0, Character.JobBasic, ifDominion, Character);
            netIO.SendPacket(p);
            if (Character.Rebirth || Character.Job == Character.Job3)
            {
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2X.Keys
                            where !Character.Skills2_1.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2_1.Values) list.Add(i);
                    }

                    p.Skills(list, 1, Character.Job2X, ifDominion, Character);
                    netIO.SendPacket(p);
                }
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2T.Keys
                            where !Character.Skills2_2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2_2.Values) list.Add(i);
                    }
                    p.Skills(list, 2, Character.Job2T, ifDominion, Character);
                    netIO.SendPacket(p);
                }
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills3.Keys
                            where !Character.Skills3.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills3.Values) list.Add(i);
                    }

                    p.Skills(list, 3, Character.Job3, ifDominion, Character);
                    netIO.SendPacket(p);
                }
            }
            else
            {
                if (Character.Job == Character.Job2X)
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2X.Keys
                            where !Character.Skills2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2.Values) list.Add(i);
                    }

                    p.Skills(list, 1, Character.Job2X, ifDominion, Character);
                    netIO.SendPacket(p);
                }

                if (Character.Job == Character.Job2T)
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2T.Keys
                            where !Character.Skills2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2.Values) list.Add(i);
                    }
                    p.Skills(list, 2, Character.Job2T, ifDominion, Character);
                    netIO.SendPacket(p);
                }

                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    if (Character.DominionReserveSkill)
                    {
                        var p2 = new SSMG_SKILL_RESERVE_LIST();
                        p2.Skills = Character.SkillsReserve.Values.ToList();
                        netIO.SendPacket(p2);
                    }
                    else
                    {
                        var p2 = new SSMG_SKILL_RESERVE_LIST();
                        p2.Skills = new List<SagaDB.Skill.Skill>();
                        netIO.SendPacket(p2);
                    }
                }
                else
                {
                    var p2 = new SSMG_SKILL_RESERVE_LIST();
                    p2.Skills = Character.SkillsReserve.Values.ToList();
                    netIO.SendPacket(p2);
                }
            }


            if (Character.JobJoint != PC_JOB.NONE)
            {
                list.Clear();
                {
                    var skill =
                        from c in SkillFactory.Instance.SkillList(Character.JobJoint)
                        where c.Value <= Character.JointJobLevel
                        select c;
                    foreach (var i in skill)
                    {
                        var sk = SkillFactory.Instance.GetSkill(i.Key, 1);
                        list.Add(sk);
                    }
                }
                var p2 = new SSMG_SKILL_JOINT_LIST();
                p2.Skills = list;
                netIO.SendPacket(p2);
            }
            else
            {
                var p2 = new SSMG_SKILL_JOINT_LIST();
                p2.Skills = new List<SagaDB.Skill.Skill>();
                netIO.SendPacket(p2);
            }
        }

        public void SendSkillDummy()
        {
            SendSkillDummy(3311, 1);
        }

        public void SendSkillDummy(uint skillid, byte level)
        {
            if (Character.Tasks.ContainsKey("SkillCast"))
            {
                if (Character.Tasks["SkillCast"].getActivated())
                {
                    Character.Tasks["SkillCast"].Deactivate();
                    Character.Tasks.Remove("SkillCast");
                }

                var arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.dActor = 0;
                arg.skill = SkillFactory.Instance.GetSkill(skillid, level);
                arg.x = 0;
                arg.y = 0;
                arg.hp = new List<int>();
                arg.sp = new List<int>();
                arg.mp = new List<int>();
                arg.hp.Add(0);
                arg.sp.Add(0);
                arg.mp.Add(0);
                arg.flag.Add(AttackFlag.NONE);
                //arg.affectedActors.Add(this.Character);
                arg.argType = SkillArg.ArgType.Active;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);
            }
        }

        public void OnSkillRangeAttack(CSMG_SKILL_RANGE_ATTACK p)
        {
            var p2 = new SSMG_SKILL_RANGEA_RESULT();
            p2.ActorID = p.ActorID;
            if (!Character.Status.Additions.ContainsKey("自由射击"))
                p2.Speed = 410;
            else
                p2.Speed = 0;
            netIO.SendPacket(p2);
            Character.TTime["远程蓄力"] = DateTime.Now;

            if (Character.Tasks.ContainsKey("RangeAttack"))
            {
                Character.Tasks["RangeAttack"].Deactivate();
                Character.Tasks.Remove("RangeAttack");
            }

            var ra = new RangeAttack(this);
            Character.Tasks.Add("RangeAttack", ra);
            ra.Activate();
        }

        /// <summary>
        ///     重置技能
        /// </summary>
        /// <param name="job">1为1转，2为2转</param>
        public void ResetSkill(byte job)
        {
            var totalPoints = 0;
            var delList = new List<uint>();
            switch (job)
            {
                case 1:
                    foreach (var i in Character.Skills.Values)
                        if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(i.ID))
                        {
                            totalPoints += i.Level + 2;
                            delList.Add(i.ID);
                        }

                    Character.SkillPoint += (ushort)totalPoints;
                    foreach (var i in delList) Character.Skills.Remove(i);
                    break;
                case 2:
                    if (!Character.Rebirth)
                    {
                        foreach (var i in Character.Skills2.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList) Character.Skills2.Remove(i);
                        if (Character.Job == Character.Job2X)
                            Character.SkillPoint2X += (ushort)totalPoints;
                        if (Character.Job == Character.Job2T)
                            Character.SkillPoint2T += (ushort)totalPoints;
                    }
                    else
                    {
                        Character.SkillPoint2X = 0;
                        foreach (var i in Character.Skills2_1.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList)
                        {
                            Character.Skills2_1.Remove(i);
                            Character.Skills2.Remove(i);
                        }

                        Character.SkillPoint2X += (ushort)totalPoints;

                        totalPoints = 0;
                        delList.Clear();
                        Character.SkillPoint2T = 0;
                        foreach (var i in Character.Skills2_2.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList)
                        {
                            Character.Skills2_2.Remove(i);
                            Character.Skills2.Remove(i);
                        }

                        Character.SkillPoint2T += (ushort)totalPoints;
                    }

                    break;
                case 3:
                    foreach (var i in Character.Skills3.Values)
                        if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(i.ID))
                        {
                            totalPoints += i.Level + 2;
                            delList.Add(i.ID);
                        }

                    Character.SkillPoint3 += (ushort)totalPoints;
                    foreach (var i in delList) Character.Skills3.Remove(i);
                    break;
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
        }

        public void OnFishBaitsEquip(CSMG_FF_FISHBAIT_EQUIP p)
        {
            if (p.InventorySlot == 0)
            {
                Character.EquipedBaitID = 0;

                var p2 = new SSMG_FISHBAIT_EQUIP_RESULT();
                p2.InventoryID = 0;
                p2.IsEquip = 1;
                netIO.SendPacket(p2);
            }
            else
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item.ItemID >= 10104900 || item.ItemID <= 10104906)
                {
                    Character.EquipedBaitID = item.ItemID;

                    var p2 = new SSMG_FISHBAIT_EQUIP_RESULT();
                    p2.InventoryID = p.InventorySlot;
                    p2.IsEquip = 0;
                    netIO.SendPacket(p2);
                }
            }
        }
    }
}