using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha
{
    /// <summary>
    ///     水族網（アクアウェーブ）
    /// </summary>
    public class AquaWave : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] speed = { 400, 500, 600, 700 };
            var map = MapManager.Instance.GetMap(sActor.MapID);

            var SX = new byte[3];
            var SY = new byte[3];

            var EX = new byte[3];
            var EY = new byte[3];

            //計算起始座標 1 與 2

            //#region Calc Start and End Pos

            var dir = SkillHandler.Instance.GetDirection(sActor);
            switch (dir)
            {
                case SkillHandler.ActorDirection.East:
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, -1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 0, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 0, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 0, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.North:
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, -1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -1, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, -1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.South:
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 1, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.West:
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, -1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 0, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 0, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 0, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.NorthEast:
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, -1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, 13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.NorthWest:
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, -1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, 13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.SouthEast:
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, -13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, -13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 13, -13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
                case SkillHandler.ActorDirection.SouthWest:
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 1, out SX[0], out SY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 1, 0, out SX[1], out SY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 1, out SX[2], out SY[2]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, -13, SX[0], SY[0], out EX[0], out EY[0]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, -13, SX[1], SY[1], out EX[1], out EY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -13, -13, SX[2], SY[2], out EX[2], out EY[2]);
                    break;
            }

            //#endregion

            //建立海嘯
            for (var i = 0; i < 3; i++)
            {
                //创建设置型技能技能体
                var actor = new ActorSkill(args.skill, sActor);
                //设定技能体位置            
                actor.MapID = sActor.MapID;
                actor.X = SagaLib.Global.PosX8to16(SX[0], map.Width);
                actor.Y = SagaLib.Global.PosY8to16(SY[0], map.Height);
                actor.Speed = speed[level];

                //创建AI类
                var ai = new MobAI(actor, true);
                //寻路
                var path = ai.FindPath(SX[i], SY[i], EX[i], EY[i]);

                if (path.Count >= 2)
                {
                    //根据现有路径推算一步
                    var deltaX = path[path.Count - 1].x - path[path.Count - 2].x;
                    var deltaY = path[path.Count - 1].y - path[path.Count - 2].y;
                    deltaX = path[path.Count - 1].x + deltaX;
                    deltaY = path[path.Count - 1].y + deltaY;
                    var node = new MapNode();
                    node.x = (byte)deltaX;
                    node.y = (byte)deltaY;
                    path.Add(node);
                }

                if (path.Count == 1)
                {
                    //根据现有路径推算一步
                    var deltaX = path[path.Count - 1].x - SagaLib.Global.PosX16to8(SX[i], map.Width);
                    var deltaY = path[path.Count - 1].y - SagaLib.Global.PosY16to8(SY[i], map.Height);
                    deltaX = path[path.Count - 1].x + deltaX;
                    deltaY = path[path.Count - 1].y + deltaY;
                    var node = new MapNode();
                    node.x = (byte)deltaX;
                    node.y = (byte)deltaY;
                    path.Add(node);
                }

                //设定技能体的事件处理器，由于技能体不需要得到消息广播，因此创建个空处理器
                actor.e = new NullEventHandler();
                //在指定地图注册技能体Actor
                map.RegisterActor(actor);
                //设置Actor隐身属性为非
                actor.invisble = false;
                //广播隐身属性改变事件，以便让玩家看到技能体
                map.OnActorVisibilityChange(actor);
                //创建技能效果处理对象

                var timer = new Activator(sActor, actor, args, path);
                timer.Activate();
            }
        }

        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly ActorSkill actor;
            private readonly Actor caster;
            private readonly Elements element;
            private readonly float factor = 1f;
            private readonly Map map;
            private readonly List<MapNode> path;
            private readonly SkillArg skill;
            private int count;
            private bool stop;

            public Activator(Actor caster, ActorSkill actor, SkillArg args, List<MapNode> path)
            {
                this.actor = actor;
                this.caster = caster;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actor.MapID);
                period = 200;
                dueTime = 200;
                this.path = path;
                factor = CalcFactor(skill.skill.Level);
                element = Elements.Water;
            }

            /// <summary>
            ///     计算伤害加成
            /// </summary>
            /// <param name="level">技能等级</param>
            /// <returns>伤害加成</returns>
            private float CalcFactor(byte level)
            {
                float[] factors = { 0f, 1.5f, 1.75f, 2.0f };
                return factors[level];
            }

            public override void CallBack()
            {
                //同步锁，表示之后的代码是线程安全的，也就是，不允许被第二个线程同时访问
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    if (path.Count <= count + 1)
                    {
                        Deactivate();
                        //在指定地图删除技能体（技能效果结束）
                        map.DeleteActor(actor);
                    }
                    else
                    {
                        try
                        {
                            var pos = new short[2];
                            var pos2 = new short[2];
                            pos[0] = SagaLib.Global.PosX8to16(path[count].x, map.Width);
                            pos[1] = SagaLib.Global.PosY8to16(path[count].y, map.Height);
                            pos2[0] = SagaLib.Global.PosX8to16(path[count + 1].x, map.Width);
                            pos2[1] = SagaLib.Global.PosY8to16(path[count + 1].y, map.Height);
                            map.MoveActor(Map.MOVE_TYPE.START, actor, pos, 0, actor.Speed);

                            //取得当前格子内的Actor
                            var list = map.GetActorsArea(actor, 50, false);
                            var affected = new List<Actor>();

                            //筛选有效对象
                            foreach (var i in list)
                                if (SkillHandler.Instance.CheckValidAttackTarget(caster, i))
                                    affected.Add(i);

                            if (map.GetActorsArea(pos2[0], pos2[1], 50).Count != 0 ||
                                map.Info.walkable[path[count + 1].x, path[count + 1].y] != 2)
                            {
                                if (stop)
                                {
                                    Deactivate();
                                    //在指定地图删除技能体（技能效果结束）
                                    map.DeleteActor(actor);
                                    //return 前必须解锁
                                    ClientManager.LeaveCriticalArea();
                                    return;
                                }

                                stop = true;
                            }

                            foreach (var i in affected)
                            {
                                var addition = new Stiff(skill.skill, i, 400);
                                SkillHandler.ApplyAddition(i, addition);
                                map.MoveActor(Map.MOVE_TYPE.START, i, pos2, 500, i.Speed, true);
                                if (i.type == ActorType.MOB || i.type == ActorType.PET || i.type == ActorType.SHADOW)
                                {
                                    var mob = (MobEventHandler)i.e;
                                    mob.AI.OnPathInterupt();
                                }
                            }

                            skill.affectedActors.Clear();
                            SkillHandler.Instance.MagicAttack(caster, affected, skill, element, factor);
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actor, false);
                        }
                        catch
                        {
                        }

                        count++;
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
    }

    //#endregion
}