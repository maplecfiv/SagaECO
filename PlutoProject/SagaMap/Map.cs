using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.DefWar;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Mob;
using SagaDB.Ring;
using SagaDB.Treasure;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Dungeon;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Packets.Client;
using SagaMap.Packets.Client.Possession;
using SagaMap.Scripting;
using SagaMap.Tasks.Item;
using Item = SagaDB.Item.Item;

namespace SagaMap
{
    public class Map
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

        internal object RegisterActor()
        {
            throw new NotImplementedException();
        }

        public enum EVENT_TYPE
        {
            APPEAR,
            DISAPPEAR,
            MOTION,
            EMOTION,
            CHAT,
            SKILL,
            CHANGE_EQUIP,
            CHANGE_STATUS,
            BUFF_CHANGE,
            ACTOR_SELECTION,
            YAW_UPDATE,
            CHAR_INFO_UPDATE,
            PLAYER_SIZE_UPDATE,
            ATTACK,
            HPMPSP_UPDATE,
            LEVEL_UP,
            PLAYER_MODE,
            SHOW_EFFECT,
            POSSESSION,
            PARTY_NAME_UPDATE,
            SPEED_UPDATE,
            SIGN_UPDATE,
            RING_NAME_UPDATE,
            WRP_RANKING_UPDATE,
            ATTACK_TYPE_CHANGE,
            PLAYERSHOP_CHANGE,
            PLAYERSHOP_CHANGE_CLOSE,
            WAITTYPE,
            FURNITURE_SIT,
            PAPER_CHANGE,
            TELEPORT,
            SKILL_CANCEL
        }

        public enum MOVE_TYPE
        {
            START,
            STOP
        }

        public enum TOALL_EVENT_TYPE
        {
            CHAT
        }

        private const uint ID_BORDER_MOB = 10000;
        private const uint ID_BORDER_PET = 20000;
        private const uint ID_BORDER_GOLEM = 40000;
        private const uint ID_BORDER_ITEM = 50000;
        private const uint ID_BORDER_EVENT = 60000;
        private const uint ID_BORDER_ANOMOB = 110000;
        private const uint ID_BORDER_SKILL = 120000;
        private const uint ID_BORDER2 = 0x3B9ACA00; //border for possession items

        private static uint nextPcId;

        private static readonly object registerlock = new object(); //注册对象时的锁

        private readonly Dictionary<uint, List<Actor>> actorsByRegion;


        private readonly Dictionary<string, ActorPC> pcByName;

        public MapInfo Info;
        private uint nextAnoMobID;
        private uint nextEventId;
        private uint nextGolemId;
        private uint nextItemId;
        private uint nextMobId;
        private uint nextPetId;
        private uint nextSkillID;

        public uint OriID;

        /* 创建副本地图返回客户端原始的地图ID相关*/
        public bool returnori = false;

        public Map(MapInfo info)
        {
            ID = info.id;
            Name = info.name;
            Width = info.width;
            Height = info.height;
            Info = info;

            Actors = new Dictionary<uint, Actor>();
            actorsByRegion = new Dictionary<uint, List<Actor>>();
            pcByName = new Dictionary<string, ActorPC>();
            if (nextPcId == 0)
                nextPcId = 0x10;
            nextMobId = ID_BORDER_MOB + 1;
            nextItemId = ID_BORDER_ITEM + 1;
            nextPetId = ID_BORDER_PET + 1;
            nextEventId = ID_BORDER_EVENT + 1;
            nextGolemId = ID_BORDER_GOLEM + 1;
            nextAnoMobID = ID_BORDER_ANOMOB + 1;
            nextSkillID = ID_BORDER_SKILL + 1;
        }

        public uint ID { get; set; }

        public string Name { get; }

        public ushort Width { get; }

        public ushort Height { get; }

        public Dictionary<uint, Actor> Actors { get; }


        public short[] GetRandomPos()
        {
            var ret = new short[2];

            ret[0] = (short)Global.Random.Next(-12700, +12700);
            ret[1] = (short)Global.Random.Next(-12700, +12700);

            return ret;
        }

        public short[] GetRandomPosAroundActor(Actor actor)
        {
            var ret = new short[2];

            ret[0] = (short)Global.Random.Next(actor.X - 100, actor.X + 100);
            ret[1] = (short)Global.Random.Next(actor.Y - 100, actor.Y + 100);

            return ret;
        }

        public short[] GetRandomPosAroundActor2(Actor actor)
        {
            var ret = new short[2];

            ret[0] = (short)Global.Random.Next(actor.X - 600, actor.X + 600);
            ret[1] = (short)Global.Random.Next(actor.Y - 600, actor.Y + 600);

            return ret;
        }

        public short[] GetRandomPosAroundPos(short x, short y, int range)
        {
            var ret = new short[2];
            byte new_x, new_y;
            var count = 0;
            do
            {
                if (count >= 1000)
                {
                    ret[0] = x;
                    ret[1] = y;
                    return ret;
                }

                ret[0] = (short)Global.Random.Next(x - range, x + range);
                ret[1] = (short)Global.Random.Next(y - range, y + range);
                new_x = Global.PosX16to8(ret[0], Width);
                new_y = Global.PosY16to8(ret[1], Height);
                count++;
                if (new_x >= Width)
                    new_x = (byte)(Width - 1);
                if (new_y >= Height)
                    new_y = (byte)(Height - 1);
            } while (Info.walkable[new_x, new_y] != 2);

            return ret;
        }

