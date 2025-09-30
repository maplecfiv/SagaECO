using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill;
using SagaMap.Skill.Additions;

namespace SagaMap.Mob.AICommands
{
    public class Move : AICommand
    {
        private readonly MobAI mob;
        private readonly short x;
        private readonly short y;
        public DateTime BackTimer = DateTime.Now;
        private int index;

        private List<MapNode> path;

        public Move(MobAI mob, short x, short y)
        {
            this.mob = mob;
            this.mob.map.FindFreeCoord(x, y, out x, out y);
            this.x = x;
            this.y = y;
            if (mob.Mode.NoMove)
            {
                Status = CommandStatus.FINISHED;
            }
            else
            {
                path = mob.FindPath(Global.PosX16to8(mob.Mob.X, mob.map.Width),
                    Global.PosY16to8(mob.Mob.Y, mob.map.Height), Global.PosX16to8(x, mob.map.Width),
                    Global.PosY16to8(y, mob.map.Height));
                Status = CommandStatus.INIT;
            }
        }

        public string GetName()
        {
            return "Move";
        }

        public void Update(object para)
        {
            try
            {
                MapNode node;
                if (Status == CommandStatus.FINISHED)
                    return;
                if (mob.Cannotmovebeforefight) return;
                if (mob.CannotAttack > DateTime.Now && mob.Mode.isAnAI)
                    return;
                if (mob.Mob.Status.Additions.ContainsKey("石像坐下休息"))
                    return;
                if (path.Count == 0 || !mob.CanMove)
                {
                    Status = CommandStatus.FINISHED;
                    return;
                }

                if (DateTime.Now > mob.BackTimer.AddSeconds(4))
                    mob.BackTimer = DateTime.Now;
                if (DateTime.Now > mob.BackTimer.AddSeconds(2) && !mob.noreturn) //2秒后返回
                    if (mob.Mob.HP < mob.Mob.MaxHP)
                        returnAndInitialize();
                if (mob.Mob.type == ActorType.GOLEM)
                    if (!mob.Mode.RunAway && mob.Mob.Status.Additions.ContainsKey("石像击杀怪物CD") &&
                        Global.Random.Next(0, 100) < 20 && !mob.Mob.Status.Additions.ContainsKey("石像坐下休息") &&
                        !mob.Mob.Status.Additions.ContainsKey("石像坐下休息CD"))
                    {
                        var lefttime = Global.Random.Next(10000, 185000);
                        ((OtherAddition)mob.Mob.Status.Additions["石像击杀怪物CD"]).endTime =
                            DateTime.Now + new TimeSpan(0, 0, 0, 0, lefttime);
                        var skills = new OtherAddition(null, mob.Mob, "石像坐下休息", lefttime);
                        skills.dueTime = 2000;
                        skills.OnAdditionStart += (s, e) =>
                        {
                            ((ActorGolem)mob.Mob).Motion = 135;
                            ((ActorGolem)mob.Mob).MotionLoop = true;
                            var parg = new ChatArg();
                            parg.motion = MotionType.SIT;
                            parg.loop = 1;
                            mob.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, mob.Mob, true);
                        };
                        skills.OnAdditionEnd += (s, e) =>
                        {
                            ((ActorGolem)mob.Mob).MotionLoop = false;
                            SkillHandler.RemoveAddition(mob.Mob, "石像击杀怪物CD");
                            var skills2 = new OtherAddition(null, mob.Mob, "石像坐下休息CD", 300000);
                            SkillHandler.ApplyAddition(mob.Mob, skills2);
                        };
                        SkillHandler.ApplyAddition(mob.Mob, skills);
                        return;
                    }

                if (index + 1 < path.Count)
                {
                    node = path[index];
                    var dst = new short[2]
                        { Global.PosX8to16(node.x, mob.map.Width), Global.PosY8to16(node.y, mob.map.Height) };
                    mob.map.MoveActor(Map.MOVE_TYPE.START, mob.Mob, dst,
                        MobAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)), 0, true);
                }
                else
                {
                    node = path[path.Count - 1];
                    var dst = new short[2]
                        { Global.PosX8to16(node.x, mob.map.Width), Global.PosY8to16(node.y, mob.map.Height) };
                    mob.map.MoveActor(Map.MOVE_TYPE.START, mob.Mob, dst,
                        MobAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)), (ushort)(mob.Mob.Speed / 10), true);
                    Status = CommandStatus.FINISHED;
                }

                index++;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex, null);
            }
        }

        public CommandStatus Status { get; set; }

        public void Dispose()
        {
            Status = CommandStatus.FINISHED;
        }

        private void returnAndInitialize()
        {
            if (mob.Mob.type == ActorType.GOLEM) return;
            var pos = new short[2] { mob.X_pb, mob.Y_pb };
            mob.map.MoveActor(Map.MOVE_TYPE.START, mob.Mob, pos, 1, 1000, false, MoveType.WARP2);
            mob.Mob.HP = mob.Mob.MaxHP;

            if (mob.Mob.type == ActorType.MOB)
            {
                if (((ActorMob)mob.Mob).AttackedForEvent != 0)
                    mob.Mob.e.OnActorReturning(mob.Mob);
                ((ActorMob)mob.Mob).AttackedForEvent = 0;
                mob.Mob.BattleStartTime = DateTime.Now;
            }

            //清空HP相关SKILL状态
            mob.SkillOfHPClear();
            mob.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, mob.Mob, false);
        }

        public void FindPath()
        {
            path = mob.FindPath(Global.PosX16to8(mob.Mob.X, mob.map.Width), Global.PosY16to8(mob.Mob.Y, mob.map.Height),
                Global.PosX16to8(x, mob.map.Width), Global.PosY16to8(y, mob.map.Height));
            index = 0;
        }
    }
}