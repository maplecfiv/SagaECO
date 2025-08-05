using System;
using System.Threading;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Partner;
using SagaMap.Scripting;
using SagaMap.Tasks.Partner;

namespace SagaMap.ActorEventHandlers
{
    public class PartnerEventHandler : ActorEventHandler
    {
        private readonly ActorPartner partner;
        public PartnerAI AI;
        private PartnerCallback currentCall;
        private ActorPC currentPC;

        public PartnerEventHandler(ActorPartner partner)
        {
            this.partner = partner;
            AI = new PartnerAI(partner);
        }

        public event PartnerCallback Dying;
        public event PartnerCallback Attacking;
        public event PartnerCallback Moving;
        public event PartnerCallback Defending;
        public event PartnerCallback SkillUsing;
        public event PartnerCallback Updating;

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
            if (!partner.VisibleActors.Contains(aActor.ActorID))
                partner.VisibleActors.Add(aActor.ActorID);
            if (aActor.ActorID == partner.Owner.ActorID && partner.type != ActorType.SHADOW)
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
            if (partner.VisibleActors.Contains(dActor.ActorID))
                partner.VisibleActors.Remove(dActor.ActorID);
            if (dActor.type == ActorType.PC && dActor.ActorID != partner.Owner.ActorID)
                if (AI.Hate.ContainsKey(dActor.ActorID))
                    AI.Hate.Remove(dActor.ActorID);
        }

        public void OnActorSkillUse(Actor sActor, MapEventArgs args)
        {
            var arg = (SkillArg)args;
            try
            {
                AI.OnSeenSkillUse(arg);
            }
            catch
            {
            }

            try
            {
                if (SkillUsing != null)
                {
                    if (AI.lastAttacker != null)
                    {
                        if (AI.lastAttacker.type == ActorType.PC)
                            RunCallback(SkillUsing, (ActorPC)AI.lastAttacker);
                        else
                            RunCallback(SkillUsing, null);
                    }
                    else
                    {
                        RunCallback(SkillUsing, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed, MoveType moveType)
        {
        }

        public void OnActorStopsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
            try
            {
                if (Moving != null)
                {
                    if (AI.lastAttacker != null)
                    {
                        if (AI.lastAttacker.type == ActorType.PC)
                            RunCallback(Moving, (ActorPC)AI.lastAttacker);
                        else
                            RunCallback(Moving, null);
                    }
                    else
                    {
                        RunCallback(Moving, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
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
            partner.VisibleActors.Clear();
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
            OnDie(true);
        }

        public void OnDie(bool loot)
        {
            partner.Buff.Dead = true;
            partner.ClearTaskAddition();
            if (partner.type != ActorType.SHADOW)
            {
                var eh = (PCEventHandler)partner.Owner.e;
                eh.Client.DeletePartner();
                if (partner.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                {
                    var p = new CSMG_ITEM_MOVE();
                    p.data = new byte[11];
                    var item = partner.Owner.Inventory.Equipments[EnumEquipSlot.PET];
                    if (item.Durability != 0) item.Durability--;
                    eh.Client.SendItemInfo(item);
                    eh.Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.PET_FRIENDLY_DOWN,
                        partner.Name));
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
                partner.Owner.Slave.Remove(partner);
                AI.Pause();
                var task = new DeleteCorpse(partner);
                partner.Tasks.Add("DeleteCorpse", task);
                task.Activate();
                if (partner.Tasks.ContainsKey("Shadow"))
                {
                    partner.Tasks["Shadow"].Deactivate();
                    partner.Tasks.Remove("Shadow");
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
            var arg = (SkillArg)args;
            AI.OnSeenSkillUse(arg);
            try
            {
                if (Attacking != null)
                {
                    if (AI.lastAttacker != null)
                    {
                        if (AI.lastAttacker.type == ActorType.PC)
                            RunCallback(Attacking, (ActorPC)AI.lastAttacker);
                        else
                            RunCallback(Attacking, null);
                    }
                    else
                    {
                        RunCallback(Attacking, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnHPMPSPUpdate(Actor sActor)
        {
            if (Defending != null)
            {
                if (AI.lastAttacker != null)
                {
                    if (AI.lastAttacker.type == ActorType.PC)
                        RunCallback(Defending, (ActorPC)AI.lastAttacker);
                    else
                        RunCallback(Defending, null);
                }
                else
                {
                    RunCallback(Defending, null);
                }
            }
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
                    AI.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SPEED_UPDATE, null, partner, true);
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

        private void RunCallback(PartnerCallback callback, ActorPC pc)
        {
            currentCall = callback;
            currentPC = pc;
            var th = new Thread(Run);
            th.Start();
        }

        private void Run()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                if (currentCall != null)
                {
                    if (currentPC != null)
                    {
                        currentCall.Invoke(this, currentPC);
                    }
                    else
                    {
                        if (AI.map.Creator != null)
                            currentCall.Invoke(this, AI.map.Creator);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            ClientManager.LeaveCriticalArea();
        }

        public void OnActorFurnitureList(object obj)
        {
        }

        public void OnUpdate(Actor aActor)
        {
            try
            {
                if (Updating != null)
                {
                    if (AI.lastAttacker != null)
                    {
                        if (AI.lastAttacker.type == ActorType.PC)
                            RunCallback(Updating, (ActorPC)AI.lastAttacker);
                        else
                            RunCallback(Updating, null);
                    }
                    else
                    {
                        RunCallback(Updating, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        #endregion
    }
}