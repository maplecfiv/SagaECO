using System;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Skill;

namespace SagaMap.Mob.AICommands.SpecificAI
{
    public class　CCAttack : AICommand
    {
        private readonly MobAI mob;
        public bool active;
        private bool attacking;
        private MobAttack attacktask;
        private int counter;
        private Actor dest;
        private short x, y;

        public CCAttack(MobAI mob)
        {
            this.mob = mob;
            Status = CommandStatus.INIT;
            var aspd = 0;
            if (this.mob.Mob.type == ActorType.MOB)
            {
                var tar = (ActorMob)this.mob.Mob;
                aspd = tar.BaseData.aspd;
            }

            if (this.mob.Mob.type == ActorType.PET)
            {
                var pet = (ActorPet)this.mob.Mob;
                aspd = pet.BaseData.aspd;
            }

            if (this.mob.Mob.type == ActorType.SHADOW || this.mob.Mob.type == ActorType.GOLEM ||
                this.mob.Mob.type == ActorType.PC)
                aspd = this.mob.Mob.Status.aspd;

            aspd += this.mob.Mob.Status.aspd_skill;

            if (aspd > 960)
                aspd = 960;

            attacktask = new MobAttack(mob, dest);
            x = mob.Mob.X;
            y = mob.Mob.Y;
        }

        public string GetName()
        {
            return "CCAttack";
        }

        public void Update(object para)
        {
            ActorPet pet = null;
            if (mob.Mob.type == ActorType.PET)
                pet = (ActorPet)mob.Mob;
            if (mob.Hate.Count == 0)
                if (!hasPlayerInSight())
                {
                    counter++;
                    if (counter > 2333)
                    {
                        mob.Pause();
                        counter = 0;
                        return;
                    }
                }

            if (pet != null)
                if (!mob.Hate.ContainsKey(pet.Owner.ActorID))
                    mob.Hate.Add(pet.Owner.ActorID, 1);
            if (mob.Master != null)
                if (!mob.Hate.ContainsKey(mob.Master.ActorID))
                    mob.Hate.Add(mob.Master.ActorID, 1);
            if (mob.Mob.Tasks.ContainsKey("AutoCast"))
            {
                if (attacktask.Activated) attacktask.Deactivate();
                attacking = false;
                return;
            }

            //施放主人战斗中技能，放在这个位置保证平时状态
            if ((DateTime.Now - mob.LastSkillCast).TotalSeconds >= 1)
            {
                if (mob.Master != null)
                    if (mob.Master.type == ActorType.PC)
                    {
                        var pc = (ActorPC)mob.Master;
                        if (pc.BattleStatus == 1)
                            if (Global.Random.Next(0, 99) < mob.Mode.EventMasterCombatSkillRate)
                            {
                                mob.OnShouldCastSkill(mob.Mode.EventMasterCombat, pc);
                                mob.LastSkillCast = DateTime.Now;
                            }
                    }

                if (pet != null)
                {
                    var pc = pet.Owner;
                    if (pc.BattleStatus == 1)
                        if (Global.Random.Next(0, 99) < mob.Mode.EventMasterCombatSkillRate)
                        {
                            mob.OnShouldCastSkill(mob.Mode.EventMasterCombat, pc);
                            mob.LastSkillCast = DateTime.Now;
                        }
                }
            }

            dest = CurrentTarget();
            if ((mob.Mode.Active || mob.Mob.Buff.Zombie) && (dest == null || dest == mob.Master)) CheckAggro();
            if (dest == null)
            {
                mob.AIActivity = Activity.IDLE;
                if (mob.commands.ContainsKey("Chase")) mob.commands.Remove("Chase");
                ;
                return;
            }

            mob.AIActivity = Activity.BUSY;
            if (mob.commands.ContainsKey("Move")) mob.commands.Remove("Move");
            attacktask.dActor = dest;

            //施放技能，放在这个位置保证追踪模式下的技能优先
            if ((DateTime.Now - mob.LastSkillCast).TotalSeconds >= 2)
                if (Global.Random.Next(0, 99) < mob.Mode.EventAttackingSkillRate)
                {
                    mob.OnShouldCastSkill(mob.Mode.EventAttacking, dest);
                    mob.LastSkillCast = DateTime.Now;
                }

            if (mob.commands.ContainsKey("Chase"))
            {
                if (attacktask.Activated) attacktask.Deactivate();
                attacking = false;
                return;
            }

            if (this.x != mob.Mob.X || this.y != mob.Mob.Y)
            {
                short x, y;
                mob.map.FindFreeCoord(mob.Mob.X, mob.Mob.Y, out x, out y, mob.Mob);
                var skip = false;
                if (mob.Mob.type == ActorType.PET)
                    if (((ActorPet)mob.Mob).BaseData.mobType == MobType.MAGIC_CREATURE)
                        skip = true;
                if ((mob.Mob.X == x && mob.Mob.Y == y) || mob.Mode.RunAway || skip)
                {
                }
                else
                {
                    var dst = new short[2] { x, y };

                    mob.map.MoveActor(Map.MOVE_TYPE.START, mob.Mob, dst,
                        MobAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)), (ushort)(mob.Mob.Speed / 20));
                    return;
                }

