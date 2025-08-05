using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Treasure;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Tasks.Item;

namespace SagaMap
{
    public partial class Map
    {
        public void AddItemDrop(uint itemID, string treasureGroup, Actor ori, bool party, bool Public1, bool Public20,
            ushort count = 1, ushort minCount = 0, ushort maxCount = 0, int rate = 10000, bool roll = false,
            uint pictID = 0)
        {
            Actor owner = null;
            ActorMob MMob = null;
            if (ori.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)ori.e;
                if (eh.AI.firstAttacker != null)
                    owner = eh.AI.firstAttacker;
                MMob = (ActorMob)ori;
            }

            var owners = new List<Actor>();
            var owners2 = new List<Actor>();
            if (owner != null)
                if (owner.type == ActorType.PC)
                {
                    var pc = (ActorPC)owner;
                    if (pc.Party != null)
                        foreach (var i in pc.Party.Members.Values)
                        {
                            if (!i.Online)
                                continue;
                            if (i.MapID != ori.MapID)
                                continue;
                            owners2.Add(i);
                        }

                    if (pc.Party != null && party)
                    {
                        foreach (var i in pc.Party.Members.Values)
                        {
                            if (!i.Online)
                                continue;
                            if (i.MapID != ori.MapID)
                                continue;
                            if (rate != 10000 && party)
                            {
                                if (Global.Random.Next(0, 10000) <= rate)
                                    owners.Add(i);
                            }
                            else
                            {
                                owners.Add(i);
                            }
                        }
                    }
                    else if (Public20)
                    {
                        var map = MapManager.Instance.GetMap(ori.MapID);
                        var actors = map.GetActorsArea(ori, 3000, false, true);
                        if (ori.type == ActorType.MOB)
                        {
                            var mob = (ActorMob)ori;
                            foreach (var ac in actors)
                                if (((MobEventHandler)mob.e).AI.DamageTable.ContainsKey(ac.ActorID))
                                    if (((MobEventHandler)mob.e).AI.DamageTable[ac.ActorID] > ori.MaxHP * 0.2f)
                                        if (!owners.Contains(ac))
                                            owners.Add(ac);
                            //owners2.Add(ac);
                        }
                    }
                    else if (Public1)
                    {
                        var map = MapManager.Instance.GetMap(ori.MapID);
                        var actors = map.GetActorsArea(ori, 3000, false, true);
                        if (ori.type == ActorType.MOB)
                        {
                            var mob = (ActorMob)ori;
                            foreach (var ac in actors)
                                if (((MobEventHandler)mob.e).AI.DamageTable.ContainsKey(ac.ActorID))
                                {
                                    var damage = ((MobEventHandler)mob.e).AI.DamageTable[ac.ActorID];
                                    if (damage >= 1 /*ori.MaxHP * 0.001f*/ && damage < ori.MaxHP * 0.2f)
                                        if (!owners.Contains(ac))
                                            owners.Add(ac);
                                }
                        }
                    }
                    else
                    {
                        owners.Add(owner);
                    }

                    if (Public1 || Public20) //直接入包
                    {
                        Item item = null;
                        //List<string> IPs = new List<string>();
                        foreach (var i in owners)
                            if (i.type == ActorType.PC)
                            {
                                var pcs = (ActorPC)i;
                                byte countss = 0;
                                foreach (var x in MapClientManager.Instance.OnlinePlayer)
                                    if (x.Character.Account.MacAddress == pcs.Account.MacAddress &&
                                        pcs.Account.GMLevel < 20)
                                        countss++;
                                if (countss > 1)
                                {
                                    MapClient.FromActorPC((ActorPC)i).SendSystemMessage("系统检测到您有可能多开，因此无法获得野外BOSS奖励。");
                                    continue;
                                }

                                item = ItemFactory.Instance.GetItem(itemID, true);
                                item.Stack = count;
                                var arg = new EffectArg();
                                arg.actorID = 0xFFFFFFFF;
                                arg.effectID = 7116;
                                arg.x = Global.PosX16to8(MMob.X, Width);
                                arg.y = Global.PosY16to8(MMob.Y, Height);
                                arg.oneTime = false;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);
                                arg.effectID = 7115;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);

                                MapClient.FromActorPC((ActorPC)i).AddItem(item, true);
                            }
                    }
                    else if (party)
                    {
                        Item item = null;
                        foreach (var i in owners)
                            if (i.type == ActorType.PC)
                            {
                                item = ItemFactory.Instance.GetItem(itemID, true);
                                item.Stack = count;
                                var arg = new EffectArg();
                                arg.actorID = 0xFFFFFFFF;
                                arg.effectID = 7116;
                                arg.x = Global.PosX16to8(MMob.X, Width);
                                arg.y = Global.PosY16to8(MMob.Y, Height);
                                arg.oneTime = false;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);
                                arg.effectID = 7115;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);

