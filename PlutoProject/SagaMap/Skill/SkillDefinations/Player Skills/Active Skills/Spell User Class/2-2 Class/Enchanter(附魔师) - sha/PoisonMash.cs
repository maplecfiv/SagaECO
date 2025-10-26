using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha {
    /// <summary>
    ///     毒沼地帶（ポイズンマーシュ）
    /// </summary>
    public class PoisonMash : ISkill, MobISkill {
        private readonly bool MobUse;

        public PoisonMash() {
            MobUse = false;
        }

        public PoisonMash(bool MobUse) {
            this.MobUse = MobUse;
        }

        //#region Timer

        private class Activator : MultiRunTask {
            private readonly ActorSkill actor;
            private readonly float factor;
            private readonly Map map;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private readonly int times;
            private int count;
            private int lifetime;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level) {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 0.02f * level;
                DueTime = 0;
                Period = 500;
                lifetime = new[] { 0, 30000, 20000, 16000, 14000, 10000 }[level];
                times = new[] { 0, 60, 40, 33, 28, 20 }[level];
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            public override void CallBack() {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (lifetime > 0 && count < times) {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(actor, 300, false);
                        var realAffected = new List<Actor>();
                        //取得有效Actor（即怪物）
                        var d = (uint)(sActor.MaxHP * factor);
                        //施加魔法伤害
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i)) {
                                if (i is ActorMob) {
                                    if (!(i as ActorMob).BaseData.fly)
                                        realAffected.Add(i);
                                }
                                else {
                                    realAffected.Add(i);
                                }
                            }

                        SkillHandler.Instance.FixAttack(sActor, realAffected, skill, Elements.Earth, d);
                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        lifetime -= Period;
                        count++;
                    }
                    else {
                        Deactivate();
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 100))
                return -17;
            if (args.x >= map.Width || args.y >= map.Height)
                return -6;
            if (map.Info.earth[args.x, args.y] > 0)
                return 0;
            return -12;
        }

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 100)) {
                args.result = -17;
                return;
            }

            if (args.x >= map.Width || args.y >= map.Height) {
                args.result = -6;
                return;
            }

            if (map.Info.earth[args.x, args.y] > 0)
                args.result = 0;
            else
                args.result = -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.Info.earth[args.x, args.y] == 0) {
                MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("无法在指定的坐标使用");
                return;
            }

            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            //设定技能体位置
            actor.MapID = dActor.MapID;
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
            if (MobUse) level = 1;
            //创建技能效果处理对象
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}