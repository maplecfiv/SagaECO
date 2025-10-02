using System;
using System.Collections.Generic;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    internal class FireWall : ISkill
    {
        //#region Timer

        private class Activator : MultiRunTask
        {
            private readonly List<ActorSkill> actorSkill = new List<ActorSkill>();
            private readonly Actor caster;
            private readonly int[] count = new int[5];
            private readonly int countMax = 3;
            private readonly bool[] destroyFlag = new bool[5];
            private readonly float factor = 1.0f;
            private readonly Map map;
            private readonly SkillArg skill;
            private readonly DateTime timeMax = DateTime.Now + new TimeSpan(0, 0, 0, 0, 10000);
            private int flag;


            public Activator(Actor caster, SkillArg args, byte level, int flag, List<ActorSkill> actorSkill)
            {
                this.caster = caster;
                this.actorSkill = actorSkill;
                skill = args.Clone();
                map = MapManager.Instance.GetMap(actorSkill[0].MapID);
                period = 100;
                dueTime = 0;
                this.flag = flag;


                switch (level)
                {
                    case 1:
                        factor = 0.25f;
                        countMax = 4;
                        break;
                    case 2:
                        factor = 0.3f;
                        countMax = 5;
                        break;
                    case 3:
                        factor = 0.35f;
                        countMax = 6;
                        break;
                    case 4:
                        factor = 0.4f;
                        countMax = 7;
                        break;
                    case 5:
                        factor = 0.45f;
                        countMax = 8;
                        break;
                }
            }


            public override void CallBack()
            {
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                var pos = new short[2];
                var ii = 0;
                var affected = new List<Actor>();
                var affectedCheck = new List<Actor>();
                try
                {
                    if (DateTime.Now <= timeMax)
                    {
                        foreach (var i in actorSkill)
                        {
                            affectedCheck.Clear();
                            skill.affectedActors.Clear();
                            if (count[ii] <= countMax - 1)
                            {
                                affected = map.GetActorsArea(i, 50, false);
                                foreach (var j in affected)
                                {
                                    if (!SkillHandler.Instance.CheckValidAttackTarget(caster, i)) continue;
                                    if (!affectedCheck.Contains(j))
                                    {
                                        affectedCheck.Add(j);
                                        count[ii] = count[ii] + 1;
                                        if (j.LastX != 0 && j.LastY != 0)
                                        {
                                            pos[0] = j.LastX;
                                            pos[1] = j.LastY;
                                            map.MoveActor(Map.MOVE_TYPE.START, j, pos, 500, 500);
                                            pos[0] = (short)(2 * j.X - j.LastX);
                                            pos[1] = (short)(2 * j.Y - j.LastY);
                                            map.MoveActor(Map.MOVE_TYPE.START, j, pos, 500, 500);
                                        }
                                    }
                                }

                                SkillHandler.Instance.MagicAttack(caster, affectedCheck, skill, Elements.Fire, factor);
                                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, actorSkill[0],
                                    false);
                            }
                            else
                            {
                                if (destroyFlag[ii] == false)
                                    map.DeleteActor(i);
                                destroyFlag[ii] = true;
                            }

                            ii += 1;
                        }
                    }
                    else
                    {
                        Deactivate();
                        foreach (Actor i in actorSkill)
                            map.DeleteActor(i);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
                //测试去除技能同步锁ClientManager.LeaveCriticalArea();
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
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actorSkill = new List<ActorSkill>();
            var deltaX = 0;
            var deltaY = 0;
            var argX = (int)args.x;
            var argY = (int)args.y;
            var castorX = (int)SagaLib.Global.PosX16to8(sActor.X, map.Width);
            var castorY = (int)SagaLib.Global.PosY16to8(sActor.Y, map.Height);
            deltaX = argX - castorX;
            deltaY = argY - castorY;
            var pos = new Dictionary<int, int[]>();
            var n = 0;
            var flag = 0;
            for (var i = 0; i <= 4; i++)
            {
                var actorT = new ActorSkill(args.skill, sActor);
                actorSkill.Add(actorT);
                actorSkill[i].e = new NullEventHandler();
                actorSkill[i].MapID = sActor.MapID;
                var posT = new int[2];
                posT[0] = 0;
                posT[1] = 0;
                pos.Add(i, posT);
            }

            if (deltaX != 0 || deltaY != 0)
            {
                if (Math.Abs(deltaX) != Math.Abs(deltaY))
                {
                    actorSkill.Remove(actorSkill[actorSkill.Count - 1]);
                    actorSkill.Remove(actorSkill[actorSkill.Count - 1]);
                    if (Math.Abs(deltaY) > Math.Abs(deltaX))
                    {
                        pos[0][0] = argX;
                        pos[0][1] = argY;
                        pos[1][0] = argX + 1;
                        pos[1][1] = argY;
                        pos[2][0] = argX - 1;
                        pos[2][1] = argY;
                        if (deltaY > 0)
                            flag = 1; //Mark which kind of area was the wall setted.
                        else
                            flag = 2;
                    }
                    else
                    {
                        pos[0][0] = argX;
                        pos[0][1] = argY;
                        pos[1][0] = argX;
                        pos[1][1] = argY + 1;
                        pos[2][0] = argX;
                        pos[2][1] = argY - 1;
                        if (deltaX > 0)
                            flag = 3;
                        else
                            flag = 4;
                    }
                }
                else
                {
                    if (deltaX * deltaY < 0)
                    {
                        pos[0][0] = argX;
                        pos[0][1] = argY;
                        pos[1][0] = argX + 1;
                        pos[1][1] = argY + 1;
                        pos[2][0] = argX - 1;
                        pos[2][1] = argY - 1;
                        if (argX > 0)
                        {
                            pos[3][0] = argX;
                            pos[3][1] = argY + 1;
                            pos[4][0] = argX - 1;
                            pos[4][1] = argY;
                            flag = 5;
                        }
                        else
                        {
                            pos[3][0] = argX;
                            pos[3][1] = argY - 1;
                            pos[4][0] = argX + 1;
                            pos[4][1] = argY;
                            flag = 6;
                        }
                    }
                    else
                    {
                        pos[0][0] = argX;
                        pos[0][1] = argY;
                        pos[1][0] = argX + 1;
                        pos[1][1] = argY - 1;
                        pos[2][0] = argX - 1;
                        pos[2][1] = argY + 1;
                        if (argX > 0)
                        {
                            pos[3][0] = argX;
                            pos[3][1] = argY - 1;
                            pos[4][0] = argX - 1;
                            pos[4][1] = argY;
                            flag = 7;
                        }
                        else
                        {
                            pos[3][0] = argX;
                            pos[3][1] = argY + 1;
                            pos[4][0] = argX + 1;
                            pos[4][1] = argY;
                            flag = 8;
                        }
                    }
                }
            }
            else
            {
                pos[0][0] = argX;
                pos[0][1] = argY;
                pos[1][0] = argX;
                pos[1][1] = argY + 1;
                pos[2][0] = argX;
                pos[2][1] = argY - 1;
                flag = 9;
            }

            foreach (var i in actorSkill)
            {
                if (pos[n][0] != 0 && pos[n][1] != 0)
                {
                    i.X = SagaLib.Global.PosX8to16((byte)pos[n][0], map.Width);
                    i.Y = SagaLib.Global.PosY8to16((byte)pos[n][1], map.Height);
                    map.RegisterActor(i);
                    i.invisble = false;
                    map.OnActorVisibilityChange(i);
                }

                n = n + 1;
            }

            var timer = new Activator(sActor, args, level, flag, actorSkill);
            timer.Activate();
        }

        //#endregion
    }
}