using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.
    Alchemist_炼金术士____far {
    /// <summary>
    ///     藥品投擲（薬品投擲）
    /// </summary>
    public class ThrowChemical : ISkill {
        //#region Timer

        private class Activator : MultiRunTask {
            private readonly ActorSkill actor;
            private readonly Map map;
            private readonly int rate;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private float factor;
            private int lifetime;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level) {
                int[] periods = { 0, 15000, 15000, 20000, 20000, 18000 };
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 0.1f * level;
                DueTime = 1000;
                Period = periods[level];
                lifetime = 20000;
                rate = 40 - 5 * level;
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack() {
                //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (lifetime > 0) {
                        lifetime -= Period;
                        var affected = map.GetActorsArea(actor, 150, false);
                        foreach (var act in affected)
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                                switch (skill.skill.Level) {
                                    case 1:
                                        if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                                SkillHandler.DefaultAdditions.鈍足, rate)) {
                                            var s1 = new MoveSpeedDown(skill.skill, act, 10000);
                                            SkillHandler.ApplyAddition(act, s1);
                                        }

                                        break;
                                    case 2:
                                        if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                                SkillHandler.DefaultAdditions.Silence, rate)) {
                                            var s2 = new Silence(skill.skill, act, 10000);
                                            SkillHandler.ApplyAddition(act, s2);
                                        }

                                        break;
                                    case 3:
                                        if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                                SkillHandler.DefaultAdditions.Poison, rate)) {
                                            var s3 = new Poison(skill.skill, act, 10000);
                                            SkillHandler.ApplyAddition(act, s3);
                                        }

                                        break;
                                    case 4:
                                        if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                                SkillHandler.DefaultAdditions.Confuse, rate)) {
                                            var s4 = new Confuse(skill.skill, act, 10000);
                                            SkillHandler.ApplyAddition(act, s4);
                                        }

                                        break;
                                    case 5:
                                        if (SkillHandler.Instance.CanAdditionApply(sActor, act,
                                                SkillHandler.DefaultAdditions.Stun, rate)) {
                                            var s5 = new Stun(skill.skill, act, 10000);
                                            SkillHandler.ApplyAddition(act, s5);
                                        }

                                        break;
                                }
                    }
                    else {
                        Deactivate();
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
                //解開同步鎖
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (!map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 150))
                switch (args.skill.Level) {
                    case 1:
                        if (SkillHandler.Instance.CountItem(pc, 10000308) > 0) {
                            SkillHandler.Instance.TakeItem(pc, 10000308, 1);
                            return 0;
                        }

                        return -2;
                    case 2:
                        if (SkillHandler.Instance.CountItem(pc, 10000303) > 0) {
                            SkillHandler.Instance.TakeItem(pc, 10000303, 1);
                            return 0;
                        }

                        return -2;
                    case 3:
                        if (SkillHandler.Instance.CountItem(pc, 10000302) > 0) {
                            SkillHandler.Instance.TakeItem(pc, 10000302, 1);
                            return 0;
                        }

                        return -2;
                    case 4:
                        if (SkillHandler.Instance.CountItem(pc, 10000300) > 0) {
                            SkillHandler.Instance.TakeItem(pc, 10000300, 1);
                            return 0;
                        }

                        return -2;
                    case 5:
                        if (SkillHandler.Instance.CountItem(pc, 10000307) > 0) {
                            SkillHandler.Instance.TakeItem(pc, 10000307, 1);
                            return 0;
                        }

                        return -2;
                }

            return -17;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 150)) return;
            //建立設置型技能實體
            var actor = new ActorSkill(args.skill, sActor);
            //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
            //設定技能位置
            actor.MapID = dActor.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            //設定技能的事件處理器，由於技能體不需要得到消息廣播，因此建立空處理器
            actor.e = new NullEventHandler();
            //在指定地圖註冊技能Actor
            map.RegisterActor(actor);
            //設置Actor隱身屬性為False
            actor.invisble = false;
            //廣播隱身屬性改變事件，以便讓玩家看到技能實體
            map.OnActorVisibilityChange(actor);
            //建立技能效果處理物件
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}