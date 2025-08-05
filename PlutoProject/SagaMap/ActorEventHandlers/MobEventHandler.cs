using System;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.ODWar;
using SagaMap.Scripting;
using SagaMap.Skill;
using SagaMap.Skill.Additions;
using SagaMap.Tasks.Mob;

namespace SagaMap.ActorEventHandlers
{
    public class MobEventHandler : ActorEventHandler
    {
        public MobAI AI;
        private MobCallback currentCall;
        private ActorPC currentPC;
        public ActorMob mob;

        public MobEventHandler(ActorMob mob)
        {
            this.mob = mob;
            AI = new MobAI(mob);
        }

        public event MobCallback Dying;
        public event MobCallback Attacking;
        public event MobCallback Moving;
        public event MobCallback Defending;
        public event MobCallback Returning;
        public event MobCallback SkillUsing;
        public event MobCallback Updating;
        public event MobCallback FirstTimeDefending;

        #region ActorEventHandler Members

        public void OnActorSkillCancel(Actor sActor)
        {
        }

        public void OnActorAppears(Actor aActor)
        {
            if (!mob.VisibleActors.Contains(aActor.ActorID))
                mob.VisibleActors.Add(aActor.ActorID);
            if (aActor.type == ActorType.PC)
                if (!AI.Activated)
                    AI.Start();
            if (aActor.type == ActorType.SHADOW && AI.Hate.Count != 0)
                if (!AI.Hate.ContainsKey(aActor.ActorID))
                    AI.Hate.Add(aActor.ActorID, mob.MaxHP);
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
            if (dActor.type == ActorType.PC)
                if (AI.Hate.ContainsKey(dActor.ActorID))
                    AI.Hate.Remove(dActor.ActorID);
        }