                this.x = mob.Mob.X;
                this.y = mob.Mob.Y;
            }

            if (dest.HP == 0)
            {
                if (pet != null)
                {
                    if (dest.ActorID != pet.Owner.ActorID)
                    {
                        if (mob.Hate.ContainsKey(dest.ActorID)) mob.Hate.Remove(dest.ActorID);
                        if (attacktask.Activated) attacktask.Deactivate();
                        attacktask = null;
                        return;
                    }
                }
                else
                {
                    if (mob.Hate.ContainsKey(dest.ActorID)) mob.Hate.Remove(dest.ActorID);
                    if (attacktask.Activated) attacktask.Deactivate();
                    attacktask = null;
                    return;
                }
            }

            /*ActorEventHandlers.PC_EventHandler eh = (SagaMap.ActorEventHandlers.PC_EventHandler)dest.e;
            if (eh.C.state == MapClient.SESSION_STATE.LOGGEDOFF)
            {
                if (mob.Hate.ContainsKey(dest.id)) mob.Hate.Remove(dest.id);
                if (attacktask.Activated() == true) attacktask.Deactivate();
                attacktask = null;
                return;
            }*/
            float size;
            if (mob.Mob.type != ActorType.PC)
            {
                size = ((ActorMob)mob.Mob).BaseData.range;
                if (((ActorMob)mob.Mob).range != 0)
                    size = ((ActorMob)mob.Mob).range;
            }
            else
            {
                size = 1;
            }

            var ifChase = false;
            if (mob.Mob.type == ActorType.PET)
                if (dest.type == ActorType.PC)
                    if (((ActorPet)mob.Mob).BaseData.mobType == MobType.MAGIC_CREATURE &&
                        MobAI.GetLengthD(mob.Mob.X, mob.Mob.Y, dest.X, dest.Y) > 0)
                        ifChase = true;