                                MapClient.FromActorPC((ActorPC)i).AddItem(item, true);
                            }
                    }
                    else //掉率在地上
                    {
                        //List<string> IPs = new List<string>();
                        foreach (var i in owners)
                        {
                            Item itemDroped = null;
                            if (i.type == ActorType.PC)
                            {
                                var pcs = (ActorPC)i;
                                /*if (IPs.Contains(pcs.Account.LastIP))
                                    continue;
                                else
                                    IPs.Add(pcs.Account.LastIP);*/
                            }

                            if (itemID != 0)
                            {
                                itemDroped = ItemFactory.Instance.GetItem(itemID, true);
                                if (minCount == 0 && maxCount == 0)
                                    itemDroped.Stack = count;
                                else
                                    itemDroped.Stack = (ushort)Global.Random.Next(minCount, maxCount);
                            }

                            if (itemID == 10020758) itemDroped.PictID = pictID;
                            if (treasureGroup != null)
                            {
                                if (TreasureFactory.Instance.Items.ContainsKey(treasureGroup))
                                {
                                    var item2 = TreasureFactory.Instance.GetRandomItem(treasureGroup);
                                    itemDroped = ItemFactory.Instance.GetItem(item2.ID, true);
                                    itemDroped.Stack = (ushort)item2.Count;
                                }
                                else
                                {
                                    itemDroped = ItemFactory.Instance.GetItem(itemID, true);
                                }
                            }

                            var actor = new ActorItem(itemDroped);
                            if (roll) actor.Roll = true;
                            actor.e = new ItemEventHandler(actor);
                            actor.Owner = i;
                            actor.Party = party;
                            actor.MapID = ID;
                            short[] pos;
                            if (party)
                            {
                                pos = GetRandomPosAroundActor(ori);
                            }
                            else if (Public1)
                            {
                                pos = new short[2];
                                pos[0] = MMob.X;
                                pos[1] = MMob.Y;
                            }
                            else
                            {
                                pos = new short[2];
                                pos[0] = MMob.X;
                                pos[1] = MMob.Y;
                            }

                            actor.X = pos[0];
                            actor.Y = pos[1];
                            RegisterActor(actor);
                            actor.invisble = false;
                            OnActorVisibilityChange(actor);

                            //中秋节活动
                            if (party)
                            {
                                var arg = new EffectArg();
                                arg.actorID = 0xFFFFFFFF;
                                arg.effectID = 7116;
                                arg.x = Global.PosX16to8(pos[0], Width);
                                arg.y = Global.PosY16to8(pos[1], Height);
                                arg.oneTime = false;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);
                            }
                            else if (Public1)
                            {
                                var arg = new EffectArg();
                                arg.actorID = 0xFFFFFFFF;
                                arg.effectID = 7116;
                                arg.x = Global.PosX16to8(MMob.X, Width);
                                arg.y = Global.PosY16to8(MMob.Y, Height);
                                arg.oneTime = false;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);
                                arg.effectID = 7115;
                                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, ori, false);
                            }

                            var task = new DeleteItem(actor);
                            task.Activate();
                            actor.Tasks.Add("DeleteItem", task);
                        }
                    }
                }
        }
    }
}