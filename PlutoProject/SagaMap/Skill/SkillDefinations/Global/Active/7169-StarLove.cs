using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Global.Active
{
    /// <summary>
    ///     フォートレスサークル
    /// </summary>
    public class StarLove : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly Actor dActor;
            private readonly SkillArg skill;
            private readonly Map map;
            private byte skilllevel;
            private readonly float factor = 24.0f;
            private readonly int countMax = 1;
            private int count, lifetime;

            public Activator(Actor caster, ActorSkill actor, Actor dActor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                this.dActor = dActor;
                dueTime = 650;
                skill = args.Clone();
                skilllevel = level;
                map = MapManager.Instance.GetMap(actor.MapID);
                lifetime = 500; //持续时间
                //factor = factors[level] + caster.Status.Cardinal_Rank;
                var pc = caster as ActorPC;

                period = 500;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //ClientManager.EnterCriticalArea();
                try
                {
                    if (count < countMax)
                    {
                        var map = MapManager.Instance.GetMap(caster.MapID);
                        var actors = MapManager.Instance.GetMap(caster.MapID).GetActorsArea(dActor, 200, true);
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                            {
                                var damage1 = SkillHandler.Instance.CalcDamage(true, caster, i, skill,
                                    SkillHandler.DefType.Def, Elements.Neutral, 0, factor);
                                var damage2 = SkillHandler.Instance.CalcDamage(false, caster, i, skill,
                                    SkillHandler.DefType.Def, Elements.Neutral, 0, factor);
                                var enddamage = damage1 + damage2;
                                SkillHandler.Instance.FixAttack(caster, i, skill, Elements.Neutral, enddamage);
                                SkillHandler.Instance.ShowVessel(i, enddamage);
                            }

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
                //ClientManager.LeaveCriticalArea();
            }

            //#endregion
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建设置型技能技能体
            var actor = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //设定技能体位置
            actor.MapID = dActor.MapID;
            //actor.X = SagaLib.Global.PosX8to16((byte)sActor.X, map.Width);
            //actor.Y = SagaLib.Global.PosY8to16((byte)sActor.Y, map.Height);
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
            var timer = new Activator(sActor, actor, dActor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}