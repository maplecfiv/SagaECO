using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.Traveler
{
    public class ThunderFall : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly float factor = 1.0f;
            private readonly Map map;
            private readonly List<Point> path;
            private readonly SkillArg skill;
            private int count;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, byte level)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                skill.skill = SkillFactory.Instance.GetSkill(23003, 1);
                map = MapManager.Instance.GetMap(actor.MapID);
                Period = 200;
                DueTime = 0;
                path = GetStraightPath(Global.PosX16to8(caster.X, map.Width),
                    Global.PosY16to8(caster.Y, map.Height), args.x, args.y);
            }

            public List<Point> GetStraightPath(byte fromx, byte fromy, byte tox, byte toy)
            {
                var path = new List<Point>();
                if (fromx == tox && fromy == toy)
                    return path;
                double k; // 
                double nowx = fromx;
                double nowy = fromy;
                int x = fromx;
                int y = fromy;
                sbyte addx, addy;
                if (Math.Abs(toy - fromy) <= Math.Abs(tox - fromx))
                {
                    if (tox == fromx)
                    {
                        if (fromy < toy)
                            for (var i = fromy + 1; i <= toy; i++)
                            {
                                var t = new Point();
                                t.x = fromx;
                                t.y = (byte)i;
                                path.Add(t);
                            }
                        else
                            for (var i = fromy - 1; i <= toy; i--)
                            {
                                var t = new Point();
                                t.x = fromx;
                                t.y = (byte)i;
                                path.Add(t);
                            }
                    }
                    else
                    {
                        k = Math.Abs((double)(toy - fromy) / (tox - fromx));
                        if (toy < fromy)
                            addy = -1;
                        else
                            addy = 1;
                        if (tox < fromx)
                            addx = -1;
                        else
                            addx = 1;
                        while (Math.Round(nowx) != tox)
                        {
                            x += addx;
                            if (Math.Round(nowy) != Math.Round(nowy + k * addy))
                                y += addy;
                            nowx += addx;
                            nowy += k * addy;

                            var t = new Point();
                            t.x = (byte)x;
                            t.y = (byte)y;
                            path.Add(t);
                        }
                    }
                }
                else
                {
                    if (toy == fromy)
                    {
                        if (fromx < tox)
                            for (var i = fromx + 1; i <= tox; i++)
                            {
                                var t = new Point();
                                t.x = (byte)i;
                                t.y = fromy;
                                path.Add(t);
                            }
                        else
                            for (var i = fromx - 1; i <= tox; i--)
                            {
                                var t = new Point();
                                t.x = (byte)i;
                                t.y = fromy;
                                path.Add(t);
                            }
                    }
                    else
                    {
                        k = Math.Abs((double)(tox - fromx) / (toy - fromy));
                        if (toy < fromy)
                            addy = -1;
                        else
                            addy = 1;
                        if (tox < fromx)
                            addx = -1;
                        else
                            addx = 1;
                        while (Math.Round(nowy) != toy)
                        {
                            y += addy;
                            if (Math.Round(nowx) != Math.Round(nowx + k * addx))
                                x += addx;
                            nowy += addy;
                            nowx += k * addx;

                            var t = new Point();
                            t.x = (byte)x;
                            t.y = (byte)y;
                            path.Add(t);
                        }
                    }
                }

                return path;
            }


            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问ClientManager.EnterCriticalArea();
                try
                {
                    if (count < path.Count)
                    {
                        skill.x = path[count].x;
                        skill.y = path[count].y;

                        var actors = map.GetRoundAreaActors(Global.PosX8to16(skill.x, map.Width),
                            Global.PosY8to16(skill.y, map.Height), 150);
                        var affected = new List<Actor>();
                        skill.affectedActors.Clear();
                        foreach (var i in actors)
                            if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                                affected.Add(i);

                        SkillHandler.Instance.MagicAttack(caster, affected, skill, Elements.Wind, factor);

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
                    Logger.getLogger().Error(ex, ex.Message);
                }
                //解开同步锁ClientManager.LeaveCriticalArea();
            }

            public class Point
            {
                public byte x, y;
            }
        }

        //#endregion

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
            actor.MapID = sActor.MapID;
            actor.X = Global.PosX8to16(args.x, map.Width);
            actor.Y = Global.PosY8to16(args.y, map.Height);
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
            /*
            uint NextSkillID = 22000;
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, 1, 0));
            */
        }

        //#endregion
    }
}