using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Packets.Client;
using SagaMap.Packets.Client.Item;
using SagaMap.Tasks.Mob;

namespace SagaMap.ActorEventHandlers
{
    public class PetEventHandler : ActorEventHandler
    {
        private readonly ActorPet mob;
        public MobAI AI;

        public PetEventHandler(ActorPet mob)
        {
            this.mob = mob;
            AI = new MobAI(mob);
        }

        #region ActorEventHandler Members

        public void OnActorSkillCancel(Actor sActor)
        {
        }

        public void OnActorReturning(Actor sActor)
        {
        }

        public void OnActorPaperChange(ActorPC aActor)
        {
        }

        public void OnActorAppears(Actor aActor)
        {
            if (!mob.VisibleActors.Contains(aActor.ActorID))
                mob.VisibleActors.Add(aActor.ActorID);
            if (aActor.ActorID == mob.Owner.ActorID && mob.type != ActorType.SHADOW)
                if (!AI.Hate.ContainsKey(aActor.ActorID))
                    AI.Hate.Add(aActor.ActorID, 1);
        }

        public void OnPlayerShopChange(Actor aActor)
        {
        }

        public void OnPlayerShopChangeClose(Actor aActor)
        {
        }

        public void OnActorChangeEquip(Actor sActor, MapEventArgs args)
        {
        }

        public void OnActorChat(Actor cActor, MapEventArgs args)
        {
        }

        public void OnActorDisappears(Actor dActor)
        {
            if (mob.VisibleActors.Contains(dActor.ActorID))
                mob.VisibleActors.Remove(dActor.ActorID);
            if (dActor.type == ActorType.PC && dActor.ActorID != mob.Owner.ActorID)
                if (AI.Hate.ContainsKey(dActor.ActorID))
                    AI.Hate.Remove(dActor.ActorID);
        }

        public void OnActorSkillUse(Actor sActor, MapEventArgs args)
        {
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed, MoveType moveType)
        {
        }

        public void OnActorStopsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
        }

        public void OnCreate(bool success)
        {
        }


        public void OnActorChangeEmotion(Actor aActor, MapEventArgs args)
        {
        }

        public void OnActorChangeMotion(Actor aActor, MapEventArgs args)
        {
        }

        public void OnActorChangeWaitType(Actor aActor)
        {
        }

        public void OnDelete()
        {
            //TODO: add something
            mob.VisibleActors.Clear();
            AI.Pause();
        }


        public void OnCharInfoUpdate(Actor aActor)
        {
        }


        public void OnPlayerSizeChange(Actor aActor)
        {
        }

        public void OnDie()
        {
            mob.Buff.Dead = true;
            mob.ClearTaskAddition();
            if (mob.type != ActorType.SHADOW)
            {
                var eh = (PCEventHandler)mob.Owner.e;
                eh.Client.DeletePet();
                var p = new CSMG_ITEM_MOVE();
                p.data = new byte[11];
                if (mob.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                {
                    var item = mob.Owner.Inventory.Equipments[EnumEquipSlot.PET];
                    if (item.Durability != 0) item.Durability--;

                    eh.Client.SendItemInfo(item);

                    eh.Client.SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.PET_FRIENDLY_DOWN, mob.Name));
                    var arg = new EffectArg();
                    arg.actorID = eh.Client.Character.ActorID;
                    arg.effectID = 8044;
                    eh.OnShowEffect(eh.Client.Character, arg);
                    p.InventoryID = item.Slot;
                    p.Target = ContainerType.BODY;
                    p.Count = 1;
                    eh.Client.OnItemMove(p);
                }
            }
            else
            {
                mob.Owner.Slave.Remove(mob);
                AI.Pause();
                var task = new DeleteCorpse(mob);
                mob.Tasks.Add("DeleteCorpse", task);
                task.Activate();
                if (mob.Tasks.ContainsKey("Shadow"))
                {
                    mob.Tasks["Shadow"].Deactivate();
                    mob.Tasks.Remove("Shadow");
                }
            }
        }

        public void OnKick()
        {
            throw new NotImplementedException();
        }

        public void OnMapLoaded()
        {
            throw new NotImplementedException();
        }

        public void OnReSpawn()
        {
            throw new NotImplementedException();
        }

        public void OnSendMessage(string from, string message)
        {
            throw new NotImplementedException();
        }

        public void OnSendWhisper(string name, string message, byte flag)
        {
            throw new NotImplementedException();
        }

        public void OnTeleport(short x, short y)
        {
            throw new NotImplementedException();
        }

        public void OnAttack(Actor aActor, MapEventArgs args)
        {
        }

        public void OnHPMPSPUpdate(Actor sActor)
        {
        }

        public void OnPlayerChangeStatus(ActorPC aActor)
        {
        }

        public void OnActorChangeBuff(Actor sActor)
        {
        }

        public void OnLevelUp(Actor sActor, MapEventArgs args)
        {
        }

        public void OnPlayerMode(Actor aActor)
        {
        }

        public void OnShowEffect(Actor aActor, MapEventArgs args)
        {
        }

        public void OnActorPossession(Actor aActor, MapEventArgs args)
        {
        }

        public void OnActorPartyUpdate(ActorPC aActor)
        {
        }

        public void OnActorSpeedChange(Actor mActor)
        {
        }

        public void OnSignUpdate(Actor aActor)
        {
        }

        public void PropertyUpdate(UpdateEvent arg, int para)
        {
            switch (arg)
            {
                case UpdateEvent.SPEED:
                    AI.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SPEED_UPDATE, null, mob, true);
                    break;
            }
        }

        public void PropertyRead(UpdateEvent arg)
        {
        }

        public void OnActorRingUpdate(ActorPC aActor)
        {
        }

        public void OnActorWRPRankingUpdate(ActorPC aActor)
        {
        }

        public void OnActorChangeAttackType(ActorPC aActor)
        {
        }

        public void OnActorFurnitureSit(ActorPC aActor)
        {
        }

        public void OnActorFurnitureList(object obj)
        {
        }

        public void OnUpdate(Actor aActor)
        {
        }

        #endregion
    }
}