using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Gladiator
{
    /// <summary>
    ///     プレッシャー
    /// </summary>
    public class Pressure : ISkill
    {
        #region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly int countMax;
            private readonly Actor dActor;
            private readonly float factor = 0;
            private readonly Map map;
            private readonly SkillArg skill;
            private int count;

            public Activator(Actor caster, Actor theDActor, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                period = 1000;
                dueTime = 1000;
                int[] Counts = { 0, 30, 30, 30, 40, 45 };
                countMax = Counts[level];
                dActor = theDActor;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        //取得设置型技能，技能体周围7x7范围的怪（范围300，300代表3格，以自己为中心的3格范围就是7x7）
                        var actors = map.GetActorsArea(dActor, 200, true);
                        var affected = new List<Actor>();
                        //取得有效Actor（即怪物）

                        //施加魔法伤害
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                            {
                                var skill2 = new DefaultBuff(skill.skill, i, "Pressure", 5000);
                                skill2.OnAdditionStart += StartEventHandler;
                                skill2.OnAdditionEnd += EndEventHandler;
                                SkillHandler.ApplyAddition(i, skill2);
                            }

                        SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Neutral, factor);

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
                //解开同步锁
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
            }

            private void StartEventHandler(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;
                float[] value = { 0, 0.03f, 0.06f, 0.09f, 0.12f, 0.15f };
                float[] speedless = { 0, 180, 210, 240, 270, 300 };
                //降ASPD
                if (skill.Variable.ContainsKey("PRESSURE_ASPD"))
                    skill.Variable.Remove("PRESSURE_ASPD");
                skill.Variable.Add("PRESSURE_ASPD", (short)(actor.Status.aspd * value[level]));
                actor.Status.aspd_skill -= (short)(actor.Status.aspd * value[level]);
                //降CSPD
                if (skill.Variable.ContainsKey("PRESSURE_CSPD"))
                    skill.Variable.Remove("PRESSURE_CSPD");
                skill.Variable.Add("PRESSURE_CSPD", (short)(actor.Status.cspd * value[level]));
                actor.Status.cspd_skill -= (short)(actor.Status.cspd * value[level]);
                //降SPEED
                if (skill.Variable.ContainsKey("PRESSURE_SPEED"))
                    skill.Variable.Remove("PRESSURE_SPEED");
                skill.Variable.Add("PRESSURE_SPEED", (short)speedless[level]);
                actor.Status.speed_skill -= (short)speedless[level];
                actor.Buff.SpeedDown = true;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void EndEventHandler(Actor actor, DefaultBuff skill)
            {
                actor.Status.aspd_skill += (short)skill.Variable["PRESSURE_ASPD"];
                actor.Status.cspd_skill += (short)skill.Variable["PRESSURE_CSPD"];
                actor.Status.speed_skill += (short)skill.Variable["PRESSURE_SPEED"];
                actor.Buff.SpeedDown = false;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
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
            var timer = new Activator(sActor, dActor, actor, args, level);
            timer.Activate();
        }

        #endregion
    }
}