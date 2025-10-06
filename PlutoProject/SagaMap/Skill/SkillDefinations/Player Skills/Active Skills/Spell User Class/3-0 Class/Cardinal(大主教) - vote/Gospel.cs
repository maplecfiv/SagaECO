using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote
{
    /// <summary>
    ///     ゴスペル
    /// </summary>
    public class Gospel : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly int countMax;
            private readonly int[] countMaxs = { 0, 5, 5, 16, 10, 13 };
            private readonly float factor;
            private readonly float[] factors = { 0f, -0.8f, -0.9f, -0.7f, -1.0f, -1.1f, -100f };
            private readonly int lifetime;
            private readonly Map map;
            private readonly SkillArg skill;
            private int count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                countMax = countMaxs[level];
                map = MapManager.Instance.GetMap(actor.MapID);
                var periods = new[] { 0, 1000, 1000, 500, 1000, 800, 100 };
                lifetime = 60000 * level;
                factor = factors[level] + caster.Status.Cardinal_Rank;
                Period = periods[level];
                DueTime = 0;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(actor, 200, false);
                        var affected = new List<Actor>();
                        //取得有效Actor（即怪物）

                        //施加火属性魔法伤害
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (i.type == ActorType.PC)
                            {
                                var pc = (ActorPC)i;
                                if (!SkillHandler.Instance.CheckValidAttackTarget(caster, pc) && pc.Online)
                                {
                                    affected.Add(pc);
                                    if (!pc.Status.Additions.ContainsKey("GospelBonus"))
                                    {
                                        var gospelBonus = new DefaultBuff(skill.skill, pc, "GospelBonus", lifetime);
                                        gospelBonus.OnAdditionStart += gospelBonus_OnAdditionStart;
                                        gospelBonus.OnAdditionEnd += gospelBonus_OnAdditionEnd;
                                        SkillHandler.ApplyAddition(pc, gospelBonus);
                                    }
                                }
                            }

                        SkillHandler.Instance.MagicAttack(caster, affected, skill, SkillHandler.DefType.IgnoreAll,
                            Elements.Holy, factor);

                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        count++;
                    }
                    else
                    {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                //解开同步锁ClientManager.LeaveCriticalArea();
            }

            private void gospelBonus_OnAdditionStart(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;
                var max_atk1_add = 30 + 30 * skill.skill.Level;
                var max_atk2_add = 30 + 30 * skill.skill.Level;
                var max_atk3_add = 30 + 30 * skill.skill.Level;
                var min_atk1_add = 30 + 30 * skill.skill.Level;
                var min_atk2_add = 30 + 30 * skill.skill.Level;
                var min_atk3_add = 30 + 30 * skill.skill.Level;
                var max_matk_add = 30 + 30 * skill.skill.Level;
                var min_matk_add = 30 + 30 * skill.skill.Level;
                var def_add = 30 + 10 * level;
                var mdef_add = 30 + 10 * level;
                var hpbonus = (uint)(actor.MaxHP * ((10 + 5 * level) / 100.0f));
                var mpbonus = (uint)(actor.MaxMP * ((10 + 5 * level) / 100.0f));
                var spbonus = (uint)(actor.MaxSP * ((10 + 5 * level) / 100.0f));

                if (skill.Variable.ContainsKey("GospelBonusHP"))
                    skill.Variable.Remove("GospelBonusHP");
                skill.Variable.Add("GospelBonusHP", (int)hpbonus);
                actor.Status.hp_skill += (short)hpbonus;

                if (skill.Variable.ContainsKey("GospelBonusMP"))
                    skill.Variable.Remove("GospelBonusMP");
                skill.Variable.Add("GospelBonusMP", (int)mpbonus);
                actor.Status.mp_skill += (short)mpbonus;

                if (skill.Variable.ContainsKey("GospelBonusSP"))
                    skill.Variable.Remove("GospelBonusSP");
                skill.Variable.Add("GospelBonusSP", (int)spbonus);
                actor.Status.sp_skill += (short)spbonus;

                //大傷
                if (skill.Variable.ContainsKey("GospelMax_ATK1"))
                    skill.Variable.Remove("GospelBonus_Max_ATK1");
                skill.Variable.Add("GospelBonus_Max_ATK1", max_atk1_add);
                actor.Status.max_atk1_skill += (short)max_atk1_add;

                if (skill.Variable.ContainsKey("GospelBonus_Max_ATK2"))
                    skill.Variable.Remove("GospelBonus_Max_ATK2");
                skill.Variable.Add("GospelBonus_Max_ATK2", max_atk2_add);
                actor.Status.max_atk2_skill += (short)max_atk2_add;

                if (skill.Variable.ContainsKey("GospelBonus_Max_ATK3"))
                    skill.Variable.Remove("GospelBonus_Max_ATK3");
                skill.Variable.Add("GospelBonus_Max_ATK3", max_atk3_add);
                actor.Status.max_atk3_skill += (short)max_atk3_add;

                //小伤
                if (skill.Variable.ContainsKey("GospelBonus_Min_ATK1"))
                    skill.Variable.Remove("GospelBonus_Min_ATK1");
                skill.Variable.Add("GospelBonus_Min_ATK1", min_atk1_add);
                actor.Status.min_atk1_skill += (short)min_atk1_add;

                if (skill.Variable.ContainsKey("GospelBonus_Min_ATK2"))
                    skill.Variable.Remove("GospelBonus_Min_ATK2");
                skill.Variable.Add("GospelBonus_Min_ATK2", min_atk2_add);
                actor.Status.min_atk2_skill += (short)min_atk2_add;

                if (skill.Variable.ContainsKey("GospelBonus_Min_ATK3"))
                    skill.Variable.Remove("GospelBonus_Min_ATK3");
                skill.Variable.Add("GospelBonus_Min_ATK3", min_atk3_add);
                actor.Status.min_atk3_skill += (short)min_atk3_add;
                //魔伤
                if (skill.Variable.ContainsKey("GospelBonus_Max_MATK"))
                    skill.Variable.Remove("GospelBonus_Max_MATK");
                skill.Variable.Add("GospelBonus_Max_MATK", max_matk_add);
                actor.Status.max_matk_skill += (short)max_matk_add;

                if (skill.Variable.ContainsKey("GospelBonus_Min_MATK"))
                    skill.Variable.Remove("GospelBonus_Min_MATK");
                skill.Variable.Add("GospelBonus_Min_MATK", min_matk_add);
                actor.Status.min_matk_skill += (short)min_matk_add;

                //防御
                if (skill.Variable.ContainsKey("GospelBonus_Def"))
                    skill.Variable.Remove("GospelBonus_Def");
                skill.Variable.Add("GospelBonus_Def", def_add);
                actor.Status.def_add_skill += (short)def_add;

                if (skill.Variable.ContainsKey("GospelBonus_MDef"))
                    skill.Variable.Remove("GospelBonus_MDef");
                skill.Variable.Add("GospelBonus_MDef", mdef_add);
                actor.Status.mdef_add_skill += (short)mdef_add;

                actor.Buff.MaxAtkUp = true;
                actor.Buff.MaxMagicAtkUp = true;
                actor.Buff.MinAtkUp = true;
                actor.Buff.MinMagicAtkUp = true;
                actor.Buff.MaxHPUp = true;
                actor.Buff.MaxSPUp = true;
                actor.Buff.MaxMPUp = true;
                actor.Buff.DefUp = true;
                actor.Buff.MagicDefUp = true;

                //PC.StatusFactory.Instance.CalcHPMPSP((ActorPC)actor);
                //Network.Client.MapClient.FromActorPC((ActorPC)actor).SendPlayerInfo();
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void gospelBonus_OnAdditionEnd(Actor actor, DefaultBuff skill)
            {
                actor.Status.hp_skill -= (short)skill.Variable["GospelBonusHP"];

                actor.Status.mp_skill -= (short)skill.Variable["GospelBonusMP"];

                actor.Status.sp_skill -= (short)skill.Variable["GospelBonusSP"];

                //大傷
                actor.Status.max_atk1_skill -= (short)skill.Variable["GospelBonus_Max_ATK1"];
                actor.Status.max_atk2_skill -= (short)skill.Variable["GospelBonus_Max_ATK2"];
                actor.Status.max_atk3_skill -= (short)skill.Variable["GospelBonus_Max_ATK3"];

                //小傷
                actor.Status.min_atk1_skill -= (short)skill.Variable["GospelBonus_Min_ATK1"];
                actor.Status.min_atk2_skill -= (short)skill.Variable["GospelBonus_Min_ATK2"];
                actor.Status.min_atk3_skill -= (short)skill.Variable["GospelBonus_Min_ATK3"];

                //魔伤
                actor.Status.max_matk_skill -= (short)skill.Variable["GospelBonus_Max_MATK"];
                actor.Status.min_matk_skill -= (short)skill.Variable["GospelBonus_Min_MATK"];

                //防御
                actor.Status.def_add_skill -= (short)skill.Variable["GospelBonus_Def"];
                actor.Status.mdef_add_skill -= (short)skill.Variable["GospelBonus_MDef"];

                actor.Buff.MaxAtkUp = false;
                actor.Buff.MaxMagicAtkUp = false;
                actor.Buff.MinAtkUp = false;
                actor.Buff.MinMagicAtkUp = false;
                actor.Buff.MaxHPUp = false;
                actor.Buff.MaxSPUp = false;
                actor.Buff.MaxMPUp = false;
                actor.Buff.DefUp = false;
                actor.Buff.MagicDefUp = false;

                //PC.StatusFactory.Instance.CalcHPMPSP((ActorPC)actor);
                //Network.Client.MapClient.FromActorPC((ActorPC)actor).SendPlayerInfo();
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 300) ||
                map.CheckActorSkillIsHeal(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 300)) return -17;
            ////待测试
            //if (map.CheckActorSkillInRange(dActor.X, dActor.Y, 300))
            //{
            //    return -17;
            //}
            if (CheckPossible(pc))
                return 0;
            return -5;

            //return 0;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (SkillHandler.Instance.isEquipmentRight(pc, ItemType.STRINGS) ||
                    pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                    return true;
                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillIsHeal(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 300)) return;
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
            actor.e = new NullEventHandler();
            //在指定地图注册技能体Actor
            map.RegisterActor(actor);
            //设置Actor隐身属性为非
            actor.invisble = false;
            //广播隐身属性改变事件，以便让玩家看到技能体
            map.OnActorVisibilityChange(actor);
            //設置系
            actor.Stackable = false;
            //创建技能效果处理对象
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}