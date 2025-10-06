using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Skill;

namespace SagaMap.Mob
{
    public class MobAttack : MultiRunTask
    {
        private readonly MobAI mob;
        public Actor dActor;

        public MobAttack(MobAI mob, Actor dActor)
        {
            DueTime = 0;
            this.mob = mob;
            Period = calcDelay(mob.Mob);
            this.dActor = dActor;
        }

        private int calcDelay(Actor actor)
        {
            var aspd = 0;
            uint delay = 0;
            if (mob.Mob.type == ActorType.MOB)
            {
                var tar = (ActorMob)mob.Mob;
                aspd = tar.BaseData.aspd;
            }

            if (mob.Mob.type == ActorType.PET)
            {
                var pet = (ActorPet)mob.Mob;
                aspd = pet.BaseData.aspd;
            }

            if (mob.Mob.type == ActorType.SHADOW || mob.Mob.type == ActorType.GOLEM ||
                mob.Mob.type == ActorType.PC)
                aspd = mob.Mob.Status.aspd;

            aspd += actor.Status.aspd_skill;
            if (aspd > 960)
                aspd = 960;
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                delay = 2000 - (uint)(1.0f - aspd / 1000.0f);
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.doubleHand)
                        delay = (uint)(delay / 0.7f);
            }
            else
            {
                delay = 2000 - (uint)(1.0f - aspd / 1000.0f);
            }

            if (actor.Status.aspd_skill_perc >= 1f)
                delay = (uint)(delay / actor.Status.aspd_skill_perc);
            return (int)delay;
        }

        public override void CallBack()
        {
            //ClientManager.EnterCriticalArea();
            try
            {
                if (!mob.CanAttack)
                    //ClientManager.LeaveCriticalArea();
                    return;
                if (mob.Mob.HP == 0 || dActor.HP == 0 || !mob.Hate.ContainsKey(dActor.ActorID) ||
                    mob.Mob.Tasks.ContainsKey("AutoCast"))
                {
                    if (mob.Hate.ContainsKey(dActor.ActorID)) mob.Hate.Remove(dActor.ActorID);
                    if (Activated) Deactivate();
                    //ClientManager.LeaveCriticalArea();
                    return;
                }

                if (mob.Mob.type == ActorType.PET)
                {
                    var pet = (ActorPet)mob.Mob;
                    if (pet.Owner.ActorID == dActor.ActorID)
                    {
                        if (Activated) Deactivate();
                        //ClientManager.LeaveCriticalArea();
                        return;
                    }
                }

                if (mob.Master != null)
                {
                    if (dActor.ActorID == mob.Master.ActorID)
                        //ClientManager.LeaveCriticalArea();
                        return;
                    if (dActor.type == ActorType.MOB)
                        if (((MobEventHandler)dActor.e).AI.Master != null)
                            if (((MobEventHandler)dActor.e).AI.Master.ActorID == mob.Master.ActorID)
                                return;
                }

                if (dActor.type == ActorType.PC)
                {
                    var pc = (ActorPC)dActor;
                    if (pc.HP == 0)
                    {
                        if (mob.Hate.ContainsKey(dActor.ActorID)) mob.Hate.Remove(dActor.ActorID);
                        if (Activated) Deactivate();
                        //ClientManager.LeaveCriticalArea();
                        return;
                    }

                    if (pc.Status.Additions.ContainsKey("Hiding") || pc.Status.Additions.ContainsKey("Cloaking"))
                    {
                        if (mob.Hate.ContainsKey(pc.ActorID))
                            mob.Hate.Remove(pc.ActorID);
                        if (Activated)
                            Deactivate();
                        return;
                    }
                }

                var arg = new SkillArg();
                SkillHandler.Instance.Attack(mob.Mob, dActor, arg);
                mob.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, mob.Mob, true);
                Period = calcDelay(mob.Mob);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
            //ClientManager.LeaveCriticalArea();
        }
    }
}