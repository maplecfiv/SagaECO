using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha {
    /// <summary>
    ///     解放4代元素（エレメンタルボール）
    /// </summary>
    public class ElementBall : ISkill {
        //#region Timer

        private class Activator : MultiRunTask {
            private readonly ActorSkill actor;
            private readonly Elements ElementAttack = Elements.Earth;
            private readonly float factor;
            private readonly Map map;
            private readonly Actor sActor;
            private readonly SkillArg skill;
            private readonly int times;
            private int count;

            public Activator(Actor _sActor, ActorSkill _dActor, SkillArg _args, byte level) {
                sActor = _sActor;
                actor = _dActor;
                skill = _args.Clone();
                factor = 1.0f + 0.2f * level;
                DueTime = 0;
                times = 8;
                Period = 7000 / times;
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            private Elements getElement() //土→火→水→風
            {
                switch (ElementAttack) {
                    case Elements.Earth:
                        return Elements.Fire;
                    case Elements.Fire:
                        return Elements.Water;
                    case Elements.Water:
                        return Elements.Wind;
                    case Elements.Wind:
                        return Elements.Earth;
                }

                return Elements.Neutral;
            }

            public override void CallBack() {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try {
                    if (count < times) {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(actor, 300, false);
                        var affected = new List<Actor>();
                        //取得有效Actor（即怪物）

                        //施加魔法伤害
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                                affected.Add(i);

                        SkillHandler.Instance.MagicAttack(sActor, affected, skill, getElement(), factor);
                        //广播技能效果
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
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
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
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
            //创建技能效果处理对象
            var timer = new Activator(sActor, actor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}