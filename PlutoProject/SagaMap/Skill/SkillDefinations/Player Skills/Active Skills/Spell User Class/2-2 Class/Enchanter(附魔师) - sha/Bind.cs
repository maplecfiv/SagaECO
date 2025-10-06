using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Enchanter_附魔师____sha
{
    /// <summary>
    ///     大地束縛（バインド）
    /// </summary>
    public class Bind : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly Actor actor;
            private readonly byte level;
            private readonly Map map;
            private readonly SkillArg skill;
            private int lifetime;

            public Activator(Actor _actor, SkillArg _args, byte _level)
            {
                level = _level;
                actor = _actor;
                skill = _args;
                DueTime = 0;
                Period = 1000;
                map = MapManager.Instance.GetMap(actor.MapID);
                lifetime = 5000 + 5000 * level;
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (lifetime > 0)
                    {
                        lifetime -= Period;
                        try
                        {
                            var actors = map.GetActorsArea(actor, 300, false);
                            //取得有效Actor（即怪物）
                            var rate = 5 + 5 * level;

                            foreach (var i in actors)
                                if (SkillHandler.Instance.CheckValidAttackTarget(actor, i))
                                    if (SkillHandler.Instance.CanAdditionApply(actor, i,
                                            SkillHandler.DefaultAdditions.鈍足, rate))
                                    {
                                        var skill2 = new MoveSpeedDown(skill.skill, i, lifetime);
                                        SkillHandler.ApplyAddition(i, skill2);
                                    }

                            //广播技能效果
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        }
                        catch (Exception ex)
                        {
                            Logger.ShowError(ex);
                        }
                    }
                    else
                    {
                        Deactivate();
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
        }

        //#endregion

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 100))
                return -17;

            if (args.x >= map.Width || args.y >= map.Height)
                return -6;
            if (map.Info.earth[args.x, args.y] > 0)
                return 0;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.Info.earth[args.x, args.y] == 0)
            {
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
            //创建技能效果处理对象
            var timer = new Activator(actor, args, level);
            timer.Activate();
        }

        //#endregion
    }
}