            if (MobAI.GetLengthD(mob.Mob.X, mob.Mob.Y, dest.X, dest.Y) > size * 150 || ifChase)
            {
                if (!mob.Mode.RunAway || MobAI.GetLengthD(mob.Mob.X, mob.Mob.Y, dest.X, dest.Y) < 2000)
                {
                    var chase = new Chase(mob, dest);
                    mob.commands.Add("Chase", chase);
                    if (attacktask.Activated) attacktask.Deactivate();
                    attacking = false;
                }
            }
            else
            {
                if (!mob.Mode.RunAway || Global.Random.Next(0, 99) < 70)
                {
                    if (mob.CanAttack)
                    {
                        if (pet != null)
                            if (dest.ActorID == pet.Owner.ActorID)
                                return;
                        if (attacktask.Activated == false) attacktask.Activate();
                        attacking = true;
                    }
                }
                else
                {
                    var chase = new Chase(mob, dest);
                    mob.commands.Add("Chase", chase);
                    if (attacktask.Activated) attacktask.Deactivate();
                    attacking = false;
                }
            }
        }

        public CommandStatus Status { get; set; }

        public void Dispose()
        {
            if (dest == null) return;
            if (attacking && attacktask != null) attacktask.Deactivate();
            attacktask = null;
            Status = CommandStatus.FINISHED;
        }

        private Actor CurrentTarget()
        {
            uint id = 0;
            uint hate = 0;
            Actor tmp = null;
            var ids = new uint[mob.Hate.Keys.Count];
            mob.Hate.Keys.CopyTo(ids, 0);
            for (uint i = 0; i < mob.Hate.Keys.Count; i++) //Find out the actorPC with the highest hate value
            {
                if (ids[i] == 0) continue;
                if (ids[i] == mob.Mob.ActorID)
                    continue;
                if (mob.Master != null)
                    if (ids[i] == mob.Master.ActorID && mob.Hate.Count > 1)
                        continue;
                if (hate < mob.Hate[ids[i]])
                {
                    hate = mob.Hate[ids[i]];
                    id = ids[i];
                    tmp = mob.map.GetActor(id);
                    if (tmp == null)
                    {
                        mob.Hate.Remove(id);
                        id = 0;
                        hate = 0;
                        active = false;
                        i = 0;
                        continue;
                    }

                    if (tmp.Status.Additions.ContainsKey("Hiding"))
                    {
                        mob.Hate.Remove(id);
                        continue;
                    }

                    if (tmp.Status.Additions.ContainsKey("IAmTree"))
                    {
                        mob.Hate.Remove(id);
                        continue;
                    }

                    active = true;

                    if (tmp.type == ActorType.PC && mob.Mob.type != ActorType.PET)
                        if (((ActorPC)tmp).PossessionTarget != 0)
                        {
                            mob.Hate.Remove(id);
                            id = 0;
                            hate = 0;
                            active = false;
                            i = 0;
                        }
                }
            }

            if (id != 0) //Now the id is refer to the PC with the highest hate to the Mob.现在这个ID是怪物对最高仇恨者的ID
            {
                tmp = mob.map.GetActor(id);
                if (tmp != null)
                    if (tmp.HP == 0)
                    {
                        mob.Hate.Remove(tmp.ActorID);
                        id = 0;
                        active = false;
                    }
            }

            if (id == 0)
            {
                active = false;
                return null;
            }

            if (dest != null)
                if (dest.ActorID != id)
                    if (attacktask.Activated)
                        attacktask.Deactivate();
            return tmp;
        }

        private void CheckAggro()
        {
            var distance = double.MaxValue;
            Actor target = null;
            var isSlavaOfPc = false;
            if (mob.Master != null)
            {
                if (!mob.Hate.ContainsKey(mob.Master.ActorID))
                    mob.Hate.Add(mob.Master.ActorID, 1);
                if (mob.Master.type == ActorType.PC)
                    isSlavaOfPc = true;
            }

            foreach (var id in mob.Mob.VisibleActors)
            {
                var i = mob.map.GetActor(id);
                if (i == null)
                    continue;
                if (i.Buff.Transparent)
                    continue;
                if (i.MapID != mob.map.ID)
                    continue;
                if (i.Status.Additions.ContainsKey("IAmTree"))
                    continue;
                if (i.HP == 0)
                    continue;
                if (mob.Mob.type != ActorType.PC && i.type == ActorType.MOB)
                {
                    var eh = (MobEventHandler)i.e;
                    if (eh.AI.Mode.Symbol && !isSlavaOfPc) mob.Hate.Add(i.ActorID, 20);
                    //SendAggroEffect();
                }

                if (mob.Mob.type != ActorType.PC && !mob.Mob.Buff.Zombie && i.type != ActorType.PC &&
                    i.type != ActorType.PET && i.type != ActorType.SHADOW && !(i.type == ActorType.MOB && isSlavaOfPc))
                    continue;
                if (mob.Mob.type == ActorType.PC)
                    if (!SkillHandler.Instance.CheckValidAttackTarget(mob.Mob, i))
                        continue;
                if (isSlavaOfPc && i.type == ActorType.SHADOW)
                    continue;
                if (isSlavaOfPc && i.type == ActorType.PET)
                    continue;
                if (isSlavaOfPc && i.type == ActorType.PC)
                    continue;
                if (mob.Mob.type != ActorType.PC && i.type == ActorType.PC)
                    if (((ActorPC)i).PossessionTarget != 0)
                        continue;
                var len = MobAI.GetLengthD(i.X, i.Y, mob.Mob.X, mob.Mob.Y);
                if (len < distance)
                {
                    byte x, y, x2, y2;
                    x = Global.PosX16to8(mob.Mob.X, mob.map.Width);
                    y = Global.PosY16to8(mob.Mob.Y, mob.map.Height);
                    x2 = Global.PosX16to8(i.X, mob.map.Width);
                    y2 = Global.PosY16to8(i.Y, mob.map.Height);

                    var path = mob.FindPath(x, y, x2, y2);
                    if (path[path.Count - 1].x == x2 && path[path.Count - 1].y == y2)
                    {
                        if (i.type == ActorType.SHADOW && target != i)
                        {
                            distance = 0;
                            target = i;
                        }
                        else
                        {
                            distance = len;
                            target = i;
                        }
                    }
                }
            }

            if (distance <= 1000)
            {
                mob.Hate.Add(target.ActorID, 20);
                SendAggroEffect();
            }
        }

        private void SendAggroEffect()
        {
            var arg = new EffectArg();
            arg.actorID = mob.Mob.ActorID;
            arg.effectID = 4539;
            mob.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, mob.Mob, true);
        }

        private bool hasPlayerInSight()
        {
            if (mob.Mob.type == ActorType.PC)
                return true;
            foreach (var id in mob.Mob.VisibleActors)
            {
                var i = mob.map.GetActor(id);
                if (i == null)
                    continue;
                if (i.MapID != mob.map.ID)
                    continue;
                if (i.type == ActorType.PC)
                    if (((ActorPC)i).Online)
                        return true;
            }

            return false;
        }
    }
}