        public Actor GetActor(uint id)
        {
            try
            {
                return Actors[id];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActorPC GetPC(string name)
        {
            try
            {
                return pcByName[name];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActorPC GetPC(uint charID)
        {
            try
            {
                var chr = from c in pcByName.Values
                          where c.CharID == charID
                          select c;
                return Enumerable.First<ActorPC>(chr);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private uint GetNewActorID(ActorType type)
        {
            uint newID = 0;
            uint startID = 0;

            if (type == ActorType.PC)
            {
                newID = nextPcId;
                startID = nextPcId;
            }
            else
            {
                if (type == ActorType.MOB)
                {
                    newID = nextMobId;
                    startID = nextMobId;
                }
                else if (type == ActorType.PET || type == ActorType.SHADOW || type == ActorType.PARTNER)
                {
                    newID = nextPetId;
                    startID = nextPetId;
                }
                else if (type == ActorType.EVENT || type == ActorType.FURNITURE)
                {
                    newID = nextEventId;
                    startID = nextEventId;
                }
                else if (type == ActorType.GOLEM)
                {
                    newID = nextGolemId;
                    startID = nextGolemId;
                }
                else if (type == ActorType.ANOTHERMOB)
                {
                    newID = nextAnoMobID;
                    startID = nextAnoMobID;
                }
                else if (type == ActorType.SKILL)
                {
                    newID = nextSkillID;
                    startID = nextSkillID;
                }
                else
                {
                    newID = nextItemId;
                    startID = nextItemId;
                }
            }

            if (newID >= 10000 && type == ActorType.PC)
                newID = 16;

            if (newID >= 20000 && type == ActorType.MOB)
                newID = ID_BORDER_MOB + 1;
            if (newID >= 30000 && (type == ActorType.PET || type == ActorType.PARTNER))
                newID = ID_BORDER_PET + 1;
            if (newID >= 50000 && type == ActorType.GOLEM)
                newID = ID_BORDER_GOLEM + 1;
            if (newID >= 60000 && type == ActorType.ITEM)
                newID = ID_BORDER_ITEM + 1;
            if (newID >= 70000 && type == ActorType.EVENT)
                newID = ID_BORDER_EVENT + 1;
            if (newID >= 120000 && type == ActorType.ANOTHERMOB)
                newID = ID_BORDER_ANOMOB + 1;
            if (newID >= 140000 && type == ActorType.SKILL)
                newID = ID_BORDER_SKILL + 1;
            if (newID >= uint.MaxValue)
                newID = 1;

            while (Actors.ContainsKey(newID))
            {
                newID++;

                if (newID >= 10000 && type == ActorType.PC)
                    newID = 16;

                if (newID >= 20000 && type == ActorType.MOB)
                    newID = ID_BORDER_MOB + 1;
                if (newID >= 30000 && (type == ActorType.PET || type == ActorType.PARTNER))
                    newID = ID_BORDER_PET + 1;
                if (newID >= 50000 && type == ActorType.GOLEM)
                    newID = ID_BORDER_GOLEM + 1;
                if (newID >= 60000 && type == ActorType.ITEM)
                    newID = ID_BORDER_ITEM + 1;
                if (newID >= 70000 && type == ActorType.EVENT)
                    newID = ID_BORDER_EVENT + 1;
                if (newID >= 120000 && type == ActorType.ANOTHERMOB)
                    newID = ID_BORDER_ANOMOB + 1;
                if (newID >= 140000 && type == ActorType.SKILL)
                    newID = ID_BORDER_SKILL + 1;
                if (newID >= uint.MaxValue)
                    newID = 1;

                if (newID == startID) return 0;
            }

            if (type == ActorType.PC)
                nextPcId = newID + 1;
            else if (type == ActorType.MOB)
                nextMobId = newID + 1;
            else if (type == ActorType.PET || type == ActorType.PARTNER)
                nextPetId = newID + 1;
            else if (type == ActorType.FURNITURE || type == ActorType.EVENT)
                nextEventId = newID + 1;
            else if (type == ActorType.GOLEM)
                nextGolemId = newID + 1;
            else if (type == ActorType.ANOTHERMOB)
                nextAnoMobID = newID + 1;
            else if (type == ActorType.SKILL)
                nextSkillID = newID + 1;
            else
                nextItemId = newID + 1;


            return newID;
        }

        public bool RegisterActor(Actor nActor)
        {
            // default: no success
            var succes = false;

            // set the actorID and the actor's region on this map
            uint newID = 0;
            if (Global.clientMananger != null)
                ClientManager.EnterCriticalArea();
            if (nActor.type == ActorType.MOB && ((ActorMob)nActor).AnotherID != 0)
                newID = GetNewActorID(ActorType.ANOTHERMOB);
            else
                newID = GetNewActorID(nActor.type);
            if (Global.clientMananger != null)
                ClientManager.LeaveCriticalArea();
            if (nActor.type == ActorType.ITEM)
            {
                var item = (ActorItem)nActor;
                if (item.PossessionItem)
                    newID += ID_BORDER2;
            }

            if (newID != 0)
            {
                nActor.ActorID = newID;
                nActor.region = GetRegion(nActor.X, nActor.Y);

                if (GetRegionPlayerCount(nActor.region) == 0 && nActor.type == ActorType.PC)
                    MobAIToggle(nActor.region, true);
                // make the actor invisible (when the actor is ready: set it to false & call OnActorVisibilityChange)
                nActor.invisble = true;

                // add the new actor to the tables
                //DateTime time = DateTime.Now;
                if (Global.clientMananger != null)
                    ClientManager.EnterCriticalArea();
                lock (registerlock)
                {
                    try
                    {
                        while (Actors.ContainsKey(nActor.ActorID))
                        {
                            if (nActor.type == ActorType.MOB && ((ActorMob)nActor).AnotherID != 0)
                                nActor.ActorID = GetNewActorID(ActorType.ANOTHERMOB);
                            else
                                nActor.ActorID = GetNewActorID(nActor.type);
                            if (nActor.type == ActorType.ITEM && ((ActorItem)nActor).PossessionItem)
                                nActor.ActorID += ID_BORDER2;
                        }

                        Actors.Add(nActor.ActorID, nActor);
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                        Logger.ShowError("oh,fuck!");
                    }
                }

                if (Global.clientMananger != null)
                    ClientManager.LeaveCriticalArea();
                //double usedtime = (DateTime.Now - time).TotalMilliseconds;
                //if (usedtime > 0)
                //    Logger.ShowError("在地图:" + ID + " 注册ID: " + nActor.ActorID + " 花费时间:" + usedtime + "ms");

                if (nActor.type == ActorType.PC && !pcByName.ContainsKey(nActor.Name))
                    pcByName.Add(nActor.Name, (ActorPC)nActor);

                if (!actorsByRegion.ContainsKey(nActor.region))
                    actorsByRegion.Add(nActor.region, new List<Actor>());

                actorsByRegion[nActor.region].Add(nActor);

                succes = true;
            }

            nActor.MapID = ID;
            if (nActor.type == ActorType.PC)
            {
                var pc = (ActorPC)nActor;
                if (Info.Flag.Test(MapFlags.Wrp))
                    pc.Mode = PlayerMode.WRP;
                else if (pc.Mode == PlayerMode.KNIGHT_EAST || pc.Mode == PlayerMode.KNIGHT_FLOWER ||
                         pc.Mode == PlayerMode.KNIGHT_NORTH || pc.Mode == PlayerMode.KNIGHT_ROCK ||
                         pc.Mode == PlayerMode.KNIGHT_SOUTH || pc.Mode == PlayerMode.KNIGHT_WEST)
                    pc.Mode = pc.Mode;
                else
                    pc.Mode = PlayerMode.NORMAL;
            }

            nActor.e.OnCreate(succes);
            return succes;
        }

        public bool RegisterActor(Actor nActor, uint SessionID)
        {
            // default: no success
            var succes = false;

            // set the actorID and the actor's region on this map
            var newID = SessionID;

            if (newID != 0)
            {
                nActor.ActorID = newID;
                nActor.region = GetRegion(nActor.X, nActor.Y);
                if (GetRegionPlayerCount(nActor.region) == 0 && nActor.type == ActorType.PC)
                    MobAIToggle(nActor.region, true);

                // make the actor invisible (when the actor is ready: set it to false & call OnActorVisibilityChange)
                nActor.invisble = true;

                // add the new actor to the tables
                if (!Actors.ContainsKey(nActor.ActorID)) Actors.Add(nActor.ActorID, nActor);

                if (nActor.type == ActorType.PC && !pcByName.ContainsKey(nActor.Name))
                    pcByName.Add(nActor.Name, (ActorPC)nActor);

                if (!actorsByRegion.ContainsKey(nActor.region))
                    actorsByRegion.Add(nActor.region, new List<Actor>());

                actorsByRegion[nActor.region].Add(nActor);

                succes = true;
            }

            if (nActor.type == ActorType.PC)
            {
                var eh = (PCEventHandler)nActor.e;
                if (eh.Client.state != MapClient.SESSION_STATE.DISCONNECTED)
                {
                    eh.Client.state = MapClient.SESSION_STATE.LOADING;
                }
                else
                {
                    MapServer.charDB.SaveChar((ActorPC)nActor, false, false);
                    MapServer.accountDB.WriteUser(((ActorPC)nActor).Account);
                }
            }

            nActor.MapID = ID;
            if (nActor.type == ActorType.PC)
            {
                var pc = (ActorPC)nActor;
                if (Info.Flag.Test(MapFlags.Wrp))
                    pc.Mode = PlayerMode.WRP;
                else if (pc.Mode == PlayerMode.KNIGHT_EAST || pc.Mode == PlayerMode.KNIGHT_FLOWER ||
                         pc.Mode == PlayerMode.KNIGHT_NORTH || pc.Mode == PlayerMode.KNIGHT_ROCK ||
                         pc.Mode == PlayerMode.KNIGHT_SOUTH || pc.Mode == PlayerMode.KNIGHT_WEST)
                    pc.Mode = pc.Mode;
                else
                    pc.Mode = PlayerMode.NORMAL;
            }

            nActor.e.OnCreate(succes);
            return succes;
        }

        public void OnActorVisibilityChange(Actor dActor)
        {
            if (dActor.invisble)
            {
                dActor.invisble = false;
                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.DISAPPEAR, null, dActor, false);
                dActor.invisble = true;
            }

            else
            {
                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.APPEAR, null, dActor, false);
            }
        }

        public void DeleteActor(Actor dActor)
        {
            SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.DISAPPEAR, null, dActor, false);

            if (dActor.type == ActorType.PC && pcByName.ContainsKey(dActor.Name))
                pcByName.Remove(dActor.Name);
            //ClientManager.EnterCriticalArea();
            Actors.Remove(dActor.ActorID);

            if (actorsByRegion.ContainsKey(dActor.region))
            {
                actorsByRegion[dActor.region].Remove(dActor);
                if (GetRegionPlayerCount(dActor.region) == 0) MobAIToggle(dActor.region, false);
            }

            //ClientManager.LeaveCriticalArea();
            dActor.e.OnDelete();
            if (IsDungeon)
                if (DungeonMap.MapType == MapType.End)
                {
                    var count = 0;
                    foreach (var i in Actors.Values)
                        if (i.type == ActorType.MOB)
                            count++;
                    if (count == 0) DungeonFactory.Instance.GetDungeon(Creator.DungeonID).Destory(DestroyType.BossDown);
                }
        }

        public void MoveActor(MOVE_TYPE mType, Actor mActor, short[] pos, ushort dir, ushort speed)
        {
            MoveActor(mType, mActor, pos, dir, speed, false);
        }

        public void MoveActor(MOVE_TYPE mType, Actor mActor, short[] pos, ushort dir, ushort speed, bool sendToSelf)
        {
            MoveActor(mType, mActor, pos, dir, speed, sendToSelf, MoveType.RUN);
        }

        // make sure only 1 thread at a time is executing this method
        public void MoveActor(MOVE_TYPE mType, Actor mActor, short[] pos, ushort dir, ushort speed, bool sendToSelf,
            MoveType moveType)
        {
            //if (moveCounter >= 50)
            //    Logger.ShowDebug("Recurssion over 50 times!", Logger.defaultlogger);
            //Debug.Assert(moveCounter < 50, "Recurssion over 50 times!");
            //moveCounter++;
            try
            {
                var knockBack = false;
                if (mActor.Status != null)
                {
                    if (mActor.Status.Additions.ContainsKey("Meditatioon"))
                    {
                        mActor.Status.Additions["Meditatioon"].AdditionEnd();
                        mActor.Status.Additions.Remove("Meditatioon");
                    }

                    if (mActor.Status.Additions.ContainsKey("Hiding"))
                    {
                        mActor.Status.Additions["Hiding"].AdditionEnd();
                        mActor.Status.Additions.Remove("Hiding");
                    }

                    if (mActor.Status.Additions.ContainsKey("fish"))
                    {
                        mActor.Status.Additions["fish"].AdditionEnd();
                        mActor.Status.Additions.Remove("fish");
                    }

                    if (mActor.Status.Additions.ContainsKey("IAmTree"))
                    {
                        mActor.Status.Additions["IAmTree"].AdditionEnd();
                        mActor.Status.Additions.Remove("IAmTree");
                    }
                }

                // check wheter the destination is in range, if not kick the client
                if ( /*!this.MoveStepIsInRange(mActor, pos) ||*/ mActor.HP == 0 && mActor.type != ActorType.GOLEM &&
                                                                 mActor.type != ActorType.SKILL)
                {
                    pos = new short[2] { mActor.X, mActor.Y };
                    dir = 600;
                    knockBack = true;
                    sendToSelf = true;
                }

                if (mActor.type == ActorType.PC)
                {
                    var pc = (ActorPC)mActor;
                    if (pc.CInt["WaitEventID"] != 0)
                    {
                        MapClient.FromActorPC(pc).EventActivate((uint)pc.CInt["WaitEventID"]);
                        pc.CInt["WaitEventID"] = 0;
                    }

                    //坐地板状态与客户端不同步现象以及回血移动后不停止临时解决方案
                    pc.Buff.Sit = false;
                    pc.Motion = MotionType.STAND;
                    pc.MotionLoop = false;
                }

                if (mActor.type == ActorType.PC && !knockBack)
                {
                    var pc = (ActorPC)mActor;
                    var possessioned = pc.PossesionedActors;
                    foreach (var i in possessioned)
                    {
                        if (i == pc) continue;
                        if (i.MapID == mActor.MapID)
                            MoveActor(mType, i, pos, dir, speed);
                    }

                    if (pc.Online)
                    {
                        var client = MapClient.FromActorPC(pc);
                        if ((DateTime.Now - client.moveStamp).TotalSeconds >= 2)
                        {
                            if (client.Character.Party != null)
                                PartyManager.Instance.UpdateMemberPosition(client.Character.Party, client.Character);
                            client.moveStamp = DateTime.Now;
                        }
                    }
                }


                //scroll through all actors that "could" see the mActor at "from"
                //or are going "to see" mActor, or are still seeing mActor
                if (!knockBack)
                    for (short deltaY = -1; deltaY <= 1; deltaY++)
                        for (short deltaX = -1; deltaX <= 1; deltaX++)
                        {
                            var region = (uint)(mActor.region + deltaX * 10000 + deltaY);
                            if (!actorsByRegion.ContainsKey(region)) continue;

                            //ClientManager.EnterCriticalArea();
                            var list = actorsByRegion[region].ToArray();

                            //ClientManager.LeaveCriticalArea();
                            foreach (var actor in list)
                            {
                                if (actor.ActorID == mActor.ActorID && !sendToSelf) continue;
                                if (!actorsByRegion[region].Contains(actor))
                                    continue;
                                if (actor.Status == null)
                                {
                                    DeleteActor(actor);
                                    continue;
                                }

                                // A) INFORM OTHER ACTORS
                                //actor "could" see mActor at its "from" position
                                if (ACanSeeB(actor, mActor))
                                {
                                    //actor will still be able to see mActor
                                    if (ACanSeeB(actor, mActor, pos[0], pos[1]))
                                    {
                                        if (mType == MOVE_TYPE.START)
                                        {
                                            if (moveType != MoveType.RUN)
                                                actor.e.OnActorStartsMoving(mActor, pos, dir, speed, moveType);
                                            else
                                                actor.e.OnActorStartsMoving(mActor, pos, dir, speed);
                                        }
                                        else
                                        {
                                            actor.e.OnActorStopsMoving(mActor, pos, dir, speed);
                                        }
                                    }
                                    //actor won't be able to see mActor anymore
                                    else
                                    {
                                        actor.e.OnActorDisappears(mActor);
                                    }
                                }
                                //actor "could not" see mActor, but will be able to see him now
                                else if (ACanSeeB(actor, mActor, pos[0], pos[1]))
                                {
                                    actor.e.OnActorAppears(mActor);

                                    //send move / move stop
                                    if (mType == MOVE_TYPE.START)
                                    {
                                        if (moveType != MoveType.RUN)
                                            actor.e.OnActorStartsMoving(mActor, pos, dir, speed, moveType);
                                        else
                                            actor.e.OnActorStartsMoving(mActor, pos, dir, speed);
                                    }
                                    else
                                    {
                                        actor.e.OnActorStopsMoving(mActor, pos, dir, speed);
                                    }
                                }

                                // B) INFORM mActor
                                //mActor "could" see actor on its "from" position
                                if (ACanSeeB(mActor, actor))
                                {
                                    //mActor won't be able to see actor anymore
                                    if (!ACanSeeB(mActor, pos[0], pos[1], actor)) mActor.e.OnActorDisappears(actor);
                                    //mAactor will still be able to see actor
                                }

                                else if (ACanSeeB(mActor, pos[0], pos[1], actor))
                                {
                                    //mActor "could not" see actor, but will be able to see him now
                                    //send pcinfo
                                    mActor.e.OnActorAppears(actor);
                                }
                            }
                        }
                else
                    mActor.e.OnActorStopsMoving(mActor, pos, dir, speed);

                //update x/y/z/yaw of the actor    
                mActor.LastX = mActor.X;
                mActor.LastY = mActor.Y;
                mActor.X = pos[0];
                mActor.Y = pos[1];
                if (mActor.type == ActorType.FURNITURE) ((ActorFurniture)mActor).Z = pos[2];
                if (dir <= 360)
                    mActor.Dir = dir;

                //update the region of the actor
                var newRegion = GetRegion(pos[0], pos[1]);
                if (mActor.region != newRegion)
                {
                    actorsByRegion[mActor.region].Remove(mActor);
                    //turn off all the ai if the old region has no player on it
                    if (GetRegionPlayerCount(mActor.region) == 0) MobAIToggle(mActor.region, false);
                    mActor.region = newRegion;
                    if (GetRegionPlayerCount(mActor.region) == 0 && mActor.type == ActorType.PC)
                        MobAIToggle(mActor.region, true);

                    if (!actorsByRegion.ContainsKey(newRegion))
                        actorsByRegion.Add(newRegion, new List<Actor>());

                    actorsByRegion[newRegion].Add(mActor);
                }
            }

            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
            //moveCounter--;
        }

        public int GetRegionPlayerCount(uint region)
        {
            List<Actor> actors;
            var count = 0;
            if (!actorsByRegion.ContainsKey(region)) return 0;
            actors = actorsByRegion[region];
            var removelist = new List<int>();
            for (var i = 0; i < actors.Count; i++)
            {
                Actor actor;
                if (actors[i] == null)
                {
                    removelist.Add(i);
                    continue;
                }

                actor = actors[i];
                if (actor.type == ActorType.PC) count++;
            }

            foreach (var i in removelist) actors.RemoveAt(i);
            return count;
        }

        public void MobAIToggle(uint region, bool toggle)
        {
            /*
            List<Actor> actors;
            if (!this.actorsByRegion.ContainsKey(region)) return;
            actors = this.actorsByRegion[region];
            foreach (Actor actor in actors)
            {
                if (actor.type == ActorType.MOB)
                {
                    ActorMob npc = (ActorMob)actor;

                    ActorEventHandlers.MobEventHandler mob = (ActorEventHandlers.MobEventHandler)npc.e;
                    if (mob.AI == null) continue;
                    switch (toggle)
                    {
                        case true:
                            if (mob.AI.Activated) continue;
                            mob.AI.Start();
                            break;
                        case false:
                            if (mob.AI.Hate.Count != 0) continue;
                            mob.AI.Pause();
                            break;
                    }
                }
            }*/
        }

        public bool MoveStepIsInRange(Actor mActor, short[] to)
        {
            if (mActor.type == ActorType.PC)
            {
                var pc = (ActorPC)mActor;
                var client = MapClient.FromActorPC(pc);
                if (client.AI != null)
                    if (client.AI.Activated)
                        return true;
                var span = DateTime.Now - client.moveCheckStamp;
                //if (span.TotalMilliseconds > 50)
                {
                    double maximal;
                    if (span.TotalMilliseconds > 1000)
                        maximal = mActor.Speed * 2f;
                    else if (span.TotalMilliseconds > 100)
                        maximal = mActor.Speed * (span.TotalMilliseconds / 1000) * 5f;
                    else
                        maximal = mActor.Speed * 0.5f;
                    // Disabled, until we have something better
                    if (Math.Abs(mActor.X - to[0]) > maximal)
                        return false;
                    if (Math.Abs(mActor.Y - to[1]) > maximal)
                        return false;
                    //we don't check for z , yet, to allow falling from great hight
                    //if (System.Math.Abs(mActor.z - to[2]) > mActor.maxMoveRange) return false;
                }
            }

            return true;
        }


        public uint GetRegion(float x, float y)
        {
            var REGION_DIAMETER = Global.MAX_SIGHT_RANGE * 2;

            // best case we should now load the size of the map from a config file, however that's not
            // possible yet, so we just create a region code off the values x/y

            /*
            values off x/y are like:
            x = -20 500.0f
            y =   1 000.0f

            before we convert them to uints we make them positive, and store the info wheter they were negative
            x  = - 25 000.0f;
            nx = 1;
            y  =  1 000.0f;
            ny = 0;

            no we convert them to uints

            ux = 25 000;
            nx = 1;
            uy =  1 000;
            ny = 0;

            now we do ux = (uint) ( ux / REGION_DIAMETER ) [the same for uy]
            we have:

            ux = 2;
            nx = 1;
            uy = 0;
            ny = 0;

            off this data we generate the region code:
             > we use a uint as region code
             > max value of an uint32 is 4 294 967 295
             > the syntax of the region code is ux[5digits].uy[5digits]
             if(!nx) ux = ux + 50000;
             else ux = 50000 - ux;
             if(!ny) uy = uy + 50000;
             else uy = 50000 - uy;

            uint regionCode = 49998 50001
            uint regionCode = 4999850001

            Note:
             We inform an Actor(Player) about all other Actors in its own region and the 8 regions around
             this region. Because of this REGION_DIAMETER has to be MAX_SIGHT_RANGE (or greater).
             Also check SVN/SagaMap/doc/mapRegions.bmp
            */
            // init nx,ny
            var nx = false;
            var ny = false;
            // make x,y positive
            if (x < 0)
            {
                x = x - 2 * x;
                nx = true;
            }

            if (y < 0)
            {
                y = y - 2 * y;
                ny = true;
            }

            // convert x,y to uints
            var ux = (uint)x;
            var uy = (uint)y;
            // divide through REGION_DIAMETER
            ux = ux / REGION_DIAMETER;
            uy = uy / REGION_DIAMETER;
            // calc ux
            if (ux > 4999) ux = 4999;
            if (!nx) ux = ux + 5000;
            else ux = 5000 - ux;
            // calc uy
            if (uy > 4999) uy = 4999;
            if (!ny) uy = uy + 5000;
            else uy = 5000 - uy;
            // finally generate the region code and return it
            return ux * 10000 + uy;
        }

        public bool ACanSeeB(Actor A, Actor B)
        {
            if (A == null || B == null)
                return false;
            if (B.invisble) return false;
            if (Math.Abs(A.X - B.X) > A.sightRange) return false;
            if (Math.Abs(A.Y - B.Y) > A.sightRange) return false;
            return true;
        }

        public bool ACanSeeB(Actor A, Actor B, float bx, float by)
        {
            if (A == null || B == null)
                return false;
            if (B.invisble) return false;
            if (Math.Abs(A.X - bx) > A.sightRange) return false;
            if (Math.Abs(A.Y - by) > A.sightRange) return false;
            return true;
        }

        public bool ACanSeeB(Actor A, float ax, float ay, Actor B)
        {
            if (A == null || B == null)
                return false;
            if (B.invisble) return false;
            if (Math.Abs(ax - B.X) > A.sightRange) return false;
            if (Math.Abs(ay - B.Y) > A.sightRange) return false;
            return true;
        }

        public bool ACanSeeB(Actor A, Actor B, float sightrange)
        {
            if (A == null || B == null)
                return false;
            if (B.invisble) return false;
            if (Math.Abs(A.X - B.X) > sightrange) return false;
            if (Math.Abs(A.Y - B.Y) > sightrange) return false;
            return true;
        }

        public void SendVisibleActorsToActor(Actor jActor)
        {
            //search all actors which can be seen by jActor and tell jActor about them
            for (short deltaY = -1; deltaY <= 1; deltaY++)
                for (short deltaX = -1; deltaX <= 1; deltaX++)
                {
                    var region = (uint)(jActor.region + deltaX * 10000 + deltaY);
                    if (!actorsByRegion.ContainsKey(region)) continue;
                    var list = actorsByRegion[region].ToArray();
                    var listAF = new List<Actor>();
                    foreach (var actor in list)
                        try
                        {
                            if (actor.ActorID == jActor.ActorID) continue;
                            if (actor.Status == null)
                            {
                                DeleteActor(actor);
                                continue;
                            }

                            //check wheter jActor can see actor, if yes: inform jActor
                            if (ACanSeeB(jActor, actor))
                            {
                                if (actor.type == ActorType.FURNITURE &&
                                    ItemFactory.Instance.GetItem(((ActorFurniture)actor).ItemID).BaseData.itemType !=
                                    ItemType.FF_CASTLE && ID > 90001000)
                                    listAF.Add(actor);
                                else
                                    jActor.e.OnActorAppears(actor);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ShowError(ex);
                        }

                    if (listAF.Count > 0)
                    {
                        var afs = new List<ActorFurniture>();
                        var index = 0;
                        foreach (var i in listAF)
                        {
                            if (index >= 40)
                            {
                                jActor.e.OnActorFurnitureList(afs);
                                afs.Clear();
                                index = 0;
                            }

                            afs.Add((ActorFurniture)i);
                            index++;
                        }

                        if (afs.Count > 0)
                            jActor.e.OnActorFurnitureList(afs);
                    }
                }
        }

        public void TeleportActor(Actor sActor, short x, short y)
        {
            if (sActor.Status.Additions.ContainsKey("Meditatioon"))
            {
                sActor.Status.Additions["Meditatioon"].AdditionEnd();
                sActor.Status.Additions.Remove("Meditatioon");
            }

            if (sActor.Status.Additions.ContainsKey("Hiding"))
            {
                sActor.Status.Additions["Hiding"].AdditionEnd();
                sActor.Status.Additions.Remove("Hiding");
            }

            if (sActor.Status.Additions.ContainsKey("fish"))
            {
                sActor.Status.Additions["fish"].AdditionEnd();
                sActor.Status.Additions.Remove("fish");
            }

            if (sActor.Status.Additions.ContainsKey("Cloaking"))
            {
                sActor.Status.Additions["Cloaking"].AdditionEnd();
                sActor.Status.Additions.Remove("Cloaking");
            }

            if (sActor.Status.Additions.ContainsKey("IAmTree"))
            {
                sActor.Status.Additions["IAmTree"].AdditionEnd();
                sActor.Status.Additions.Remove("IAmTree");
            }

            if (sActor.Status.Additions.ContainsKey("Invisible"))
            {
                sActor.Status.Additions["Invisible"].AdditionEnd();
                sActor.Status.Additions.Remove("Invisible");
            }

            if (sActor.HP == 0)
                return;
            if (sActor.type != ActorType.PC)
                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.DISAPPEAR, null, sActor, false);

            actorsByRegion[sActor.region].Remove(sActor);
            if (GetRegionPlayerCount(sActor.region) == 0) MobAIToggle(sActor.region, false);

            sActor.X = x;
            sActor.Y = y;
            sActor.region = GetRegion(x, y);
            if (GetRegionPlayerCount(sActor.region) == 0 && sActor.type == ActorType.PC)
                MobAIToggle(sActor.region, true);

            if (!actorsByRegion.ContainsKey(sActor.region)) actorsByRegion.Add(sActor.region, new List<Actor>());
            actorsByRegion[sActor.region].Add(sActor);

            sActor.e.OnTeleport(x, y);
            if (sActor.type != ActorType.PC)
            {
                SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.APPEAR, null, sActor, false);
            }
            else
            {
                var pos = new short[2];
                pos[0] = x;
                pos[1] = y;
                MoveActor(MOVE_TYPE.START, sActor, pos, sActor.Dir, sActor.Speed, false, MoveType.VANISH2);
            }

            SendVisibleActorsToActor(sActor);
        }

        public void SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE etype, MapEventArgs args, Actor sActor,
            bool sendToSourceActor)
        {
            try
            {
                for (short deltaY = -1; deltaY <= 1; deltaY++)
                    for (short deltaX = -1; deltaX <= 1; deltaX++)
                    {
                        var region = (uint)(sActor.region + deltaX * 10000 + deltaY);
                        if (!actorsByRegion.ContainsKey(region)) continue;
                        var actors = actorsByRegion[region].ToArray();
                        foreach (var actor in actors)
                            try
                            {
                                if (!sendToSourceActor && actor.ActorID == sActor.ActorID) continue;
                                if (actor.Status == null)
                                {
                                    if (etype != EVENT_TYPE.DISAPPEAR)
                                        DeleteActor(actor);
                                    continue;
                                }

                                if (ACanSeeB(actor, sActor))
                                    switch (etype)
                                    {
                                        case EVENT_TYPE.PLAYERSHOP_CHANGE:
                                            if (sActor.type == ActorType.PC || sActor.type == ActorType.EVENT)
                                                actor.e.OnPlayerShopChange(sActor);
                                            break;
                                        case EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE:
                                            if (sActor.type == ActorType.PC || sActor.type == ActorType.EVENT)
                                                actor.e.OnPlayerShopChangeClose(sActor);
                                            break;

                                        case EVENT_TYPE.PLAYER_SIZE_UPDATE:
                                            actor.e.OnPlayerSizeChange(sActor);
                                            break;


                                        case EVENT_TYPE.CHAR_INFO_UPDATE:
                                            actor.e.OnCharInfoUpdate(sActor);
                                            break;

                                        case EVENT_TYPE.CHANGE_STATUS:
                                            if (sActor.type != ActorType.PC)
                                                break;

                                            actor.e.OnPlayerChangeStatus((ActorPC)sActor);
                                            break;

                                        case EVENT_TYPE.APPEAR:
                                            actor.e.OnActorAppears(sActor);
                                            break;

                                        case EVENT_TYPE.DISAPPEAR:
                                            actor.e.OnActorDisappears(sActor);
                                            break;

                                        case EVENT_TYPE.EMOTION:
                                            actor.e.OnActorChangeEmotion(sActor, args);
                                            break;

                                        case EVENT_TYPE.MOTION:
                                            actor.e.OnActorChangeMotion(sActor, args);
                                            break;

                                        case EVENT_TYPE.WAITTYPE:
                                            actor.e.OnActorChangeWaitType(sActor);
                                            break;

                                        case EVENT_TYPE.CHAT:
                                            actor.e.OnActorChat(sActor, args);
                                            break;

                                        case EVENT_TYPE.SKILL:
                                            actor.e.OnActorSkillUse(sActor, args);
                                            break;

                                        case EVENT_TYPE.CHANGE_EQUIP:
                                            actor.e.OnActorChangeEquip(sActor, args);
                                            break;
                                        case EVENT_TYPE.ATTACK:
                                            actor.e.OnAttack(sActor, args);
                                            break;
                                        case EVENT_TYPE.HPMPSP_UPDATE:
                                            actor.e.OnHPMPSPUpdate(sActor);
                                            break;
                                        case EVENT_TYPE.BUFF_CHANGE:
                                            actor.e.OnActorChangeBuff(sActor);
                                            break;
                                        case EVENT_TYPE.LEVEL_UP:
                                            actor.e.OnLevelUp(sActor, args);
                                            break;
                                        case EVENT_TYPE.PLAYER_MODE:
                                            actor.e.OnPlayerMode(sActor);
                                            break;
                                        case EVENT_TYPE.SHOW_EFFECT:
                                            actor.e.OnShowEffect(sActor, args);
                                            break;
                                        case EVENT_TYPE.POSSESSION:
                                            actor.e.OnActorPossession(sActor, args);
                                            break;
                                        case EVENT_TYPE.PARTY_NAME_UPDATE:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorPartyUpdate((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.SPEED_UPDATE:
                                            actor.e.OnActorSpeedChange(sActor);
                                            break;
                                        case EVENT_TYPE.SIGN_UPDATE:
                                            if (sActor.type == ActorType.PC || sActor.type == ActorType.EVENT)
                                                actor.e.OnSignUpdate(sActor);
                                            break;
                                        case EVENT_TYPE.RING_NAME_UPDATE:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorRingUpdate((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.WRP_RANKING_UPDATE:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorWRPRankingUpdate((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.ATTACK_TYPE_CHANGE:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorChangeAttackType((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.FURNITURE_SIT:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorFurnitureSit((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.PAPER_CHANGE:
                                            if (sActor.type != ActorType.PC)
                                                break;
                                            actor.e.OnActorPaperChange((ActorPC)sActor);
                                            break;
                                        case EVENT_TYPE.SKILL_CANCEL:
                                            actor.e.OnActorSkillCancel(sActor);
                                            break;
                                    }
                            }
                            catch (Exception ex)
                            {
                                Logger.ShowError(ex);
                            }
                    }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void SendEventToAllActors(TOALL_EVENT_TYPE etype, MapEventArgs args, Actor sActor,
            bool sendToSourceActor)
        {
            foreach (var actor in Actors.Values)
            {
                if (sActor != null)
                    if (!sendToSourceActor && actor.ActorID == sActor.ActorID)
                        continue;

                switch (etype)
                {
                    case TOALL_EVENT_TYPE.CHAT:
                        actor.e.OnActorChat(sActor, args);
                        break;
                }
            }
        }

        public void SendActorToMap(Actor mActor, Map newMap, short x, short y)
        {
            SendActorToMap(mActor, newMap, x, y, false);
        }

        public void SendActorToMap(Actor mActor, Map newMap, short x, short y, bool possession)
        {
            // todo: add support for multiple map servers
            if (mActor.Status.Additions.ContainsKey("Meditatioon"))
            {
                mActor.Status.Additions["Meditatioon"].AdditionEnd();
                mActor.Status.Additions.Remove("Meditatioon");
            }

            if (mActor.Status.Additions.ContainsKey("Hiding"))
            {
                mActor.Status.Additions["Hiding"].AdditionEnd();
                mActor.Status.Additions.Remove("Hiding");
            }

            if (mActor.Status.Additions.ContainsKey("fish"))
            {
                mActor.Status.Additions["fish"].AdditionEnd();
                mActor.Status.Additions.Remove("fish");
            }

            if (mActor.Status.Additions.ContainsKey("Cloaking"))
            {
                mActor.Status.Additions["Cloaking"].AdditionEnd();
                mActor.Status.Additions.Remove("Cloaking");
            }

            if (mActor.Status.Additions.ContainsKey("IAmTree"))
            {
                mActor.Status.Additions["IAmTree"].AdditionEnd();
                mActor.Status.Additions.Remove("IAmTree");
            }

            if (mActor.Status.Additions.ContainsKey("Invisible"))
            {
                mActor.Status.Additions["Invisible"].AdditionEnd();
                mActor.Status.Additions.Remove("Invisible");
            }

            if (mActor.HP == 0)
                return;
            //send also the possessioned actors to the same map
            if (mActor.type == ActorType.PC)
            {
                var pc = (ActorPC)mActor;
                if (pc.PossessionTarget != 0 && !possession)
                    return;
                var possessioned = pc.PossesionedActors;
                foreach (var i in possessioned)
                {
                    if (i == pc) continue;
                    SendActorToMap(i, newMap, x, y);
                }
            }

            // obtain the new map
            var mapid = (byte)newMap.ID;
            if (mapid == mActor.MapID)
            {
                TeleportActor(mActor, x, y);
                return;
            }

            // delete the actor from this map
            DeleteActor(mActor);

            // update the actor
            mActor.MapID = mapid;
            mActor.X = x;
            mActor.Y = y;
            mActor.Buff.Dead = false;

            // register the actor in the new map
            if (mActor.type != ActorType.PC)
            {
                newMap.RegisterActor(mActor);
            }
            else
            {
                ((ActorPC)mActor).Motion = MotionType.STAND;
                newMap.RegisterActor(mActor, mActor.ActorID);
                var client = MapClient.FromActorPC((ActorPC)mActor);
                if (client.AI != null) client.AI.map = newMap;
            }
        }

        public void SendActorToMap(Actor mActor, uint mapid, short x, short y)
        {
            SendActorToMap(mActor, mapid, x, y, false);
        }

        public void SendActorToMap(Actor mActor, uint mapid, short x, short y, bool possession)
        {
            // todo: add support for multiple map servers
            if (mActor.Status.Additions.ContainsKey("Meditatioon"))
            {
                mActor.Status.Additions["Meditatioon"].AdditionEnd();
                mActor.Status.Additions.Remove("Meditatioon");
            }

            if (mActor.Status.Additions.ContainsKey("Hiding"))
            {
                mActor.Status.Additions["Hiding"].AdditionEnd();
                mActor.Status.Additions.Remove("Hiding");
            }

            if (mActor.Status.Additions.ContainsKey("fish"))
            {
                mActor.Status.Additions["fish"].AdditionEnd();
                mActor.Status.Additions.Remove("fish");
            }

            if (mActor.Status.Additions.ContainsKey("Cloaking"))
            {
                mActor.Status.Additions["Cloaking"].AdditionEnd();
                mActor.Status.Additions.Remove("Cloaking");
            }

            if (mActor.Status.Additions.ContainsKey("IAmTree"))
            {
                mActor.Status.Additions["IAmTree"].AdditionEnd();
                mActor.Status.Additions.Remove("IAmTree");
            }

            if (mActor.Status.Additions.ContainsKey("Invisible"))
            {
                mActor.Status.Additions["Invisible"].AdditionEnd();
                mActor.Status.Additions.Remove("Invisible");
            }

            if (mActor.HP == 0)
                return;

            if (mActor.type == ActorType.PC)
            {
                var pc = (ActorPC)mActor;
                if (pc.PossessionTarget != 0 && !possession)
                    return;
                var possessioned = pc.PossesionedActors;
                foreach (var i in possessioned)
                {
                    if (i == pc) continue;
                    SendActorToMap(i, mapid, x, y, true);
                }
            }

            // obtain the new map
            Map newMap;
            if (mapid == mActor.MapID)
            {
                TeleportActor(mActor, x, y);
                return;
            }

            newMap = MapManager.Instance.GetMap(mapid);
            if (newMap == null)
                return;
            // delete the actor from this map
            DeleteActor(mActor);
            if (x == 0f && y == 0f)
            {
                var pos = newMap.GetRandomPos();
                x = pos[0];
                y = pos[1];
            }

            // update the actor
            mActor.MapID = mapid;
            mActor.X = x;
            mActor.Y = y;
            mActor.Buff.Dead = false;

            // register the actor in the new map
            if (mActor.type != ActorType.PC)
            {
                newMap.RegisterActor(mActor);
            }
            else
            {
                ((ActorPC)mActor).Motion = MotionType.STAND;
                newMap.RegisterActor(mActor, mActor.ActorID);
                var client = MapClient.FromActorPC((ActorPC)mActor);
                if (client.AI != null) client.AI.map = newMap;
            }
        }

        private void SendActorToActor(Actor mActor, Actor tActor)
        {
            if (mActor.MapID == tActor.MapID)
                TeleportActor(mActor, tActor.X, tActor.Y);
            else
                SendActorToMap(mActor, tActor.MapID, tActor.X, tActor.Y);
        }

        /// <summary>
        ///     计算两目标之间的距离
        /// </summary>
        /// <param name="sActor">目标1</param>
        /// <param name="dActor">目标2</param>
        /// <returns></returns>
        public static short Distance(Actor sActor, Actor dActor)
        {
            return (short)Math.Sqrt((dActor.X - sActor.X) * (dActor.X - sActor.X) +
                                    (dActor.Y - sActor.Y) * (dActor.Y - sActor.Y));
        }

        public void Announce(string text)
        {
            var list = Actors.Values.ToList();
            foreach (var i in list)
                if (i.type == ActorType.PC)
                    MapClient.FromActorPC((ActorPC)i).SendAnnounce(text);
        }

        public void FindFreeCoord(short x, short y, out short x2, out short y2, params Actor[] excludes)
        {
            if (GetActorsArea(x, y, 8, excludes).Count == 0)
            {
                x2 = x;
                y2 = y;
                return;
            }

            var X = (short)Global.Random.Next(-100, 100);
            var Y = (short)Global.Random.Next(-100, 100);
            for (var i = X; i < 200; i += 100)
                for (var j = Y; j < 200; j += 100)
                    if (GetActorsArea((short)(x + i), (short)(y + j), 8, excludes).Count == 0)
                    {
                        x2 = (short)(x + i);
                        y2 = (short)(y + j);
                        return;
                    }

            x2 = x;
            y2 = y;
        }

        /// <summary>
        ///     计算2点之间的夹角
        /// </summary>
        /// <param name="x">原点X</param>
        /// <param name="y">原点Y</param>
        /// <param name="x2">目标点X</param>
        /// <param name="y2">目标点Y</param>
        /// 都是actor坐标 不是地图坐标！！！
        /// <returns></returns>
        public ushort CalcDir(short x, short y, short x2, short y2)
        {
            var vecX = (short)(x2 - x);
            var vecY = (short)(y2 - y);
            //注意注意：actor坐标y和地图方向是反的！反的！反的！
            if (vecX < 0) return (ushort)(Math.Acos(-vecY / Math.Sqrt(vecX * vecX + vecY * vecY)) / Math.PI * 180);

            return (ushort)(360 - Math.Acos(-vecY / Math.Sqrt(vecX * vecX + vecY * vecY)) / Math.PI * 180);
        }

        //换算成平面直角坐标系的角度..
        public ushort DirChange(ushort dir)
        {
            var d = 270 - dir;
            if (d < 0)
                d += 360;
            return (ushort)d;
        }

        public List<Actor> GetActorsArea(Actor sActor, short range, bool includeSourceActor)
        {
            return GetActorsArea(sActor, range, includeSourceActor, true);
        }

        public List<Actor> GetActorsArea(Actor sActor, short range, bool includeSourceActor, bool includeInvisibleActor)
        {
            var actors = new List<Actor>();
            for (short deltaY = -1; deltaY <= 1; deltaY++)
                for (short deltaX = -1; deltaX <= 1; deltaX++)
                {
                    var region = (uint)(GetRegion(sActor.X, sActor.Y) + deltaX * 1000000 + deltaY);
                    if (!actorsByRegion.ContainsKey(region)) continue;

                    var list = actorsByRegion[region].ToArray();
                    foreach (var actor in list)
                    {
                        if (!includeSourceActor && actor.ActorID == sActor.ActorID) continue;
                        if (!includeInvisibleActor && actor.Buff.Transparent) continue;

                        if (ACanSeeB(actor, sActor, range)) actors.Add(actor);
                    }
                }

            return actors;
        }

        public Actor GetRandomAreaActor(Actor sActor, short range)
        {
            var actors = GetActorsArea(sActor, range, false, false);
            if (actors.Count == 0) return null;
            var num = Global.Random.Next(0, actors.Count - 1);
            return actors[num];
        }

        //获得路程长度
        public double GetLengthD(short x, short y, short x2, short y2)
        {
            return Math.Sqrt((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        }

        //计算三角形面积，其他函数用
        public double TriangleArea(double a, double b, double c)
        {
            var p = (a + b + c) / 2;
            return Math.Sqrt(p * Math.Abs(p - a) * Math.Abs(p - b) * Math.Abs(p - c));
        }

        //获取任意矩形内角色列表
        public List<Actor> GetRectAreaActors(short x1, short y1, short x2, short y2, short x3, short y3, short x4,
            short y4, bool includeInvisibleActor = false)
        {
            var actors = new List<Actor>();
            var list = Actors.Values.ToArray();
            var a = GetLengthD(x1, y1, x2, y2);
            var b = GetLengthD(x2, y2, x3, y3);
            var ab = GetLengthD(x1, y1, x3, y3);
            var c = GetLengthD(x3, y3, x4, y4);
            var d = GetLengthD(x4, y4, x1, y1);

            var area = TriangleArea(a, b, ab) + TriangleArea(c, d, ab);
            double e, f, g, h;

            foreach (var actor in list)
            {
                if (actor == null)
                    continue;
                if (!includeInvisibleActor && actor.Buff.Transparent) continue;
                e = GetLengthD(actor.X, actor.Y, x1, y1);
                f = GetLengthD(actor.X, actor.Y, x2, y2);
                g = GetLengthD(actor.X, actor.Y, x3, y3);
                h = GetLengthD(actor.X, actor.Y, x4, y4);
                var dd = TriangleArea(a, e, f) + TriangleArea(b, f, g) + TriangleArea(c, g, h) + TriangleArea(d, h, e);
                if (TriangleArea(a, e, f) + TriangleArea(b, f, g) + TriangleArea(c, g, h) + TriangleArea(d, h, e)
                    <= area + 1)
                    actors.Add(actor);
            }

            return actors;
        }

        //真·圆形判定
        public List<Actor> GetRoundAreaActors(short x, short y, short range, bool includeInvisibleActor = false)
        {
            var actors = new List<Actor>();
            for (short deltaY = -1; deltaY <= 1; deltaY++)
                for (short deltaX = -1; deltaX <= 1; deltaX++)
                {
                    var region = (uint)(GetRegion(x, y) + deltaX * 1000000 + deltaY);
                    if (!actorsByRegion.ContainsKey(region)) continue;

                    var list = actorsByRegion[region].ToArray();
                    foreach (var actor in list)
                    {
                        if (actor == null)
                            continue;
                        if (!includeInvisibleActor && actor.Buff.Transparent) continue;

                        if ((actor.X - x) * (actor.X - x) + (actor.Y - y) * (actor.Y - y) <= range * range)
                            actors.Add(actor);
                    }
                }

            return actors;
        }

        public List<Actor> GetActorsArea(short x, short y, short range, params Actor[] excludes)
        {
            return GetActorsArea(x, y, range, true, excludes);
        }

        public List<Actor> GetActorsArea(short x, short y, short range, bool includeInvisibleActor,
            params Actor[] excludes)
        {
            var actors = new List<Actor>();
            for (short deltaY = -1; deltaY <= 1; deltaY++)
                for (short deltaX = -1; deltaX <= 1; deltaX++)
                {
                    var region = (uint)(GetRegion(x, y) + deltaX * 1000000 + deltaY);
                    if (!actorsByRegion.ContainsKey(region)) continue;

                    var list = actorsByRegion[region].ToArray();
                    foreach (var actor in list)
                    {
                        var skip = false;
                        if (excludes != null)
                            foreach (var j in excludes)
                                if (actor == j)
                                    skip = true;

                        if (actor == null)
                            continue;
                        if (skip) continue;
                        if (!includeInvisibleActor && actor.Buff.Transparent) continue;

                        if (actor.X >= x - range && actor.X <= x + range && actor.Y >= y - range && actor.Y <= y + range)
                            actors.Add(actor);
                    }
                }

            return actors;
        }

        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, byte x, byte y, int range, int count, int delay,
            ActorMob.MobInfo mobinfo, AIMode Ai)
        {
            return SpawnCustomMob(MobID, MapID, 0, 0, 0, 0, x, y, range, count, delay, mobinfo, Ai, null, 0);
        }

        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, byte x, byte y, int range, int count, int delay,
            ActorMob.MobInfo mobinfo, AIMode Ai, MobCallback Event, byte Callbacktype)
        {
            return SpawnCustomMob(MobID, MapID, 0, 0, 0, 0, x, y, range, count, delay, mobinfo, Ai, Event,
                Callbacktype);
        }

        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, uint PictID, uint AnotherID, byte AnotherCamp,
            byte x, byte y, int range, int count, int delay, ActorMob.MobInfo mobinfo, AIMode Ai, MobCallback Event,
            byte Callbacktype)
        {
            return SpawnCustomMob(MobID, MapID, PictID, 0, AnotherID, AnotherCamp, x, y, range, count, delay, mobinfo,
                Ai, Event, Callbacktype);
        }

        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, uint PictID, uint RideID, uint AnotherID,
            byte AnotherCamp, byte x, byte y, int range, int count, int delay, ActorMob.MobInfo mobinfo, AIMode Ai,
            MobCallback Event, byte Callbacktype)
        {
            return SpawnCustomMob(MobID, MapID, PictID, RideID, AnotherID, AnotherCamp, x, y, range, count, delay,
                mobinfo, Ai, Event, Callbacktype, false);
        }

        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, uint PictID, uint RideID, uint AnotherID,
            byte AnotherCamp, byte x, byte y, int range, int count, int delay, ActorMob.MobInfo mobinfo, AIMode Ai,
            MobCallback Event, byte Callbacktype, bool noreturn)
        {
            return SpawnCustomMob(MobID, MapID, PictID, RideID, AnotherID, AnotherCamp, x, y, range, count, delay,
                mobinfo, Ai, Event, Callbacktype, false, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="MobID"></param>
        /// <param name="MapID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="range"></param>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        /// <param name="mobinfo"></param>
        /// <param name="Ai"></param>
        /// <param name="Event"></param>
        /// <param name="Callbacktype">1=死亡事件 2=怪物技能使用时事件 3=怪物移动时事件 4=怪物攻击时事件 5=被攻击时事件</param>
        /// <returns></returns>
        public List<ActorMob> SpawnCustomMob(uint MobID, uint MapID, uint PictID, uint RideID, uint AnotherID,
            byte AnotherCamp, byte x, byte y, int range, int count, int delay, ActorMob.MobInfo mobinfo, AIMode Ai,
            MobCallback Event, byte Callbacktype, bool noreturn, bool NoAIForNPC)
        {
            try
            {
                //Map map = MapManager.Instance.GetMap(MapID);
                var mobs = new List<ActorMob>();
                for (var i = 0; i < count; i++)
                {
                    if (ID == 10054001)
                        AnotherID = 0;
                    var mob = new ActorMob(MobID, mobinfo);
                    if (mobinfo.level != 0)
                        mob.Level = mobinfo.level;
                    mob.MapID = MapID;
                    mob.PictID = PictID;
                    mob.Camp = AnotherCamp;
                    mob.AnotherID = AnotherID;
                    mob.RideID = RideID;
                    //mob.AnotherMark = 1;
                    //if (map == null) continue;
                    int min_x, max_x, min_y, max_y;
                    min_x = x - range;
                    max_x = x + range;
                    min_y = y - range;
                    max_y = y + range;
                    if (min_x < 0) min_x = 0;
                    if (max_x >= Width)
                        max_x = Width - 1;
                    if (min_y < 0) min_y = 0;
                    if (max_y >= Height)
                        max_y = Height - 1;
                    int x_new, y_new;
                    x_new = (byte)Global.Random.Next(min_x, max_x);
                    y_new = (byte)Global.Random.Next(min_y, max_y);

                    var counter = 0;
                    try
                    {
                        while (Info.walkable[x_new, y_new] != 2)
                        {
                            if (counter > 1000 || range == 0) break;
                            x_new = (byte)Global.Random.Next(min_x, max_x);
                            y_new = (byte)Global.Random.Next(min_y, max_y);
                            counter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }

                    if (counter > 1000)
                        continue;
                    mob.X = Global.PosX8to16((byte)x_new, Width);
                    mob.Y = Global.PosY8to16((byte)y_new, Height);
                    mob.Dir = (ushort)Global.Random.Next(0, 7);
                    var eh = new MobEventHandler(mob);
                    mob.e = eh;
                    if (Ai != null)
                        eh.AI.Mode = Ai;
                    else eh.AI.Mode = new AIMode(0);
                    eh.AI.X_Ori = Global.PosX8to16(x, Width);
                    eh.AI.Y_Ori = Global.PosY8to16(y, Height);
                    eh.AI.X_Spawn = mob.X;
                    eh.AI.Y_Spawn = mob.Y;
                    eh.AI.MoveRange = (short)(range * 100);
                    eh.AI.SpawnDelay = delay * 1000;
                    RegisterActor(mob);
                    mob.invisble = false;
                    mob.sightRange = 2500;
                    SendVisibleActorsToActor(mob);
                    OnActorVisibilityChange(mob);

                    if (Event != null)
                        switch (Callbacktype)
                        {
                            case 1:
                                eh.Dying += Event;
                                break;
                            case 2:
                                eh.SkillUsing += Event;
                                break;
                            case 3:
                                eh.Moving += Event;
                                break;
                            case 4:
                                eh.Attacking += Event;
                                break;
                            case 5:
                                eh.Defending += Event;
                                break;
                            case 6:
                                eh.Returning += Event;
                                break;
                            case 7:
                                eh.FirstTimeDefending += Event;
                                break;
                        }

                    mobs.Add(mob);
                    eh.AI.noreturn = noreturn;
                    if (!NoAIForNPC)
                        eh.AI.Start();
                    else eh.AI.Pause();

                    if (delay > 1800)
                        MobFactory.Instance.BossList.Add(mob);
                }

                return mobs;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                return null;
            }
        }

        public ActorMob SpawnMob(uint mobID, short x, short y, short moveRange, Actor master)
        {
            var mob = new ActorMob(mobID);
            mob.MapID = ID;
            mob.X = x;
            mob.Y = y;
            var eh = new MobEventHandler(mob);
            mob.e = eh;
            eh.AI.MoveRange = moveRange;
            if (MobAIFactory.Instance.Items.ContainsKey(mob.MobID))
                eh.AI.Mode = MobAIFactory.Instance.Items[mob.MobID];
            else
                eh.AI.Mode = new AIMode(0);
            eh.AI.Master = master;
            eh.AI.X_Ori = x;
            eh.AI.Y_Ori = y;
            eh.AI.X_Spawn = x;
            eh.AI.Y_Spawn = y;
            if (eh.AI.Master != null) eh.AI.OnAttacked(master, 1);
            RegisterActor(mob);
            mob.invisble = false;
            mob.sightRange = 2000;
            SendVisibleActorsToActor(mob);
            OnActorVisibilityChange(mob);
            eh.AI.Start();
            return mob;
        }


        public bool CheckActorSkillInRange(short x, short y, short range)
        {
            var actors = GetActorsArea(x, y, range);
            foreach (var i in actors)
            {
                if (i.type != ActorType.SKILL)
                    continue;
                if (CheckActorSkillIsHeal(x, y, range))
                    continue;
                if (CheckActorSkillIsMusic(x, y, range))
                    continue;
                var skill = (ActorSkill)i;
                if (!skill.Stackable)
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     检测是否为演奏系技能（未完成）
        /// </summary>
        public bool CheckActorSkillIsMusic(short x, short y, short range)
        {
            var actors = GetActorsArea(x, y, range);
            foreach (var i in actors)
            {
                if (i.type != ActorType.SKILL)
                    continue;
                var skill = (ActorSkill)i;
                if (skill.Skill.ID == 2310 || //森巴
                    skill.Skill.ID == 2313 || //古典
                    skill.Skill.ID == 2311 || //重金属
                    skill.Skill.ID == 2312 || //摇滚
                    skill.Skill.ID == 2309 || //变化
                                              //skill.Skill.ID == 2314 ||//安魂曲??
                    skill.Skill.ID == 2307 || //混合
                    skill.Skill.ID == 2308 //流行
                   )
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     检测是否为恢复系技能
        /// </summary>
        public bool CheckActorSkillIsHeal(short x, short y, short range)
        {
            var actors = GetActorsArea(x, y, range);
            foreach (var i in actors)
            {
                if (i.type != ActorType.SKILL)
                    continue;
                var skill = (ActorSkill)i;
                if (skill.Skill.ID == 3434 || //福音
                    skill.Skill.ID == 2536 //GU回复
                   )
                    return true;
            }

            return false;
        }

        public int CountActorType(ActorType type)
        {
            var actors = Actors.Values.ToList();
            var count = 0;
            foreach (var i in actors)
                if (i.type == type)
                    count++;
            return count;
        }

        public void SendEffect(Actor actor, uint effect)
        {
            var arg = new EffectArg();
            arg.actorID = actor.ActorID;
            arg.effectID = effect;
            SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, actor, true);
        }

        public void SendEffect(Actor actor, byte x, byte y, uint effectID)
        {
            var arg = new EffectArg();
            arg.effectID = effectID;
            arg.actorID = 0xFFFFFFFF;
            arg.x = x;
            arg.y = y;
            SendEventToAllActorsWhoCanSeeActor(EVENT_TYPE.SHOW_EFFECT, arg, actor, true);
        }


        public void DefWarChange(DefWar text)
        {
            var list = Actors.Values.ToList();
            foreach (var i in list)
                if (i.type == ActorType.PC && ((ActorPC)i).DefWarShow)
                    MapClient.FromActorPC((ActorPC)i).SendDefWarChange(text);
        }


        public void DefWarResult(byte r1, byte r2, int exp, int jobexp, int cp, byte u = 0)
        {
            //*
            var list = Actors.Values.ToList();
            foreach (var i in list)
                if (i.type == ActorType.PC)
                    MapClient.FromActorPC((ActorPC)i).SendDefWarResult(r1, r2, exp, jobexp, cp, u);
            //*/
        }


        public void DefWarState(byte rate)
        {
            var list = Actors.Values.ToList();
            foreach (var i in list)
                if (i.type == ActorType.PC)
                    MapClient.FromActorPC((ActorPC)i).SendDefWarState(rate);
        }

        public void DefWarStates(Dictionary<uint, byte> l)
        {
            var list = Actors.Values.ToList();
            foreach (var i in list)
                if (i.type == ActorType.PC)
                    MapClient.FromActorPC((ActorPC)i).SendDefWarStates(l);
        }

        public bool IsMapInstance { get; set; }

        public bool IsDungeon { get; set; }

        public DungeonMap DungeonMap { get; set; }

        public uint ClientExitMap { get; set; }

        public byte ClientExitX { get; set; }

        public byte ClientExitY { get; set; }

        public bool AutoDispose { get; set; }

        public ActorPC Creator { get; set; }

        public Ring Ring { get; set; }

        public uint ResurrectionLimit { get; set; }

        public void OnDestrory()
        {
            var pcs = new List<Actor>();
            var items = new List<Actor>();
            var other = new List<Actor>();
            if (MobSpawnManager.Instance.Spawns.ContainsKey(ID))
            {
                foreach (var mob in MobSpawnManager.Instance.Spawns[ID])
                {
                    var eh = (MobEventHandler)mob.e;
                    AIThread.Instance.RemoveAI(eh.AI);
                    //Mob.NewAIThread.Instance.RemoveAI(eh.NewAI);
                    foreach (var i in mob.Tasks.Values) i.Deactivate();
                    mob.Tasks.Clear();
                }

                MobSpawnManager.Instance.Spawns[ID].Clear();
                MobSpawnManager.Instance.Spawns.Remove(ID);
            }

            foreach (var mob in Actors.Values)
                if (mob.type == ActorType.MOB)
                {
                    var eh = (MobEventHandler)mob.e;
                    AIThread.Instance.RemoveAI(eh.AI);
                    foreach (var i in mob.Tasks.Values) i.Deactivate();
                    mob.Tasks.Clear();
                }

            foreach (var i in Actors.Values)
                if (i.type == ActorType.PC)
                    pcs.Add(i);
                else if (i.type == ActorType.ITEM)
                    items.Add(i);
                else if (i.type == ActorType.GOLEM)
                    try
                    {
                        var golem = (ActorGolem)i;
                        MapServer.charDB.SaveChar(golem.Owner, false);
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }
                else
                    other.Add(i);

            foreach (var i in other)
            {
                i.ClearTaskAddition();
                DeleteActor(i);
            }

            var map = MapManager.Instance.GetMap(ClientExitMap);
            foreach (var i in items)
            {
                var item = (ActorItem)i;
                if (item.Item.PossessionedActor != null)
                {
                    var p = new CSMG_POSSESSION_CANCEL();
                    p.PossessionPosition = PossessionPosition.NONE;
                    var eh = (PCEventHandler)item.Item.PossessionedActor.e;
                    var posActor = item.Item.PossessionedActor;
                    if (posActor.Online)
                        eh.Client.OnPossessionCancel(p);
                    else
                        posActor.PossessionTarget = 0;
                    eh.Client.map.SendActorToMap((Actor)posActor, (uint)ClientExitMap,
                        Global.PosX8to16(ClientExitX, map.Width),
                        Global.PosY8to16(ClientExitY, map.Height));
                    if (!posActor.Online)
                    {
                        MapServer.charDB.SaveChar(posActor, false, false);
                        MapServer.accountDB.WriteUser(posActor.Account);
                        MapManager.Instance.GetMap(posActor.MapID).DeleteActor(posActor);
                        MapClient.FromActorPC(posActor).DisposeActor();
                        posActor.Account = null;
                    }

                    if (pcs.Contains(posActor))
                        pcs.Remove(posActor);
                }

                i.Speed = Configuration.Configuration.Instance.Speed;
                i.ClearTaskAddition();
                DeleteActor(i);
            }

            foreach (var i in pcs)
            {
                i.Speed = Configuration.Configuration.Instance.Speed;
                try
                {
                    SendActorToMap(i, (uint)ClientExitMap, Global.PosX8to16(ClientExitX, map.Width),
                        Global.PosY8to16(ClientExitY, map.Height));
                }
                catch
                {
                }
            }
        }
    }
}