        public void OnActorReturning(Actor sActor)
        {
            try
            {
                if (Returning != null)
                {
                    if (AI.lastAttacker != null)
                    {
                        if (AI.lastAttacker.type == ActorType.PC)
                        {
                            RunCallback(Returning, (ActorPC)AI.lastAttacker);
                            return;
                        }

                        if (AI.lastAttacker.type == ActorType.SHADOW)
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                                if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                                {
                                    var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                    RunCallback(Returning, pc);
                                    return;
                                }
                    }

                    RunCallback(Returning, null);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
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
                        {
                            RunCallback(SkillUsing, (ActorPC)AI.lastAttacker);
                            return;
                        }

                        if (AI.lastAttacker.type == ActorType.SHADOW)
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                                if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                                {
                                    var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                    RunCallback(SkillUsing, pc);
                                    return;
                                }
                    }

                    RunCallback(SkillUsing, null);
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
                        {
                            RunCallback(Moving, (ActorPC)AI.lastAttacker);
                        }
                        else if (AI.lastAttacker.type == ActorType.SHADOW)
                        {
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                            {
                                if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                                {
                                    var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                    RunCallback(Moving, pc);
                                }
                                else
                                {
                                    RunCallback(Moving, null);
                                }
                            }
                            else
                            {
                                RunCallback(Moving, null);
                            }
                        }
                        else
                        {
                            RunCallback(Moving, null);
                        }
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
            AI.Pause();
        }


        public void OnCharInfoUpdate(Actor aActor)
        {
        }


        public void OnPlayerSizeChange(Actor aActor)
        {
        }

        private bool checkDropSpecial()
        {
            if (AI.firstAttacker != null)
            {
                if (AI.firstAttacker.Status != null)
                {
                    foreach (var i in AI.firstAttacker.Status.Additions.Values)
                        if (i.GetType() == typeof(Knowledge))
                        {
                            var know = (Knowledge)i;
                            if (know.MobTypes.Contains(mob.BaseData.mobType))
                                return true;
                        }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        public void OnDie()
        {
            OnDie(true);
        }

        public void OnDie(bool loot)
        {
            if (mob.Buff.Dead) return;
            mob.Buff.Dead = true;
            try
            {
                if (mob.Owner != null)
                    mob.Owner.Slave.Remove(mob);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            if (AI.firstAttacker != null)
                if (AI.firstAttacker.type == ActorType.GOLEM)
                {
                    var golem = (ActorGolem)AI.firstAttacker;
                    var ehs = (MobEventHandler)golem.e;
                    var skills = new OtherAddition(null, golem, "石像击杀怪物CD", Global.Random.Next(10000, 45000));
                    skills.OnAdditionStart += (s, e) => { ehs.AI.Mode.mask.SetValue(1, false); };
                    skills.OnAdditionEnd += (s, e) => { ehs.AI.Mode.mask.SetValue(1, true); };
                    SkillHandler.ApplyAddition(golem, skills);
                }

            if (mob.Status.Additions.ContainsKey("Rebone"))
            {
                mob.Buff.Dead = false;

                mob.HP = mob.MaxHP;
                SkillHandler.RemoveAddition(mob, "Rebone");
                AI.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, mob, false);
                var zombie = new Zombie(mob);
                SkillHandler.ApplyAddition(mob, zombie);
                mob.Status.undead = true;
                AI.DamageTable.Clear();
                AI.Hate.Clear();
                AI.firstAttacker = null;
            }
            else
            {
                try
                {
                    if (Dying != null)
                    {
                        if (AI.lastAttacker != null)
                        {
                            if (AI.lastAttacker.type == ActorType.PC)
                            {
                                RunCallback(Dying, (ActorPC)AI.lastAttacker);
                            }
                            else if (AI.lastAttacker.type == ActorType.SHADOW)
                            {
                                if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                                {
                                    if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type ==
                                        ActorType.PC)
                                    {
                                        var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                        RunCallback(Dying, pc);
                                    }
                                    else
                                    {
                                        RunCallback(Dying, null);
                                    }
                                }
                                else
                                {
                                    RunCallback(Dying, null);
                                }
                            }
                            else
                            {
                                RunCallback(Dying, null);
                            }
                        }
                        else
                        {
                            RunCallback(Dying, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }

                AI.Pause();
                if (loot)
                {
                    //分配经验
                    ExperienceManager.Instance.ProcessMobExp(mob);
                    //drops
                    //special drops

                    //boss掉心
                    if (Configuration.Configuration.Instance.ActiveSpecialLoot)
                        if (mob.BaseData.mobType.ToString().Contains("BOSS") && AI.SpawnDelay >= 1800000)
                        {
                            if (Global.Random.Next(0, 10000) <= Configuration.Configuration.Instance.BossSpecialLootRate)
                                for (var i = 0; i < Configuration.Configuration.Instance.BossSpecialLootNum; i++)
                                    AI.map.AddItemDrop(Configuration.Configuration.Instance.BossSpecialLootID, null, mob, false,
                                        false, false);
                        }
                        else if (Global.Random.Next(0, 10000) <= Configuration.Configuration.Instance.NomalMobSpecialLootRate &&
                                 ((MobEventHandler)mob.e).AI.SpawnDelay != 0)
                        {
                            for (var i = 0; i < Configuration.Configuration.Instance.NomalMobSpecialLootNum; i++)
                                AI.map.AddItemDrop(Configuration.Configuration.Instance.NomalMobSpecialLootID, null, mob, false,
                                    false, false);
                        }

                    //drops
                    var dropDeterminator = Global.Random.Next(0, 10000);
                    int baseValue = 0, maxVlaue = 0;
                    var stamp = false;
                    var special = false;
                    ActorPC owner = null;
                    if (mob.type == ActorType.MOB)
                    {
                        var actors = MapManager.Instance.GetMap(mob.MapID).GetActorsArea(mob, 12700, false)
                            .Where(x => x.type == ActorType.PC && (x as ActorPC).Online).ToList();
                        var eh = (MobEventHandler)mob.e;
                        if (eh.AI.firstAttacker != null && eh.AI.firstAttacker.type == ActorType.PC)
                            owner = (ActorPC)eh.AI.firstAttacker;
                    }

                    //印章因为目前掉率全都是0,所以取了最小的万分之一
                    if (mob.BaseData.stampDrop != null)
                        if (Global.Random.Next(0, 9999) <= mob.BaseData.stampDrop.Rate *
                            Configuration.Configuration.Instance.CalcStampDropRateForPC(owner))
                        {
                            AI.map.AddItemDrop(mob.BaseData.stampDrop.ItemID, null, mob, false, false, false);
                            stamp = true;
                        }

                    //dropDeterminator = this.mob.BaseData.dropItems.Sum(x => x.Rate) + this.mob.BaseData.dropItemsSpecial.Sum(x => x.Rate);
                    //特殊掉落(知识掉落)
                    if ((!stamp || Configuration.Configuration.Instance.MultipleDrop) && checkDropSpecial())
                        foreach (var i in mob.BaseData.dropItemsSpecial)
                        {
                            dropDeterminator = Global.Random.Next(0, 9999);
                            if (!Configuration.Configuration.Instance.MultipleDrop)
                            {
                                maxVlaue = baseValue +
                                           (int)(i.Rate * Configuration.Configuration.Instance.CalcSpecialDropRateForPC(owner) /
                                                 100.0f);
                                if (dropDeterminator >= baseValue && dropDeterminator < maxVlaue)
                                {
                                    AI.map.AddItemDrop(i.ItemID, i.TreasureGroup, mob, i.Party, i.Public, i.Public20);
                                    special = true;
                                }

                                baseValue = maxVlaue;
                            }
                            else
                            {
                                if (dropDeterminator < i.Rate * Configuration.Configuration.Instance.CalcSpecialDropRateForPC(owner) /
                                    100.0f)
                                {
                                    AI.map.AddItemDrop(i.ItemID, i.TreasureGroup, mob, i.Party, i.Public, i.Public20);
                                    special = true;
                                }
                            }
                        }

                    baseValue = 0;
                    maxVlaue = 0;
                    //如果已经掉落印章,并且掉落特殊物品,同时开启了多重掉落
                    if ((!stamp && !special) || Configuration.Configuration.Instance.MultipleDrop)
                    {
                        if (Configuration.Configuration.Instance.MultipleDrop)
                        {
                            foreach (var i in mob.BaseData.dropItems)
                            {
                                var denominator = mob.BaseData.dropItems.Sum(x => x.Rate);

                                //这里简单的做一个头发的过滤
                                if (i.ItemID != 10000000)
                                    continue;

                                if (Global.Random.Next(1, denominator) <
                                    i.Rate * Configuration.Configuration.Instance.CalcGlobalDropRateForPC(owner))
                                    AI.map.AddItemDrop(i.ItemID, i.TreasureGroup, mob, i.Party, i.Public, i.Public20);
                            }
                        }
                        else
                        {
                            //如果这个怪物有掉落的话...
                            if (mob.BaseData.dropItems.Count > 0)
                            {
                                maxVlaue = baseValue = 0;
                                var oneshotdrop = false;
                                var denominator = Global.Random.Next(1, mob.BaseData.dropItems.Sum(x => x.Rate));

                                for (var ix = 0; ix < (int)Configuration.Configuration.Instance.CalcGlobalDropRateForPC(owner); ix++)
                                    foreach (var i in mob.BaseData.dropItems)
                                    {
                                        if (oneshotdrop)
                                            continue;

                                        maxVlaue = baseValue + i.Rate;
                                        if (denominator >= baseValue && denominator < maxVlaue)
                                        {
                                            //这里简单的做一个头发的过滤, 掉了个头发也算是掉东西了.
                                            if (i.ItemID != 10000000)
                                            {
                                                AI.map.AddItemDrop(i.ItemID, i.TreasureGroup, mob, i.Party, i.Public,
                                                    i.Public20);
                                                oneshotdrop = true;
                                            }
                                            else
                                            {
                                                if (ix == (int)Configuration.Configuration.Instance.CalcGlobalDropRateForPC(owner) -
                                                    1)
                                                    oneshotdrop = true;
                                            }
                                        }

                                        baseValue = maxVlaue;
                                    }
                            }
                        }
                    }
                }

                mob.ClearTaskAddition();
                AI.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, mob, false);
                var task = new DeleteCorpse(mob);
                mob.Tasks.Add("DeleteCorpse", task);
                task.Activate();

                if (AI.SpawnDelay != 0)
                {
                    var respawn = new Respawn(mob, AI.SpawnDelay);
                    mob.Tasks.Add("Respawn", respawn);
                    respawn.Activate();

                    /*if (this.AI.Announce != "" && this.AI.SpawnDelay >= 300)
                    {
                        Tasks.Mob.RespawnAnnounce respawnannounce = new Tasks.Mob.RespawnAnnounce(this.mob, this.AI.SpawnDelay - 300000);
                        this.mob.Tasks.Add("RespawnAnnounce", respawnannounce);
                        respawnannounce.Activate();
                    }*/
                }

                AI.firstAttacker = null;
                mob.Status.attackingActors.Clear();
                AI.DamageTable.Clear();
                mob.VisibleActors.Clear();

                if (AI.Mode.Symbol || AI.Mode.SymbolTrash) ODWarManager.Instance.SymbolDown(mob.MapID, mob);
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
                        {
                            RunCallback(Attacking, (ActorPC)AI.lastAttacker);
                            return;
                        }

                        if (AI.lastAttacker.type == ActorType.SHADOW)
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                                if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                                {
                                    var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                    RunCallback(Attacking, pc);
                                    return;
                                }
                    }

                    RunCallback(Attacking, null);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnHPMPSPUpdate(Actor sActor)
        {
            /*if (Skill.SkillHandler.Instance.isBossMob(this.mob))
            {
                if (!this.mob.Tasks.ContainsKey("MobRecover"))
                {
                    Tasks.Mob.MobRecover MobRecover = new SagaMap.Tasks.Mob.MobRecover((ActorMob)this.mob);
                    this.mob.Tasks.Add("MobRecover", MobRecover);
                    MobRecover.Activate();
                }
            }*/ //关闭怪物回复线程以节省资源
            if (sActor.HP < sActor.MaxHP * 0.05f) return;
            if (Defending != null)
            {
                if (AI.lastAttacker != null)
                {
                    if (AI.lastAttacker.type == ActorType.PC)
                    {
                        RunCallback(Defending, (ActorPC)AI.lastAttacker);
                        return;
                    }

                    if (AI.lastAttacker.type == ActorType.SHADOW)
                        if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                            {
                                var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                RunCallback(Defending, pc);
                                return;
                            }
                }

                RunCallback(Defending, null);
            }

            if (FirstTimeDefending != null && !mob.FirstDefending)
            {
                mob.FirstDefending = true;
                if (AI.lastAttacker != null)
                {
                    if (AI.lastAttacker.type == ActorType.PC)
                    {
                        RunCallback(FirstTimeDefending, (ActorPC)AI.lastAttacker);
                        return;
                    }

                    if (AI.lastAttacker.type == ActorType.SHADOW)
                        if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master != null)
                            if (((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master.type == ActorType.PC)
                            {
                                var pc = (ActorPC)((PetEventHandler)((ActorShadow)AI.lastAttacker).e).AI.Master;
                                RunCallback(FirstTimeDefending, pc);
                                return;
                            }
                }

                RunCallback(FirstTimeDefending, null);
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

        private void RunCallback(MobCallback callback, ActorPC pc)
        {
            try
            {
                currentCall = callback;
                currentPC = pc;
                var th = new Thread(Run);
                th.Start();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        private DateTime mark = DateTime.Now;

        private void Run()
        {
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

        public void OnActorPaperChange(ActorPC aActor)
        {
        }

        #endregion
    }
}