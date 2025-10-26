using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote {
    /// <summary>
    ///     流行演奏（ポップス）
    /// </summary>
    public class PopMusic : ISkill {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置
            actor.MapID = sActor.MapID;
            actor.X = sActor.X;
            actor.Y = sActor.Y;
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

            //註冊施放者的Buff
            var skill = new DefaultBuff(args.skill, sActor, "MAGINTDEXUPCircle_Caster", 30000 + 30000 * level);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
            if (sActor.skillsong != null) map.DeleteActor(sActor.skillsong);
            sActor.skillsong = actor;
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill) {
            actor.Buff.Playing = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill) {
            actor.Buff.Playing = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private class Activator : MultiRunTask {
            private readonly ActorSkill actor;
            private readonly int countMax = 3;
            private readonly int lifeTime;
            private readonly Map map;
            private readonly SkillArg skill;
            private Actor caster;
            private int count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level) {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                Period = 500;
                DueTime = 0;
                lifeTime = 30000 + 30000 * level;
                countMax = (30000 + 30000 * level) / Period;
            }

            public override void CallBack() {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (count < countMax) {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(actor, 200, false);
                        //取得有效Actor

                        skill.affectedActors.Clear();
                        foreach (var act in actors)
                            if (act.type == ActorType.PC || act.type == ActorType.PET)
                                if (!act.Status.Additions.ContainsKey("PopMusic")) {
                                    var skill2 = new DefaultBuff(skill.skill, act, "PopMusic",
                                        lifeTime - count * Period, 200);
                                    skill2.OnAdditionStart += StartEventHandler;
                                    skill2.OnAdditionEnd += EndEventHandler;
                                    skill2.OnUpdate += TimerEventHandler;
                                    SkillHandler.ApplyAddition(act, skill2);
                                }

                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        count++;
                    }
                    else {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }

            private void StartEventHandler(Actor actor, DefaultBuff skill) {
                int level = skill.skill.Level;
                short[] DEX = { 6, 7, 8, 9, 10 };
                short[] INT = { 5, 6, 7, 9, 10 };
                short[] MAG = { 5, 6, 7, 9, 10 };

                //DEX
                if (skill.Variable.ContainsKey("PopMusic_DEX"))
                    skill.Variable.Remove("PopMusic_DEX");
                skill.Variable.Add("PopMusic_DEX", DEX[level - 1]);
                actor.Status.dex_skill += DEX[level - 1];
                //INT
                if (skill.Variable.ContainsKey("PopMusic_INT"))
                    skill.Variable.Remove("PopMusic_INT");
                skill.Variable.Add("PopMusic_INT", INT[level - 1]);
                actor.Status.int_skill += INT[level - 1];
                //MAG
                if (skill.Variable.ContainsKey("PopMusic_MAG"))
                    skill.Variable.Remove("PopMusic_MAG");
                skill.Variable.Add("PopMusic_MAG", MAG[level - 1]);
                actor.Status.mag_skill += MAG[level - 1];

                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
            }

            private void EndEventHandler(Actor actor, DefaultBuff skill) {
                //DEX
                actor.Status.dex_skill -= (short)skill.Variable["PopMusic_DEX"];
                //INT
                actor.Status.int_skill -= (short)skill.Variable["PopMusic_INT"];
                //MAG
                actor.Status.mag_skill -= (short)skill.Variable["PopMusic_MAG"];

                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
            }

            private void TimerEventHandler(Actor actor, DefaultBuff skill) {
                int ranges = Map.Distance(this.actor, actor);
                if (ranges > 200) skill.AdditionEnd();
            }
        }

        //#endregion
    }
}