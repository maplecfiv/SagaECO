using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using SagaDB;
using SagaDB.Actor;
using SagaDB.DefWar;
using SagaDB.DEMIC;
using SagaDB.DualJob;
using SagaDB.ECOShop;
using SagaDB.EnhanceTable;
using SagaDB.FlyingGarden;
using SagaDB.Iris;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Marionette;
using SagaDB.MasterEnchance;
using SagaDB.Mob;
using SagaDB.Npc;
using SagaDB.ODWar;
using SagaDB.Partner;
using SagaDB.PProtect;
using SagaDB.Quests;
using SagaDB.Ring;
using SagaDB.Skill;
using SagaDB.Team;
using SagaDB.Theater;
using SagaDB.Title;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Dungeon;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Packets.Client.AbyssTeam;
using SagaMap.Packets.Client.Actor;
using SagaMap.Packets.Client.Another;
using SagaMap.Packets.Client.Bond;
using SagaMap.Packets.Client.Chat;
using SagaMap.Packets.Client.Community;
using SagaMap.Packets.Client.DEM;
using SagaMap.Packets.Client.DualJob;
using SagaMap.Packets.Client.Fish;
using SagaMap.Packets.Client.FlyingCastle;
using SagaMap.Packets.Client.FlyingGarden;
using SagaMap.Packets.Client.Golem;
using SagaMap.Packets.Client.InfiniteCorridor;
using SagaMap.Packets.Client.IrisCard;
using SagaMap.Packets.Client.Item;
using SagaMap.Packets.Client.Login;
using SagaMap.Packets.Client.Navi;
using SagaMap.Packets.Client.NCShop;
using SagaMap.Packets.Client.NPC;
using SagaMap.Packets.Client.Partner;
using SagaMap.Packets.Client.Party;
using SagaMap.Packets.Client.Possession;
using SagaMap.Packets.Client.PProtect;
using SagaMap.Packets.Client.Quest;
using SagaMap.Packets.Client.Ring;
using SagaMap.Packets.Client.Skill;
using SagaMap.Packets.Client.Tamaire;
using SagaMap.Packets.Client.Trade;
using SagaMap.Packets.Client.VShop;
using SagaMap.Packets.Server;
using SagaMap.Packets.Server.AbyssTeam;
using SagaMap.Packets.Server.Actor;
using SagaMap.Packets.Server.Another;
using SagaMap.Packets.Server.AnotherAncientArk;
using SagaMap.Packets.Server.Bond;
using SagaMap.Packets.Server.Chat;
using SagaMap.Packets.Server.Community;
using SagaMap.Packets.Server.DefWar;
using SagaMap.Packets.Server.DEM;
using SagaMap.Packets.Server.DualJob;
using SagaMap.Packets.Server.FFGarden;
using SagaMap.Packets.Server.FGarden;
using SagaMap.Packets.Server.Fish;
using SagaMap.Packets.Server.Golem;
using SagaMap.Packets.Server.IrisCard;
using SagaMap.Packets.Server.Item;
using SagaMap.Packets.Server.Login;
using SagaMap.Packets.Server.MosterGuide;
using SagaMap.Packets.Server.NCShop;
using SagaMap.Packets.Server.NPC;
using SagaMap.Packets.Server.Partner;
using SagaMap.Packets.Server.Party;
using SagaMap.Packets.Server.Possession;
using SagaMap.Packets.Server.PProtect;
using SagaMap.Packets.Server.Quest;
using SagaMap.Packets.Server.Ring;
using SagaMap.Packets.Server.Skill;
using SagaMap.Packets.Server.Stamp;
using SagaMap.Packets.Server.Tamaire;
using SagaMap.Packets.Server.Theater;
using SagaMap.Packets.Server.Trade;
using SagaMap.Packets.Server.VShop;
using SagaMap.Partner;
using SagaMap.Scripting;
using SagaMap.Skill;
using SagaMap.Skill.Additions;
using SagaMap.Tasks.Golem;
using SagaMap.Tasks.Item;
using SagaMap.Tasks.Partner;
using SagaMap.Tasks.PC;
using SagaMap.Tasks.Skill;
using SagaMap.Tasks.System;
using AIMode = SagaMap.Mob.AIMode;
// using FurniturePlace = SagaDB.FFGarden.FurniturePlace;s
using Item = SagaDB.Item.Item;
using Marionette = SagaMap.Tasks.PC.Marionette;
using SkillCast = SagaMap.Tasks.PC.SkillCast;
using StatusFactory = SagaMap.PC.StatusFactory;
using Timer = SagaMap.Scripting.Timer;
using Version = SagaLib.Version;

namespace SagaMap.Network.Client
{
    public class MapClient : SagaLib.Client
    {
        public enum SESSION_STATE
        {
            LOGIN,
            AUTHENTIFICATED,
            REDIRECTING,
            DISCONNECTED,
            LOADING,
            LOADED
        }

        private Account account;
        public MobAI AI;
        public bool CheckAPI;
        private string client_Version;
        public bool firstLogin = true;

        private uint frontWord, backWord;

        //end

        public Map map;
        private Dictionary<uint, PlayerShopItem> sellShop = new Dictionary<uint, PlayerShopItem>();

        //an添加

        private Dictionary<uint, PlayerShopItem> soldItem = new Dictionary<uint, PlayerShopItem>();
        public SESSION_STATE state;

        public MapClient(Socket mSock, Dictionary<ushort, Packet> mCommandTable)
        {
            NetIo = new NetIO(mSock, mCommandTable, this);
            NetIo.SetMode(NetIO.Mode.Server);
            NetIo.FirstLevelLength = 2;
            if (NetIo.sock.Connected) OnConnect();
        }

        /// <summary>
        ///     玩家商店变量(0为关 1为开)广播用
        /// </summary>
        public byte Shopswitch { get; set; }

        /// <summary>
        ///     玩家商店标题
        /// </summary>
        public string Shoptitle { get; set; }

        public ActorPC Character { get; set; }

        public Map Map
        {
            get => map;
            set => map = value;
        }

        public override string ToString()
        {
            try
            {
                var ip = "";
                var name = "";
                if (NetIo != null)
                    ip = NetIo.sock.RemoteEndPoint.ToString();
                if (Character != null) name = Character.Name;
                if (ip != "" || name != "") return string.Format("{0}({1})", name, ip);

                return "MapClient";
            }
            catch (Exception exception)
            {
                Logger.ShowError(exception, null);
                return "MapClient";
            }
        }

        public static MapClient FromActorPC(ActorPC pc)
        {
            var eh = (PCEventHandler)pc.e;
            return eh.Client;
        }

        public override void OnConnect()
        {
        }

        private void SendHack()
        {
            /*try
            {
                if ((this.hackStamp - DateTime.Now).TotalMinutes >= 10)
                {
                    hackCount = 0;
                    hackStamp = DateTime.Now;
                }
                hackCount++;
                if (hackCount > 10)
                {
                    foreach (MapClient i in MapClientManager.Instance.OnlinePlayer)
                    {
                        i.SendAnnounce("Dont hack");
                    }
                    //this.NetIo.Disconnect();
                }
                else if (hackCount > 2)
                {
                    this.SendSystemMessage("Dont hack");
                }
            }
            catch { }*/ //暂时关闭HACK
        }

        public override void OnDisconnect()
        {
            npcSelectResult = 0;
            npcShopClosed = true;
            try
            {
                state = SESSION_STATE.DISCONNECTED;
                MapClientManager.Instance.Clients.Remove(this);
                //如果脚本线程不为空，则强制中断
                if (scriptThread != null)
                    try
                    {
                        scriptThread.Abort();
                    }
                    catch
                    {
                    }

                if (Character == null)
                    return;

                Character.VisibleActors.Clear();

                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.PLAYER_LOG_OUT, (object)Character.Name));
                Logger.ShowInfo(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                MapClientManager.Instance.OnlinePlayer.Count);
                MapServer.shouldRefreshStatistic = true;

                if (Logger.defaultSql != null)
                    Logger.defaultSql.SQLExecuteNonQuery("UPDATE `char` SET `online` = 0 WHERE `char_id` = " +
                                                         Character.CharID);

                if (Character.HP == 0)
                {
                    /*this.Character.HP = 1;
                    if (this.Character.SaveMap == 0)
                    {
                        this.Character.SaveMap = 10023100;
                        this.Character.SaveX = 242;
                        this.Character.SaveY = 128;
                    }
                    if (Configuration.Instance.HostedMaps.Contains(this.Character.SaveMap))
                    {
                        MapInfo info = MapInfoFactory.Instance.MapInfo[this.Character.SaveMap];
                        this.Character.MapID = this.Character.SaveMap;
                        this.Character.X = Global.PosX8to16(this.Character.SaveX, info.width);
                        this.Character.Y = Global.PosY8to16(this.Character.SaveY, info.height);
                    }*/
                }

                if (Character.TenkActor != null)
                {
                    var map = MapManager.Instance.GetMap(Character.TenkActor.MapID);
                    map.DeleteActor(Character.TenkActor);
                    if (ScriptManager.Instance.Events.ContainsKey(Character.TenkActor.EventID))
                        ScriptManager.Instance.Events.Remove(Character.TenkActor.EventID);
                    Character.TenkActor = null;
                }

                if (Character.FlyingGarden != null)
                {
                    if (Character.FlyingGarden.RopeActor != null)
                    {
                        var map = MapManager.Instance.GetMap(Character.FlyingGarden.RopeActor.MapID);
                        map.DeleteActor(Character.FlyingGarden.RopeActor);
                        if (ScriptManager.Instance.Events.ContainsKey(Character.FlyingGarden.RopeActor.EventID))
                            ScriptManager.Instance.Events.Remove(Character.FlyingGarden.RopeActor.EventID);
                        Character.FlyingGarden.RopeActor = null;
                    }

                    if (Character.FlyingGarden.RoomMapID != 0)
                    {
                        var roomMap = MapManager.Instance.GetMap(Character.FlyingGarden.RoomMapID);
                        var gardenMap = MapManager.Instance.GetMap(Character.FlyingGarden.MapID);
                        roomMap.ClientExitMap = gardenMap.ClientExitMap;
                        roomMap.ClientExitX = gardenMap.ClientExitX;
                        roomMap.ClientExitY = gardenMap.ClientExitY;
                        MapManager.Instance.DeleteMapInstance(roomMap.ID);
                        Character.FlyingGarden.RoomMapID = 0;
                    }

                    if (Character.FlyingGarden.MapID != 0)
                    {
                        MapManager.Instance.DeleteMapInstance(Character.FlyingGarden.MapID);
                        Character.FlyingGarden.MapID = 0;
                    }
                }

                if (Map.IsMapInstance && Character.PossessionTarget == 0 && !golemLogout)
                {
                    Character.MapID = Map.ClientExitMap;
                    var map = MapManager.Instance.GetMap(Map.ClientExitMap);
                    Character.X = Global.PosX8to16(Map.ClientExitX, map.Width);
                    Character.Y = Global.PosY8to16(Map.ClientExitY, map.Width);
                }

                Character.Online = false;
                if (logger != null)
                {
                    logger.Dispose();
                    logger = null;
                }

                if (Character.Marionette != null)
                    MarionetteDeactivate(true);

                RecruitmentManager.Instance.DeleteRecruitment(Character);

                PartyManager.Instance.PlayerOffline(Character.Party, Character);
                RingManager.Instance.PlayerOffline(Character.Ring, Character);

                var possessioned = Character.PossesionedActors;
                foreach (var i in possessioned)
                {
                    var item = GetPossessionItem(Character, i.PossessionPosition);
                    if (item.PossessionOwner != Character && item.PossessionOwner != null)
                    {
                        var actor = PossessionItemAdd(i, i.PossessionPosition, "");
                        i.PossessionTarget = actor.ActorID;

                        Character.Inventory.DeleteItem(item.Slot, 1);
                        var arg = new PossessionArg();
                        arg.fromID = i.ActorID;
                        arg.toID = i.ActorID;
                        arg.result = (int)i.PossessionPosition;
                        Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, i, true);
                    }
                    else
                    {
                        if (i != Character)
                        {
                            i.PossessionTarget = 0;
                            if (i.Online)
                            {
                                var arg = new PossessionArg();
                                arg.fromID = i.ActorID;
                                arg.toID = i.PossessionTarget;
                                arg.cancel = true;
                                arg.result = (int)i.PossessionPosition;
                                arg.x = Global.PosX16to8(i.X, Map.Width);
                                arg.y = Global.PosY16to8(i.Y, Map.Height);
                                arg.dir = (byte)(i.Dir / 45);
                                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, i, true);
                            }
                            else
                            {
                                if (MapManager.Instance.GetMap(i.MapID) != null)
                                    MapManager.Instance.GetMap(i.MapID).DeleteActor(i);
                                MapServer.charDB.SaveChar(i, false, false);
                                MapServer.accountDB.WriteUser(i.Account);
                                FromActorPC(i).DisposeActor();
                                i.Account = null;
                                continue;
                            }
                        }
                    }

                    MapServer.charDB.SaveChar(i, false, false);
                    MapServer.accountDB.WriteUser(i.Account);
                }

                if (golemLogout && Character.PossessionTarget == 0)
                {
                    Character.Golem.MapID = Character.MapID;
                    var mainfo = MarionetteFactory.Instance.Items[Character.Golem.Item.BaseData.marionetteID];
                    Character.Golem.PictID = mainfo.PictID;
                    Character.Golem.X = Character.X;
                    Character.Golem.Y = Character.Y;
                    Character.Golem.Dir = Character.Dir;
                    Character.Golem.Owner = Character;
                    Character.Golem.e = new NullEventHandler();
                    if (Character.Golem.BuyLimit > Character.Gold)
                        Character.Golem.BuyLimit = (uint)Character.Gold;
                    if (Character.Golem.GolemType >= GolemType.Plant && Character.Golem.GolemType <= GolemType.Strange)
                    {
                        var eh = new MobEventHandler(Character.Golem);
                        Character.Golem.e = eh;
                        eh.AI.Mode = new AIMode(0);
                        eh.AI.X_Ori = Character.X;
                        eh.AI.Y_Ori = Character.Y;
                        eh.AI.X_Spawn = Character.X;
                        eh.AI.Y_Spawn = Character.Y;
                        eh.AI.MoveRange = (short)(map.Width * 100);
                        eh.AI.Start();
                        if (Character.Golem.GolemType != GolemType.Buy)
                        {
                            var task = new GolemTask(Character.Golem);
                            task.Activate();
                            Character.Golem.Tasks.Add("GolemTask", task);
                        }
                    }

                    map.RegisterActor(Character.Golem);
                    Character.Golem.invisble = false;
                    Character.Golem.Item.Durability--;
                    map.OnActorVisibilityChange(Character.Golem);
                }

                //release resource
                Character.VisibleActors.Clear();
                MapManager.Instance.DisposeMapInstanceOnLogout(Character.CharID);

                Map.DeleteActor(Character);
                MapServer.charDB.SaveChar(Character);
                MapServer.accountDB.WriteUser(Character.Account);

                //防止下线后还存取仓库
                Character.Inventory.WareHouse = null;

                if (Character.Pet != null)
                    DeletePet();
                if (Character.Partner != null)
                    DeletePartner();
                //release resource
                if (!golemLogout && Character.PossessionTarget == 0)
                {
                    DisposeActor();
                }
                else
                {
                    foreach (var i in Character.Tasks)
                    {
                        if (i.Value is Timer) ScriptManager.Instance.Timers.Remove(i.Value.Name);
                        i.Value.Deactivate();
                    }

                    Character.Tasks.Clear();
                }

                //退出副本
                OnPProtectCreatedOut(null);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void DisposeActor()
        {
            MapManager.Instance.DisposeMapInstanceOnLogout(Character.CharID);
            foreach (var i in Character.Tasks)
            {
                if (i.Value is Timer) ScriptManager.Instance.Timers.Remove(i.Value.Name);
                i.Value.Deactivate();
            }

            Character.Tasks.Clear();
            Character.ClearTaskAddition();
            //this.chara.e = null;
            Character.Inventory = null;
            Character.Golem = null;

            Character.Stamp.Dispose();
            Character.FlyingGarden = null;
            Character.Status = null;
            Character.ClearVarialbes();
            Character.Marionette = null;
            Character.NPCStates.Clear();
            Character.Skills = null;
            Character.Skills2 = null;
            Character.SkillsReserve = null;
            Character.Elements.Clear();
            Character.Pet = null;
            Character.Partner = null;
            Character.Quest = null;
            Character.Ring = null;
            Character.Slave.Clear();
            Character.Account = null;
            Character = null;
        }

        public Event currentEvent;
        private uint currentEventID;
        public Shop currentShop;
        private uint currentVShopCategory;
        public string inputContent;
        public bool npcJobSwitch;
        public bool npcJobSwitchRes;
        public int npcSelectResult;
        public bool npcShopClosed;
        public Thread scriptThread;
        public uint selectedPet;
        public bool syntheseFinished;
        public Dictionary<uint, uint> syntheseItem;
        public bool vshopClosed = Configuration.Configuration.Instance.VShopClosed;

        public void OnNPCPetSelect(CSMG_NPC_PET_SELECT p)
        {
            selectedPet = p.Result;
        }

        private string ff()
        {
            return Environment.CurrentDirectory;
        }

        public void OnVShopBuy(CSMG_VSHOP_BUY p)
        {
            if (!vshopClosed)
            {
                var items = p.Items;
                var counts = p.Counts;
                var points = new uint[items.Length];
                var rental = new int[items.Length];
                var k = 0;
                uint neededPoints = 0;
                for (var i = 0; i < items.Length; i++)
                {
                    var cat = from item in ECOShopFactory.Instance.Items.Values
                              where item.Items.ContainsKey(items[i])
                              select item;

                    if (cat.Count() > 0)
                    {
                        var category = cat.First();
                        if (counts[i] > 0)
                        {
                            var chip = category.Items[items[i]];
                            points[i] = chip.points;
                            rental[i] = chip.rental;
                        }
                    }
                }

                for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
                if (Character.VShopPoints >= neededPoints)
                {
                    Character.UsedVShopPoints += neededPoints;
                    Character.VShopPoints -= neededPoints;
                    for (k = 0; k < items.Length; k++)
                    {
                        if (counts[k] <= 0)
                            continue;
                        var item = ItemFactory.Instance.GetItem(items[k]);
                        item.Stack = (ushort)counts[k];
                        if (rental[k] > 0)
                        {
                            item.Rental = true;
                            item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                        }

                        Logger.LogItemGet(Logger.EventType.ItemVShopGet, Character.Name + "(" + Character.CharID + ")",
                            item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("VShopBuy Count:{0}", item.Stack), false);
                        AddItem(item, true);
                    }
                }
            }
        }

        public void OnNCShopBuy(CSMG_NCSHOP_BUY p)
        {
            switch (Character.UsingShopType)
            {
                case PlayerUsingShopType.None:
                    break;
                case PlayerUsingShopType.GShop:
                    HandleGShopBuy(p);
                    break;
                case PlayerUsingShopType.NCShop:
                    HandleNCShopBuy(p);
                    break;
            }
        }

        public void HandleNCShopBuy(CSMG_NCSHOP_BUY p)
        {
            var items = p.Items;
            var counts = p.Counts;
            var points = new uint[items.Length];
            var rental = new int[items.Length];
            var k = 0;
            uint neededPoints = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var cat = from item in NCShopFactory.Instance.Items.Values
                          where item.Items.ContainsKey(items[i])
                          select item;

                if (cat.Count() > 0)
                {
                    var category = cat.First();
                    if (counts[i] > 0)
                    {
                        var chip = category.Items[items[i]];
                        points[i] = chip.points;
                        rental[i] = chip.rental;
                    }
                }
            }

            for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
            if (Character.CP >= neededPoints)
            {
                Character.UsedVShopPoints += neededPoints;
                Character.CP -= neededPoints;
                for (k = 0; k < items.Length; k++)
                {
                    if (counts[k] <= 0)
                        continue;
                    var item = ItemFactory.Instance.GetItem(items[k]);
                    item.Stack = (ushort)counts[k];
                    if (rental[k] > 0)
                    {
                        item.Rental = true;
                        item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                    }

                    Logger.LogItemGet(Logger.EventType.ItemVShopGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("NCShopBuy Count:{0}", item.Stack), false);
                    AddItem(item, true);
                }
            }
        }

        public void HandleGShopBuy(CSMG_NCSHOP_BUY p)
        {
            var items = p.Items;
            var counts = p.Counts;
            var points = new uint[items.Length];
            var rental = new int[items.Length];
            var k = 0;
            uint neededPoints = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var cat = from item in GShopFactory.Instance.Items.Values
                          where item.Items.ContainsKey(items[i])
                          select item;

                if (cat.Count() > 0)
                {
                    var category = cat.First();
                    if (counts[i] > 0)
                    {
                        var chip = category.Items[items[i]];
                        points[i] = chip.points;
                        rental[i] = chip.rental;
                    }
                }
            }

            for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
            if (Character.Gold >= neededPoints)
            {
                Character.Gold -= neededPoints;
                for (k = 0; k < items.Length; k++)
                {
                    if (counts[k] <= 0)
                        continue;
                    var item = ItemFactory.Instance.GetItem(items[k]);
                    item.Stack = (ushort)counts[k];
                    if (rental[k] > 0)
                    {
                        item.Rental = true;
                        item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                    }

                    Logger.LogItemGet(Logger.EventType.ItemNPCGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("GShopBuy Count:{0}", item.Stack), false);
                    AddItem(item, true);
                }
            }
        }

        public void OnNCShopCategoryRequest(CSMG_NCSHOP_CATEGORY_REQUEST p)
        {
            var category = NCShopFactory.Instance.Items[p.Page + 1];
            var p1 = new SSMG_NCSHOP_INFO_HEADER();
            p1.Page = p.Page;
            NetIo.SendPacket(p1);
            currentVShopCategory = p.Page + 1;
            foreach (var i in category.Items.Keys)
            {
                var p2 = new SSMG_NCSHOP_INFO();
                p2.Point = category.Items[i].points;
                p2.ItemID = i;
                p2.Comment = category.Items[i].comment;
                NetIo.SendPacket(p2);
            }

            var p3 = new SSMG_NCSHOP_INFO_FOOTER();
            NetIo.SendPacket(p3);
        }

        public void OnNCShopClose(CSMG_NCSHOP_CLOSE p)
        {
            Character.UsingShopType = PlayerUsingShopType.None;
            vshopClosed = true;
        }

        public void OnVShopClose(CSMG_VSHOP_CLOSE p)
        {
            vshopClosed = true;
        }

        public void OnVShopCategoryRequest(CSMG_VSHOP_CATEGORY_REQUEST p)
        {
            if (!vshopClosed)
            {
                var category = ECOShopFactory.Instance.Items[p.Page + 1];
                var p1 = new SSMG_VSHOP_INFO_HEADER();
                p1.Page = p.Page;
                NetIo.SendPacket(p1);
                currentVShopCategory = p.Page + 1;
                foreach (var i in category.Items.Keys)
                {
                    var p2 = new SSMG_VSHOP_INFO();
                    p2.Point = category.Items[i].points;
                    p2.ItemID = i;
                    p2.Comment = category.Items[i].comment;
                    NetIo.SendPacket(p2);
                }

                var p3 = new SSMG_VSHOP_INFO_FOOTER();
                NetIo.SendPacket(p3);
            }
        }

        public void OnNPCJobSwitch(CSMG_NPC_JOB_SWITCH p)
        {
            if (!npcJobSwitch)
                return;
            npcJobSwitchRes = false;
            if (p.Unknown != 0)
            {
                npcJobSwitchRes = true;
                var item = Character.Inventory.GetItem(Configuration.Configuration.Instance.JobSwitchReduceItem,
                    Inventory.SearchType.ITEM_ID);
                if (item != null || p.ItemUseCount == 0)
                {
                    if (item != null)
                    {
                        if (item.Stack >= p.ItemUseCount)
                            DeleteItem(item.Slot, (ushort)p.ItemUseCount, true);
                        else
                            return;
                    }

                    Character.SkillsReserve.Clear();
                    //check maximal reservalbe skill count
                    var count = 0;
                    if (Character.Job == Character.Job2X)
                        count = Character.JobLevel2X / 10;
                    if (Character.Job == Character.Job2T)
                        count = Character.JobLevel2T / 10;
                    if (count >= p.Skills.Length)
                        //set reserved skills
                        foreach (var i in p.Skills)
                            if (Character.Skills2.ContainsKey(i))
                                Character.SkillsReserve.Add(i, Character.Skills2[i]);

                    //clear skills
                    ResetSkill(2);

                    //change job and reduce job level
                    var levelLost = 0;
                    if (Character.Job == Character.Job2X)
                    {
                        Character.Job = Character.Job2T;
                        levelLost = (int)(Character.JobLevel2T / 5 - p.ItemUseCount);
                        if (levelLost <= 0)
                            levelLost = 0;
                        if (Character.SkillPoint2T > levelLost)
                            Character.SkillPoint2T -= (ushort)levelLost;
                        else
                            Character.SkillPoint2T = 0;
                        Character.JobLevel2T -= (byte)levelLost;
                        Character.JEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2T);
                    }
                    else
                    {
                        Character.Job = Character.Job2X;
                        levelLost = (int)(Character.JobLevel2X / 5 - p.ItemUseCount);
                        if (levelLost <= 0)
                            levelLost = 0;
                        if (Character.SkillPoint2X > levelLost)
                            Character.SkillPoint2X -= (ushort)levelLost;
                        else
                            Character.SkillPoint2X = 0;
                        Character.JobLevel2X -= (byte)levelLost;
                        Character.JEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2);
                    }

                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();

                    var arg = new EffectArg();
                    arg.effectID = 4131;
                    arg.actorID = Character.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Character, true);
                }
            }

            npcJobSwitch = false;
        }

        public void OnNPCInputBox(CSMG_NPC_INPUTBOX p)
        {
            inputContent = p.Content;
        }

        public void OnNPCShopBuy(CSMG_NPC_SHOP_BUY p)
        {
            var goods = p.Goods;
            var counts = p.Counts;
            if (Character.HP == 0) return;
            if (currentShop != null && goods.Length > 0)
            {
                uint gold = 0;
                switch (currentShop.ShopType)
                {
                    case ShopType.None:
                        gold = (uint)Character.Gold;
                        break;
                    case ShopType.CP:
                        gold = Character.CP;
                        break;
                    case ShopType.ECoin:
                        gold = Character.ECoin;
                        break;
                }

                for (var i = 0; i < goods.Length; i++)
                    if (currentShop.Goods.Contains(goods[i]))
                    {
                        var item = ItemFactory.Instance.GetItem(goods[i]);
                        item.Stack = (ushort)counts[i];
                        short buyrate = 0;
                        if (currentShop.ShopType == ShopType.None)
                            buyrate = Character.Status.buy_rate;
                        var price = (uint)(item.BaseData.price * ((float)(currentShop.SellRate + buyrate) / 200));
                        if (price == 0) price = 1;
                        price = price * item.Stack;
                        if (gold >= price)
                        {
                            gold -= price;
                            Logger.LogItemGet(Logger.EventType.ItemNPCGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("ShopBuy Count:{0}", item.Stack), false);
                            AddItem(item, true);
                        }
                    }

                switch (currentShop.ShopType)
                {
                    case ShopType.None:
                        Character.Gold = (int)gold;
                        break;
                    case ShopType.CP:
                        Character.CP = gold;
                        break;
                    case ShopType.ECoin:
                        Character.ECoin = gold;
                        break;
                }

                Character.Inventory.CalcPayloadVolume();
                SendCapacity();
            }
            else
            {
                if (currentEvent != null)
                {
                    var gold = Character.Gold;

                    switch (Character.TInt["ShopType"])
                    {
                        case 0:
                            gold = (uint)Character.Gold;
                            break;
                        case 1:
                            gold = Character.CP;
                            break;
                        case 2:
                            gold = Character.ECoin;
                            break;
                    }

                    for (var i = 0; i < goods.Length; i++)
                        if (currentEvent.Goods.Contains(goods[i]))
                        {
                            var item = ItemFactory.Instance.GetItem(goods[i]);
                            item.Stack = (ushort)counts[i];
                            var price = (int)(item.BaseData.price * ((float)Character.Status.buy_rate / 1000));
                            if (price == 0) price = 1;
                            price = price * item.Stack;
                            if (gold >= price)
                            {
                                gold -= price;
                                Logger.LogItemGet(Logger.EventType.ItemNPCGet,
                                    Character.Name + "(" + Character.CharID + ")",
                                    item.BaseData.name + "(" + item.ItemID + ")",
                                    string.Format("AddItem Count:{0}", item.Stack), false);
                                AddItem(item, true);
                            }
                        }
                    //this.Character.Gold = gold;

                    switch (Character.TInt["ShopType"])
                    {
                        case 0:
                            Character.Gold = gold;
                            break;
                        case 1:
                            Character.CP = (uint)gold;
                            break;
                        case 2:
                            Character.ECoin = (uint)gold;
                            break;
                    }

                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnNPCShopSell(CSMG_NPC_SHOP_SELL p)
        {
            var goods = p.Goods;
            var counts = p.Counts;

            if (currentShop != null)
            {
                uint total = 0;
                for (var i = 0; i < goods.Length; i++)
                {
                    var itemDroped = Character.Inventory.GetItem(goods[i]);
                    if (itemDroped == null)
                        return;
                    if (counts[i] > itemDroped.Stack)
                        counts[i] = itemDroped.Stack;
                    Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                        itemDroped.BaseData.name + "(" + itemDroped.ItemID + ")",
                        string.Format("NPCShopSell Count:{0}", counts[i]), false);

                    DeleteItem(goods[i], (ushort)counts[i], true);

                    var price = (uint)(itemDroped.BaseData.price * counts[i] *
                                       ((float)(10 + Character.Status.sell_rate) / 100));
                    total += price;
                }

                Character.Gold += (int)total;
                Character.Inventory.CalcPayloadVolume();
                SendCapacity();
            }
            else
            {
                if (currentEvent != null)
                {
                    uint total = 0;
                    for (var i = 0; i < goods.Length; i++)
                    {
                        var itemDroped = Character.Inventory.GetItem(goods[i]);
                        if (itemDroped == null)
                            return;
                        Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                            itemDroped.BaseData.name + "(" + itemDroped.ItemID + ")",
                            string.Format("NPCShopSell Count:{0}", counts[i]), false);

                        DeleteItem(goods[i], (ushort)counts[i], true);

                        var price = (uint)(itemDroped.BaseData.price * counts[i] *
                                           ((float)(10 + Character.Status.sell_rate) / 100));

                        total += price;
                    }

                    Character.Gold += (int)total;
                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnNPCShopClose(CSMG_NPC_SHOP_CLOSE p)
        {
            npcShopClosed = true;
        }

        public void OnNPCSelect(CSMG_NPC_SELECT p)
        {
            npcSelectResult = p.Result;
        }

        public void OnNPCSynthese(CSMG_NPC_SYNTHESE p)
        {
            var ids = p.SynIDs;
            foreach (var item in ids)
                if (!syntheseItem.ContainsKey(ids[item.Key]))
                    syntheseItem.Add(item.Key, item.Value);
        }

        public void OnNPCSyntheseFinish(CSMG_NPC_SYNTHESE_FINISH p)
        {
            syntheseFinished = true;
        }

        public void OnNPCEventStart(CSMG_NPC_EVENT_START p)
        {
            if (scriptThread == null)
            {
                if (tradingTarget != null || trading || Character.Buff.GetReadyPossession)
                {
                    SendEventStart(p.EventID);
                    SendCurrentEvent(p.EventID);
                    SendEventEnd();
                    return;
                }

                //if (p.EventID < 20000000 || p.EventID >= 0xF0000000)Unknow为啥要限制编号
                if (true)
                {
                    if (p.EventID >= 11000000)
                    {
                        if (NPCFactory.Instance.Items.ContainsKey(p.EventID))
                        {
                            var npc = NPCFactory.Instance.Items[p.EventID];
                            uint mapid;
                            if (map.IsMapInstance)
                            {
                                if (map.OriID != 0)
                                    mapid = map.OriID;
                                else
                                    mapid = map.ID * 100 / 1000;
                            }
                            else
                            {
                                mapid = map.ID;
                            }

                            if (npc.MapID == mapid)
                            {
                                if (Math.Abs(Character.X - Global.PosX8to16(npc.X, map.Width)) > 700 ||
                                    Math.Abs(Character.Y - Global.PosY8to16(npc.Y, map.Height)) > 700)
                                {
                                    SendEventStart(p.EventID);
                                    SendCurrentEvent(p.EventID);
                                    SendEventEnd();
                                    return;
                                }
                            }
                            else
                            {
                                SendEventStart(p.EventID);
                                SendCurrentEvent(p.EventID);
                                SendEventEnd();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (p.EventID != 10000315 && p.EventID != 10000316) //Exception for flying garden events
                        {
                            if (map.Info.events.ContainsKey(p.EventID))
                            {
                                var pos = map.Info.events[p.EventID];
                                byte x, y;
                                x = Global.PosX16to8(Character.X, map.Width);
                                y = Global.PosY16to8(Character.Y, map.Height);
                                var valid = false;
                                for (var i = 0; i < pos.Length / 2; i++)
                                    if (Math.Abs(pos[i * 2] - x) <= 3 && Math.Abs(pos[i * 2 + 1] - y) <= 3)
                                    {
                                        valid = true;
                                        break;
                                    }

                                if (!valid)
                                {
                                    SendHack();
                                    SendEventStart(p.EventID);
                                    SendCurrentEvent(p.EventID);
                                    SendEventEnd();
                                    return;
                                }
                            }
                            else
                            {
                                SendHack();
                                SendEventStart(p.EventID);
                                SendCurrentEvent(p.EventID);
                                SendEventEnd();
                                return;
                            }
                        }
                    }

                    EventActivate(p.EventID);
                }
                else
                {
                    SendEventStart(p.EventID);
                    SendCurrentEvent(p.EventID);
                    SendEventEnd();
                }
            }
            else
            {
                SendEventStart(p.EventID);
                SendCurrentEvent(p.EventID);
                SendEventEnd();
            }
        }

        public void EventActivate(uint EventID)
        {
            if (Character.Account.GMLevel > 100)
                SendSystemMessage("触发ID:" + EventID);
            if (ScriptManager.Instance.Events.ContainsKey(EventID))
            {
                var thread = new Thread(RunScript);
                thread.Name = string.Format("ScriptThread({0}) of player:{1}", thread.ManagedThreadId, Character.Name);
                ClientManager.AddThread(thread);
                if (scriptThread != null)
                {
                    Logger.ShowDebug("current script thread != null, currently running:" + currentEventID,
                        Logger.defaultlogger);
                    scriptThread.Abort();
                }

                currentEventID = EventID;
                scriptThread = thread;
                thread.Start();
            }
            else
            {
                SendEventStart(EventID);

                SendCurrentEvent(EventID);

                SendNPCMessageStart();
                if (account.GMLevel > 0)
                    SendNPCMessage(EventID, string.Format(LocalManager.Instance.Strings.NPC_EventID_NotFound, EventID),
                        131, "System Error");
                else
                    SendNPCMessage(EventID,
                        string.Format(LocalManager.Instance.Strings.NPC_EventID_NotFound_Msg, EventID), 131, "");
                SendNPCMessageEnd();
                SendEventEnd();
                Logger.ShowWarning("No script loaded for EventID:" + EventID);
            }
        }

        private void RunScript()
        {
            ClientManager.EnterCriticalArea();
            Event evnt = null;
            try
            {
                evnt = ScriptManager.Instance.Events[currentEventID];
                if (currentEventID < 0xFFFF0000)
                {
                    SendEventStart(currentEventID);
                    SendCurrentEvent(currentEventID);
                }

                currentEvent = evnt;
                currentEvent.CurrentPC = Character;
                var runscript = true;
                if (Character.Quest != null)
                {
                    if (Character.Quest.Detail.NPCSource == evnt.EventID)
                    {
                        if (Character.Quest.CurrentCount1 == 0 && Character.Quest.Status == QuestStatus.OPEN)
                        {
                            Character.Quest.CurrentCount1 = 1;
                            evnt.OnTransportSource(Character);
                            evnt.OnQuestUpdate(Character, Character.Quest);
                            runscript = false;
                        }
                        else
                        {
                            if (Character.Quest.CurrentCount2 == 1)
                            {
                                evnt.OnTransportCompleteSrc(Character);
                                runscript = false;
                            }
                        }
                    }

                    if (Character.Quest.Detail.NPCDestination == evnt.EventID)
                    {
                        if (Character.Quest.CurrentCount2 == 0 && Character.Quest.Status == QuestStatus.OPEN)
                        {
                            evnt.OnTransportDest(Character);
                            if (Character.Quest.CurrentCount3 == 0)
                            {
                                Character.Quest.CurrentCount2 = 1;
                                Character.Quest.Status = QuestStatus.COMPLETED;
                                evnt.OnQuestUpdate(Character, Character.Quest);
                                SendQuestStatus();
                                runscript = false;
                            }
                        }
                        else
                        {
                            evnt.OnTransportCompleteDest(Character);
                            runscript = false;
                        }
                    }
                }

                if (runscript) currentEvent.OnEvent(Character);
                if (currentEventID < 0xFFFF0000)
                    SendEventEnd();
            }
            catch (ThreadAbortException)
            {
                try
                {
                    ClientManager.RemoveThread(scriptThread.Name);
                    ClientManager.LeaveCriticalArea(scriptThread);
                    if (evnt != null)
                        evnt.CurrentPC = null;
                    currentEvent = null;
                    if (Character != null)
                        Logger.ShowWarning(string.Format(
                            "Player:{0} logged out while script thread is still running, terminating the script thread!",
                            Character.Name));
                }
                catch
                {
                }

                scriptThread = null;
            }
            catch (Exception ex)
            {
                try
                {
                    if (Character.Online)
                    {
                        if (Character.Account.GMLevel > 2)
                        {
                            SendNPCMessageStart();
                            SendNPCMessage(currentEventID,
                                "Script Error(" + ScriptManager.Instance.Events[currentEventID] + "):" + ex.Message,
                                131, "System Error");
                            SendNPCMessageEnd();
                        }

                        SendEventEnd();
                    }

                    Logger.ShowWarning("Script Error(" + ScriptManager.Instance.Events[currentEventID] + "):" +
                                       ex.Message + "\r\n" + ex.StackTrace);
                }
                catch
                {
                }
            }

            if (evnt != null)
                evnt.CurrentPC = null;
            scriptThread = null;
            currentEvent = null;
            ClientManager.RemoveThread(Thread.CurrentThread.Name);
            ClientManager.LeaveCriticalArea();
        }

        public void SendEventStart(uint id)
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_EVENT_START();
            NetIo.SendPacket(p);
            var p2 = new SSMG_NPC_EVENT_START_RESULT();
            p2.NPCID = id;
            NetIo.SendPacket(p2);
        }

        public void SendEventEnd()
        {
            if (!Character.Online)
                return;
            /*string args = "05 F4 00";
            byte[] buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            Packet ps1 = new Packet();
            ps1.data = buf;*/
            //this.NetIo.SendPacket(ps1);
            var p = new SSMG_NPC_EVENT_END();
            NetIo.SendPacket(p);
        }

        public void SendCurrentEvent(uint eventid)
        {
            var p = new SSMG_NPC_CURRENT_EVENT();
            p.EventID = eventid;
            NetIo.SendPacket(p);
        }

        public void SendNPCMessageStart()
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_MESSAGE_START();
            NetIo.SendPacket(p);
        }

        public void SendNPCMessageEnd()
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_MESSAGE_END();
            NetIo.SendPacket(p);
        }

        public void SendNPCMessage(uint npcID, string message, ushort motion, string title)
        {
            try
            {
                if (!Character.Online)
                    return;
                var p = new SSMG_NPC_MESSAGE();
                if (message.Contains('%'))
                {
                    var newmessage = "";
                    var temp = "";
                    var paras = message.Split('%');
                    for (var i = 0; i < paras.Length; i++)
                    {
                        temp = temp + paras[i];
                        temp = temp.Replace("$P", "");
                        if (i != paras.Length - 1)
                            temp = temp + "$P";
                        newmessage += temp;
                    }

                    message = newmessage;
                }

                if (message.Length > 50)
                {
                    var count = message.Length / 50;
                    var messages = new List<string>();
                    for (var i = 0; i < count; i++)
                        messages.Add(message.Substring(50 * i, 50));
                    if (message.Length != count * 50)
                        messages.Add(message.Substring(count * 50, message.Length - count * 50));
                    foreach (var item in messages)
                    {
                        p = new SSMG_NPC_MESSAGE();
                        p.SetMessage(npcID, 1, item, motion, title);
                        NetIo.SendPacket(p);
                    }
                }
                else
                {
                    p.SetMessage(npcID, 1, message, motion, title);
                    NetIo.SendPacket(p);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void SendNPCWait(uint wait)
        {
            var p = new SSMG_NPC_WAIT();
            p.Wait = wait;
            NetIo.SendPacket(p);
        }

        public void SendNPCPlaySound(uint soundID, byte loop, uint volume, byte balance)
        {
            SendNPCPlaySound(soundID, loop, volume, balance, false);
        }

        public void SendNPCPlaySound(uint soundID, byte loop, uint volume, byte balance, bool stopBGM)
        {
            var p = new SSMG_NPC_PLAY_SOUND();
            if (stopBGM) p.ID = 0x05EE;
            p.SoundID = soundID;
            p.Loop = loop;
            p.Volume = volume;
            p.Balance = balance;
            NetIo.SendPacket(p);
        }

        public void SendChangeBGM(uint soundID, byte loop, uint volume, byte balance)
        {
            var p = new SSMG_NPC_CHANGE_BGM();
            p.SoundID = soundID;
            p.Loop = loop;
            p.Volume = volume;
            p.Balance = balance;
            NetIo.SendPacket(p);
        }

        public void SendNPCShowEffect(uint actorID, byte x, byte y, ushort height, uint effectID, bool oneTime)
        {
            var p = new SSMG_NPC_SHOW_EFFECT();
            p.ActorID = actorID;
            p.EffectID = effectID;
            p.X = x;
            p.Y = y;
            p.height = height;
            p.OneTime = oneTime;
            NetIo.SendPacket(p);
        }

        public void SendNPCStates()
        {
            var AllInvolvedNPCStates =
                (from npc in NPCFactory.Instance.Items.Values where npc.MapID == Character.MapID select npc)
                .ToDictionary(i => i.ID, i => false);
            for (var i = 0; i < AllInvolvedNPCStates.Count; i++)
            {
                var npcid = AllInvolvedNPCStates.Keys.ElementAt(i);
                if (Character.NPCStates.ContainsKey(npcid))
                    AllInvolvedNPCStates[npcid] = Character.NPCStates[npcid];
            }

            var unloadedCount = AllInvolvedNPCStates.Count;
            var loadedCount = 0;
            var pages = new List<Dictionary<uint, bool>>();
            while (unloadedCount > 0)
                if (unloadedCount > 100)
                {
                    pages.Add(AllInvolvedNPCStates.Skip(loadedCount).Take(100).ToDictionary(i => i.Key, i => i.Value));
                    loadedCount += 100;
                    unloadedCount -= 100;
                }
                else
                {
                    pages.Add(AllInvolvedNPCStates.Skip(loadedCount).Take(unloadedCount)
                        .ToDictionary(i => i.Key, i => i.Value));
                    loadedCount += unloadedCount;
                    unloadedCount = 0;
                }

            foreach (var subpage in pages)
            {
                var p = new SSMG_NPC_STATES();
                p.PutNPCStates(subpage);
                NetIo.SendPacket(p);
            }
            /*
            if (this.Character.NPCStates.ContainsKey(this.map.ID))
            {
                foreach (uint i in this.chara.NPCStates[this.map.ID].Keys)
                {
                    if (this.chara.NPCStates[this.map.ID][i])
                    {
                        Packets.Server.SSMG_NPC_SHOW p = new SagaMap.Packets.Server.SSMG_NPC_SHOW();
                        p.NPCID = i;
                        this.NetIo.SendPacket(p);
                    }
                    else
                    {
                        Packets.Server.SSMG_NPC_HIDE p = new SagaMap.Packets.Server.SSMG_NPC_HIDE();
                        p.NPCID = i;
                        this.NetIo.SendPacket(p);
                    }
                }
            }
            */
        }

        public void OnFFGardenFurnitureUse(CSMG_FF_FURNITURE_USE p)
        {
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            if (furniture.Motion == 111)
                furniture.Motion = 622;
            else furniture.Motion = 111;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);

            /*if(furniture.ItemID == 31164100)
           {
               byte level = 1;
               if (Character.CInt["料理EXP"] > 5000)
                   level = 2;
               if (Character.CInt["料理EXP"] > 30000)
                   level =3;
               if (Character.CInt["料理EXP"] > 150000)
                   level = 4;
               if (Character.CInt["料理EXP"] > 500000)
                   level = 5;
               Scripting.SkillEvent.Instance.Synthese(Character, 2009, level);
               SendSystemMessage("当前料理经验：" + Character.CInt["料理EXP"].ToString() + " 等级：" + level.ToString());

           }
           //Item item = ItemFactory.Instance.GetItem(furniture.ItemID);
          if (item.BaseData.eventID != 0)
           {
               EventActivate(item.BaseData.eventID);
           }*/
        }

        public void OnFFGardenOtherJoin(CSMG_FFGARDEN_JOIN_OTHER p)
        {
            var ringID = MapServer.charDB.GetFlyCastleRindID(p.ff_id);
            var ring = RingManager.Instance.GetRing(ringID);
            if (ring == null || ring.FlyingCastle == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            OnFFGardenJoin(ringID);
        }

        public void OnFFGardenJoin(CSMG_FFGARDEN_JOIN p)
        {
            if (Character.Ring == null || Character.Ring.FlyingCastle == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            OnFFGardenJoin(Character.Ring.ID);
        }

        public void OnFFGardenJoin(uint ringid)
        {
            var ring = RingManager.Instance.GetRing(ringid);
            if (ring == null || ring.FlyingCastle == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            MapServer.charDB.GetFlyingCastleFurniture(ring);
            if (ring.FlyingCastle.MapID == 0)
            {
                var map = MapManager.Instance.GetMap(Character.MapID);
                ring.FlyingCastle.MapID = MapManager.Instance.CreateMapInstance(ring, 90001000, Character.MapID,
                    Global.PosX16to8(Character.X, map.Width), Global.PosY16to8(Character.Y, map.Height), false);
                map = MapManager.Instance.GetMap(ring.FlyingCastle.MapID);
                var ffmap = MapManager.Instance.GetMap(ring.FlyingCastle.MapID);
                var aa = new List<uint>();
                foreach (var y in ffmap.Actors)
                    if (y.Value.type == ActorType.FURNITURE)
                        aa.Add(y.Key);
                foreach (var i in aa) ffmap.DeleteActor(ffmap.Actors[i]);
                foreach (var i in ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.FARM])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.FISHERY])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.HOUSE])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }
            }

            Character.BattleStatus = 0;
            Character.Speed = 200;
            FromActorPC(Character).SendChangeStatus();

            if (Configuration.Configuration.Instance.HostedMaps.Contains(Character.MapID))
            {
                MapManager.Instance.GetMap(Character.MapID);
                if (Character.Marionette != null)
                    MarionetteDeactivate();
                Map.SendActorToMap(Character, ring.FlyingCastle.MapID, 20, 20, true);
            }

            /*Packet p = new Packet();
            string args = "20 44 00 00 00 01 00 00 00 00 F6 31 FF 94 00 64";
            byte[] buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            this.NetIo.SendPacket(p);*/
        }

        public void OnFFurnitureSetup(CSMG_FF_FURNITURE_SETUP p)
        {
            if (Character.MapID == 90001999 || Character.MapID == 91000999) // 家具設定
            {
                if (Character.Account.GMLevel < 250)
                {
                    SendSystemMessage("哎哟——？");
                    return;
                }

                CustomMapManager.Instance.SerFFofFurnitureSetup(this, p);
            }

            if (Character.Ring == null || Character.Ring.FlyingCastle == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FlyingCastle.MapID && Character.MapID != ring.FlyingCastle.RoomMapID)
                return;
            if (ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN].Count +
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM].Count <
                Configuration.Configuration.Instance.MaxFurnitureCount)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                var actor = new ActorFurniture();

                if (Character.Account.GMLevel < 100)
                    DeleteItem(p.InventorySlot, 1, false);

                actor.MapID = Character.MapID;
                actor.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.X = p.X;
                actor.Y = p.Y;
                actor.Z = p.Z;
                actor.Xaxis = p.Xaxis;
                actor.Yaxis = p.Yaxis;
                actor.Zaxis = p.Zaxis;
                actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
                actor.PictID = item.PictID;
                actor.e = new NullEventHandler();
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);

                if (Character.MapID == Character.Ring.FlyingCastle.MapID)
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN].Add(actor);
                else
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM].Add(actor);
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_SETUP, actor.Name,
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN].Count +
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM].Count,
                    Configuration.Configuration.Instance.MaxFurnitureCount));
                MapServer.charDB.SaveFlyCastle(ring);
            }
            else
            {
                SendSystemMessage(LocalManager.Instance.Strings.FG_FUTNITURE_MAX);
            }
        }

        public void OnFFFurnitureRemoveCastle(CSMG_FF_FURNITURE_REMOVE_CASTLE p)
        {
            if (Character.MapID == 90001999) CustomMapManager.Instance.RemoveFurnitureCastle(this, p);
        }

        public void OnFFFurnitureReset(CSMG_FF_FURNITURE_RESET p)
        {
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            var p2 = new SSMG_FF_FURNITURE_RESET();
            p2.AID = 1;
            p2.ActorID = furniture.ActorID;
            p2.RindID = Character.ActorID;
            NetIo.SendPacket(p2);
        }

        public void OnFFFurnitureRemove(CSMG_FF_FURNITURE_REMOVE p)
        {
            if (Character.MapID == 90001999 || Character.MapID == 91000999)
            {
                CustomMapManager.Instance.RemoveFurniture(this, p);
                return;
            }

            if (Character.Ring.FlyingCastle == null)
                return;
            if (Character.MapID != Character.Ring.FlyingCastle.MapID && Character.MapID != Character.Ring.FlyingCastle.RoomMapID)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            Map map = null;
            map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;

            if (Character.MapID == Character.Ring.FlyingCastle.MapID)
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN].Remove(furniture);
            else
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM].Remove(furniture);
            map.DeleteActor(actor);
            var item = ItemFactory.Instance.GetItem(furniture.ItemID);
            item.PictID = furniture.PictID;
            MapServer.charDB.SaveFlyCastle(ring);
            AddItem(item, false);
            SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_REMOVE, furniture.Name,
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.GARDEN].Count +
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM].Count,
                Configuration.Configuration.Instance.MaxFurnitureCount));
        }

        public void OnFFFurnitureRoomAppear()
        {
            var p = new Packet();
            var args = "20 44 00 00 00 01 00 00 00 00 F6 31 FF 94 00 64";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            NetIo.SendPacket(p);
        }

        public void OnFFurnitureCastleSetup(CSMG_FF_CASTLE_SETUP p)
        {
            if (Character.MapID == 90001999)
            {
                CustomMapManager.Instance.SerFFFurnitureCastleSetup(this, p);
                return;
            }

            if (Character.Ring == null || Character.Ring.FlyingCastle == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FlyingCastle.MapID && Character.MapID != ring.FlyingCastle.RoomMapID)
                return;
            if (ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.HOUSE].Count > 0)
            {
                SendSystemMessage("无法重复设置");
                return;
            }

            var item = Character.Inventory.GetItem(p.InventorySlot);
            var actor = new ActorFurnitureUnit();

            DeleteItem(p.InventorySlot, 1, false);

            actor.MapID = Character.MapID;
            actor.ItemID = item.ItemID;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.X = p.X;
            actor.Z = p.Z;
            actor.Yaxis = p.Yaxis;
            actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
            actor.PictID = item.PictID;
            actor.e = new NullEventHandler();
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            if (Character.MapID == Character.Ring.FlyingCastle.MapID)
                ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.HOUSE].Add(actor);
            MapServer.charDB.SaveFlyCastle(ring);
        }

        public void OnFFFurnitureRoomEnter(CSMG_FF_FURNITURE_ROOM_ENTER p)
        {
            if (p.data == 1)
            {
                if (this.map.ID == 90001999)
                {
                    CustomMapManager.Instance.SerFFRoomEnter(this);
                    return;
                }

                var ring = RingManager.Instance.GetRing(Character.Ring.ID);

                if (ring == null)
                    return;
                if (ring.FlyingCastle.RoomMapID == 0)
                {
                    ring.FlyingCastle.RoomMapID =
                        MapManager.Instance.CreateMapInstance(ring, 91000000, ring.FlyingCastle.MapID, 6, 7, false);
                    //spawn furnitures
                    var map = MapManager.Instance.GetMap(ring.FlyingCastle.RoomMapID);
                    foreach (var i in ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.ROOM])
                    {
                        i.e = new NullEventHandler();
                        map.RegisterActor(i);
                        i.invisble = false;
                    }
                }

                Map.SendActorToMap(Character, ring.FlyingCastle.RoomMapID, 20, 36, true);
            }
        }

        public void OnFFurnitureUnitSetup(CSMG_FF_UNIT_SETUP p)
        {
            if (Character.Ring == null || Character.Ring.FlyingCastle == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FlyingCastle.MapID && Character.MapID != ring.FlyingCastle.RoomMapID)
                return;
            var item = Character.Inventory.GetItem(p.InventorySlot);
            var actor = new ActorFurnitureUnit();

            DeleteItem(p.InventorySlot, 1, false);
            actor.MapID = Character.MapID;
            actor.ItemID = item.ItemID;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.X = p.X;
            actor.Y = 0;
            actor.Z = p.Z;
            actor.Xaxis = 0;
            actor.Yaxis = p.Yaxis;
            actor.Zaxis = 0;
            actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
            actor.PictID = item.PictID;
            actor.e = new NullEventHandler();


            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            if (Character.MapID == Character.Ring.FlyingCastle.MapID)
            {
                if (item.ItemID == 30300000)
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.FISHERY].Add(actor);
                else if (item.ItemID == 30260000)
                    ring.FlyingCastle.Furnitures[SagaDB.FlyingCastle.FurniturePlace.FARM].Add(actor);
            }

            MapServer.charDB.SaveFlyCastle(ring);
        }

        public void OnTamaireRentalRequest(CSMG_TAMAIRE_RENTAL_REQUEST p)
        {
            var lender = MapServer.charDB.GetChar(p.Lender);
            TamaireRentalManager.Instance.ProcessRental(Character, lender);
            SendTamaire();
            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        public void SendTamaire()
        {
            if (Character.TamaireRental == null)
                return;
            if (Character.TamaireRental.CurrentLender == 0)
                return;
            var p = new SSMG_TAMAIRE_RENTAL();
            var lender = MapServer.charDB.GetChar(Character.TamaireRental.CurrentLender);
            p.JobType = lender.TamaireLending.JobType;
            p.RentalDue = Character.TamaireRental.RentDue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            p.Factor = (short)((1f -
                                TamaireRentalManager.Instance.CalcFactor(lender.TamaireLending.Baselv -
                                                                         Character.Level)) * 1000);
            NetIo.SendPacket(p);
        }

        public void OnTamaireRentalTerminateRequest(CSMG_TAMAIRE_RENTAL_TERMINATE_REQUEST p)
        {
            OnTamaireRentalTerminate(1);
        }

        public void OnTamaireRentalTerminate(byte reason)
        {
            var p = new SSMG_TAMAIRE_RENTAL_TERMINATE();
            p.Reason = reason;
            NetIo.SendPacket(p);
        }

        public void OpenTamaireListUI()
        {
            var p = new SSMG_TAMAIRE_LIST_UI();
            NetIo.SendPacket(p);
        }

        public void OnGolemWarehouse(CSMG_GOLEM_WAREHOUSE p)
        {
            var p1 = new SSMG_GOLEM_WAREHOUSE();
            p1.ActorID = Character.ActorID;
            p1.Title = Character.Golem.Title;
            NetIo.SendPacket(p1);

            foreach (var i in Character.Inventory.GetContainer(ContainerType.GOLEMWAREHOUSE))
            {
                var p2 = new SSMG_GOLEM_WAREHOUSE_ITEM();
                p2.InventorySlot = i.Slot;
                p2.Container = ContainerType.GOLEMWAREHOUSE;
                p2.Item = i;
                NetIo.SendPacket(p2);
            }

            var p3 = new SSMG_GOLEM_WAREHOUSE_ITEM_FOOTER();
            NetIo.SendPacket(p3);
        }

        public void OnGolemWarehouseSet(CSMG_GOLEM_WAREHOUSE_SET p)
        {
            if (Character.Golem != null)
                Character.Golem.Title = p.Title;
        }

        public void OnGolemWarehouseGet(CSMG_GOLEM_WAREHOUSE_GET p)
        {
            var item = Character.Inventory.GetItem(p.InventoryID);
            if (item != null)
            {
                var count = p.Count;
                if (item.Stack >= count)
                {
                    var newItem = item.Clone();
                    newItem.Stack = count;
                    if (newItem.Stack > 0)
                        Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                            Character.Name + "(" + Character.CharID + ")",
                            newItem.BaseData.name + "(" + newItem.ItemID + ")",
                            string.Format("GolemWarehouseGet Count:{0}", count), false);

                    Character.Inventory.DeleteItem(p.InventoryID, count);
                    Logger.LogItemGet(Logger.EventType.ItemGolemGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("GolemWarehouse Count:{0}", item.Stack), false);
                    AddItem(newItem, false);
                    var p1 = new SSMG_GOLEM_WAREHOUSE_GET();
                    NetIo.SendPacket(p1);
                }
            }
        }

        public void OnGolemShopBuySell(CSMG_GOLEM_SHOP_BUY_SELL p)
        {
            var actor = map.GetActor(p.ActorID);
            var items = p.Items;
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;
                uint gold = 0;
                foreach (var i in items.Keys)
                {
                    var item = Character.Inventory.GetItem(i);
                    if (item == null)
                        continue;
                    if (items[i] == 0)
                        continue;
                    //if (item.BaseData.noTrade)
                    //continue;
                    var newItem = item.Clone();
                    if (item.Stack >= items[i])
                    {
                        uint inventoryID = 0;
                        foreach (var j in golem.BuyShop.Keys)
                            if (golem.BuyShop[j].ItemID == newItem.ItemID)
                            {
                                inventoryID = j;
                                break;
                            }

                        gold += golem.BuyShop[inventoryID].Price * items[i];
                        if (golem.BuyLimit < gold)
                        {
                            gold -= golem.BuyShop[inventoryID].Price * items[i];
                            break;
                        }

                        newItem.Stack = items[i];
                        Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                            Character.Name + "(" + Character.CharID + ")", item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("GolemSell Count:{0}", items[i]), false);
                        DeleteItem(i, items[i], true);
                        golem.BuyShop[inventoryID].Count -= items[i];

                        if (golem.BoughtItem.ContainsKey(item.ItemID))
                        {
                            golem.BoughtItem[item.ItemID].Count += items[i];
                        }
                        else
                        {
                            golem.BoughtItem.Add(item.ItemID, new GolemShopItem());
                            golem.BoughtItem[item.ItemID].Price = golem.BuyShop[inventoryID].Price;
                            golem.BoughtItem[item.ItemID].Count += items[i];
                        }

                        if (newItem.Stack > 0)
                            Logger.LogItemGet(Logger.EventType.ItemGolemGet,
                                Character.Name + "(" + Character.CharID + ")",
                                newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                string.Format("GolemBuy Count:{0}", newItem.Stack), false);
                        if (golem.BuyShop[inventoryID].Count == 0) //新加
                            golem.BuyShop.Remove(inventoryID);
                        //golem.Owner.Inventory.AddItem(ContainerType.BODY, newItem);
                    }
                }

                //golem.Owner.Gold -= (int)gold;
                golem.BuyLimit -= gold;
                Character.Gold += (int)gold;
            }
        }

        public void OnGolemShopSellBuy(CSMG_GOLEM_SHOP_SELL_BUY p)
        {
            var actor = map.GetActor(p.ActorID);
            var items = p.Items;
            var p1 = new SSMG_GOLEM_SHOP_SELL_ANSWER();
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;
                uint gold = 0;
                foreach (var i in items.Keys)
                {
                    var item = ItemFactory.Instance.GetItem(golem.SellShop[i].ItemID);
                    if (item == null)
                    {
                        p1.Result = -4;
                        NetIo.SendPacket(p1);
                        return;
                    }

                    if (items[i] == 0)
                    {
                        p1.Result = -2;
                        NetIo.SendPacket(p1);
                        return;
                    }

                    /*if (item.BaseData.noTrade)
                    {
                        p1.Result = -1;
                        this.NetIo.SendPacket(p1);
                        return;
                    }*/
                    if (golem.SellShop[i].Count >= items[i])
                    {
                        gold += golem.SellShop[i].Price * items[i];
                        if (Character.Gold < gold)
                        {
                            p1.Result = -7;
                            NetIo.SendPacket(p1);
                            return;
                        }

                        /*if (gold + golem.Owner.Gold >= 99999999)
                        {
                            p1.Result = -9;
                            this.NetIo.SendPacket(p1);
                            return;
                        }*/
                        var newItem = item.Clone();
                        newItem.Stack = items[i];
                        if (newItem.Stack > 0)
                            Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                                Character.Name + "(" + Character.CharID + ")",
                                newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                string.Format("GolemSell Count:{0}", items[i]), false);
                        //golem.Owner.Inventory.DeleteItem(i, items[i]);
                        golem.SellShop[i].Count -= items[i];
                        if (golem.SoldItem.ContainsKey(item.ItemID))
                        {
                            golem.SoldItem[item.ItemID].Count += items[i];
                        }
                        else
                        {
                            golem.SoldItem.Add(item.ItemID, new GolemShopItem());
                            golem.SoldItem[item.ItemID].Price = golem.SellShop[i].Price;
                            golem.SoldItem[item.ItemID].Count += items[i];
                        }

                        if (golem.SellShop[i].Count == 0) golem.SellShop.Remove(i);

                        if (golem.SellShop.Count == 0)
                        {
                            golem.invisble = true;
                            map.OnActorVisibilityChange(golem);
                        }

                        Logger.LogItemGet(Logger.EventType.ItemGolemGet, Character.Name + "(" + Character.CharID + ")",
                            item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("GolemBuy Count:{0}", item.Stack), false);
                        AddItem(newItem, true);
                    }
                    else
                    {
                        p1.Result = -5;
                        NetIo.SendPacket(p1);
                        return;
                    }
                }

                //golem.Owner.Gold += (int)gold;
                Character.Gold -= (int)gold;
                /*try
                {
                     SagaMap.MapServer. charDB.SaveChar(golem.Owner, true, false);
                }
                catch (Exception ex) { Logger.ShowError(ex); }*/
            }
        }

        public void OnGolemShopOpen(CSMG_GOLEM_SHOP_OPEN p)
        {
            var actor = map.GetActor(p.ActorID);
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;

                if (golem.GolemType == GolemType.Sell)
                {
                    var p1 = new SSMG_GOLEM_SHOP_OPEN_OK();
                    p1.ActorID = p.ActorID;
                    NetIo.SendPacket(p1);
                    var p2 = new SSMG_GOLEM_SHOP_HEADER();
                    p2.ActorID = p.ActorID;
                    NetIo.SendPacket(p2);
                    foreach (var i in golem.SellShop.Keys)
                    {
                        var item = golem.Owner.Inventory.GetItem(i);
                        if (item == null)
                            continue;
                        var p3 = new SSMG_GOLEM_SHOP_ITEM();
                        p3.InventorySlot = i;
                        p3.Container = ContainerType.BODY;
                        p3.Price = golem.SellShop[i].Price;
                        p3.ShopCount = golem.SellShop[i].Count;
                        p3.Item = item;
                        NetIo.SendPacket(p3);
                    }

                    var p4 = new SSMG_GOLEM_SHOP_FOOTER();
                    NetIo.SendPacket(p4);
                }

                if (golem.GolemType == GolemType.Buy)
                {
                    var p2 = new SSMG_GOLEM_SHOP_BUY_HEADER();
                    p2.ActorID = p.ActorID;
                    NetIo.SendPacket(p2);

                    var p3 = new SSMG_GOLEM_SHOP_BUY_ITEM();
                    p3.Items = golem.BuyShop;
                    p3.BuyLimit = golem.BuyLimit;
                    NetIo.SendPacket(p3);
                }
            }
        }

        public void OnGolemShopSellClose(CSMG_GOLEM_SHOP_SELL_CLOSE p)
        {
            var p1 = new SSMG_GOLEM_SHOP_SELL_SET();
            NetIo.SendPacket(p1);
        }

        public void OnGolemShopSellSetup(CSMG_GOLEM_SHOP_SELL_SETUP p)
        {
            var ids = p.InventoryIDs;
            var counts = p.Counts;
            var prices = p.Prices;
            if (ids.Length != 0)
                for (var i = 0; i < ids.Length; i++)
                {
                    if (!Character.Golem.SellShop.ContainsKey(ids[i]))
                    {
                        var item = new GolemShopItem();
                        item.InventoryID = ids[i];
                        item.ItemID = Character.Inventory.GetItem(ids[i]).ItemID;
                        Character.Golem.SellShop.Add(ids[i], item);
                    }

                    if (counts[i] == 0)
                    {
                        Character.Golem.SellShop.Remove(ids[i]);
                    }
                    else
                    {
                        Character.Golem.SellShop[ids[i]].Count = counts[i];
                        Character.Golem.SellShop[ids[i]].Price = prices[i];
                    }
                }

            Character.Golem.Title = p.Comment;
        }

        public void OnGolemShopSell(CSMG_GOLEM_SHOP_SELL p)
        {
            var p1 = new SSMG_GOLEM_SHOP_SELL_SETUP();
            p1.Comment = Character.Golem.Title;
            NetIo.SendPacket(p1);
        }

        public void OnGolemShopBuyClose(CSMG_GOLEM_SHOP_BUY_CLOSE p)
        {
            var p1 = new SSMG_GOLEM_SHOP_BUY_SET();
            NetIo.SendPacket(p1);
        }

        public void OnGolemShopBuySetup(CSMG_GOLEM_SHOP_BUY_SETUP p)
        {
            var ids = p.InventoryIDs;
            var counts = p.Counts;
            var prices = p.Prices;
            if (ids.Length != 0)
                for (var i = 0; i < ids.Length; i++)
                {
                    if (!Character.Golem.BuyShop.ContainsKey(ids[i]))
                    {
                        var item = new GolemShopItem();
                        item.InventoryID = ids[i];
                        item.ItemID = Character.Inventory.GetItem(ids[i]).ItemID;
                        Character.Golem.BuyShop.Add(ids[i], item);
                    }

                    if (counts[i] == 0)
                    {
                        Character.Golem.BuyShop.Remove(ids[i]);
                    }
                    else
                    {
                        Character.Golem.BuyShop[ids[i]].Count = counts[i];
                        Character.Golem.BuyShop[ids[i]].Price = prices[i];
                    }
                }

            Character.Golem.BuyLimit = p.BuyLimit;
            Character.Golem.Title = p.Comment;
        }

        public void OnGolemShopBuy(CSMG_GOLEM_SHOP_BUY p)
        {
            var p1 = new SSMG_GOLEM_SHOP_BUY_SETUP();
            p1.BuyLimit = Character.Golem.BuyLimit;
            p1.Comment = Character.Golem.Title;
            Character.Golem.BuyShop.Clear();
            NetIo.SendPacket(p1);
        }

        public DateTime hpmpspStamp = DateTime.Now;

        private uint masterpartner;
        public DateTime moveCheckStamp = DateTime.Now;
        public DateTime moveStamp = DateTime.Now;
        private uint pupilinpartner;

        //发送虚拟actor

        public void TitleProccess(ActorPC pc, uint ID, uint value)
        {
            if (CheckTitle((int)ID)) return;
            if (TitleFactory.Instance.Items.ContainsKey(ID))
            {
                //應該逐項條件檢查 先放著
                /*
                string name = "称号" + ID.ToString() + "完成度";
                pc.AInt[name] += (int)value;
                if (pc.ALong[name] >= (long)t.PrerequisiteCount)
                    UnlockTitle(pc, ID);
                */
            }
        }

        public void UnlockTitle(ActorPC pc, uint ID)
        {
            if (TitleFactory.Instance.Items.ContainsKey(ID))
            {
                var t = TitleFactory.Instance.Items[ID];
                var name = "称号" + ID + "完成度";
                if (Character.ALong[name] >= t.PrerequisiteCount)
                {
                    //應該逐項條件檢查 先放著
                    /*
                    if (!CheckTitle((int)ID))
                    {
                        SetTitle((int)ID, true);
                        SendSystemMessage("恭喜你！解锁了『" + t.name + "』称号！");
                        Skill.SkillHandler.Instance.ShowEffectOnActor(pc, 5420);
                    }
                    */
                }
            }
        }

        public void OnPlayerSetOption(CSMG_PLAYER_SETOPTION p)
        {
            if (Character == null)
                return;


            //SendSystemMessage("OPTION Result:" +  (Packets.Server.SSMG_ACTOR_OPTION.Options)p.GetOption);
            //SendSystemMessage("PACKET: " + p.DumpData());
            //SendSystemMessage("-----------");

            var unk = (SSMG_ACTOR_OPTION.Options)p.GetOption;

            foreach (var item in Enum.GetValues(typeof(SSMG_ACTOR_OPTION.Options)).Cast<Enum>()
                         .Where(item => unk.HasFlag(item)))
                switch ((SSMG_ACTOR_OPTION.Options)item)
                {
                    case SSMG_ACTOR_OPTION.Options.NONE:
                        ResetOption();
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_TRADE:
                        Character.canTrade = false;
                        Character.CInt["canTrade"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_EQUPMENT:
                        Character.showEquipment = false;
                        Character.CInt["showEquipment"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_FRIEND:
                        Character.canFriend = false;
                        Character.CInt["canFriend"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_BOND:
                        Character.canMentor = false;
                        Character.CInt["canMentor"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_PARTY:
                        Character.canParty = false;
                        Character.CInt["canParty"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_PATNER:
                        Character.canChangePartnerDisplay = false;
                        Character.CInt["canChangePartnerDisplay"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_REVIVE_MESSAGE:
                        Character.showRevive = false;
                        Character.CInt["showRevive"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_RING:
                        Character.canRing = false;
                        Character.CInt["canRing"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_SKILL:
                        Character.canWork = false;
                        Character.CInt["canWork"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_SPIRIT_POSSESSION:
                        Character.canPossession = false;
                        Character.CInt["canPossession"] = 0;
                        break;
                }
        }

        public void ResetOption()
        {
            if (Character == null)
                return;

            Character.canTrade = true;
            Character.canParty = true;
            Character.canWork = true;
            Character.canRing = true;
            Character.canPossession = true;
            Character.canFriend = true;
            Character.showEquipment = true;
            Character.showRevive = true;
            Character.canMentor = true;
            Character.canChangePartnerDisplay = true;
            Character.CInt["canTrade"] = 1;
            Character.CInt["canParty"] = 1;
            Character.CInt["canPossession"] = 1;
            Character.CInt["canRing"] = 1;
            Character.CInt["showRevive"] = 1;
            Character.CInt["canWork"] = 1;
            Character.CInt["canMentor"] = 1;
            Character.CInt["showEquipment"] = 1;
            Character.CInt["canChangePartnerDisplay"] = 1;
            Character.CInt["canFriend"] = 1;
        }

        public void OnPlayerSetTitle(CSMG_PLAYER_SETTITLE p)
        {
            if ((p.GetTSubID < 100000 || CheckTitle((int)p.GetTSubID)) &&
                (p.GetTPredID < 100000 || CheckTitle((int)p.GetTPredID)) &&
                (p.GetTBattleID < 100000 || CheckTitle((int)p.GetTBattleID)))
            {
                Character.AInt["称号_主语"] = (int)p.GetTSubID;
                Character.AInt["称号_连词"] = (int)p.GetTConjID;
                Character.AInt["称号_谓语"] = (int)p.GetTPredID;
                Character.AInt["称号_战斗"] = (int)p.GetTBattleID;
                StatusFactory.Instance.CalcStatus(Character);
                SendPCTitleInfo();
            }
            else
            {
                SendSystemMessage("非法的称号ID");
            }
        }

        public void OnPlayerOpenDailyStamp(CSMG_DAILY_STAMP_OPEN p2)
        {
            SendNPCPlaySound(3501, 0, 100, 50);

            //Hide Daily Stamp Icon
            var ds = new SSMG_PLAYER_SHOW_DAILYSTAMP();
            ds.Type = 0;
            NetIo.SendPacket(ds);


            var thisDay = DateTime.Today;

            if (Character.AStr["DailyStamp_DAY"] != thisDay.ToString("d"))
            {
                if (Character.AInt["每日盖章"] == 10) Character.AInt["每日盖章"] = 0;


                Character.AStr["DailyStamp_DAY"] = thisDay.ToString("d");
                Character.AInt["每日盖章"] += 1;


                var p = new SSMG_NPC_DAILY_STAMP();
                p.StampCount = (byte)Character.AInt["每日盖章"];
                p.Type = 2;
                NetIo.SendPacket(p);

                if (Character.AInt["每日盖章"] == 5)
                {
                    EventActivate(19230002);
                    return;
                }

                if (Character.AInt["每日盖章"] == 10)
                {
                    //Character.AInt["每日盖章"] = 0;

                    EventActivate(19230003);
                    return;
                }

                //Normal Stamp
                EventActivate(19230001);
            }
            else
            {
                var p = new SSMG_NPC_DAILY_STAMP();
                p.StampCount = (byte)Character.AInt["每日盖章"];
                p.Type = 1;
                NetIo.SendPacket(p);
            }
        }

        public void OnPlayerTitleRequire(CSMG_PLAYER_TITLE_REQUIRE p)
        {
            if (p.tID == 9 && Character.Gold >= 10000000)
                TitleProccess(Character, 9, 1);

            /*
            p2.tID = p.tID;
            p2.mark = 1;
            if (Character.AInt["称号" + p.tID.ToString() + "完成度"] != 0)
                p2.task = (ulong)Character.AInt["称号" + p.tID.ToString() + "完成度"];
            else p2.task = 0;
            */
            uint id = p.ID;
            if (TitleFactory.Instance.Items.ContainsKey(id))
            {
                var p2 = new SSMG_PLAYER_TITLE_REQ();
                p2.tID = TitleFactory.Instance.Items[id].ID;
                p2.PutPrerequisite(TitleFactory.Instance.Items[id].Prerequisites.Values.ToList());
                NetIo.SendPacket(p2);
            }
        }

        public void OnBondRequestFromMaster(CSMG_BOND_REQUEST_FROM_MASTER p)
        {
            var pupilin = MapClientManager.Instance.FindClient(p.CharID);
            SendSystemMessage("师徒系统尚未实装。");

            var result = CheckMasterToPupilinInvitation(pupilin);
            var p1 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p1.Result = result;
            NetIo.SendPacket(p1);
        }

        public int CheckMasterToPupilinInvitation(MapClient pupilin)
        {
            if (pupilin == null)
                return -2; //相手が見つかりませんでした
            if (!pupilin.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (pupilin.trading || pupilin.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Pupilins.Count >= Character.PupilinLimit)
                return -7; //これ以上弟子を取ることが出来ません
            if (pupilin.Character.Level >= 50 || pupilin.Character.JobLevel2T != 0 ||
                pupilin.Character.JobLevel2X != 0 || pupilin.Character.Rebirth)
                return -6; //そのキャラクターは弟子になれません
            if (pupilin.masterpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Pupilins.Contains(pupilin.Character.CharID))
                return -3; //既に師匠がいます
            try
            {
                var p2 = new SSMG_BOND_INVITE_TO_PUPILIN();
                p2.MasterID = Character.CharID;
                pupilin.NetIo.SendPacket(p2);
                pupilinpartner = pupilin.Character.CharID;
                pupilin.masterpartner = Character.CharID;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return -11; //申請中です
        }

        public void OnBondRequestFromPupilin(CSMG_BOND_REQUEST_FROM_PUPILIN p)
        {
            var master = MapClientManager.Instance.FindClient(p.CharID);
            SendSystemMessage("师徒系统尚未实装。");

            var result = CheckPupilinToMasterInvitation(master);
            var p1 = new SSMG_BOND_INVITE_PUPILIN_RESULT();
            p1.Result = result;
            NetIo.SendPacket(p1);
        }

        public int CheckPupilinToMasterInvitation(MapClient master)
        {
            if (master == null)
                return -2; //相手が見つかりませんでした
            if (!master.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (master.trading || master.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Master == master.Character.CharID)
                return -7; //既に師匠がいます
            if (master.Character.Level <= 55 || !master.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (master.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Master != 0)
                return -3; //既に違う人の弟子になっています
            try
            {
                var p2 = new SSMG_BOND_INVITE_TO_MASTER();
                p2.PupilinID = Character.CharID;
                master.NetIo.SendPacket(p2);
                masterpartner = master.Character.CharID;
                master.pupilinpartner = Character.CharID;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return -11; //申請中です
        }

        public void OnBondPupilinAnswer(CSMG_BOND_REQUEST_PUPILIN_ANSWER p)
        {
            var master = MapClientManager.Instance.FindClient(p.MasterCharID);
            var result = CheckPupilinToMasterAnswer(p.Rejected, master);
            var p2 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p2.Result = result;
            master.NetIo.SendPacket(p2);
        }

        public int CheckPupilinToMasterAnswer(bool rejected, MapClient master)
        {
            if (rejected)
                return -4; //拒否されました
            if (master == null)
                return -2; //相手が見つかりませんでした
            if (!master.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (master.trading || master.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Master == master.Character.CharID)
                return -7; //既に師匠がいます
            if (master.Character.Level <= 55 || !master.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (master.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Master != 0)
                return -3; //既に違う人の弟子になっています
            try
            {
                Character.Master = master.Character.CharID;
                master.Character.Pupilins.Add(Character.CharID);
                masterpartner = 0;
                master.pupilinpartner = 0;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //師匠になりました
        }

        public void OnBondMasterAnswer(CSMG_BOND_REQUEST_MASTER_ANSWER p)
        {
            var pupilin = MapClientManager.Instance.FindClient(p.PupilinCharID);
            var result = CheckMasterToPupilinAnswer(p.Rejected, pupilin);
            var p2 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p2.Result = result;
            pupilin.NetIo.SendPacket(p2);
        }

        public int CheckMasterToPupilinAnswer(bool rejected, MapClient pupilin)
        {
            if (rejected)
                return -4; //拒否されました
            if (pupilin == null)
                return -2; //相手が見つかりませんでした
            if (!pupilin.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (pupilin.trading || pupilin.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Pupilins.Count >= Character.PupilinLimit)
                return -7; //これ以上弟子を取ることが出来ません
            if (pupilin.Character.Level <= 55 || !pupilin.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (pupilin.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Pupilins.Contains(pupilin.Character.CharID))
                return -3; //既に師匠がいます
            try
            {
                Character.Pupilins.Add(pupilin.Character.CharID);
                pupilin.Character.Master = Character.CharID;
                pupilinpartner = 0;
                pupilin.masterpartner = 0;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //師匠になりました
        }

        public void OnBondBreak(CSMG_BOND_CANCEL p)
        {
            var target = MapClientManager.Instance.FindClient(p.TargetCharID);
            SendSystemMessage("師徒系統尚未實裝。");
            try
            {
                if (Character.Pupilins.Contains(target.Character.CharID))
                    Character.Pupilins.Remove(target.Character.CharID);
                if (Character.Master == target.Character.CharID)
                    Character.Master = 0;
                if (target.Character.Pupilins.Contains(Character.CharID))
                    target.Character.Pupilins.Remove(Character.CharID);
                if (target.Character.Master == Character.CharID)
                    target.Character.Master = 0;
            }
            catch
            {
            }

            var p1 = new SSMG_BOND_BREAK_RESULT();
            NetIo.SendPacket(p1);
            var p2 = new SSMG_BOND_BREAK_RESULT();
            target.NetIo.SendPacket(p2);
        }

        public void OnPlayerCancleTitleNew(CSMG_PLAYER_TITLE_CANCLENEW p)
        {
            var index = (int)p.tID;
            byte page = 1;
            var bounsflag = false;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var value = new BitMask_Long();
            var name = "N称号记录" + page;
            if (Character.AStr[name] == "")
            {
                value.Value = 0;
            }
            else
            {
                value.Value = ulong.Parse(Character.AStr[name]);
                if (value.Test((ulong)Math.Pow(2, index - 1)))
                    bounsflag = true;
                value.SetValueForNum(index, false);
            }

            Character.AStr[name] = value.Value.ToString();

            if (bounsflag)
            {
                var t = TitleFactory.Instance.Items[p.tID];
                foreach (var item in t.Bonus.Keys)
                {
                    var it = ItemFactory.Instance.GetItem(item);
                    it.Stack = t.Bonus[item];
                    AddItem(it, true);
                }

                if (t.Bonus.Count > 0)
                    SendSystemMessage("获得了称号『" + t.name + "』的奖励！");
                else
                    SendSystemMessage("称号『" + t.name + "』没有物品奖励。");
            }
        }

        public bool CheckTitle(int ID)
        {
            if (ID > 100000) return true;
            var index = ID;
            byte page = 1;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var value = new BitMask_Long();
            var name = "称号记录" + page;
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            var mark = (ulong)Math.Pow(2, index - 1);
            return value.Test(mark);
        }

        public void SendPCTitleInfo()
        {
            if (Character == null)
                return;
            var p = new SSMG_PLAYER_TITLE();
            var titles = new List<uint>();
            titles.Add((uint)Character.AInt["称号_主语"]);
            titles.Add((uint)Character.AInt["称号_连词"]);
            titles.Add((uint)Character.AInt["称号_谓语"]);
            titles.Add((uint)Character.AInt["称号_战斗"]);

            NetIo.SendPacket(p);
            StatusFactory.Instance.CalcStatus(Character);
        }

        public void SendTitleList()
        {
            if (Character == null)
                return;
            var p = new SSMG_PLAYER_TITLE_LIST();
            var unknown1 = new List<ulong>();
            unknown1.Add(0);
            unknown1.Add(0);
            unknown1.Add(0);
            p.PutUnknown1(unknown1);

            var unknown2 = new List<ulong>();
            unknown2.Add(0);
            unknown2.Add(0);
            unknown2.Add(0);
            p.PutUnknown2(unknown2);

            var titles = new List<ulong>();
            if (Character.AStr["称号记录1"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录1"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录2"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录2"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录3"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录3"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录4"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录4"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录5"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录5"]));
            else
                titles.Add(0);
            p.PutTitles(titles);

            var ntitles = new List<ulong>();
            if (Character.AStr["N称号记录1"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录1"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录2"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录2"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录3"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录3"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录4"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录4"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录5"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录5"]));
            else
                ntitles.Add(0);
            p.PutTitles(ntitles);

            NetIo.SendPacket(p);
        }

        public void SetTitle(int n, bool v)
        {
            var index = n;
            var value = new BitMask_Long();
            byte page = 1;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var name = "称号记录" + page;
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            value.SetValueForNum(index, v);
            Character.AStr[name] = value.Value.ToString();


            name = "N" + name;
            value = new BitMask_Long();
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            value.SetValueForNum(index, v);
            Character.AStr[name] = value.Value.ToString();

            SendTitleList();
        }

        public void SendPetInfo()
        {
            if (Character.Partner == null)
                return;
            Partner.StatusFactory.Instance.CalcPartnerStatus(Character.Partner);
            SendPetDetailInfo();
            SendPetBasicInfo();
        }

        public void SendPetDetailInfo()
        {
            if (Character.Partner != null)
            {
                var p = new SSMG_PARTNER_INFO_DETAIL();
                var pet = Character.Partner;
                p.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                p.MaxHP = pet.MaxHP;
                p.MaxMP = pet.MaxMP;
                p.MaxSP = pet.MaxSP;
                p.MoveSpeed = pet.Speed;
                p.MinPhyATK = pet.Status.min_atk1;
                p.MaxPhyATK = pet.Status.max_atk1;
                p.MinMAGATK = pet.Status.min_matk;
                p.MaxMAGATK = pet.Status.max_matk;
                p.DEF = pet.Status.def;
                p.DEFAdd = pet.Status.def_add;
                p.MDEF = pet.Status.mdef;
                p.MDEFAdd = pet.Status.mdef_add;
                p.ShortHit = pet.Status.hit_melee;
                p.LongHit = pet.Status.hit_ranged;
                p.ShortAvoid = pet.Status.avoid_melee;
                p.LongAvoid = pet.Status.avoid_ranged;
                p.ASPD = pet.Status.aspd;
                p.CSPD = pet.Status.cspd;
                NetIo.SendPacket(p);
            }
        }

        public void SendPetBasicInfo()
        {
            if (Character.Partner != null)
            {
                var p = new SSMG_PARTNER_INFO_BASIC();
                var pet = Character.Partner;
                p.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                p.Level = pet.Level;

                var bexp = ExperienceManager.Instance.GetExpForLevel(pet.Level, LevelType.CLEVEL2);
                var nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(pet.Level + 1), LevelType.CLEVEL2);
                var cexp = (uint)((float)(pet.exp - bexp) / (nextExp - bexp) * 1000);

                p.EXPPercentage = cexp;
                p.Rebirth = 0;
                if (pet.rebirth)
                    p.Rebirth = 1;
                var rank = (byte)(pet.BaseData.base_rank + pet.rank);
                p.Rank = rank;
                p.ReliabilityColor = pet.reliability;
                p.ReliabilityUpRate = pet.reliabilityuprate;

                if (pet.nextfeedtime > DateTime.Now)
                    p.NextFeedTime = (uint)(pet.nextfeedtime - DateTime.Now).TotalSeconds;
                else
                    p.NextFeedTime = 0;

                p.AIMode = pet.ai_mode;
                //p.MaxNextFeedTime = no data
                //p.CustomAISheet = no data
                //p.AICommandCount1 = no data
                //p.AICommandCount2 = no data
                p.PerkPoint = pet.perkpoint;
                //p.PerkListCount = no data
                p.Perk0 = pet.perk0;
                p.Perk1 = pet.perk1;
                p.Perk2 = pet.perk2;
                p.Perk3 = pet.perk3;
                p.Perk4 = pet.perk4;
                p.Perk5 = pet.perk5;
                if (pet.equipments.ContainsKey(EnumPartnerEquipSlot.WEAPON))
                    p.WeaponID = pet.equipments[EnumPartnerEquipSlot.WEAPON].ItemID;
                if (pet.equipments.ContainsKey(EnumPartnerEquipSlot.COSTUME))
                    p.ArmorID = pet.equipments[EnumPartnerEquipSlot.COSTUME].ItemID;
                NetIo.SendPacket(p);
            }
        }

        public void OnAnoPaperEquip(CSMG_ANO_PAPER_EQUIP p)
        {
            if (Character.AnotherPapers.ContainsKey(p.paperID))
            {
                Character.UsingPaperID = p.paperID;
                StatusFactory.Instance.CalcStatus(Character);
                var p1 = new SSMG_ANO_EQUIP_RESULT();
                p1.PaperID = Character.UsingPaperID;
                SendPlayerInfo();
                NetIo.SendPacket(p1);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PAPER_CHANGE, null, Character, true);
            }
        }

        public void OnAnoPaperTakeOff(CSMG_ANO_PAPER_TAKEOFF p)
        {
            Character.UsingPaperID = 0;
            StatusFactory.Instance.CalcStatus(Character);
            var p1 = new SSMG_ANO_TAKEOFF_RESULT();
            p1.PaperID = Character.UsingPaperID;
            NetIo.SendPacket(p1);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PAPER_CHANGE, null, Character, true);
            SendPlayerInfo();
        }

        public void CreateAnotherPaper(uint paperID)
        {
            var detail = new AnotherDetail();
            detail.value = new BitMask_Long();
            detail.lv = 0;
            foreach (var item in AnotherFactory.Instance.AnotherPapers[paperID].Keys)
                if (!detail.skills.ContainsKey(item))
                    detail.skills.Add(item, 0);
            if (!Character.AnotherPapers.ContainsKey(paperID))
                Character.AnotherPapers.Add(paperID, detail);
        }

        public void OnAnoPaperCompound(CSMG_ANO_PAPER_COMPOUND p)
        {
            var penItem = Character.Inventory.GetItem(p.SlotID);
            var paperID = p.paperID;
            var lv = (byte)(Character.AnotherPapers[paperID].lv + 1);
            var value = (ulong)(0xff << (8 * (lv - 1)));
            if (lv == 1) value = 0xff;
            if (Character.AnotherPapers[paperID].value.Test(value))
            {
                if (lv > 1)
                {
                    uint penID = 0;
                    if (AnotherFactory.Instance.AnotherPapers[paperID][lv].requestItem2 == penItem.ItemID)
                        penID = penItem.ItemID;
                    else if (AnotherFactory.Instance.AnotherPapers[paperID][lv].requestItem1 == penItem.ItemID)
                        penID = penItem.ItemID;
                    else return;
                    if (CountItem(penID) < 1) return;
                    DeleteItemID(penID, 1, true);
                }

                Character.AnotherPapers[paperID].lv = lv;
                var p2 = new SSMG_ANO_PAPER_COMPOUND_RESULT();
                p2.lv = lv;
                p2.paperID = paperID;
                NetIo.SendPacket(p2);
            }
            else
            {
                return;
            }
        }

        public void OnAnoPaperUse(CSMG_ANO_PAPER_USE p)
        {
            var paperItem = Character.Inventory.GetItem(p.slotID);
            if (paperItem != null)
            {
                var paperID = p.paperID;
                if (AnotherFactory.Instance.AnotherPapers[paperID][1].paperItems1.Contains(paperItem.ItemID))
                {
                    var lv = AnotherFactory.Instance.GetPaperLv(Character.AnotherPapers[paperID].value.Value);
                    var value = GetPaperValue(paperID, (byte)(lv + 1), paperItem.ItemID);
                    if (value == 0) return;
                    if (!Character.AnotherPapers[paperID].value.Test(value))
                        Character.AnotherPapers[paperID].value.SetValue(value, true);
                    else return;
                    DeleteItem(p.slotID, 1, true);
                    var p2 = new SSMG_ANO_PAPER_USE_RESULT();
                    p2.value = Character.AnotherPapers[paperID].value.Value;
                    p2.paperID = paperID;
                    NetIo.SendPacket(p2);
                    MapServer.charDB.SavePaper(Character);
                }
            }
        }

        public ulong GetPaperValue(byte paperID, byte lv, uint ItemID)
        {
            ulong value = 0;
            if (!AnotherFactory.Instance.AnotherPapers.ContainsKey(paperID)) return 0;
            if (!AnotherFactory.Instance.AnotherPapers[paperID].ContainsKey(lv)) return 0;
            if (!AnotherFactory.Instance.AnotherPapers[paperID][lv].paperItems1.Contains(ItemID)) return 0;
            var index = AnotherFactory.Instance.AnotherPapers[paperID][lv].paperItems1.IndexOf(ItemID);
            switch (index)
            {
                case 0:
                    value = 0x1;
                    break;
                case 1:
                    value = 0x2;
                    break;
                case 2:
                    value = 0x4;
                    break;
                case 3:
                    value = 0x8;
                    break;
                case 4:
                    value = 0x10;
                    break;
                case 5:
                    value = 0x20;
                    break;
                case 6:
                    value = 0x40;
                    break;
                case 7:
                    value = 0x80;
                    break;
            }

            value = value << (8 * (lv - 1));
            return value;
        }

        public void OnAnoUIOpen(CSMG_ANO_UI_OPEN p)
        {
            try
            {
                CreateAnotherPaper(1);
                CreateAnotherPaper(2);
                CreateAnotherPaper(4);
                CreateAnotherPaper(6);
                CreateAnotherPaper(7);
                CreateAnotherPaper(8);
                CreateAnotherPaper(9);
                CreateAnotherPaper(10);
                CreateAnotherPaper(11);
                CreateAnotherPaper(13);
                var List1 = new List<ushort>();
                var List2 = new List<ulong>();
                var List3 = new List<byte>();
                var p2 = new SSMG_ANO_SHOW_INFOBOX();
                p2.index = p.index;
                p2.cexp = 0;
                p2.usingPaperID = Character.UsingPaperID;
                foreach (var item in Character.AnotherPapers.Keys)
                    if (AnotherFactory.Instance.AnotherPapers.ContainsKey(item))
                        if (AnotherFactory.Instance.AnotherPapers[item][1].type == p.index)
                            List1.Add((ushort)item);

                p2.papersID = List1;
                if (Character.UsingPaperID != 0)
                    p2.usingPaperValue = Character.AnotherPapers[Character.UsingPaperID].value.Value;
                for (var i = 0; i < List1.Count; i++) List2.Add(Character.AnotherPapers[List1[i]].value.Value);
                p2.paperValues = List2;
                if (Character.UsingPaperID != 0)
                    p2.usingLv =
                        Character.AnotherPapers[Character.UsingPaperID]
                            .lv; //AnotherFactory.Instance.GetPaperLv(this.Character.AnotherPapers[this.Character.UsingPaperID].value.Value);
                for (var i = 0; i < List1.Count; i++)
                    List3.Add(Character.AnotherPapers[List1[i]]
                        .lv); //AnotherFactory.Instance.GetPaperLv(this.Character.AnotherPapers[List1[i]].value.Value));
                p2.papersLv = List3;
                /*if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_1 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[0];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[1]);
                }
                p2.paperSkillsEXP_1 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_2 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[1];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[2]);
                }
                p2.paperSkillsEXP_2 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_3 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[2];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[3]);
                }
                p2.paperSkillsEXP_3 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_4 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[3];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[4]);
                }
                p2.paperSkillsEXP_4 = List2;*/
                NetIo.SendPacket(p2);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnMirrorOpenRequire(CSMG_PLAYER_MIRROR_OPEN p)
        {
            var p1 = new SSMG_PLAYER_OPEN_MIRROR_WINDOW();
            p1.SetFace = new List<ushort>(15);
            p1.SetHairStyle = new List<ushort>(15);
            p1.SetHairColor = new List<ushort>(15);
            p1.SetUnknow = new List<uint>(15);
            p1.SetHairColorStorageSlot = new List<byte>(15);
            NetIo.SendPacket(p1);
        }

        public void OnRequireRebirthReward(CSMG_PLAYER_REQUIRE_REBIRTHREWARD p)
        {
            var p1 = new SSMG_PLAYER_OPEN_REBIRTHREWARD_WINDOW();
            p1.SetOpen = 0x0A;
            NetIo.SendPacket(p1);
        }

        public void OnCharFormChange(CSMG_CHAR_FORM p)
        {
            Character.TailStyle = p.tailstyle;
            Character.WingStyle = p.wingstyle;
            Character.WingColor = p.wingcolor;
            Character.e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
        }

        public void OnPlayerFaceView(CSMG_ITEM_FACEVIEW p)
        {
            var p2 = new Packet(3);
            p2.ID = 0x1CF3;
            NetIo.SendPacket(p2);
        }

        public void OnPlayerFaceChange(CSMG_ITEM_FACECHANGE p)
        {
            var itemID = Character.Inventory.GetItem(p.SlotID).ItemID;
            if (itemID == FaceFactory.Instance.Faces[p.FaceID])
            {
                DeleteItem(p.SlotID, 1, true);
                Character.Face = p.FaceID;
                SendPlayerInfo();
            }
        }

        /*public void SendNaviList(Packets.Client.CSMG_NAVI_OPEN p)
        {
            Packets.Server.SSMG_NAVI_LIST p1 = new Packets.Server.SSMG_NAVI_LIST();
            p1.CategoryId = p.CategoryId;
            p1.Count = 18;
            p1.Navi = this.Character.Navi;
            this.NetIo.SendPacket(p1);
        }*/
        public void SendAnotherButton()
        {
            var p = new SSMG_ANO_BUTTON_APPEAR();
            p.Type = 1;
            NetIo.SendPacket(p);
        }

        public void SendRingFF()
        {
            MapServer.charDB.GetFlyCastle(Character);
            if (Character.Ring != null)
                if (Character.Ring.FlyingCastle != null)
                {
                    SendRingFFObtainMode();
                    SendRingFFHealthMode();
                    SendRingFFIsLock();
                    SendRingFFName();
                    SendRingFFMaterialPoint();
                    SendRingFFMaterialConsume();
                    SendRingFFLevel();
                    SendRingFFNextFeeTime();
                }
        }

        private void SendRingFFObtainMode()
        {
            var p = new SSMG_FF_OBTAIN_MODE();
            p.value = Character.Ring.FlyingCastle.ObMode;
            NetIo.SendPacket(p);
        }

        private void SendRingFFHealthMode()
        {
            var p = new SSMG_FF_HEALTH_MODE();
            p.value = Character.Ring.FlyingCastle.HealthMode;
            NetIo.SendPacket(p);
        }

        private void SendRingFFIsLock()
        {
            var p = new SSMG_FF_ISLOCK();
            if (Character.Ring.FlyingCastle.IsLock)
                p.value = 1;
            else
                p.value = 0;
            NetIo.SendPacket(p);
        }

        private void SendRingFFName()
        {
            var p = new SSMG_FF_RINGSELF();
            p.name = Character.Ring.FlyingCastle.Name;
            NetIo.SendPacket(p);
        }

        private void SendRingFFMaterialPoint()
        {
            var p = new SSMG_FF_MATERIAL_POINT();
            p.value = Character.Ring.FlyingCastle.MaterialPoint;
            NetIo.SendPacket(p);
        }

        private void SendRingFFMaterialConsume()
        {
            var p = new SSMG_FF_MATERIAL_CONSUME();
            p.value = Character.Ring.FlyingCastle.MaterialConsume;
            NetIo.SendPacket(p);
        }

        private void SendRingFFLevel()
        {
            var p = new SSMG_FF_LEVEL();
            p.level = Character.Ring.FlyingCastle.Level;
            p.value = Character.Ring.FlyingCastle.FFexp;
            NetIo.SendPacket(p);
            var p1 = new SSMG_FF_F_LEVEL();
            p1.level = Character.Ring.FlyingCastle.FLevel;
            p1.value = Character.Ring.FlyingCastle.FFFexp;
            NetIo.SendPacket(p1);
            var p2 = new SSMG_FF_SU_LEVEL();
            p2.level = Character.Ring.FlyingCastle.SULevel;
            p2.value = Character.Ring.FlyingCastle.FFSUexp;
            NetIo.SendPacket(p2);
            var p3 = new SSMG_FF_BP_LEVEL();
            p3.level = Character.Ring.FlyingCastle.BPLevel;
            p3.value = Character.Ring.FlyingCastle.FFBPexp;
            NetIo.SendPacket(p3);
            var p4 = new SSMG_FF_DEM_LEVEL();
            p4.level = Character.Ring.FlyingCastle.DEMLevel;
            p4.value = Character.Ring.FlyingCastle.FFDEMexp;
            NetIo.SendPacket(p4);
        }

        private void SendRingFFNextFeeTime()
        {
            var p = new SSMG_FF_NEXTFEE_DATE();
            p.UpdateTime = DateTime.Now;
            NetIo.SendPacket(p);
        }


        private void SendEffect(uint effect)
        {
            var arg = new EffectArg();
            arg.actorID = Character.ActorID;
            arg.effectID = effect;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Character, true);
        }

        public void ResetStatusPoint()
        {
            var setting = Configuration.Configuration.Instance.StartupSetting[Character.Race];
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Str, Character.Str);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Dex, Character.Dex);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Int, Character.Int);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Vit, Character.Vit);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Agi, Character.Agi);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Mag, Character.Mag);

            Character.Str = setting.Str;
            Character.Dex = setting.Dex;
            Character.Int = setting.Int;
            Character.Vit = setting.Vit;
            Character.Agi = setting.Agi;
            Character.Mag = setting.Mag;

            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        public void SendRange()
        {
            var p = new SSMG_ITEM_EQUIP();
            p.InventorySlot = 0xFFFFFFFF;
            p.Target = ContainerType.NONE;
            p.Result = 1;
            p.Range = Character.Range;
            NetIo.SendPacket(p);
        }

        public void SendActorID()
        {
            var p2 = new SSMG_ACTOR_SPEED();
            p2.ActorID = Character.ActorID;
            p2.Speed = Character.Speed;
            //p2.Speed = 96;
            NetIo.SendPacket(p2);
        }

        public void SendStamp()
        {
            var p1 = new SSMG_STAMP_INFO();
            p1.Page = 0;
            p1.Stamp = Character.Stamp;
            NetIo.SendPacket(p1);
            var p2 = new SSMG_STAMP_INFO();
            p2.Page = 1;
            p2.Stamp = Character.Stamp;
            NetIo.SendPacket(p2);
        }

        public void SendActorMode()
        {
            Character.e.OnPlayerMode(Character);
        }

        public void SendCharOption()
        {
            var sum = 0;
            if (Character.CInt["canTrade"] == 0) sum += 1;
            if (Character.CInt["canParty"] == 0) sum += 2;
            if (Character.CInt["canPossession"] == 0) sum += 4;
            if (Character.CInt["canRing"] == 0) sum += 8;
            if (Character.CInt["showRevive"] == 0) sum += 16;
            if (Character.CInt["canWork"] == 0) sum += 32;
            if (Character.CInt["canMentor"] == 0) sum += 256;
            if (Character.CInt["showEquipment"] == 0) sum += 512;
            if (Character.CInt["canChangePartnerDisplay"] == 0) sum += 1024;
            if (Character.CInt["canFriend"] == 0) sum += 2048;

            if (sum == 0)
            {
                var p4 = new SSMG_ACTOR_OPTION();
                p4.Option = SSMG_ACTOR_OPTION.Options.NONE;
                NetIo.SendPacket(p4);
            }
            else
            {
                var p4 = new SSMG_ACTOR_OPTION();
                p4.RawOption = sum;
                NetIo.SendPacket(p4);
            }
        }

        public void SendCharInfo()
        {
            if (Character.Online)
            {
                SkillHandler.Instance.CastPassiveSkills(Character);

                SendAttackType();
                var p1 = new SSMG_PLAYER_INFO();
                p1.Player = Character;
                NetIo.SendPacket(p1);

                SendPlayerInfo();
            }
        }

        public void SendPlayerInfo()
        {
            if (Character.Online)
            {
                SendPlayerStatsBreak(Character);
                SendGoldUpdate();
                SendActorHPMPSP(Character);
                SendStatus();
                SendRange();
                SendStatusExtend();
                SendCapacity();
                //SendMaxCapacity();
                SendPlayerJob();
                SendSkillList();
                SendPlayerLevel();
                SendEXP();
                SendActorMode();
                SendCL();
                SendMotionList();
                SendPlayerEXPoints(Character);

                SendPlayerDualJobInfo();
                SendPlayerDualJobSkillList();
            }
        }

        private void SendPlayerStatsBreak(ActorPC actor)
        {
            var p = new SSMG_PLAYER_STATS_BREAK();
            p.STATS = (byte)(StatsBreakType.Str | StatsBreakType.Agi | StatsBreakType.Vit | StatsBreakType.Int |
                             StatsBreakType.Dex | StatsBreakType.Mag);
            NetIo.SendPacket(p);
        }

        private void SendPlayerEXPoints(ActorPC actor)
        {
            var p = new SSMG_PLAYER_EXPOINT();
            p.EXStatPoint = actor.EXStatPoint;
            p.CanUseStatPoint = actor.StatsPoint;
            p.EXSkillPoint = actor.EXSkillPoint;
            NetIo.SendPacket(p);
        }

        public void SendMotionList()
        {
            if (Character.Online)
            {
                var p = new SSMG_CHAT_EXPRESSION_UNLOCK();
                p.unlock = 0xffffffff;
                NetIo.SendPacket(p);
                var p2 = new SSMG_CHAT_EXEMOTION_UNLOCK();
                p2.List1 = 0xffffffff;
                p2.List2 = 0xffffffff;
                p2.List3 = 0xffffffff;
                p2.List4 = 0xffffffff;
                p2.List5 = 0xffffffff;
                NetIo.SendPacket(p2);
            }
        }

        public void SendAttackType()
        {
            if (Character.Online)
            {
                //去掉攻击类型消息显示
                Dictionary<EnumEquipSlot, Item> equips;
                if (Character.Form == DEM_FORM.NORMAL_FORM)
                    equips = Character.Inventory.Equipments;
                else
                    equips = Character.Inventory.Parts;
                if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND) || (equips.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                                                                     equips[EnumEquipSlot.LEFT_HAND].BaseData
                                                                         .itemType == ItemType.BOW))
                {
                    var item = new Item();
                    if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                        equips[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BOW)
                        item = equips[EnumEquipSlot.LEFT_HAND];
                    else
                        item = equips[EnumEquipSlot.RIGHT_HAND];
                    Character.Status.attackType = item.AttackType;
                    //switch (item.AttackType)
                    //{
                    //    case ATTACK_TYPE.BLOW:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREBLOW_TEXT);
                    //        break;
                    //    case ATTACK_TYPE.STAB:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTURESTAB_TEXT);
                    //        break;
                    //    case ATTACK_TYPE.SLASH:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTURESLASH_TEXT);
                    //        break;
                    //    default:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREERROR_TEXT);
                    //        break;
                    //}
                }
                else
                {
                    Character.Status.attackType = ATTACK_TYPE.BLOW;
                    //SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREBLOW_TEXT);
                }

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK_TYPE_CHANGE, null, Character, true);
            }
        }

        public void SendStatus()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_STATUS();
                if (Character.Form == DEM_FORM.MACHINA_FORM || Character.Race != PC_RACE.DEM)
                {
                    p.AgiBase = (ushort)(Character.Agi + Character.Status.m_agi_chip);
                    p.AgiRevide = (short)(Character.Status.agi_rev + Character.Status.agi_item +
                                          Character.Status.agi_mario + Character.Status.agi_skill +
                                          Character.Status.agi_iris);
                    p.AgiBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                    p.DexBase = (ushort)(Character.Dex + Character.Status.m_dex_chip);
                    p.DexRevide = (short)(Character.Status.dex_rev + Character.Status.dex_item +
                                          Character.Status.dex_mario + Character.Status.dex_skill +
                                          Character.Status.dex_iris);
                    p.DexBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                    p.IntBase = (ushort)(Character.Int + Character.Status.m_int_chip);
                    p.IntRevide = (short)(Character.Status.int_rev + Character.Status.int_item +
                                          Character.Status.int_mario + Character.Status.int_skill +
                                          Character.Status.int_iris);
                    p.IntBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                    p.VitBase = (ushort)(Character.Vit + Character.Status.m_vit_chip);
                    p.VitRevide = (short)(Character.Status.vit_rev + Character.Status.vit_item +
                                          Character.Status.vit_mario + Character.Status.vit_skill +
                                          Character.Status.vit_iris);
                    p.VitBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                    p.StrBase = (ushort)(Character.Str + Character.Status.m_str_chip);
                    p.StrRevide = (short)(Character.Status.str_rev + Character.Status.str_item +
                                          Character.Status.str_mario + Character.Status.str_skill +
                                          Character.Status.str_iris);
                    p.StrBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                    p.MagBase = (ushort)(Character.Mag + Character.Status.m_mag_chip);
                    p.MagRevide = (short)(Character.Status.mag_rev + Character.Status.mag_item +
                                          Character.Status.mag_mario + Character.Status.mag_skill +
                                          Character.Status.mag_iris);
                    p.MagBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                    NetIo.SendPacket(p);
                }
                else
                {
                    p.AgiBase = (ushort)(Character.Agi + Character.Status.m_agi_chip);
                    p.AgiRevide = (short)(Character.Status.agi_rev - Character.Status.m_agi_chip +
                                          Character.Status.agi_item + Character.Status.agi_mario +
                                          Character.Status.agi_skill + Character.Status.agi_iris);
                    p.AgiBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                    p.DexBase = (ushort)(Character.Dex + Character.Status.m_dex_chip);
                    p.DexRevide = (short)(Character.Status.dex_rev - Character.Status.m_dex_chip +
                                          Character.Status.dex_item + Character.Status.dex_mario +
                                          Character.Status.dex_skill + Character.Status.dex_iris);
                    p.DexBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                    p.IntBase = (ushort)(Character.Int + Character.Status.m_int_chip);
                    p.IntRevide = (short)(Character.Status.int_rev - Character.Status.m_int_chip +
                                          Character.Status.int_item + Character.Status.int_mario +
                                          Character.Status.int_skill + Character.Status.int_iris);
                    p.IntBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                    p.VitBase = (ushort)(Character.Vit + Character.Status.m_vit_chip);
                    p.VitRevide = (short)(Character.Status.vit_rev - Character.Status.m_vit_chip +
                                          Character.Status.vit_item + Character.Status.vit_mario +
                                          Character.Status.vit_skill + Character.Status.vit_iris);
                    p.VitBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                    p.StrBase = (ushort)(Character.Str + Character.Status.m_str_chip);
                    p.StrRevide = (short)(Character.Status.str_rev - Character.Status.m_str_chip +
                                          Character.Status.str_item + Character.Status.str_mario +
                                          Character.Status.str_skill + Character.Status.str_iris);
                    p.StrBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                    p.MagBase = (ushort)(Character.Mag + Character.Status.m_mag_chip);
                    p.MagRevide = (short)(Character.Status.mag_rev - Character.Status.m_mag_chip +
                                          Character.Status.mag_item + Character.Status.mag_mario +
                                          Character.Status.mag_skill + Character.Status.mag_iris);
                    p.MagBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Mag);

                    NetIo.SendPacket(p);
                }
            }
        }

        public void SendStatusExtend()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_STATUS_EXTEND();

                switch (Character.Status.attackType)
                {
                    case ATTACK_TYPE.BLOW:
                        p.ATK1Max = Character.Status.max_atk1;
                        p.ATK1Min = Character.Status.min_atk1;
                        break;
                    case ATTACK_TYPE.SLASH:
                        p.ATK1Max = Character.Status.max_atk2;
                        p.ATK1Min = Character.Status.min_atk2;
                        break;
                    case ATTACK_TYPE.STAB:
                        p.ATK1Max = Character.Status.max_atk3;
                        p.ATK1Min = Character.Status.min_atk3;
                        break;
                }

                p.ATK2Max = Character.Status.max_atk2;
                p.ATK2Min = Character.Status.min_atk2;
                p.ATK3Max = Character.Status.max_atk3;
                p.ATK3Min = Character.Status.min_atk3;
                p.MATKMax = Character.Status.max_matk;
                p.MATKMin = Character.Status.min_matk;

                p.ASPD = Character.Status.aspd; // + this.Character.Status.aspd_skill);
                p.CSPD = Character.Status.cspd; // + this.Character.Status.cspd_skill);

                p.AvoidCritical = Character.Status.avoid_critical;
                p.AvoidMagic = Character.Status.avoid_magic;
                p.AvoidMelee = Character.Status.avoid_melee;
                p.AvoidRanged = Character.Status.avoid_ranged;

                p.DefAddition = (ushort)Character.Status.def_add;
                p.DefBase = Character.Status.def;
                p.MDefAddition = (ushort)Character.Status.mdef_add;
                p.MDefBase = Character.Status.mdef;

                p.HitCritical = Character.Status.hit_critical;
                p.HitMagic = Character.Status.hit_magic;
                p.HitMelee = Character.Status.hit_melee;
                p.HitRanged = Character.Status.hit_ranged;

                p.Speed = Character.Speed;

                NetIo.SendPacket(p);
            }
        }

        public void SendCapacity()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_CAPACITY();
                /*p.CapacityBack = this.Character.Inventory.Volume[ContainerType.BACK_BAG];
                p.CapacityBody = this.Character.Inventory.Volume[ContainerType.BODY];
                p.CapacityLeft = this.Character.Inventory.Volume[ContainerType.LEFT_BAG];
                p.CapacityRight = this.Character.Inventory.Volume[ContainerType.RIGHT_BAG];
                p.PayloadBack = this.Character.Inventory.Payload[ContainerType.BACK_BAG];
                p.PayloadBody = this.Character.Inventory.Payload[ContainerType.BODY];
                p.PayloadLeft = this.Character.Inventory.Payload[ContainerType.LEFT_BAG];
                p.PayloadRight = this.Character.Inventory.Payload[ContainerType.RIGHT_BAG];*/
                p.Payload = Character.Inventory.Payload[ContainerType.BODY];
                p.Volume = Character.Inventory.Volume[ContainerType.BODY];
                p.MaxPayload = Character.Inventory.MaxPayload[ContainerType.BODY];
                p.MaxVolume = Character.Inventory.MaxVolume[ContainerType.BODY];
                NetIo.SendPacket(p);
            }
        }

        public void SendMaxCapacity()
        {
            /*if (this.Character.Online)
            {
                Packets.Server.SSMG_PLAYER_MAX_CAPACITY p = new SagaMap.Packets.Server.SSMG_PLAYER_MAX_CAPACITY();
                /*p.CapacityBack = this.Character.Inventory.MaxVolume[ContainerType.BACK_BAG];
                p.CapacityBody = this.Character.Inventory.MaxVolume[ContainerType.BODY];
                p.CapacityLeft = this.Character.Inventory.MaxVolume[ContainerType.LEFT_BAG];
                p.CapacityRight = this.Character.Inventory.MaxVolume[ContainerType.RIGHT_BAG];
                p.PayloadBack = this.Character.Inventory.MaxPayload[ContainerType.BACK_BAG];
                p.PayloadBody = this.Character.Inventory.MaxPayload[ContainerType.BODY];
                p.PayloadLeft = this.Character.Inventory.MaxPayload[ContainerType.LEFT_BAG];
                p.PayloadRight = this.Character.Inventory.MaxPayload[ContainerType.RIGHT_BAG];
                p.Payload = this.Character.Inventory.MaxPayload[ContainerType.BODY];
                p.Volume = this.Character.Inventory.MaxVolume[ContainerType.BODY];
                this.NetIo.SendPacket(p);
            }*/
        }

        public void SendChangeMap()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_CHANGE_MAP();
                if (map.returnori)
                    p.MapID = map.OriID;
                else
                    p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                if (map.IsDungeon)
                {
                    p.DungeonDir = map.DungeonMap.Dir;
                    p.DungeonX = map.DungeonMap.X;
                    p.DungeonY = map.DungeonMap.Y;
                    Character.Speed = Configuration.Configuration.Instance.Speed;
                }

                if (fgTakeOff)
                {
                    p.FGTakeOff = fgTakeOff;
                    fgTakeOff = false;
                }

                //this.Character.Speed = Configuration.Instance.Speed;
                NetIo.SendPacket(p);
            }
        }

        public void SendGotoFG()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (!fgMap.IsMapInstance)
                    Logger.ShowDebug(string.Format("MapID:{0} isn't a valid flying garden!"), Logger.defaultlogger);
                var owner = fgMap.Creator;
                var p = new SSMG_PLAYER_GOTO_FG();
                p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                p.Equiptments = owner.FlyingGarden.FlyingGardenEquipments;
                NetIo.SendPacket(p);
            }
        }

        public void SendGotoFF()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (fgMap.ID == 90001999) //铲除一个神经病逻辑
                {
                    CustomMapManager.Instance.SendGotoSerFFMap(this);
                    return;
                }

                if (!fgMap.IsMapInstance)
                    Logger.ShowDebug(string.Format("MapID:{0} isn't a valid flying garden!"), Logger.defaultlogger);
                var p = new SSMG_FF_ENTER();
                p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                p.RingID = Character.Ring.ID;
                p.RingHouseID = 30250000;
                NetIo.SendPacket(p);
            }
        }

        public void SendDungeonEvent()
        {
            if (Character.Online)
            {
                if (!map.IsMapInstance || !map.IsDungeon)
                    return;
                foreach (var i in map.DungeonMap.Gates.Keys)
                {
                    if (map.DungeonMap.Gates[i].NPCID != 0)
                    {
                        var p = new SSMG_NPC_SHOW();
                        p.NPCID = map.DungeonMap.Gates[i].NPCID;
                        NetIo.SendPacket(p);
                    }

                    if (map.DungeonMap.Gates[i].ConnectedMap != null)
                    {
                        if (i != GateType.Central && i != GateType.Exit)
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;

                            switch (i)
                            {
                                case GateType.North:
                                    p1.EventID = 12001501;
                                    break;
                                case GateType.East:
                                    p1.EventID = 12001502;
                                    break;
                                case GateType.South:
                                    p1.EventID = 12001503;
                                    break;
                                case GateType.West:
                                    p1.EventID = 12001504;
                                    break;
                            }

                            switch (map.DungeonMap.Gates[i].Direction)
                            {
                                case Direction.In:
                                    p1.EffectID = 9002;
                                    break;
                                case Direction.Out:
                                    p1.EffectID = 9005;
                                    break;
                            }

                            NetIo.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;
                            p1.EventID = 12001505;
                            p1.EffectID = 9005;
                            NetIo.SendPacket(p1);
                        }

                        if (map.DungeonMap.Gates[i].NPCID != 0)
                        {
                            var p = new SSMG_CHAT_MOTION();
                            p.ActorID = map.DungeonMap.Gates[i].NPCID;
                            p.Motion = (MotionType)621;
                            NetIo.SendPacket(p);
                        }
                    }
                    else
                    {
                        if (i == GateType.Entrance)
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;
                            p1.EventID = 12001505;
                            p1.EffectID = 9003;
                            NetIo.SendPacket(p1);
                        }
                    }
                }
            }
        }

        public void SendFGEvent()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (!fgMap.IsMapInstance)
                    return;
                if (map.ID / 10 == 7000000)
                    if (map.Creator.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE] != 0)
                    {
                        var p1 = new SSMG_NPC_SET_EVENT_AREA();
                        p1.EventID = 10000315;
                        p1.StartX = 6;
                        p1.StartY = 7;
                        p1.EndX = 6;
                        p1.EndY = 7;
                        NetIo.SendPacket(p1);
                    }
            }
        }

        public void SendGoldUpdate()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_GOLD_UPDATE();
                p.Gold = (ulong)Character.Gold;
                NetIo.SendPacket(p);
            }
        }

        public void SendActorHPMPSP(Actor actor)
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_MAX_HPMPSP();
                p.ActorID = actor.ActorID;
                p.MaxHP = actor.MaxHP;
                p.MaxMP = actor.MaxMP;
                p.MaxSP = actor.MaxSP;
                p.MaxEP = actor.MaxEP;
                NetIo.SendPacket(p);
                var p10 = new SSMG_PLAYER_HPMPSP();
                p10.ActorID = actor.ActorID;
                p10.HP = actor.HP;
                p10.MP = actor.MP;
                p10.SP = actor.SP;
                p10.EP = actor.EP;
                NetIo.SendPacket(p10);
                if (actor == Character)
                    if (Character.Party != null)
                        PartyManager.Instance.UpdateMemberHPMPSP(Character.Party, Character);
                //if ((DateTime.Now - hpmpspStamp).TotalSeconds >= 2)
                //{
                //    hpmpspStamp = DateTime.Now;
                //}
            }
        }

        public void SendCharXY()
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_MOVE();
                p.ActorID = Character.ActorID;
                p.Dir = Character.Dir;
                p.X = Character.X;
                p.Y = Character.Y;
                p.MoveType = MoveType.WARP;
                NetIo.SendPacket(p);
            }
        }

        public void SendPlayerLevel()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_LEVEL();
                p.Level = Character.Level;
                p.JobLevel = Character.JobLevel1;
                p.JobLevel2T = Character.JobLevel2T;
                p.JobLevel2X = Character.JobLevel2X;
                p.JobLevel3 = Character.JobLevel3;
                p.IsDualJob = Character.DualJobID != 0 ? (byte)1 : (byte)0;
                if (p.IsDualJob == 0x1)
                    p.DualjobLevel = Character.PlayerDualJobList[Character.DualJobID].DualJobLevel;

                p.UseableStatPoint = Character.StatsPoint;
                p.SkillPoint = Character.SkillPoint;
                p.Skill2XPoint = Character.SkillPoint2X;
                p.Skill2TPoint = Character.SkillPoint2T;
                p.Skill3Point = Character.SkillPoint3;
                NetIo.SendPacket(p);
            }
        }

        public void SendPlayerJob()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_JOB();
                p.Job = Character.Job;
                if (Character.JobJoint != PC_JOB.NONE)
                    p.JointJob = Character.JobJoint;
                if (Character.DualJobID != 0)
                    p.DualJob = Character.DualJobID;
                NetIo.SendPacket(p);
            }
        }


        public void SendCharInfoUpdate()
        {
            if (Character.Online)
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
        }

        public void SendAnnounce(string text)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_CHAT_PUBLIC();
                p11.ActorID = 0;
                p11.Message = text;
                NetIo.SendPacket(p11);
            }
        }

        public void SendPkMode()
        {
            if (Character.Online)
            {
                Character.Mode = PlayerMode.COLISEUM_MODE;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, Character, true);
            }
        }

        public void SendNormalMode()
        {
            if (Character.Online)
            {
                Character.Mode = PlayerMode.NORMAL;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, Character, true);
            }
        }

        public void SendPlayerSizeUpdate()
        {
            if (Character.Online)
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_SIZE_UPDATE, null, Character, true);
        }

        public void OnMove(CSMG_PLAYER_MOVE p)
        {
            if (Character.Online)
            {
                if (state != SESSION_STATE.LOADED)
                    return;
                switch (p.MoveType)
                {
                    case MoveType.RUN:
                        Map.MoveActor(Map.MOVE_TYPE.START, Character, new short[2] { p.X, p.Y }, p.Dir,
                            Character.Speed);
                        moveCheckStamp = DateTime.Now;
                        break;
                    case MoveType.CHANGE_DIR:
                        Map.MoveActor(Map.MOVE_TYPE.STOP, Character, new short[2] { p.X, p.Y }, p.Dir, Character.Speed);
                        break;
                    case MoveType.WALK:
                        Map.MoveActor(Map.MOVE_TYPE.START, Character, new short[2] { p.X, p.Y }, p.Dir, Character.Speed,
                            false, MoveType.WALK);
                        moveCheckStamp = DateTime.Now;
                        break;
                }

                if (Character.CInt["NextMoveEventID"] != 0)
                {
                    EventActivate((uint)Character.CInt["NextMoveEventID"]);
                    Character.CInt["NextMoveEventID"] = 0;
                    Character.CInt.Remove("NextMoveEventID");
                }

                if (Character.TTime["特殊刀攻击间隔"] != DateTime.Now)
                    Character.TTime["特殊刀攻击间隔"] = DateTime.Now;
            }
        }

        public void SendActorSpeed(Actor actor, ushort speed)
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_SPEED();
                p.ActorID = actor.ActorID;
                p.Speed = speed;
                NetIo.SendPacket(p);
            }
        }

        public void SendEXP()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_EXP();
                ulong cexp, jexp;
                ulong bexp = 0, nextExp = 0;
                if (!Character.Rebirth || Character.Job != Character.Job3)
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.Level + 1), LevelType.CLEVEL);
                }
                else
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL2);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.Level + 1), LevelType.CLEVEL2);
                }

                cexp = (uint)((float)(Character.CEXP - bexp) / (nextExp - bexp) * 1000);
                if (Character.JobJoint == PC_JOB.NONE)
                {
                    if (Character.DualJobID != 0)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(
                            Character.PlayerDualJobList[Character.DualJobID].DualJobLevel, LevelType.DUALJ);
                        nextExp = ExperienceManager.Instance.GetExpForLevel(
                            (uint)(Character.PlayerDualJobList[Character.DualJobID].DualJobLevel + 1), LevelType.DUALJ);
                    }
                    else if (Character.Job == Character.JobBasic)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel1, LevelType.JLEVEL);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel1 + 1),
                            LevelType.JLEVEL);
                    }
                    else if (Character.Job == Character.Job2X)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel2X + 1),
                            LevelType.JLEVEL2);
                    }
                    else if (Character.Job == Character.Job2T)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel2T + 1),
                            LevelType.JLEVEL2);
                    }
                    else
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel3, LevelType.JLEVEL3);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel3 + 1),
                            LevelType.JLEVEL3);
                    }

                    if (Character.DualJobID != 0)
                        jexp = (uint)((float)(Character.PlayerDualJobList[Character.DualJobID].DualJobExp - bexp) /
                            (nextExp - bexp) * 1000);
                    else
                        jexp = (uint)((float)(Character.JEXP - bexp) / (nextExp - bexp) * 1000);
                }
                else
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.JointJobLevel, LevelType.JLEVEL2);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JointJobLevel + 1),
                        LevelType.JLEVEL2);

                    jexp = (uint)((float)(Character.JointJEXP - bexp) / (nextExp - bexp) * 1000);
                }

                p.EXPPercentage = (uint)cexp >= 1000 ? 999 : (uint)cexp;
                p.JEXPPercentage = (uint)jexp >= 1000 ? 999 : (uint)jexp;
                p.WRP = Character.WRP;
                p.ECoin = Character.ECoin;
                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    p.Exp = (uint)Character.DominionCEXP;
                    p.JExp = (uint)Character.DominionJEXP;
                }
                else
                {
                    p.Exp = (long)Character.CEXP;
                    if (Character.DualJobID != 0)
                        p.JExp = (long)Character.PlayerDualJobList[Character.DualJobID].DualJobExp;
                    else
                        p.JExp = (long)Character.JEXP;
                }

                NetIo.SendPacket(p);
            }
        }

        public void SendEXPMessage(long exp, long jexp, long pexp, SSMG_PLAYER_EXP_MESSAGE.EXP_MESSAGE_TYPE type)
        {
            var p = new SSMG_PLAYER_EXP_MESSAGE();
            p.EXP = exp;
            p.JEXP = jexp;
            p.PEXP = pexp;
            p.Type = type;
            NetIo.SendPacket(p);
        }

        public void SendLvUP(Actor pc, byte type)
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_LEVEL_UP();
                p.ActorID = pc.ActorID;
                p.Level = pc.Level;
                p.LvType = type;

                NetIo.SendPacket(p);
            }
        }

        public void OnPlayerGreetings(CSMG_PLAYER_GREETINGS p)
        {
            if (Character.TTime["打招呼时间"] + new TimeSpan(0, 0, 10) > DateTime.Now)
            {
                SendSystemMessage("不可以频繁打招呼哦。");
                return;
            }

            var actor = map.GetActor(p.ActorID);
            if (actor.Buff.FishingState)
            {
                SendSystemMessage("对方正在钓鱼，不要打扰人家哦。");
                return;
            }

            if (actor != null)
                if (actor.type == ActorType.PC)
                {
                    var target = (ActorPC)actor;
                    if (target.Online && Character.Online)
                    {
                        var dir = map.CalcDir(Character.X, Character.Y, target.X, target.Y);
                        var dir2 = map.CalcDir(target.X, target.Y, Character.X, Character.Y);

                        var ys = new short[2];
                        ys[0] = Character.X;
                        ys[1] = Character.Y;
                        map.MoveActor(Map.MOVE_TYPE.START, Character, ys, dir, 500, true, MoveType.CHANGE_DIR);
                        if (Character.Partner != null)
                        {
                            ys = new short[2];
                            ys[0] = Character.Partner.X;
                            ys[1] = Character.Partner.Y;
                            map.MoveActor(Map.MOVE_TYPE.START, Character.Partner, ys, dir, 500, true,
                                MoveType.CHANGE_DIR);
                        }

                        ys = new short[2];
                        ys[0] = target.X;
                        ys[1] = target.Y;
                        map.MoveActor(Map.MOVE_TYPE.START, target, ys, dir2, 500, true, MoveType.CHANGE_DIR);
                        if (target.Partner != null)
                        {
                            ys = new short[2];
                            ys[0] = target.Partner.X;
                            ys[1] = target.Partner.Y;
                            map.MoveActor(Map.MOVE_TYPE.START, target.Partner, ys, dir, 500, true, MoveType.CHANGE_DIR);
                        }

                        ushort motionid = 163;
                        byte loop = 0;
                        switch (Global.Random.Next(0, 31))
                        {
                            case 0:
                                motionid = 113;
                                loop = 1;
                                break;
                            case 1:
                                motionid = 163;
                                break;
                            case 2:
                                motionid = 509;
                                break;
                            case 3:
                                motionid = 159;
                                break;
                            case 4:
                                motionid = 210;
                                break;
                            case 5:
                                motionid = 509;
                                break;
                            case 6:
                                motionid = 300;
                                break;
                            case 7:
                                motionid = 2035;
                                break;
                            case 8:
                                motionid = 2040;
                                break;
                            case 9:
                                motionid = 1520;
                                break;
                            case 10:
                                motionid = 1521;
                                break;
                            case 11:
                                motionid = 2020;
                                break;
                            case 12:
                                motionid = 2020;
                                break;
                            case 13:
                                motionid = 2064;
                                break;
                            case 14:
                                motionid = 2065;
                                break;
                            case 15:
                                motionid = 2066;
                                break;
                            case 16:
                                motionid = 2067;
                                break;
                            case 17:
                                motionid = 2069;
                                break;
                            case 18:
                                motionid = 2070;
                                break;
                            case 19:
                                motionid = 1524;
                                break;
                            case 20:
                                motionid = 2084;
                                break;
                            case 21:
                                motionid = 2095;
                                break;
                            case 22:
                                motionid = 2091;
                                break;
                            case 23:
                                motionid = 2085;
                                break;
                            case 24:
                                motionid = 2109;
                                break;
                            case 25:
                                motionid = 2125;
                                break;
                            case 26:
                                motionid = 2098;
                                break;
                            case 27:
                                motionid = 2079;
                                loop = 1;
                                break;
                            case 28:
                                motionid = 1523;
                                break;
                            case 29:
                                motionid = 2080;
                                break;
                            case 30:
                                motionid = 2138;
                                break;
                            case 31:
                                motionid = 2139;
                                break;
                        }

                        var tclient = FromActorPC(target);
                        SendMotion((MotionType)motionid, loop);
                        tclient.SendMotion((MotionType)motionid, loop);
                        SendSystemMessage("你问候了 " + target.Name);
                        tclient.SendSystemMessage(Character.Name + " 正在向你打招呼~");

                        if (target.AStr["打招呼每日重置2"] != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            target.AStr["打招呼每日重置2"] = DateTime.Now.ToString("yyyy-MM-dd");
                            if (target.CIDict["打招呼的玩家"].Count > 0)
                                target.CIDict["打招呼的玩家"] = new VariableHolderA<int, int>();
                            target.AInt["今日被打招呼次数"] = 0;
                        }

                        if (!target.CIDict["打招呼的玩家"].ContainsKey(Character.Account.AccountID))
                        {
                            target.CIDict["打招呼的玩家"][Character.Account.AccountID] = 0;
                            tclient.TitleProccess(target, 62, 1);
                            if (target.AInt["今日被打招呼次数"] < 1)
                            {
                                target.AInt["今日被打招呼次数"]++;
                                //int cp = Global.Random.Next(100, 280);
                                var ep = 10;
                                target.EP = Math.Min(target.EP + (uint)ep, target.MaxEP);
                                //tclient.SendSystemMessage("被亲切地问候了！获得" + cp + "CP。");
                            }
                        }

                        Character.TTime["打招呼时间"] = DateTime.Now;
                    }
                }
        }

        public void OnPlayerElements(CSMG_PLAYER_ELEMENTS p)
        {
            var p1 = new SSMG_PLAYER_ELEMENTS();
            var elements = new Dictionary<Elements, int>();
            foreach (var i in Character.AttackElements.Keys)
                elements.Add(i,
                    Character.AttackElements[i] + Character.Status.attackElements_item[i] +
                    Character.Status.attackelements_iris[i] + Character.Status.attackElements_skill[i]);
            p1.AttackElements = elements;
            elements.Clear();
            foreach (var i in Character.Elements.Keys)
                elements.Add(i,
                    Character.Elements[i] + Character.Status.elements_item[i] + Character.Status.elements_iris[i] +
                    Character.Status.elements_skill[i]);
            p1.DefenceElements = elements;
            NetIo.SendPacket(p1);
        }

        public void OnPlayerElements()
        {
            var p1 = new SSMG_PLAYER_ELEMENTS();
            var elements = new Dictionary<Elements, int>();
            foreach (var i in Character.AttackElements.Keys)
                elements.Add(i,
                    Character.AttackElements[i] + Character.Status.attackElements_item[i] +
                    Character.Status.attackelements_iris[i] + Character.Status.attackElements_skill[i]);
            p1.AttackElements = elements;
            elements.Clear();
            foreach (var i in Character.Elements.Keys)
                elements.Add(i,
                    Character.Elements[i] + Character.Status.elements_item[i] + Character.Status.elements_iris[i] +
                    Character.Status.elements_skill[i]);
            p1.DefenceElements = elements;
            NetIo.SendPacket(p1);
        }

        public void OnRequestPCInfo(CSMG_ACTOR_REQUEST_PC_INFO p)
        {
            var p1 = new SSMG_ACTOR_PC_INFO();
            var pc = map.GetActor(p.ActorID);

            if (pc == null) return;

            if (pc.type == ActorType.PC)
            {
                var a = (ActorPC)pc;
                a.WRPRanking = WRPRankingManager.Instance.GetRanking(a);
            }

            p1.Actor = pc;

            NetIo.SendPacket(p1);
            if (pc.type == ActorType.PC)
            {
                var actor = (ActorPC)pc;
                if (actor.Ring != null) Character.e.OnActorRingUpdate(actor);
            }
        }

        public void OnStatsPreCalc(CSMG_PLAYER_STATS_PRE_CALC p)
        {
            //backup
            ushort str, dex, intel, agi, vit, mag;
            var p1 = new SSMG_PLAYER_STATS_PRE_CALC();
            str = Character.Str;
            dex = Character.Dex;
            intel = Character.Int;
            agi = Character.Agi;
            vit = Character.Vit;
            mag = Character.Mag;

            Character.Str = p.Str;
            Character.Dex = p.Dex;
            Character.Int = p.Int;
            Character.Agi = p.Agi;
            Character.Vit = p.Vit;
            Character.Mag = p.Mag;

            StatusFactory.Instance.CalcStatus(Character);

            p1.ASPD = Character.Status.aspd;
            p1.ATK1Max = Character.Status.max_atk1;
            p1.ATK1Min = Character.Status.min_atk1;
            p1.ATK2Max = Character.Status.max_atk2;
            p1.ATK2Min = Character.Status.min_atk2;
            p1.ATK3Max = Character.Status.max_atk3;
            p1.ATK3Min = Character.Status.min_atk3;
            p1.AvoidCritical = Character.Status.avoid_critical;
            p1.AvoidMagic = Character.Status.avoid_magic;
            p1.AvoidMelee = Character.Status.avoid_melee;
            p1.AvoidRanged = Character.Status.avoid_ranged;
            p1.CSPD = Character.Status.cspd;
            p1.DefAddition = (ushort)Character.Status.def_add;
            p1.DefBase = Character.Status.def;
            p1.HitCritical = Character.Status.hit_critical;
            p1.HitMagic = Character.Status.hit_magic;
            p1.HitMelee = Character.Status.hit_melee;
            p1.HitRanged = Character.Status.hit_ranged;
            p1.MATKMax = Character.Status.max_matk;
            p1.MATKMin = Character.Status.min_matk;
            p1.MDefAddition = (ushort)Character.Status.mdef_add;
            p1.MDefBase = Character.Status.mdef;
            p1.Speed = Character.Speed;
            p1.HP = (ushort)Character.MaxHP;
            p1.MP = (ushort)Character.MaxMP;
            p1.SP = (ushort)Character.MaxSP;
            uint count = 0;
            foreach (var i in Character.Inventory.MaxVolume.Values) count += i;
            p1.Capacity = (ushort)count;
            count = 0;
            foreach (var i in Character.Inventory.MaxPayload.Values) count += i;
            p1.Payload = (ushort)count;

            //resotre
            Character.Str = str;
            Character.Dex = dex;
            Character.Int = intel;
            Character.Agi = agi;
            Character.Vit = vit;
            Character.Mag = mag;

            StatusFactory.Instance.CalcStatus(Character);

            NetIo.SendPacket(p1);
        }

        public void OnStatsUp(CSMG_PLAYER_STATS_UP p)
        {
            if (Configuration.Configuration.Instance.Version < Version.Saga13)
            {
                switch (p.Type)
                {
                    case 0:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Str))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                            Character.Str += 1;
                        }

                        break;
                    case 1:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Dex))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                            Character.Dex += 1;
                        }

                        break;
                    case 2:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Int))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                            Character.Int += 1;
                        }

                        break;
                    case 3:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Vit))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                            Character.Vit += 1;
                        }

                        break;
                    case 4:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Agi))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                            Character.Agi += 1;
                        }

                        break;
                    case 5:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Mag))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                            Character.Mag += 1;
                        }

                        break;
                }
            }
            else
            {
                if (p.Str > 0)
                    for (int i = p.Str; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Str))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                            Character.Str += 1;
                        }

                if (p.Dex > 0)
                    for (int i = p.Dex; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Dex))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                            Character.Dex += 1;
                        }

                if (p.Int > 0)
                    for (int i = p.Int; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Int))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                            Character.Int += 1;
                        }

                if (p.Vit > 0)
                    for (int i = p.Vit; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Vit))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                            Character.Vit += 1;
                        }

                if (p.Agi > 0)
                    for (int i = p.Agi; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Agi))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                            Character.Agi += 1;
                        }

                if (p.Mag > 0)
                    for (int i = p.Mag; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Mag))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                            Character.Mag += 1;
                        }
            }

            StatusFactory.Instance.CalcStatus(Character);
            SendActorHPMPSP(Character);
            SendStatus();
            SendStatusExtend();
            SendCapacity();
            //SendMaxCapacity();
            SendPlayerLevel();
        }

        public void SendWRPRanking(ActorPC pc)
        {
            var p = new SSMG_ACTOR_WRP_RANKING();
            p.ActorID = pc.ActorID;
            p.Ranking = pc.WRPRanking;
            NetIo.SendPacket(p);
        }

        public void RevivePC(ActorPC pc)
        {
            pc.HP = pc.MaxHP;
            pc.MP = pc.MaxMP;
            pc.SP = pc.MaxSP;
            pc.EP = pc.MaxEP;

            if (pc.Job == PC_JOB.CARDINAL)
                pc.EP = 5000;

            if (pc.Job == PC_JOB.ASTRALIST) //魔法师
                pc.EP = 0;

            if (!pc.Status.Additions.ContainsKey("HolyVolition"))
            {
                var skill = new DefaultBuff(null, pc, "HolyVolition", 2000);
                SkillHandler.ApplyAddition(pc, skill);
            }

            if (pc.SaveMap == 0)
            {
                pc.SaveMap = 91000999;
                pc.SaveX = 21;
                pc.SaveY = 21;
            }

            pc.BattleStatus = 0;
            SendChangeStatus();

            pc.Buff.Dead = false;
            pc.Buff.TurningPurple = false;
            pc.Motion = MotionType.STAND;
            pc.MotionLoop = false;
            SkillHandler.Instance.ShowVessel(pc, (int)-pc.MaxHP);
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, pc, true);

            SkillHandler.Instance.ShowEffectByActor(pc, 5116);
            SkillHandler.Instance.CastPassiveSkills(pc);
            SendPlayerInfo();

            if (!pc.Tasks.ContainsKey("Recover")) //自然恢复
            {
                var reg = new Recover(FromActorPC(pc));
                pc.Tasks.Add("Recover", reg);
                reg.Activate();
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);

            if (scriptThread != null)
                ClientManager.RemoveThread(scriptThread.Name);
            scriptThread = null;
            currentEvent = null;

            /*Scripting.Event evnt = null;
            if (evnt != null)
            {
                evnt.CurrentPC = null;
                this.scriptThread = null;
                this.currentEvent = null;
                ClientManager.RemoveThread(System.Threading.Thread.CurrentThread.Name);
                //ClientManager.LeaveCriticalArea();
            }*/
        }

        public void OnPlayerReturnHome(CSMG_PLAYER_RETURN_HOME p)
        {
            if (Character.HP == 0)
            {
                Character.HP = 1;
                Character.MP = 1;
                Character.SP = 1;
            }

            if (Character.SaveMap == 0)
            {
                Character.SaveMap = 10023100;
                Character.SaveX = 242;
                Character.SaveY = 128;
            }

            Character.BattleStatus = 0;
            SendChangeStatus();
            Character.Buff.Dead = false;
            Character.Buff.TurningPurple = false;
            Character.Motion = MotionType.STAND;
            Character.MotionLoop = false;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            if (map.ID == Character.SaveMap)
            {
                if (Map.Info.Healing)
                {
                    if (!Character.Tasks.ContainsKey("CityRecover"))
                    {
                        var task = new CityRecover(this);
                        Character.Tasks.Add("CityRecover", task);
                        task.Activate();
                    }
                }
                else
                {
                    if (Character.Tasks.ContainsKey("CityRecover"))
                    {
                        Character.Tasks["CityRecover"].Deactivate();
                        Character.Tasks.Remove("CityRecover");
                    }
                }

                if (Map.Info.Cold || map.Info.Hot || map.Info.Wet)
                {
                    if (!Character.Tasks.ContainsKey("CityDown"))
                    {
                        var task = new CityDown(this);
                        Character.Tasks.Add("CityDown", task);
                        task.Activate();
                    }
                }
                else
                {
                    if (Character.Tasks.ContainsKey("CityDown"))
                    {
                        Character.Tasks["CityDown"].Deactivate();
                        Character.Tasks.Remove("CityDown");
                    }
                }
            }

            if (Configuration.Configuration.Instance.HostedMaps.Contains(Character.SaveMap))
            {
                var info = MapInfoFactory.Instance.MapInfo[Character.SaveMap];
                Map.SendActorToMap(Character, Character.SaveMap, Global.PosX8to16(Character.SaveX, info.width),
                    Global.PosY8to16(Character.SaveY, info.height));
            }

            Event evnt = null;
            if (evnt != null)
            {
                evnt.CurrentPC = null;
                scriptThread = null;
                currentEvent = null;
                ClientManager.RemoveThread(Thread.CurrentThread.Name);
                ClientManager.LeaveCriticalArea();
            }
        }


        public void SendDefWarChange(DefWar text)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_DEFWAR_SET();
                p11.MapID = map.ID;
                p11.Data = text;
                NetIo.SendPacket(p11);
            }
        }

        public void SendDefWarResult(byte r1, byte r2, int exp, int jobexp, int cp, byte u = 0)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_DEFWAR_RESULT();
                p11.Result1 = r1;
                p11.Result2 = r2;
                p11.EXP = exp;
                p11.JOBEXP = jobexp;
                p11.CP = cp;

                p11.Unknown = u;
                NetIo.SendPacket(p11);
            }
        }

        public void SendDefWarState(byte rate)
        {
            if (Character.Online)
            {
                var p1 = new SSMG_DEFWAR_STATE();
                p1.MapID = map.ID;
                p1.Rate = rate;
                NetIo.SendPacket(p1);
            }
        }

        public void SendDefWarStates(Dictionary<uint, byte> list)
        {
            if (Character.Online)
            {
                var p1 = new SSMG_DEFWAR_STATES();
                p1.List = list;
                NetIo.SendPacket(p1);
            }
        }

        public bool chipShop;
        private uint currentChipCategory;
        public bool demCLBuy;

        public bool demic;
        public bool demParts;

        public void SendCL()
        {
            if (Character.Race == PC_RACE.DEM && state != SESSION_STATE.AUTHENTIFICATED)
            {
                var p1 = new SSMG_DEM_COST_LIMIT_UPDATE();
                p1.Result = 0;
                p1.CurrentEP = Character.EPUsed;
                p1.EPRequired = (short)(ExperienceManager.Instance.GetEPRequired(Character) - Character.EPUsed);
                p1.CL = Character.CL;
                NetIo.SendPacket(p1);
            }
        }

        public void OnDEMCostLimitBuy(CSMG_DEM_COST_LIMIT_BUY p)
        {
            if (demCLBuy)
            {
                var ep = p.EP;
                var p1 = new SSMG_DEM_COST_LIMIT_UPDATE();
                if (Character.EP >= ep)
                {
                    Character.EP = (uint)(Character.EP - ep);
                    ExperienceManager.Instance.ApplyEP(Character, ep);
                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();
                    p1.Result = SSMG_DEM_COST_LIMIT_UPDATE.Results.OK;
                }
                else
                {
                    p1.Result = SSMG_DEM_COST_LIMIT_UPDATE.Results.NOT_ENOUGH_EP;
                }

                p1.CurrentEP = Character.EPUsed;
                p1.EPRequired = (short)(ExperienceManager.Instance.GetEPRequired(Character) - Character.EPUsed);
                p1.CL = Character.CL;
                NetIo.SendPacket(p1);
            }
        }

        public void OnDEMCostLimitClose(CSMG_DEM_COST_LIMIT_CLOSE p)
        {
            demCLBuy = false;
        }

        public void OnDEMFormChange(CSMG_DEM_FORM_CHANGE p)
        {
            if (Character.Form != p.Form)
            {
                Character.Form = p.Form;

                SkillHandler.Instance.CastPassiveSkills(Character);
                StatusFactory.Instance.CalcStatus(Character);

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
                SendPlayerInfo();
                SendAttackType();

                var p1 = new SSMG_DEM_FORM_CHANGE();
                p1.Form = Character.Form;
                NetIo.SendPacket(p1);
            }
        }

        public void OnDEMPartsUnequip(CSMG_DEM_PARTS_UNEQUIP p)
        {
            if (Character.Race == PC_RACE.DEM && demParts)
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null) return;
                var ifUnequip = Character.Inventory.IsContainerParts(Character.Inventory.GetContainerType(item.Slot));
                if (ifUnequip)
                {
                    var slots = item.EquipSlot;
                    if (slots.Count > 1)
                        for (var i = 1; i < slots.Count; i++)
                            Character.Inventory.Parts.Remove(slots[i]);

                    SSMG_ITEM_DELETE p2;
                    SSMG_ITEM_ADD p3;
                    var slot = item.Slot;

                    if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot,
                            ContainerType.BODY, 1))
                    {
                        if (item.Stack == 0)
                        {
                            if (slot == Character.Inventory.LastItem.Slot)
                            {
                                var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                                p1.InventorySlot = item.Slot;
                                p1.Target = ContainerType.BODY;
                                NetIo.SendPacket(p1);
                                var p4 = new SSMG_ITEM_EQUIP();
                                p4.InventorySlot = 0xffffffff;
                                p4.Target = ContainerType.NONE;
                                p4.Result = 3;
                                StatusFactory.Instance.CalcRange(Character);
                                if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                                {
                                    SendAttackType();
                                    SkillHandler.Instance.CastPassiveSkills(Character);
                                }

                                p4.Range = Character.Range;
                                NetIo.SendPacket(p4);
                                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character,
                                    true);

                                StatusFactory.Instance.CalcStatus(Character);
                                SendPlayerInfo();
                            }
                            else
                            {
                                p2 = new SSMG_ITEM_DELETE();
                                p2.InventorySlot = slot;
                                NetIo.SendPacket(p2);
                                if (slot != item.Slot)
                                {
                                    item = Character.Inventory.GetItem(item.Slot);
                                    var p5 = new SSMG_ITEM_COUNT_UPDATE();
                                    p5.InventorySlot = item.Slot;
                                    p5.Stack = item.Stack;
                                    NetIo.SendPacket(p5);
                                    item = Character.Inventory.LastItem;
                                    p3 = new SSMG_ITEM_ADD();
                                    p3.Container = ContainerType.BODY;
                                    p3.InventorySlot = item.Slot;
                                    p3.Item = item;
                                    NetIo.SendPacket(p3);
                                }
                                else
                                {
                                    item = Character.Inventory.LastItem;
                                    var p4 = new SSMG_ITEM_COUNT_UPDATE();
                                    p4.InventorySlot = item.Slot;
                                    p4.Stack = item.Stack;
                                    NetIo.SendPacket(p4);
                                }
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_ITEM_COUNT_UPDATE();
                            p1.InventorySlot = item.Slot;
                            p1.Stack = item.Stack;
                            NetIo.SendPacket(p1);
                            if (Character.Inventory.LastItem.Stack == 1)
                            {
                                p3 = new SSMG_ITEM_ADD();
                                p3.Container = ContainerType.BODY;
                                p3.InventorySlot = Character.Inventory.LastItem.Slot;
                                p3.Item = Character.Inventory.LastItem;
                                NetIo.SendPacket(p3);
                            }
                            else
                            {
                                item = Character.Inventory.LastItem;
                                var p4 = new SSMG_ITEM_COUNT_UPDATE();
                                p4.InventorySlot = item.Slot;
                                p4.Stack = item.Stack;
                                NetIo.SendPacket(p4);
                            }
                        }
                    }

                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnDEMPartsEquip(CSMG_DEM_PARTS_EQUIP p)
        {
            if (Character.Race == PC_RACE.DEM && demParts)
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null) return;
                var result = CheckEquipRequirement(item);
                if (result < 0)
                {
                    var p4 = new SSMG_ITEM_EQUIP();
                    p4.InventorySlot = 0xffffffff;
                    p4.Target = ContainerType.NONE;
                    p4.Result = result;
                    p4.Range = Character.Range;
                    NetIo.SendPacket(p4);
                    return;
                }

                foreach (var i in item.EquipSlot)
                    if (Character.Inventory.Parts.ContainsKey(i))
                    {
                        var oriItem = Character.Inventory.Parts[i];

                        foreach (var j in oriItem.EquipSlot)
                        {
                            if (!Character.Inventory.Parts.ContainsKey(j))
                                continue;
                            var dummyItem = Character.Inventory.Parts[j];
                            if (dummyItem.Stack == 0)
                            {
                                Character.Inventory.Parts.Remove(j);
                                continue;
                            }

                            var container = (ContainerType)Enum.Parse(typeof(ContainerType), j.ToString()) + 200;
                            if (Character.Inventory.MoveItem(container, (int)dummyItem.Slot, ContainerType.BODY,
                                    dummyItem.Stack))
                            {
                                var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                                p1.InventorySlot = dummyItem.Slot;
                                p1.Target = ContainerType.BODY;
                                NetIo.SendPacket(p1);
                                var p4 = new SSMG_ITEM_EQUIP();
                                p4.InventorySlot = 0xffffffff;
                                p4.Target = ContainerType.NONE;
                                p4.Result = 1;
                                p4.Range = Character.Range;
                                NetIo.SendPacket(p4);

                                StatusFactory.Instance.CalcStatus(Character);
                                SendPlayerInfo();
                            }
                        }
                    }

                var count = item.Stack;
                if (count == 0) return;

                var dst = (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString()) + 200;
                if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot, dst,
                        count))
                {
                    if (item.Stack == 0)
                    {
                        var p4 = new SSMG_ITEM_EQUIP();
                        dst = (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString());
                        p4.Target = dst;
                        p4.Result = 2;
                        p4.InventorySlot = item.Slot;
                        StatusFactory.Instance.CalcRange(Character);
                        p4.Range = Character.Range;
                        NetIo.SendPacket(p4);
                    }
                    else
                    {
                        var p5 = new SSMG_ITEM_COUNT_UPDATE();
                        p5.InventorySlot = item.Slot;
                        p5.Stack = item.Stack;
                        NetIo.SendPacket(p5);
                    }
                }

                var slots = item.EquipSlot;
                if (slots.Count > 1)
                    for (var i = 1; i < slots.Count; i++)
                    {
                        var dummy = item.Clone();
                        dummy.Stack = 0;
                        dst = (ContainerType)Enum.Parse(typeof(ContainerType), slots[i].ToString()) + 200;
                        Character.Inventory.AddItem(dst, dummy);
                    }

                if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                {
                    SendAttackType();
                    SkillHandler.Instance.CastPassiveSkills(Character);
                }

                //SkillHandler.Instance.CheckBuffValid(this.Character);

                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);
            }
        }

        public void OnDEMPartsClose(CSMG_DEM_PARTS_CLOSE p)
        {
            demParts = false;
        }

        public void OnDEMDemicInitialize(CSMG_DEM_DEMIC_INITIALIZE p)
        {
            if (demic)
            {
                var page = p.Page;
                DEMICPanel panel = null;
                bool[,] table = null;
                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    if (Character.Inventory.DominionDemicChips.ContainsKey(page))
                    {
                        panel = Character.Inventory.DominionDemicChips[page];
                        table = Character.Inventory.validTable(page, true);
                    }
                }
                else
                {
                    if (Character.Inventory.DemicChips.ContainsKey(page))
                    {
                        panel = Character.Inventory.DemicChips[page];
                        table = Character.Inventory.validTable(page, false);
                    }
                }

                var p1 = new SSMG_DEM_DEMIC_INITIALIZED();
                p1.Page = page;

                if (panel != null)
                {
                    if (Character.EP > 0)
                    {
                        Character.EP--;
                        foreach (var i in panel.Chips)
                        {
                            if (i.Data.skill1 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill1))
                                {
                                    if (Character.Skills[i.Data.skill1].Level > 1)
                                        Character.Skills[i.Data.skill1].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill1);
                                }

                            if (i.Data.skill2 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill2))
                                {
                                    if (Character.Skills[i.Data.skill2].Level > 1)
                                        Character.Skills[i.Data.skill2].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill2);
                                }

                            if (i.Data.skill3 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill3))
                                {
                                    if (Character.Skills[i.Data.skill3].Level > 1)
                                        Character.Skills[i.Data.skill3].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill3);
                                }

                            AddItem(ItemFactory.Instance.GetItem(i.ItemID), true);
                        }

                        panel.Chips.Clear();
                        int engageTask;
                        var term = Global.Random.Next(0, 99);
                        if (term <= 10)
                            engageTask = 2;
                        else if (term <= 40)
                            engageTask = 1;
                        else
                            engageTask = 0;
                        panel.EngageTask1 = 255;
                        panel.EngageTask2 = 255;
                        for (var i = 0; i < engageTask; i++)
                        {
                            var valid = new List<byte[]>();
                            for (var j = 0; j < 9; j++)
                                for (var k = 0; k < 9; k++)
                                    if (table[k, j])
                                        valid.Add(new[] { (byte)k, (byte)j });

                            var coord = valid[Global.Random.Next(0, valid.Count - 1)];
                            var task = (byte)(coord[0] + coord[1] * 9);
                            if (i == 0)
                                panel.EngageTask1 = task;
                            else
                                panel.EngageTask2 = task;
                        }

                        SendActorHPMPSP(Character);

                        p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.OK;
                        p1.EngageTask = panel.EngageTask1;
                        p1.EngageTask2 = panel.EngageTask2;

                        StatusFactory.Instance.CalcStatus(Character);
                        SendPlayerInfo();
                    }
                    else
                    {
                        p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.NOT_ENOUGH_EP;
                    }
                }
                else
                {
                    p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.FAILED;
                }

                NetIo.SendPacket(p1);
            }
        }

        public void OnDEMDemicConfirm(CSMG_DEM_DEMIC_CONFIRM p)
        {
            if (demic)
            {
                var chips = p.Chips;
                var page = p.Page;
                for (var i = 0; i < 9; i++)
                    for (var j = 0; j < 9; j++)
                    {
                        var chipID = chips[j, i];
                        if (ChipFactory.Instance.ByChipID.ContainsKey(chipID))
                        {
                            var chip = new Chip(ChipFactory.Instance.ByChipID[chipID]);
                            if (CountItem(chip.ItemID) > 0)
                            {
                                chip.X = (byte)j;
                                chip.Y = (byte)i;
                                if (Character.Inventory.InsertChip(page, chip)) DeleteItemID(chip.ItemID, 1, true);
                            }
                        }
                    }

                var p1 = new SSMG_DEM_DEMIC_CONFIRM_RESULT();
                p1.Page = page;
                p1.Result = SSMG_DEM_DEMIC_CONFIRM_RESULT.Results.OK;
                NetIo.SendPacket(p1);

                StatusFactory.Instance.CalcStatus(Character);
                SkillHandler.Instance.CastPassiveSkills(Character);
                SendPlayerInfo();
            }
        }

        public void OnDEMDemicClose(CSMG_DEM_DEMIC_CLOSE p)
        {
            demic = false;
        }

        public void OnDEMStatsPreCalc(CSMG_DEM_STATS_PRE_CALC p)
        {
            //backup
            ushort str, dex, intel, agi, vit, mag;
            str = Character.Str;
            dex = Character.Dex;
            intel = Character.Int;
            agi = Character.Agi;
            vit = Character.Vit;
            mag = Character.Mag;

            Character.Str = p.Str;
            Character.Dex = p.Dex;
            Character.Int = p.Int;
            Character.Agi = p.Agi;
            Character.Vit = p.Vit;
            Character.Mag = p.Mag;

            StatusFactory.Instance.CalcStatus(Character);

            {
                var p1 = new SSMG_PLAYER_STATS_PRE_CALC();
                p1.ASPD = Character.Status.aspd;
                p1.ATK1Max = Character.Status.max_atk1;
                p1.ATK1Min = Character.Status.min_atk1;
                p1.ATK2Max = Character.Status.max_atk2;
                p1.ATK2Min = Character.Status.min_atk2;
                p1.ATK3Max = Character.Status.max_atk3;
                p1.ATK3Min = Character.Status.min_atk3;
                p1.AvoidCritical = Character.Status.avoid_critical;
                p1.AvoidMagic = Character.Status.avoid_magic;
                p1.AvoidMelee = Character.Status.avoid_melee;
                p1.AvoidRanged = Character.Status.avoid_ranged;
                p1.CSPD = Character.Status.cspd;
                p1.DefAddition = (ushort)Character.Status.def_add;
                p1.DefBase = Character.Status.def;
                p1.HitCritical = Character.Status.hit_critical;
                p1.HitMagic = Character.Status.hit_magic;
                p1.HitMelee = Character.Status.hit_melee;
                p1.HitRanged = Character.Status.hit_ranged;
                p1.MATKMax = Character.Status.max_matk;
                p1.MATKMin = Character.Status.min_matk;
                p1.MDefAddition = (ushort)Character.Status.mdef_add;
                p1.MDefBase = Character.Status.mdef;
                p1.Speed = Character.Speed;
                p1.HP = (ushort)Character.MaxHP;
                p1.MP = (ushort)Character.MaxMP;
                p1.SP = (ushort)Character.MaxSP;
                uint count = 0;
                foreach (var i in Character.Inventory.MaxVolume.Values) count += i;
                p1.Capacity = (ushort)count;
                count = 0;
                foreach (var i in Character.Inventory.MaxPayload.Values) count += i;
                p1.Payload = (ushort)count;

                NetIo.SendPacket(p1);
            }
            {
                var p1 = new SSMG_DEM_STATS_PRE_CALC();
                p1.ASPD = Character.Status.aspd;
                p1.ATK1Max = Character.Status.max_atk1;
                p1.ATK1Min = Character.Status.min_atk1;
                p1.ATK2Max = Character.Status.max_atk2;
                p1.ATK2Min = Character.Status.min_atk2;
                p1.ATK3Max = Character.Status.max_atk3;
                p1.ATK3Min = Character.Status.min_atk3;
                p1.AvoidCritical = Character.Status.avoid_critical;
                p1.AvoidMagic = Character.Status.avoid_magic;
                p1.AvoidMelee = Character.Status.avoid_melee;
                p1.AvoidRanged = Character.Status.avoid_ranged;
                p1.CSPD = Character.Status.cspd;
                p1.DefAddition = (ushort)Character.Status.def_add;
                p1.DefBase = Character.Status.def;
                p1.HitCritical = Character.Status.hit_critical;
                p1.HitMagic = Character.Status.hit_magic;
                p1.HitMelee = Character.Status.hit_melee;
                p1.HitRanged = Character.Status.hit_ranged;
                p1.MATKMax = Character.Status.max_matk;
                p1.MATKMin = Character.Status.min_matk;
                p1.MDefAddition = (ushort)Character.Status.mdef_add;
                p1.MDefBase = Character.Status.mdef;
                p1.Speed = Character.Speed;
                p1.HP = (ushort)Character.MaxHP;
                p1.MP = (ushort)Character.MaxMP;
                p1.SP = (ushort)Character.MaxSP;
                uint count = 0;
                foreach (var i in Character.Inventory.MaxVolume.Values) count += i;
                p1.Capacity = (ushort)count;
                count = 0;
                foreach (var i in Character.Inventory.MaxPayload.Values) count += i;
                p1.Payload = (ushort)count;

                NetIo.SendPacket(p1);
            }

            //resotre
            Character.Str = str;
            Character.Dex = dex;
            Character.Int = intel;
            Character.Agi = agi;
            Character.Vit = vit;
            Character.Mag = mag;

            StatusFactory.Instance.CalcStatus(Character);
        }

        public void OnDEMChipCategory(CSMG_DEM_CHIP_CATEGORY p)
        {
            if (chipShop)
                if (ChipShopFactory.Instance.Items.ContainsKey(p.Category))
                {
                    currentChipCategory = p.Category;
                    var category = ChipShopFactory.Instance.Items[p.Category];
                    var p1 = new SSMG_DEM_CHIP_SHOP_HEADER();
                    p1.CategoryID = p.Category;
                    NetIo.SendPacket(p1);

                    foreach (var i in category.Items.Values)
                    {
                        var p2 = new SSMG_DEM_CHIP_SHOP_DATA();
                        p2.EXP = (uint)i.EXP;
                        p2.JEXP = (uint)i.JEXP;
                        p2.ItemID = i.ItemID;
                        p2.Description = i.Description;
                        NetIo.SendPacket(p2);
                    }

                    var p3 = new SSMG_DEM_CHIP_SHOP_FOOTER();
                    NetIo.SendPacket(p3);
                }
        }

        public void OnDEMChipClose(CSMG_DEM_CHIP_CLOSE p)
        {
            chipShop = false;
        }

        public void OnDEMChipBuy(CSMG_DEM_CHIP_BUY p)
        {
            if (chipShop)
            {
                var items = p.ItemIDs;
                var counts = p.Counts;

                for (var i = 0; i < items.Length; i++)
                {
                    var cat = from item in ChipShopFactory.Instance.Items.Values
                              where item.Items.ContainsKey(items[i])
                              select item;

                    if (cat.Count() > 0)
                    {
                        var category = cat.First();
                        if (counts[i] > 0)
                        {
                            var chip = category.Items[items[i]];
                            if (Character.CEXP > chip.EXP * (ulong)counts[i] &&
                                Character.JEXP > chip.JEXP * (ulong)counts[i])
                            {
                                Character.CEXP -= (uint)(chip.EXP * (ulong)counts[i]);
                                Character.JEXP -= (uint)(chip.JEXP * (ulong)counts[i]);
                                var item = ItemFactory.Instance.GetItem(items[i]);
                                item.Stack = (ushort)counts[i];
                                AddItem(item, true);
                            }
                        }
                    }

                    SendEXP();
                }
            }
        }

        public void OnPartnerMotion(CSMG_PARTNER_PARTNER_MOTION p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET)) return;
            var itemid = Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.id;
            var motions = PartnerFactory.Instance.GetPartnerMotion(itemid);
            if (motions == null) return;
            var id = (int)p.id;

            var partner = Character.Partner;

            var motion = motions[id];
            SendMotion((MotionType)motion.MasterMotionID, 1);

            var arg = new ChatArg();
            arg.motion = (MotionType)motion.PartnerMotionID;
            arg.loop = 1;
            partner.Motion = arg.motion;
            partner.MotionLoop = true;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, arg, partner, true);
        }

        //#region Partner System

        public enum TALK_EVENT
        {
            SUMMONED = 10000,
            BATTLE = 10002,
            MASTERDEAD = 10003,
            NORMAL = 10004,
            MASTERDEBUFF = 10006,
            JOINPARTY = 10007,
            LEAVEPARTY = 10008,
            MASTEROVERWEIGHT = 10009,

            //??????????? = 10013,
            //MASTER PYING = 10014,
            //MASTER PYED = 10015,
            //MASTER ORG = 10016,
            //MASTER DOGEZA = 10017,
            MASTERFIGHTING = 10018,
            MASTERLVUP = 10019,
            MASTERQUIT = 10020,
            MASTERSIT = 10026,
            MASTERRALEX = 10027,
            MASTERBOW = 10028,

            //MASTER TURN = 10029.
            //PARTNER TUSTEE UP(BLUE) = 80100, 
            MASTERLOGIN = 80101,
            LVUP = 80102,
            EQUIP = 80103,

            //PARTNER TUSTEE CHANGE(CHANGE COLOR) = 80104,
            EAT = 80105,

            EATREADY = 80106
            //MASTER GOT DAMAGE FOR 5 MINUTE = 80107,
            //ATTACK COMMAND = 80109,
            //SUPPORT COMMAND = 80110,
            //FOLLOW COMMAND = 80111,
            //?????? COMMAND = 80112
        }

        public void SendPartner(Item item)
        {
            try
            {
                if (item.ChangeMode || item.ChangeMode2)
                    return;
                if (item.BaseData.itemType == ItemType.PARTNER)
                {
                    if (item.ActorPartnerID == 0) item.ActorPartnerID = MapServer.charDB.CreatePartner(item);
                    var partner = MapServer.charDB.GetActorPartner(item.ActorPartnerID, item);
                    if (partner.nextfeedtime > DateTime.Now)
                    {
                        if (!partner.Tasks.ContainsKey("Feed")) //This is a double check here
                        {
                            var seconds = (uint)(partner.nextfeedtime - DateTime.Now).TotalSeconds;
                            var task = new Feed(this, partner, seconds);
                            partner.Tasks.Add("Feed", task);
                            task.Activate();
                        }
                    }
                    else
                    {
                        partner.reliabilityuprate = 100;
                    }

                    partner.Range = 500;
                    //partner.BaseData.range = 1;
                    Character.Partner = partner;
                    partner.MapID = Character.MapID;
                    partner.X = Character.X;
                    partner.Y = Character.Y;
                    partner.Owner = Character;


                    var eh = new PartnerEventHandler(partner);
                    eh.AI.Master = Character;
                    if (PartnerAIFactory.Instance.Items.ContainsKey(item.BaseData.petID))
                        eh.AI.Mode = PartnerAIFactory.Instance.Items[item.BaseData.petID];
                    else
                        eh.AI.Mode = new Partner.AIMode(1);
                    SetPartnerSkills(partner);
                    eh.AI.Start();
                    partner.e = eh;
                    //if (!eh.AI.Hate.ContainsKey(Character.ActorID))
                    //    eh.AI.Hate.Add(Character.ActorID, 10);

                    //Mob.AIThread.Instance.RegisterAI(eh.AI);
                    if (!partner.Tasks.ContainsKey("ReliabilityGrow"))
                    {
                        var task = new ReliabilityGrow(partner);
                        partner.Tasks.Add("ReliabilityGrow", task);
                        task.Activate();
                    }

                    //信赖度关闭
                    map.RegisterActor(partner);
                    partner.invisble = false;
                    map.OnActorVisibilityChange(partner);
                    map.SendVisibleActorsToActor(partner);
                    partner.HP = partner.MaxHP;
                    if (Character.Rebirth == false)
                    {
                        ushort level = Character.Level;
                        partner.Level = (byte)level;
                        partner.MaxHP = (uint)(350 + 11650 * (float)(level / 110));
                        partner.MaxSP = (uint)(50 + 1450 * (float)(level / 110));
                        partner.MaxMP = (uint)(50 + 1450 * (float)(level / 110));
                        partner.HP = partner.MaxHP;
                        partner.SP = partner.MaxSP;
                        partner.MP = partner.MaxMP;
                        partner.Status.min_atk1 = (ushort)(20 + 650 * (float)(level / 110));
                        partner.Status.min_atk2 = (ushort)(20 + 650 * (float)(level / 110));
                        partner.Status.min_atk3 = (ushort)(20 + 650 * (float)(level / 110));
                        partner.Status.max_atk1 = (ushort)(40 + 850 * (float)(level / 110));
                        partner.Status.max_atk2 = (ushort)(40 + 850 * (float)(level / 110));
                        partner.Status.max_atk3 = (ushort)(40 + 850 * (float)(level / 110));
                        partner.Status.min_matk = (ushort)(40 + 800 * (float)(level / 110));
                        partner.Status.max_matk = (ushort)(60 + 1300 * (float)(level / 110));
                        partner.Status.def = (ushort)(10 + 10 * (float)(level / 110));
                        ;
                        partner.Status.def_add = (short)(20 + 300 * (float)(level / 110));
                        partner.Status.mdef = (ushort)(10 + 15 * (float)(level / 110));
                        partner.Status.mdef_add = (short)(30 + 320 * (float)(level / 110));
                        partner.Status.aspd = (short)(775 * (float)(level / 110));
                        partner.Status.cspd = (short)(655 * (float)(level / 110));
                        partner.Status.hit_melee = (ushort)(300 * (float)(level / 110));
                        partner.Status.avoid_melee = (ushort)(100 * (float)(level / 110));
                        partner.Status.hit_ranged = (ushort)(300 * (float)(level / 110));
                        partner.Status.avoid_melee = (ushort)(100 * (float)(level / 110));
                        partner.Status.hit_critical = 30;
                        partner.Status.avoid_critical = 30;
                        partner.Speed = 350;
                    }
                    else
                    {
                        ushort level = Character.Level;
                        partner.Level = (byte)level;
                        partner.MaxHP = (uint)(700 + 26300 * (float)(level / 110));
                        partner.MaxSP = (uint)(100 + 2400 * (float)(level / 110));
                        partner.MaxMP = (uint)(100 + 2400 * (float)(level / 110));
                        partner.HP = partner.MaxHP;
                        partner.SP = partner.MaxSP;
                        partner.MP = partner.MaxMP;
                        partner.Status.min_atk1 = (ushort)(40 + 1200 * (float)(level / 110));
                        partner.Status.min_atk2 = (ushort)(40 + 1200 * (float)(level / 110));
                        partner.Status.min_atk3 = (ushort)(40 + 1200 * (float)(level / 110));
                        partner.Status.max_atk1 = (ushort)(60 + 2100 * (float)(level / 110));
                        partner.Status.max_atk2 = (ushort)(60 + 2100 * (float)(level / 110));
                        partner.Status.max_atk3 = (ushort)(60 + 2100 * (float)(level / 110));
                        partner.Status.min_matk = (ushort)(60 + 1500 * (float)(level / 110));
                        partner.Status.max_matk = (ushort)(100 + 2500 * (float)(level / 110));
                        partner.Status.def = (ushort)(15 + 15 * (float)(level / 110));
                        ;
                        partner.Status.def_add = (short)(50 + 600 * (float)(level / 110));
                        partner.Status.mdef = (ushort)(15 + 20 * (float)(level / 110));
                        partner.Status.mdef_add = (short)(50 + 700 * (float)(level / 110));
                        partner.Status.aspd = (short)(775 * (float)(level / 110));
                        partner.Status.cspd = (short)(655 * (float)(level / 110));
                        partner.Status.hit_melee = (ushort)(450 * (float)(level / 110));
                        partner.Status.avoid_melee = (ushort)(180 * (float)(level / 110));
                        partner.Status.hit_ranged = (ushort)(450 * (float)(level / 110));
                        partner.Status.avoid_melee = (ushort)(180 * (float)(level / 110));
                        partner.Status.hit_critical = 30;
                        partner.Status.avoid_critical = 30;
                        partner.Speed = 350;
                    }

                    var taft = new TalkAtFreeTime(this);
                    partner.Tasks.Add("TalkAtFreeTime", taft);
                    taft.Activate();
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void SetPartnerSkills(ActorPartner partner)
        {
            //比较呆，需要完善，只做了主动技能！
            var eh = (PartnerEventHandler)partner.e;
            if (partner.equipcubes_activeskill.Count > 0)
            {
                eh.AI.Mode.EventAttacking.Clear();
                for (var i = 0; i < partner.equipcubes_activeskill.Count; i++)
                {
                    var cubeid = partner.equipcubes_activeskill[i];
                    if (PartnerFactory.Instance.actcubes_db_uniqueID.ContainsKey(cubeid))
                    {
                        var acd = PartnerFactory.Instance.actcubes_db_uniqueID[cubeid];

                        eh.AI.Mode.EventAttacking.Add(acd.skillID, 20);
                        eh.AI.Mode.EventAttackingSkillRate = 10;
                    }
                }
            }
        }

        public void DeletePartner()
        {
            if (Character.Partner == null)
                return;
            /*if (this.Character.Pet.Ride)
            {
                return;
            }*/
            var partner = Character.Partner;
            var eh = (PartnerEventHandler)Character.Partner.e;
            eh.AI.Pause();
            eh.AI.Activated = false;
            if (partner.Tasks.ContainsKey("Feed"))
            {
                partner.Tasks["Feed"].Deactivate();
                partner.Tasks.Remove("Feed");
            }

            if (partner.Tasks.ContainsKey("ReliabilityGrow"))
            {
                partner.Tasks["ReliabilityGrow"].Deactivate();
                partner.Tasks.Remove("ReliabilityGrow");
            }

            if (partner.Tasks.ContainsKey("TalkAtFreeTime"))
            {
                partner.Tasks["TalkAtFreeTime"].Deactivate();
                partner.Tasks.Remove("TalkAtFreeTime");
            }

            MapServer.charDB.SavePartner(Character.Partner);
            MapManager.Instance.GetMap(Character.Partner.MapID).DeleteActor(Character.Partner);


            Character.Partner = null;
        }

        private void Speak(Actor actor, string message)
        {
            if (message == "") return;
            var arg = new ChatArg();
            arg.content = message;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAT, arg, actor, false);
        }

        public void PartnerTalking(ActorPartner partner, TALK_EVENT type, int rate)
        {
            PartnerTalking(partner, type, rate, 20000);
        }

        private string getmessage(List<string> m)
        {
            var s = "";
            if (m.Count == 0) return s;
            if (m.Count == 1)
            {
                s = m[0];
                s = s.Replace("name", Character.Name);
                return s;
            }

            ;
            s = m[Global.Random.Next(0, m.Count - 1)];
            s = s.Replace("name", Character.Name);
            return s;
        }


        internal void OnPartnerTalk(CSMG_PARTNER_TALK p)
        {
            var partner = Character.Partner;
            if (partner == null)
                return;

            Speak(partner, p.Msg);
        }

        public void PartnerTalking(ActorPartner partner, TALK_EVENT type, int rate, int delay)
        {
            if (partner == null) return;
            if (Global.Random.Next(0, 100) > rate) return;
            if (partner.Status.Additions.ContainsKey("PartnerShutUp") && delay != 0) return;
            if (delay > 0)
                if (!partner.Status.Additions.ContainsKey("PartnerShutUp") && type != TALK_EVENT.NORMAL)
                {
                    var cd = new OtherAddition(null, partner, "PartnerShutUp", delay);
                    SkillHandler.ApplyAddition(partner, cd);
                }

            if (type != TALK_EVENT.NORMAL)
            {
                if (!partner.Status.Additions.ContainsKey("NotAtFreeTime"))
                {
                    var cd = new OtherAddition(null, partner, "NotAtFreeTime", 50000);
                    SkillHandler.ApplyAddition(partner, cd);
                }
                else
                {
                }
            }

            var id = partner.BaseData.id;
            var item = Character.Inventory.Equipments[EnumEquipSlot.PET];
            PartnerFactory.Instance.GetPartnerTalks(id);
            //if (ti == null) return;
            var packet = new SSMG_PARTNER_SEND_TALK();
            packet.PartnerID = item.ItemID;
            switch (type)
            {
                case TALK_EVENT.SUMMONED:
                    //if (ti.Onsummoned.Count > 0)
                    //    Speak(partner, getmessage(ti.Onsummoned));
                    packet.Parturn = (uint)TALK_EVENT.SUMMONED;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.BATTLE:
                    //if (ti.OnBattle.Count > 0)
                    //    Speak(partner, getmessage(ti.OnBattle));
                    packet.Parturn = (uint)TALK_EVENT.BATTLE;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERDEAD:
                    //if (ti.OnMasterDead.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterDead));
                    packet.Parturn = (uint)TALK_EVENT.MASTERDEAD;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.NORMAL:
                    //if (ti.OnNormal.Count > 0)
                    //    Speak(partner, getmessage(ti.OnNormal));
                    packet.Parturn = (uint)TALK_EVENT.NORMAL;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.JOINPARTY:
                    //if (ti.OnJoinParty.Count > 0)
                    //    Speak(partner, getmessage(ti.OnJoinParty));
                    packet.Parturn = (uint)TALK_EVENT.JOINPARTY;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.LEAVEPARTY:
                    //if (ti.OnLeaveParty.Count > 0)
                    //    Speak(partner, getmessage(ti.OnLeaveParty));
                    packet.Parturn = (uint)TALK_EVENT.LEAVEPARTY;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERFIGHTING:
                    //if (ti.OnMasterFighting.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterFighting));
                    packet.Parturn = (uint)TALK_EVENT.MASTERFIGHTING;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERLVUP:
                    //if (ti.OnMasterLevelUp.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterLevelUp));
                    packet.Parturn = (uint)TALK_EVENT.MASTERLVUP;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERQUIT:
                    //if (ti.OnMasterQuit.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterQuit));
                    packet.Parturn = (uint)TALK_EVENT.MASTERQUIT;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERSIT:
                    //if (ti.OnMasterSit.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterSit));
                    packet.Parturn = (uint)TALK_EVENT.MASTERSIT;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERRALEX:
                    //if (ti.OnMasterRelax.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterRelax));
                    packet.Parturn = (uint)TALK_EVENT.MASTERRALEX;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERBOW:
                    //if (ti.OnMasterBow.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterBow));
                    packet.Parturn = (uint)TALK_EVENT.MASTERBOW;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.MASTERLOGIN:
                    //if (ti.OnMasterLogin.Count > 0)
                    //    Speak(partner, getmessage(ti.OnMasterLogin));
                    packet.Parturn = (uint)TALK_EVENT.MASTERLOGIN;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.LVUP:
                    //if (ti.OnLevelUp.Count > 0)
                    //    Speak(partner, getmessage(ti.OnLevelUp));
                    packet.Parturn = (uint)TALK_EVENT.LVUP;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.EQUIP:
                    //if (ti.OnEquip.Count > 0)
                    //    Speak(partner, getmessage(ti.OnEquip));
                    packet.Parturn = (uint)TALK_EVENT.EQUIP;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.EAT:
                    //if (ti.OnEat.Count > 0)
                    //    Speak(partner, getmessage(ti.OnEat));
                    packet.Parturn = (uint)TALK_EVENT.EAT;
                    NetIo.SendPacket(packet);
                    break;
                case TALK_EVENT.EATREADY:
                    //if (ti.OnEatReady.Count > 0)
                    //    Speak(partner, getmessage(ti.OnEatReady));
                    packet.Parturn = (uint)TALK_EVENT.EATREADY;
                    NetIo.SendPacket(packet);
                    break;
            }
        }

        public void AddPartnerItemSkills(Item item)
        {
            if (item.BaseData.possibleSkill != 0) //装备附带主动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.possibleSkill, 1);
                if (skill != null)
                    if (!Character.Partner.Skills.ContainsKey(item.BaseData.possibleSkill))
                        Character.Partner.Skills.Add(item.BaseData.possibleSkill, skill);
            }

            if (item.BaseData.passiveSkill != 0) //装备附带被动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.passiveSkill, 1);
                if (skill != null)
                    if (!Character.Partner.Skills.ContainsKey(item.BaseData.passiveSkill))
                    {
                        Character.Partner.Skills.Add(item.BaseData.passiveSkill, skill);
                        if (!skill.BaseData.active)
                        {
                            var arg = new SkillArg();
                            arg.skill = skill;
                            SkillHandler.Instance.SkillCast(Character.Partner, Character.Partner, arg);
                        }
                    }
            }
        }

        public void CleanPartnerItemSkills(Item item)
        {
            if (item.BaseData.possibleSkill != 0) //装备附带主动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.possibleSkill, 1);
                if (skill != null)
                    if (Character.Partner.Skills.ContainsKey(item.BaseData.possibleSkill))
                        Character.Partner.Skills.Remove(item.BaseData.possibleSkill);
            }

            if (item.BaseData.passiveSkill != 0) //装备附带被动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.passiveSkill, 1);
                if (skill != null)
                    if (Character.Partner.Skills.ContainsKey(item.BaseData.passiveSkill))
                        Character.Partner.Skills.Remove(item.BaseData.passiveSkill);
                //SkillHandler.Instance.CastPassiveSkills(this.Character); to be checked
            }
        }

        /// <summary>
        ///     partner装备卸下过程，卸下该格子里的装备对应的所有格子里的道具，并移除道具附加的技能
        /// </summary>
        /// <param name="p"></param>
        public void OnPartnerItemUnequipt(EnumPartnerEquipSlot eqslot)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            var partner = Character.Partner;
            if (!partner.equipments.ContainsKey(eqslot))
                return;
            var oriItem = partner.equipments[eqslot];
            CleanPartnerItemSkills(oriItem);
            foreach (var i in oriItem.PartnerEquipSlot)
                if (partner.equipments.ContainsKey(i))
                {
                    if (partner.equipments[i].Stack == 0)
                    {
                        partner.equipments.Remove(i);
                    }
                    else
                    {
                        AddItem(partner.equipments[i], false);
                        partner.equipments.Remove(i);
                    }
                }

            MapServer.charDB.SavePartnerEquip(partner);
            var p = new SSMG_PARTNER_EQUIP_RESULT();
            p.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
            p.EquipItemID = oriItem.ItemID;
            p.PartnerEquipSlot = eqslot;
            p.MoveType = 1;
            NetIo.SendPacket(p);

            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);

            //broadcast
            SendPetBasicInfo();
            SendPetDetailInfo();
            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        /// <summary>
        ///     检查partner道具装备条件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int CheckPartnerEquipRequirement(Item item)
        {
            if (Character.Buff.Dead || Character.Buff.Confused || Character.Buff.Frosen || Character.Buff.Paralysis ||
                Character.Buff.Sleep || Character.Buff.Stone || Character.Buff.Stun)
                return -3;
            //if (lv < item.BaseData.possibleLv)
            //  return -15;
            if ((item.BaseData.itemType == ItemType.RIDE_PET || item.BaseData.itemType == ItemType.RIDE_PARTNER) &&
                Character.Marionette != null)
                return -2;
            if (item.BaseData.possibleRebirth)
                if (!Character.Partner.rebirth)
                    return -31;
            return 0;
        }

        /// <summary>
        ///     加点预览
        /// </summary>
        /// <param name="p"></param>
        public void OnPartnerPerkPreview(CSMG_PARTNER_PERK_PREVIEW p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;

            byte p0, p1, p2, p3, p4, p5;
            var partner = Character.Partner;
            p0 = partner.perk0;
            p1 = partner.perk1;
            p2 = partner.perk2;
            p3 = partner.perk3;
            p4 = partner.perk4;
            p5 = partner.perk5;


            partner.perk0 = p.Perk0;
            partner.perk1 = p.Perk1;
            partner.perk2 = p.Perk2;
            partner.perk3 = p.Perk3;
            partner.perk4 = p.Perk4;
            partner.perk5 = p.Perk5;

            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);


            var pp2 = new SSMG_PARTNER_PERK_PREVIEW();
            pp2.MaxPhyATK = partner.Status.max_atk1;
            pp2.MinPhyATK = partner.Status.min_atk1;

            pp2.MaxMAGATK = partner.Status.max_matk;
            pp2.MinMAGATK = partner.Status.min_matk;

            pp2.DEFAdd = (ushort)partner.Status.def_add;
            pp2.MaxHP = partner.MaxHP;
            pp2.DEF = partner.Status.def;

            pp2.MDEFAdd = (ushort)partner.Status.mdef_add;
            pp2.CSPD = partner.Status.cspd;
            pp2.MDEF = partner.Status.mdef;

            pp2.LongHit = partner.Status.hit_ranged;
            pp2.ShortHit = partner.Status.hit_melee;

            pp2.LongAvoid = partner.Status.avoid_ranged;
            pp2.ShortAvoid = partner.Status.avoid_melee;
            pp2.ASPD = partner.Status.aspd;

            partner.perk0 = p0;
            partner.perk1 = p1;
            partner.perk2 = p2;
            partner.perk3 = p3;
            partner.perk4 = p4;
            partner.perk5 = p5;

            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);

            NetIo.SendPacket(pp2);
        }

        /// <summary>
        ///     加点确认
        /// </summary>
        /// <param name="p"></param>
        public void OnPartnerPerkConfirm(CSMG_PARTNER_PERK_CONFIRM p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            var pp2 = new SSMG_PARTNER_PERK_CONFIRM();
            var partner = Character.Partner;
            if (p.Perk0 > 0)
                for (var i = 0; i < p.Perk0; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk0++;
                    }

            if (p.Perk1 > 0)
                for (var i = 0; i < p.Perk1; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk1++;
                    }

            if (p.Perk2 > 0)
                for (var i = 0; i < p.Perk2; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk2++;
                    }

            if (p.Perk3 > 0)
                for (var i = 0; i < p.Perk3; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk3++;
                    }

            if (p.Perk4 > 0)
                for (var i = 0; i < p.Perk4; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk4++;
                    }

            if (p.Perk5 > 0)
                for (var i = 0; i < p.Perk5; i++)
                    if (partner.perkpoint >= 1)
                    {
                        partner.perkpoint--;
                        partner.perk5++;
                    }

            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);
            pp2.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;

            pp2.Perkpoints = partner.perkpoint;
            pp2.Perk0 = partner.perk0;
            pp2.Perk1 = partner.perk1;
            pp2.Perk2 = partner.perk2;
            pp2.Perk3 = partner.perk3;
            pp2.Perk4 = partner.perk4;
            pp2.Perk5 = partner.perk5;

            pp2.MaxPhyATK = partner.Status.max_atk1;
            pp2.MinPhyATK = partner.Status.min_atk1;

            pp2.MaxMAGATK = partner.Status.max_matk;
            pp2.MinMAGATK = partner.Status.min_matk;

            pp2.DEFAdd = (ushort)partner.Status.def_add;
            pp2.MaxHP = partner.MaxHP;
            pp2.DEF = partner.Status.def;

            pp2.MDEFAdd = (ushort)partner.Status.mdef_add;
            pp2.CSPD = partner.Status.cspd;
            pp2.MDEF = partner.Status.mdef;

            pp2.LongHit = partner.Status.hit_ranged;
            pp2.ShortHit = partner.Status.hit_melee;

            pp2.LongAvoid = partner.Status.avoid_ranged;
            pp2.ShortAvoid = partner.Status.avoid_melee;
            pp2.ASPD = partner.Status.aspd;

            NetIo.SendPacket(pp2);
            MapServer.charDB.SavePartner(partner);
            SendPetBasicInfo();
            SendPetDetailInfo();
            StatusFactory.Instance.CalcStatus(Character);
        }

        /// <summary>
        ///     装备partner道具
        /// </summary>
        public void OnPartnerItemEquipt(CSMG_PARTNER_SETEQUIP p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            var partner = Character.Partner;
            if (p.EquipItemInventorySlot == 0xFFFFFFFF)
            {
                OnPartnerItemUnequipt(p.PartnerEquipSlot);
                return;
            }

            var item = Character.Inventory.GetItem(p.EquipItemInventorySlot); //item是这次装备的装备
            if (item == null)
                return;
            var count = item.Stack; //count是实际移动数量

            int result; //返回不能装备的类型
            result = CheckPartnerEquipRequirement(item); //检查装备条件
            if (result < 0) //不能装备
            {
                var p4 = new SSMG_ITEM_EQUIP();
                p4.InventorySlot = 0xffffffff;
                p4.Target = ContainerType.NONE;
                p4.Result = result;
                p4.Range = Character.Range;
                NetIo.SendPacket(p4);
                return;
            }

            var targetslots = new List<EnumPartnerEquipSlot>(); //PartnerEquipSlot involved in this item target slots
            foreach (var i in item.PartnerEquipSlot)
                if (!targetslots.Contains(i))
                    targetslots.Add(i);
            //卸下
            foreach (var i in targetslots)
            {
                //检查
                if (!partner.equipments.ContainsKey(i))
                    //该格子原来就是空的 直接下一个格子 特殊检查在循环外写
                    continue;
                //卸下
                //foreach (EnumPartnerEquipSlot j in partner.equipments[i].EquipSlot)
                {
                    //j位置的装备正式卸下
                    if (partner.equipments[i].BaseData.itemType == ItemType.UNION_WEAPON)
                        OnPartnerItemUnequipt(EnumPartnerEquipSlot.WEAPON);
                    else
                        OnPartnerItemUnequipt(EnumPartnerEquipSlot.COSTUME);
                }
            }

            if (count == 0) return;
            DeleteItem(item.Slot, 1, false);
            if (item.Stack > 0)
            {
                partner.equipments.Add(item.PartnerEquipSlot[0], item); //maybe check this in the future
                var pr = new SSMG_PARTNER_EQUIP_RESULT();
                pr.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                pr.EquipItemID = item.ItemID;
                pr.PartnerEquipSlot = item.PartnerEquipSlot[0];
                pr.MoveType = 0;
                NetIo.SendPacket(pr);
            }

            //renew stauts
            MapServer.charDB.SavePartnerEquip(partner);
            AddPartnerItemSkills(item);
            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);
            //broadcast
            SendPetBasicInfo();
            SendPetDetailInfo();
            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        public void OnPartnerFoodListSet(CSMG_PARTNER_SETFOOD p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            var partner = Character.Partner;
            byte foodslot = 0;
            if (p.MoveType == 1) //into
            {
                partner.foods.Add(ItemFactory.Instance.GetItem(p.ItemID));
                for (var i = 0; i < partner.foods.Count; i++)
                    if (partner.foods[i].ItemID == p.ItemID)
                        foodslot = (byte)i;
                //foodslot = (byte)(partner.foods.IndexOf(ItemFactory.Instance.GetItem(p.ItemID)));
            }
            else //out
            {
                //foodslot = (byte)(partner.foods.IndexOf(ItemFactory.Instance.GetItem(p.ItemID)));
                for (var i = 0; i < partner.foods.Count; i++)
                    if (partner.foods[i].ItemID == p.ItemID)
                        foodslot = (byte)i;
                partner.foods.RemoveAt(foodslot);
            }

            var pr = new SSMG_PARTNER_FOOD_LIST_RESULT();
            pr.FoodSlot = foodslot;
            pr.MoveType = p.MoveType;
            pr.FoodItemID = p.ItemID;
            NetIo.SendPacket(pr);
        }

        private float GetReliabilityRate(byte reliability)
        {
            switch (reliability)
            {
                case 0:
                    return 0f;
                case 1:
                    return 0.1f;
                case 2:
                    return 0.2f;
                case 3:
                    return 0.3f;
                case 4:
                    return 0.4f;
                case 5:
                    return 0.5f;
                case 6:
                    return 0.6f;
                case 7:
                    return 0.7f;
                case 8:
                    return 0.8f;
                case 9:
                    return 0.9f;
            }

            return 1f;
        }

        public bool OnPartnerFeed(uint FoodInventorySlot)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return false;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return false;
            var item = Character.Inventory.GetItem(FoodInventorySlot);
            if (item == null) return false;
            var food = PartnerFactory.Instance.GetPartnerFood(item.ItemID);
            var partner = Character.Partner;
            if (partner.Tasks.ContainsKey("Feed"))
            {
                SendSystemMessage("伙伴已经饱了哦");
                return false;
            }

            var fexp = food.rankexp;
            if (partner.reliability >= 4 && !partner.rebirth)
            {
                fexp = 0;
                SendSystemMessage("伙伴 " + partner.Name + " 已经达到未转生前的最大信赖度");
            }

            ExperienceManager.Instance.ApplyPartnerReliabilityEXP(partner, fexp);
            var nextlvup = "";
            if (partner.reliability + 1 < 10)
                nextlvup = "(" + partner.reliabilityexp + "/" +
                           ExperienceManager.Instance.PartnerReliabilityEXPChart[(byte)(partner.reliability + 1)] + ")";
            SendSystemMessage("伙伴 " + partner.Name + " 获得了" + fexp + "点信赖经验值" + nextlvup);
            ExperienceManager.Instance.ApplyPartnerLvExp(Character, food.rankexp);
            partner.reliabilityuprate = (ushort)(food.reliabilityuprate + GetReliabilityRate(partner.reliability));
            var second = (int)food.nextfeedtime;
            partner.nextfeedtime = DateTime.Now + new TimeSpan(0, 0, second);
            //need food calculation here if rank changes need to send rank_update //check if ok to eat at this time
            if (!partner.Tasks.ContainsKey("Feed")) //This is a double check here
            {
                var task = new Feed(this, partner, food.nextfeedtime);
                partner.Tasks.Add("Feed", task);
                task.Activate();
            }

            //DeleteItem(item.Slot, 1, false);
            var pr = new SSMG_PARTNER_FEED_RESULT();
            pr.MessageSwitch = 0;
            pr.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
            pr.FoodItemID = food.itemID;
            pr.ReliabilityUpRate = partner.reliabilityuprate;
            if (partner.nextfeedtime > DateTime.Now)
                pr.NextFeedTime = (uint)(partner.nextfeedtime - DateTime.Now).TotalSeconds;
            else
                pr.NextFeedTime = 0;
            pr.PartnerRank = Character.Partner.rank;
            NetIo.SendPacket(pr);
            Partner.StatusFactory.Instance.CalcPartnerStatus(partner);
            SendPetBasicInfo();
            SendPetDetailInfo();
            StatusFactory.Instance.CalcStatus(Character);
            if (food.itemID == 168500107) //成功使用【超级棒棒糖Ver.A】10次
                TitleProccess(Character, 29, 1);
            return true;
        }

        public void OnPartnerCubeLearn(uint CubeItemID)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            var item = ItemFactory.Instance.GetItem(CubeItemID);
            if (item == null) return;
            var cube = PartnerFactory.Instance.GetCubeItemID(item.ItemID);
            switch (cube.cubetype)
            {
                case PartnerCubeType.CONDITION:
                    if (!Character.Partner.equipcubes_condition.Contains(cube.uniqueID))
                    {
                        Character.Partner.equipcubes_condition.Add(cube.uniqueID);
                    }
                    else
                    {
                        SendSystemMessage("这个技能块已经学习过了。");
                        return;
                    }

                    break;
                case PartnerCubeType.ACTION:
                    if (!Character.Partner.equipcubes_action.Contains(cube.uniqueID))
                    {
                        Character.Partner.equipcubes_action.Add(cube.uniqueID);
                    }
                    else
                    {
                        SendSystemMessage("这个技能块已经学习过了。");
                        return;
                    }

                    break;
                case PartnerCubeType.ACTIVESKILL:
                    if (!Character.Partner.equipcubes_activeskill.Contains(cube.uniqueID))
                    {
                        Character.Partner.equipcubes_activeskill.Add(cube.uniqueID);
                    }
                    else
                    {
                        SendSystemMessage("这个技能块已经学习过了。");
                        return;
                    }

                    break;
                case PartnerCubeType.PASSIVESKILL:
                    if (!Character.Partner.equipcubes_passiveskill.Contains(cube.uniqueID))
                    {
                        Character.Partner.equipcubes_passiveskill.Add(cube.uniqueID);
                    }
                    else
                    {
                        SendSystemMessage("这个技能块已经学习过了。");
                        return;
                    }

                    break;
            }

            DeleteItemID(CubeItemID, 1, true);
            SetPartnerSkills(Character.Partner);
            SendSystemMessage("技能块【" + cube.cubename + "】学习成功了！");
            OnPartnerAIOpen();
            MapServer.charDB.SavePartnerCube(Character.Partner);
        }

        public void OnPartnerCubeDelete(CSMG_PARTNER_CUBE_DELETE p)
        {
            var cubeID = p.CubeID;
            if (Character.Partner.equipcubes_activeskill.Contains(cubeID))
                Character.Partner.equipcubes_activeskill.Remove(cubeID);
            if (Character.Partner.equipcubes_passiveskill.Contains(cubeID))
                Character.Partner.equipcubes_passiveskill.Remove(cubeID);
            if (Character.Partner.equipcubes_action.Contains(cubeID))
                Character.Partner.equipcubes_action.Remove(cubeID);
            if (Character.Partner.equipcubes_condition.Contains(cubeID))
                Character.Partner.equipcubes_condition.Remove(cubeID);
            SetPartnerSkills(Character.Partner);
            OnPartnerAIOpen();
            MapServer.charDB.SavePartnerCube(Character.Partner);
        }

        public void OnPartnerAIOpen()
        {
            try
            {
                if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                    return;
                if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                    return;
                var partner = Character.Partner;
                var pr = new SSMG_PARTNER_AI_DETAIL((byte)partner.equipcubes_condition.Count,
                    (byte)partner.equipcubes_action.Count, (byte)partner.equipcubes_activeskill.Count,
                    (byte)partner.equipcubes_passiveskill.Count);
                pr.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                pr.Conditions_ID = partner.ai_conditions;
                pr.Reactions_ID = partner.ai_reactions;
                pr.Time_Intervals = partner.ai_intervals;
                pr.AI_states = partner.ai_states;
                pr.BasicAI = partner.basic_ai_mode;
                pr.Cubes_Condition = partner.equipcubes_condition;
                pr.Cubes_Action = partner.equipcubes_action;
                pr.Cubes_Activeskill = partner.equipcubes_activeskill;
                pr.Cubes_Passiveskill = partner.equipcubes_passiveskill;
                NetIo.SendPacket(pr);
                SendSystemMessage("PARTNER AI系統尚未實裝。");
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnPartnerAISetup(CSMG_PARTNER_AI_DETAIL_SETUP p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            //SendSystemMessage("当前版本无法自定义宠物AI");
            //return;
            var partner = Character.Partner;
            partner.ai_conditions = p.Conditions_ID;
            partner.ai_reactions = p.Reactions_ID;
            partner.ai_intervals = p.Time_Intervals;
            partner.ai_states = p.AI_states;
            partner.basic_ai_mode = p.BasicAI;
            var pr = new SSMG_PARTNER_AI_SETUP_RESULT();
            pr.Success = 0;
            pr.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
            NetIo.SendPacket(pr);
        }

        public void OnPartnerAIClose()
        {
        }

        public void OnPartnerAIModeSelection(CSMG_PARTNER_AI_MODE_SELECTION p)
        {
            if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                return;
            if (!Character.Inventory.Equipments[EnumEquipSlot.PET].IsPartner)
                return;
            if (p.AIMode >= 3)
            {
                SendSystemMessage("PARTNER AI系統尚未實裝。");
                return;
            }

            var partner = Character.Partner;
            partner.ai_mode = p.AIMode;
            var pr = new SSMG_PARTNER_AI_MODE_SELECTION();
            pr.PartnerInventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
            pr.AIMode = partner.ai_mode;
            NetIo.SendPacket(pr);
        }

        //#endregion

        private readonly int hackCount = 0;

        //技能独立cd列表
        private readonly Dictionary<uint, DateTime> SingleCDLst = new Dictionary<uint, DateTime>();
        private DateTime assassinateStamp = DateTime.Now;
        private DateTime attackStamp = DateTime.Now;
        private bool AttactFinished;
        private int delay;
        private DateTime hackStamp = DateTime.Now;
#pragma warning disable CS0169 // 从不使用字段“MapClient.lastAttackRandom”
        private short lastAttackRandom;
#pragma warning restore CS0169 // 从不使用字段“MapClient.lastAttackRandom”
        private short lastCastRandom;

        private CSMG_SKILL_ATTACK Lastp;

        private Thread main;
        public List<uint> nextCombo = new List<uint>();
        private DateTime skillDelay = DateTime.Now;

        public DateTime SkillDelay
        {
            set => skillDelay = value;
        }

        public void OnSkillLvUP(CSMG_SKILL_LEVEL_UP p)
        {
            var p1 = new SSMG_SKILL_LEVEL_UP();
            var skillID = p.SkillID;
            byte type = 0;
            if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(skillID))
                type = 1;
            else if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(skillID))
                type = 2;
            else if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(skillID))
                type = 3;
            else if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(skillID))
                type = 4;
            if (type == 0)
            {
                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_EXIST;
            }
            else
            {
                if (type == 1)
                {
                    if (!Character.Skills.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills[skillID].Level);
                        if (Character.JobLevel1 < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills[skillID].Level == Character.Skills[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint -= 1;
                                Character.Skills[skillID] = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills[skillID].Level + 1));
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 2)
                {
                    if (!Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills2[skillID].Level);
                        if (Character.JobLevel2X < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint2X < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills2[skillID].Level == Character.Skills2[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint2X -= 1;
                                var data = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills2[skillID].Level + 1));
                                Character.Skills2[skillID] = data;
                                Character.Skills2_1[skillID] = data;
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 3)
                {
                    if (!Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills2[skillID].Level);
                        if (Character.JobLevel2T < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint2T < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills2[skillID].Level == Character.Skills2[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint2T -= 1;
                                var data = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills2[skillID].Level + 1));
                                Character.Skills2[skillID] = data;
                                Character.Skills2_2[skillID] = data;
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }

                if (type == 4)
                {
                    if (!Character.Skills3.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        var skill = SkillFactory.Instance.GetSkill(skillID, Character.Skills3[skillID].Level);
                        if (Character.JobLevel3 < skill.JobLv)
                        {
                            SendSystemMessage(string.Format("{1} 未达到技能升级等级！需求等级：{0}", skill.JobLv, skill.Name));
                            return;
                        }

                        if (Character.SkillPoint3 < 1)
                        {
                            p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            if (skill.MaxLevel == 6 && Character.Skills[skillID].Level == 5)
                            {
                                SendSystemMessage("无法直接领悟这项技能的精髓。");
                                return;
                            }

                            if (Character.Skills3[skillID].Level == Character.Skills3[skillID].MaxLevel)
                            {
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.SKILL_MAX_LEVEL_EXEED;
                            }
                            else
                            {
                                Character.SkillPoint3 -= 1;
                                Character.Skills3[skillID] = SkillFactory.Instance.GetSkill(skillID,
                                    (byte)(Character.Skills3[skillID].Level + 1));
                                p1.Result = SSMG_SKILL_LEVEL_UP.LearnResult.OK;
                                p1.SkillID = skillID;
                            }
                        }
                    }
                }
            }

            p1.SkillPoints = Character.SkillPoint;
            if (Character.Job == Character.Job2X)
            {
                p1.SkillPoints2 = Character.SkillPoint2X;
                p1.Job = 1;
            }
            else if (Character.Job == Character.Job2T)
            {
                p1.SkillPoints2 = Character.SkillPoint2T;
                p1.Job = 2;
            }
            else if (Character.Job == Character.Job3)
            {
                p1.SkillPoints2 = Character.SkillPoint3;
                p1.Job = 3;
            }
            else
            {
                p1.Job = 0;
            }

            NetIo.SendPacket(p1);
            SendSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            MapServer.charDB.SaveChar(Character, true);
        }

        public void OnSkillLearn(CSMG_SKILL_LEARN p)
        {
            var p1 = new SSMG_SKILL_LEARN();
            var skillID = p.SkillID;
            byte type = 0;
            if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(skillID))
                type = 1;
            else if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(skillID))
                type = 2;
            else if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(skillID))
                type = 3;
            else if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(skillID))
                type = 4;
            if (type == 0)
            {
                p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_EXIST;
            }
            else
            {
                if (type == 1)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.JobBasic)[skillID];
                    if (Character.Skills.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;

                            if (Character.JobLevel1 < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }

                            else
                            {
                                Character.SkillPoint -= 3;
                                Character.Skills.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                if (Character.Job == Character.Job2X)
                                    p1.SkillPoints2 = Character.SkillPoint2X;
                                else if (Character.Job == Character.Job2T)
                                    p1.SkillPoints2 = Character.SkillPoint2T;
                            }
                        }
                    }
                }
                else if (type == 2)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job2X)[skillID];
                    if (Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint2X < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel2X < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint2X -= 3;
                                Character.Skills2.Add(skillID, skill);
                                Character.Skills2_1.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint2X;
                            }
                        }
                    }
                }
                else if (type == 3)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job2T)[skillID];

                    if (Character.Skills2.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint2T < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel2T < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint2T -= 3;
                                Character.Skills2.Add(skillID, skill);
                                Character.Skills2_2.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint2T;
                            }
                        }
                    }
                }
                else if (type == 4)
                {
                    var jobLV = SkillFactory.Instance.SkillList(Character.Job3)[skillID];

                    if (Character.Skills3.ContainsKey(skillID))
                    {
                        p1.Result = SSMG_SKILL_LEARN.LearnResult.SKILL_NOT_LEARNED;
                    }
                    else
                    {
                        if (Character.SkillPoint3 < 3)
                        {
                            p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_SKILL_POINT;
                        }
                        else
                        {
                            var skill = SkillFactory.Instance.GetSkill(skillID, 1);
                            if (skill.JobLv != 0)
                                jobLV = skill.JobLv;
                            if (Character.JobLevel3 < jobLV)
                            {
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.NOT_ENOUGH_JOB_LEVEL;
                            }
                            else
                            {
                                Character.SkillPoint3 -= 3;
                                Character.Skills3.Add(skillID, skill);
                                p1.Result = SSMG_SKILL_LEARN.LearnResult.OK;
                                p1.SkillID = skillID;
                                p1.SkillPoints = Character.SkillPoint;
                                p1.SkillPoints2 = Character.SkillPoint3;
                            }
                        }
                    }
                }
            }

            NetIo.SendPacket(p1);
            SendSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            MapServer.charDB.SaveChar(Character, true);
        }

        public void OnSkillAttack(CSMG_SKILL_ATTACK p, bool auto)
        {
            var needthread = true;

            if (Character == null)
                return;
            if (!Character.Online || Character.HP == 0)
                return;

            var dActor = Map.GetActor(p.ActorID);
            SkillArg arg;

            var sActor = map.GetActor(Character.ActorID);
            if (sActor == null) return;
            if (dActor == null) return;
            if (sActor.MapID != dActor.MapID) return;
            if (sActor.TInt["targetID"] != dActor.ActorID)
            {
                sActor.TInt["targetID"] = (int)dActor.ActorID;
                //SendSystemMessage("锁定了【" + dActor.Name + "】作为目标");
                //Character.AutoAttack = true;

                Character.PartnerTartget = dActor; // Partner will follow the entity assigned to PartnerTarget.
            }

            if (needthread)
            {
                if (!auto && Character.AutoAttack) //客户端发来的攻击，但已开启自动
                {
                    Character.TInt["攻击检测"] += 1;
                    if (Character.TInt["攻击检测"] >= 3)
                        ScriptManager.Instance.VariableHolder.AInt[Character.Name + "攻击检测"] += Character.TInt["攻击检测"];
                    Lastp = p;
                    //return;
                }

                if (auto && !Character.AutoAttack) //自动攻击，但人物处于不能自动攻击状态
                    return;
            }

            byte s = 0;

            //射程判定
            if (Character == null || dActor == null)
                return;
            if (Character.Range + 1
                < Math.Max(Math.Abs(Character.X - dActor.X) / 100
                    , Math.Abs(Character.Y - dActor.Y) / 100))
            {
                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            Character.LastAttackActorID = 0;

            //this.lastAttackRandom = p.Random;
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (Character.Status.Additions.ContainsKey("Stun") || Character.Status.Additions.ContainsKey("Sleep") ||
                Character.Status.Additions.ContainsKey("Frosen") ||
                Character.Status.Additions.ContainsKey("Stone"))
                return;
            if (dActor == null || DateTime.Now < attackStamp)
            {
                if (s == 1)
                {
                    arg = new SkillArg();
                    arg.sActor = Character.ActorID;
                    arg.type = (ATTACK_TYPE)0xff;
                    arg.affectedActors.Add(Character);
                    arg.Init();
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                    Character.AutoAttack = false;
                    return;
                }

                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            if (dActor.HP == 0 || dActor.Buff.Dead)
            {
                arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.type = (ATTACK_TYPE)0xff;
                arg.affectedActors.Add(Character);
                arg.Init();
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);
                Character.AutoAttack = false;
                return;
            }

            arg = new SkillArg();

            delay = (int)(2000 * (1.0f - Character.Status.aspd / 1000.0f));
            delay = (int)(delay * arg.delayRate);
            if (Character.Status.aspd_skill_perc >= 1f)
                delay = (int)(delay / Character.Status.aspd_skill_perc);

            if (!needthread && Character.HP > 0)
                SkillHandler.Instance.Attack(Character, dActor, arg); //攻击

            if (Character.HP > 0 && !AttactFinished && needthread) //处于战斗状态
                SkillHandler.Instance.Attack(Character, dActor, arg); //攻击

            if (arg.affectedActors.Count > 0)
                attackStamp = DateTime.Now + new TimeSpan(0, 0, 0, 0, delay);

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, Character, true);

            AttactFinished = false;
            PartnerTalking(Character.Partner, TALK_EVENT.BATTLE, 1, 20000);
            //新加
            if (needthread && s == 0)
            {
                Lastp = p;
                Character.LastAttackActorID = dActor.ActorID;
                delay = (int)(2000 * (1.0f - Character.Status.aspd / 1000.0f));
                delay = (int)(delay * arg.delayRate);
                if (Character.Status.aspd_skill_perc >= 1f)
                    delay = (int)(delay / Character.Status.aspd_skill_perc);

                //谜一样的弓,双枪延迟缩短,先注释掉
                //if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                //{
                //    ItemType it = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType;
                //    if (it == ItemType.DUALGUN || it == ItemType.BOW)
                //        delay = (int)(delay * 0.6f);
                //}

                try
                {
                    if (main != null)
                        ClientManager.RemoveThread(main.Name);
                    if (Character == null)
                        return;
                    if (this == null)
                        return;
                    main = new Thread(MainLoop);
                    main.Name = string.Format("ThreadPoolMainLoopAUTO({0})" + Character.Name,
                        (object)main.ManagedThreadId);
                    ClientManager.AddThread(main);
                    Character.AutoAttack = true;
                    main.Start();
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
            }
        }


        private void MainLoop()
        {
            try
            {
                if (Character == null)
                {
                    if (main != null)
                        ClientManager.RemoveThread(main.Name);
                    return;
                }

                if (this == null)
                    return;

                if (delay <= 0)
                    delay = 60;
                Thread.Sleep((int)delay);

                if (Character != null)
                {
                    OnSkillAttack(Lastp, true);
                    Character.TInt["攻击检测"] = 0;
                }
                else
                {
                    ClientManager.RemoveThread(main.Name);
                }
            }

            catch (Exception ex)
            {
                Logger.ShowError(main.Name + " Thread " + ex);
            }
        }


        public void OnSkillChangeBattleStatus(CSMG_SKILL_CHANGE_BATTLE_STATUS p)
        {
            if (p.Status == 0)
                Character.AutoAttack = false;

            if (Character.BattleStatus != p.Status)
            {
                Character.BattleStatus = p.Status;
                SendChangeStatus();
            }

            if (Character.Tasks.ContainsKey("RangeAttack") && Character.BattleStatus == 0)
            {
                Character.Tasks["RangeAttack"].Deactivate();
                Character.Tasks.Remove("RangeAttack");
                Character.TInt["RangeAttackMark"] = 0;
            }

            if (Character.Tasks.ContainsKey("SkillCast") && Character.BattleStatus == 0 &&
                (Character.Skills.ContainsKey(14000) || Character.Skills3.ContainsKey(14000)) &&
                (Character.Job == PC_JOB.CARDINAL || Character.Job == PC_JOB.ASTRALIST))
            {
                /*if (this.Character.Tasks["SkillCast"].getActivated())
                {
                    this.Character.Tasks["SkillCast"].Deactivate();
                    this.Character.Tasks.Remove("SkillCast");
                }*/

                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, Character, true);

                var p2 = new SSMG_SKILL_CAST_RESULT();
                p2.ActorID = Character.ActorID;
                p2.Result = 20;
                NetIo.SendPacket(p2);
            }
        }

        public void OnSkillCast(CSMG_SKILL_CAST p)
        {
            OnSkillCast(p, true);
        }

        private bool checkSkill(uint skillID, byte skillLV)
        {
            Dictionary<uint, byte> skills;
            Dictionary<uint, byte> skills2X;
            Dictionary<uint, byte> skills2T;
            Dictionary<uint, byte> skills3;
            Dictionary<uint, byte> skillsHeat;
            skills = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p1);
            skills2X = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p21);
            skills2T = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p22);
            skills3 = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.p3);
            skillsHeat = SkillFactory.Instance.CheckSkillList(Character, SkillFactory.SkillPaga.none);

            if (Character.Skills.ContainsKey(skillID))
                if (Character.Skills[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2.ContainsKey(skillID))
                if (Character.Skills2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2_1.ContainsKey(skillID))
                if (Character.Skills2_1[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2_2.ContainsKey(skillID))
                if (Character.Skills2_2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills2.ContainsKey(skillID))
                if (Character.Skills2[skillID].Level >= skillLV)
                    return true;
            if (Character.Skills3.ContainsKey(skillID))
                if (Character.Skills3[skillID].Level >= skillLV)
                    return true;
            if (Character.SkillsReserve.ContainsKey(skillID))
                if (Character.SkillsReserve[skillID].Level >= skillLV)
                    return true;
            if (Character.SkillsReserve.ContainsKey(skillID) && Character.DominionReserveSkill)
                if (Character.SkillsReserve[skillID].Level >= skillLV)
                    return true;
            if (Character.JobJoint != PC_JOB.NONE)
            {
                var skill =
                    from c in SkillFactory.Instance.SkillList(Character.JobJoint)
                    where c.Value <= Character.JointJobLevel
                    select c;
                foreach (var i in skill)
                    if (i.Key == skillID && Character.JointJobLevel >= i.Value)
                        return true;
            }

            return false;
        }

        public void OnSkillCast(CSMG_SKILL_CAST p, bool useMPSP)
        {
            OnSkillCast(p, useMPSP, false);
        }

        /// <summary>
        ///     检查技能是否符合使用条件
        /// </summary>
        /// <param name="skill">技能数据</param>
        /// <param name="arg">技能参数</param>
        /// <param name="mp">mp</param>
        /// <param name="sp">sp</param>
        /// <param name="ep">ep</param>
        /// <returns>结果</returns>
        private short CheckSkillUse(SagaDB.Skill.Skill skill, SkillArg arg, ushort mp, ushort sp, ushort ep)
        {
            if (SingleCDLst.ContainsKey(arg.skill.ID) && DateTime.Now < SingleCDLst[arg.skill.ID] &&
                !nextCombo.Contains(arg.skill.ID))
                return -30;
            if (arg.skill.ID == 3372)
            {
                SingleCDLst.Clear();
                return 0;
            }

            if (DateTime.Now < skillDelay && !nextCombo.Contains(arg.skill.ID))
                return -30;
            if (Character.SP < sp || Character.MP < mp || Character.EP < ep)
            {
                if (Character.SP < sp && Character.MP < mp)
                    return -1;
                if (Character.SP < sp)
                    return -16;
                if (Character.MP < mp)
                    return -15;
                return -62;
            }

            if (!SkillHandler.Instance.CheckSkillCanCastForWeapon(Character, arg))
                return -5;

            if (Character.Status.Additions.ContainsKey("Silence"))
                return -7;

            if (Character.Status.Additions.ContainsKey("居合模式"))
            {
                if (arg.skill.ID != 2129)
                    return -7;
                Character.Status.Additions["居合模式"].AdditionEnd();
                Character.Status.Additions.Remove("居合模式");
            }

            if (GetPossessionTarget() != null)
                if (GetPossessionTarget().Buff.Dead && arg.skill.ID != 3055)
                    return -27;
            if (scriptThread != null) return -59;
            if (skill.NoPossession)
                if (Character.Buff.GetReadyPossession || Character.PossessionTarget != 0)
                    return -25;

            if (skill.NotBeenPossessed)
                if (Character.PossesionedActors.Count > 0)
                    return -24;

            if (Character.Tasks.ContainsKey("SkillCast"))
            {
                if (arg.skill.ID == 3311)
                    return 0;
                return -8;
            }

            var res = (short)SkillHandler.Instance.TryCast(Character, Map.GetActor(arg.dActor), arg);
            if (res < 0)
                return res;
            return 0;
        }

        public void OnSkillCast(CSMG_SKILL_CAST p, bool useMPSP, bool nocheck)
        {
            if (((!checkSkill(p.SkillID, p.SkillLv) && Character.Account.GMLevel < 2) ||
                 (p.Random == lastCastRandom && Character.Account.GMLevel < 2)) && !nocheck)
            {
                SendHack();
                if (hackCount > 2)
                    return;
            }

            //断掉自动放技能
            Character.AutoAttack = false;
            if (main != null)
                ClientManager.RemoveThread(main.Name);


            lastCastRandom = p.Random;
            var skill = SkillFactory.Instance.GetSkill(p.SkillID, p.SkillLv);
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (Character.Tasks.ContainsKey("Regeneration"))
            {
                Character.Tasks["Regeneration"].Deactivate();
                Character.Tasks.Remove("Regeneration");
            }

            var arg = new SkillArg();
            arg.sActor = Character.ActorID;
            arg.dActor = p.ActorID;
            arg.skill = skill;
            arg.x = p.X;
            arg.y = p.Y;
            arg.argType = SkillArg.ArgType.Cast;
            ushort sp, mp, ep;
            //凭依时消耗加倍
            if (Character.PossessionTarget != 0)
            {
                sp = (ushort)(skill.SP * 2);
                mp = (ushort)(skill.MP * 2);
            }
            else
            {
                sp = skill.SP;
                mp = skill.MP;
            }

            if (Character.Status.Additions.ContainsKey("SwordEaseSp"))
                //sp = (ushort)(skill.SP * 2);
                //mp = (ushort)(skill.MP * 2);
                sp = (ushort)(skill.SP * 0.7);
            //mp = (ushort)(skill.MP * 0.7);
            if (Character.Status.Additions.ContainsKey("元素解放"))
            {
                sp = (ushort)(skill.SP * 2);
                mp = (ushort)(skill.MP * 2);
            }

            if (Character.Status.zenList.Contains((ushort)skill.ID) ||
                Character.Status.darkZenList.Contains((ushort)skill.ID))
                mp = (ushort)(mp * 2);

            if (Character.Status.Additions.ContainsKey("EnergyExcess")) //能量增幅耗蓝加深
            {
                float[] rate = { 0, 0.05f, 0.16f, 0.28f, 0.4f, 0.65f };
                mp += (ushort)(skill.MP * rate[Character.TInt["EnergyExcess"]]);
            }

            if (!useMPSP)
            {
                sp = 0;
                mp = 0;
            }

            ep = skill.EP;
            arg.useMPSP = useMPSP;
            //检查技能是否复合使用条件 0为符合, 其他为使用失败
            arg.result = CheckSkillUse(skill, arg, mp, sp, ep);

            if (arg.result == 0)
            {
                //使物理技能的读条时间受aspd影响,法系读条受cspd影响.
                //2018.07.13 现在魔法系职业的读条时间不可能小于0.5秒.
                if (skill.BaseData.flag.Test(SkillFlags.PHYSIC))
                    arg.delay = (uint)(skill.CastTime * (1.0f - Character.Status.aspd / 1000.0f));
                else
                    arg.delay = (uint)Math.Max(skill.CastTime * (1.0f - Character.Status.cspd / 1000.0f), 500);
                if (arg.skill.ID == 2559)
                {
                    if (Character.Gold >= 90000000)
                        arg.delay = (uint)(arg.delay * 0.5f);
                    else if (Character.Gold >= 50000000)
                        arg.delay = (uint)(arg.delay * 0.6f);
                    else if (Character.Gold >= 5000000)
                        arg.delay = (uint)(arg.delay * 0.7f);
                    else if (Character.Gold >= 500000)
                        arg.delay = (uint)(arg.delay * 0.8f);
                    else if (Character.Gold >= 50000)
                        arg.delay = (uint)(arg.delay * 0.9f);
                }

                if (Character.Status.delayCancelList.ContainsKey((ushort)arg.skill.ID))
                {
                    var rate = Character.Status.delayCancelList[(ushort)arg.skill.ID];
                    arg.delay = (uint)(arg.delay * (1f - rate / 100.0f));
                }

                //bool get = Character.Status.Additions.ContainsKey("EaseCt");
                if (Character.Status.Additions.ContainsKey("EaseCt") && arg.skill.ID != 2238) //杀界模块
                {
                    var eclv = new[] { 0f, 0.5f, 0.7f, 0.8f, 0.9f, 1.0f }[Character.Status.EaseCt_lv];
                    arg.delay = (uint)(arg.delay * (1.0f - eclv));
                    SkillHandler.RemoveAddition(Character, "EaseCt");
                }


                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);
                //if (this.Character.Status.Additions.ContainsKey("SwordEaseSp"))
                //{
                //    this.nextCombo.Clear();
                //    OnSkillCastComplete(arg);
                //}
                //else 
                if (skill.CastTime > 0 && !nextCombo.Contains(arg.skill.ID))
                {
                    var task = new SkillCast(this, arg);
                    Character.Tasks.Add("SkillCast", task);

                    task.Activate();
                    nextCombo.Clear();
                    ;
                }
                else
                {
                    nextCombo.Clear();
                    OnSkillCastComplete(arg);
                }

                if (Character.Status.Additions.ContainsKey("Parry"))
                    arg.delay = (uint)arg.skill.BaseData.delay;
            }
            else
            {
                Character.e.OnActorSkillUse(Character, arg);
            }
        }

        public void OnSkillCastComplete(SkillArg skill)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (skill.dActor != 0xFFFFFFFF)
            {
                var dActor = Map.GetActor(skill.dActor);
                if (dActor != null)
                {
                    skill.argType = SkillArg.ArgType.Active;
                    PartnerTalking(Character.Partner, TALK_EVENT.BATTLE, 1, 20000);
                    if (skill.useMPSP)
                    {
                        uint mpCost = skill.skill.MP;
                        uint spCost = skill.skill.SP;
                        uint epCost = skill.skill.EP;
                        if (Character.Status.sp_rate_down_iris < 100)
                            spCost = (uint)(spCost * (Character.Status.sp_rate_down_iris / 100.0f));
                        if (Character.Status.mp_rate_down_iris < 100)
                            mpCost = (uint)(mpCost * (Character.Status.mp_rate_down_iris / 100.0f));

                        if (Character.Status.doubleUpList.Contains((ushort)skill.skill.ID))
                            spCost = (ushort)(spCost * 2);

                        if (Character.Status.Additions.ContainsKey("SwordEaseSp"))
                            //mpCost = (ushort)(mpCost*0.7);
                            spCost = (ushort)(spCost * 0.7);
                        if (Character.Status.Additions.ContainsKey("HarvestMaster"))
                        {
                            mpCost = (ushort)(mpCost * (float)(1.0f - Character.Status.HarvestMaster_Lv * 0.05));
                            spCost = (ushort)(spCost * (float)(1.0f - Character.Status.HarvestMaster_Lv * 0.05));
                        }

                        if (skill.skill.ID == 2527 && (Character.Skills2_2.ContainsKey(2355) ||
                                                       Character.DualJobSkill.Exists(x => x.ID == 2355))) //当技能为神速斩
                        {
                            //这里取副职的拔刀斩等级
                            var duallv = 0;
                            if (Character.DualJobSkill.Exists(x => x.ID == 2355))
                                duallv = Character.DualJobSkill.FirstOrDefault(x => x.ID == 2355).Level;

                            //这里取主职的拔刀斩等级
                            var mainlv = 0;
                            if (Character.Skills2_2.ContainsKey(2355))
                                mainlv = Character.Skills2_2[2355].Level;
                            //获取最高的拔刀斩等级
                            var maxlevel = Math.Max(duallv, mainlv);
                            spCost = (ushort)(spCost - spCost * maxlevel * 0.04f);
                        }

                        if (Character.PossessionTarget != 0)
                        {
                            mpCost = (ushort)(mpCost * 2);
                            spCost = (ushort)(spCost * 2);
                        }

                        if (Character.Status.Additions.ContainsKey("Zensss"))
                            mpCost *= 2;

                        if (Character.Status.Additions.ContainsKey("EnergyExcess")) //能量增幅耗蓝加深
                        {
                            float[] rate = { 0, 0.05f, 0.16f, 0.28f, 0.4f, 0.65f };
                            mpCost += (ushort)(mpCost * rate[Character.TInt["EnergyExcess"]]);
                        }

                        if (mpCost > Character.MP && spCost > Character.SP)
                        {
                            skill.result = -1;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        if (mpCost > Character.MP)
                        {
                            skill.result = -15;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        if (spCost > Character.SP)
                        {
                            skill.result = -16;
                            Character.e.OnActorSkillUse(Character, skill);
                            return;
                        }

                        Character.MP -= mpCost;
                        if (Character.MP < 0)
                            Character.MP = 0;

                        Character.SP -= spCost;
                        if (Character.SP < 0)
                            Character.SP = 0;

                        Character.EP -= epCost;
                        if (Character.EP < 0)
                            Character.EP = 0;

                        SendActorHPMPSP(Character);
                    }

                    SkillHandler.Instance.SkillCast(Character, dActor, skill);
                }
                else
                {
                    skill.result = -11;
                    Character.e.OnActorSkillUse(Character, skill);
                }
            }
            else
            {
                skill.argType = SkillArg.ArgType.Active;
                if (skill.useMPSP)
                {
                    if (skill.skill.MP > Character.MP && skill.skill.SP > Character.SP)
                    {
                        skill.result = -1;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    if (skill.skill.MP > Character.MP)
                    {
                        skill.result = -15;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    if (skill.skill.SP > Character.SP)
                    {
                        skill.result = -16;
                        Character.e.OnActorSkillUse(Character, skill);
                        return;
                    }

                    Character.MP -= skill.skill.MP;
                    Character.SP -= skill.skill.SP;
                    SendActorHPMPSP(Character);
                }

                SkillHandler.Instance.SkillCast(Character, Character, skill);
            }

            if (Character.Pet != null)
                if (Character.Pet.Ride)
                    SkillHandler.Instance.ProcessPetGrowth(Character.Pet, PetGrowthReason.UseSkill);

            //技能延迟
            //if (this.Character.Status.Additions.ContainsKey("SwordEaseSp"))
            //{
            //    skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay * 0.2f));
            //}
            //else 
            if (Character.Status.delayCancelList.ContainsKey((ushort)skill.skill.ID))
                skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0,
                    (int)(skill.skill.Delay *
                          (1f - Character.Status.delayCancelList[(ushort)skill.skill.ID] / 100.0f)));
            else
                skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.Delay);

            //if (this.Character.Status.Additions.ContainsKey("DelayOut"))
            //    skillDelay = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1);

            if (Character.Status.Additions.ContainsKey("OverWork") &&
                !skill.skill.BaseData.flag.Test(SkillFlags.PHYSIC)) //狂乱时间
            {
                var DelayTime = (Character.Status.Additions["OverWork"] as DefaultBuff).Variable["OverWork"];
                skillDelay = DateTime.Now +
                             new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay * (1f - DelayTime / 100.0f)));
            }

            if (Character.Status.aspd_skill_perc >= 1f)
                skillDelay = DateTime.Now +
                             new TimeSpan(0, 0, 0, 0, (int)(skill.skill.Delay / Character.Status.aspd_skill_perc));

            //独立cd
            if (!SingleCDLst.ContainsKey(skill.skill.ID))
                SingleCDLst.Add(skill.skill.ID, DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.SinglgCD));
            else
                SingleCDLst[skill.skill.ID] = DateTime.Now + new TimeSpan(0, 0, 0, 0, skill.skill.SinglgCD);
            //if (!this.Character.Status.Additions.ContainsKey("DelayOut"))
            //{
            //    

            //}

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, Character, true);

            if (skill.skill.Effect != 0 &&
                (skill.skill.Target == 4 || (skill.skill.Target == 2 && skill.sActor == skill.dActor)))
            {
                var eff = new EffectArg();
                eff.actorID = skill.dActor;
                eff.effectID = skill.skill.Effect;
                eff.x = skill.x;
                eff.y = skill.y;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, eff, Character, true);
            }

            if (Character.Tasks.ContainsKey("AutoCast"))
            {
                Character.Tasks["AutoCast"].Activate();
            }
            else
            {
                if (skill.autoCast.Count != 0)
                {
                    Character.Buff.CannotMove = true;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                    var task = new AutoCast(Character, skill);
                    Character.Tasks.Add("AutoCast", task);
                    task.Activate();
                }
            }
        }

        public void SendChangeStatus()
        {
            if (Character.Tasks.ContainsKey("Regeneration"))
            {
                Character.Tasks["Regeneration"].Deactivate();
                Character.Tasks.Remove("Regeneration");
            }

            if (Character.Motion != MotionType.NONE && Character.Motion != MotionType.DEAD)
            {
                Character.Motion = MotionType.NONE;
                Character.MotionLoop = false;
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, Character, true);
        }

        public void SendRevive(byte level)
        {
            Character.Buff.Dead = false;
            Character.Buff.TurningPurple = false;
            Character.Motion = MotionType.STAND;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);

            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 0.1f;
                    break;
                case 2:
                    factor = 0.2f;
                    break;
                case 3:
                    factor = 0.45f;
                    break;
                case 4:
                    factor = 0.5f;
                    break;
                case 5:
                    factor = 0.75f;
                    break;
                case 6:
                    factor = 1f;
                    break;
            }

            Character.HP = (uint)(Character.MaxHP * factor);
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, Character, true);
            var arg = new SkillArg();
            arg.sActor = Character.ActorID;
            arg.dActor = 0;
            arg.skill = SkillFactory.Instance.GetSkill(10002, level);
            arg.x = 0;
            arg.y = 0;
            arg.hp = new List<int>();
            arg.sp = new List<int>();
            arg.mp = new List<int>();
            arg.hp.Add((int)(-Character.MaxHP * factor));
            arg.sp.Add(0);
            arg.mp.Add(0);
            arg.flag.Add(AttackFlag.HP_HEAL);
            arg.affectedActors.Add(Character);
            arg.argType = SkillArg.ArgType.Active;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);

            if (!Character.Tasks.ContainsKey("AutoSave"))
            {
                var task = new AutoSave(Character);
                Character.Tasks.Add("AutoSave", task);
                task.Activate();
            }

            if (!Character.Tasks.ContainsKey("Recover")) //自然恢复
            {
                var reg = new Recover(FromActorPC(Character));
                Character.Tasks.Add("Recover", reg);
                reg.Activate();
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();
        }

        public void SendSkillList()
        {
            var p = new SSMG_SKILL_LIST();
            Dictionary<uint, byte> skills;
            Dictionary<uint, byte> skills2X;
            Dictionary<uint, byte> skills2T;
            Dictionary<uint, byte> skills3;
            var list = new List<SagaDB.Skill.Skill>();
            var ifDominion = map.Info.Flag.Test(MapFlags.Dominion);
            if (ifDominion)
            {
                skills = new Dictionary<uint, byte>();
                skills2X = new Dictionary<uint, byte>();
                skills2T = new Dictionary<uint, byte>();
                skills3 = new Dictionary<uint, byte>();
            }
            else
            {
                skills = SkillFactory.Instance.SkillList(Character.JobBasic);
                skills2X = SkillFactory.Instance.SkillList(Character.Job2X);
                skills2T = SkillFactory.Instance.SkillList(Character.Job2T);
                skills3 = SkillFactory.Instance.SkillList(Character.Job3);
            }

            {
                var skill =
                    from c in skills.Keys
                    where !Character.Skills.ContainsKey(c)
                    select c;
                foreach (var i in skill)
                {
                    var sk = SkillFactory.Instance.GetSkill(i, 0);
                    list.Add(sk);
                }

                foreach (var i in Character.Skills.Values) list.Add(i);
            }
            p.Skills(list, 0, Character.JobBasic, ifDominion, Character);
            NetIo.SendPacket(p);
            if (Character.Rebirth || Character.Job == Character.Job3)
            {
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2X.Keys
                            where !Character.Skills2_1.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2_1.Values) list.Add(i);
                    }

                    p.Skills(list, 1, Character.Job2X, ifDominion, Character);
                    NetIo.SendPacket(p);
                }
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2T.Keys
                            where !Character.Skills2_2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2_2.Values) list.Add(i);
                    }
                    p.Skills(list, 2, Character.Job2T, ifDominion, Character);
                    NetIo.SendPacket(p);
                }
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills3.Keys
                            where !Character.Skills3.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills3.Values) list.Add(i);
                    }

                    p.Skills(list, 3, Character.Job3, ifDominion, Character);
                    NetIo.SendPacket(p);
                }
            }
            else
            {
                if (Character.Job == Character.Job2X)
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2X.Keys
                            where !Character.Skills2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2.Values) list.Add(i);
                    }

                    p.Skills(list, 1, Character.Job2X, ifDominion, Character);
                    NetIo.SendPacket(p);
                }

                if (Character.Job == Character.Job2T)
                {
                    list.Clear();
                    {
                        var skill =
                            from c in skills2T.Keys
                            where !Character.Skills2.ContainsKey(c)
                            select c;
                        foreach (var i in skill)
                        {
                            var sk = SkillFactory.Instance.GetSkill(i, 0);
                            list.Add(sk);
                        }

                        foreach (var i in Character.Skills2.Values) list.Add(i);
                    }
                    p.Skills(list, 2, Character.Job2T, ifDominion, Character);
                    NetIo.SendPacket(p);
                }

                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    if (Character.DominionReserveSkill)
                    {
                        var p2 = new SSMG_SKILL_RESERVE_LIST();
                        p2.Skills = Character.SkillsReserve.Values.ToList();
                        NetIo.SendPacket(p2);
                    }
                    else
                    {
                        var p2 = new SSMG_SKILL_RESERVE_LIST();
                        p2.Skills = new List<SagaDB.Skill.Skill>();
                        NetIo.SendPacket(p2);
                    }
                }
                else
                {
                    var p2 = new SSMG_SKILL_RESERVE_LIST();
                    p2.Skills = Character.SkillsReserve.Values.ToList();
                    NetIo.SendPacket(p2);
                }
            }


            if (Character.JobJoint != PC_JOB.NONE)
            {
                list.Clear();
                {
                    var skill =
                        from c in SkillFactory.Instance.SkillList(Character.JobJoint)
                        where c.Value <= Character.JointJobLevel
                        select c;
                    foreach (var i in skill)
                    {
                        var sk = SkillFactory.Instance.GetSkill(i.Key, 1);
                        list.Add(sk);
                    }
                }
                var p2 = new SSMG_SKILL_JOINT_LIST();
                p2.Skills = list;
                NetIo.SendPacket(p2);
            }
            else
            {
                var p2 = new SSMG_SKILL_JOINT_LIST();
                p2.Skills = new List<SagaDB.Skill.Skill>();
                NetIo.SendPacket(p2);
            }
        }

        public void SendSkillDummy()
        {
            SendSkillDummy(3311, 1);
        }

        public void SendSkillDummy(uint skillid, byte level)
        {
            if (Character.Tasks.ContainsKey("SkillCast"))
            {
                if (Character.Tasks["SkillCast"].getActivated())
                {
                    Character.Tasks["SkillCast"].Deactivate();
                    Character.Tasks.Remove("SkillCast");
                }

                var arg = new SkillArg();
                arg.sActor = Character.ActorID;
                arg.dActor = 0;
                arg.skill = SkillFactory.Instance.GetSkill(skillid, level);
                arg.x = 0;
                arg.y = 0;
                arg.hp = new List<int>();
                arg.sp = new List<int>();
                arg.mp = new List<int>();
                arg.hp.Add(0);
                arg.sp.Add(0);
                arg.mp.Add(0);
                arg.flag.Add(AttackFlag.NONE);
                //arg.affectedActors.Add(this.Character);
                arg.argType = SkillArg.ArgType.Active;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);
            }
        }

        public void OnSkillRangeAttack(CSMG_SKILL_RANGE_ATTACK p)
        {
            var p2 = new SSMG_SKILL_RANGEA_RESULT();
            p2.ActorID = p.ActorID;
            if (!Character.Status.Additions.ContainsKey("自由射击"))
                p2.Speed = 410;
            else
                p2.Speed = 0;
            NetIo.SendPacket(p2);
            Character.TTime["远程蓄力"] = DateTime.Now;

            if (Character.Tasks.ContainsKey("RangeAttack"))
            {
                Character.Tasks["RangeAttack"].Deactivate();
                Character.Tasks.Remove("RangeAttack");
            }

            var ra = new RangeAttack(this);
            Character.Tasks.Add("RangeAttack", ra);
            ra.Activate();
        }

        /// <summary>
        ///     重置技能
        /// </summary>
        /// <param name="job">1为1转，2为2转</param>
        public void ResetSkill(byte job)
        {
            var totalPoints = 0;
            var delList = new List<uint>();
            switch (job)
            {
                case 1:
                    foreach (var i in Character.Skills.Values)
                        if (SkillFactory.Instance.SkillList(Character.JobBasic).ContainsKey(i.ID))
                        {
                            totalPoints += i.Level + 2;
                            delList.Add(i.ID);
                        }

                    Character.SkillPoint += (ushort)totalPoints;
                    foreach (var i in delList) Character.Skills.Remove(i);
                    break;
                case 2:
                    if (!Character.Rebirth)
                    {
                        foreach (var i in Character.Skills2.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList) Character.Skills2.Remove(i);
                        if (Character.Job == Character.Job2X)
                            Character.SkillPoint2X += (ushort)totalPoints;
                        if (Character.Job == Character.Job2T)
                            Character.SkillPoint2T += (ushort)totalPoints;
                    }
                    else
                    {
                        Character.SkillPoint2X = 0;
                        foreach (var i in Character.Skills2_1.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job2X).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList)
                        {
                            Character.Skills2_1.Remove(i);
                            Character.Skills2.Remove(i);
                        }

                        Character.SkillPoint2X += (ushort)totalPoints;

                        totalPoints = 0;
                        delList.Clear();
                        Character.SkillPoint2T = 0;
                        foreach (var i in Character.Skills2_2.Values)
                            if (SkillFactory.Instance.SkillList(Character.Job2T).ContainsKey(i.ID))
                            {
                                totalPoints += i.Level + 2;
                                delList.Add(i.ID);
                            }

                        foreach (var i in delList)
                        {
                            Character.Skills2_2.Remove(i);
                            Character.Skills2.Remove(i);
                        }

                        Character.SkillPoint2T += (ushort)totalPoints;
                    }

                    break;
                case 3:
                    foreach (var i in Character.Skills3.Values)
                        if (SkillFactory.Instance.SkillList(Character.Job3).ContainsKey(i.ID))
                        {
                            totalPoints += i.Level + 2;
                            delList.Add(i.ID);
                        }

                    Character.SkillPoint3 += (ushort)totalPoints;
                    foreach (var i in delList) Character.Skills3.Remove(i);
                    break;
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
        }

        public void OnFishBaitsEquip(CSMG_FF_FISHBAIT_EQUIP p)
        {
            if (p.InventorySlot == 0)
            {
                Character.EquipedBaitID = 0;

                var p2 = new SSMG_FISHBAIT_EQUIP_RESULT();
                p2.InventoryID = 0;
                p2.IsEquip = 1;
                NetIo.SendPacket(p2);
            }
            else
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item.ItemID >= 10104900 || item.ItemID <= 10104906)
                {
                    Character.EquipedBaitID = item.ItemID;

                    var p2 = new SSMG_FISHBAIT_EQUIP_RESULT();
                    p2.InventoryID = p.InventorySlot;
                    p2.IsEquip = 0;
                    NetIo.SendPacket(p2);
                }
            }
        }

        public void SendWarpPoints()
        {
        }

        public void ProcessWarp(CSMG_INFINITECORRIDOR_WARP p)
        {
        }

        public void ProcessTrap(CSMG_INFINITECORRIDOR_TRAP p)
        {
        }

        public void OnNewMosterDiscover(uint mobID)
        {
            if (MobFactory.Instance.Mobs.ContainsKey(mobID))
            {
                var mob = MobFactory.Instance.Mobs[mobID];
                if (mob.guideFlag == 1)
                {
                    var p = new SSMG_MOSTERGUIDE_NEW_RECORD();
                    p.guideID = mob.guideID;
                    NetIo.SendPacket(p);
                }
            }
        }

        public void SendMosterGuide()
        {
            var MobList =
                (from m in MobFactory.Instance.Mobs.Values where m.guideFlag == 3 orderby m.guideID select m)
                .ToDictionary(m => m.guideID, m => m.id);

            //switch m.guideFlag to 1 when enabled
            var boolstates = new bool[MobList.Keys.Max()];

            for (short i = 0; i < boolstates.Length; i++)
            {
                var state = false;
                if (MobList.ContainsKey(i))
                    if (Character.MosterGuide.ContainsKey(MobList[i]))
                        state = Character.MosterGuide[MobList[i]];
                boolstates[i] = state;
            }

            var masks = new List<BitMask>();
            byte index = 0;
            var BitmaskSize = 32;
            var skip = BitmaskSize * index;
            while (skip < boolstates.Length)
            {
                var items = boolstates.Select(x => x).Skip(skip).Take(BitmaskSize).ToArray();
                masks.Add(new BitMask(items));
                index++;
                skip = BitmaskSize * index;
            }

            var p = new SSMG_MOSTERGUIDE_RECORDS();
            p.Records = masks;
            NetIo.SendPacket(p);
        }

        public void MarionetteActivate(uint marionetteID)
        {
            MarionetteActivate(marionetteID, true, true);
        }

        public void MarionetteActivate(uint marionetteID, bool delay, bool duration)
        {
            var marionette = MarionetteFactory.Instance[marionetteID];
            if (marionette != null)
            {
                var task = new Marionette(this, marionette.Duration);
                if (Character.Tasks.ContainsKey("Marionette") && duration)
                {
                    MarionetteDeactivate();
                    Character.Tasks["Marionette"].Deactivate();
                    Character.Tasks.Remove("Marionette");
                }

                if (!duration && Character.Marionette != null)
                {
                    foreach (uint i in Character.Marionette.skills)
                    {
                        var skill = SkillFactory.Instance.GetSkill(i, 1);
                        if (skill != null)
                            if (Character.Skills.ContainsKey(i))
                                Character.Skills.Remove(i);
                    }

                    SkillHandler.Instance.CastPassiveSkills(Character);
                }

                if (!Character.Tasks.ContainsKey("Marionette"))
                {
                    Character.Tasks.Add("Marionette", task);
                    task.Activate();
                }

                if (delay)
                {
                    if (!Character.Status.Additions.ContainsKey("MarioTimeUp"))
                        Character.NextMarionetteTime = DateTime.Now + new TimeSpan(0, 0, marionette.Delay);
                    else
                        Character.NextMarionetteTime =
                            DateTime.Now + new TimeSpan(0, 0, (int)(marionette.Delay * 0.6f));
                }

                Character.Marionette = marionette;
                SendCharInfoUpdate();
                foreach (uint i in marionette.skills)
                {
                    var skill = SkillFactory.Instance.GetSkill(i, 1);
                    if (skill != null)
                        if (!Character.Skills.ContainsKey(i))
                        {
                            skill.NoSave = true;
                            Character.Skills.Add(i, skill);
                            if (!skill.BaseData.active)
                            {
                                var arg = new SkillArg();
                                arg.skill = skill;
                                SkillHandler.Instance.SkillCast(Character, Character, arg);
                            }
                        }
                }

                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
            }
        }

        public void MarionetteDeactivate()
        {
            MarionetteDeactivate(false);
        }

        public void MarionetteDeactivate(bool disconnecting)
        {
            if (Character.Marionette == null)
                return;
            var marionette = Character.Marionette;
            Character.Marionette = null;
            if (!disconnecting) SendCharInfoUpdate();
            foreach (uint i in marionette.skills)
            {
                var skill = SkillFactory.Instance.GetSkill(i, 1);
                if (skill != null)
                    if (Character.Skills.ContainsKey(i))
                        Character.Skills.Remove(i);
            }

            SkillHandler.Instance.CastPassiveSkills(Character);
            StatusFactory.Instance.CalcStatus(Character);
            if (!disconnecting)
            {
                SendPlayerInfo();
                SendMotion(MotionType.JOY, 0);
            }
        }

        public WarehousePlace currentWarehouse = WarehousePlace.Current;

        public void OnItemWarePage(CSMG_ITEM_WARE_PAGE p)
        {
            var page = p.PageID;
            var place = (WarehousePlace)page;
            currentWarehouse = place;
            SendWareItems(place);
        }

        public void OnItemWareClose(CSMG_ITEM_WARE_CLOSE p)
        {
            currentWarehouse = WarehousePlace.Current;
        }

        public void OnItemWareGet(CSMG_ITEM_WARE_GET p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = 1;
            }
            else
            {
                var item = Character.Inventory.GetItem(currentWarehouse, p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else
                {
                    Item newItem;
                    switch (Character.Inventory.DeleteWareItem(currentWarehouse, item.Slot, p.Count))
                    {
                        case InventoryDeleteResult.ALL_DELETED:
                            var p1 = new SSMG_ITEM_DELETE();
                            p1.InventorySlot = item.Slot;
                            NetIo.SendPacket(p1);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.STACK_UPDATED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            NetIo.SendPacket(p2);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.ERROR:
                            result = -99;
                            break;
                    }
                }
            }

            var p5 = new SSMG_ITEM_WARE_GET_RESULT();
            p5.Result = result;
            NetIo.SendPacket(p5);
        }

        public void OnItemWarePut(CSMG_ITEM_WARE_PUT p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = -1;
            }
            else
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else if (Character.Inventory.WareTotalCount >= Configuration.Configuration.Instance.WarehouseLimit)
                {
                    result = -4;
                }
                else
                {
                    Logger.LogItemLost(Logger.EventType.ItemWareLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("WarePut Count:{0}", p.Count), false);
                    DeleteItem(p.InventoryID, p.Count, false);
                    var newItem = item.Clone();
                    newItem.Stack = p.Count;
                    switch (Character.Inventory.AddWareItem(currentWarehouse, newItem))
                    {
                        case InventoryAddResult.NEW_INDEX:
                            var p1 = new SSMG_ITEM_WARE_ITEM();
                            p1.Place = WarehousePlace.Current;
                            p1.InventorySlot = newItem.Slot;
                            p1.Item = newItem;
                            NetIo.SendPacket(p1);
                            break;
                        case InventoryAddResult.STACKED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            NetIo.SendPacket(p2);
                            break;
                        case InventoryAddResult.MIXED:
                            var p3 = new SSMG_ITEM_COUNT_UPDATE();
                            p3.InventorySlot = item.Slot;
                            p3.Stack = item.Stack;
                            NetIo.SendPacket(p3);
                            var p4 = new SSMG_ITEM_WARE_ITEM();
                            p4.InventorySlot = Character.Inventory.LastItem.Slot;
                            p4.Item = Character.Inventory.LastItem;
                            p4.Place = WarehousePlace.Current;
                            NetIo.SendPacket(p4);
                            break;
                    }

                    SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_PUT, item.BaseData.name,
                        p.Count));
                }
            }

            var p5 = new SSMG_ITEM_WARE_PUT_RESULT();
            p5.Result = result;
            NetIo.SendPacket(p5);
        }

        public void SendWareItems(WarehousePlace place)
        {
            var p = new SSMG_ITEM_WARE_HEADER();
            p.Place = place;
            p.CountCurrent = Character.Inventory.WareHouse[place].Count;
            p.CountAll = Configuration.Configuration.Instance.WarehouseLimit;
            p.CountMax = Configuration.Configuration.Instance.WarehouseLimit;
            //p.Gold = this.Character.Account.Bank;
            NetIo.SendPacket(p);

            foreach (var i in Character.Inventory.WareHouse.Keys)
            {
                if (i == WarehousePlace.Current)
                    continue;
                if (i != place) continue;
                foreach (var j in Character.Inventory.WareHouse[i])
                {
                    //if (j.Refine == 0)
                    //    j.Clear();

                    var p1 = new SSMG_ITEM_WARE_ITEM();
                    p1.Item = j;
                    p1.InventorySlot = j.Slot;
                    if (i == place)
                        p1.Place = WarehousePlace.Current;
                    else
                        p1.Place = i;
                    NetIo.SendPacket(p1);
                }
            }

            var p2 = new SSMG_ITEM_WARE_FOOTER();
            NetIo.SendPacket(p2);
        }

        public void OnFGardenWareClose(CSMG_FG_WARE_CLOSE p)
        {
            currentWarehouse = WarehousePlace.Current;
        }

        public void OnFGardenWareGet(CSMG_FG_WARE_GET p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = 1;
            }
            else
            {
                var item = Character.Inventory.GetItem(currentWarehouse, p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else
                {
                    Item newItem;
                    switch (Character.Inventory.DeleteWareItem(currentWarehouse, item.Slot, p.Count))
                    {
                        case InventoryDeleteResult.ALL_DELETED:
                            var p1 = new SSMG_ITEM_DELETE();
                            p1.InventorySlot = item.Slot;
                            NetIo.SendPacket(p1);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.STACK_UPDATED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            NetIo.SendPacket(p2);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.ERROR:
                            result = -99;
                            break;
                    }
                }
            }

            var p5 = new SSMG_ITEM_WARE_GET_RESULT();
            p5.Result = result;
            NetIo.SendPacket(p5);
        }

        public void OnFGardenWarePut(CSMG_FG_WARE_PUT p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = -1;
            }
            else
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else if (Character.Inventory.WareTotalCount >= Configuration.Configuration.Instance.WarehouseLimit)
                {
                    result = -4;
                }
                else
                {
                    Logger.LogItemLost(Logger.EventType.ItemWareLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")", string.Format("WarePut Count:{0}", p.Count),
                        false);
                    DeleteItem(p.InventoryID, p.Count, false);
                    var newItem = item.Clone();
                    newItem.Stack = p.Count;
                    switch (Character.Inventory.AddWareItem(currentWarehouse, newItem))
                    {
                        case InventoryAddResult.NEW_INDEX:
                            var p1 = new SSMG_FG_WARE_ITEM();
                            p1.InventorySlot = newItem.Slot;
                            p1.Item = newItem;
                            NetIo.SendPacket(p1);
                            break;
                        case InventoryAddResult.STACKED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            NetIo.SendPacket(p2);
                            break;
                        case InventoryAddResult.MIXED:
                            var p3 = new SSMG_ITEM_COUNT_UPDATE();
                            p3.InventorySlot = item.Slot;
                            p3.Stack = item.Stack;
                            NetIo.SendPacket(p3);
                            var p4 = new SSMG_FG_WARE_ITEM();
                            p4.InventorySlot = Character.Inventory.LastItem.Slot;
                            p4.Item = Character.Inventory.LastItem;
                            NetIo.SendPacket(p4);
                            break;
                    }

                    SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_PUT, item.BaseData.name,
                        p.Count));
                }
            }

            var p5 = new SSMG_ITEM_WARE_PUT_RESULT();
            p5.Result = result;
            NetIo.SendPacket(p5);
        }

        public void SendFGardenWareItems()
        {
            currentWarehouse = WarehousePlace.FGarden;
            var p0 = new SSMG_FG_WARE_SENDCOUNT();
            Character.Inventory.WareHouse[currentWarehouse] = new List<Item>(300);
            p0.CurrentCount = Character.Inventory.WareHouse[currentWarehouse].Capacity;
            NetIo.SendPacket(p0);

            var p1 = new SSMG_FG_WARE_HEADER();
            NetIo.SendPacket(p1);

            foreach (var j in Character.Inventory.WareHouse[currentWarehouse])
            {
                //if (j.Refine == 0)
                //    j.Clear();

                var p2 = new SSMG_FG_WARE_ITEM();
                p2.Item = j;
                p2.InventorySlot = j.Slot;
                //if (i == place)
                //    p2.Place = WarehousePlace.Current;
                //else
                //    p2.Place = i;
                NetIo.SendPacket(p2);
            }

            var p3 = new SSMG_FG_WARE_FOOTER();
            NetIo.SendPacket(p3);
        }

        private ActorPC teamPartner;

        public void OnAbyssTeamListRequest(CSMG_ABYSSTEAM_LIST_REQUEST p)
        {
        }

        public void OnAbyssTeamListClose(CSMG_ABYSSTEAM_LIST_CLOSE p)
        {
        }

        public void OnAbyssTeamBreakRequest(CSMG_ABYSSTEAM_BREAK_REQUEST p)
        {
            var team = Character.Team;
            var members = new List<ActorPC>();
            var teamName = "";
            if (team != null)
            {
                teamName = team.Name;
                members = team.Members.Values.ToList();
            }

            var Result = unchecked((byte)CheckAbyssTeamBreakRequest(team));

            if (Result == 0)
            {
                var p1 = new SSMG_ABYSSTEAM_BREAK();
                p1.TeamName = teamName;
                MapClient member;
                for (var i = 0; i < members.Count; i++)
                    if (members[i].Online)
                    {
                        member = FromActorPC(members[i]);
                        if (member != null) member.NetIo.SendPacket(p1);
                    }
            }
            else
            {
                var p1 = new SSMG_ABYSSTEAM_BREAK();
                p1.Result = Result;
                NetIo.SendPacket(p1);
            }
        }

        private int CheckAbyssTeamBreakRequest(Team team)
        {
            if (team == null)
                return -3; //既にチームが解散されています
            try
            {
                AbyssTeamManager.Instance.TeamDismiss(team);
            }
            catch
            {
            }

            return 0; //チーム「%s」を解散しました
        }

        public void OnAbyssTeamLeaveRequest(CSMG_ABYSSTEAM_LEAVE_REQUEST p)
        {
            var team = Character.Team;
            var teamName = "";
            if (team != null)
                teamName = team.Name;

            _ = unchecked((byte)CheckAbyssTeamLeaveRequest(team));
            var p1 = new SSMG_ABYSSTEAM_LEAVE();
            NetIo.SendPacket(p1);
        }

        private int CheckAbyssTeamLeaveRequest(Team team)
        {
            if (team == null)
                return -3; //既にチームが解散されています
            try
            {
                AbyssTeamManager.Instance.DeleteMember(team, Character.CharID);
            }
            catch
            {
            }

            return 0; //チーム「%s」を脱退しました
        }

        public void OnAbyssTeamRegistRequest(CSMG_ABYSSTEAM_REGIST_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamRegistRequest(p.LeaderID, p.Password));
            if (Result == 2)
            {
                var target = MapClientManager.Instance.FindClient(p.LeaderID);
                teamPartner = target.Character;
                var p1 = new SSMG_ABYSSTEAM_REGIST_APPROVAL();
                p1.CharID = Character.CharID;
                p1.Name = Character.Name;
                p1.Level = Character.Level;
                p1.Job = Character.Job;
                target.NetIo.SendPacket(p1);
            }

            var p2 = new SSMG_ABYSSTEAM_REGIST_APPLY();
            p2.Result = Result;
            NetIo.SendPacket(p2);
        }

        private int CheckAbyssTeamRegistRequest(uint leaderID, string pass)
        {
            var team = AbyssTeamManager.Instance.GetTeam(leaderID);
            if (teamPartner != null)
                return -9; //参加申し込みの上限に達しているため、申請を行えませんでした
            if (Character.Team != null)
                return -8; //既にチームに参加しています
            if (team == null)
                return -7; //チームが解散されているため入れませんでした
            if (!team.Leader.Online)
                return -6; //リーダーがいないため入れませんでした
            if (!team.JobRequirements.Contains(Character.Job))
                return -4; //条件に合わなかったため入れませんでした
            if (pass != team.Pass)
                return -3; //パスワードが違います
            if (team.Members.Count >= team.MaxMember)
                return -2; //チームが満員のため入れませんでした
            var leader = MapClientManager.Instance.FindClient(leaderID);
            if (leader.teamPartner != null)
                return -10; //申請許可を待っているユーザーは多数居たため、加入することは出来ませんでした
            return 2; //加入申請中です
        }

        public void OnAbyssTeamRegistApproval(CSMG_ABYSSTEAM_REGIST_APPROVAL p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var p1 = new SSMG_ABYSSTEAM_REGIST_APPLY();
            var approved = false;
            if (p.Result == 0)
                approved = true;
            var Result = unchecked((byte)CheckAbyssTeamRegistApproval(approved));
            p1.Result = Result;
            if (Result == 1)
                p1.TeamName = Character.Team.Name;
            client.NetIo.SendPacket(p1);
        }

        private int CheckAbyssTeamRegistApproval(bool approved)
        {
            if (!approved)
                return -5; //参加できませんでした
            var team = Character.Team;
            try
            {
                AbyssTeamManager.Instance.AddMember(team, teamPartner);
                return 1; //%sに参加しました
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }
            //何らかの原因で失敗しました
            //参加申請を出したユーザーがロビー内に居なかったため、参加申請はキャンセルされました
            //既に他のチームに加入しているため、加入することが出来ませんでした。
        }

        public void OnAbyssTeamSetCreateRequest(CSMG_ABYSSTEAM_SET_CREATE_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamSetCreate(p));
            var p1 = new SSMG_ABYSSTEAM_SET_CREATE();
            p1.Result = Result;
            NetIo.SendPacket(p1);
        }

        private int CheckAbyssTeamSetCreate(CSMG_ABYSSTEAM_SET_CREATE_REQUEST p)
        {
            if (!AbyssTeamManager.Instance.CheckRegistLimit())
                return -2; //チーム登録数の上限に達しているため、チームを登録することが出来ませんでした
            try
            {
                var job = new List<PC_JOB>();
                AbyssTeamManager.Instance.CreateTeam(Character, p.TeamName, p.Comment, p.Password, p.IsFromSave,
                    p.MinLV, p.MaxLV, job);
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //チームを登録しました
        }

        public void OnAbyssTeamSetOpenRequest(CSMG_ABYSSTEAM_SET_OPEN_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamSetOpen());
            var p1 = new SSMG_ABYSSTEAM_SET_OPEN();
            p1.Result = Result;
            if (Result == 0)
                p1.Floor = 100;
            NetIo.SendPacket(p1);
        }

        private int CheckAbyssTeamSetOpen()
        {
            if (Character.Team != null)
            {
                if (Character.Team.Leader == Character)
                    return -2; //既にチームを登録しているため、チーム登録が行えませんでした
                return -3; //既に他のチームに加入しているため、チーム登録が行えませんでした
            }

            if (!AbyssTeamManager.Instance.CheckRegistLimit())
                return -4; //チーム登録数の上限に達しているため、チームを登録することが出来ませんでした
            if (Character.AbyssFloor == 0)
                return -1; //何らかの原因で失敗しました
            return 0;
        }

        public bool itemexchange;

        public void OnItemExchangeConfirm(CSMG_ITEM_EXCHANGE_CONFIRM p)
        {
            var inventoryid = p.InventorySlot;
            var exchangetargetid = p.ExchangeTargetID;

            var item = Character.Inventory.GetItem(inventoryid);
            if (item == null || item.ItemID == 10000000)
            {
                var p2 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                NetIo.SendPacket(p2);
                SendCapacity();
                return;
            }

            if (!ItemExchangeListFactory.Instance.ExchangeList.ContainsKey(item.ItemID))
            {
                var p3 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                NetIo.SendPacket(p3);
                SendCapacity();
                return;
            }

            var oriitem = ItemExchangeListFactory.Instance.ExchangeList[item.ItemID].OriItemID;

            var canexchangelist = ExchangeFactory.Instance.ExchangeItems[oriitem];

            if (!canexchangelist.ItemsID.Contains(exchangetargetid) && canexchangelist.OriItemID != exchangetargetid)
            {
                var p4 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                NetIo.SendPacket(p4);
                SendCapacity();
                return;
            }

            var targetitem = ItemFactory.Instance.GetItem(exchangetargetid, true);

            DeleteItem(inventoryid, 1, true);
            AddItem(targetitem, true);
            //Logger.ShowInfo("Receive Item Exchange Request. Type:" + exchangetype + ", itemslot:" + inventoryid + ", targetid:" + exchangetargetid);

            var p1 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
            NetIo.SendPacket(p1);
            SendCapacity();
        }

        public void OnItemExchangeWindowClose(CSMG_ITEM_EXCHANGE_CLOSE p)
        {
            itemexchange = false;
        }

        public ActorPC ringPartner;

        public void OnRingEmblemUpload(CSMG_RING_EMBLEM_UPLOAD p)
        {
            var p1 = new SSMG_RING_EMBLEM_UPLOAD_RESULT();
            if (Character.Ring == null)
                return;

            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.RingMaster) ||
                Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.Ring2ndMaster))
            {
                var data = p.Data;
                if (data[0] == 0x89)
                {
                    if (Character.Ring.Fame >= Configuration.Configuration.Instance.RingFameNeededForEmblem)
                    {
                        p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.OK;
                        MapServer.charDB.RingEmblemUpdate(Character.Ring, p.Data);
                    }
                    else
                    {
                        p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.FAME_NOT_ENOUGH;
                    }
                }
                else
                {
                    p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.WRONG_FORMAT;
                }
            }

            NetIo.SendPacket(p1);
        }

        public void OnChatRing(CSMG_CHAT_RING p)
        {
            if (Character.Ring == null)
                return;
            RingManager.Instance.RingChat(Character.Ring, Character, p.Content);

            //TODO:ECOE用
            //Logger.ShowChat("[R]" + this.Character.Name + " :" + p.Content, null);
        }

        public void OnRingRightSet(CSMG_RING_RIGHT_SET p)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.RingMaster) ||
                Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.Ring2ndMaster))
                RingManager.Instance.SetMemberRight(Character.Ring, p.CharID, p.Right);
        }

        public void OnRingKick(CSMG_RING_KICK p)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.KickRight))
                RingManager.Instance.DeleteMember(Character.Ring, Character.Ring.GetMember(p.CharID),
                    SSMG_RING_QUIT.Reasons.KICK);
        }

        public void OnRingQuit(CSMG_RING_QUIT p)
        {
            var p1 = new SSMG_RING_QUIT_RESULT();
            if (Character.Ring == null)
            {
                p1.Result = -1;
            }
            else
            {
                if (Character != Character.Ring.Leader)
                    RingManager.Instance.DeleteMember(Character.Ring, Character, SSMG_RING_QUIT.Reasons.LEAVE);
                else
                    RingManager.Instance.RingDismiss(Character.Ring);
            }

            NetIo.SendPacket(p1);
        }

        public void OnRingInviteAnswer(CSMG_RING_INVITE_ANSWER p, bool accepted)
        {
            if (accepted)
            {
                var p1 = new SSMG_RING_INVITE_ANSWER_RESULT();
                var result = CheckRingInviteAnswer();
                p1.Result = (SSMG_RING_INVITE_ANSWER_RESULT.RESULTS)result;
                if (result >= 0)
                    RingManager.Instance.AddMember(ringPartner.Ring, Character);
                NetIo.SendPacket(p1);
            }

            ringPartner = null;
        }

        private int CheckRingInviteAnswer()
        {
            if (ringPartner == null)
                return -2;
            if (Character.Ring != null)
                return -11;
            if (ringPartner.Ring.MemberCount >= ringPartner.Ring.MaxMemberCount)
                return -12;
            var index = ringPartner.Ring.IndexOf(ringPartner);
            if (!ringPartner.Ring.Rights[index].Test(RingRight.AddRight))
                return -14;
            return 0;
        }

        public void OnRingInvite(CSMG_RING_INVITE p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var p1 = new SSMG_RING_INVITE_RESULT();
            Character.Ring.IndexOf(Character);
            var result = CheckRingInvite(client);
            p1.Result = result;
            if (result == 0)
            {
                client.ringPartner = Character;
                var p2 = new SSMG_RING_INVITE();
                p2.CharID = Character.CharID;
                p2.CharName = Character.Name;
                p2.RingName = Character.Ring.Name;
                client.NetIo.SendPacket(p2);
            }

            NetIo.SendPacket(p1);
        }

        private int CheckRingInvite(MapClient client)
        {
            if (client == null)
                return -1; //相手が見つかりません
            if (!client.Character.canRing)
                return -3; //相手がリング招待不許可設定です
            if (client.Character.Ring == null)
                return -4; //相手はリングに加入済みです
            if (Character.Ring != null)
                return -5; //リングを組んでいないので誘えません 
            var index = Character.Ring.IndexOf(Character);
            if (!Character.Ring.Rights[index].Test(RingRight.AddRight))
                return -6; //招待権限を持っていません
            if (Character.Ring.IndexOf(client.Character) >= 0)
                return -9; //既にリングに入っています
            if (Character.Ring.MemberCount >= Character.Ring.MaxMemberCount)
                return -10; //誘った相手に招待権限がありません
            return 0;
        }

        public void SendRingMember()
        {
            if (Character.Ring == null)
                return;
            foreach (var i in Character.Ring.Members.Values)
            {
                var p = new SSMG_RING_MEMBER_INFO();
                p.Member(i, Character.Ring);
                NetIo.SendPacket(p);
                SendRingMemberInfo(i);
            }
        }

        public void SendRingInfo(SSMG_RING_INFO.Reason reason)
        {
            if (Character.PlayerTitleID != 0)
            {
                var p1 = new SSMG_RING_NAME();
                p1.Player = Character;
                NetIo.SendPacket(p1);
            }

            if (Character.Ring == null)
                return;
            if (reason != SSMG_RING_INFO.Reason.UPDATED)
            {
                var p = new SSMG_RING_INFO();
                var p1 = new SSMG_RING_NAME();
                p.Ring(Character.Ring, reason);
                p1.Player = Character;
                NetIo.SendPacket(p);
                NetIo.SendPacket(p1);
                SendRingMember();
            }
            else
            {
                var p = new SSMG_RING_INFO_UPDATE();
                p.RingID = Character.Ring.ID;
                p.Fame = Character.Ring.Fame;
                p.CurrentMember = (byte)Character.Ring.MemberCount;
                p.MaxMember = (byte)Character.Ring.MaxMemberCount;
                NetIo.SendPacket(p);
            }
        }

        public void SendRingMemberInfo(ActorPC pc)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.IsMember(pc))
                if (pc.Online)
                {
                    var i = (uint)Character.Ring.IndexOf(pc);
                    var p = new SSMG_PARTY_MEMBER_STATE();
                    p.PartyIndex = i;
                    p.CharID = pc.CharID;
                    p.Online = pc.Online;
                    NetIo.SendPacket(p);
                    var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                    p2.PartyIndex = i;
                    p2.CharID = pc.CharID;
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                    p2.Job = pc.Job;
                    p2.Level = pc.Level;
                    p2.JobLevel = pc.CurrentJobLevel;
                    NetIo.SendPacket(p2);
                }
        }

        public void SendRingMemberState(ActorPC pc)
        {
            if (Character.Ring == null) return;
            if (Character.Ring.IsMember(pc))
            {
                var i = Character.Ring.IndexOf(pc);
                var p = new SSMG_PARTY_MEMBER_STATE();
                p.PartyIndex = (uint)i;
                p.CharID = pc.CharID;
                p.Online = pc.Online;
                NetIo.SendPacket(p);
            }
        }

        public void SendChatRing(string name, string content)
        {
            var p = new SSMG_CHAT_RING();
            p.Sender = name;
            p.Content = content;
            NetIo.SendPacket(p);
        }

        public void SendRingMeDelete(SSMG_RING_QUIT.Reasons reason)
        {
            var p = new SSMG_RING_QUIT();
            p.RingID = Character.Ring.ID;
            p.Reason = reason;
            NetIo.SendPacket(p);
        }

        public void SendRingMemberDelete(ActorPC pc)
        {
            var p = new SSMG_RING_MEMBER_INFO();
            p.Member(pc, null);
            NetIo.SendPacket(p);
        }

        public bool irisAddSlot;
        private uint irisAddSlotItem;
        private uint irisAddSlotMaterial;
        public bool irisCardAssemble;
        private uint irisCardItem;
        public bool irisGacha;


        public void OnIrisGachaCancel(CSMG_IRIS_GACHA_CANCEL p)
        {
            irisGacha = false;
        }


        private DrawType GetDrawTypeFromItem(uint itemID)
        {
            var drawtype = DrawType.Random;
            switch (itemID)
            {
                case 10067300:
                case 10067310:
                case 10067320:
                case 16003300:
                case 16003310:
                case 16003313:
                    break;
                case 10067301:
                    drawtype = DrawType.NomalOnly;
                    break;
                case 10067302:
                    drawtype = DrawType.UnCommonOnly;
                    break;
                case 10067303:
                    drawtype = DrawType.RarityOnly;
                    break;
                case 10067304:
                    drawtype = DrawType.SuperRarityOnly;
                    break;
                case 16003311:
                    drawtype = DrawType.AtleastOneSuperRarity;
                    break;
                default:
                    drawtype = DrawType.Random;
                    break;
            }

            return drawtype;
        }

        public void OnIrisGacha(CSMG_IRIS_GACHA_DRAW p)
        {
            var itemID = p.ItemID;

            var key = string.Format("{0},{1},{2}", p.PayFlag, p.SessionID, p.ItemID);

            if (CountItem(itemID) > 0)
                if (IrisGachaFactory.Instance.IrisGacha.ContainsKey(key))
                {
                    //根据使用的抽卡道具获取抽卡方式
                    var drawType = GetDrawTypeFromItem(itemID);

                    var gacha = IrisGachaFactory.Instance.IrisGacha[key];
                    var cards = new Dictionary<uint, byte>();
                    DeleteItemID(itemID, 1, true);

                    //这里获取本页所有的18张卡片
                    var selectedcards = IrisCardFactory.Instance.Items.Values.Where(x => x.Page == gacha.PageID)
                        .ToList();

                    //加入字典? 意义不明
                    foreach (var item in selectedcards)
                        cards.Add(item.ID, (byte)item.Rarity);

                    //声明结果对象
                    var results = new List<uint>();

                    var retitems = new List<Item>();


                    IrisDrawRate drawrate = null;

                    if (IrisDrawRateFactory.Instance.DrawRate.ContainsKey(key))
                        drawrate = IrisDrawRateFactory.Instance.DrawRate[key];

                    //先把所有的卡片抽出来
                    for (var i = 0; i < gacha.Count; i++)
                    {
                        var lottery = Global.Random.Next(0, 1000);
                        var Lcards = new List<uint>();
                        byte rank = 1;

                        if (lottery < (drawrate != null ? drawrate.SuperRatityRate : 5)) rank = 4;
                        else if (lottery < (drawrate != null ? drawrate.RatityRate : 55)) rank = 3;
                        else if (lottery < (drawrate != null ? drawrate.UnCommonRate : 185)) rank = 2;
                        else rank = 1;


                        while (cards.Count(x => x.Value == rank) == 0)
                            if (rank - 1 > 0)
                                rank -= 1;
                            else
                                rank = 4;

                        //不存在保底
                        //if (i == 9)
                        //{
                        //    rank = 2;//保底变R
                        //    if (lottery < 50) rank = 3;
                        //    if (lottery < 8) rank = 4;
                        //}
                        foreach (var i2 in cards)
                            if (i2.Value == rank)
                                Lcards.Add(i2.Key);

                        uint itemid = 0;

                        if (Lcards.Count == 1)
                            itemid = Lcards[0];
                        else
                            itemid = Lcards[Global.Random.Next(0, Lcards.Count - 1)];

                        var item = ItemFactory.Instance.GetItem(itemid);
                        item.Stack = 1;
                        item.Identified = true;
                        retitems.Add(item);
                    }

                    //这里根据drawtype 对已经抽到的10张卡片进行加工
                    var idx = 0;
                    switch (drawType)
                    {
                        case DrawType.Random:
                            break;
                        case DrawType.NomalOnly:
                            var uncommon = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Common).ToList();
                            for (var i = 0; i < uncommon.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Common).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(uncommon[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.UnCommonOnly:
                            var ununcommon = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Uncommon).ToList();
                            for (var i = 0; i < ununcommon.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Uncommon).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(ununcommon[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.RarityOnly:
                            var unrarity = retitems
                                .Where(x => IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Rare).ToList();
                            for (var i = 0; i < unrarity.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Rare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(unrarity[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.SuperRarityOnly:
                            var unsuperrarity = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.SuperRare).ToList();
                            for (var i = 0; i < unsuperrarity.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.SuperRare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(unsuperrarity[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.AtleastOneSuperRarity:
                            var superrarity = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity == Rarity.SuperRare).ToList();
                            if (superrarity.Count == 0)
                            {
                                idx = Global.Random.Next(0, retitems.Count - 1);
                                retitems.RemoveAt(idx);

                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.SuperRare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                    }


                    //这里把卡片给出去
                    foreach (var item in retitems)
                    {
                        results.Add(item.ItemID);
                        AddItem(item, true);
                    }

                    var p2 = new SSMG_IRIS_GACHA_RESULT();
                    p2.ItemIDs = results;
                    NetIo.SendPacket(p2);
                }
        }

        public void OnIrisCardAssembleCancel(CSMG_IRIS_CARD_ASSEMBLE_CANCEL p)
        {
            irisCardAssemble = false;
        }

        public void OnIrisCardAssemble(CSMG_IRIS_CARD_ASSEMBLE_CONFIRM p)
        {
            var cardID = p.CardID;
            if (CountItem(cardID) > 0)
            {
                if (IrisCardFactory.Instance.Items.ContainsKey(cardID))
                {
                    var card = IrisCardFactory.Instance.Items[cardID];
                    if (card.NextCard != 0)
                    {
                        var rates = new int[4] { 90, 60, 30, 5 };
                        var counts = new int[4] { 10, 2, 2, 2 };
                        var SupportItem = p.SupportItem;
                        var ProtectItem = p.ProtectItem;

                        var rate = rates[card.Rank];
                        var count = counts[card.Rank];
                        if (SupportItem == 10087101 || SupportItem == 10087100)
                            rate += 100;
                        else if (SupportItem != 0)
                            rate += 5;
                        if (CountItem(cardID) >= count)
                        {
                            if (Character.Gold >= 5000)
                            {
                                Character.Gold -= 5000;

                                if (SupportItem != 0)
                                    DeleteItemID(SupportItem, 1, true);

                                if (ProtectItem != 0)
                                    DeleteItemID(ProtectItem, 1, true);

                                if (Global.Random.Next(0, 99) < rate)
                                {
                                    DeleteItemID(cardID, (ushort)count, true);
                                    AddItem(ItemFactory.Instance.GetItem(card.NextCard), true);
                                    var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                    p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.OK;
                                    NetIo.SendPacket(p1);
                                }
                                else
                                {
                                    if (ProtectItem == 0)
                                        DeleteItemID(cardID, (ushort)count, true);

                                    var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                    p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.FAILED;
                                    NetIo.SendPacket(p1);
                                    irisCardAssemble = false;
                                }
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.NOT_ENOUGH_GOLD;
                                NetIo.SendPacket(p1);
                                irisCardAssemble = false;
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                            p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.SUCCESS_NOT_ENOUGH_ITEM;
                            NetIo.SendPacket(p1);
                            irisCardAssemble = false;
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.SUCCESS_NOT_ENOUGH_ITEM;
                        NetIo.SendPacket(p1);
                        irisCardAssemble = false;
                    }
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.NO_ITEM;
                NetIo.SendPacket(p1);
                irisCardAssemble = false;
            }
        }

        public void OnIrisCardClose(CSMG_IRIS_CARD_CLOSE p)
        {
            irisCardItem = 0;
        }

        public void OnIrisCardLock(CSMG_IRIS_CARD_LOCK p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                item.Locked = true;
                SendItemIdentify(item.Slot);
                var p1 = new SSMG_IRIS_CARD_LOCK_RESULT();
                NetIo.SendPacket(p1);
            }
        }

        public void OnIrisCardUnlock(CSMG_IRIS_CARD_UNLOCK p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                item.Locked = false;
                SendItemIdentify(item.Slot);

                var p1 = new SSMG_IRIS_CARD_UNLOCK_RESULT();
                p1.Result = (byte)(CountItem(16003400u) > 0 ? 0x00 : 0x01);
                if (CountItem(16003400u) > 0)
                    DeleteItem(GetItem(16003400u)[0].Slot, 1, true);
                NetIo.SendPacket(p1);
            }
        }

        /// <summary>
        ///     给武器打洞
        /// </summary>
        /// <param name="pc"></param>
        protected void ItemAddSlot(ActorPC pc)
        {
            var items = new List<uint>();
            foreach (var i in pc.Inventory.GetContainer(ContainerType.BODY))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.BACK_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.LEFT_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.RIGHT_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].CurrentSlot < 10)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                if (pc.Inventory.Equipments[EnumEquipSlot.UPPER_BODY].CurrentSlot < 10)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.UPPER_BODY].Slot);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                if (pc.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].CurrentSlot < 5)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].Slot);

            //思念的结晶
            if (CountItem(10073000) > 0)
                items.Insert(0, GetItem(10073000)[0].Slot);

            //大的思念结晶
            if (CountItem(10073100) > 0)
                items.Insert(0, GetItem(10073100)[0].Slot);

            //真实的思念结晶
            if (CountItem(10073200) > 0)
                items.Insert(0, GetItem(10073200)[0].Slot);

            //插槽用钻孔机3
            if (CountItem(6001400) > 0)
                items.Add(GetItem(16001400)[0].Slot);

            //插槽用钻孔机4
            if (CountItem(16001401) > 0)
                items.Add(GetItem(16001401)[0].Slot);

            //插槽用钻孔机5
            if (CountItem(16001402) > 0)
                items.Add(GetItem(16001402)[0].Slot);

            //插槽用钻孔机6
            if (CountItem(16001403) > 0)
                items.Add(GetItem(16001403)[0].Slot);

            //插槽用钻孔机7
            if (CountItem(16001404) > 0)
                items.Add(GetItem(16001404)[0].Slot);

            //插槽用钻孔机8
            if (CountItem(16001405) > 0)
                items.Add(GetItem(16001405)[0].Slot);

            //插槽用钻孔机9
            if (CountItem(16001407) > 0)
                items.Add(GetItem(16001407)[0].Slot);

            //插槽用钻孔机10
            if (CountItem(16001408) > 0)
                items.Add(GetItem(16001408)[0].Slot);

            //武具保险书·扩展插槽
            if (CountItem(16001500) > 0)
                items.Add(GetItem(16001500)[0].Slot);

            //∽スロット用ドリル（ビギナー）
            if (CountItem(16001406) > 0)
                items.Add(GetItem(16001406)[0].Slot);

            var p = new SSMG_IRIS_ADD_SLOT_ITEM_LIST();
            p.Items = items;
            NetIo.SendPacket(p);
        }

        public void OnIrisCardRemove(CSMG_IRIS_CARD_REMOVE p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                if (!item.Locked)
                {
                    if (p.CardSlot < item.Cards.Count)
                    {
                        var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.OK;
                        NetIo.SendPacket(p1);

                        var card = item.Cards[p.CardSlot];
                        AddItem(ItemFactory.Instance.GetItem(card.ID), true);
                        item.Cards.RemoveAt(p.CardSlot);
                        SendItemCardInfo(item);
                        SendItemCardAbility(item);
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                        NetIo.SendPacket(p1);
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                    p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                    NetIo.SendPacket(p1);
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                NetIo.SendPacket(p1);
            }
        }

        public void OnIrisCardInsert(CSMG_IRIS_CARD_INSERT p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                if (item.Cards.Count < item.CurrentSlot)
                {
                    var card = Character.Inventory.GetItem(p.InventorySlot);
                    if (card != null)
                        if (card.BaseData.itemType == ItemType.IRIS_CARD)
                        {
                            if (IrisCardFactory.Instance.Items.ContainsKey(card.BaseData.id))
                            {
                                DeleteItem(card.Slot, 1, true);
                                var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                                p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.OK;
                                NetIo.SendPacket(p1);
                                var cardInfo = IrisCardFactory.Instance.Items[card.BaseData.id];
                                item.Cards.Add(cardInfo);
                                SendItemCardInfo(item);
                                SendItemCardAbility(item);
                                StatusFactory.Instance.CalcStatus(Character);
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                                p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.CANNOT_SET;
                                NetIo.SendPacket(p1);
                            }
                        }
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                    p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.SLOT_OVER;
                    NetIo.SendPacket(p1);
                }
            }
        }

        public void OnIrisCardOpen(CSMG_IRIS_CARD_OPEN p)
        {
            var item = Character.Inventory.GetItem(p.InventorySlot);
            if (item != null)
            {
                if (item.CurrentSlot > 0)
                {
                    irisCardItem = item.Slot;
                    var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                    p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.OK;
                    NetIo.SendPacket(p1);

                    SendItemCardAbility(item);
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                    p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.NO_SLOT;
                    NetIo.SendPacket(p1);
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.NO_ITEM;
                NetIo.SendPacket(p1);
            }
        }

        public void OnIrisAddSlotConfirm(CSMG_IRIS_ADD_SLOT_CONFIRM p)
        {
            if (irisAddSlot)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item != null)
                {
                    var gold = item.BaseData.possibleLv * 1000;

                    var material = p.Material;
                    var protectitem = p.ProtectItem;
                    var supportitem = p.SupportItem;
                    if (CountItem(material) > 0)
                    {
                        if (Character.Gold > gold)
                        {
                            if ((!item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && item.CurrentSlot < 10) ||
                                (item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && item.CurrentSlot < 5))
                            {
                                Character.Gold -= gold;

                                DeleteItemID(material, 1, true);


                                var baseRate = 0;
                                if (!item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE))
                                    baseRate = 100 - item.CurrentSlot * 10;
                                else
                                    baseRate = 100 - item.CurrentSlot * 20;

                                if (baseRate < 0)
                                    baseRate = 5;

                                if (supportitem == 16001406 && item.CurrentSlot < 2)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 100;
                                }
                                else if (supportitem == 16001400 && item.CurrentSlot < 3)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001401 && item.CurrentSlot < 4)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001402 && item.CurrentSlot < 5)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001403 && item.CurrentSlot < 6)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001404 && item.CurrentSlot < 7)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001405 && item.CurrentSlot < 8)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001407 && item.CurrentSlot < 9)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001408 && item.CurrentSlot < 10)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }

                                if (protectitem != 0)
                                    DeleteItemID(protectitem, 1, true);


                                if (Global.Random.Next(1, 100) < baseRate)
                                {
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.OK;
                                    NetIo.SendPacket(p1);
                                    SendEffect(5145);
                                    item.CurrentSlot++;
                                    SendItemInfo(item);

                                    ItemAddSlot(Character);
                                    //this.irisAddSlot = false;
                                }
                                else if (protectitem != 0)
                                {
                                    //DeleteItemID(p.ProtectItem, 1, true);
                                    SendSystemMessage("装备打洞失败！使用了一本防爆书（打洞）。");
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                    NetIo.SendPacket(p1);

                                    ItemAddSlot(Character);
                                    //this.irisAddSlot = false;
                                }
                                else
                                {
                                    DeleteItem(item.Slot, 1, true);
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                    NetIo.SendPacket(p1);

                                    irisAddSlot = false;
                                }
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                NetIo.SendPacket(p1);

                                irisAddSlot = false;
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                            p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NOT_ENOUGH_GOLD;
                            NetIo.SendPacket(p1);

                            irisAddSlot = false;
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                        p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_RIGHT_MATERIAL;
                        NetIo.SendPacket(p1);

                        irisAddSlot = false;
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_ITEM;
                    NetIo.SendPacket(p1);

                    irisAddSlot = false;
                }
            }
        }

        public void OnIrisAddSlotCancel(CSMG_IRIS_ADD_SLOT_CANCEL p)
        {
            irisAddSlot = false;
        }

        public void OnIrisAddSlotItemSelect(CSMG_IRIS_ADD_SLOT_ITEM_SELECT p)
        {
            if (irisAddSlot)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item != null)
                {
                    var gold = item.BaseData.possibleLv * 1000;
                    uint material = 0;
                    if (item.BaseData.possibleLv <= 30)
                        material = 10073000;
                    else if (item.BaseData.possibleLv <= 70)
                        material = 10073100;
                    else
                        material = 10073200;
                    if (Character.Gold > gold)
                    {
                        if (item.CurrentSlot < 5)
                        {
                            irisAddSlotMaterial = material;
                            irisAddSlotItem = item.Slot;

                            var p1 = new SSMG_IRIS_ADD_SLOT_MATERIAL();
                            p1.Slot = 1;
                            p1.Material = material;
                            p1.Gold = gold;
                            NetIo.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                            p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                            NetIo.SendPacket(p1);
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                        p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NOT_ENOUGH_GOLD;
                        NetIo.SendPacket(p1);
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_ITEM;
                    NetIo.SendPacket(p1);
                    irisAddSlot = false;
                }
            }
        }

        public void SendItemCardInfo(Item item)
        {
            var p = new SSMG_ITEM_IRIS_CARD_INFO();
            p.Item = item;
            NetIo.SendPacket(p);
        }

        public void SendItemCardAbility(Item item)
        {
            var p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Deck;
            p.AbilityVectors = item.AbilityVectors(true);
            p.VectorValues = item.VectorValues(true, false).Values.ToList();
            p.VectorLevels = item.VectorValues(true, true).Values.ToList();
            var release = item.ReleaseAbilities(true);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                p.ElementsAttack = item.IrisElements(true);
            else
                p.ElementsAttack = Item.ElementsZero();
            if (item.EquipSlot[0] == EnumEquipSlot.UPPER_BODY || item.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE)
                p.ElementsDefence = item.IrisElements(true);
            else
                p.ElementsDefence = Item.ElementsZero();
            NetIo.SendPacket(p);

            p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Max;
            p.AbilityVectors = item.AbilityVectors(false);
            p.VectorValues = item.VectorValues(false, false).Values.ToList();
            p.VectorLevels = item.VectorValues(false, true).Values.ToList();
            release = item.ReleaseAbilities(false);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                p.ElementsAttack = item.IrisElements(false);
            else
                p.ElementsAttack = Item.ElementsZero();
            if (item.EquipSlot[0] == EnumEquipSlot.UPPER_BODY || item.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE)
                p.ElementsDefence = item.IrisElements(false);
            else
                p.ElementsDefence = Item.ElementsZero();
            NetIo.SendPacket(p);

            p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Total;
            p.AbilityVectors = Character.IrisAbilityValues.Keys.ToList();
            p.VectorValues = Character.IrisAbilityValues.Values.ToList();
            p.VectorLevels = Character.IrisAbilityLevels.Values.ToList();
            release = Item.ReleaseAbilities(Character.IrisAbilityLevels);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            p.ElementsAttack = Character.Status.attackelements_iris;
            p.ElementsDefence = Character.Status.elements_iris;
            NetIo.SendPacket(p);
        }

        public void OnNaviOpen(CSMG_NAVI_OPEN p)
        {
        }

        public bool fgTakeOff = false;
        private bool golemLogout;
        private PacketLogger.PacketLogger logger;
        private bool needSendGolem;
        public DateTime ping = DateTime.Now;

        public void OnPing(CSMG_PING p)
        {
            if (Character != null)
                if (Character.Online)
                {
                    ping = DateTime.Now;
                    if (!Character.Tasks.ContainsKey("Ping"))
                    {
                        var pa = new Ping(this);
                        Character.Tasks.Add("Ping", pa);
                        pa.Activate();
                    }
                }

            var p2 = new SSMG_PONG();
            NetIo.SendPacket(p2);
        }

        public void OnSendVersion(CSMG_SEND_VERSION p)
        {
            if (Configuration.Configuration.Instance.ClientVersion == null ||
                Configuration.Configuration.Instance.ClientVersion == p.GetVersion())
            {
                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.CLIENT_CONNECTING, p.GetVersion()));
                client_Version = p.GetVersion();

                var p1 = new SSMG_VERSION_ACK();
                p1.SetResult(SSMG_VERSION_ACK.Result.OK);
                p1.SetVersion(client_Version);
                NetIo.SendPacket(p1);
                //Official HK server will now request for Hackshield GUID check , we don't know its algorithms, so not implemented
                var p2 = new SSMG_LOGIN_ALLOWED();
                frontWord = (uint)Global.Random.Next();
                backWord = (uint)Global.Random.Next();
                p2.FrontWord = frontWord;
                p2.BackWord = backWord;
                NetIo.SendPacket(p2);
            }
            else
            {
                var p2 = new SSMG_VERSION_ACK();
                p2.SetResult(SSMG_VERSION_ACK.Result.VERSION_MISSMATCH);
                NetIo.SendPacket(p2);
            }
        }

        public void OnLogin(CSMG_LOGIN p)
        {
            p.GetContent();
            if (MapServer.shutingdown)
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                NetIo.SendPacket(p1);
                return;
            }

            if (AJImode.Instance.StopLogin)
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                NetIo.SendPacket(p1);
                return;
            }

            if (MapServer.accountDB.CheckPassword(p.UserName, p.Password, frontWord, backWord))
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.OK;
                p1.Unknown1 = 0x100;
                p1.TimeStamp = (uint)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                NetIo.SendPacket(p1);
                /*if(MapClientManager.Instance.OnlinePlayer.Count > 3)
                    System.Environment.Exit(System.Environment.ExitCode);*/


                account = MapServer.accountDB.GetUser(p.UserName);
                var check = from acc in MapClientManager.Instance.OnlinePlayer
                            where acc.account.Name == account.Name
                            select acc;
                foreach (var i in check) i.NetIo.Disconnect();

                account.LastIP = NetIo.sock.RemoteEndPoint.ToString().Split(':')[0];
                account.MacAddress = p.MacAddress;

                //这里检查同mac的已在线玩家, 如果大于或等于2个. 则断开当前请求的连接
                var players = MapClientManager.Instance.OnlinePlayer;
                var insamemac = players.Count(x => x.account.MacAddress == account.MacAddress);
                var insameip = players.Count(x => x.account.LastIP == account.LastIP);
                var onlinecount = Math.Max(insamemac, insameip);
                if (onlinecount >= Configuration.Configuration.Instance.MaxCharacterInMapServer)
                {
                    NetIo.Disconnect();
                    return;
                }


                //VariableHolderA<string, int> list = ScriptManager.Instance.VariableHolder.Adict["多开MAC限制"];
                //VariableHolderA<string, int> dailyban = ScriptManager.Instance.VariableHolder.Adict["多开当日限制登录的账号"];
                //if (ScriptManager.Instance.VariableHolder.AStr["多开MAC限制时间"] != DateTime.Now.ToString("yyyy-MM-dd"))
                //{
                //    ScriptManager.Instance.VariableHolder.AStr["多开MAC限制时间"] = DateTime.Now.ToString("yyyy-MM-dd");
                //    list = new VariableHolderA<string, int>();
                //    dailyban = new VariableHolderA<string, int>();
                //}

                //if(dailyban.ContainsKey(account.Name))
                //{
                //    netIO.Disconnect();
                //    return;
                //}
                //if (account.AccountID > 247)
                //{
                //    if (!list.ContainsKey(account.MacAddress))
                //        list[account.MacAddress] = 0;
                //    if (list[account.MacAddress] == 0)
                //        list[account.MacAddress] = account.AccountID;
                //    else
                //    {
                //        if ((list[account.MacAddress] != account.AccountID || dailyban.ContainsKey(account.Name)) && account.GMLevel < 20)
                //        {
                //            netIO.Disconnect();
                //            //这个ban造成了不好的影响，暂时关掉
                //            //dailyban[account.Name] = account.AccountID;
                //            return;
                //        }
                //    }
                //}

                //byte count = 0;
                //foreach (MapClient i in MapClientManager.Instance.OnlinePlayer)
                //{
                //    if (i.Character.Account.LastIP == account.LastIP && i.Character.Account.GMLevel < 20)
                //    {
                //        count++;
                //    }
                //}
                //if (count > 2)
                //{
                //    netIO.Disconnect();
                //    return;
                //}

                var charIDs = MapServer.charDB.GetCharIDs(account.AccountID);

                account.Characters = new List<ActorPC>();
                for (var i = 0; i < charIDs.Length; i++) account.Characters.Add(MapServer.charDB.GetChar(charIDs[i]));
                state = SESSION_STATE.AUTHENTIFICATED;
            }
            else
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BADPASS;
                NetIo.SendPacket(p1);
            }
        }

        public void OnCharSlot(CSMG_CHAR_SLOT p)
        {
            if (state == SESSION_STATE.AUTHENTIFICATED)
            {
                var chr =
                    from c in account.Characters
                    where c.Slot == p.Slot
                    select c;
                Character = chr.First();
                //if (MapClientManager.Instance.OnlinePlayer.Count > 10)
                //{

                //}
                MapServer.charDB.GetDualJobInfo(Character);
                if (Character.DualJobID != 0)
                    Character.DualJobLevel = Character.PlayerDualJobList[Character.DualJobID].DualJobLevel;
                if (Character.PossessionTarget != 0)
                {
                    if (Character.PossessionTarget < 10000)
                    {
                        var client = (MapClient)MapClientManager.Instance.GetClient(Character.PossessionTarget);
                        if (client != null)
                        {
                            var pc = client.Character;
                            ActorPC found = null;
                            foreach (var i in pc.PossesionedActors)
                                if (i.CharID == Character.CharID)
                                {
                                    found = i;
                                    break;
                                }

                            if (found != null)
                            {
                                found.Inventory = Character.Inventory;
                                Character = found;

                                Character.MapID = pc.MapID;
                                Character.X = pc.X;
                                Character.Y = pc.Y;
                            }
                            else
                            {
                                Character.PossessionTarget = 0;
                            }
                        }
                        else
                        {
                            Character.PossessionTarget = 0;
                        }
                    }
                    else
                    {
                        var map = MapManager.Instance.GetMap(Character.MapID);
                        if (map != null)
                        {
                            var actor = map.GetActor(Character.PossessionTarget);
                            if (actor == null)
                            {
                                Character.PossessionTarget = 0;
                            }
                            else
                            {
                                if (actor.type == ActorType.ITEM)
                                {
                                    var item = (ActorItem)actor;
                                    if (item.Item.PossessionedActor.CharID == Character.CharID)
                                    {
                                        var pc = item.Item.PossessionedActor;
                                        pc.Inventory = Character.Inventory;
                                        Character = pc;
                                        Character.MapID = item.MapID;
                                        Character.X = item.X;
                                        Character.Y = item.Y;
                                    }
                                    else
                                    {
                                        Character.PossessionTarget = 0;
                                    }
                                }
                                else
                                {
                                    Character.PossessionTarget = 0;
                                }
                            }
                        }
                        else
                        {
                            Character.PossessionTarget = 0;
                        }
                    }
                }

                if (Character.Golem != null)
                {
                    var map = MapManager.Instance.GetMap(Character.MapID);
                    if (map != null)
                    {
                        var actor = map.GetActor(Character.Golem.ActorID);
                        if (actor != null)
                        {
                            if (actor.type == ActorType.GOLEM)
                            {
                                var golem = (ActorGolem)actor;

                                if (golem.Owner.CharID == Character.CharID)
                                {
                                    golem.Owner.Inventory.WareHouse = Character.Inventory.WareHouse;
                                    Character.Inventory = golem.Owner.Inventory;
                                    Character.Gold = golem.Owner.Gold;
                                    Character.Golem = golem;
                                    golem.ClearTaskAddition();
                                    map.DeleteActor(golem);
                                }
                                else
                                {
                                    Character.Golem = null;
                                }
                            }
                        }
                        else
                        {
                            Character.Golem = null;
                        }
                    }
                    else
                    {
                        Character.Golem = null;
                    }
                }

                if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                    if (Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.itemType == ItemType.RIDE_PET ||
                        Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.itemType == ItemType.RIDE_PARTNER)
                    {
                        Character.Pet = new ActorPet(Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.petID,
                            Character.Inventory.Equipments[EnumEquipSlot.PET]);
                        Character.Pet.Ride = true;
                        Character.Pet.Owner = Character;
                    }

                Character.e = new PCEventHandler(this);
                if (Character.Account == null) Character.Account = account;
                Character.Online = true;
                Character.Party = PartyManager.Instance.GetParty(Character.Party);
                PartyManager.Instance.PlayerOnline(Character.Party, Character);

                Character.Ring = RingManager.Instance.GetRing(Character.Ring);
                RingManager.Instance.PlayerOnline(Character.Ring, Character);

                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.PLAYER_LOG_IN, Character.Name));
                Logger.ShowInfo(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                MapClientManager.Instance.OnlinePlayer.Count);

                var SerPC = ScriptManager.Instance.VariableHolder;
                var day = DateTime.Now.ToString("d");

                //记录最大在线人数
                if (MapClientManager.Instance.OnlinePlayer.Count > SerPC.AInt[day + "最大在线人数"])
                {
                    SerPC.AInt[day + "最大在线人数"] = MapClientManager.Instance.OnlinePlayer.Count;
                    SerPC.AStr[day + "最大在线人数日期"] = DateTime.Now.ToString("T");
                }

                //记录当日账号登陆数量
                if (!SerPC.Adict[day + "账号统计"].ContainsKey(Character.Account.Name))
                {
                    SerPC.Adict[day + "账号统计"][Character.Account.Name] = 1;
                    SerPC.AInt[day + "登陆账号数"]++;
                }
                else
                {
                    SerPC.Adict[day + "账号统计"][Character.Account.Name]++;
                }

                //记录当日IP登陆数量
                if (!SerPC.Adict[day + "IP统计"].ContainsKey(Character.Account.LastIP))
                {
                    SerPC.Adict[day + "IP统计"][Character.Account.LastIP] = 1;
                    SerPC.AInt[day + "登陆IP数"]++;
                }
                else
                {
                    SerPC.Adict[day + "IP统计"][Character.Account.LastIP]++;
                }

                MapServer.shouldRefreshStatistic = true;

                Map = MapManager.Instance.GetMap(Character.MapID);
                if (this.map == null)
                {
                    if (Character.SaveMap == 0)
                    {
                        Character.SaveMap = 10023100;
                        Character.SaveX = 242;
                        Character.SaveY = 128;
                    }

                    Character.MapID = Character.SaveMap;
                    map = MapManager.Instance.GetMap(Character.SaveMap);
                    Character.X = Global.PosX8to16(Character.SaveX, map.Width);
                    Character.Y = Global.PosY8to16(Character.SaveY, map.Height);
                }

                if (this.map.IsMapInstance && Character.PossessionTarget == 0)
                {
                    var map = this.map;
                    Character.MapID = this.map.ClientExitMap;
                    this.map = MapManager.Instance.GetMap(this.map.ClientExitMap);
                    Character.X = Global.PosX8to16(map.ClientExitX, this.map.Width);
                    Character.Y = Global.PosY8to16(map.ClientExitY, this.map.Height);
                }

                if (Character.Race != PC_RACE.DEM)
                {
                    if (Character.CEXP < ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL))
                        Character.CEXP = ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL);
                    if (Character.DominionCEXP <
                        ExperienceManager.Instance.GetExpForLevel(Character.DominionLevel, LevelType.CLEVEL))
                        Character.DominionCEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.DominionLevel, LevelType.CLEVEL);
                    if (Character.DominionJEXP <
                        ExperienceManager.Instance.GetExpForLevel(Character.DominionJobLevel, LevelType.JLEVEL2))
                        Character.DominionJEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.DominionJobLevel, LevelType.JLEVEL2);
                    if (Character.Job == Character.JobBasic)
                    {
                        if (Character.JEXP <
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel1, LevelType.JLEVEL))
                            Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(Character.JobLevel1, LevelType.JLEVEL);
                    }
                    else if (Character.Job == Character.Job2X)
                    {
                        if (Character.JEXP <
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2))
                            Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2);
                    }
                    else if (Character.Job == Character.Job2T)
                    {
                        if (Character.JEXP <
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2))
                            Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2);
                    }
                    else
                    {
                        if (Character.JEXP <
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel3, LevelType.JLEVEL3))
                            Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(Character.JobLevel3, LevelType.JLEVEL3);
                    }
                }

                if (Character.DominionStr < Configuration.Configuration.Instance.StartupSetting[Character.Race].Str)
                    Character.DominionStr = Configuration.Configuration.Instance.StartupSetting[Character.Race].Str;
                if (Character.DominionDex < Configuration.Configuration.Instance.StartupSetting[Character.Race].Dex)
                    Character.DominionDex = Configuration.Configuration.Instance.StartupSetting[Character.Race].Dex;
                if (Character.DominionInt < Configuration.Configuration.Instance.StartupSetting[Character.Race].Int)
                    Character.DominionInt = Configuration.Configuration.Instance.StartupSetting[Character.Race].Int;
                if (Character.DominionVit < Configuration.Configuration.Instance.StartupSetting[Character.Race].Vit)
                    Character.DominionVit = Configuration.Configuration.Instance.StartupSetting[Character.Race].Vit;
                if (Character.DominionAgi < Configuration.Configuration.Instance.StartupSetting[Character.Race].Agi)
                    Character.DominionAgi = Configuration.Configuration.Instance.StartupSetting[Character.Race].Agi;
                if (Character.DominionMag < Configuration.Configuration.Instance.StartupSetting[Character.Race].Mag)
                    Character.DominionMag = Configuration.Configuration.Instance.StartupSetting[Character.Race].Mag;

                Character.WRPRanking = WRPRankingManager.Instance.GetRanking(Character);

                foreach (var i in Character.Inventory.Equipments.Values)
                    if (i.BaseData.jointJob != PC_JOB.NONE)
                    {
                        Character.JobJoint = i.BaseData.jointJob;
                        break;
                    }

                Map.RegisterActor(Character);
                state = SESSION_STATE.LOADING;
                if (Character.Golem != null) needSendGolem = true;

                //现在,死亡的角色上线会回到记录点
                if (Character.HP == 0)
                {
                    if (Character.SaveMap == 0)
                    {
                        Character.SaveMap = 10023000;
                        Character.SaveX = 123;
                        Character.SaveY = 233;
                    }

                    Character.MapID = Character.SaveMap;
                    map = MapManager.Instance.GetMap(Character.SaveMap);
                    Character.X = Global.PosX8to16(Character.SaveX, map.Width);
                    Character.Y = Global.PosY8to16(Character.SaveY, map.Height);
                    Character.HP = 1;
                }

                if (this.map.ID == 90001999 || Character.MapID == 91000999)
                {
                    SendGotoFF();

                    var p2 = new Packet();
                    p2.data = new byte[3];
                    p2.ID = 0x122a;
                    NetIo.SendPacket(p);
                }

                if (!Character.Tasks.ContainsKey("Recover")) //自然恢复
                {
                    var reg = new Recover(this);
                    Character.Tasks.Add("Recover", reg);
                    reg.Activate();
                }

                /*
                SagaDB.LevelLimit.LevelLimit LL = SagaDB.LevelLimit.LevelLimit.Instance;

                if (this.Character.Level >= LL.NowLevelLimit)
                {
                    this.SendAnnounce("当前等级暂时到达上限，正在接受圣塔的福利...");
                    this.SendAnnounce(string.Format("下次等级上限 {0} 级的解除时间为 {1}",LL.NextLevelLimit,LL.NextTime.ToString()));
                    SagaMap.LevelLimit.LevelLimitManager.Instance.SendReachInfo(this);
                }*/
                StatusFactory.Instance.CalcStatus(Character);
                /*if (this.chara.EPLoginTime < DateTime.Now)
                {
                    this.chara.EP += 10;
                    if (this.chara.EP > this.chara.MaxEP)
                        this.chara.EP = this.chara.MaxEP;
                    this.chara.EPLoginTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
                }
                else
                {
                    TimeSpan span = this.chara.EPLoginTime - DateTime.Now;
                    SendSystemMessage(string.Format(Manager.LocalManager.Instance.Strings.EP_INCREASE, (int)span.Hours + 1));
                }*/ //改革！

                //packet logger for unnormal player
                foreach (var i in Configuration.Configuration.Instance.MonitorAccounts)
                    if (account.Name.StartsWith(i))
                    {
                        logger = new PacketLogger.PacketLogger(this);
                        break;
                    }

                if (Logger.defaultSql != null)
                    Logger.defaultSql.SQLExecuteNonQuery("UPDATE `char` SET `online` = 1 WHERE `char_id` = " +
                                                         Character.CharID);
            }
        }

        public void OnMapLoaded(CSMG_PLAYER_MAP_LOADED p)
        {
            CheckAPI = false;
            Character.invisble = false;
            Character.VisibleActors.Clear();
            Map.OnActorVisibilityChange(Character);
            Map.SendVisibleActorsToActor(Character);

            if (Character.MapID == 21190000)
                Character.CInt["NextMoveEventID"] = 80000103;
            if (Character.MapID == 90001999)
                CustomMapManager.Instance.EnterFFOnMapLoaded(this);
            //if (this.Character.MapID == 90001999 || this.Character.MapID == 91000999)
            //CustomMapManager.Instance.SendGotoSerFFMap(this);

            if (Character.TInt["TempBGM"] > 0)
            {
                var p3 = new SSMG_NPC_CHANGE_BGM();
                p3.SoundID = (uint)Character.TInt["TempBGM"];
                p3.Loop = 50;
                p3.Volume = 100;
                NetIo.SendPacket(p3);
                Character.TInt["TempBGM"] = 0;
                Character.TInt.Remove("TempBGM");
            }

            if (needSendGolem)
            {
                var golem = Character.Golem;
                if (golem.GolemType == GolemType.Sell)
                {
                    var p2 = new SSMG_GOLEM_SHOP_SELL_RESULT();
                    p2.SoldItems = golem.SoldItem;
                    NetIo.SendPacket(p2);
                }

                if (golem.GolemType == GolemType.Buy)
                {
                    var p2 = new SSMG_GOLEM_SHOP_BUY_RESULT();
                    p2.BoughtItems = golem.BoughtItem;
                    NetIo.SendPacket(p2);
                }

                if (golem.GolemType >= GolemType.Plant && golem.GolemType <= GolemType.Strange)
                    OnGolemWarehouse(new CSMG_GOLEM_WAREHOUSE());
                Character.Golem.SoldItem.Clear();
                Character.Golem.BoughtItem.Clear();
                Character.Golem.SellShop.Clear();
                Character.Golem.BuyShop.Clear();
                needSendGolem = false;
            }

            SendFGEvent();
            SendDungeonEvent();

            if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
            {
                if (Character.Inventory.Equipments[EnumEquipSlot.PET].IsPet &&
                    Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.itemType == ItemType.PET)
                {
                    SendPet(Character.Inventory.Equipments[EnumEquipSlot.PET]);
                }
                else if (Character.Inventory.Equipments[EnumEquipSlot.PET].BaseData.itemType == ItemType.PARTNER)
                {
                    SendPartner(Character.Inventory.Equipments[EnumEquipSlot.PET]);
                    PartnerTalking(Character.Partner, TALK_EVENT.MASTERLOGIN, 100);
                }
            }

            StatusFactory.Instance.CalcStatus(Character);
            SendEquip();
            Character.e.OnActorChangeBuff(Character);

            if (Character.PossessionTarget != 0)
            {
                var actor = Map.GetActor(Character.PossessionTarget);
                if (actor != null)
                    Character.e.OnActorAppears(actor);
            }

            foreach (Actor i in Character.PossesionedActors)
                if (i != Character)
                    i.e.OnActorAppears(Character);

            SendQuestInfo();
            SendQuestPoints();
            SendQuestCount();
            SendQuestTime();
            SendQuestStatus();
            SendStamp();
            SendTamaire();
            SendTitleList();
            SendWRPRanking(Character);

            if (ODWarFactory.Instance.Items.ContainsKey(map.ID))
            {
                if (!Character.Skills.ContainsKey(2457))
                {
                    var skill = SkillFactory.Instance.GetSkill(2457, 1);
                    skill.NoSave = true;
                    Character.Skills.Add(2457, skill);
                }
            }
            else
            {
                if (Character.Skills.ContainsKey(2457)) Character.Skills.Remove(2457);
            }

            if (map.Info.Flag.Test(MapFlags.Dominion))
            {
                if (Character.WRPRanking <= 10)
                {
                    if (!Character.Skills.ContainsKey(10500))
                    {
                        var skill = SkillFactory.Instance.GetSkill(10500, 1);
                        skill.NoSave = true;
                        Character.Skills.Add(10500, skill);
                    }
                }
                else
                {
                    if (Character.Skills.ContainsKey(10500)) Character.Skills.Remove(10500);
                }
            }
            else
            {
                if (Character.Skills.ContainsKey(10500)) Character.Skills.Remove(10500);
            }

            SendPlayerInfo();


            if (this.map.Info.Flag.Test(MapFlags.Wrp)) SendSystemMessage(LocalManager.Instance.Strings.WRP_ENTER);

            SendPartyInfo();
            SendRingInfo(SSMG_RING_INFO.Reason.NONE);
            SendRingFF();
            PartyManager.Instance.UpdateMemberPosition(Character.Party, Character);
            if (this.map.IsDungeon && Character.Party != null)
                PartyManager.Instance.UpdateMemberDungeonPosition(Character.Party, Character);

            if (TheaterFactory.Instance.Items.ContainsKey(this.map.ID))
            {
                var nextMovie = TheaterFactory.Instance.GetNextMovie(map.ID);
                if (nextMovie != null)
                {
                    var p3 = new SSMG_THEATER_INFO();
                    p3.MessageType = SSMG_THEATER_INFO.Type.MESSAGE;
                    p3.Message = LocalManager.Instance.Strings.THEATER_WELCOME;
                    NetIo.SendPacket(p3);
                    p3 = new SSMG_THEATER_INFO();
                    p3.MessageType = SSMG_THEATER_INFO.Type.MOVIE_ADDRESS;
                    p3.Message = nextMovie.URL;
                    NetIo.SendPacket(p3);
                }
            }

            if (!Character.Tasks.ContainsKey("QuestTime"))
            {
                var task = new QuestTime(this);
                Character.Tasks.Add("QuestTime", task);
                task.Activate();
            }

            if (!Character.Tasks.ContainsKey("AutoSave"))
            {
                var task = new AutoSave(Character);
                Character.Tasks.Add("AutoSave", task);
                task.Activate();
            }
            //SP改革
            /*if (!this.Character.Tasks.ContainsKey("SpRecover"))
            {
                Tasks.PC.SpRecover task = new SagaMap.Tasks.PC.SpRecover(this);
                this.Character.Tasks.Add("SpRecover", task);
                task.Activate();
            }*/

            if (Map.Info.Healing)
            {
                if (!Character.Tasks.ContainsKey("CityRecover"))
                {
                    var task = new CityRecover(this);
                    Character.Tasks.Add("CityRecover", task);
                    task.Activate();
                }
            }
            else
            {
                if (Character.Tasks.ContainsKey("CityRecover"))
                {
                    Character.Tasks["CityRecover"].Deactivate();
                    Character.Tasks.Remove("CityRecover");
                }
            }

            if (Map.Info.Cold || this.map.Info.Hot || this.map.Info.Wet)
            {
                if (!Character.Tasks.ContainsKey("CityDown"))
                {
                    var task = new CityDown(this);
                    Character.Tasks.Add("CityDown", task);
                    task.Activate();
                }
            }
            else
            {
                if (Character.Tasks.ContainsKey("CityDown"))
                {
                    Character.Tasks["CityDown"].Deactivate();
                    Character.Tasks.Remove("CityDown");
                }
            }

            if (Character.PossessionTarget != 0)
                if (!Character.Tasks.ContainsKey("PossessionRecover"))
                {
                    var task = new PossessionRecover(this);
                    Character.Tasks.Add("PossessionRecover", task);
                    task.Activate();
                }

            var p1 = new SSMG_LOGIN_FINISHED();
            NetIo.SendPacket(p1);
            state = SESSION_STATE.LOADED;

            if (Character.TInt["NextMapEvent"] > 0)
            {
                EventActivate((uint)Character.TInt["NextMapEvent"]);
                Character.TInt["NextMapEvent"] = 0;
            }

            if (Character.MapID == 90001999)
                CustomMapManager.Instance.EnterFFOnMapLoaded(this);

            //SendMails();
            //SendGifts();

            SendNPCStates();

            //誰的坑！！快填平！
            //SendMosterGuide();

            Character.Speed = 410;
            SendActorSpeed(Character, Character.Speed);
            if (Character.TranceID != 0)
                this.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);


            if (DefWarManager.Instance.IsDefWar(this.map.ID) && Character.DefWarShow)
            {
                var p2 = new SSMG_DEFWAR_INFO();
                p2.List = DefWarManager.Instance.GetDefWarList(map.ID);
                NetIo.SendPacket(p2);
            }
            //SendGifts();

            //Check Actor is leaving Tent Map
            if (Character.TenkActor != null)
                //檢查是否離開TentActor的所在地圖及TentMap
                if (Character.MapID != Character.TenkActor.TentMapID && Character.MapID != Character.TenkActor.MapID)
                {
                    //刪除當前地圖的TentActor
                    var cmap = MapManager.Instance.GetMap(Character.TenkActor.MapID);
                    cmap.DeleteActor(Character.TenkActor);

                    //若TentMap 已創建則刪除地圖
                    if (Character.TenkActor.TentMapID != 0)
                    {
                        var map = MapManager.Instance.GetMap(Character.TenkActor.Caster.MapID);
                        MapManager.Instance.DeleteMapInstance(map.ID);
                    }

                    Logger.ShowInfo("Destory Player's Tent : " + Character.Name + " (" + Character.TenkActor.EventID +
                                    ")");

                    Character.TenkActor = null;
                }

            //Check API Item
            if (CheckAPI == false)
            {
                var pr = new Process.Process();
                pr.CheckAPIItem(Character.CharID, this);
                CheckAPI = true;
            }


            //Send Daily Stamp
            var thisDay = DateTime.Today;
            if (Character.AStr["DailyStamp_DAY"] != thisDay.ToString("d"))
            {
                var ds = new SSMG_PLAYER_SHOW_DAILYSTAMP();
                ds.Type = 1;
                NetIo.SendPacket(ds);
            }

            //MapDefaultScript
            if (ScriptManager.Instance.Events.ContainsKey(Character.MapID))
                EventActivate(Character.MapID);
        }

        public void OnLogout(CSMG_LOGOUT p)
        {
            //竟然不清状态。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            //Logout sequence
            //this.PossessionPrepareCancel();
            if (p.Result == (CSMG_LOGOUT.Results)1)
                golemLogout = true;
            else
                golemLogout = false;
            var p1 = new SSMG_LOGOUT();
            p1.Result = (SSMG_LOGOUT.Results)p.Result;
            PartnerTalking(Character.Partner, TALK_EVENT.MASTERQUIT, 100, 5000);
            NetIo.SendPacket(p1);
        }

        public void OnSSOLogout(CSMG_SSO_LOGOUT p)
        {
            //竟然不清状态。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            //Packets.Server.SSMG_SSO_LOGOUT p1 = new Packets.Server.SSMG_SSO_LOGOUT();
            //this.NetIo.SendPacket(p1);
            NetIo.Disconnect();
        }

        private ActorPC partyPartner;

        public void OnPartyRoll(CSMG_PARTY_ROLL p)
        {
            if (Character.Party == null) return;
            if (Character.Party.Leader != Character) return;

            if (p.status == 1)
            {
                Character.Party.Roll = 0;
                foreach (var item in Character.Party.Members.Values)
                    if (item.Online)
                        FromActorPC(item).SendRollInfo(item);
            }

            if (p.status == 0)
            {
                Character.Party.Roll = 1;
                foreach (var item in Character.Party.Members.Values)
                    if (item.Online)
                        FromActorPC(item).SendRollInfo(item);
            }
        }

        public void SendRollInfo(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p2 = new SSMG_PARTY_ROLL();
                    byte roll = 0;
                    if (pc.Party.Roll == 0) roll = 1;
                    p2.status = roll;
                    NetIo.SendPacket(p2);
                }
        }

        public void OnPartyName(CSMG_PARTY_NAME p)
        {
            if (Character.Party == null) return;
            if (p.Name == "") return;
            if (Character.Party.Leader != Character) return;
            Character.Party.Name = p.Name;
            PartyManager.Instance.UpdatePartyName(Character.Party);
        }

        public void OnPartyKick(CSMG_PARTY_KICK p)
        {
            if (Character.Party == null)
                return;
            if (Character.Party.Leader != Character)
                return;
            var p1 = new SSMG_PARTY_KICK();
            if (Character.Party.IsMember(p.CharID))
            {
                PartyManager.Instance.DeleteMember(Character.Party, p.CharID, SSMG_PARTY_DELETE.Result.KICKED);
                p1.Result = 0;
            }
            else
            {
                p1.Result = -1; //指定プレイヤーが存在しません
            }

            NetIo.SendPacket(p1);
        }

        public void OnPartyQuit(CSMG_PARTY_QUIT p)
        {
            var p1 = new SSMG_PARTY_QUIT();
            if (Character.Party == null)
            {
                p1.Result = -1; //パーティーに所属していません
            }
            else
            {
                if (Character != Character.Party.Leader)
                    PartyManager.Instance.DeleteMember(Character.Party, Character.CharID,
                        SSMG_PARTY_DELETE.Result.QUIT);
                else
                    PartyManager.Instance.PartyDismiss(Character.Party);
            }

            NetIo.SendPacket(p1);
        }

        public void OnPartyInviteAnswer(CSMG_PARTY_INVITE_ANSWER p)
        {
            if (partyPartner == null) return;
            if (partyPartner.CharID != p.CharID) return;
            var client = FromActorPC(partyPartner);
            if ((client.Character.Mode == PlayerMode.KNIGHT_EAST || client.Character.Mode == PlayerMode.KNIGHT_FLOWER ||
                 client.Character.Mode == PlayerMode.KNIGHT_NORTH
                 || client.Character.Mode == PlayerMode.KNIGHT_ROCK ||
                 client.Character.Mode == PlayerMode.KNIGHT_SOUTH || client.Character.Mode == PlayerMode.KNIGHT_WEST)
                && (Character.Mode == PlayerMode.KNIGHT_EAST || Character.Mode == PlayerMode.KNIGHT_FLOWER ||
                    Character.Mode == PlayerMode.KNIGHT_NORTH
                    || Character.Mode == PlayerMode.KNIGHT_ROCK || Character.Mode == PlayerMode.KNIGHT_SOUTH ||
                    Character.Mode == PlayerMode.KNIGHT_WEST)
               )
                if (client.Character.Mode != Character.Mode)
                    return;

            if (client.Character.Party != null)
            {
                if (client.Character.Party.MemberCount >= 8)
                    return;
                PartyManager.Instance.AddMember(client.Character.Party, Character);
                PartnerTalking(Character.Partner, TALK_EVENT.JOINPARTY, 100, 0);
            }
            else
            {
                var party = PartyManager.Instance.CreateParty(partyPartner);
                PartyManager.Instance.AddMember(party, Character);
                PartnerTalking(partyPartner.Partner, TALK_EVENT.JOINPARTY, 100, 0);
                PartnerTalking(client.Character.Partner, TALK_EVENT.JOINPARTY, 100, 0);
            }
        }

        public void OnPartyInvite(CSMG_PARTY_INVITE p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var result = CheckPartyInvite(client);
            var p1 = new SSMG_PARTY_INVITE_RESULT();
            p1.InviteResult = result;
            if (result >= 0)
            {
                var p2 = new SSMG_PARTY_INVITE();
                p2.CharID = Character.CharID;
                p2.Name = Character.Name;
                client.partyPartner = Character;
                client.NetIo.SendPacket(p2);
            }

            NetIo.SendPacket(p1);
        }

        private int CheckPartyInvite(MapClient client)
        {
            if (client == null)
                return -2; //プレイヤーが存在しません
            if (client.scriptThread != null || client.trading)
                return -3; //相手がパーティに誘えない状態になりました
            if (client.Character.Party != null)
            {
                if (Character.Party != null)
                    if (Character.Party.IsMember(client.Character.CharID))
                        return -11; //既にパーティーが存在しています
                return -10; //既にパーティーに所属しています
            }

            if (Character.Party != null)
                if (Character.Party.MemberCount == 8)
                    return -12; //パーティー人数が限界を超えてます
            return 0;
        }

        public void SendPartyInfo()
        {
            if (Character.Party == null)
                return;
            var p = new SSMG_PARTY_INFO();
            p.Party(Character.Party, Character);
            var p1 = new SSMG_PARTY_NAME();
            p1.Party(Character.Party, Character);
            NetIo.SendPacket(p);
            NetIo.SendPacket(p1);
            SendPartyMember();
        }

        public void SendPartyMeDelete(SSMG_PARTY_DELETE.Result reason)
        {
            var p = new SSMG_PARTY_DELETE();
            p.PartyID = Character.Party.ID;
            p.PartyName = Character.Party.Name;
            p.Reason = reason;
            NetIo.SendPacket(p);
        }

        public void SendPartyMemberDelete(uint pc)
        {
            var p = new SSMG_PARTY_MEMBER();
            p.PartyIndex = -1;
            p.CharID = pc;
            p.CharName = "";
            NetIo.SendPacket(p);
        }

        public void SendPartyMemberPosition(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p1 = new SSMG_PARTY_MEMBER_POSITION();
                    p1.PartyIndex = Character.Party.IndexOf(pc);
                    p1.CharID = pc.CharID;
                    var mapid = pc.MapID;
                    var map = MapManager.Instance.GetMap(pc.MapID);
                    if (map.returnori)
                        mapid = map.OriID;
                    p1.MapID = mapid;
                    p1.X = Global.PosX16to8(pc.X, MapManager.Instance.GetMap(pc.MapID).Width);
                    p1.Y = Global.PosY16to8(pc.Y, MapManager.Instance.GetMap(pc.MapID).Height);
                    NetIo.SendPacket(p1);
                }
        }

        public void SendPartyMemberDeungeonPosition(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (this.map.IsDungeon)
                {
                    var map = MapManager.Instance.GetMap(pc.MapID);
                    if (map.IsDungeon)
                    {
                        var p = new SSMG_PARTY_MEMBER_DUNGEON_POSITION();
                        p.CharID = pc.CharID;
                        p.MapID = map.ID;
                        p.X = map.DungeonMap.X;
                        p.Y = map.DungeonMap.Y;
                        p.Dir = map.DungeonMap.Dir;
                        NetIo.SendPacket(p);
                    }
                }
        }

        public void SendPartyMemberDetail(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                    p2.PartyIndex = Character.Party.IndexOf(pc);
                    p2.CharID = pc.CharID;
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                    p2.Job = pc.Job;
                    p2.Level = pc.Level;
                    p2.JobLevel = pc.CurrentJobLevel;
                    NetIo.SendPacket(p2);
                }
        }

        public void SendPartyMemberState(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
            {
                var i = Character.Party.IndexOf(pc);
                var p = new SSMG_PARTY_MEMBER_STATE();
                p.PartyIndex = i;
                p.CharID = pc.CharID;
                p.Online = pc.Online;
                NetIo.SendPacket(p);
            }
        }

        public void SendPartyMemberHPMPSP(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
            {
                var i = Character.Party.IndexOf(pc);
                var p3 = new SSMG_PARTY_MEMBER_HPMPSP();
                p3.PartyIndex = i;
                p3.CharID = pc.CharID;
                p3.HP = pc.HP;
                p3.MaxHP = pc.MaxHP;
                p3.MP = pc.MP;
                p3.MaxMP = pc.MaxMP;
                p3.SP = pc.SP;
                p3.MaxSP = pc.MaxSP;
                NetIo.SendPacket(p3);
            }
        }

        public void SendPartyMemberInfo(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                    try
                    {
                        var i = Character.Party.IndexOf(pc);
                        var p = new SSMG_PARTY_MEMBER_STATE();
                        p.PartyIndex = i;
                        p.CharID = pc.CharID;
                        p.Online = pc.Online;
                        NetIo.SendPacket(p);
                        var p1 = new SSMG_PARTY_MEMBER_POSITION();
                        p1.PartyIndex = i;
                        p1.CharID = pc.CharID;
                        p1.MapID = pc.MapID;
                        p1.X = Global.PosX16to8(pc.X, MapManager.Instance.GetMap(pc.MapID).Width);
                        p1.Y = Global.PosY16to8(pc.Y, MapManager.Instance.GetMap(pc.MapID).Height);
                        NetIo.SendPacket(p1);
                        var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                        p2.PartyIndex = i;
                        p2.CharID = pc.CharID;
                        if (Configuration.Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                        p2.Job = pc.Job;
                        p2.Level = pc.Level;
                        p2.JobLevel = pc.CurrentJobLevel;
                        NetIo.SendPacket(p2);
                        var p3 = new SSMG_PARTY_MEMBER_HPMPSP();
                        p3.PartyIndex = i;
                        p3.CharID = pc.CharID;
                        p3.HP = pc.HP;
                        p3.MaxHP = pc.MaxHP;
                        p3.MP = pc.MP;
                        p3.MaxMP = pc.MaxMP;
                        p3.SP = pc.SP;
                        p3.MaxSP = pc.MaxSP;
                        NetIo.SendPacket(p3);
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }
        }

        private void SendPartyMember()
        {
            if (Character.Party == null)
                return;
            foreach (var i in Character.Party.Members.Keys)
            {
                var p = new SSMG_PARTY_MEMBER();
                p.PartyIndex = i;
                p.CharID = Character.Party[i].CharID;
                p.CharName = Character.Party[i].Name;
                p.Leader = Character.Party.Leader == Character.Party[i];
                NetIo.SendPacket(p);
            }

            var party = Character.Party;
            foreach (var i in party.Members.Keys) SendPartyMemberInfo(party[i]);
        }

        public void OnChat(CSMG_CHAT_PUBLIC p)
        {
            if (!AtCommand.Instance.ProcessCommand(this, p.Content))
            {
                if (p.Content.Substring(0, 1) == "!")
                {
                    if (Character.Account.GMLevel > 100)
                        SendSystemMessage("Command error。");
                    return;
                }

                var arg = new ChatArg();
                arg.content = p.Content;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAT, arg, Character, true);
            }
        }

        public void OnTakeGift(CSMG_CHAT_GIFT_TAKE p)
        {
            MapServer.charDB.GetGifts(Character);
            var GiftID = p.GiftID;
            var Type = p.type;
            if (Type == 0)
            {
                var gift = from G in Character.Gifts
                           where GiftID == G.MailID
                           select G;
                if (gift == null)
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                if (gift.Count() == 0)
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                var Gift = gift.First();
                if (Gift.AccountID != Character.Account.AccountID)
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                if (!MapServer.charDB.DeleteGift(Gift))
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                foreach (var i in Gift.Items.Keys)
                {
                    var ItemID = i;
                    var Count = Gift.Items[i];
                    var item = ItemFactory.Instance.GetItem(ItemID);
                    item.Stack = Count;
                    AddItem(item, true);
                }

                Character.Gifts.Remove(Gift);
            }
            else
            {
                var gift = from G in Character.Gifts
                           where GiftID == G.MailID
                           select G;
                if (gift == null)
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                if (gift.Count() == 0)
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                var Gift = gift.First();
                if (!MapServer.charDB.DeleteGift(Gift))
                {
                    SendSystemMessage("unexpected command");
                    return;
                }

                Character.Gifts.Remove(Gift);
            }

            var p3 = new SSMG_GIFT_TAKERECIPT();
            p3.type = Type;
            p3.MailID = GiftID;
            NetIo.SendPacket(p3);
        }

        public void OnChatParty(CSMG_CHAT_PARTY p)
        {
            if (Character != null)
            {
                if (p.Content.Substring(0, 1) == "!")
                    return;
                PartyManager.Instance.PartyChat(Character.Party, Character, p.Content);
            }
        }

        public void OnExpression(CSMG_CHAT_EXPRESSION p)
        {
            var arg = new ChatArg();
            arg.expression = p.Motion;
            if (p.Loop == 0)
                Character.EMotionLoop = false;
            else
                Character.EMotionLoop = true;
            if (p.Motion <= 4)
                Character.EMotion = p.Motion;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, arg, Character, true);
        }

        public void OnWaitType(CSMG_CHAT_WAITTYPE p)
        {
            Character.WaitType = p.type;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.WAITTYPE, null, Character, true);
        }

        public void OnMotion(CSMG_CHAT_MOTION p)
        {
            //Cancel Cloak
            if (Character.Status.Additions.ContainsKey("Cloaking"))
                SkillHandler.RemoveAddition(Character, "Cloaking");

            var arg = new ChatArg();
            arg.motion = p.Motion;
            arg.loop = p.Loop;
            Character.Motion = arg.motion;
            if (arg.loop == 1)
                Character.MotionLoop = true;
            else
                Character.MotionLoop = false;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, arg, Character, true);

            if ((int)p.Motion == 140 || (int)p.Motion == 141 || (int)p.Motion == 159 || (int)p.Motion == 113 ||
                (int)p.Motion == 210
                || (int)p.Motion == 555 || (int)p.Motion == 556 || (int)p.Motion == 557 || (int)p.Motion == 558 ||
                (int)p.Motion == 559
                || (int)p.Motion == 400)
                if (Character.Partner != null)
                {
                    var partner = Character.Partner;
                    var parg = new ChatArg();
                    parg.motion = p.Motion;
                    parg.loop = 1;
                    partner.Motion = parg.motion;
                    partner.MotionLoop = true;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, partner, true);
                }
        }

        public void OnEmotion(CSMG_CHAT_EMOTION p)
        {
            var arg = new ChatArg();
            arg.emotion = p.Emotion;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.EMOTION, arg, Character, true);
        }

        public void OnSit(CSMG_CHAT_SIT p)
        {
            var arg = new ChatArg();

            if (Character.Motion != MotionType.SIT)
            {
                arg.motion = MotionType.SIT;
                arg.loop = 1;
                Character.Motion = MotionType.SIT;
                Character.MotionLoop = true;
                PartnerTalking(Character.Partner, TALK_EVENT.MASTERSIT, 50, 5000);

                if (Character.Partner != null)
                {
                    var partner = Character.Partner;
                    var parg = new ChatArg();
                    parg.motion = (MotionType)135;
                    parg.loop = 1;
                    partner.Motion = parg.motion;
                    partner.MotionLoop = true;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, partner, true);
                }

                if (!Character.Tasks.ContainsKey("Regeneration"))
                {
                    var reg = new Regeneration(this);
                    Character.Tasks.Add("Regeneration", reg);
                    reg.Activate();
                }
            }
            else
            {
                if (Character.Tasks.ContainsKey("Regeneration"))
                {
                    Character.Tasks["Regeneration"].Deactivate();
                    Character.Tasks.Remove("Regeneration");
                }

                arg.motion = MotionType.STAND;
                arg.loop = 0;
                Character.Motion = MotionType.NONE;
                Character.MotionLoop = false;
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, arg, Character, true);
        }

        public void OnSign(CSMG_CHAT_SIGN p)
        {
            Character.Sign = p.Content;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SIGN_UPDATE, null, Character, true);
        }

        public void SendMotion(MotionType motion, byte loop)
        {
            var arg = new ChatArg();
            arg.motion = motion;
            arg.loop = loop;
            Character.Motion = arg.motion;
            if (arg.loop == 1)
                Character.MotionLoop = true;
            else
                Character.MotionLoop = false;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, arg, Character, true);

            if (arg.loop == 0)
                Character.Motion = (MotionType)111;

            if ((int)arg.motion == 140 || (int)arg.motion == 141 || (int)arg.motion == 159 || (int)arg.motion == 113 ||
                (int)arg.motion == 210
                || (int)arg.motion == 555 || (int)arg.motion == 556 || (int)arg.motion == 557 ||
                (int)arg.motion == 558 || (int)arg.motion == 559
                || (int)arg.motion == 400)
                if (Character.Partner != null)
                {
                    var partner = Character.Partner;
                    var parg = new ChatArg();
                    parg.motion = arg.motion;
                    parg.loop = loop;
                    partner.Motion = parg.motion;
                    partner.MotionLoop = true;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, partner, true);
                }
        }

        public void SendSystemMessage(string content)
        {
            if (Character.Online)
            {
                var p = new SSMG_CHAT_PUBLIC();
                p.ActorID = 0xFFFFFFFF;
                p.Message = content;
                NetIo.SendPacket(p);
            }
        }

        public void SendSystemMessage(SSMG_SYSTEM_MESSAGE.Messages message)
        {
            var p = new SSMG_SYSTEM_MESSAGE();
            p.Message = message;
            NetIo.SendPacket(p);
        }

        public void SendChatParty(string sender, string content)
        {
            var p = new SSMG_CHAT_PARTY();
            p.Sender = sender;
            p.Content = content;
            NetIo.SendPacket(p);
        }

        /// <summary>
        ///     查看目标装备
        /// </summary>
        public void OnPlayerEquipOpen(uint charID)
        {
            var pc = map.GetPC(charID);
            if (pc.Fictitious)
            {
                SendSystemMessage("無法查看已融合的裝備列表。");
                return;
            }

            if (!pc.showEquipment)
            {
                SendSystemMessage("對方已設置不允許查看裝備列表。");
                return;
            }

            FromActorPC(pc).SendSystemMessage(Character.Name + "正在查看你的裝備列表");
            var p1 = new SSMG_PLAYER_EQUIP_START();
            NetIo.SendPacket(p1);
            var p2 = new SSMG_PLAYER_EQUIP_NAME();
            p2.ActorName = pc.Name;
            NetIo.SendPacket(p2);
            foreach (var i in pc.Inventory.Equipments)
            {
                var item = i.Value;
                if (item == null)
                    continue;
                var p3 = new SSMG_PLAYER_EQUIP_INFO();
                p3.InventorySlot = item.Slot;
                p3.Container = pc.Inventory.GetContainerType(item.Slot);
                p3.Item = item;
                NetIo.SendPacket(p3);
            }

            var p4 = new SSMG_PLAYER_EQUIP_END();
            NetIo.SendPacket(p4);
        }

        public void OnPlayerFurnitureSit(CSMG_PLAYER_FURNITURE_SIT p)
        {
            if (p.unknown != -1)
            {
                Character.FurnitureID = p.FurnitureID;
                Character.FurnitureID_old = (uint)p.unknown;
            }
            else
            {
                Character.FurnitureID_old = 255;
                Character.FurnitureID = 255;
            }

            var p1 = new SSMG_PLAYER_FURNITURE_SIT();
            p1.FurnitureID = p.FurnitureID;
            p1.unknown = p.unknown;
            NetIo.SendPacket(p1);

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.FURNITURE_SIT, null, Character, true);
        }

        //#region 商人商店

        //an添加 (MarkChat)
        public void OnPlayerShopOpen(CSMG_PLAYER_SHOP_OPEN p)
        {
            var actor = map.GetActor(p.ActorID); //mark3
            var pc = (ActorPC)actor;
            if (pc.Fictitious)
            {
                var EventID = pc.TInt["虚拟玩家EventID"];
                EventActivate((uint)EventID);
                Character.TInt["触发的虚拟玩家ID"] = (int)pc.ActorID;
            }
            else
            {
                var client = FromActorPC(pc);
                client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.SHOP_OPEN, Character.Name));
                var p1 = new SSMG_PLAYER_SHOP_HEADER2();
                p1.ActorID = p.ActorID;
                NetIo.SendPacket(p1);
                var p2 = new SSMG_PLAYER_SHOP_HEADER();
                p2.ActorID = p.ActorID;
                NetIo.SendPacket(p2);
                foreach (var i in pc.Playershoplist.Keys)
                {
                    var item = pc.Inventory.GetItem(i);
                    if (item == null)
                        continue;
                    var p3 = new SSMG_PLAYER_SHOP_ITEM();
                    p3.InventorySlot = i;
                    p3.Container = ContainerType.BODY;
                    p3.Price = pc.Playershoplist[i].Price;
                    p3.ShopCount = pc.Playershoplist[i].Count;
                    p3.Item = item;
                    NetIo.SendPacket(p3);
                }

                var p4 = new SSMG_PLAYER_SHOP_FOOTER();
                NetIo.SendPacket(p4);
            }
        }

        public void OnPlayerSetShopSetup(CSMG_PLAYER_SETSHOP_SETUP p) //mark11
        {
            if (p.Comment.Length < 1)
            {
                SendSystemMessage("输入商店名称");
                return;
            }

            var ids = p.InventoryIDs;
            var counts = p.Counts;
            var prices = p.Prices;
            Shoptitle = p.Comment;
            try
            {
                if (ids.Length != 0)
                {
                    if (Character.Playershoplist != null)
                        Character.Playershoplist.Clear();
                    for (var i = 0; i < ids.Length; i++)
                    {
                        if (!Character.Playershoplist.ContainsKey(ids[i]))
                        {
                            var item = new PlayerShopItem();
                            item.InventoryID = ids[i];
                            item.ItemID = Character.Inventory.GetItem(ids[i]).ItemID;
                            //if(ItemFactory.Instance.GetItem(item.ItemID).BaseData.itemType != ItemType.FACECHANGE
                            //&& item.ItemID != 950000005 && item.ItemID != 100000000 && item.ItemID != 110128500 && item.ItemID != 110132000 && item.ItemID != 110165300)

                            var item2 = Character.Inventory.GetItem(ids[i]);
                            if (item2.BaseData.itemType == ItemType.IRIS_CARD)
                            {
                                SendSystemMessage("卡片物品：【" + item2.BaseData.name + "】目前无法上架交易。");
                                continue;
                            }

                            if ((item2.EquipSlot.Count < 1 || item2.BaseData.itemType == ItemType.PET ||
                                 item2.BaseData.itemType == ItemType.PARTNER
                                 || item2.BaseData.itemType == ItemType.RIDE_PET ||
                                 item2.BaseData.itemType == ItemType.RIDE_PARTNER) &&
                                item2.BaseData.itemType != ItemType.FURNITURE
                                && item2.BaseData.itemType != ItemType.FG_GARDEN_MODELHOUSE &&
                                item2.BaseData.itemType != ItemType.FG_GARDEN_FLOOR &&
                                item2.BaseData.itemType != ItemType.FG_ROOM_FLOOR
                                && item2.BaseData.itemType != ItemType.FG_FLYING_SAIL &&
                                item2.BaseData.itemType != ItemType.FG_ROOM_WALL && Character.Account.GMLevel < 200)
                            {
                                SendSystemMessage("无法上架的物品：【" + item2.BaseData.name + "】目前无法上架交易。");
                                continue;
                            }

                            if (item2.Refine > 30)
                            {
                                SendSystemMessage("无法强化大于等于31的物品：【" + item2.BaseData.name + "】");
                                continue;
                            }

                            Character.Playershoplist.Add(ids[i], item);
                        }

                        if (counts[i] == 0)
                        {
                            Character.Playershoplist.Remove(ids[i]);
                        }
                        else
                        {
                            Character.Playershoplist[ids[i]].Count = counts[i];
                            Character.Playershoplist[ids[i]].Price = prices[i];
                            SendShopGoodInfo(ids[i], counts[i], prices[i]);
                        }
                    }
                }
                else
                {
                    Character.Playershoplist.Clear();
                }
            }
            catch (Exception ex1)
            {
                Logger.ShowError(ex1);
            }

            var p1 = new SSMG_PLAYER_SHOP_APPEAR();
            p1.ActorID = Character.ActorID;
            p1.Title = Shoptitle;
            if (ids.Length != 0 && Shoptitle != "" && Character.Playershoplist.Count > 0)
            {
                Shopswitch = 1;
                p1.button = 1;
            }
            else
            {
                Shopswitch = 0;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE, null, Character, true);
                p1.button = 0;
            }

            NetIo.SendPacket(p1);
        }

        public void SendShopGoodInfo(uint slotid, ushort count, ulong gold)
        {
            var p = new SSMG_PLAYER_SHOP_GOLD_UPDATA();
            p.SlotID = slotid;
            p.Count = count;
            p.gold = gold;
            NetIo.SendPacket(p);
        }

        public void OnPlayerShopSellBuy(CSMG_PLAYER_SHOP_SELL_BUY p)
        {
            var actor = map.GetActor(p.ActorID);
            var items = p.Items;
            var p1 = new SSMG_PLAYER_SHOP_ANSWER();
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                var client = FromActorPC(pc);
                long gold = 0;
                //if (client.Character.Account.MacAddress == Character.Account.MacAddress && client.account.GMLevel < 200)
                //{
                //    MapClient clients = FromActorPC(Character);
                //    clients.SendSystemMessage("发生未知的错误，交易失败。");
                //    return;
                //}
                foreach (var i in items.Keys)
                {
                    var item = pc.Inventory.GetItem(i);
                    /*GAME_SMSG_STALL_VEN_DEAL_ERR1,"露店を見ていないのに取引しようとしました"
                        GAME_SMSG_STALL_VEN_DEAL_ERR2,"露店を見ている最中に相手が変わりました"
                        GAME_SMSG_STALL_VEN_DEAL_ERR3,"指定した人は商人ではありません"
                        GAME_SMSG_STALL_VEN_DEAL_ERR4,"露店を開いていません"
                        GAME_SMSG_STALL_VEN_DEAL_ERR5,"露店の客として認識されていませんでした"
                        GAME_SMSG_STALL_VEN_DEAL_ERR6,"指定した露店は自分の露店です"
                        GAME_SMSG_STALL_VEN_DEAL_ERR7,"取引可能な商品がありませんでした"
                        GAME_SMSG_STALL_VEN_DEAL_ERR8,"現在の所持金が取引金額に達していませんでした"
                        GAME_SMSG_STALL_VEN_DEAL_ERR9,"これ以上アイテムを所持することはできません"
                        GAME_SMSG_STALL_VEN_DEAL_ERR10,"相手の所持金が上限に達したためキャンセルされました"*/
                    if (item == null)
                    {
                        p1.Result = -4;
                        NetIo.SendPacket(p1);
                        return;
                    }

                    if (item.ItemID == 950000006 || item.ItemID == 950000007)
                    {
                        p1.Result = -1;
                        NetIo.SendPacket(p1);
                        return;
                    }

                    if (items[i] == 0)
                    {
                        p1.Result = -2;
                        NetIo.SendPacket(p1);
                        return;
                    }

                    /*if (item.IsEquipt)
                    {
                        p1.Result = -4;
                        this.NetIo.SendPacket(p1);
                        return;
                    }*/
                    if (item.Stack >= items[i])
                    {
                        gold += (long)(pc.Playershoplist[i].Price * items[i]);
                        var singleprice = (long)(pc.Playershoplist[i].Price * items[i]);
                        if (Character.Gold < gold)
                        {
                            p1.Result = -7;
                            NetIo.SendPacket(p1);
                            return;
                        }

                        if (gold + pc.Gold >= 999999999999)
                        {
                            p1.Result = -9;
                            NetIo.SendPacket(p1);
                            return;
                        }

                        uint cpfee = 0; //(uint)(100 + singleprice * 0.01f);
                        /*if(client.Character.CP < cpfee)
                        {
                            client.SendSystemMessage("玩家: " + this.Character.Name + " 试图向您购买物品，可是您的CP不足，无法贩卖。");
                            this.SendSystemMessage("无法向玩家: " + client.Character.Name + " 购买物品，因为他的CP不足了。");
                            return;
                        }
                        //client.Character.CP -= cpfee;*/

                        var newItem = item.Clone();
                        newItem.Stack = items[i];
                        if (newItem.Stack > 0)
                            Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                                Character.Name + "(" + Character.CharID + ")",
                                newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                string.Format("GolemSell Count:{0}", items[i]), false);
                        var result = pc.Inventory.DeleteItem(i, items[i]);
                        pc.Playershoplist[i].Count -= items[i];

                        SendShopGoodInfo(i, pc.Playershoplist[i].Count, pc.Playershoplist[i].Price);

                        if (pc.Playershoplist[i].Count == 0)
                            pc.Playershoplist.Remove(i);
                        //返回卖家info
                        switch (result)
                        {
                            case InventoryDeleteResult.STACK_UPDATED:
                                var p2 = new SSMG_ITEM_COUNT_UPDATE();
                                p2.InventorySlot = item.Slot;
                                p2.Stack = item.Stack;
                                client.NetIo.SendPacket(p2);
                                break;
                            case InventoryDeleteResult.ALL_DELETED:
                                var p3 = new SSMG_ITEM_DELETE();
                                p3.InventorySlot = item.Slot;
                                client.NetIo.SendPacket(p3);
                                break;
                        }


                        client.Character.Inventory.CalcPayloadVolume();
                        client.SendCapacity();
                        client.SendSystemMessage("玩家: " + Character.Name + " 向您购买了 " + newItem.Stack + " 个 [" +
                                                 newItem.BaseData.name + "]，售价：" + singleprice + "G");
                        client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_DELETED,
                            item.BaseData.name, items[i]));
                        Logger.LogItemGet(Logger.EventType.ItemGolemGet, Character.Name + "(" + Character.CharID + ")",
                            item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("GolemBuy Count:{0}", item.Stack), false);
                        SendSystemMessage("向玩家: " + client.Character.Name + " 购买了 " + newItem.Stack + " 个 [" +
                                          newItem.BaseData.name + "]，花费：" + singleprice + "G");
                        if (newItem.BaseData.itemType == ItemType.PARTNER)
                            newItem.ActorPartnerID = 0;
                        AddItem(newItem, true);


                        var log = new Logger("玩家交易记录.txt");
                        var text = "\r\n玩家: " + Character.Name + " 向玩家：" + client.Character.Name + " 购买了 " +
                                   newItem.Stack + " 个 [" + newItem.BaseData.name + "]，花费：" + singleprice + "G";
                        text += "\r\n买家IP/MAC：" + Character.Account.LastIP + "/" + Character.Account.MacAddress +
                                "   卖家IP/MAC：" + client.Character.Account.LastIP + "/" +
                                client.Character.Account.MacAddress;
                        if (newItem.Refine > 10)
                            text += "\r\n装备道具：" + newItem.BaseData.name + " 强化次数" + newItem.Refine;
                        log.WriteLog(text);


                        if (Character.Account.MacAddress == client.Character.Account.MacAddress ||
                            Character.Account.LastIP == client.Character.Account.LastIP)
                        {
                            var log2 = new Logger("同IP或MAC的玩家交易记录.txt");
                            var text2 = "\r\n玩家: " + Character.Name + " 向玩家：" + client.Character.Name + " 购买了 " +
                                        newItem.Stack + " 个 [" + newItem.BaseData.name + "]，花费：" + singleprice + "G";
                            text2 += "\r\n买家IP/MAC：" + Character.Account.LastIP + "/" + Character.Account.MacAddress +
                                     "   卖家IP/MAC：" + client.Character.Account.LastIP + "/" +
                                     client.Character.Account.MacAddress;
                            if (newItem.Refine > 10)
                                text2 += "\r\n装备道具：" + newItem.BaseData.name + " 强化次数" + newItem.Refine;
                            log2.WriteLog(text2);
                        }
                    }
                    else
                    {
                        p1.Result = -5;
                        NetIo.SendPacket(p1);
                        return;
                    }
                }

                Character.Gold -= gold;
                pc.Gold += gold;

                if (pc.Playershoplist.Count == 0)
                {
                    pc.Fictitious = false;
                    client.Shopswitch = 0;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, pc, true);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE, null, Character,
                        true);
                }
            }
        }

        public void OnPlayerShopBuyClose(CSMG_PLAYER_SETSHOP_CLOSE p)
        {
            //this.Shopswitch = 0;
            //this.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE, null, this.Character, true);
            //Packets.Server.SSMG_PLAYER_SETSHOP_SET p1 = new SagaMap.Packets.Server.SSMG_PLAYER_SETSHOP_SET();
            //this.NetIo.SendPacket(p1);

            var p1 = new SSMG_PLAYER_SHOP_CLOSE();
            // Reason is positive int
            /*
             GAME_SMSG_STALL_VEN_CLOSE_ERR1,"露店商の商品は売り切れました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR2,"露店商が商品の再設定をしています"
            GAME_SMSG_STALL_VEN_CLOSE_ERR3,"露店商が遠く離れました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR4,"露店商がマップ移動しました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR5,"露店商がトレードを始めました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR6,"露店商が憑依しました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR8,"露店商が行動不能状態となりました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR9,"露店商がいなくなりました"
            GAME_SMSG_STALL_VEN_CLOSE_ERR10,"露店商がイベントを始めました"
            */
            NetIo.SendPacket(p1);
        }

        public void OnPlayerShopChangeClose(CSMG_PLAYER_SETSHOP_OPEN p)
        {
            Character.Playershoplist.Clear();
            Character.Fictitious = false;
            Shopswitch = 0;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE, null, Character, true);
        }

        public void OnPlayerShopChange(CSMG_PLAYER_SETSHOP_SETUP p)
        {
            Shoptitle = p.Comment;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE, null, Character, true);
            if (Shopswitch == 0 && Shoptitle == "")
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYERSHOP_CHANGE_CLOSE, null, Character, true);
        }

        public void OnPlayerSetShop(CSMG_PLAYER_SETSHOP_OPEN p)
        {
            var p1 = new SSMG_PLAYER_SETSHOP_OPEN_SETUP();
            var p2 = new SSMG_PLAYER_SHOP_APPEAR();
            p2.ActorID = Character.ActorID;
            p2.Title = Shoptitle;
            Shopswitch = 0;
            p2.button = 0;
            p1.Comment = Shoptitle;
            NetIo.SendPacket(p1);
            NetIo.SendPacket(p2);
        }

        //#endregion

        public void OnGroupJoin()
        {
        }

        public void OnGroupMemberJoin()
        {
        }

        public void OnGroupMemberKick()
        {
        }

        public void OnGroupLeave()
        {
            if (Character.Party.Leader == Character)
            {
                var p2 = new SSMG_AAA_GROUP_DESTROY();
                NetIo.SendPacket(p2);
            }
        }

        public void OnGroupSelect()
        {
        }

        public void OnGroupUpdate()
        {
        }

        public void OnGroupChangeState()
        {
        }

        public void OnGroupStart()
        {
        }

        public void OnGroupRestart()
        {
        }

        public bool changeDualJob;

        public void OnDualChangeRequest(CSMG_DUALJOB_CHANGE_CONFIRM p)
        {
            Character.DualJobID = p.DualJobID;

            if (!Character.PlayerDualJobList.ContainsKey(Character.DualJobID))
            {
                var dualjobinfo = new PlayerDualJobInfo();
                dualjobinfo.DualJobExp = 0;
                dualjobinfo.DualJobID = Character.DualJobID;
                dualjobinfo.DualJobLevel = 1;
                Character.PlayerDualJobList.Add(Character.DualJobID, dualjobinfo);
            }

            if (Character.PlayerDualJobList[Character.DualJobID].DualJobLevel <= 0)
            {
                Character.PlayerDualJobList[Character.DualJobID].DualJobLevel = 1;
                Character.PlayerDualJobList[Character.DualJobID].DualJobExp = 0;
            }

            var skills = new List<SagaDB.Skill.Skill>();
            var ids = p.DualJobSkillList;
            foreach (var item in ids)
                if (item != 0)
                {
                    var sks = DualJobSkillFactory.Instance.items[Character.DualJobID].Where(x =>
                        x.DualJobID == Character.DualJobID && x.SkillID == item &&
                        x.LearnSkillLevel.Where(y => y <= Character.DualJobLevel).Count() > 0).FirstOrDefault();
                    if (sks != null)
                    {
                        var sk = DualJobSkillFactory.Instance.items[Character.DualJobID]
                            .FirstOrDefault(x => x.SkillID == item);
                        var lv = sk.LearnSkillLevel.Count(x => x > 0 && x <= Character.DualJobLevel);
                        skills.Add(SkillFactory.Instance.GetSkill(item, (byte)lv));
                    }
                }

            Character.DualJobSkill = skills;
            Character.DualJobLevel = Character.PlayerDualJobList[Character.DualJobID].DualJobLevel;

            MapServer.charDB.SaveDualJobInfo(Character, true);

            SendPlayerInfo();
            SendPlayerDualJobSkillList();

            SkillHandler.Instance.CastPassiveSkills(Character);

            changeDualJob = false;

            var pi = new SSMG_DUALJOB_SET_DUALJOB_INFO();
            pi.Result = true;
            pi.RetType = 0x00;
            NetIo.SendPacket(pi);
        }


        public void SendPlayerDualJobInfo()
        {
            var p2 = new SSMG_DUALJOB_INFO_SEND();
            p2.JobList = new byte[25]
            {
                0xC, 0x0, 0x1, 0x0, 0x2, 0x0, 0x3, 0x0, 0x4, 0x0, 0x5, 0x0, 0x6, 0x0, 0x7, 0x0, 0x8, 0x0, 0x9, 0x0, 0xa,
                0x0, 0xb, 0x0, 0xc
            };
            var levels = new byte[13];
            levels[0] = 0x0C;
            for (byte i = 1; i <= 0x0C; i++)
                if (Character.PlayerDualJobList.ContainsKey(i))
                    levels[i] = Character.PlayerDualJobList[i].DualJobLevel;
                else
                    levels[i] = 0;
            p2.JobLevel = levels;
            NetIo.SendPacket(p2);
        }

        public void SendPlayerDualJobSkillList()
        {
            var p1 = new SSMG_DUALJOB_SKILL_SEND();
            p1.Skills = Character.DualJobSkill;
            p1.SkillLevels = Character.DualJobSkill;

            NetIo.SendPacket(p1);
        }

        public void OnDualJobWindowClose()
        {
            changeDualJob = false;
        }

        /// <summary>
        ///     打开副职转职窗口
        /// </summary>
        /// <param name="pc">角色对象</param>
        /// <param name="ChangeDualJob">是否允许更改副职系统(是否为习得副职系统)</param>
        public void OpenDualJobChangeUI(ActorPC pc, bool ChangeDualJob)
        {
            var p = new SSMG_DUALJOB_WINDOW_OPEN();
            if (ChangeDualJob)
                p.CanChange = 0x01;
            else
                p.CanChange = 0x00;

            p.SetDualJobList(0x0C,
                new byte[]
                {
                    0x0, 0x1, 0x0, 0x2, 0x0, 0x3, 0x0, 0x4, 0x0, 0x5, 0x0, 0x6, 0x0, 0x7, 0x0, 0x8, 0x0, 0x9, 0x0, 0xa,
                    0x0, 0xb, 0x0, 0xc
                });

            var dualjoblevel = new byte[12];
            for (var i = 0; i < dualjoblevel.Length; i++)
                if (pc.PlayerDualJobList.ContainsKey(byte.Parse((i + 1).ToString())))
                    dualjoblevel[i] = pc.PlayerDualJobList[byte.Parse((i + 1).ToString())].DualJobLevel;
                else
                    dualjoblevel[i] = 0;
            p.DualJobLevel = dualjoblevel;
            p.CurrentDualJobSerial = pc.DualJobID;
            if (ChangeDualJob)
                p.CurrentSkillList = pc.DualJobSkill;
            else
                p.CurrentSkillList = new List<SagaDB.Skill.Skill>();

            NetIo.SendPacket(p);
        }

        public void OnPossessionRequest(CSMG_POSSESSION_REQUEST p)
        {
            var target = (ActorPC)Map.GetActor(p.ActorID);
            var pos = p.PossessionPosition;
            var result = TestPossesionPosition(target, pos);
            if (result >= 0)
            {
                Character.Buff.GetReadyPossession = true;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                var reduce = 0;
                if (Character.Status.Additions.ContainsKey("TranceSpdUp"))
                {
                    var passive = (DefaultPassiveSkill)Character.Status.Additions["TranceSpdUp"];
                    reduce = passive["TranceSpdUp"];
                }

                var task = new Possession(this, target, pos, p.Comment, reduce);
                Character.Tasks.Add("Possession", task);
                task.Activate();
            }
            else
            {
                var p1 = new SSMG_POSSESSION_RESULT();
                p1.FromID = Character.ActorID;
                p1.ToID = 0xFFFFFFFF;
                p1.Result = result;
                NetIo.SendPacket(p1);
            }
        }

        public void OnPossessionCancel(CSMG_POSSESSION_CANCEL p)
        {
            var pos = p.PossessionPosition;
            switch (pos)
            {
                case PossessionPosition.NONE:
                    var actor = Map.GetActor(Character.PossessionTarget);
                    if (actor == null)
                        return;
                    var arg = new PossessionArg();
                    arg.fromID = Character.ActorID;
                    arg.cancel = true;
                    arg.result = (int)Character.PossessionPosition;
                    arg.x = Global.PosX16to8(Character.X, Map.Width);
                    arg.y = Global.PosY16to8(Character.Y, Map.Height);
                    arg.dir = (byte)(Character.Dir / 45);
                    if (actor.type == ActorType.ITEM)
                    {
                        var item = GetPossessionItem(Character, Character.PossessionPosition);
                        item.PossessionedActor = null;
                        item.PossessionOwner = null;
                        Character.PossessionTarget = 0;
                        Character.PossessionPosition = PossessionPosition.NONE;
                        arg.toID = 0xFFFFFFFF;
                        Map.DeleteActor(actor);
                    }
                    else if (actor.type == ActorType.PC)
                    {
                        var pc = (ActorPC)actor;
                        arg.toID = pc.ActorID;
                        var item = GetPossessionItem(pc, Character.PossessionPosition);
                        if (item.PossessionOwner != Character)
                        {
                            item.PossessionedActor = null;
                            Character.PossessionTarget = 0;
                            Character.PossessionPosition = PossessionPosition.NONE;
                        }
                        else
                        {
                            var item2 = GetPossessionItem(Character, Character.PossessionPosition);
                            item2.PossessionedActor = null;
                            item2.PossessionOwner = null;
                            Character.PossessionTarget = 0;
                            Character.PossessionPosition = PossessionPosition.NONE;
                            var p3 = new CSMG_ITEM_MOVE();
                            p3.data = new byte[9];
                            p3.InventoryID = item.Slot;
                            p3.Target = ContainerType.BODY;
                            p3.Count = 1;
                            FromActorPC(pc).OnItemMove(p3, true);
                            pc.Inventory.DeleteItem(item.Slot, 1);

                            var p2 = new SSMG_ITEM_DELETE();
                            p2.InventorySlot = item.Slot;
                            ((PCEventHandler)pc.e).Client.NetIo.SendPacket(p2);
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, pc, true);
                        }

                        item.PossessionedActor = null;
                        item.PossessionOwner = null;

                        StatusFactory.Instance.CalcStatus(Character);
                        SendPlayerInfo();
                        StatusFactory.Instance.CalcStatus(pc);
                        ((PCEventHandler)pc.e).Client.SendPlayerInfo();
                    }

                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, Character, true);
                    break;
                default:
                    var item3 = GetPossessionItem(Character, pos);
                    if (item3 == null)
                        return;
                    if (item3.PossessionedActor == null)
                        return;
                    var arg2 = new PossessionArg();
                    arg2.fromID = item3.PossessionedActor.ActorID;
                    arg2.toID = Character.ActorID;
                    arg2.cancel = true;
                    arg2.result = (int)item3.PossessionedActor.PossessionPosition;
                    arg2.x = Global.PosX16to8(Character.X, Map.Width);
                    arg2.y = Global.PosY16to8(Character.Y, Map.Height);
                    arg2.dir = (byte)(Character.Dir / 45);


                    if (item3.PossessionOwner != Character && item3.PossessionOwner != null)
                    {
                        var item4 = GetPossessionItem(item3.PossessionedActor,
                            item3.PossessionedActor.PossessionPosition);
                        if (item4 != null)
                        {
                            item4.PossessionedActor = null;
                            item4.PossessionOwner = null;
                        }

                        var p3 = new CSMG_ITEM_MOVE();
                        p3.data = new byte[9];
                        p3.InventoryID = item3.Slot;
                        p3.Target = ContainerType.BODY;
                        p3.Count = 1;
                        OnItemMove(p3, true);
                        Character.Inventory.DeleteItem(item3.Slot, 1);

                        var p2 = new SSMG_ITEM_DELETE();
                        p2.InventorySlot = item3.Slot;
                        NetIo.SendPacket(p2);
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);

                        Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg2, Character, true);
                        if (((PCEventHandler)item3.PossessionedActor.e).Client.state == SESSION_STATE.DISCONNECTED)
                        {
                            var itemactor = PossessionItemAdd(item3.PossessionedActor,
                                item3.PossessionedActor.PossessionPosition, "");
                            item3.PossessionedActor.PossessionTarget = itemactor.ActorID;
                            MapServer.charDB.SaveChar(item3.PossessionedActor, false, false);
                            MapServer.accountDB.WriteUser(item3.PossessionedActor.Account);
                            return;
                        }
                    }
                    else
                    {
                        var actor2 = map.GetActor(Character.PossessionTarget);
                        if (actor2 != null)
                        {
                            if (actor2.type == ActorType.ITEM)
                                map.DeleteActor(actor2);
                            if (!item3.PossessionedActor.Online) arg2.fromID = 0xFFFFFFFF;
                        }

                        Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg2, Character, true);
                    }

                    item3.PossessionedActor.PossessionTarget = 0;
                    item3.PossessionedActor.PossessionPosition = PossessionPosition.NONE;
                    item3.PossessionedActor = null;
                    item3.PossessionOwner = null;
                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();
                    break;
            }
        }

        public void PossessionPerform(ActorPC target, PossessionPosition position, string comment)
        {
            var result = TestPossesionPosition(target, position);
            if (result >= 0)
            {
                var arg = new PossessionArg();
                arg.fromID = Character.ActorID;
                arg.toID = target.ActorID;
                arg.result = result;
                arg.comment = comment;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, Character, true);

                var pos = "";
                switch (position)
                {
                    case PossessionPosition.RIGHT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_RIGHT;
                        break;
                    case PossessionPosition.LEFT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_LEFT;
                        break;
                    case PossessionPosition.NECK:
                        pos = LocalManager.Instance.Strings.POSSESSION_NECK;
                        break;
                    case PossessionPosition.CHEST:
                        pos = LocalManager.Instance.Strings.POSSESSION_ARMOR;
                        break;
                }

                SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                if (target == Character)
                {
                    Character.PossessionTarget = PossessionItemAdd(Character, position, comment).ActorID;
                    Character.PossessionPosition = position;
                }
                else
                {
                    FromActorPC(target)
                        .SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                    Character.PossessionTarget = target.ActorID;
                    Character.PossessionPosition = position;
                    var item = GetPossessionItem(target, position);
                    item.PossessionedActor = Character;
                }

                if (!Character.Tasks.ContainsKey("PossessionRecover"))
                {
                    var task = new PossessionRecover(this);
                    Character.Tasks.Add("PossessionRecover", task);
                    task.Activate();
                }

                SkillHandler.Instance.CastPassiveSkills(Character);
                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
                StatusFactory.Instance.CalcStatus(target);
                ((PCEventHandler)target.e).Client.SendPlayerInfo();
            }
            else
            {
                var p1 = new SSMG_POSSESSION_RESULT();
                p1.FromID = Character.ActorID;
                p1.ToID = 0xFFFFFFFF;
                p1.Result = result;
                NetIo.SendPacket(p1);
            }
        }

        private int TestPossesionPosition(ActorPC target, PossessionPosition pos)
        {
            Item item = null;
            if (Character.PossessionTarget != 0)
                return -1; //憑依失敗 : 憑依中です
            if (Character.PossesionedActors.Count != 0)
                return -2; //憑依失敗 : 宿主です
            if (target.type != ActorType.PC)
                return -3; //憑依失敗 : プレイヤーのみ憑依可能です
            var targetPC = target;
            //if (Math.Abs(target.Level - this.Character.Level) > 30)
            //    return -4; //憑依失敗 : レベルが離れすぎです
            switch (pos)
            {
                case PossessionPosition.CHEST:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.UPPER_BODY];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.LEFT_HAND:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.NECK:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE];
                    else
                        return -5; //憑依失敗 : 装備がありません
                    break;
                case PossessionPosition.RIGHT_HAND:
                    if (targetPC.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        if (targetPC.Buff.FishingState) return -15;
                        item = targetPC.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                    }
                    else
                    {
                        return -5; //憑依失敗 : 装備がありません
                    }

                    break;
            }

            if (item == null)
                return -5; //憑依失敗 : 装備がありません
            if (item.Stack == 0)
                return -5; ////憑依失敗 : 装備がありません
            if (item.PossessionedActor != null)
                return -6; //憑依失敗 : 誰かが憑依しています
            if (item.BaseData.itemType == ItemType.CARD || item.BaseData.itemType == ItemType.THROW ||
                item.BaseData.itemType == ItemType.ARROW || item.BaseData.itemType == ItemType.BULLET)
                return -7; //憑依失敗 : 憑依不可能なアイテムです
            if (targetPC.PossesionedActors.Count >= 3)
                return -8; //憑依失敗 : 満員宿主です
            if (Character.Marionette != null || targetPC.Marionette != null
                                             || Character.Buff.Confused || Character.Buff.Frosen ||
                                             Character.Buff.Paralysis
                                             || Character.Buff.Sleep || Character.Buff.Stone || Character.Buff.Stun)
                return -15; //憑依失敗 : 状態異常中です
            if (targetPC.PossessionTarget != 0)
                return -16; //憑依失敗 : 相手は憑依中です
            if (target.Buff.GetReadyPossession)
                return -17; //憑依失敗 : 相手はGetReadyPossession中です
            if (target.Buff.Dead)
                return -18; //憑依失敗 : 相手は行動不能状態です
            if (scriptThread != null || ((PCEventHandler)target.e).Client.scriptThread != null ||
                target.Buff.FishingState)
                return -19; //憑依失敗 : イベント中です
            if (Character.Tasks.ContainsKey("ItemCast"))
                return -19; //憑依失敗 : イベント中です
            if (Character.MapID != target.MapID)
                return -20; //憑依失敗 : 相手とマップが違います
            if (Math.Abs(target.X - Character.X) > 300 || Math.Abs(target.Y - Character.Y) > 300)
                return -21; //憑依失敗 : 相手と離れすぎています
            if (!target.canPossession)
                return -22; //憑依失敗 : 相手が憑依不許可設定中です
            if (Character.Pet != null)
                if (Character.Pet.Ride)
                    return -27; //憑依失敗 : 騎乗中です
            if (Character.Buff.Dead)
                return -29; //憑依失敗: 憑依できない状態です
            if (Character.Race == PC_RACE.DEM)
                return -29; //憑依失敗 : 憑依できない状態です
            if (targetPC.Race == PC_RACE.DEM && targetPC.Form == DEM_FORM.MACHINA_FORM)
                return -31; //憑依失敗 : マシナフォームのＤＥＭキャラクターに憑依することはできません
            /*
            if (this.Character.Buff.GetReadyPossession == true || this.Character.PossessionTarget != 0)
                return -100; //憑依失敗 : 何らかの原因で出来ませんでした
            */
            return (int)pos;
        }

        private Item GetPossessionItem(ActorPC target, PossessionPosition pos)
        {
            Item item = null;
            switch (pos)
            {
                case PossessionPosition.CHEST:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        item = target.Inventory.Equipments[EnumEquipSlot.UPPER_BODY];
                    break;
                case PossessionPosition.LEFT_HAND:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        item = target.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                    break;
                case PossessionPosition.NECK:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        item = target.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE];
                    break;
                case PossessionPosition.RIGHT_HAND:
                    if (target.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        item = target.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                    break;
            }

            return item;
        }

        private ActorItem PossessionItemAdd(ActorPC target, PossessionPosition position, string comment)
        {
            var itemDroped = GetPossessionItem(target, position);
            if (itemDroped == null) return null;
            itemDroped.PossessionedActor = target;
            itemDroped.PossessionOwner = target;
            var actor = new ActorItem(itemDroped);
            actor.e = new ItemEventHandler(actor);
            actor.MapID = target.MapID;
            actor.X = target.X;
            actor.Y = target.Y;
            actor.Comment = comment;
            Map.RegisterActor(actor);
            actor.invisble = false;
            Map.OnActorVisibilityChange(actor);
            return actor;
        }

        private ActorPC GetPossessionTarget()
        {
            if (Character.PossessionTarget == 0)
                return null;
            var actor = Map.GetActor(Character.PossessionTarget);
            if (actor == null)
                return null;
            if (actor.type != ActorType.PC)
                return null;
            return (ActorPC)actor;
        }

        private void PossessionPrepareCancel()
        {
            if (Character.Buff.GetReadyPossession)
            {
                Character.Buff.GetReadyPossession = false;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                if (Character.Tasks.ContainsKey("Possession"))
                {
                    Character.Tasks["Possession"].Deactivate();
                    Character.Tasks.Remove("Possession");
                }
            }
        }

        public void OnPossessionCatalogRequest(CSMG_POSSESSION_CATALOG_REQUEST p)
        {
            var list = new List<ActorItem>();
            foreach (var actor in map.Actors.Values)
                if (actor is ActorItem)
                {
                    var item = (ActorItem)actor;
                    if (item.Item.PossessionedActor.PossessionPosition == p.Position)
                        list.Add(item);
                }

            var pageSize = 5;
            var skip = pageSize * (p.Page - 1);
            var items = list.Select(x => x)
                .Skip(skip)
                .Take(pageSize)
                .ToArray();
            for (var i = 0; i < items.Length; i++)
            {
                var p1 = new SSMG_POSSESSION_CATALOG();
                p1.ActorID = items[i].ActorID;
                p1.comment = items[i].Comment;
                p1.Index = (uint)i + 1;
                p1.Item = items[i].Item;
                NetIo.SendPacket(p1);
            }

            var p2 = new SSMG_POSSESSION_CATALOG_END();
            p2.Page = p.Page;
            NetIo.SendPacket(p2);
        }

        public void OnPossessionCatalogItemInfoRequest(CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST p)
        {
            var target = map.GetActor(p.ActorID);
            if (target != null)
                if (target is ActorItem)
                {
                    var item = (ActorItem)target;
                    var p2 = new SSMG_POSSESSION_CATALOG_ITEM_INFO();
                    p2.ActorID = item.ActorID;
                    p2.ItemID = item.Item.ItemID;
                    p2.Level = item.Item.BaseData.possibleLv;
                    p2.X = Global.PosX16to8(item.X, map.Width);
                    p2.Y = Global.PosY16to8(item.Y, map.Height);
                    NetIo.SendPacket(p2);
                }
        }

        public void OnPartnerPossessionRequest(CSMG_POSSESSION_PARTNER_REQUEST p)
        {
            var partneritem = Character.Inventory.GetItem(p.InventorySlot);
            if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                if (partneritem == Character.Inventory.Equipments[EnumEquipSlot.PET])
                    return;
            var partner = Character.Partner;
            if (partner == null) return;
            var Pict = partneritem.BaseData.petID;
            if (Pict == partner.BaseData.pictid) return;
            if (partneritem != null)
            {
                switch (p.PossessionPosition)
                {
                    case PossessionPosition.RIGHT_HAND:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        {
                            Character.PossessionPartnerSlotIDinRightHand = Pict;
                            Character.Status.max_atk1_petpy = (short)(partner.Status.max_atk1 / 2);
                            Character.Status.min_atk1_petpy = (short)(partner.Status.min_atk1 / 2);
                            Character.Status.max_matk_petpy = (short)(partner.Status.max_matk / 2);
                            Character.Status.min_matk_petpy = (short)(partner.Status.min_matk / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinRightHand = 0;
                            Character.Status.max_atk1_petpy = 0;
                            Character.Status.min_atk1_petpy = 0;
                            Character.Status.max_matk_petpy = 0;
                            Character.Status.min_matk_petpy = 0;
                        }

                        break;
                    case PossessionPosition.LEFT_HAND:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        {
                            Character.PossessionPartnerSlotIDinLeftHand = Pict;
                            Character.Status.def_add_petpy = (short)(partner.Status.def_add / 2);
                            Character.Status.mdef_add_petpy = (short)(partner.Status.mdef_add / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinLeftHand = 0;
                            Character.Status.def_add_petpy = 0;
                            Character.Status.mdef_add_petpy = 0;
                        }

                        break;
                    case PossessionPosition.NECK:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                        {
                            Character.PossessionPartnerSlotIDinAccesory = Pict;
                            Character.Status.aspd_petpy = (short)(partner.Status.aspd / 2);
                            Character.Status.cspd_petpy = (short)(partner.Status.cspd / 2);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinAccesory = 0;
                            Character.Status.aspd_petpy = 0;
                            Character.Status.cspd_petpy = 0;
                        }

                        break;
                    case PossessionPosition.CHEST:
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                        {
                            Character.PossessionPartnerSlotIDinClothes = Pict;
                            Character.Status.hp_petpy = (short)(partner.MaxHP / 20);
                        }
                        else
                        {
                            Character.PossessionPartnerSlotIDinClothes = 0;
                            Character.Status.hp_petpy = 0;
                        }

                        break;
                }

                StatusFactory.Instance.CalcStatus(Character);
                var p1 = new SSMG_POSSESSION_PARTNER_RESULT();
                p1.InventorySlot = p.InventorySlot;
                p1.Pos = p.PossessionPosition;
                NetIo.SendPacket(p1);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
            }
        }

        public void OnPartnerPossessionCancel(CSMG_POSSESSION_PARTNER_CANCEL p)
        {
            var p1 = new SSMG_POSSESSION_PARTNER_CANCEL();
            NetIo.SendPacket(p1);
            switch (p.PossessionPosition)
            {
                case PossessionPosition.RIGHT_HAND:
                    Character.PossessionPartnerSlotIDinRightHand = 0;
                    break;
                case PossessionPosition.LEFT_HAND:
                    Character.PossessionPartnerSlotIDinLeftHand = 0;
                    break;
                case PossessionPosition.NECK:
                    Character.PossessionPartnerSlotIDinAccesory = 0;
                    break;
                case PossessionPosition.CHEST:
                    Character.PossessionPartnerSlotIDinClothes = 0;
                    break;
            }

            p1.Pos = p.PossessionPosition;
            NetIo.SendPacket(p1);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
        }

        private PProtect pp;

        public void OnPProtectCreatedIniti(CSMG_PPROTECT_CREATED_INITI p)
        {
            var p1 = new SSMG_PPROTECT_CREATED_INITI();
            NetIo.SendPacket(p1);
        }

        /// <summary>
        ///     打开列表
        /// </summary>
        public void OnPProtectListOpen(CSMG_PPROTECT_LIST_OPEN p)
        {
            ushort max = 0;
            var pp = PProtectManager.Instance.GetPProtectsOfPage(p.Page, out max, p.Search);
            //pp.Add(new PProtect { ID = 0xffff, Leader = this.chara, Name = "123", MaxMember = 5, Message = "233", TaskID = 12000 });
            //pp.Add(new PProtect { ID = 0xffee, Leader = this.chara, Name = "987", MaxMember = 1, Message = "000", TaskID = 12000 });
            var p1 = new SSMG_PPROTECT_LIST();

            p1.PageMax = max;
            if (p.Page < max)
                p1.Page = p.Page;
            else
                p1.Page = max;
            p1.List = pp;
            p1.DumpData();
            NetIo.SendPacket(p1);
        }

        /// <summary>
        ///     创建招募
        /// </summary>
        public void OnPProtectCreated(CSMG_PPROTECT_CREATED_INFO p)
        {
            pp = new PProtect();
            pp.Leader = Character;
            pp.Members.Add(pp.Leader);
            pp.Name = p.name;
            pp.Password = p.password;
            pp.Message = p.message;
            pp.MaxMember = p.maxMember;
            pp.TaskID = p.taskID;

            PProtectManager.Instance.ADD(pp);

            var p1 = new SSMG_PPROTECT_CREATED_RESULT();

            NetIo.SendPacket(p1);

            var p2 = new SSMG_PPROTECT_CHAT_INFO();
            p2.SetData(Character, 0, 0, 0, 0, 0, 0);
            NetIo.SendPacket(p2);
        }


        /// <summary>
        ///     修改招募信息
        /// </summary>
        public void OnPProtectCreatedRevise(CSMG_PPROTECT_CREATED_REVISE p)
        {
            if (pp == null)
                return;
            pp.Name = p.name;
            pp.Password = p.password;
            pp.Message = p.message;
            pp.MaxMember = p.maxMember;
            pp.TaskID = p.taskID;


            var p1 = new SSMG_PPROTECT_CREATED_REVISE_RESULT();
            p1.SetData(p.name, p.message, p.taskID, p.maxMember, 0, 0);
            //string ss = p1.DumpData();
            NetIo.SendPacket(p1);

            for (var i = 0; i < pp.Members.Count; i++)
            {
                var client = MapClientManager.Instance.FindClient((ActorPC)pp.Members[i]);
                if (client != null && client.Character != Character)
                {
                    p1 = new SSMG_PPROTECT_CREATED_REVISE_RESULT();
                    p1.SetData(p.name, p.message, p.taskID, p.maxMember, 0, 0);
                    client.NetIo.SendPacket(p1);
                }
            }
        }

        /// <summary>
        ///     加入
        /// </summary>
        public void OnPProtectADD(CSMG_PPROTECT_ADD p)
        {
            addPProtect(p.PPID, p.Password);
        }

        public void OnPProtectADD1(CSMG_PPROTECT_ADD_1 p)
        {
            var ppt = PProtectManager.Instance.GetPProtect(p.PPID);
            if (ppt != null)
            {
                var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT_1();
                p1.List = ppt.Members;
                NetIo.SendPacket(p1);
            }
        }

        private void addPProtect(uint ppid, string password)
        {
            if (pp != null) OnPProtectCreatedOut(null);

            var ppt = PProtectManager.Instance.GetPProtect(ppid);
            if (ppt != null)
            {
                if (!ppt.IsPassword || ppt.Password == password)
                {
                    SSMG_PPROTECT_CHAT_INFO p2;
                    for (var i = 0; i < ppt.Members.Count; i++)
                    {
                        var client = MapClientManager.Instance.FindClient(ppt.Members[i]);
                        if (client != null)
                        {
                            p2 = new SSMG_PPROTECT_CHAT_INFO();
                            p2.SetData(Character, (byte)ppt.Members.Count, 0, 0, 0, 0, 0); //
                            client.NetIo.SendPacket(p2);

                            var p3 = new SSMG_PPROTECT_CHAT_INFO();
                            p3.SetData(ppt.Members[i], (byte)i, 0, 0, 0, 0, 0);
                            NetIo.SendPacket(p3);
                        }
                    }

                    p2 = new SSMG_PPROTECT_CHAT_INFO();
                    p2.SetData(Character, (byte)ppt.Members.Count, 0, 0, 0, 0, 0); //

                    NetIo.SendPacket(p2);
                    ppt.Members.Add(Character);
                    var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT();
                    p1.SetData(ppt.Name, password, 0, 1, 0);
                    NetIo.SendPacket(p1);
                    pp = ppt;
                }
                else
                {
                    var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT();
                    p1.SetData("", "", 0xFB, 0, 0xFF);
                    NetIo.SendPacket(p1);
                    //密码错误
                }
            }
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        public void OnPProtectReady(CSMG_PPROTECT_READY p)
        {
            if (pp != null && pp.Leader == Character)
                //队长进入房间操作
                return;
            var p1 = new SSMG_PPROTECT_READY_RESULT();
            SSMG_PPROTECT_READY p2;
            switch (p.State)
            {
                case 1: //准备
                    if (true)
                    {
                        //条件符合
                        p1.Code = 1;

                        for (var i = 0; i < pp.Members.Count; i++)
                        {
                            var client = MapClientManager.Instance.FindClient((ActorPC)pp.Members[i]);
                            if (client != null && client.Character != Character)
                            {
                                p2 = new SSMG_PPROTECT_READY();
                                p2.Index = (byte)pp.Members.IndexOf(Character);
                                p2.Code = 1;
                                client.NetIo.SendPacket(p2);
                            }
                        }
                    }
                    else
                    {
                        //条件不符
                        p1.Code = 0xFE;
                    }

                    break;
                case 0: //取消
                    {
                        for (var i = 0; i < pp.Members.Count; i++)
                        {
                            var client = MapClientManager.Instance.FindClient((ActorPC)pp.Members[i]);
                            if (client != null && client.Character != Character)
                            {
                                p2 = new SSMG_PPROTECT_READY();
                                p2.Index = (byte)pp.Members.IndexOf(Character);
                                p2.Code = 0;
                                client.NetIo.SendPacket(p2);
                            }
                        }
                    }
                    p1.Code = 0;
                    break;
            }

            NetIo.SendPacket(p1);
        }


        /// <summary>
        ///     退出招募
        /// </summary>
        public void OnPProtectCreatedOut(CSMG_PPROTECT_CREATED_OUT p)
        {
            if (pp == null)
                return;
            if (Character == pp.Leader)
            {
                //招募人退出
                PProtectManager.Instance.Remove(pp.ID);

                for (var i = 0; i < pp.Members.Count; i++)
                {
                    var client = MapClientManager.Instance.FindClient((ActorPC)pp.Members[i]);
                    if (client != null)
                    {
                        var p1 = new SSMG_PPROTECT_CREATED_OUT_RESULT();
                        p1.SetName(pp.Name);
                        client.NetIo.SendPacket(p1);
                    }
                }

                pp.Members.Clear();
            }
            else
            {
                //成员退出
                for (var i = 0; i < pp.Members.Count; i++)
                {
                    var client = MapClientManager.Instance.FindClient((ActorPC)pp.Members[i]);
                    if (client != null && client.Character != Character)
                    {
                        var p1 = new SSMG_PPROTECT_CREATED_OUT();
                        //int iii = pp.Members.IndexOf(this.Character);
                        p1.Index = (byte)pp.Members.IndexOf(Character);
                        client.NetIo.SendPacket(p1);
                    }
                }

                var p2 = new SSMG_PPROTECT_CREATED_OUT_RESULT_1();
                p2.SetName(pp.Name);
                NetIo.SendPacket(p2);

                pp.Members.Remove(Character);
            }

            pp = null;
        }

        public bool itemEnhance;
        public bool itemFusion;
        public uint itemFusionEffect, itemFusionView;
        public bool itemMasterEnhance;
        public uint kujiboxID0 = 120000000;
        public uint kujinum_max = 1000;

        public void OnItemChange(CSMG_ITEM_CHANGE p)
        {
            var item = Character.Inventory.GetItem(p.InventorySlot);


            if (!KujiListFactory.Instance.ItemTransformList.ContainsKey(p.ChangeID))
            {
                if (account.GMLevel >= 200) SendSystemMessage("錯誤！沒找到ChangeID！");
                return;
            }


            var it = KujiListFactory.Instance.ItemTransformList[p.ChangeID];
            var newitem = ItemFactory.Instance.GetItem(it.product);


            //道具锁
            if (item.ChangeMode || item.ChangeMode2)
                return;

            item.ChangeMode = true;

            //unknown
            var p1 = new SSMG_ITEM_CHANGE_ADD();
            NetIo.SendPacket(p1);
            //添加道具锁
            var p2 = new SSMG_ITEM_INFO();
            p2.Item = item;
            p2.InventorySlot = p.InventorySlot;
            p2.Container = Character.Inventory.GetContainerType(item.Slot);
            NetIo.SendPacket(p2);


            //添加pet道具
            //pet属性设置
            newitem.Refine = 1;
            newitem.Durability = item.Durability;
            newitem.HP = (short)(item.BaseData.atk1 + item.BaseData.matk);
            newitem.Atk1 = (short)(item.BaseData.atk1 + item.BaseData.str * 30);
            newitem.Atk2 = (short)(item.BaseData.atk2 + item.BaseData.str * 30);
            newitem.Atk3 = (short)(item.BaseData.atk3 + item.BaseData.str * 30);
            newitem.MAtk = (short)(item.BaseData.matk + item.BaseData.mag * 30);
            newitem.Def = (short)(item.BaseData.def + item.BaseData.intel * 30 + item.BaseData.str * 20);
            newitem.MDef = (short)(item.BaseData.mdef + item.BaseData.intel * 20 + item.BaseData.mag * 30);
            newitem.HitCritical = (short)(item.BaseData.atk1 + item.BaseData.matk);
            newitem.AvoidMagic = (short)(item.BaseData.atk1 + item.BaseData.matk);
            newitem.AvoidCritical = (short)(item.BaseData.atk1 + item.BaseData.matk + 3);
            newitem.AvoidMelee = (short)(item.BaseData.atk1 + item.BaseData.matk + 1);
            newitem.AvoidRanged = (short)(item.BaseData.atk1 + item.BaseData.matk + 2);
            newitem.HitMagic = (short)(item.BaseData.atk1 + item.BaseData.matk + 4);
            newitem.HitMelee = (short)(item.BaseData.atk1 + item.BaseData.matk + 5);
            newitem.HitRanged = (short)(item.BaseData.atk1 + item.BaseData.matk + 6);
            //白框
            newitem.ChangeMode2 = true;
            AddItem(newitem, false);

            SendSystemMessage(item.BaseData.name + "已成功轉換。");


            /*
            try
            {
                List<uint> slots = p.SlotList();
                if (Character.Inventory.GetItem(slots[0]) == null) return;
                if (Character.Inventory.GetItem(slots[0]).Cards.Count > 0)
                {
                    SendSystemMessage("原装备还存在卡片，无法进行升级。");
                    return;
                }

                if (!KujiListFactory.Instance.ItemTransformList.ContainsKey(p.ChangeID))
                    return;
                if (p.ChangeID < 200)
                    return;
                KujiListFactory.ItemTransform it = KujiListFactory.Instance.ItemTransformList[p.ChangeID];

                for (int i = 0; i < it.Stuffs.Count; i++)
                {
                    Item stuff = Character.Inventory.GetItem(slots[i]);
                    if (stuff.ItemID != it.Stuffs[i])
                        return;
                }
                Item newitem = ItemFactory.Instance.GetItem(it.product);
                Item oriitem = Character.Inventory.GetItem(slots[0]);

                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i] == 0)
                        continue;
                    Item stuff = Character.Inventory.GetItem(slots[i]);
                    newitem.HP += stuff.HP;
                    newitem.MP += stuff.MP;
                    newitem.SP += stuff.SP;
                    newitem.WeightUp += stuff.WeightUp;
                    newitem.VolumeUp += stuff.VolumeUp;
                    newitem.Str += stuff.Str;
                    newitem.Dex += stuff.Dex;
                    newitem.Int += stuff.Int;
                    newitem.Vit += stuff.Vit;
                    newitem.Agi += stuff.Agi;
                    newitem.Mag += stuff.Mag;
                    newitem.Atk1 += stuff.Atk1;
                    newitem.Atk2 += stuff.Atk2;
                    newitem.Atk3 += stuff.Atk3;
                    newitem.MAtk += stuff.MAtk;
                    newitem.Def += stuff.Def;
                    newitem.MDef += stuff.MDef;
                    newitem.ASPD += stuff.ASPD;
                    newitem.CSPD += stuff.CSPD;
                    DeleteItem(slots[i], 1, true);
                }
                //继承强化
                newitem.Refine = oriitem.Refine;
                newitem.Refine_Sharp = oriitem.Refine_Sharp;
                newitem.Refine_Enchanted = oriitem.Refine_Enchanted;
                newitem.Refine_Vitality = oriitem.Refine_Vitality;
                newitem.Refine_Hit = oriitem.Refine_Hit;
                newitem.Refine_Mhit = oriitem.Refine_Mhit;
                newitem.Refine_Regeneration = oriitem.Refine_Regeneration;
                newitem.Refine_Lucky = oriitem.Refine_Lucky;
                newitem.Refine_Dexterity = oriitem.Refine_Dexterity;
                newitem.Refine_ATKrate = oriitem.Refine_ATKrate;
                newitem.Refine_MATKrate = oriitem.Refine_MATKrate;
                newitem.Refine_Def = oriitem.Refine_Def;
                newitem.Refine_Mdef = oriitem.Refine_Mdef;

                newitem.PictID = oriitem.PictID;
                AddItem(newitem, true);
            }
            catch (Exception ex)
            {
                SagaLib.Logger.ShowError(ex);
            }
            */

            //Character.TInt["套装Change的ID"] = (int)p.InventorySlot;
            //EventActivate(99660000);

            /*if (this.Character.CInt["110武器变型DBID"] != 0)
                return;
            Item item = this.Character.Inventory.GetItem(p.InventorySlot);
            //获得物品Pet数据
            uint PetID = 0;
            if (p.ChangeID == 0x1C || p.ChangeID == 0x1B)
                PetID = 10085000;//カムイ 神剣・武御雷 / 煌刃・白虎のペットチェンジ後の姿
            else if (p.ChangeID == 0x1D)
                PetID = 10085100;//リゼル 血剣ダインスレイブのペットチェンジ後の姿
            else if (p.ChangeID == 0x24 || p.ChangeID == 0x25)
                PetID = 10085600;//ヨシュア 神槍・ブリューナク / 天槍・リンドブルムのペットチェンジ後の姿
            else if (p.ChangeID == 0x1E)
                PetID = 10085200;//テリア 護聖剣・ジブリール
            else if (p.ChangeID == 0x1f)
                PetID = 10085300;//キーノ 冥王爪・オルトロスのペットチェンジ後の姿
            else if (p.ChangeID == 0x28 || p.ChangeID == 0x29)
                PetID = 10085800;//ソレイユ 月天弓・アルテミス / 轟天弓・バハムートのペットチェンジ後の姿
            else if (p.ChangeID == 0x2A || p.ChangeID == 0x2B || p.ChangeID == 0x2C)
                PetID = 10085900;//グリヴァー 烈神銃・サラマンドラ / 烈神銃・サラマンドラ（２丁）穿竜砲・ヤタガラスのペットチェンジ後の姿
            else if (p.ChangeID == 0x26 || p.ChangeID == 0x27)
                PetID = 10085701;//レネット 黒書・ネクロノミコン / 黎明杖・カドゥケウスのペットチェンジ後の姿
            else if (p.ChangeID == 0x2E)
                PetID = 10086100;//フロール 浄絃・ソウルセラフィムのペットチェンジ後の姿
            else if (p.ChangeID == 0x20 || p.ChangeID == 0x21)
                PetID = 10085400;//ツバキ 降魔槌・クレイオス / 祓魔槌・ウロボロスのペットチェンジ後の姿
            else if (p.ChangeID == 0x22 || p.ChangeID == 0x23)
                PetID = 10085500;//カノン 闇葬鎌・タルタロス / 断罪斧・レヴァイアサンのペットチェンジ後の姿、幼女
            else if (p.ChangeID == 0x2D)
                PetID = 10086000;//プリーシュ 煉獄鞭・アナフィエルのペットチェンジ後の姿
            else
                return;
            //道具锁
            if (item.ChangeMode || item.ChangeMode2)
                return;
            item.ChangeMode = true;
            this.Character.CInt["110武器变型DBID"] = 10;
            //Logger.ShowError(string.Format("60052300:{0},{1}", item.DBID, this.Character.CInt["110武器变型DBID"]));
            //unknown
            Packets.Server.SSMG_ITEM_CHANGE_ADD p1 = new SagaMap.Packets.Server.SSMG_ITEM_CHANGE_ADD();
            this.NetIo.SendPacket(p1);
            //添加道具锁
            Packets.Server.SSMG_ITEM_INFO p2 = new SagaMap.Packets.Server.SSMG_ITEM_INFO();
            p2.Item = item;
            p2.InventorySlot = p.InventorySlot;
            p2.Container = this.Character.Inventory.GetContainerType(item.Slot);
            this.NetIo.SendPacket(p2);

            //添加pet道具
            SagaDB.Item.Item ChangeItem = ItemFactory.Instance.GetItem(PetID);
            this.AddItem(ChangeItem, true);
            Item petitem = this.Character.Inventory.GetItem(ChangeItem.Slot);
            //pet属性设置
            petitem.Refine = 1;
            petitem.Durability = item.Durability;
            petitem.HP = (short)(item.BaseData.atk1 + item.BaseData.matk);
            petitem.Atk1 = (short)(item.BaseData.atk1 + item.BaseData.str * 30);
            petitem.Atk2 = (short)(item.BaseData.atk2 + item.BaseData.str * 30);
            petitem.Atk3 = (short)(item.BaseData.atk3 + item.BaseData.str * 30);
            petitem.MAtk = (short)(item.BaseData.matk + item.BaseData.mag * 30);
            petitem.Def = (short)(item.BaseData.def + item.BaseData.intel * 30 + item.BaseData.str * 20);
            petitem.MDef = (short)(item.BaseData.mdef + item.BaseData.intel * 20 + item.BaseData.mag * 30);
            petitem.HitCritical = (short)(item.BaseData.atk1 + item.BaseData.matk);
            petitem.AvoidMagic = (short)(item.BaseData.atk1 + item.BaseData.matk);
            petitem.AvoidCritical = (short)(item.BaseData.atk1 + item.BaseData.matk + 3);
            petitem.AvoidMelee = (short)(item.BaseData.atk1 + item.BaseData.matk + 1);
            petitem.AvoidRanged = (short)(item.BaseData.atk1 + item.BaseData.matk + 2);
            petitem.HitMagic = (short)(item.BaseData.atk1 + item.BaseData.matk + 4);
            petitem.HitMelee = (short)(item.BaseData.atk1 + item.BaseData.matk + 5);
            petitem.HitRanged = (short)(item.BaseData.atk1 + item.BaseData.matk + 6);
            //白框
            petitem.ChangeMode2 = true;
            SendItemInfo(petitem);*/
        }

        public void OnItemMasterEnhanceClose(CSMG_ITEM_MASTERENHANCE_CLOSE p)
        {
            itemMasterEnhance = false;
            //关闭界面处理
        }

        public void SendMasterEnhanceAbleEquiList()
        {
            var p2 = new SSMG_ITEM_MASTERENHANCE_RESULT();
            p2.Result = 0;
            var packet = new Packet(10);
            packet.data = new byte[10];
            packet.ID = 0x1f59;

            if (Character.Gold < 5000)
            {
                p2.Result = -1;
                NetIo.SendPacket(p2);
                NetIo.SendPacket(packet);
                itemMasterEnhance = false;
                return;
            }

            var lst = Character.Inventory.GetContainer(ContainerType.BODY);
            var itemlist = lst.Where(x => (x.IsWeapon || x.IsArmor) && !x.Potential).ToList();
            var p = new SSMG_ITEM_MASTERENHANCE_LIST();
            p.Items = itemlist;

            if (itemlist.Count > 0)
            {
                NetIo.SendPacket(p);
            }
            else
            {
                p2.Result = -2;
                NetIo.SendPacket(p2);
                NetIo.SendPacket(packet);
                itemMasterEnhance = false;
            }
        }

        public void OnItemMasterEnhanceSelect(CSMG_ITEM_MASTERENHANCE_SELECT p)
        {
            Character.Inventory.GetItem(p.InventorySlot);
            var lst = new List<MasterEnhanceMaterial>();

            foreach (var itemkey in MasterEnhanceMaterialFactory.Instance.Items.Keys)
                if (CountItem(itemkey) > 0)
                    lst.Add(MasterEnhanceMaterialFactory.Instance.Items[itemkey]);

            var p1 = new SSMG_ITEM_MASTERENHANCE_DETAIL();
            p1.Items = lst;
            NetIo.SendPacket(p1);
        }

        public void OnItemMasterEnhanceConfirm(CSMG_ITEM_MASTERENHANCE_CONFIRM p)
        {
            var p2 = new SSMG_ITEM_MASTERENHANCE_RESULT();
            p2.Result = 0;

            var packet = new Packet(10);
            packet.data = new byte[10];
            packet.ID = 0x1f59;

            var item = Character.Inventory.GetItem(p.InventorySlot);
            if (item == null)
            {
                p2.Result = -4;

                NetIo.SendPacket(p2);
                NetIo.SendPacket(packet);
                itemMasterEnhance = false;
                return;
            }

            var materialid = p.ItemID;

            if (CountItem(materialid) <= 0)
            {
                p2.Result = -3;

                NetIo.SendPacket(p2);
                NetIo.SendPacket(packet);
                itemMasterEnhance = false;
                return;
            }

            DeleteItemID(materialid, 1, true);

            var Material = MasterEnhanceMaterialFactory.Instance.Items[materialid];
            var value = (short)Global.Random.Next(Material.MinValue, Material.MaxValue);

            item.Potential = true;
            switch (Material.Ability)
            {
                case MasterEnhanceType.STR:
                    item.Str += value;
                    break;
                case MasterEnhanceType.DEX:
                    item.Dex += value;
                    break;
                case MasterEnhanceType.INT:
                    item.Int += value;
                    break;
                case MasterEnhanceType.VIT:
                    item.Vit += value;
                    break;
                case MasterEnhanceType.AGI:
                    item.Agi += value;
                    break;
                case MasterEnhanceType.MAG:
                    item.Mag += value;
                    break;
            }

            SendEffect(5145);

            SendItemInfo(item);

            p2.Result = 0;
            NetIo.SendPacket(p2);

            SendMasterEnhanceAbleEquiList();
        }

        public void OnItemChangeCancel(CSMG_ITEM_CHANGE_CANCEL p)
        {
            var PetItem = Character.Inventory.GetItem(p.InventorySlot);
            var ChangeItem = Character.Inventory.GetItem2();
            ChangeItem.Durability = PetItem.Durability;
            ChangeItem.ChangeMode = false;

            SendItemInfo(ChangeItem);
            DeleteItem(PetItem.Slot, 1, false);

            SendSystemMessage(PetItem.BaseData.name + "已轉換原始的狀態。");


            // Logger.ShowError(string.Format("OnItemChangeCancel:{0}", this.Character.CInt["110武器变型DBID"]));
            /*
            if (this.Character.CInt["110武器变型DBID"] != 0)
            {

                Item PetItem = this.Character.Inventory.GetItem(p.InventorySlot);
                Item ChangeItem = this.Character.Inventory.GetItem2();
                ChangeItem.Durability = PetItem.Durability;
                ChangeItem.ChangeMode = false;
                this.Character.CInt["110武器变型DBID"] = 0;
                SendItemInfo(ChangeItem);
                this.DeleteItem(PetItem.Slot, 1, true);
            }
            else
            {
                return;
            }
            */
        }

        public void OnItemFusionCancel(CSMG_ITEM_FUSION_CANCEL p)
        {
            itemFusionEffect = 0;
            itemFusionView = 0;
            itemFusion = false;
        }

        public void OnItemFusion(CSMG_ITEM_FUSION p)
        {
            itemFusionEffect = p.EffectItem;
            itemFusionView = p.ViewItem;
            itemFusion = false;
        }

        public void OnItemEnhanceClose(CSMG_ITEM_ENHANCE_CLOSE p)
        {
            itemEnhance = false;
            irisAddSlot = false;
        }

        public void OnItemEnhanceSelect(CSMG_ITEM_ENHANCE_SELECT p)
        {
            var item = Character.Inventory.GetItem(p.InventorySlot);
            if (item != null)
            {
                var list = new List<EnhanceDetail>();
                if (item.IsWeapon)
                {
                    if (CountItem(90000044) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000044;
                        detail.type = EnhanceType.Atk;
                        detail.value = FindEnhancementValue(item, 90000044);
                        list.Add(detail);
                    }

                    if (CountItem(90000045) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000045;
                        detail.type = EnhanceType.MAtk;
                        detail.value = FindEnhancementValue(item, 90000045);
                        list.Add(detail);
                    }

                    if (CountItem(90000046) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000046;
                        detail.type = EnhanceType.Cri;
                        detail.value = FindEnhancementValue(item, 90000046);
                        list.Add(detail);
                    }
                }

                if (item.IsArmor)
                {
                    if (CountItem(90000043) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000043;
                        detail.type = EnhanceType.HP;
                        detail.value = FindEnhancementValue(item, 90000043);
                        list.Add(detail);
                    }

                    if (CountItem(90000044) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000044;
                        detail.type = EnhanceType.Def;
                        detail.value = FindEnhancementValue(item, 90000044);
                        list.Add(detail);
                    }

                    if (CountItem(90000045) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000045;
                        detail.type = EnhanceType.MDef;
                        detail.value = FindEnhancementValue(item, 90000045);
                        list.Add(detail);
                    }

                    if (CountItem(90000046) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000046;
                        detail.type = EnhanceType.AvoidCri;
                        detail.value = FindEnhancementValue(item, 90000046);
                        list.Add(detail);
                    }
                }

                if (item.BaseData.itemType == ItemType.SHIELD)
                {
                    if (CountItem(90000044) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000044;
                        detail.type = EnhanceType.Def;
                        detail.value = FindEnhancementValue(item, 90000044);
                        list.Add(detail);
                    }

                    if (CountItem(90000045) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000045;
                        detail.type = EnhanceType.MDef;
                        detail.value = FindEnhancementValue(item, 90000045);
                        list.Add(detail);
                    }
                }

                if (item.BaseData.itemType == ItemType.ACCESORY_NECK)
                    if (CountItem(90000045) > 0)
                    {
                        var detail = new EnhanceDetail();
                        detail.material = 90000045;
                        detail.type = EnhanceType.MDef;
                        detail.value = FindEnhancementValue(item, 90000045);
                        list.Add(detail);
                    }

                var p1 = new SSMG_ITEM_ENHANCE_DETAIL();
                p1.Items = list;
                NetIo.SendPacket(p1);
            }
        }

        public short FindEnhancementValue(Item item, uint itemID)
        {
            var hps = new short[31]
            {
                0,
                100, 20, 70, 30, 80, 40, 90, 50, 100, 150,
                150, 60, 110, 70, 200, 200, 120, 80, 130, 250,
                250, 90, 140, 100, 250, 250, 150, 110, 160, 400
            };
            var atk_def_matk = new short[31]
            {
                0,
                10, 3, 5, 3, 6, 3, 7, 3, 8, 13,
                13, 3, 9, 3, 15, 15, 10, 3, 11, 20,
                20, 3, 12, 3, 22, 22, 13, 3, 14, 25
            };
            var mdef = new short[31]
            {
                0,
                10, 2, 5, 2, 6, 3, 6, 3, 6, 15,
                15, 4, 7, 4, 10, 10, 7, 4, 7, 15,
                15, 5, 8, 5, 15, 15, 8, 5, 8, 25
            };
            var cris = new short[31]
            {
                0,
                5, 1, 3, 2, 4, 3, 4, 3, 5, 9,
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4,
                5, 1, 2, 3, 4, 5, 1, 2, 3, 5
            };
            switch (itemID)
            {
                case 90000043:
                    if (item.IsArmor)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.LifeEnhance; j++)
                            i += hps[j];
                        return (short)(i + hps[item.LifeEnhance + 1]);
                    }

                    break;
                case 90000044:
                    if (item.IsWeapon)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.PowerEnhance; j++)
                            i += atk_def_matk[j];
                        return (short)(i + atk_def_matk[item.PowerEnhance + 1]);
                    }

                    if (item.IsArmor || item.BaseData.itemType == ItemType.SHIELD)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.PowerEnhance; j++)
                            i += atk_def_matk[j];
                        return (short)(i + atk_def_matk[item.PowerEnhance + 1]);
                    }

                    break;
                case 90000045:
                    if (item.IsWeapon)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.MagEnhance; j++)
                            i += atk_def_matk[j];
                        return (short)(i + atk_def_matk[item.MagEnhance + 1]);
                    }
                    else
                    {
                        short i = 0;
                        for (var j = 0; j <= item.MagEnhance; j++)
                            i += mdef[j];
                        return (short)(i + mdef[item.MagEnhance + 1]);
                    }
                case 90000046:
                    if (item.IsWeapon)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.CritEnhance; j++)
                            i += cris[j];
                        return (short)(i + cris[item.CritEnhance + 1]);
                    }

                    if (item.IsArmor)
                    {
                        short i = 0;
                        for (var j = 0; j <= item.CritEnhance; j++)
                            i += cris[j];
                        return (short)(i + cris[item.CritEnhance + 1]);
                    }

                    break;
            }

            return 0;
        }

        /// <summary>
        ///     取得玩家身上指定道具的信息
        /// </summary>
        /// <param name="ID">道具ID</param>
        /// <returns>道具清单</returns>
        protected List<Item> GetItem(uint ID)
        {
            var result = new List<Item>();
            for (var i = 2; i < 6; i++)
            {
                var list = Character.Inventory.Items[(ContainerType)i];
                var query = from it in list
                            where it.ItemID == ID
                            select it;
                result.AddRange(query);
            }

            return result;
        }

        public void OnItemEnhanceConfirm(CSMG_ITEM_ENHANCE_CONFIRM p)
        {
            var item = Character.Inventory.GetItem(p.InventorySlot);
            var failed = false;
            var p1 = new SSMG_ITEM_ENHANCE_RESULT();
            p1.Result = 0;
            if (item != null)
            {
                if (CountItem(p.Material) > 0 && item.Refine < 30)
                {
                    if (Character.Gold >= 5000)
                    {
                        Character.Gold -= 5000;

                        Logger.ShowInfo("Refine Item:" + item.BaseData.name + "[" + p.InventorySlot +
                                        "] Protect Item: " +
                                        (ItemFactory.Instance.GetItem(p.ProtectItem).ItemID != 10000000
                                            ? ItemFactory.Instance.GetItem(p.ProtectItem).BaseData.name
                                            : "None") +
                                        Environment.NewLine + "Material: " +
                                        (ItemFactory.Instance.GetItem(p.Material).ItemID != 10000000
                                            ? ItemFactory.Instance.GetItem(p.Material).BaseData.name
                                            : "None") +
                                        " SupportItem: " +
                                        (ItemFactory.Instance.GetItem(p.SupportItem).ItemID != 10000000
                                            ? ItemFactory.Instance.GetItem(p.SupportItem).BaseData.name
                                            : "None"));


                        Logger.ShowInfo("BaseLevel: " + p.BaseLevel + " JLevel: " + p.JobLevel);
                        Logger.ShowInfo("ExpRate: " + p.ExpRate + " JExpRate: " + p.JExpRate);

                        var Material = p.Material;

                        uint supportitemid = 0;
                        var enhancesupported = false;
                        if (ItemFactory.Instance.GetItem(p.SupportItem).ItemID != 10000000)
                        {
                            enhancesupported = true;
                            supportitemid = ItemFactory.Instance.GetItem(p.SupportItem).ItemID;
                        }

                        uint protectitemid = 0;
                        var enhanceprotected = false;
                        if (ItemFactory.Instance.GetItem(p.ProtectItem).ItemID != 10000000)
                        {
                            enhanceprotected = true;
                            protectitemid = ItemFactory.Instance.GetItem(p.ProtectItem).ItemID;
                        }

                        //重寫！ - KK
                        //成功率加成
                        //解决了保护和辅助道具强行使用的问题 by [黑白照] 2018.07.02 
                        var finalrate = 0;
                        var used_material = "";
                        var crystal_addon = 0;
                        var skill_addon = 0;
                        var protect_addon = 0;
                        var support_addon = 0;
                        var matsuri_addon = 0;
                        var recycle_addon = 0;
                        var nextlevel = item.Refine + 1;

                        //BaseRate
                        finalrate += EnhanceTableFactory.Instance.Table[nextlevel].BaseRate;

                        //一般結晶
                        if (p.Material >= 90000043 && p.Material <= 90000046)
                            crystal_addon = EnhanceTableFactory.Instance.Table[nextlevel].Crystal;


                        //強化結晶
                        if (p.Material >= 90000053 && p.Material <= 90000056)
                            crystal_addon = EnhanceTableFactory.Instance.Table[nextlevel].EnhanceCrystal;

                        //超強化結晶
                        if (p.Material >= 16004500 && p.Material <= 16004800)
                            crystal_addon = EnhanceTableFactory.Instance.Table[nextlevel].SPCrystal;

                        //強化王結晶
                        if (p.Material >= 10087400 && p.Material <= 10087403)
                            crystal_addon = EnhanceTableFactory.Instance.Table[nextlevel].KingCrystal;

                        //防爆
                        if (enhanceprotected && protectitemid == 16001300)
                            protect_addon = EnhanceTableFactory.Instance.Table[nextlevel].ExplodeProtect;

                        //防重設
                        if (enhanceprotected && protectitemid == 10118200)
                            protect_addon = EnhanceTableFactory.Instance.Table[nextlevel].ResetProtect;

                        //奧義
                        if (ItemFactory.Instance.GetItem(p.ProtectItem).ItemID == 10087200)
                            support_addon = EnhanceTableFactory.Instance.Table[nextlevel].Okugi;

                        //神髓
                        if (ItemFactory.Instance.GetItem(p.ProtectItem).ItemID == 10087201)
                            support_addon = EnhanceTableFactory.Instance.Table[nextlevel].Shinzui;

                        //強化祭活動
                        if (Configuration.Configuration.Instance.EnhanceMatsuri)
                            matsuri_addon = EnhanceTableFactory.Instance.Table[nextlevel].Matsuri;


                        //回收活動
                        //未實裝(需連動回收系統)
                        /*
                        if (SagaMap.Configuration.Instance.Recycle)
                            finalrate += EnhanceTableFactory.Instance.Table[nextlevel].Recycle;
                        */


                        //被動技能加成
                        //一般結晶
                        if (p.Material >= 90000043 && p.Material <= 90000046)
                        {
                            if (p.Material == 90000043 && Character.Status.Additions.ContainsKey("BoostHp"))
                            {
                                used_material = "強化技能-命";
                                skill_addon =
                                    (Character.Status.Additions["BoostHp"] as DefaultPassiveSkill).Variable["BoostHp"];
                            }

                            if (p.Material == 90000044 && Character.Status.Additions.ContainsKey("BoostPower"))
                            {
                                used_material = "強化技能-力";
                                skill_addon =
                                    (Character.Status.Additions["BoostPower"] as DefaultPassiveSkill).Variable[
                                        "BoostPower"];
                            }

                            if (p.Material == 90000045 && Character.Status.Additions.ContainsKey("BoostMagic"))
                            {
                                used_material = "強化技能-魔";
                                skill_addon =
                                    (Character.Status.Additions["BoostMagic"] as DefaultPassiveSkill).Variable[
                                        "BoostMagic"];
                            }

                            if (p.Material == 90000046 && Character.Status.Additions.ContainsKey("BoostCritical"))
                            {
                                used_material = "強化技能-暴擊";
                                skill_addon =
                                    (Character.Status.Additions["BoostCritical"] as DefaultPassiveSkill).Variable[
                                        "BoostCritical"];
                            }
                        }

                        //強化結晶
                        if (p.Material >= 90000053 && p.Material <= 90000056)
                        {
                            if (p.Material == 90000053 && Character.Status.Additions.ContainsKey("BoostHp"))
                            {
                                used_material = "強化技能-命";
                                skill_addon =
                                    (Character.Status.Additions["BoostHp"] as DefaultPassiveSkill).Variable["BoostHp"];
                            }

                            if (p.Material == 90000054 && Character.Status.Additions.ContainsKey("BoostPower"))
                            {
                                used_material = "強化技能-力";
                                skill_addon =
                                    (Character.Status.Additions["BoostPower"] as DefaultPassiveSkill).Variable[
                                        "BoostPower"];
                            }

                            if (p.Material == 90000055 && Character.Status.Additions.ContainsKey("BoostMagic"))
                            {
                                used_material = "強化技能-魔";
                                skill_addon =
                                    (Character.Status.Additions["BoostMagic"] as DefaultPassiveSkill).Variable[
                                        "BoostMagic"];
                            }

                            if (p.Material == 90000056 && Character.Status.Additions.ContainsKey("BoostCritical"))
                            {
                                used_material = "強化技能-暴擊";
                                skill_addon =
                                    (Character.Status.Additions["BoostCritical"] as DefaultPassiveSkill).Variable[
                                        "BoostCritical"];
                            }
                        }

                        //超強化結晶
                        if (p.Material >= 16004500 && p.Material <= 16004800)
                        {
                            if (p.Material == 16004600 && Character.Status.Additions.ContainsKey("BoostHp"))
                            {
                                used_material = "強化技能-命";
                                skill_addon =
                                    (Character.Status.Additions["BoostHp"] as DefaultPassiveSkill).Variable["BoostHp"];
                            }

                            if (p.Material == 16004700 && Character.Status.Additions.ContainsKey("BoostPower"))
                            {
                                used_material = "強化技能-力";
                                skill_addon =
                                    (Character.Status.Additions["BoostPower"] as DefaultPassiveSkill).Variable[
                                        "BoostPower"];
                            }

                            if (p.Material == 16004800 && Character.Status.Additions.ContainsKey("BoostMagic"))
                            {
                                used_material = "強化技能-魔";
                                skill_addon =
                                    (Character.Status.Additions["BoostMagic"] as DefaultPassiveSkill).Variable[
                                        "BoostMagic"];
                            }

                            if (p.Material == 16004500 && Character.Status.Additions.ContainsKey("BoostCritical"))
                            {
                                used_material = "強化技能-暴擊";
                                skill_addon =
                                    (Character.Status.Additions["BoostCritical"] as DefaultPassiveSkill).Variable[
                                        "BoostCritical"];
                            }
                        }

                        //強化王結晶
                        if (p.Material >= 10087400 && p.Material <= 10087403)
                        {
                            if (p.Material == 10087400 && Character.Status.Additions.ContainsKey("BoostHp"))
                            {
                                used_material = "強化技能-命";
                                skill_addon =
                                    (Character.Status.Additions["BoostHp"] as DefaultPassiveSkill).Variable["BoostHp"];
                            }

                            if (p.Material == 10087401 && Character.Status.Additions.ContainsKey("BoostPower"))
                            {
                                used_material = "強化技能-力";
                                skill_addon =
                                    (Character.Status.Additions["BoostPower"] as DefaultPassiveSkill).Variable[
                                        "BoostPower"];
                            }

                            if (p.Material == 10087403 && Character.Status.Additions.ContainsKey("BoostMagic"))
                            {
                                used_material = "強化技能-魔";
                                skill_addon =
                                    (Character.Status.Additions["BoostMagic"] as DefaultPassiveSkill).Variable[
                                        "BoostMagic"];
                            }

                            if (p.Material == 10087402 && Character.Status.Additions.ContainsKey("BoostCritical"))
                            {
                                used_material = "強化技能-暴擊";
                                skill_addon =
                                    (Character.Status.Additions["BoostCritical"] as DefaultPassiveSkill).Variable[
                                        "BoostCritical"];
                            }
                        }


                        //結算
                        finalrate += crystal_addon + matsuri_addon + skill_addon + protect_addon + support_addon +
                                     recycle_addon;


                        FromActorPC(Character).SendSystemMessage("----------------強化成功率結算---------------");
                        FromActorPC(Character).SendSystemMessage("正強化裝備到：" + (item.Refine + 1));

                        FromActorPC(Character)
                            .SendSystemMessage("基本機率：" + EnhanceTableFactory.Instance.Table[nextlevel].BaseRate / 100 +
                                               "%");
                        FromActorPC(Character).SendSystemMessage("結晶加成：" + crystal_addon / 100 + "%");
                        FromActorPC(Character).SendSystemMessage("補助加成：" + support_addon / 100 + "%");
                        FromActorPC(Character).SendSystemMessage("防爆加成：" + protect_addon / 100 + "%");
                        FromActorPC(Character).SendSystemMessage("強化祭加成：" + matsuri_addon / 100 + "%");
                        FromActorPC(Character)
                            .SendSystemMessage("被動技加成：" + used_material + " -" + skill_addon / 100 + "%");
                        FromActorPC(Character).SendSystemMessage("回收活动加成：" + recycle_addon / 100 + "%");

                        FromActorPC(Character).SendSystemMessage("你的最終強化成功率為：" + finalrate / 100 + "%");
                        FromActorPC(Character).SendSystemMessage("----------------強化成功率結算---------------");

                        if (finalrate > 9999)
                            finalrate = 9999;

                        var refrate = Global.Random.Next(0, 9999);

                        if (enhanceprotected)
                        {
                            if (CountItem(protectitemid) < 1)
                            {
                                p1.Result = 0x00;
                                p1.OrignalRefine = item.Refine;
                                p1.ExpectedRefine = (ushort)(item.Refine + 1);
                                NetIo.SendPacket(p1);
                                p1.Result = 0xfc;
                                NetIo.SendPacket(p1);
                                itemEnhance = false;
                                return;
                            }

                            DeleteItemID(protectitemid, 1, true);
                        }

                        if (enhancesupported)
                        {
                            if (CountItem(supportitemid) < 1)
                            {
                                p1.Result = 0x00;
                                p1.OrignalRefine = item.Refine;
                                p1.ExpectedRefine = (ushort)(item.Refine + 1);
                                NetIo.SendPacket(p1);
                                p1.Result = 0xfc;
                                NetIo.SendPacket(p1);
                                itemEnhance = false;
                                return;
                            }

                            DeleteItemID(supportitemid, 1, true);
                        }


                        if (CountItem(Material) < 1)
                        {
                            p1.Result = 0x00;
                            p1.OrignalRefine = item.Refine;
                            p1.ExpectedRefine = (ushort)(item.Refine + 1);
                            NetIo.SendPacket(p1);
                            p1.Result = 0xfc;
                            NetIo.SendPacket(p1);
                            itemEnhance = false;
                            return;
                        }

                        if (refrate <= finalrate)
                        {
                            if (item.IsArmor)
                                switch (p.Material)
                                {
                                    //一般水晶
                                    case 90000043:
                                        item.HP = FindEnhancementValue(item, 90000043);
                                        DeleteItemID(90000043, 1, true);
                                        item.LifeEnhance++;
                                        break;
                                    case 90000044:
                                        item.Def = FindEnhancementValue(item, 90000044);
                                        DeleteItemID(90000044, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000045:
                                        item.MDef = FindEnhancementValue(item, 90000045);
                                        DeleteItemID(90000045, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 90000046:
                                        item.AvoidCritical = FindEnhancementValue(item, 90000046);
                                        DeleteItemID(90000046, 1, true);
                                        item.CritEnhance++;
                                        break;


                                    //強化水晶
                                    case 90000053:
                                        item.HP = FindEnhancementValue(item, 90000043);
                                        DeleteItemID(90000053, 1, true);
                                        item.LifeEnhance++;
                                        break;
                                    case 90000054:
                                        item.Def = FindEnhancementValue(item, 90000054);
                                        DeleteItemID(90000054, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000055:
                                        item.MDef = FindEnhancementValue(item, 90000055);
                                        DeleteItemID(90000055, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 90000056:
                                        item.AvoidCritical = FindEnhancementValue(item, 90000056);
                                        DeleteItemID(90000056, 1, true);
                                        item.CritEnhance++;
                                        break;


                                    //超強化水晶
                                    case 16004600:
                                        item.HP = FindEnhancementValue(item, 16004600);
                                        DeleteItemID(16004600, 1, true);
                                        item.LifeEnhance++;
                                        break;
                                    case 16004700:
                                        item.Def = FindEnhancementValue(item, 16004700);
                                        DeleteItemID(16004700, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 16004800:
                                        item.MDef = FindEnhancementValue(item, 16004800);
                                        DeleteItemID(16004800, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 16004500:
                                        item.AvoidCritical = FindEnhancementValue(item, 16004500);
                                        DeleteItemID(16004500, 1, true);
                                        item.CritEnhance++;
                                        break;


                                    //強化王
                                    case 10087400:
                                        item.HP = FindEnhancementValue(item, 10087400);
                                        DeleteItemID(10087400, 1, true);
                                        item.LifeEnhance++;
                                        break;
                                    case 10087401:
                                        item.Def = FindEnhancementValue(item, 10087401);
                                        DeleteItemID(10087401, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 10087403:
                                        item.MDef = FindEnhancementValue(item, 10087403);
                                        DeleteItemID(10087403, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 10087402:
                                        item.AvoidCritical = FindEnhancementValue(item, 10087402);
                                        DeleteItemID(10087402, 1, true);
                                        item.CritEnhance++;
                                        break;
                                }

                            if (item.IsWeapon)
                                switch (p.Material)
                                {
                                    //一般水晶
                                    case 90000044:
                                        item.Atk1 = FindEnhancementValue(item, 90000044);
                                        item.Atk2 = item.Atk1;
                                        item.Atk3 = item.Atk1;
                                        DeleteItemID(90000044, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000045:
                                        item.MAtk = FindEnhancementValue(item, 90000045);
                                        DeleteItemID(90000045, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 90000046:
                                        item.HitCritical = FindEnhancementValue(item, 90000046);
                                        DeleteItemID(90000046, 1, true);
                                        item.CritEnhance++;
                                        break;

                                    //強化水晶
                                    case 90000054:
                                        item.Atk1 = FindEnhancementValue(item, 90000054);
                                        item.Atk2 = item.Atk1;
                                        item.Atk3 = item.Atk1;
                                        DeleteItemID(90000054, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000055:
                                        item.MAtk = FindEnhancementValue(item, 90000055);
                                        DeleteItemID(90000055, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 90000056:
                                        item.HitCritical = FindEnhancementValue(item, 90000056);
                                        DeleteItemID(90000056, 1, true);
                                        item.CritEnhance++;
                                        break;

                                    //超強化水晶
                                    case 16004700:
                                        item.Atk1 = FindEnhancementValue(item, 16004700);
                                        item.Atk2 = item.Atk1;
                                        item.Atk3 = item.Atk1;
                                        DeleteItemID(16004700, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 16004800:
                                        item.MAtk = FindEnhancementValue(item, 16004800);
                                        DeleteItemID(16004800, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 16004500:
                                        item.HitCritical = FindEnhancementValue(item, 16004500);
                                        DeleteItemID(16004500, 1, true);
                                        item.CritEnhance++;
                                        break;

                                    //強化王水晶
                                    case 10087401:
                                        item.Atk1 = FindEnhancementValue(item, 10087401);
                                        item.Atk2 = item.Atk1;
                                        item.Atk3 = item.Atk1;
                                        DeleteItemID(10087401, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 10087403:
                                        item.MAtk = FindEnhancementValue(item, 10087403);
                                        DeleteItemID(10087403, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 10087402:
                                        item.HitCritical = FindEnhancementValue(item, 10087402);
                                        DeleteItemID(10087402, 1, true);
                                        item.CritEnhance++;
                                        break;
                                }

                            if (item.BaseData.itemType == ItemType.SHIELD)
                                switch (p.Material)
                                {
                                    //一般水晶
                                    case 90000044:
                                        item.Def = FindEnhancementValue(item, 90000044);
                                        DeleteItemID(90000044, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000045:
                                        item.MDef = FindEnhancementValue(item, 90000045);
                                        DeleteItemID(90000045, 1, true);
                                        item.MagEnhance++;
                                        break;

                                    //強化水晶
                                    case 90000054:
                                        item.Def = FindEnhancementValue(item, 90000054);
                                        DeleteItemID(90000054, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 90000055:
                                        item.MDef = FindEnhancementValue(item, 90000055);
                                        DeleteItemID(90000055, 1, true);
                                        item.MagEnhance++;
                                        break;
                                        ;

                                    //超強化水晶
                                    case 16004700:
                                        item.Def = FindEnhancementValue(item, 16004700);
                                        DeleteItemID(16004700, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 16004800:
                                        item.MDef = FindEnhancementValue(item, 16004800);
                                        DeleteItemID(16004800, 1, true);
                                        item.MagEnhance++;
                                        break;

                                    //強化王
                                    case 10087401:
                                        item.Def = FindEnhancementValue(item, 10087401);
                                        DeleteItemID(10087401, 1, true);
                                        item.PowerEnhance++;
                                        break;
                                    case 10087403:
                                        item.MDef = FindEnhancementValue(item, 10087403);
                                        DeleteItemID(10087403, 1, true);
                                        item.MagEnhance++;
                                        break;
                                }

                            if (item.BaseData.itemType == ItemType.ACCESORY_NECK)
                                switch (p.Material)
                                {
                                    case 90000045:
                                        item.MDef = FindEnhancementValue(item, 90000045);
                                        DeleteItemID(90000045, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 90000055:
                                        item.MDef = FindEnhancementValue(item, 90000055);
                                        DeleteItemID(90000055, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 16004800:
                                        item.MDef = FindEnhancementValue(item, 16004800);
                                        DeleteItemID(16004800, 1, true);
                                        item.MagEnhance++;
                                        break;
                                    case 10087403:
                                        item.MDef = FindEnhancementValue(item, 10087403);
                                        DeleteItemID(10087403, 1, true);
                                        item.MagEnhance++;
                                        break;
                                }

                            SendEffect(5145);
                            p1.Result = 1;

                            item.Refine++;
                            SendItemInfo(item);
                            StatusFactory.Instance.CalcStatus(Character);
                            SendPlayerInfo();
                        }
                        else
                        {
                            failed = true;
                            SendEffect(5146);
                            if (!enhanceprotected)
                            {
                                DeleteItem(p.InventorySlot, 1, true);

                                //Delete Material..
                                if (p.Material >= 10087400 && p.Material <= 10087403)
                                {
                                    //No Delete Material
                                }
                                else if (p.Material >= 10087400 && p.Material <= 10087403)
                                {
                                    //No Delete Material
                                }
                                else
                                {
                                    DeleteItemID(p.Material, 1, true);
                                }
                            }
                            else
                            {
                                //Delete Material..
                                if (p.Material >= 10087400 && p.Material <= 10087403)
                                {
                                    //No Delete Material
                                }
                                else if (p.Material >= 10087400 && p.Material <= 10087403)
                                {
                                    //No Delete Material
                                }
                                else
                                {
                                    DeleteItemID(p.Material, 1, true);
                                }

                                //Reset Protected Only
                                if (supportitemid != 10118200)
                                {
                                    item.Clear();
                                    SendItemInfo(item);
                                }

                                //Explode Protected Only
                                if (protectitemid != 16001300)
                                    DeleteItem(p.InventorySlot, 1, true);

                                StatusFactory.Instance.CalcStatus(Character);
                                SendPlayerInfo();
                                failed = true;
                            }

                            p1.Result = 0;
                            p1.OrignalRefine = (ushort)(item.Refine + 1);
                            p1.ExpectedRefine = item.Refine;
                            NetIo.SendPacket(p1);
                            p1.Result = 0x00;
                        }
                    }
                    else
                    {
                        p1.Result = 0x00;
                        p1.OrignalRefine = item.Refine;
                        p1.ExpectedRefine = (ushort)(item.Refine + 1);
                        NetIo.SendPacket(p1);
                        p1.Result = 0xff;
                    }
                }
                else
                {
                    p1.Result = 0x00;
                    p1.OrignalRefine = item.Refine;
                    p1.ExpectedRefine = (ushort)(item.Refine + 1);
                    NetIo.SendPacket(p1);
                    p1.Result = 0xfd;
                }
            }
            else
            {
                p1.Result = 0x00;
                p1.OrignalRefine = item.Refine;
                p1.ExpectedRefine = (ushort)(item.Refine + 1);
                NetIo.SendPacket(p1);
                p1.Result = 0xfe;
            }

            if ((item.IsArmor && (CountItem(90000044) > 0 || CountItem(90000045) > 0 || CountItem(90000046) > 0 ||
                                  CountItem(90000043) > 0 || CountItem(10087400) > 0 || CountItem(10087401) > 0 ||
                                  CountItem(10087402) > 0 || CountItem(10087403) > 0))
                || (item.IsWeapon && (CountItem(90000044) > 0 || CountItem(90000045) > 0 || CountItem(90000046) > 0 ||
                                      CountItem(10087401) > 0)) || CountItem(10087402) > 0 || CountItem(10087403) > 0
                || (item.BaseData.itemType == ItemType.SHIELD && (CountItem(90000044) > 0 || CountItem(90000045) > 0 ||
                                                                  CountItem(10087401) > 0 || CountItem(10087403) > 0))
                || (item.BaseData.itemType == ItemType.ACCESORY_NECK &&
                    (CountItem(90000044) > 0 || CountItem(10087403) > 0) && item.Refine < 30 && Character.Gold >= 5000))
            {
                NetIo.SendPacket(p1);
                var p2 = new CSMG_ITEM_ENHANCE_SELECT();
                p2.InventorySlot = p.InventorySlot;
                OnItemEnhanceSelect(p2);
                return;
            }

            p1.Result = 0x00;
            p1.OrignalRefine = item.Refine;
            p1.ExpectedRefine = (ushort)(item.Refine + 1);
            NetIo.SendPacket(p1);
            p1.Result = 0xfd;

            NetIo.SendPacket(p1);

            if (failed)
            {
                var res = from enhanctitem in Character.Inventory.GetContainer(ContainerType.BODY)
                          where (enhanctitem.IsArmor && (CountItem(90000044) > 0 || CountItem(90000045) > 0 ||
                                                         CountItem(90000046) > 0 || CountItem(90000043) > 0 ||
                                                         CountItem(10087400) > 0 || CountItem(10087401) > 0 ||
                                                         CountItem(10087402) > 0 || CountItem(10087403) > 0))
                                || (enhanctitem.IsWeapon && (CountItem(90000044) > 0 || CountItem(90000045) > 0 ||
                                                             CountItem(90000046) > 0 || CountItem(10087401) > 0)) ||
                                CountItem(10087402) > 0 || CountItem(10087403) > 0
                                || (enhanctitem.BaseData.itemType == ItemType.SHIELD && (CountItem(90000044) > 0 ||
                                    CountItem(90000045) > 0 || CountItem(10087401) > 0 || CountItem(10087403) > 0))
                                || (enhanctitem.BaseData.itemType == ItemType.ACCESORY_NECK &&
                                    (CountItem(90000044) > 0 || CountItem(10087403) > 0) && item.Refine < 30 &&
                                    Character.Gold >= 5000)
                          select enhanctitem;
                var items = res.ToList();

                foreach (var itemsitem in res.ToList())
                {
                    if (itemsitem.IsArmor)
                    {
                        //生命结晶
                        if (CountItem(90000043) > 0)
                            if (!items.Exists(x => x.ItemID == 90000043))
                                items.AddRange(GetItem(90000043));
                        //力量结晶
                        if (CountItem(90000044) > 0)
                            if (!items.Exists(x => x.ItemID == 90000044))
                                items.AddRange(GetItem(90000044));
                        //会心结晶
                        if (CountItem(90000046) > 0)
                            if (!items.Exists(x => x.ItemID == 90000046))
                                items.AddRange(GetItem(90000046));
                        //魔力结晶
                        if (CountItem(90000045) > 0)
                            if (!items.Exists(x => x.ItemID == 90000045))
                                items.AddRange(GetItem(90000045));

                        //生命強化结晶
                        if (CountItem(90000053) > 0)
                            if (!items.Exists(x => x.ItemID == 90000053))
                                items.AddRange(GetItem(90000053));
                        //力量強化结晶
                        if (CountItem(90000054) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))
                                items.AddRange(GetItem(90000054));
                        //会心強化结晶
                        if (CountItem(90000056) > 0)
                            if (!items.Exists(x => x.ItemID == 90000056))
                                items.AddRange(GetItem(90000056));
                        //魔力強化结晶
                        if (CountItem(90000055) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //生命超強化结晶
                        if (CountItem(16004600) > 0)
                            if (!items.Exists(x => x.ItemID == 90000053))
                                items.AddRange(GetItem(90000053));
                        //力量超強化结晶
                        if (CountItem(16004700) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))
                                items.AddRange(GetItem(90000054));
                        //会心超強化结晶
                        if (CountItem(16004500) > 0)
                            if (!items.Exists(x => x.ItemID == 90000056))
                                items.AddRange(GetItem(90000056));
                        //魔力超強化结晶
                        if (CountItem(16004800) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //强化王的生命
                        if (CountItem(10087400) > 0)
                            if (!items.Exists(x => x.ItemID == 10087400))
                                items.AddRange(GetItem(10087400));
                        //强化王的力量
                        if (CountItem(10087401) > 0)
                            if (!items.Exists(x => x.ItemID == 10087401))
                                items.AddRange(GetItem(10087401));
                        //强化王的会心
                        if (CountItem(10087402) > 0)
                            if (!items.Exists(x => x.ItemID == 10087402))
                                items.AddRange(GetItem(10087402));
                        //强化王的魔力
                        if (CountItem(10087403) > 0)
                            if (!items.Exists(x => x.ItemID == 10087403))
                                items.AddRange(GetItem(10087403));
                    }

                    if (itemsitem.IsWeapon)
                    {
                        //力量结晶
                        if (CountItem(90000044) > 0)
                            if (!items.Exists(x => x.ItemID == 90000044))
                                items.AddRange(GetItem(90000044));
                        //会心结晶
                        if (CountItem(90000046) > 0)
                            if (!items.Exists(x => x.ItemID == 90000046))
                                items.AddRange(GetItem(90000046));
                        //魔力结晶
                        if (CountItem(90000045) > 0)
                            if (!items.Exists(x => x.ItemID == 90000045))
                                items.AddRange(GetItem(90000045));


                        //力量強化结晶
                        if (CountItem(90000054) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))
                                items.AddRange(GetItem(90000054));
                        //会心強化结晶
                        if (CountItem(90000056) > 0)
                            if (!items.Exists(x => x.ItemID == 90000056))
                                items.AddRange(GetItem(90000056));
                        //魔力強化结晶
                        if (CountItem(90000055) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //力量超強化结晶
                        if (CountItem(16004700) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))
                                items.AddRange(GetItem(90000054));
                        //会心超強化结晶
                        if (CountItem(16004500) > 0)
                            if (!items.Exists(x => x.ItemID == 90000056))
                                items.AddRange(GetItem(90000056));
                        //魔力超強化结晶
                        if (CountItem(16004800) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //强化王的力量
                        if (CountItem(10087401) > 0)
                            if (!items.Exists(x => x.ItemID == 10087401))
                                items.AddRange(GetItem(10087401));
                        //强化王的会心
                        if (CountItem(10087402) > 0)
                            if (!items.Exists(x => x.ItemID == 10087402))
                                items.AddRange(GetItem(10087402));
                        //强化王的魔力
                        if (CountItem(10087403) > 0)
                            if (!items.Exists(x => x.ItemID == 10087403))
                                items.AddRange(GetItem(10087403));
                    }

                    if (itemsitem.BaseData.itemType == ItemType.SHIELD)
                    {
                        //力量结晶
                        if (CountItem(90000044) > 0)
                            if (!items.Exists(x => x.ItemID == 90000044))
                                items.AddRange(GetItem(90000044));

                        //魔力结晶
                        if (CountItem(90000045) > 0)
                            if (!items.Exists(x => x.ItemID == 90000045))
                                items.AddRange(GetItem(90000045));


                        //力量強化结晶
                        if (CountItem(90000054) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))

                                //魔力強化结晶
                                if (CountItem(90000055) > 0)
                                    if (!items.Exists(x => x.ItemID == 90000055))
                                        items.AddRange(GetItem(90000055));


                        //力量超強化结晶
                        if (CountItem(16004700) > 0)
                            if (!items.Exists(x => x.ItemID == 90000054))
                                items.AddRange(GetItem(90000054));

                        //魔力超強化结晶
                        if (CountItem(16004800) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //强化王的力量
                        if (CountItem(10087401) > 0)
                            if (!items.Exists(x => x.ItemID == 10087401))
                                items.AddRange(GetItem(10087401));

                        //强化王的魔力
                        if (CountItem(10087403) > 0)
                            if (!items.Exists(x => x.ItemID == 10087403))
                                items.AddRange(GetItem(10087403));
                    }

                    if (itemsitem.BaseData.itemType == ItemType.ACCESORY_NECK)
                    {
                        //魔力结晶
                        if (CountItem(90000045) > 0)
                            if (!items.Exists(x => x.ItemID == 90000045))
                                items.AddRange(GetItem(90000045));


                        //魔力強化结晶
                        if (CountItem(90000055) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //魔力超強化结晶
                        if (CountItem(16004800) > 0)
                            if (!items.Exists(x => x.ItemID == 90000055))
                                items.AddRange(GetItem(90000055));


                        //强化王的魔力
                        if (CountItem(10087403) > 0)
                            if (!items.Exists(x => x.ItemID == 10087403))
                                items.AddRange(GetItem(10087403));
                    }

                    //Golbal Item

                    //防爆判断
                    if (CountItem(16001300) > 0)
                        if (!items.Exists(x => x.ItemID == 16001300))
                            items.AddRange(GetItem(16001300));

                    //防RESET判断
                    if (CountItem(10118200) > 0)
                        if (!items.Exists(x => x.ItemID == 10118200))
                            items.AddRange(GetItem(10118200));


                    //奥义判断
                    if (CountItem(10087200) > 0)
                        if (!items.Exists(x => x.ItemID == 10087200))
                            items.AddRange(GetItem(10087200));
                    //精髓判断
                    if (CountItem(10087201) > 0)
                        if (!items.Exists(x => x.ItemID == 10087201))
                            items.AddRange(GetItem(10087201));
                }

                items = items.OrderBy(x => x.Slot).ToList();
                if (items.Count > 0)
                {
                    var p2 = new SSMG_ITEM_ENHANCE_LIST();
                    p2.Items = items;
                    NetIo.SendPacket(p2);
                }
                else
                {
                    itemEnhance = false;
                    p1 = new SSMG_ITEM_ENHANCE_RESULT();
                    p1.Result = 0x00;
                    p1.OrignalRefine = item.Refine;
                    p1.ExpectedRefine = (ushort)(item.Refine + 1);
                    NetIo.SendPacket(p1);
                    p1.Result = 0xfd;
                    NetIo.SendPacket(p1);
                }
            }
        }

        public void OnItemUse(CSMG_ITEM_USE p)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            var item = Character.Inventory.GetItem(p.InventorySlot);
            var arg = new SkillArg();
            arg.sActor = Character.ActorID;
            arg.dActor = p.ActorID;
            arg.item = item;
            arg.x = p.X;
            arg.y = p.Y;
            arg.argType = SkillArg.ArgType.Item_Cast;
            arg.inventorySlot = p.InventorySlot;
            if (item == null)
                return;
            var dActor = this.map.GetActor(p.ActorID);

            if (Character.Account.GMLevel > 0)
                FromActorPC(Character).SendSystemMessage("道具ID：" + item.ItemID);

            if (item.BaseData.itemType == ItemType.POTION || item.BaseData.itemType == ItemType.FOOD)
                if (Character.Status.Additions.ContainsKey("FOODCD"))
                {
                    FromActorPC(Character).SendSystemMessage("暂时吃不下食物了哦...(30秒CD)");
                    arg.result = -21;
                }

            //Skill.Additions.Global.DefaultBuff cd = new Skill.Additions.Global.DefaultBuff(Character, "FOODCD", 30000);
            //SkillHandler.ApplyAddition(Character, cd);
            if (Character.PossessionTarget != 0)
            {
                var posse = Map.GetActor(Character.PossessionTarget);
                if (posse != null)
                {
                    if (posse.type == ActorType.PC)
                    {
                        if (arg.dActor == Character.ActorID)
                            arg.dActor = posse.ActorID;
                    }
                    else
                    {
                        arg.result = -21;
                    }
                }
            }

            if (item.BaseData.itemType == ItemType.MARIONETTE && arg.result == 0)
            {
                if (Character.Marionette == null)
                    if (DateTime.Now < Character.NextMarionetteTime)
                        arg.result = -18;
                if (Character.Pet != null)
                    if (Character.Pet.Ride)
                        arg.result = -32;
                if (Character.PossessionTarget != 0 || Character.PossesionedActors.Count > 0) arg.result = -16;
                if (Character.Race == PC_RACE.DEM) arg.result = -33;
            }

            if (GetPossessionTarget() != null && arg.result == 0)
            {
                if (GetPossessionTarget().Buff.Dead && !(item.ItemID == 10000604 || item.ItemID == 10034104))
                    arg.result = -27;
                if (arg.result == 0)
                    if (item.ItemID == 10022900)
                        arg.result = -3;
            }

            if (dActor != null && arg.result == 0)
                if (!dActor.Buff.Dead && (item.ItemID == 10000604 || item.ItemID == 10034104))
                    arg.result = -23;

            if (scriptThread != null && arg.result == 0) arg.result = -7;

            if (Character.Buff.Dead && arg.result == 0) arg.result = -9;
            if (Character.Buff.GetReadyPossession && arg.result == 0)
                arg.result = -3;

            if (arg.result == 0)
                if (Character.Tasks.ContainsKey("ItemCast"))
                    arg.result = -19;
            if (arg.result == 0)
            {
                if (item.BaseData.itemType == ItemType.UNION_FOOD)
                    if (!OnPartnerFeed(item.Slot))
                        return;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, Character, true);
                var casttime = item.BaseData.cast;
                if (item.BaseData.itemType == ItemType.POTION || item.BaseData.itemType == ItemType.FOOD)
                    casttime = 2000;
                if (item.BaseData.cast > 0)
                {
                    var task = new SkillCast(this, arg);
                    Character.Tasks.Add("ItemCast", task);
                    task.Activate();
                }
                else
                {
                    OnItemCastComplete(arg);
                }

                //Cancel Cloak
                if (Character.Status.Additions.ContainsKey("Cloaking"))
                    SkillHandler.RemoveAddition(Character, "Cloaking");


                if (Character.PossessionTarget != 0)
                {
                    var map = MapManager.Instance.GetMap(Character.MapID);
                    var TargetPossessionActor = map.GetActor(Character.PossessionTarget);

                    if (TargetPossessionActor.Status.Additions.ContainsKey("Cloaking"))
                        SkillHandler.RemoveAddition(TargetPossessionActor, "Cloaking");
                }
            }
            else
            {
                Character.e.OnActorSkillUse(Character, arg);
            }
        }

        public void OnItemCastComplete(SkillArg skill)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (skill.dActor != 0xFFFFFFFF)
            {
                var dActor = Map.GetActor(skill.dActor);
                Character.Tasks.Remove("ItemCast");
                skill.argType = SkillArg.ArgType.Item_Active;
                SkillHandler.Instance.ItemUse(Character, dActor, skill);
            }
            else
            {
                Character.Tasks.Remove("ItemCast");
                skill.argType = SkillArg.ArgType.Item_Active;
            }

            if (skill.item.BaseData.usable || skill.item.BaseData.itemType == ItemType.POTION ||
                skill.item.BaseData.itemType == ItemType.SCROLL ||
                skill.item.BaseData.itemType == ItemType.FREESCROLL)
            {
                if (skill.item.Durability > 0)
                    skill.item.Durability--;
                SendItemInfo(skill.item);
                if (skill.item.Durability == 0)
                {
                    Logger.LogItemLost(Logger.EventType.ItemUseLost, Character.Name + "(" + Character.CharID + ")",
                        skill.item.BaseData.name + "(" + skill.item.ItemID + ")",
                        string.Format("ItemUse Count:{0}", 1), false);
                    DeleteItem(skill.inventorySlot, 1, true);
                }
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, skill, Character, true);
            if (skill.item.BaseData.effectID != 0)
            {
                var eff = new EffectArg();
                eff.actorID = skill.dActor;
                eff.effectID = skill.item.BaseData.effectID;
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, eff, Character, true);
            }

            if (skill.item.ItemID >= 10047800 && skill.item.ItemID <= 10047852)
            {
                OnItemRepair(skill.item);
            }
            else if (skill.item.BaseData.activateSkill != 0)
            {
                var p1 = new CSMG_SKILL_CAST();
                p1.ActorID = skill.dActor;
                p1.SkillID = skill.item.BaseData.activateSkill;
                p1.SkillLv = 1;
                p1.X = skill.x;
                p1.Y = skill.y;
                p1.Random = (short)Global.Random.Next();
                OnSkillCast(p1, true, true);
            }

            if (skill.item.BaseData.abnormalStatus.ContainsKey(AbnormalStatus.Poisen))
                if (skill.item.BaseData.abnormalStatus[AbnormalStatus.Poisen] == 100)
                    if (Character.Status.Additions.ContainsKey("Poison"))
                    {
                        Character.Status.Additions["Poison"].AdditionEnd();
                        Character.Status.Additions.Remove("Poison");
                    }

            if (skill.item.BaseData.itemType == ItemType.MARIONETTE)
            {
                if (Character.Marionette == null)
                {
                    MarionetteActivate(skill.item.BaseData.marionetteID);
                }
                else
                {
                    if (!Character.Status.Additions.ContainsKey("ChangeMarionette"))
                        MarionetteDeactivate();
                    else
                        MarionetteActivate(skill.item.BaseData.marionetteID, false, false);
                    return;
                }
            }

            if (skill.item.BaseData.eventID != 0)
            {
                if (skill.item.BaseData.eventID == 90000529)
                    Character.TInt["技能块ItemID"] = (int)skill.item.ItemID;
                EventActivate(skill.item.BaseData.eventID);
            }

            if (skill.item.ItemID > kujiboxID0 && skill.item.ItemID <= kujiboxID0 + kujinum_max)
            {
                DeleteItem(skill.inventorySlot, 1, false);
                OnKujiBoxOpen(skill.item);
            }

            if (skill.item.BaseData.itemType == ItemType.GOLEM)
            {
                if (Character.Golem == null)
                    Character.Golem = new ActorGolem();
                Character.Golem.Item = skill.item;
                EventActivate(0xFFFFFF33);
            }
        }

        private int GetKujiRare(List<Kuji> kuji)
        {
            //
            var min = int.MaxValue;
            for (var i = 0; i < kuji.Count; i++) min = Math.Min(min, kuji[0].rank);
            return min;
        }

        private void OnKujiBoxOpen(Item box)
        {
            var kujiID = box.ItemID - kujiboxID0;

            if (KujiListFactory.Instance.KujiList.ContainsKey(kujiID))
            {
                var kujis = KujiListFactory.Instance.KujiList[kujiID];
                if (kujis.Count == 0) return;
                var rare = GetKujiRare(KujiListFactory.Instance.KujiList[kujiID]);

                var rates = new List<int>();
                var r = 0;
                for (var i = 0; i < kujis.Count; i++)
                {
                    r = r + kujis[i].rate;
                    rates.Add(r);
                }

                SkillHandler.Instance.ShowEffectOnActor(Character, 8056);
                var ratemin = 0;
                var ratemax = rates[rates.Count - 1];
                var ran = Global.Random.Next(ratemin, ratemax);
                for (var i = 0; i < kujis.Count; i++)
                    if (ran <= rates[i])
                    {
                        switch (kujis[i].rank)
                        {
                            case 1:
                                Character.AInt["SSS保底次数"] = 0;
                                SendSystemMessage("啧，可恶的欧洲人");
                                break;
                            case 2:
                            case 3:
                                Character.AInt["SS保底次数"] = 0;
                                SendSystemMessage("嘁，可恶的欧洲人");
                                break;
                            default:
                                switch (rare)
                                {
                                    case 1:
                                        Character.AInt["SSS保底次数"]++;
                                        SendSystemMessage("如果连续200次未抽到SSS级头赏，将获得彩虹钥匙。当前次数：" +
                                                          Character.AInt["SSS保底次数"] + "/200");
                                        if (Character.AInt["SSS保底次数"] >= 200)
                                        {
                                            Character.AInt["SSS保底次数累计"]++;
                                            Character.AInt["SSS保底次数"] = 0;
                                            SendSystemMessage("由于您连续200次未抽到SSS级头赏，获得了彩虹钥匙。");
                                            var item = ItemFactory.Instance.GetItem(950000032);
                                            AddItem(item, true);
                                        }

                                        break;
                                    case 2:
                                    case 3:
                                        Character.AInt["SS保底次数"]++;
                                        SendSystemMessage("如果连续200次未抽到SS/S级头赏，将获得金钥匙。当前次数：" + Character.AInt["SS保底次数"] +
                                                          "/200");
                                        if (Character.AInt["SS保底次数"] >= 200)
                                        {
                                            Character.AInt["SS保底次数累计"]++;
                                            Character.AInt["SS保底次数"] = 0;
                                            SendSystemMessage("由于您连续200次未抽到SS/S级头赏，获得了金钥匙。");
                                            var item = ItemFactory.Instance.GetItem(950000031);
                                            AddItem(item, true);
                                            TitleProccess(Character, 8, 1);
                                        }

                                        break;
                                }

                                break;
                        }

                        var kuji = ItemFactory.Instance.GetItem(kujis[i].itemid);
                        AddItem(kuji, true);
                        break;
                    }
            }
        }

        public void OnItemRepair(Item item)
        {
            var RepairItems = new List<Item>();
            foreach (var i in Character.Inventory.Items)
                foreach (var items in i.Value)
                    if (items.BaseData.repairItem == item.BaseData.id)
                        RepairItems.Add(items);

            var p = new SSMG_ITEM_EQUIP_REPAIR_LIST();
            p.Items = RepairItems;
            NetIo.SendPacket(p);
        }

        public void OnItemDrop(CSMG_ITEM_DROP p)
        {
            if (Character.Account.AccountID > 200)
            {
                var itemDroped2 = Character.Inventory.GetItem(p.InventorySlot);
                SendSystemMessage(itemDroped2.BaseData.id.ToString());
            }

            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding")) Character.Status.Additions["Hiding"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("Cloaking"))
                Character.Status.Additions["Cloaking"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            var itemDroped = Character.Inventory.GetItem(p.InventorySlot);
            var count = p.Count;
            if (count > itemDroped.Stack)
                count = itemDroped.Stack;
            var p1 = new SSMG_ITEM_PUT_ERROR();
            if (itemDroped.BaseData.events == 1)
            {
                p1.ErrorID = -3;
                NetIo.SendPacket(p1);
                return;
            }

            if (trading)
            {
                p1.ErrorID = -8;
                NetIo.SendPacket(p1);
                return;
            }

            if (itemDroped.BaseData.noTrade)
            {
                p1.ErrorID = -16;
                NetIo.SendPacket(p1);
                return;
            }

            if (itemDroped.BaseData.itemType == ItemType.DEMIC_CHIP)
            {
                p1.ErrorID = -18;
                NetIo.SendPacket(p1);
                return;
            }

            if (itemDroped.Stack > 0)
                Logger.LogItemLost(Logger.EventType.ItemDropLost, Character.Name + "(" + Character.CharID + ")",
                    itemDroped.BaseData.name + "(" + itemDroped.ItemID + ")",
                    string.Format("Drop Count:{0}", count), false);

            var result = Character.Inventory.DeleteItem(p.InventorySlot, count);
            switch (result)
            {
                case InventoryDeleteResult.STACK_UPDATED:
                    var p2 = new SSMG_ITEM_COUNT_UPDATE();
                    var item = Character.Inventory.GetItem(p.InventorySlot);
                    itemDroped = item.Clone();
                    itemDroped.Stack = count;
                    p2.InventorySlot = p.InventorySlot;
                    p2.Stack = item.Stack;
                    NetIo.SendPacket(p2);
                    break;
                case InventoryDeleteResult.ALL_DELETED:
                    var p3 = new SSMG_ITEM_DELETE();
                    p3.InventorySlot = p.InventorySlot;
                    NetIo.SendPacket(p3);
                    break;
            }

            var actor = new ActorItem(itemDroped);
            actor.e = new ItemEventHandler(actor);
            actor.MapID = Character.MapID;
            actor.X = Character.X;
            actor.Y = Character.Y;
            if (!itemDroped.BaseData.noTrade) //7月27日更新，取消交易
            {
                actor.Owner = Character;
                actor.CreateTime = DateTime.Now;
            }

            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            var task = new DeleteItem(actor);
            task.Activate();
            actor.Tasks.Add("DeleteItem", task);

            Character.Inventory.CalcPayloadVolume();
            SendCapacity();

            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_DELETED, itemDroped.BaseData.name,
                itemDroped.Stack));
        }

        public void OnItemGet(CSMG_ITEM_GET p)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding")) Character.Status.Additions["Hiding"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("Cloaking"))
                Character.Status.Additions["Cloaking"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            var item = (ActorItem)map.GetActor(p.ActorID);
            if (item == null)
                return;
            if (item.Owner != null)
                if (item.Owner.type == ActorType.PC)
                {
                    var pc = (ActorPC)item.Owner;
                    if (pc != Character && !item.Roll)
                    {
                        if (pc.Party != null && !item.Party)
                        {
                            if (!pc.Party.IsMember(Character) && !item.Party)
                                if ((DateTime.Now - item.CreateTime).TotalMinutes < 1)
                                {
                                    var p1 = new SSMG_ITEM_GET_ERROR();
                                    p1.ActorID = item.ActorID;
                                    p1.ErrorID = -10;
                                    NetIo.SendPacket(p1);
                                    return;
                                }
                        }
                        else
                        {
                            if ((DateTime.Now - item.CreateTime).TotalSeconds < 30 || item.Party)
                            {
                                var p1 = new SSMG_ITEM_GET_ERROR();
                                p1.ActorID = item.ActorID;
                                p1.ErrorID = -10;
                                NetIo.SendPacket(p1);
                                return;
                            }
                        }
                    }
                }

            if (!item.PossessionItem)
            {
                if (Character.Party != null)
                {
                    if (item.Roll || (Character.Party.Roll == 0 && Character.Party != null))
                    {
                        var mes = true;
                        if (Character.Party.Roll == 0) mes = false;
                        if (item.Roll) mes = true;
                        var winner = Character;
                        var MaxRate = 0;
                        foreach (var it in Character.Party.Members.Values)
                            if (it.MapID == Character.MapID && it.Online)
                            {
                                var rate = Global.Random.Next(0, 100);
                                if (rate > MaxRate)
                                {
                                    winner = it;
                                    MaxRate = rate;
                                }

                                foreach (var item2 in Character.Party.Members.Values)
                                    if (mes && item2.Online)
                                        FromActorPC(item2).SendSystemMessage(it.Name + " 的拾取点数为:" + rate);
                            }

                        var a = "";
                        if (mes)
                            a = "的点数最大，";
                        foreach (var item2 in Character.Party.Members.Values)
                            if (item2.Online)
                                FromActorPC(item2).SendSystemMessage(winner.Name + a + " 获得了物品[" + item.Name + "]" +
                                                                     item.Item.Stack + "个。");
                        item.LootedBy = winner.ActorID;
                        map.DeleteActor(item);
                        Logger.LogItemGet(Logger.EventType.ItemLootGet, Character.Name + "(" + Character.CharID + ")",
                            item.Item.BaseData.name + "(" + item.Item.ItemID + ")",
                            string.Format("ItemLoot Count:{0}", item.Item.Stack), false);
                        FromActorPC(winner).AddItem(item.Item, true);

                        item.Tasks["DeleteItem"].Deactivate();
                        item.Tasks.Remove("DeleteItem");
                    }
                    else
                    {
                        item.LootedBy = Character.ActorID;
                        map.DeleteActor(item);
                        Logger.LogItemGet(Logger.EventType.ItemLootGet, Character.Name + "(" + Character.CharID + ")",
                            item.Item.BaseData.name + "(" + item.Item.ItemID + ")",
                            string.Format("ItemLoot Count:{0}", item.Item.Stack), false);
                        AddItem(item.Item, true);

                        item.Tasks["DeleteItem"].Deactivate();
                        item.Tasks.Remove("DeleteItem");
                    }
                }
                else
                {
                    item.LootedBy = Character.ActorID;
                    map.DeleteActor(item);
                    Logger.LogItemGet(Logger.EventType.ItemLootGet, Character.Name + "(" + Character.CharID + ")",
                        item.Item.BaseData.name + "(" + item.Item.ItemID + ")",
                        string.Format("ItemLoot Count:{0}", item.Item.Stack), false);
                    AddItem(item.Item, true);

                    item.Tasks["DeleteItem"].Deactivate();
                    item.Tasks.Remove("DeleteItem");
                }
            }
            else
            {
                foreach (var i in item.Item.EquipSlot)
                    if (Character.Inventory.Equipments.ContainsKey(i))
                    {
                        var p1 = new SSMG_ITEM_GET_ERROR();
                        p1.ActorID = item.ActorID;
                        p1.ErrorID = -5;
                        NetIo.SendPacket(p1);
                        return;
                    }

                if (Character.Race == PC_RACE.DEM && Character.Form == DEM_FORM.MACHINA_FORM)
                {
                    var p1 = new SSMG_ITEM_GET_ERROR();
                    p1.ActorID = item.ActorID;
                    p1.ErrorID = -16;
                    NetIo.SendPacket(p1);
                    return;
                }

                if (Math.Abs(Character.Level - item.Item.PossessionedActor.Level) > 30)
                {
                    var p1 = new SSMG_ITEM_GET_ERROR();
                    p1.ActorID = item.ActorID;
                    p1.ErrorID = -4;
                    NetIo.SendPacket(p1);
                    return;
                }

                var result = CheckEquipRequirement(item.Item);
                if (result < 0)
                {
                    var p4 = new SSMG_ITEM_EQUIP();
                    p4.InventorySlot = 0xffffffff;
                    p4.Target = ContainerType.NONE;
                    p4.Result = result;
                    p4.Range = Character.Range;
                    NetIo.SendPacket(p4);
                    return;
                }

                //临时处理手段
                SendSystemMessage("自凭依道具暂时无法捡起来");
                //MapClient.FromActorPC(PC).SendSystemMessage("伙伴 " + partner.Name + " 获得了" + exp + "点经验值");
                //item.LootedBy = this.Character.ActorID;
                //this.map.DeleteActor(item);
                //Item addItem = item.Item.Clone();
                //AddItem(addItem, true);
                //Packets.Client.CSMG_ITEM_EQUIPT p2 = new SagaMap.Packets.Client.CSMG_ITEM_EQUIPT();
                //p2.InventoryID = addItem.Slot;
                //OnItemEquipt(p2);
            }

            //this.SendItems();

            SendPlayerInfo();
        }

        public int CheckEquipRequirement(Item item)
        {
            if (Character.Buff.Dead || Character.Buff.Confused || Character.Buff.Frosen || Character.Buff.Paralysis ||
                Character.Buff.Sleep || Character.Buff.Stone || Character.Buff.Stun)
                return -3;
            switch (item.BaseData.itemType)
            {
                case ItemType.ARROW:
                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType != ItemType.BOW)
                            return -6;
                    }
                    else
                    {
                        return -6;
                    }

                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        var oriItem = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        if (oriItem.PossessionedActor != null) return -10;
                    }

                    break;
                case ItemType.BULLET:
                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType !=
                            ItemType.GUN &&
                            Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType !=
                            ItemType.RIFLE &&
                            Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType !=
                            ItemType.DUALGUN)
                            return -7;
                    }
                    else
                    {
                        return -7;
                    }

                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        var oriItem = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                        if (oriItem.PossessionedActor != null) return -10;
                    }

                    break;
            }

            if (item.Durability < 1 && item.maxDurability >= 1)
                return -12;
            if (Character.Str < item.BaseData.possibleStr)
                return -16;
            if (Character.Dex < item.BaseData.possibleDex)
                return -19;
            if (Character.Agi < item.BaseData.possibleAgi)
                return -20;
            if (Character.Vit < item.BaseData.possibleVit)
                return -18;
            if (Character.Int < item.BaseData.possibleInt)
                return -21;
            if (Character.Mag < item.BaseData.possibleMag)
                return -17;
            if (!item.BaseData.possibleRace[Character.Race])
                return -13;
            if (!item.BaseData.possibleGender[Character.Gender])
                return -14;
            var lv = Character.Level;
            if (Character.Rebirth)
            {
                if (lv < item.BaseData.possibleLv - 10)
                    return -15;
            }
            else if (lv < item.BaseData.possibleLv)
            {
                return -15;
            }

            if ((item.BaseData.itemType == ItemType.RIDE_PET || item.BaseData.itemType == ItemType.RIDE_PARTNER) &&
                Character.Marionette != null)
                return -2;
            if (item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND) || item.EquipSlot.Contains(EnumEquipSlot.LEFT_HAND))
            {
                if (Character.Skills3.ContainsKey(990)) return 0;
            }
            else
            {
                if (Character.Skills3.ContainsKey(991)) return 0;
            }

            if (!item.IsParts && Character.Race != PC_RACE.DEM)
            {
                if (Character.JobJoint == PC_JOB.NONE)
                {
                    if (Character.DualJobID != 0)
                    {
                        var dualjobinfo = DualJobInfoFactory.Instance.items[Character.DualJobID];
                        if (!item.BaseData.possibleJob[Character.Job])
                            if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.BaseJobID])
                                if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.ExperJobID])
                                    if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.TechnicalJobID])
                                        if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.ChronicleJobID])
                                            return -2;
                    }
                    else
                    {
                        if (!item.BaseData.possibleJob[Character.Job])
                            return -2;
                    }
                }
                else
                {
                    if (!item.BaseData.possibleJob[Character.JobJoint])
                        if (Character.DualJobID != 0)
                        {
                            var dualjobinfo = DualJobInfoFactory.Instance.items[Character.DualJobID];
                            if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.BaseJobID])
                                if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.ExperJobID])
                                    if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.TechnicalJobID])
                                        if (!item.BaseData.possibleJob[(PC_JOB)dualjobinfo.ChronicleJobID])
                                            return -2;
                        }
                }
            }

            if (Character.Race == PC_RACE.DEM && Character.Form == DEM_FORM.MACHINA_FORM) //DEM的机械形态不能装备
                return -29;
            if (item.BaseData.possibleRebirth)
                if (!Character.Rebirth || Character.Job != Character.Job3)
                    return -31;
            return 0;
        }

        public void OnItemEquiptRepair(CSMG_ITEM_EQUIPT_REPAIR p)
        {
            var item = Character.Inventory.GetItem(p.InventoryID);
            if (CountItem(item.BaseData.repairItem) > 0)
                if (item.maxDurability > item.Durability)
                {
                    item.Durability = (ushort)(item.maxDurability - 1);
                    item.maxDurability--;
                    var arg = new EffectArg();
                    arg.actorID = Character.ActorID;
                    arg.effectID = 8043;
                    Character.e.OnShowEffect(Character, arg);
                    SendItemInfo(item);
                    DeleteItemID(item.BaseData.repairItem, 1, true);
                }
        }

        /// <summary>
        ///     装备卸下过程，卸下该格子里的装备对应的所有格子里的道具，并移除道具附加的技能
        /// </summary>
        public void OnItemUnequipt(EnumEquipSlot eqslot)
        {
            if (!Character.Inventory.Equipments.ContainsKey(eqslot))
                return;
            var oriItem = Character.Inventory.Equipments[eqslot];
            CleanItemSkills(oriItem);
            foreach (var i in oriItem.EquipSlot)
                if (Character.Inventory.Equipments.ContainsKey(i))
                {
                    if (Character.Inventory.Equipments[i].Stack == 0)
                        Character.Inventory.Equipments.Remove(i);
                    else
                        ItemMoveSub(Character.Inventory.Equipments[i], ContainerType.BODY,
                            Character.Inventory.Equipments[i].Stack);
                }
        }

        //围观梦美卖萌0.0
        //从头写！
        //重写&简化逻辑结构
        public void OnItemEquipt(CSMG_ITEM_EQUIPT p)
        {
            OnItemEquipt(p.InventoryID, p.EquipSlot);
        }

        public void OnItemEquipt(uint InventoryID, byte EquipSlot)
        {
            //特殊状态解除
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding")) Character.Status.Additions["Hiding"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("Cloaking"))
                Character.Status.Additions["Cloaking"].AdditionEnd();
            if (Character.Status.Additions.ContainsKey("fish"))
            {
                Character.Status.Additions["fish"].AdditionEnd();
                Character.Status.Additions.Remove("fish");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            if (Character.Tasks.ContainsKey("Regeneration"))
            {
                Character.Tasks["Regeneration"].Deactivate();
                Character.Tasks.Remove("Regeneration");
            }

            if (Character.Tasks.ContainsKey("Scorponok"))
            {
                Character.Tasks["Scorponok"].Deactivate();
                Character.Tasks.Remove("Scorponok");
            }

            if (Character.Tasks.ContainsKey("自由射击"))
            {
                Character.Tasks["自由射击"].Deactivate();
                Character.Tasks.Remove("自由射击");
            }

            if (Character.Tasks.ContainsKey("Possession"))
            {
                Character.Tasks["Possession"].Deactivate();
                Character.Tasks.Remove("Possession");
                if (Character.Buff.GetReadyPossession)
                {
                    Character.Buff.GetReadyPossession = false;
                    Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);
                }
            }

            //???????
            //if ((Character.Speed < 750 || Character.Speed > 1000) && Map.OriID != 70000000 && Map.OriID != 75000000 && Character.Account.GMLevel < 20)
            //{
            //    SkillHandler.SendSystemMessage(Character, "由于移动速度低于或大于正常速度，处于无法更换装备的状态。");
            //    return;
            //}
            var item = Character.Inventory.GetItem(InventoryID);
            //item是这次装备的装备
            //if (item.BaseData.itemType == ItemType.BOW || item.BaseData.itemType == ItemType.RIFLE
            //    || item.BaseData.itemType == ItemType.GUN || item.BaseData.itemType == ItemType.DUALGUN)
            //    if (Character.Job != PC_JOB.HAWKEYE)
            //        return;
            //if (Character.Account.GMLevel > 200)
            //FromActorPC(Character).SendSystemMessage("道具ID：" + item.ItemID.ToString());
            //if (this.Character.Account.GMLevel <= 255)
            //{
            //    /*if(item.BaseData.itemType == ItemType.PARTNER)
            //    {
            //        if (Character.TranceID != 0)
            //            Character.TranceID = 0;
            //        else
            //            Character.TranceID = item.BaseData.petID;
            //        this.SendCharInfoUpdate();
            //        return;
            //    }*/
            //    //if ((//item.BaseData.itemType == ItemType.RIDE_PARTNER || //item.BaseData.itemType == ItemType.RIDE_PARTNER ||
            //    //        item.BaseData.itemType == ItemType.RIDE_PET || //item.BaseData.itemType == ItemType.RIDE_PET ||
            //    //        item.BaseData.itemType == ItemType.RIDE_PET_ROBOT
            //    //        ) && this.Character.Account.GMLevel <= 200 && Character.MapID != 91000999)
            //    //    return;
            //}
            var count = item.Stack; //count是实际移动数量，考虑弹药
            if (item == null) //不存在？卡住或者用外挂了？
                return;
            int result; //返回不能装备的类型
            result = CheckEquipRequirement(item); //检查装备条件

            if (Character.Account.GMLevel >= 200) result = 0;

            if (result < 0) //不能装备
            {
                var p4 = new SSMG_ITEM_EQUIP();
                p4.InventorySlot = 0xffffffff;
                p4.Target = ContainerType.NONE;
                p4.Result = result;
                p4.Range = Character.Range;
                NetIo.SendPacket(p4);
                return;
            }

            uint oldPetHP = 0; //原宠物HP，这次不想改

            var targetslots = new List<EnumEquipSlot>(); //EquipSlot involved in this item target slots
            foreach (var i in item.EquipSlot)
                if (!targetslots.Contains(i))
                    targetslots.Add(i);
            /* 双持等以后把封包的equipslot参数对应的位置都搞定了再说
            if (EquipSlot == 15 && item.EquipSlot.Count == 1 && item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND) && (!item.NeedAmmo)) //只有非射击类的右手单手武器可以作为左持
            {
                if (this.chara.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (!this.chara.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.doubleHand && !this.chara.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].NeedAmmo)
                    {
                        //只有当右手有装备且是单手非射击类武器时才能激发左持
                        targetslots.Clear();
                        targetslots.Add(EnumEquipSlot.LEFT_HAND);
                    }
                }
            }*/
            //卸下
            foreach (var i in targetslots)
            {
                //检查
                if (!Character.Inventory.Equipments.ContainsKey(i))
                    //该格子原来就是空的 直接下一个格子 特殊检查在循环外写
                    continue;
                foreach (var j in Character.Inventory.Equipments[i].EquipSlot) //检查对应位置的之前穿的装备是否可脱下
                {
                    var oriItem = Character.Inventory.Equipments[j];
                    if (!CheckPossessionForEquipMove(oriItem))
                        //装备被PY状态中不能移动,不能填装弹药
                        return;
                    if (oriItem.NeedAmmo) //取下射击类装备前检查左手 如果左手有装备必然是弹药 需取下
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        {
                            var ammo = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND];
                            if (!CheckPossessionForEquipMove(ammo))
                                //装备被PY状态中不能移动
                                return;
                        }
                }

                if (i == EnumEquipSlot.UPPER_BODY)
                {
                    Character.PossessionPartnerSlotIDinClothes = 0;
                    Character.Status.hp_petpy = 0;
                }

                if (i == EnumEquipSlot.RIGHT_HAND)
                {
                    Character.PossessionPartnerSlotIDinRightHand = 0;
                    Character.Status.max_atk1_petpy = 0;
                    Character.Status.min_atk1_petpy = 0;
                    Character.Status.max_matk_petpy = 0;
                    Character.Status.min_matk_petpy = 0;
                }

                if (i == EnumEquipSlot.LEFT_HAND)
                {
                    Character.PossessionPartnerSlotIDinLeftHand = 0;
                    Character.Status.def_add_petpy = 0;
                    Character.Status.mdef_add_petpy = 0;
                }

                if (i == EnumEquipSlot.CHEST_ACCE)
                {
                    Character.PossessionPartnerSlotIDinAccesory = 0;
                    Character.Status.aspd_petpy = 0;
                    Character.Status.cspd_petpy = 0;
                }

                //卸下
                if (item.IsAmmo) //填装弹药，检查原左手道具是否是同种(之前检查过故左手必然是弹药)，若是，则不需取下，后面直接填装补充，否则直接卸下
                {
                    if (Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].ItemID != item.ItemID)
                    {
                        //不是同种弹药 卸下
                        CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND]);
                        ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND], ContainerType.BODY,
                            Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack);
                    }
                    else //999检查
                    {
                        if (Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack + count > 999)
                            count = (ushort)(999 - Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack);
                    }
                }
                else if (item.BaseData.itemType == ItemType.CARD ||
                         item.BaseData.itemType == ItemType.THROW) //填装投掷武器，检查原右手道具是否是同种，若是，则不需取下，后面直接填装补充，否则直接卸下
                {
                    //若是双手的投掷类？？？ 以后再说。。。
                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    {
                        if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].ItemID != item.ItemID)
                        {
                            OnItemUnequipt(EnumEquipSlot.RIGHT_HAND);
                        }
                        else //999检查
                        {
                            if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack + count > 999)
                                count = (ushort)(999 - Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                        }
                    }
                }
                else if (item.NeedAmmo) //将要装备射击类武器，需额外检查左手，左手只能装备对应的弹药种类，否则都卸下左手装备
                {
                    //弓装备前判定左手
                    if (item.BaseData.itemType == ItemType.BOW)
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType !=
                                ItemType.ARROW)
                                OnItemUnequipt(EnumEquipSlot.LEFT_HAND);

                    //枪类装备前判定左手
                    if (item.BaseData.itemType == ItemType.GUN || item.BaseData.itemType == ItemType.DUALGUN ||
                        item.BaseData.itemType == ItemType.RIFLE)
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType !=
                                ItemType.BULLET)
                                OnItemUnequipt(EnumEquipSlot.LEFT_HAND);

                    //卸下原来的右手道具
                    OnItemUnequipt(EnumEquipSlot.RIGHT_HAND);
                }
                else //将要装备的装备既不是射击武器也不是弹药也不是投掷武器
                {
                    foreach (var j in Character.Inventory.Equipments[i].EquipSlot)
                    {
                        var oriItem = Character.Inventory.Equipments[j];
                        if (j == EnumEquipSlot.RIGHT_HAND || j == EnumEquipSlot.LEFT_HAND) //手部装备需要卸下，需特别检查射击类装备相关
                        {
                            //包里东西出来
                            if (oriItem.BaseData.itemType == ItemType.HANDBAG)
                                while (Character.Inventory.GetContainer(ContainerType.RIGHT_BAG).Count > 0)
                                {
                                    var content = Character.Inventory.GetContainer(ContainerType.RIGHT_BAG).First();
                                    ItemMoveSub(content, ContainerType.BODY, content.Stack);
                                }

                            if (oriItem.BaseData.itemType == ItemType.LEFT_HANDBAG)
                                while (Character.Inventory.GetContainer(ContainerType.LEFT_BAG).Count > 0)
                                {
                                    var content = Character.Inventory.GetContainer(ContainerType.LEFT_BAG).First();
                                    ItemMoveSub(content, ContainerType.BODY, content.Stack);
                                }

                            //射击类相关右手判定：原来装备射击武器且将要装备的新武器（含右手）与原来的类别不同时，需卸下左手的弹药
                            if (oriItem.NeedAmmo && item.BaseData.itemType != oriItem.BaseData.itemType)
                                //取下弹药
                                if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                                {
                                    CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND]);
                                    ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND],
                                        ContainerType.BODY,
                                        Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack);
                                }

                            //射击类相关左手判定：原来装备射击武器且将要装备的新道具（含左手）不是对应的弹药，需卸下右手的射击武器
                            if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                                item.EquipSlot.Contains(EnumEquipSlot.LEFT_HAND))
                                switch (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType)
                                {
                                    case ItemType.DUALGUN:
                                    case ItemType.GUN:
                                    case ItemType.RIFLE:
                                        if (item.BaseData.itemType != ItemType.BULLET)
                                        {
                                            //取下射击武器
                                            CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                                            ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND],
                                                ContainerType.BODY,
                                                Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                                        }

                                        break;
                                    case ItemType.BOW:
                                        if (item.BaseData.itemType != ItemType.ARROW)
                                        {
                                            //取下射击武器
                                            CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                                            ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND],
                                                ContainerType.BODY,
                                                Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                                        }

                                        break;
                                }
                        }
                        else //非手部装备需要卸下
                        {
                            //宠物类装备卸下过程
                            if (j == EnumEquipSlot.PET)
                            {
                                if (Character.Pet != null)
                                {
                                    if (Character.Pet.Ride)
                                        //oldPetHP = this.Character.Pet.HP;
                                        //this.Character.HP = oldPetHP;
                                        //this.Character.Speed = Configuration.Instance.Speed;
                                        Character.Pet = null;
                                    DeletePet();
                                }

                                if (Character.Partner != null) DeletePartner();
                            }

                            //检查副职业切换
                            if (oriItem.BaseData.jointJob != PC_JOB.NONE) Character.JobJoint = PC_JOB.NONE;
                            //推进器技能
                            if (oriItem.BaseData.itemType == ItemType.BACK_DEMON)
                            {
                                SkillHandler.RemoveAddition(Character, "MoveUp2");
                                SkillHandler.RemoveAddition(Character, "MoveUp3");
                                SkillHandler.RemoveAddition(Character, "MoveUp4");
                                SkillHandler.RemoveAddition(Character, "MoveUp5");
                            }

                            //包里东西出来
                            if (oriItem.BaseData.itemType == ItemType.BACKPACK)
                                while (Character.Inventory.GetContainer(ContainerType.BACK_BAG).Count > 0)
                                {
                                    var content = Character.Inventory.GetContainer(ContainerType.BACK_BAG).First();
                                    ItemMoveSub(content, ContainerType.BODY, content.Stack);
                                }
                        }

                        //j位置的装备正式卸下
                        if (Character.Inventory.Equipments.ContainsKey(j)) //检查以防之前过程中已经卸下了
                        {
                            CleanItemSkills(oriItem);
                            ItemMoveSub(Character.Inventory.Equipments[j], ContainerType.BODY,
                                Character.Inventory.Equipments[j].Stack);
                        }
                    }
                }
            }

            //道具对应格子本来就是空着时却需要检查别的格子的特殊卸下
            if (item.NeedAmmo) //将要装备射击类武器，需额外检查左手，左手只能装备对应的弹药种类，否则都卸下左手装备
            {
                //弓装备前判定右手
                if (item.BaseData.itemType == ItemType.BOW)
                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType !=
                            ItemType.ARROW)
                        {
                            CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                            ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND], ContainerType.BODY,
                                Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                        }

                //枪类装备前判定右手
                if (item.BaseData.itemType == ItemType.GUN || item.BaseData.itemType == ItemType.DUALGUN ||
                    item.BaseData.itemType == ItemType.RIFLE)
                    if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType !=
                            ItemType.BULLET)
                        {
                            CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                            ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND], ContainerType.BODY,
                                Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                        }
            }
            else if (targetslots.Contains(EnumEquipSlot.LEFT_HAND) &&
                     !item.IsAmmo) //包含左手的非弹药道具(弹药与武器的匹配最先就检查过了)需要额外检查右手是不是射击武器，是否对应
            {
                if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].NeedAmmo)
                    {
                        CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]);
                        CleanItemSkills(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND]);
                        ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND], ContainerType.BODY,
                            Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack);
                    }
            }

            if (count == 0) return;
            if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot,
                    (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString()), count))
            {
                if (item.Stack == 0)
                {
                    var p4 = new SSMG_ITEM_EQUIP();
                    p4.Target = (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString());
                    p4.InventorySlot = item.Slot;
                    StatusFactory.Instance.CalcRange(Character);
                    p4.Range = Character.Range;
                    NetIo.SendPacket(p4);
                }
                else
                {
                    var p5 = new SSMG_ITEM_COUNT_UPDATE();
                    p5.InventorySlot = item.Slot;
                    p5.Stack = item.Stack;
                    NetIo.SendPacket(p5);
                }

                if (item.BaseData.itemType == ItemType.ARROW || item.BaseData.itemType == ItemType.BULLET)
                {
                    if (item.Stack == 0) Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot = item.Slot;
                    var p5 = new SSMG_ITEM_COUNT_UPDATE();
                    p5.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot;
                    p5.Stack = Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack;
                    NetIo.SendPacket(p5);
                }

                if (item.BaseData.itemType == ItemType.CARD || item.BaseData.itemType == ItemType.THROW)
                {
                    if (item.Stack == 0) Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot = item.Slot;
                    var p5 = new SSMG_ITEM_COUNT_UPDATE();
                    p5.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot;
                    p5.Stack = Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Stack;
                    NetIo.SendPacket(p5);
                }
            }

            //create dummy item to take the slots
            var slots = item.EquipSlot;
            if (slots.Count > 1)
                for (var i = 1; i < slots.Count; i++)
                {
                    var dummy = item.Clone();
                    dummy.Stack = 0;
                    Character.Inventory.AddItem((ContainerType)Enum.Parse(typeof(ContainerType), slots[i].ToString()),
                        dummy);
                }

            //renew stauts
            if (item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND))
            {
                SendAttackType();
                SkillHandler.Instance.CastPassiveSkills(Character, false);
            }

            if (item.EquipSlot.Contains(EnumEquipSlot.LEFT_HAND))
                SkillHandler.Instance.CastPassiveSkills(Character, false);
            SkillHandler.Instance.CheckBuffValid(Character);
            if (item.BaseData.itemType == ItemType.PET || item.BaseData.itemType == ItemType.PET_NEKOMATA)
                SendPet(item);
            if (item.BaseData.itemType == ItemType.PARTNER)
            {
                SendPartner(item);
                Character.Inventory.Equipments[EnumEquipSlot.PET].ActorPartnerID = item.ActorPartnerID;
                StatusFactory.Instance.CalcStatus(Character);
                PartnerTalking(Character.Partner, TALK_EVENT.SUMMONED, 100, 0);
            }

            if (item.BaseData.itemType == ItemType.RIDE_PET || item.BaseData.itemType == ItemType.RIDE_PARTNER ||
                item.BaseData.itemType == ItemType.RIDE_PET_ROBOT)
            {
                var pet = new ActorPet(item.BaseData.petID, item);
                pet.Owner = Character;
                Character.Pet = pet;

                //#region MA"匠师"模块1

                if (Character is ActorPC)
                {
                    var pc = Character;
                    if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                        if (!pc.Skills2_2.ContainsKey(132) && !pc.DualJobSkill.Exists(x => x.ID == 132))
                            return;
                }

                //#endregion

                pet.Ride = true;

                if (!pet.Owner.CInt.ContainsKey("PC_HUNMAN_HP")) pet.Owner.CInt["PC_HUNMAN_HP"] = (int)Character.HP;
                pet.MaxHP = 2000;

                //#region MA"匠师"模块2

                var OnDir = 1.0f;
                if (Character is ActorPC)
                {
                    var pc = Character;

                    if (pc.Skills3.ContainsKey(987) || pc.DualJobSkill.Exists(x => x.ID == 987))
                    {
                        var duallv = 0;
                        if (pc.DualJobSkill.Exists(x => x.ID == 987))
                            duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 987).Level;

                        var mainlv = 0;
                        if (pc.Skills3.ContainsKey(987))
                            mainlv = pc.Skills3[987].Level;

                        //OnDir = OnDir + (float)(((Math.Max(duallv, mainlv)) - 1) * 0.05f);
                        var maxlv = Math.Max(duallv, mainlv);
                        if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                        {
                            OnDir = OnDir + (maxlv - 1) * 0.05f;
                            pet.MaxHP += (ushort)(Character.MaxHP * OnDir);
                            pet.MaxMP += (ushort)(Character.MaxMP * OnDir);
                            pet.MaxSP += (ushort)(Character.MaxSP * OnDir);
                            pet.Status.min_atk1 += (ushort)(Character.Status.min_atk1 * OnDir);
                            pet.Status.min_atk2 += (ushort)(Character.Status.min_atk2 * OnDir);
                            pet.Status.min_atk3 += (ushort)(Character.Status.min_atk3 * OnDir);
                            pet.Status.max_atk1 += (ushort)(Character.Status.max_atk1 * OnDir);
                            pet.Status.max_atk2 += (ushort)(Character.Status.max_atk2 * OnDir);
                            pet.Status.max_atk3 += (ushort)(Character.Status.max_atk3 * OnDir);
                            pet.Status.min_matk += (ushort)(Character.Status.min_matk * OnDir);
                            pet.Status.max_matk += (ushort)(Character.Status.max_matk * OnDir);
                            pet.Status.hit_melee += (ushort)(Character.Status.hit_melee * OnDir);
                            pet.Status.hit_ranged += (ushort)(Character.Status.hit_ranged * OnDir);
                            if (pet.Status.def + Character.Status.def * OnDir > 90)
                                pet.Status.def = 90;
                            else
                                pet.Status.def += (ushort)(Character.Status.def * OnDir);

                            pet.Status.def_add += (short)(Character.Status.def_add * OnDir);
                            if (pet.Status.mdef + Character.Status.mdef * OnDir > 90)
                                pet.Status.mdef = 90;
                            else
                                pet.Status.mdef += (ushort)(Character.Status.mdef * OnDir);

                            pet.Status.mdef_add += (short)(Character.Status.mdef_add * OnDir);
                            pet.Status.avoid_melee += (ushort)(Character.Status.avoid_melee * OnDir);
                            pet.Status.avoid_ranged += (ushort)(Character.Status.avoid_ranged * OnDir);

                            if (pet.Status.aspd + Character.Status.aspd * OnDir > 800)
                                pet.Status.aspd = 800;
                            else
                                pet.Status.aspd += (short)(Character.Status.aspd * OnDir);
                            if (pet.Status.cspd + Character.Status.cspd * OnDir > 800)
                                pet.Status.cspd = 800;
                            else
                                pet.Status.cspd += (short)(Character.Status.cspd * OnDir);
                        }
                        else if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE"))
                        {
                            OnDir = (float)(0.25 + maxlv * 0.01f);
                            pet.MaxHP += (ushort)(Character.MaxHP * OnDir);
                            pet.MaxMP += (ushort)(Character.MaxMP * OnDir);
                            pet.MaxSP += (ushort)(Character.MaxSP * OnDir);
                            pet.Status.min_atk1 += (ushort)(Character.Status.min_atk1 * OnDir);
                            pet.Status.min_atk2 += (ushort)(Character.Status.min_atk2 * OnDir);
                            pet.Status.min_atk3 += (ushort)(Character.Status.min_atk3 * OnDir);
                            pet.Status.max_atk1 += (ushort)(Character.Status.max_atk1 * OnDir);
                            pet.Status.max_atk2 += (ushort)(Character.Status.max_atk2 * OnDir);
                            pet.Status.max_atk3 += (ushort)(Character.Status.max_atk3 * OnDir);
                            pet.Status.min_matk += (ushort)(Character.Status.min_matk * OnDir);
                            pet.Status.max_matk += (ushort)(Character.Status.max_matk * OnDir);
                            pet.Status.hit_melee += (ushort)(Character.Status.hit_melee * OnDir);
                            pet.Status.hit_ranged += (ushort)(Character.Status.hit_ranged * OnDir);

                            if (pet.Status.def + Character.Status.def * OnDir > 90)
                                pet.Status.def = 90;
                            else
                                pet.Status.def += (ushort)(Character.Status.def * OnDir);

                            pet.Status.def_add += (short)(Character.Status.def_add * OnDir);

                            if (pet.Status.mdef + Character.Status.mdef * OnDir > 90)
                                pet.Status.mdef = 90;
                            else
                                pet.Status.mdef += (ushort)(Character.Status.mdef * OnDir);

                            pet.Status.mdef_add += (short)(Character.Status.mdef_add * OnDir);
                            pet.Status.avoid_melee += (ushort)(Character.Status.avoid_melee * OnDir);
                            pet.Status.avoid_ranged += (ushort)(Character.Status.avoid_ranged * OnDir);

                            if (pet.Status.aspd + Character.Status.aspd * OnDir > 800)
                                pet.Status.aspd = 800;
                            else
                                pet.Status.aspd += (short)(Character.Status.aspd * OnDir);

                            if (pet.Status.cspd + Character.Status.cspd * OnDir > 800)
                                pet.Status.cspd = 800;
                            else
                                pet.Status.cspd += (short)(Character.Status.cspd * OnDir);
                        }
                    }
                }

                //#endregion

                /*if (oldPetHP == 0)
                    pet.HP = this.Character.HP;
                else
                    pet.HP = oldPetHP;
                pet.HP = 99;
                Character.HP = pet.MaxHP;*/
                //Character.Speed = 600;
                Character.Speed = Configuration.Configuration.Instance.Speed;

                //SendSystemMessage("[提示]在骑宠时移动速度上升33%，受到的伤害提升100%，治愈量下降70%。");
                SendPetBasicInfo();
                SendPetDetailInfo();
            }

            if (item.BaseData.jointJob != PC_JOB.NONE) Character.JobJoint = item.BaseData.jointJob;
            //凭依，跟我没关系
            if (item.PossessionedActor != null)
            {
                var arg = new PossessionArg();
                arg.fromID = item.PossessionedActor.ActorID;
                arg.toID = Character.ActorID;
                arg.result = (int)item.PossessionedActor.PossessionPosition;
                item.PossessionedActor.PossessionTarget = Character.ActorID;
                MapServer.charDB.SaveChar(item.PossessionedActor, false, false);
                MapServer.accountDB.WriteUser(item.PossessionedActor.Account);
                var pos = "";
                switch (item.PossessionedActor.PossessionPosition)
                {
                    case PossessionPosition.RIGHT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_RIGHT;
                        break;
                    case PossessionPosition.LEFT_HAND:
                        pos = LocalManager.Instance.Strings.POSSESSION_LEFT;
                        break;
                    case PossessionPosition.NECK:
                        pos = LocalManager.Instance.Strings.POSSESSION_NECK;
                        break;
                    case PossessionPosition.CHEST:
                        pos = LocalManager.Instance.Strings.POSSESSION_ARMOR;
                        break;
                }

                SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                if (item.PossessionedActor.Online)
                    FromActorPC(item.PossessionedActor)
                        .SendSystemMessage(string.Format(LocalManager.Instance.Strings.POSSESSION_DONE, pos));
                Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.POSSESSION, arg, Character, true);
            }

            AddItemSkills(item);
            //重新计算状态值
            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
            //broadcast
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);
            var list = new List<Item>();
            foreach (var i in Character.Inventory.Equipments.Values)
            {
                if (i.Stack == 0)
                    continue;
                if (CheckEquipRequirement(i) != 0) list.Add(i);
            }

            foreach (var i in list)
            {
                var p2 = new CSMG_ITEM_MOVE();
                p2.data = new byte[9];
                p2.Count = 1;
                p2.InventoryID = i.Slot;
                p2.Target = ContainerType.BODY;
                OnItemMove(p2);
            }
        }

        public void OnItemMove(CSMG_ITEM_MOVE p)
        {
            OnItemMove(p, false);
        }

        public void OnItemMove(CSMG_ITEM_MOVE p, bool possessionRemove)
        {
            OnItemMove(p.InventoryID, p.Target, p.Count, possessionRemove);
        }

        public void OnItemMove(uint InventoryID, ContainerType Target, ushort Count, bool possessionRemove)
        {
            if (Character.Status.Additions.ContainsKey("Meditatioon"))
            {
                Character.Status.Additions["Meditatioon"].AdditionEnd();
                Character.Status.Additions.Remove("Meditatioon");
            }

            if (Character.Status.Additions.ContainsKey("Hiding"))
            {
                Character.Status.Additions["Hiding"].AdditionEnd();
                Character.Status.Additions.Remove("Hiding");
            }

            if (Character.Status.Additions.ContainsKey("Cloaking"))
            {
                Character.Status.Additions["Cloaking"].AdditionEnd();
                Character.Status.Additions.Remove("Cloaking");
            }

            if (Character.Status.Additions.ContainsKey("IAmTree"))
            {
                Character.Status.Additions["IAmTree"].AdditionEnd();
                Character.Status.Additions.Remove("IAmTree");
            }

            if (Character.Status.Additions.ContainsKey("Invisible"))
            {
                Character.Status.Additions["Invisible"].AdditionEnd();
                Character.Status.Additions.Remove("Invisible");
            }

            var item = Character.Inventory.GetItem(InventoryID);
            if (Target >= ContainerType.HEAD) //移动目标错误
            {
                var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                p1.InventorySlot = item.Slot;
                p1.Result = -3;
                p1.Target = (ContainerType)(-1);
                NetIo.SendPacket(p1);
                return;
            }

            var ifUnequip = Character.Inventory.IsContainerEquip(Character.Inventory.GetContainerType(item.Slot));
            //ifUnequip &= p.Count == item.Stack;
            if (ifUnequip) //如果是卸下装备而不是在不同容器中移动
            {
                //检查
                if (item.PossessionedActor != null && !possessionRemove)
                {
                    var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                    p1.InventorySlot = item.Slot;
                    p1.Result = -4;
                    p1.Target = (ContainerType)(-1);
                    NetIo.SendPacket(p1);
                    return;
                }

                if (Character.Race == PC_RACE.DEM && Character.Form == DEM_FORM.MACHINA_FORM)
                {
                    var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                    p1.InventorySlot = item.Slot;
                    p1.Result = -10;
                    p1.Target = (ContainerType)(-1);
                    NetIo.SendPacket(p1);
                    return;
                }

                if (possessionRemove)
                    return;
                //卸下相关的额外格子
                var slots = item.EquipSlot;
                if (slots.Count > 1)
                {
                    for (var i = 0; i < slots.Count; i++)
                        if (Character.Inventory.Equipments[slots[i]].Stack == 0)
                            Character.Inventory.Equipments.Remove(slots[i]);
                        else
                            ItemMoveSub(Character.Inventory.Equipments[slots[i]], ContainerType.BODY,
                                Character.Inventory.Equipments[slots[i]].Stack);
                }
                else
                {
                    if (slots[0] == EnumEquipSlot.PET)
                    {
                        if (Character.Pet != null)
                            DeletePet();
                        if (Character.Partner != null)
                            DeletePartner();
                        StatusFactory.Instance.CalcStatus(Character);
                    }

                    //箱包类装备移动时内容物进入body
                    if (item.BaseData.itemType == ItemType.BACKPACK)
                        while (Character.Inventory.GetContainer(ContainerType.BACK_BAG).Count > 0)
                        {
                            var content = Character.Inventory.GetContainer(ContainerType.BACK_BAG).First();
                            ItemMoveSub(content, ContainerType.BODY, content.Stack);
                        }

                    if (item.BaseData.itemType == ItemType.HANDBAG)
                        while (Character.Inventory.GetContainer(ContainerType.RIGHT_BAG).Count > 0)
                        {
                            var content = Character.Inventory.GetContainer(ContainerType.RIGHT_BAG).First();
                            ItemMoveSub(content, ContainerType.BODY, content.Stack);
                        }

                    if (item.BaseData.itemType == ItemType.LEFT_HANDBAG)
                        while (Character.Inventory.GetContainer(ContainerType.LEFT_BAG).Count > 0)
                        {
                            var content = Character.Inventory.GetContainer(ContainerType.LEFT_BAG).First();
                            ItemMoveSub(content, ContainerType.BODY, content.Stack);
                        }

                    //卸下射击武器时自动卸下弹药
                    if (item.NeedAmmo)
                        if (Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            ItemMoveSub(Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND], ContainerType.BODY,
                                Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack);
                }

                //删除装备附带技能
                if (item.BaseData.jointJob != PC_JOB.NONE) Character.JobJoint = PC_JOB.NONE;
                if (item.BaseData.itemType == ItemType.BACK_DEMON)
                {
                    SkillHandler.RemoveAddition(Character, "MoveUp2");
                    SkillHandler.RemoveAddition(Character, "MoveUp3");
                    SkillHandler.RemoveAddition(Character, "MoveUp4");
                    SkillHandler.RemoveAddition(Character, "MoveUp5");
                }
            }

            //无体积装备时不能放入物品
            if (Target == ContainerType.BACK_BAG)
            {
                if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.BACK)) return;

                if (Character.Inventory.Equipments[EnumEquipSlot.BACK].BaseData.volumeUp == 0) return;
            }

            if (Target == ContainerType.RIGHT_BAG)
            {
                if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) return;

                if (Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.volumeUp == 0) return;
            }

            if (Target == ContainerType.LEFT_BAG)
            {
                if (!Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)) return;

                if (Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.volumeUp == 0) return;
            }

            /*双持以后再说
            //双持时若卸下右手则同时卸下左手
            if (item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND)
                && item.EquipSlot.Count == 1
                && this.Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)
                && this.Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]==item
                && this.Character.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND)
                && this.Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND))
            {
                ItemMoveSub(this.Character.Inventory.Equipments[EnumEquipSlot.LEFT_HAND], ContainerType.BODY, 1);
            }*/
            //正式移动道具
            ItemMoveSub(item, Target, Count);
            //CleanItemSkills(item);
            //PC.StatusFactory.Instance.CalcStatus(this.Character);
            //SendPlayerInfo();
            //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, this.Character, true);
        }

        /// <summary>
        ///     道具移动，只移动对应的真实格子的道具，不影响伪道具
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <param name="count"></param>
        public void ItemMoveSub(Item item, ContainerType container, ushort count)
        {
            SSMG_ITEM_DELETE p2;
            SSMG_ITEM_ADD p3;

            CleanItemSkills(item);
            if (item.BaseData.itemType == ItemType.RIDE_PET || item.BaseData.itemType == ItemType.RIDE_PARTNER)
            {
                StatusFactory.Instance.CalcStatus(Character);
                if (!Character.CInt.ContainsKey("PC_HUNMAN_HP"))
                    Character.CInt["PC_HUNMAN_HP"] = 99; //防止变量更改前就骑着骑宠的人上线后寻找不到PC_HUNMAN_HP值导致0HP,理论上不可能发生,以防万一
                Character.HP = (uint)Character.CInt["PC_HUNMAN_HP"];
                Character.CInt.Remove("PC_HUNMAN_HP");
            }

            var ifUnequip = Character.Inventory.IsContainerEquip(Character.Inventory.GetContainerType(item.Slot));
            var slot = item.Slot;
            //Logger.ShowError(this.Character.Inventory.GetContainerType(item.Slot).ToString());
            if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot, container,
                    count))
            {
                //Logger.ShowError(this.Character.Inventory.GetContainerType(item.Slot).ToString());
                if (item.Stack == 0)
                {
                    if (slot == Character.Inventory.LastItem.Slot)
                    {
                        if (!ifUnequip)
                        {
                            p2 = new SSMG_ITEM_DELETE();
                            p2.InventorySlot = item.Slot;
                            NetIo.SendPacket(p2);
                            p3 = new SSMG_ITEM_ADD();
                            p3.Container = container;
                            p3.InventorySlot = item.Slot;
                            item.Stack = count;
                            p3.Item = item;
                            NetIo.SendPacket(p3);
                            var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                            p1.InventorySlot = item.Slot;
                            p1.Target = container;
                            NetIo.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                            p1.InventorySlot = item.Slot;
                            p1.Target = container;
                            NetIo.SendPacket(p1);
                            var p4 = new SSMG_ITEM_EQUIP();
                            p4.InventorySlot = 0xffffffff;
                            p4.Target = ContainerType.NONE;
                            p4.Result = 1;
                            StatusFactory.Instance.CalcRange(Character);
                            if (item.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND))
                            {
                                SendAttackType();
                                SkillHandler.Instance.CastPassiveSkills(Character);
                            }

                            p4.Range = Character.Range;
                            NetIo.SendPacket(p4);
                            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);

                            if (item.EquipSlot[0] == EnumEquipSlot.PET)
                                if (Character.Pet != null)
                                    if (Character.Pet.Ride)
                                        //this.Character.Speed = Configuration.Instance.Speed;
                                        //this.Character.HP = this.Character.Pet.HP;
                                        Character.Pet = null;

                            StatusFactory.Instance.CalcStatus(Character);

                            SendPlayerInfo();
                        }
                    }
                    else
                    {
                        p2 = new SSMG_ITEM_DELETE();
                        p2.InventorySlot = slot;
                        NetIo.SendPacket(p2);
                        if (slot != item.Slot)
                        {
                            item = Character.Inventory.GetItem(item.Slot);
                            var p5 = new SSMG_ITEM_COUNT_UPDATE();
                            p5.InventorySlot = item.Slot;
                            p5.Stack = item.Stack;
                            NetIo.SendPacket(p5);
                            item = Character.Inventory.LastItem;
                            p3 = new SSMG_ITEM_ADD();
                            p3.Container = container;
                            p3.InventorySlot = item.Slot;
                            p3.Item = item;
                            NetIo.SendPacket(p3);
                        }
                        else
                        {
                            item = Character.Inventory.LastItem;
                            var p4 = new SSMG_ITEM_COUNT_UPDATE();
                            p4.InventorySlot = item.Slot;
                            p4.Stack = item.Stack;
                            NetIo.SendPacket(p4);
                        }
                    }
                }
                else
                {
                    var p1 = new SSMG_ITEM_COUNT_UPDATE();
                    p1.InventorySlot = item.Slot;
                    p1.Stack = item.Stack;
                    NetIo.SendPacket(p1);
                    if (Character.Inventory.LastItem.Stack == count)
                    {
                        p3 = new SSMG_ITEM_ADD();
                        p3.Container = container;
                        p3.InventorySlot = Character.Inventory.LastItem.Slot;
                        p3.Item = Character.Inventory.LastItem;
                        NetIo.SendPacket(p3);
                    }
                    else
                    {
                        item = Character.Inventory.LastItem;
                        var p4 = new SSMG_ITEM_COUNT_UPDATE();
                        p4.InventorySlot = item.Slot;
                        p4.Stack = item.Stack;
                        NetIo.SendPacket(p4);
                    }
                }
            }

            Character.Inventory.Items[ContainerType.BODY].RemoveAll(x => x.Stack == 0);
            Character.Inventory.CalcPayloadVolume();
            SendCapacity();
        }

        public bool CheckPossessionForEquipMove(Item item)
        {
            if (item.PossessionedActor != null)
            {
                var p4 = new SSMG_ITEM_EQUIP();
                p4.InventorySlot = 0xffffffff;
                p4.Target = ContainerType.NONE;
                p4.Result = -10;
                p4.Range = Character.Range;
                NetIo.SendPacket(p4);
                return false;
            }

            return true;
        }

        public void AddItemSkills(Item item)
        {
            if (item.BaseData.itemType == ItemType.PARTNER)
            {
                var partner = MapServer.charDB.GetActorPartner(item.ActorPartnerID, item);
                if (partner.rebirth)
                {
                    var skill = SkillFactory.Instance.GetSkill(2443, 1);
                    if (skill != null)
                        if (!Character.Skills.ContainsKey(2443))
                            Character.Skills.Add(2443, skill);
                }
            }

            if (item.BaseData.possibleSkill != 0) //装备附带主动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.possibleSkill, 1);
                if (skill != null)
                    if (!Character.Skills.ContainsKey(item.BaseData.possibleSkill))
                        Character.Skills.Add(item.BaseData.possibleSkill, skill);
            }

            if (item.BaseData.passiveSkill != 0) //装备附带被动技能
            {
                var skillID = item.BaseData.passiveSkill;
                byte lv = 0;
                foreach (var eq in Character.Inventory.Equipments)
                    if (eq.Value.BaseData.passiveSkill == skillID && eq.Value.EquipSlot[0] == eq.Key)
                        lv++;
                if (lv > 5) lv = 5;
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.passiveSkill, lv);
                if (skill != null)
                {
                    if (Character.Skills.ContainsKey(skillID))
                        Character.Skills.Remove(skillID);
                    if (lv > 0)
                    {
                        Character.Skills.Add(skillID, skill);
                        if (!skill.BaseData.active)
                        {
                            var arg = new SkillArg();
                            arg.skill = skill;
                            SkillHandler.Instance.SkillCast(Character, Character, arg);
                        }
                    }
                }

                SkillHandler.Instance.CastPassiveSkills(Character);
            }
        }

        public void CleanItemSkills(Item item)
        {
            if (item.BaseData.itemType == ItemType.PARTNER)
            {
                var partner = MapServer.charDB.GetActorPartner(item.ActorPartnerID, item);
                if (partner != null)
                    if (partner.rebirth)
                    {
                        var skill = SkillFactory.Instance.GetSkill(2443, 1);
                        if (skill != null)
                            if (Character.Skills.ContainsKey(2443))
                                Character.Skills.Remove(2443);
                    }
            }

            if (item.BaseData.possibleSkill != 0) //装备附带主动技能
            {
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.possibleSkill, 1);
                if (skill != null)
                    if (Character.Skills.ContainsKey(item.BaseData.possibleSkill))
                        Character.Skills.Remove(item.BaseData.possibleSkill);
            }

            if (item.BaseData.passiveSkill != 0) //装备附带被动技能
            {
                var skillID = item.BaseData.passiveSkill;
                //byte lv = 0;
                //foreach (var eq in Character.Inventory.Equipments)
                //    if (eq.Value.BaseData.passiveSkill == skillID && eq.Value.EquipSlot[0] == eq.Key)
                //        lv++;
                //if (lv > 5) lv = 5;
                var skill = SkillFactory.Instance.GetSkill(item.BaseData.passiveSkill, 1);
                if (skill != null)
                    if (Character.Skills.ContainsKey(skillID))
                        Character.Skills.Remove(skillID);
                //if (lv > 0)
                //{
                //    Character.Skills.Add(skillID, skill);
                //    if (!skill.BaseData.active)
                //    {
                //        SkillArg arg = new SkillArg();
                //        arg.skill = skill;
                //        SkillHandler.Instance.SkillCast(this.Character, this.Character, arg);
                //    }
                //}
                SkillHandler.Instance.CastPassiveSkills(Character);
            }
        }

        public void SendItemAdd(Item item, ContainerType container, InventoryAddResult result, int count,
            bool sendMessage)
        {
            switch (result)
            {
                case InventoryAddResult.NEW_INDEX:
                    var p = new SSMG_ITEM_ADD();
                    p.Container = container;
                    p.Item = item;
                    p.InventorySlot = item.Slot;
                    NetIo.SendPacket(p);
                    break;
                case InventoryAddResult.STACKED:
                    {
                        var p1 = new SSMG_ITEM_COUNT_UPDATE();
                        p1.InventorySlot = item.Slot;
                        p1.Stack = item.Stack;
                        NetIo.SendPacket(p1);
                    }
                    break;
                case InventoryAddResult.MIXED:
                    {
                        var p1 = new SSMG_ITEM_COUNT_UPDATE();
                        p1.InventorySlot = item.Slot;
                        p1.Stack = item.Stack;
                        NetIo.SendPacket(p1);
                        var p2 = new SSMG_ITEM_ADD();
                        p2.Container = container;
                        p2.Item = Character.Inventory.LastItem;
                        p2.InventorySlot = Character.Inventory.LastItem.Slot;
                        NetIo.SendPacket(p2);
                    }
                    break;
                case InventoryAddResult.GOWARE:
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了仓库");
                    break;
            }

            Character.Inventory.CalcPayloadVolume();
            SendCapacity();

            if (sendMessage)
            {
                if (item.Identified)
                    SendSystemMessage(
                        string.Format(LocalManager.Instance.Strings.ITEM_ADDED, item.BaseData.name, count));
                else
                    SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_ADDED,
                        Event.GetItemNameByType(item.BaseData.itemType), count));
            }
        }

        public void SendItems()
        {
            var names = Enum.GetNames(typeof(ContainerType));
            foreach (var i in names)
            {
                var container = (ContainerType)Enum.Parse(typeof(ContainerType), i);
                var items = Character.Inventory.GetContainer(container);
                var trashItem = new List<Item>();
                if (container == ContainerType.BODY) //扫描并删除身上的垃圾数据
                {
                    foreach (var j in items)
                        if (j.Stack == 0)
                            trashItem.Add(j);
                    if (trashItem.Count > 0)
                        for (var y = 0; y < trashItem.Count; y++)
                            Character.Inventory.Items[ContainerType.BODY].Remove(trashItem[y]);
                }

                foreach (var j in items)
                {
                    if (j.Stack == 0)
                        continue;
                    //if (j.Refine == 0)
                    //    j.Clear();
                    var p = new SSMG_ITEM_INFO();
                    p.Item = j;
                    p.InventorySlot = j.Slot;
                    p.Container = container;
                    NetIo.SendPacket(p);
                }
            }
        }

        public void SendItemInfo(uint slot)
        {
            var item = Character.Inventory.GetItem(slot);
            if (item == null)
                return;
            var p = new SSMG_ITEM_INFO();
            p.Item = item;
            p.InventorySlot = item.Slot;
            p.Container = Character.Inventory.GetContainerType(slot);
            NetIo.SendPacket(p);
        }

        public void SendItemInfo(Item item)
        {
            if (item == null)
                return;

            var p = new SSMG_ITEM_INFO();
            p.Item = item;
            p.InventorySlot = item.Slot;
            p.Container = Character.Inventory.GetContainerType(item.Slot);
            NetIo.SendPacket(p);

            var packet = new Packet();
            packet.data = new byte[3];
            packet.ID = 0x0203;
            packet.offset = 2;
            packet.PutByte(02);
            NetIo.SendPacket(packet);
        }

        public void SendItemIdentify(uint slot)
        {
            var item = Character.Inventory.GetItem(slot);
            if (item == null)
                return;
            var p = new SSMG_ITEM_IDENTIFY();
            p.InventorySlot = item.Slot;
            p.Identify = item.Identified;
            p.Lock = item.Locked;
            NetIo.SendPacket(p);
        }

        public void SendEquip()
        {
            var p = new SSMG_ITEM_ACTOR_EQUIP_UPDATE();
            p.Player = Character;
            NetIo.SendPacket(p);
        }

        public void AddItem(Item item, bool sendMessage)
        {
            AddItem(item, sendMessage, true);
        }

        public void CleanItem()
        {
            Character.Inventory.Items[ContainerType.BODY].Clear();
            Character.Inventory.CalcPayloadVolume();
            SendCapacity();
        }

        public void AddItem(Item item, bool sendMessage, bool fullgoware)
        {
            var stack = item.Stack;
            //SagaLib.Logger.ShowWarning("1"+item.Stack.ToString()+item.BaseData.name);
            //if (this.Character.Inventory.Items.Count < 1000 || this.Character.Account.GMLevel > 10)
            //{
            //临时解决方案↓↓↓↓↓
            //if (this.Character.Inventory.Items[ContainerType.BODY].Count + this.Character.Inventory.Equipments.Count > 100 && fullgoware)
            //{
            //    string[] names = Enum.GetNames(typeof(ContainerType));
            //    foreach (string i in names)
            //    {
            //        ContainerType container = (ContainerType)Enum.Parse(typeof(ContainerType), i);
            //        List<Item> items = this.Character.Inventory.GetContainer(container);
            //        List<Item> trashItem = new List<Item>();
            //        if (container == ContainerType.BODY)//扫描并删除身上的垃圾数据
            //        {
            //            foreach (Item j in items)
            //            {
            //                if (j.Stack == 0)
            //                    trashItem.Add(j);
            //            }
            //            if (trashItem.Count > 0)
            //            {
            //                for (int y = 0; y < trashItem.Count; y++)
            //                {
            //                    Character.Inventory.Items[ContainerType.BODY].Remove(trashItem[y]);
            //                }
            //            }
            //        }
            //    }
            //}
            //临时解决方案↑↑↑↑↑
            if (Character.Inventory.Items[ContainerType.BODY].Count + Character.Inventory.Equipments.Count > 100 &&
                fullgoware)
            {
                if (Character.CInt["背包满后仓库"] == 1)
                {
                    Character.Inventory.AddWareItem(WarehousePlace.Acropolis, item);
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了第一页仓库。");
                }
                else if (Character.CInt["背包满后仓库"] == 2)
                {
                    Character.Inventory.AddWareItem(WarehousePlace.FarEast, item);
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了第二页仓库。");
                }
                else if (Character.CInt["背包满后仓库"] == 3)
                {
                    Character.Inventory.AddWareItem(WarehousePlace.IronSouth, item);
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了第三页仓库。");
                }
                else if (Character.CInt["背包满后仓库"] == 4)
                {
                    Character.Inventory.AddWareItem(WarehousePlace.Northan, item);
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了第四页仓库。");
                }
                else
                {
                    Character.Inventory.AddWareItem(WarehousePlace.Northan, item);
                    SendSystemMessage("背包已满，物品『" + item.BaseData.name + "』存入了第四页仓库。");
                }
            }
            else
            {
                var result = Character.Inventory.AddItem(ContainerType.BODY, item);
                SendItemAdd(item, ContainerType.BODY, result, stack, sendMessage);

                Character.Inventory.CalcPayloadVolume();
                SendCapacity();
                //this.SendItems();
            }
            /*}
            else
            {
                this.SendSystemMessage("道具栏已满，无法获得道具。");
                /*this.SendSystemMessage("（本次获得的道具可以向 吉田佳美 领取，临时道具只能保存3个，请及时处理道具栏并领取。）");
                if (this.Character.CInt["临时道具1"] == 0)
                    this.Character.CInt["临时道具1"] = (int)item.ItemID;
                else if (this.Character.CInt["临时道具2"] == 0)
                    this.Character.CInt["临时道具2"] = (int)item.ItemID;
                else if (this.Character.CInt["临时道具3"] == 0)
                    this.Character.CInt["临时道具3"] = (int)item.ItemID;*/
        }

        private int CountItem(uint itemID)
        {
            var item = Character.Inventory.GetItem(itemID, Inventory.SearchType.ITEM_ID);
            if (item != null) return item.Stack;

            return 0;
        }

        public Item DeleteItemID(uint itemID, ushort count, bool message)
        {
            var item = Character.Inventory.GetItem(itemID, Inventory.SearchType.ITEM_ID);
            if (item == null) return null;
            var slot = item.Slot;
            var result = Character.Inventory.DeleteItem(item.Slot, count);
            if (item.IsEquipt)
            {
                SendEquip();
                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
            }

            switch (result)
            {
                case InventoryDeleteResult.STACK_UPDATED:
                    var p1 = new SSMG_ITEM_COUNT_UPDATE();
                    item = Character.Inventory.GetItem(slot);
                    p1.InventorySlot = slot;
                    p1.Stack = item.Stack;
                    NetIo.SendPacket(p1);
                    break;
                case InventoryDeleteResult.ALL_DELETED:
                    var p2 = new SSMG_ITEM_DELETE();
                    p2.InventorySlot = slot;
                    NetIo.SendPacket(p2);
                    if (item.IsEquipt)
                    {
                        SendAttackType();
                        var p4 = new SSMG_ITEM_EQUIP();
                        p4.InventorySlot = 0xffffffff;
                        p4.Target = ContainerType.NONE;
                        p4.Result = 1;
                        p4.Range = Character.Range;
                        NetIo.SendPacket(p4);
                    }

                    break;
            }

            Character.Inventory.CalcPayloadVolume();
            SendCapacity();
            if (message)
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_DELETED, item.BaseData.name, count));
            return item;
        }

        public void DeleteItem(uint slot, ushort count, bool message)
        {
            var item = Character.Inventory.GetItem(slot);
            var container = Character.Inventory.GetContainerType(item.Slot);
            var equiped = false;
            if (container >= ContainerType.HEAD && container <= ContainerType.PET)
                equiped = true;
            var result = Character.Inventory.DeleteItem(slot, count);
            if (equiped)
            {
                SendEquip();
                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();
            }

            switch (result)
            {
                case InventoryDeleteResult.STACK_UPDATED:
                    var p1 = new SSMG_ITEM_COUNT_UPDATE();
                    item = Character.Inventory.GetItem(slot);
                    p1.InventorySlot = slot;
                    p1.Stack = item.Stack;
                    NetIo.SendPacket(p1);
                    break;
                case InventoryDeleteResult.ALL_DELETED:
                    var p2 = new SSMG_ITEM_DELETE();
                    p2.InventorySlot = slot;
                    NetIo.SendPacket(p2);
                    Character.Inventory.GetContainerType(slot);
                    if (equiped)
                    {
                        SendAttackType();
                        var p4 = new SSMG_ITEM_EQUIP();
                        p4.InventorySlot = 0xffffffff;
                        p4.Target = ContainerType.NONE;
                        p4.Result = 1;
                        p4.Range = Character.Range;
                        NetIo.SendPacket(p4);
                        if (item.BaseData.itemType == ItemType.ARROW || item.BaseData.itemType == ItemType.BULLET)
                        {
                            var dummy = Character.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Clone();
                            dummy.Stack = 0;
                            Character.Inventory.AddItem(ContainerType.LEFT_HAND, dummy);
                        }
                    }

                    break;
            }

            Character.Inventory.CalcPayloadVolume();
            SendCapacity();
            if (message)
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_DELETED, item.BaseData.name, count));
        }

        public void SendPet(Item item)
        {
            if (item.BaseData.itemType != ItemType.BACK_DEMON && item.BaseData.itemType != ItemType.RIDE_PET &&
                item.BaseData.itemType != ItemType.RIDE_PARTNER)
            {
                var pet = new ActorPet(item.BaseData.petID, item);
                Character.Pet = pet;
                //砍掉PET
                /*
                pet.MapID = this.Character.MapID;
                pet.X = this.Character.X;
                pet.Y = this.Character.Y;
                pet.Owner = this.Character;
                ActorEventHandlers.PetEventHandler eh = new ActorEventHandlers.PetEventHandler(pet);
                pet.e = eh;
                if (Mob.MobAIFactory.Instance.Items.ContainsKey(item.BaseData.petID))
                    eh.AI.Mode = Mob.MobAIFactory.Instance.Items[item.BaseData.petID];
                else
                    eh.AI.Mode = new SagaMap.Mob.AIMode(0);
                eh.AI.Start();
                //Mob.AIThread.Instance.RegisterAI(eh.AI);

                this.map.RegisterActor(pet);
                pet.invisble = false;
                this.map.OnActorVisibilityChange(pet);
                this.map.SendVisibleActorsToActor(pet);//*/
            }
        }

        public void DeletePet()
        {
            if (Character.Partner != null)
            {
                MapManager.Instance.GetMap(Character.Partner.MapID).DeleteActor(Character.Partner);
                Character.Partner = null;
                return;
            }


            if (Character.Pet != null)
            {
            }
            else
            {
                //AI被砍掉！
                //參考SendPet()

                var eh = (PetEventHandler)Character.Pet.e;
                eh.AI.Pause();
                eh.AI.Activated = false;
                MapManager.Instance.GetMap(Character.Pet.MapID).DeleteActor(Character.Pet);
                Character.Pet = null;
            }

            //Ride 沒有被定義，請考慮Default 宣告false！
            //if (this.Character.Pet.Ride)
            //return;
        }

        public void OnItemChangeSlot(CSMG_ITEM_CHANGE_SLOT p)
        {
        }

        private bool confirmed;
        public bool npcTrade;
        public List<Item> npcTradeItem;
        private bool performed;
        private List<ushort> tradeCounts;
        private List<uint> tradeItems;
        private bool trading;
        private long tradingGold;
        private ActorPC tradingTarget;

        public void OnTradeRequest(CSMG_TRADE_REQUEST p)
        {
            var actor = Map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.PC)
                return;
            var pc = (ActorPC)actor;
            var client = FromActorPC(pc);
            var result = CheckTradeRequest(client);

            if (result == 0)
            {
                tradingTarget = pc;
                client.SendTradeRequest(Character);
            }

            var p1 = new SSMG_TRADE_REQUEST_RESULT();
            p1.Result = result;
            NetIo.SendPacket(p1);
        }

        private int CheckTradeRequest(MapClient client)
        {
            if (trading)
                return -1; //トレード中です
            if (scriptThread != null)
                return -2; //イベント中です
            if (client.trading)
                return -3; //相手がトレード中です
            if (client.scriptThread != null)
                return -4; //相手がイベント中です
            if (Character.Golem != null)
                return -7; //ゴーレムショップ起動中です
            if (client.Character.Golem != null)
                return -8; //相手がゴーレムショップ起動中です
            if (Character.PossessionTarget != 0)
                return -9; //憑依中です
            if (client.Character.PossessionTarget != 0)
                return -10; //相手が憑依中です
            if (!client.Character.canTrade)
                return -11; //相手のトレード設定が不許可になっています
            if (Character.Buff.FishingState || Character.Buff.Dead || Character.Buff.Confused ||
                Character.Buff.Frosen || Character.Buff.Paralysis || Character.Buff.Sleep || Character.Buff.Stone ||
                Character.Buff.Stun
                || client.Character.Buff.Dead || client.Character.Buff.Confused || client.Character.Buff.Frosen ||
                client.Character.Buff.Paralysis || client.Character.Buff.Sleep || client.Character.Buff.Stone ||
                client.Character.Buff.Stun)
                return -12; //トレードを行える状態ではありません
            if (Math.Abs(Character.X - client.Character.X) > 300 || Math.Abs(Character.Y - client.Character.Y) > 300)
                return -13; //トレード相手との距離が離れすぎています
            return 0;
        }


        public void OnTradeRequestAnswer(CSMG_TRADE_REQUEST_ANSWER p)
        {
            if (tradingTarget == null)
                return;
            if (tradingTarget.MapID != Character.MapID)
                return;
            var client = FromActorPC(tradingTarget);
            switch (p.Answer)
            {
                case 1:
                    trading = true;
                    client.trading = true;

                    confirmed = false;
                    performed = false;
                    client.confirmed = false;
                    client.performed = false;

                    SendTradeStart();
                    SendTradeStatus(true, false);
                    client.SendTradeStart();
                    client.SendTradeStatus(true, false);
                    break;
                default:
                    tradingTarget = null;
                    client.tradingTarget = null;
                    var p1 = new SSMG_TRADE_REQUEST_RESULT();
                    p1.Result = -6;
                    client.NetIo.SendPacket(p1);
                    break;
            }
        }

        private List<ItemType> CP10TypeList()
        {
            var list = new List<ItemType>();
            list.Add(ItemType.FURNITURE);

            return list;
        }

        private long GetGoldForRecycle(List<uint> tradeItems, List<ushort> tradeCounts)
        {
            var zero = ZeroPriceList();
            long gold = 0;
            for (var i = 0; i < this.tradeItems.Count; i++)
            {
                var item = Character.Inventory.GetItem(this.tradeItems[i]).Clone();
                if (item.Stack < this.tradeCounts[i])
                {
                    this.tradeCounts[i] = item.Stack;
                    //SendSystemMessage("你试图通过某种方法修改交易数量！你已经被记录于系统，请联系管理员接受处理。");
                    //Character.Account.Banned = true;
                    var log = new Logger("玩家异常.txt");
                    var logtext = "\r\n" + Character.Name + "使用了交易，修改了数量：" + this.tradeCounts[i] + "/" + item.Stack;
                    log.WriteLog(logtext);
                }

                item.Stack = this.tradeCounts[i];

                var g = item.BaseData.price;
                if (g < 5) g = 10;
                if (zero.Contains(item.ItemID))
                    g = 0;
                if (g > 500) g = 500;
                if (item.BaseData.itemType == ItemType.FURNITURE) //家具类
                    g = 2000;
                if (item.EquipSlot.Count >= 1) //装备类
                {
                    g = (uint)(1000 + 1000 * item.EquipSlot.Count);
                    if (g > 5000)
                        g = 5000;
                }

                gold += g * item.Stack / 100;
            }

            return gold;
        }

        private void OnTradeItemNPC(CSMG_TRADE_ITEM p)
        {
            if (tradeItems != null)
                if (tradeItems.Count != 0)
                {
                    confirmed = false;
                    performed = false;
                    SendTradeStatus(true, false);
                }

            tradeItems = p.InventoryID;
            tradeCounts = p.Count;
            tradingGold = p.Gold;


            var gold = GetGoldForRecycle(tradeItems, tradeCounts);
            var p3 = new SSMG_TRADE_GOLD();
            p3.Gold = 0; // gold;
            NetIo.SendPacket(p3);
            tradingGold = gold; // gold;
        }

        public void OnTradeItem(CSMG_TRADE_ITEM p)
        {
            if (npcTrade)
            {
                OnTradeItemNPC(p);
                return;
            }

            if (tradingTarget == null)
                return;
            var client = FromActorPC(tradingTarget);
            if (tradeItems != null)
                if (tradeItems.Count != 0)
                {
                    confirmed = false;
                    client.confirmed = false;
                    performed = false;
                    client.performed = false;
                    SendTradeStatus(true, false);
                    client.SendTradeStatus(true, false);
                }

            tradeItems = p.InventoryID;
            tradeCounts = p.Count;
            tradingGold = p.Gold;
            //if (Character.Account.GMLevel < 200)
            //    tradingGold = 0;
            var p1 = new SSMG_TRADE_ITEM_HEAD();
            client.NetIo.SendPacket(p1);
            for (var i = 0; i < tradeItems.Count; i++)
            {
                var p2 = new SSMG_TRADE_ITEM_INFO();
                var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                if (Character.Account.GMLevel < 100)
                    if (item.BaseData.noTrade || item.BaseData.itemType == ItemType.DEMIC_CHIP)
                    {
                        tradeItems[i] = 0;
                        tradeCounts[i] = 0;
                        continue;
                    }

                if (item.PossessionOwner != null)
                    if (item.PossessionOwner.CharID != Character.CharID)
                    {
                        tradeItems[i] = 0;
                        tradeCounts[i] = 0;
                        continue;
                    }

                if (item.Stack < tradeCounts[i])
                    tradeCounts[i] = item.Stack;
                item.Stack = tradeCounts[i];
                p2.Item = item;
                p2.InventorySlot = tradeItems[i];
                p2.Container = ContainerType.BODY;
                Logger.ShowInfo("尝试交易道具:" + item.ItemID + "[" + item.Name + "] " + item.Stack + "个  , 道具栏ID: " +
                                tradeItems[i]);
                client.NetIo.SendPacket(p2);
            }

            var p3 = new SSMG_TRADE_GOLD();
            p3.Gold = tradingGold;
            client.NetIo.SendPacket(p3);
            var p4 = new SSMG_TRADE_ITEM_FOOT();
            client.NetIo.SendPacket(p4);
        }

        public void OnTradeConfirm(CSMG_TRADE_CONFIRM p)
        {
            if (npcTrade)
            {
                switch (p.State)
                {
                    case 0:
                        confirmed = false;
                        break;
                    case 1:
                        confirmed = true;
                        break;
                }

                if (confirmed) SendTradeStatus(false, true);
            }

            if (tradingTarget == null)
                return;
            switch (p.State)
            {
                case 0:
                    confirmed = false;
                    break;
                case 1:
                    confirmed = true;
                    break;
            }

            if (confirmed && FromActorPC(tradingTarget).confirmed)
            {
                SendTradeStatus(false, true);
                FromActorPC(tradingTarget).SendTradeStatus(false, true);
            }
        }

        public void OnTradePerform(CSMG_TRADE_PERFORM p)
        {
            if (npcTrade)
            {
                PerformTradeNPC();
                return;
            }

            if (tradingTarget == null)
                return;
            switch (p.State)
            {
                case 0:
                    performed = false;
                    break;
                case 1:
                    performed = true;
                    break;
            }

            var client = FromActorPC(tradingTarget);
            if (performed && client.performed)
            {
                if (Character.Gold >= tradingGold &&
                    client.Character.Gold >= client.tradingGold &&
                    Character.Gold + client.tradingGold < 10000000000 && //金钱上限为1亿
                    client.Character.Gold + tradingGold < 10000000000
                   )
                {
                    SendTradeEnd(2);
                    PerformTrade();
                    client.SendTradeEnd(2);
                    client.PerformTrade();
                }

                SendTradeEnd(1);
                client.SendTradeEnd(1);
            }
        }

        public void OnTradeCancel(CSMG_TRADE_CANCEL p)
        {
            if (npcTrade)
            {
                npcTradeItem = new List<Item>();
                npcTrade = false;
                SendTradeEnd(3);
                return;
            }

            if (tradingTarget == null)
                return;

            FromActorPC(tradingTarget).SendTradeEnd(3);
            SendTradeEnd(3);
        }

        public List<uint> ZeroPriceList()
        {
            var l = KujiListFactory.Instance.ZeroPriceList;
            return l;
        }

        private void PerformTradeNPC()
        {
            npcTradeItem = new List<Item>();
            SendTradeEnd(2);
            if (tradeItems != null)
                for (var i = 0; i < tradeItems.Count; i++)
                {
                    var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                    item.Stack = tradeCounts[i];
                    Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("NPCTrade Count:{0}", item.Stack), false);
                    DeleteItem(tradeItems[i], tradeCounts[i], true);
                    npcTradeItem.Add(item);
                }

            //this.Character.Gold -= (int)this.tradingGold;
            if (Character.TInt["垃圾箱记录"] == 1)
            {
                Character.CP += (uint)tradingGold;
                Character.TInt["垃圾箱记录"] = 0;
            }

            //国庆活动
            /*Character.AInt["国庆CP_个人收集"] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.AInt["国庆CP_全服收集"] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.Adict["国庆CP_排行榜"]["国庆CP_" + Character.Account.AccountID.ToString()] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.AStr["国庆CP_" + Character.Account.AccountID.ToString()] = Character.Name;*/

            SendGoldUpdate();
            performed = true;
            npcTrade = false;

            SendTradeEnd(1);
        }

        public void PerformTrade()
        {
            if (tradingTarget == null)
                return;
            var client = FromActorPC(tradingTarget);
            if (tradeItems != null)
                for (var i = 0; i < tradeItems.Count; i++)
                {
                    if (tradeItems[i] == 0)
                        continue;
                    var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                    item.Stack = tradeCounts[i];
                    Logger.LogItemLost(Logger.EventType.ItemTradeLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("Trade Count:{0} To:{1}({2})", tradeCounts[i], client.Character.Name,
                            client.Character.CharID), false);
                    DeleteItem(tradeItems[i], tradeCounts[i], true);
                    Logger.LogItemGet(Logger.EventType.ItemTradeGet,
                        client.Character.Name + "(" + client.Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("Trade Count:{0} From:{1}({2})", item.Stack, Character.Name, Character.CharID),
                        false);
                    client.AddItem(item, true);
                }

            Character.Gold -= (int)tradingGold;
            SendGoldUpdate();
            client.Character.Gold += (int)tradingGold;
            client.SendGoldUpdate();
        }

        /// <summary>
        ///     结束交易
        /// </summary>
        /// <param name="type">1，清空变量，2，发送结束封包，3，两个都执行</param>
        public void SendTradeEnd(int type)
        {
            if (type == 1 || type == 3)
            {
                tradeCounts = null;
                tradeItems = null;
                trading = false;
                tradingGold = 0;
                tradingTarget = null;
                confirmed = false;
                performed = false;
            }

            if (type == 2 || type == 3)
            {
                var p = new SSMG_TRADE_END();
                NetIo.SendPacket(p);
            }
        }

        public void SendTradeStatus(bool canConfirm, bool canPerform)
        {
            var p = new SSMG_TRADE_STATUS();
            p.Confirm = canConfirm;
            p.Perform = canPerform;
            NetIo.SendPacket(p);
        }

        public void SendTradeStart()
        {
            if (tradingTarget == null)
                return;
            var p = new SSMG_TRADE_START();
            p.SetPara(tradingTarget.Name, 0);
            NetIo.SendPacket(p);
        }

        public void SendTradeStartNPC(string name)
        {
            if (npcTrade)
            {
                var p = new SSMG_TRADE_START();
                p.SetPara(name, 1);
                NetIo.SendPacket(p);
                SendTradeStatus(true, false);
            }
        }

        public void SendTradeRequest(ActorPC pc)
        {
            var p = new SSMG_TRADE_REQUEST();
            p.Name = pc.Name;
            NetIo.SendPacket(p);

            tradingTarget = pc;
        }

        public bool bbsClose;
        public uint bbsCost;
        public int bbsCurrentPage;
        public uint bbsID;
        public uint bbsMinContent;

        public void OnBBSRequestPage(CSMG_COMMUNITY_BBS_REQUEST_PAGE p)
        {
            bbsCurrentPage = p.Page;
            SendBBSPage();
        }

        public void SendBBSPage()
        {
            var p1 = new SSMG_COMMUNITY_BBS_PAGE_INFO();
            p1.Posts = MapServer.charDB.GetBBSPage(bbsID, bbsCurrentPage);
            NetIo.SendPacket(p1);
        }

        public void OnBBSPost(CSMG_COMMUNITY_BBS_POST p)
        {
            var p1 = new SSMG_COMMUNITY_BBS_POST_RESULT();
            var result = CheckBBSPost(p.Title, p.Content);
            if (result >= 0)
                Character.Gold -= (int)bbsCost;
            NetIo.SendPacket(p1);
            SendBBSPage();
        }

        private int CheckBBSPost(string title, string content)
        {
            if (Character.Gold <= bbsCost)
                return -2; //お金が足りません
            if (content.Length < bbsMinContent)
                return -3; //投稿内容の文字数が足りません
            if (!MapServer.charDB.BBSNewPost(Character, bbsID, title, content))
                return -1; //投稿に失敗しました
            return 0; //投稿しました
            // return -4; //投稿回数が制限に達したため失敗しました
        }

        public void OnBBSClose(CSMG_COMMUNITY_BBS_CLOSE p)
        {
            bbsClose = true;
        }

        public void OnRecruit(CSMG_COMMUNITY_RECRUIT p)
        {
            int maxPage;
            var page = p.Page;
            var res = RecruitmentManager.Instance.GetRecruitments(p.Type, page, out maxPage);
            var p1 = new SSMG_COMMUNITY_RECRUIT();
            p1.Type = p.Type;
            p1.Page = page;
            p1.MaxPage = maxPage;
            p1.Entries = res;
            NetIo.SendPacket(p1);
        }

        public void OnRecruitRequestAns(CSMG_COMMUNITY_RECRUIT_REQUEST_ANS p)
        {
            var target = MapClientManager.Instance.FindClient(p.CharID);
            if (target == null || partyPartner == null)
                return;
            if (target.Character.CharID == partyPartner.CharID)
            {
                var result = 0;
                var p1 = new SSMG_COMMUNITY_RECRUIT_JOIN_RES();
                if (Character.Mode != target.Character.Mode)
                    return;
                if (!p.Accept)
                    result = -2; //パーティー参加要請を断られました
                if (result >= 0)
                {
                    if (Character.Party == null)
                    {
                        var party = PartyManager.Instance.CreateParty(Character);
                        PartyManager.Instance.AddMember(party, partyPartner);
                    }
                    else if (Character.Party.MemberCount >= 8)
                    {
                        result = -3; //要請をしたパーティーは満員です
                    }
                }

                if (result >= 0)
                    PartyManager.Instance.AddMember(Character.Party, partyPartner);
                p1.Result = result;
                p1.CharID = Character.CharID;
                target.NetIo.SendPacket(p1);
            }
        }

        public void OnRecruitJoin(CSMG_COMMUNITY_RECRUIT_JOIN p)
        {
            var target = MapClientManager.Instance.FindClient(p.CharID);
            var result = CheckRecuitJoin(target);
            var p1 = new SSMG_COMMUNITY_RECRUIT_JOIN_RES();
            p1.Result = result;
            p1.CharID = p.CharID;
            NetIo.SendPacket(p1);
            if (result >= 0)
            {
                partyPartner = target.Character;
                target.partyPartner = Character;
                var p2 = new SSMG_COMMUNITY_RECRUIT_REQUEST();
                p2.CharID = Character.CharID;
                p2.CharName = Character.Name;
                target.NetIo.SendPacket(p2);
            }
        }

        private int CheckRecuitJoin(MapClient target)
        {
            // return -1; //パーティーサーバーとの接続に失敗しました
            // return -2; //パーティー参加要請を断られました
            if (target == null)
                return -4; //パーティー要請をした相手がオフライン中です
            if (target.Character.Party != null)
                if (target.Character.Party.MemberCount >= 8)
                    return -3; //要請をしたパーティーは満員です
            if (Character.Party != null)
                return -5; //パーティー参加中に要請はできません
            if (target.Character.CharID == Character.CharID)
                return -6; //自分にパーティー申請はできません
            var exist = false;
            var recruitlist = RecruitmentManager.Instance.GetRecruitments(RecruitmentType.Party);
            for (var i = 0; i < recruitlist.Count; i++)
                if (recruitlist[i].Creator.Name == target.Character.Name)
                    exist = true;
            if (!exist)
                return -7; //募集コメントの登録が消えています
            return 0; //参加要請中です
        }

        public void OnRecruitCreate(CSMG_COMMUNITY_RECRUIT_CREATE p)
        {
            var rec = new Recruitment();
            rec.Creator = Character;
            rec.Type = p.Type;
            rec.Title = p.Title;
            rec.Content = p.Content;
            RecruitmentManager.Instance.CreateRecruiment(rec);

            var p1 = new SSMG_COMMUNITY_RECRUIT_CREATE();
            NetIo.SendPacket(p1);
        }

        public void OnRecruitDelete(CSMG_COMMUNITY_RECRUIT_DELETE p)
        {
            RecruitmentManager.Instance.DeleteRecruitment(Character);
            var p1 = new SSMG_COMMUNITY_RECRUIT_DELETE();
            NetIo.SendPacket(p1);
        }

        public uint questID;

        public void OnDailyDungeonOpen()
        {
            //if(Character.MapID != 10054000)
            //{
            //    SendSystemMessage("当前区域无法打开每日地牢。");
            //    return;
            //}
            if (Character.AStr["每日地牢记录"] == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                SendSystemMessage("你今天已经入场过了，请明天再来吧。");
                return;
            }

            if (Character.Party != null)
            {
                SendSystemMessage("请先退出队伍。");
                return;
            }

            var p = new SSMG_DAILYDUNGEON_INFO();
            p.RemainSecond = (uint)(86400 - DateTime.Now.Hour * 3600 - DateTime.Now.Minute * 60 - DateTime.Now.Second);
            var ids = new List<byte>();
            ids.Add(0);
            p.IDs = ids;
            NetIo.SendPacket(p);
        }

        public void OnDailyDungeonJoin(CSMG_DAILYDUNGEON_JOIN p)
        {
            if (p.QID == 0)
                EventActivate(980000101);
        }

        public void OnQuestDetailRequest(CSMG_QUEST_DETAIL_REQUEST p)
        {
            if (QuestFactory.Instance.Items.ContainsKey(p.QuestID))
            {
                var quest = QuestFactory.Instance.Items[p.QuestID];
                uint map1 = 0, map2 = 0, map3 = 0;
                string name1 = " ", name2 = " ", name3 = " ";
                NPC npc1 = null, npc2 = null, npc3 = null;
                if (NPCFactory.Instance.Items.ContainsKey(quest.NPCSource))
                    npc2 = NPCFactory.Instance.Items[quest.NPCSource];
                if (NPCFactory.Instance.Items.ContainsKey(quest.NPCDestination))
                    npc3 = NPCFactory.Instance.Items[quest.NPCDestination];
                if (npc1 != null)
                {
                    map1 = npc1.MapID;
                    name1 = npc1.Name;
                }

                if (npc2 != null)
                {
                    map2 = npc2.MapID;
                    name2 = npc2.Name;
                }

                if (npc3 != null)
                {
                    map3 = npc3.MapID;
                    name3 = npc3.Name;
                }

                var p2 = new SSMG_QUEST_DETAIL();
                p2.SetDetail(quest.QuestType, quest.Name, map1, map2, map3, name1, name2, name3, quest.MapID1,
                    quest.MapID2, quest.MapID3, quest.ObjectID1, quest.ObjectID2, quest.ObjectID3, (uint)quest.Count1,
                    (uint)quest.Count2, (uint)quest.Count3, quest.TimeLimit, 0);
                NetIo.SendPacket(p2);
            }
        }

        public void OnQuestSelect(CSMG_QUEST_SELECT p)
        {
            questID = p.QuestID;
        }

        public void SendQuestInfo()
        {
            var quest = Character.Quest;
            uint map1 = 0, map2 = 0, map3 = 0;
            string name1 = " ", name2 = " ", name3 = " ";
            if (quest == null)
                return;
            var p2 = new SSMG_QUEST_ACTIVATE();
            NPC npc1 = null, npc2 = null, npc3 = null;
            npc1 = quest.NPC;
            if (npc1 == null && NPCFactory.Instance.Items.ContainsKey(currentEventID))
                npc1 = NPCFactory.Instance.Items[currentEventID];
            if (NPCFactory.Instance.Items.ContainsKey(quest.Detail.NPCSource))
                npc2 = NPCFactory.Instance.Items[quest.Detail.NPCSource];
            if (NPCFactory.Instance.Items.ContainsKey(quest.Detail.NPCDestination))
                npc3 = NPCFactory.Instance.Items[quest.Detail.NPCDestination];
            if (npc1 != null)
            {
                map1 = npc1.MapID;
                name1 = npc1.Name;
            }

            if (npc2 != null)
            {
                map2 = npc2.MapID;
                name2 = npc2.Name;
            }

            if (npc3 != null)
            {
                map3 = npc3.MapID;
                name3 = npc3.Name;
            }

            //p2.SetDetail(quest.QuestType, quest.Name, map1, map2, map3, name1, name2, name3, quest.Status, quest.Detail.MapID1, quest.Detail.MapID2, quest.Detail.MapID3, quest.Detail.ObjectID1, quest.Detail.ObjectID2, quest.Detail.ObjectID3, (uint)quest.Detail.Count1, (uint)quest.Detail.Count2, (uint)quest.Detail.Count3, quest.Detail.TimeLimit, 0);
            p2.SetDetail(quest.ID, currentEventID, quest.Detail.NPCSource, quest.Detail.NPCDestination, quest.Status,
                quest.Detail.MapID1, quest.Detail.MapID2, quest.Detail.MapID3, quest.Detail.ObjectID1,
                quest.Detail.ObjectID2, quest.Detail.ObjectID3, (uint)quest.Detail.Count1, (uint)quest.Detail.Count2,
                (uint)quest.Detail.Count3, quest.Detail.TimeLimit, 0, quest.Detail.EXP, 0, quest.Detail.JEXP,
                quest.Detail.Gold);
            p2.DumpData();
            NetIo.SendPacket(p2);
        }

        public void SendQuestPoints()
        {
            var p = new SSMG_QUEST_POINT();
            if (Character.QuestNextResetTime > DateTime.Now)
            {
                p.ResetTime = (uint)(Character.QuestNextResetTime - DateTime.Now).TotalHours;
            }
            else
            {
                var hours = (int)(DateTime.Now - Character.QuestNextResetTime).TotalHours;
                if (hours > 24000)
                {
                    Character.QuestNextResetTime =
                        DateTime.Now + new TimeSpan(0, Configuration.Configuration.Instance.QuestUpdateTime, 0, 0);
                }
                else
                {
                    if (Character.Account.questNextTime <= Character.QuestNextResetTime)
                    {
                        Character.QuestRemaining +=
                            (ushort)((hours / Configuration.Configuration.Instance.QuestUpdateTime + 1) *
                                     Configuration.Configuration.Instance.QuestUpdateAmount);
                        if (Character.QuestRemaining > Configuration.Configuration.Instance.QuestPointsMax)
                            Character.QuestRemaining = (ushort)Configuration.Configuration.Instance.QuestPointsMax;
                        Character.QuestNextResetTime = Character.QuestNextResetTime + new TimeSpan(0,
                            (hours / Configuration.Configuration.Instance.QuestUpdateTime + 1) *
                            Configuration.Configuration.Instance.QuestUpdateTime, 0, 0);
                        Character.Account.questNextTime = Character.QuestNextResetTime;
                    }
                    else
                    {
                        Character.QuestNextResetTime = Character.Account.questNextTime;
                    }
                }

                p.ResetTime = (uint)(Character.QuestNextResetTime - DateTime.Now).TotalHours;
            }

            p.QuestPoint = Character.QuestRemaining;
            NetIo.SendPacket(p);
        }

        public void SendQuestCount()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_COUNT_UPDATE();
                p.Count1 = Character.Quest.CurrentCount1;
                p.Count2 = Character.Quest.CurrentCount2;
                p.Count3 = Character.Quest.CurrentCount3;
                NetIo.SendPacket(p);
                if (Character.Quest.Status != QuestStatus.FAILED)
                    if (Character.Quest.CurrentCount1 == Character.Quest.Detail.Count1 &&
                        Character.Quest.CurrentCount2 == Character.Quest.Detail.Count2 &&
                        Character.Quest.CurrentCount3 == Character.Quest.Detail.Count3 &&
                        Character.Quest.QuestType != QuestType.TRANSPORT)
                    {
                        Character.Quest.Status = QuestStatus.COMPLETED;
                        SendQuestStatus();
                    }
            }
        }

        public void SendQuestTime()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_RESTTIME_UPDATE();
                if (Character.Quest.EndTime > DateTime.Now)
                {
                    p.RestTime = (int)(Character.Quest.EndTime - DateTime.Now).TotalMinutes;
                }
                else
                {
                    if (Character.Quest.Status != QuestStatus.COMPLETED)
                    {
                        Character.Quest.Status = QuestStatus.FAILED;
                        SendQuestStatus();
                    }
                }

                NetIo.SendPacket(p);
            }
        }

        public void SendQuestStatus()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_STATUS_UPDATE();
                p.Status = Character.Quest.Status;
                NetIo.SendPacket(p);
            }
        }

        public void SendQuestList(List<QuestInfo> quests)
        {
            var p = new SSMG_QUEST_LIST();
            p.Quests = quests;
            NetIo.SendPacket(p);
        }

        public void SendQuestWindow()
        {
            var p = new SSMG_QUEST_WINDOW();
            NetIo.SendPacket(p);
        }

        public void SendQuestDelete()
        {
            var p = new SSMG_QUEST_DELETE();
            NetIo.SendPacket(p);
        }

        public void QuestMobKilled(ActorMob mob, bool party)
        {
            if (Character.Quest != null)
                if (Character.Quest.QuestType == QuestType.HUNT)
                {
                    if (party && !Character.Quest.Detail.Party)
                        return;
                    if (mob.MapID == Character.Quest.Detail.MapID1 ||
                        mob.MapID == Character.Quest.Detail.MapID2 ||
                        mob.MapID == Character.Quest.Detail.MapID3 ||
                        (Character.Quest.Detail.MapID1 == 0 && Character.Quest.Detail.MapID2 == 0 &&
                         Character.Quest.Detail.MapID3 == 0) ||
                        (Character.Quest.Detail.MapID1 == 60000000 && map.IsDungeon) ||
                        (Character.Quest.Detail.MapID1 == map.ID / 1000 * 1000 && map.IsMapInstance) ||
                        (Character.Quest.Detail.MapID2 == map.ID / 1000 * 1000 && map.IsMapInstance) ||
                        (Character.Quest.Detail.MapID3 == map.ID / 1000 * 1000 && map.IsMapInstance))
                    {
                        if (Character.Quest.Detail.ObjectID1 == mob.MobID)
                            Character.Quest.CurrentCount1++;
                        if (Character.Quest.Detail.ObjectID1 == 0 && Character.Quest.Detail.Count1 != 0)
                            Character.Quest.CurrentCount1++;

                        if (Character.Quest.Detail.ObjectID2 == mob.MobID)
                            Character.Quest.CurrentCount2++;
                        if (Character.Quest.Detail.ObjectID2 == 0 && Character.Quest.Detail.Count2 != 0)
                            Character.Quest.CurrentCount2++;

                        if (Character.Quest.Detail.ObjectID3 == mob.MobID)
                            Character.Quest.CurrentCount3++;
                        if (Character.Quest.Detail.ObjectID3 == 0 && Character.Quest.Detail.Count3 != 0)
                            Character.Quest.CurrentCount3++;

                        if (Character.Quest.CurrentCount1 > Character.Quest.Detail.Count1)
                            Character.Quest.CurrentCount1 = Character.Quest.Detail.Count1;
                        if (Character.Quest.CurrentCount2 > Character.Quest.Detail.Count2)
                            Character.Quest.CurrentCount2 = Character.Quest.Detail.Count2;
                        if (Character.Quest.CurrentCount3 > Character.Quest.Detail.Count3)
                            Character.Quest.CurrentCount3 = Character.Quest.Detail.Count3;
                        SendQuestCount();
                    }
                }
        }

        public void EventMobKilled(ActorMob mob)
        {
            var MobId = mob.MobID;
            foreach (var i in Character.KillList)
                if (!i.Value.isFinish)
                {
                    if (i.Key == MobId)
                    {
                        i.Value.Count++;
                        SendSystemMessage("击杀任务：已击杀 " + mob.BaseData.name + " (" + i.Value.Count + "/" +
                                          i.Value.TotalCount + ")");
                        if (i.Value.Count == i.Value.TotalCount)
                            SendSystemMessage("击杀任务：击杀 " + mob.BaseData.name + " 已完成！");
                    }

                    if (i.Value.Count >= i.Value.TotalCount)
                        i.Value.isFinish = true;
                }
        }

        public void OnFGardenFurnitureUse(CSMG_FGARDEN_FURNITURE_USE p)
        {
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);

            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;

            //this.Character.NowUseFurnitureID = p.ActorID;

            //Item item = ItemFactory.Instance.GetItem(furniture.ItemID);
            var f = FurnitureFactory.Instance.GetFurniture(furniture.ItemID);

            if (f.Motion.Count() <= 0)
            {
                EventActivate(31080000);
                return;
            }

            if (f.Motion.Count() > 1)
            {
                int res;
                //多選
                var ps = new SSMG_NPC_SELECT();
                ps.SetSelect("要做什麼？", "", f.Motion.Select(x => x.ToString()).ToArray(), false);

                npcSelectResult = -1;
                NetIo.SendPacket(ps);

                var blocked = ClientManager.Blocked;
                if (blocked)
                    ClientManager.LeaveCriticalArea();
                while (npcSelectResult == -1) Thread.Sleep(500);
                if (blocked)
                    ClientManager.EnterCriticalArea();
                var ps2 = new SSMG_NPC_SELECT_RESULT();
                NetIo.SendPacket(ps2);
                res = npcSelectResult;

                SendSystemMessage("家具Select: " + res);
                SendSystemMessage("家具Set motion: " + f.Motion[res - 1]);

                /*
                if (res > 0)
                {
                    furniture.Motion = f.Motion[(res-1)];
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
                */
            }
            else
            {
                //單選
                if (furniture.Motion != f.DefaultMotion)
                {
                    furniture.Motion = f.DefaultMotion;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
                else
                {
                    furniture.Motion = f.Motion[0];
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
            }


            EventActivate(31000000);
        }

        public void OnFGardenFurnitureReconfig(CSMG_FGARDEN_FURNITURE_RECONFIG p)
        {
            if (Character.FlyingGarden == null)
                return;
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            if (Character.MapID != Character.FlyingGarden.MapID && Character.MapID != Character.FlyingGarden.RoomMapID)
            {
                var p1 = new SSMG_FG_FURNITURE_RECONFIG();
                p1.ActorID = actor.ActorID;
                p1.X = actor.X;
                p1.Y = actor.Y;
                p1.Z = ((ActorFurniture)actor).Z;
                p1.Dir = actor.Dir;
                NetIo.SendPacket(p1);
                return;
            }

            map.MoveActor(Map.MOVE_TYPE.START, actor, new[] { p.X, p.Y, p.Z }, p.Dir, 200);
        }

        public void OnFGardenFurnitureRemove(CSMG_FGARDEN_FURNITURE_REMOVE p)
        {
            if (Character.FlyingGarden == null)
                return;
            if (Character.MapID != Character.FlyingGarden.MapID && Character.MapID != Character.FlyingGarden.RoomMapID)
                return;
            Map map = null;
            map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            map.DeleteActor(actor);
            var item = ItemFactory.Instance.GetItem(furniture.ItemID);
            item.PictID = furniture.PictID;
            if (Character.MapID == Character.FlyingGarden.MapID)
                Character.FlyingGarden.Furnitures[FurniturePlace.GARDEN].Remove(furniture);
            else
                Character.FlyingGarden.Furnitures[FurniturePlace.ROOM].Remove(furniture);
            AddItem(item, false);
            SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_REMOVE, furniture.Name,
                Character.FlyingGarden.Furnitures[FurniturePlace.GARDEN].Count +
                Character.FlyingGarden.Furnitures[FurniturePlace.ROOM].Count,
                Configuration.Configuration.Instance.MaxFurnitureCount));
        }

        public void OnFGardenFurnitureSetup(CSMG_FGARDEN_FURNITURE_SETUP p)
        {
            if (Character.FlyingGarden == null)
                return;
            if (Character.MapID != Character.FlyingGarden.MapID && Character.MapID != Character.FlyingGarden.RoomMapID)
                return;
            if (Character.FlyingGarden.Furnitures[FurniturePlace.GARDEN].Count +
                Character.FlyingGarden.Furnitures[FurniturePlace.ROOM].Count <
                Configuration.Configuration.Instance.MaxFurnitureCount)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                var actor = new ActorFurniture();

                DeleteItem(p.InventorySlot, 1, false);

                actor.MapID = Character.MapID;
                actor.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.X = p.X;
                actor.Y = p.Y;
                actor.Z = p.Z;
                //actor.Dir = p.Dir;
                actor.Xaxis = p.AxleX;
                actor.Yaxis = p.AxleY;
                actor.Zaxis = p.AxleZ;
                actor.Name = item.BaseData.name;
                actor.PictID = item.PictID;
                actor.e = new NullEventHandler();
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);

                if (Character.MapID == Character.FlyingGarden.MapID)
                    Character.FlyingGarden.Furnitures[FurniturePlace.GARDEN].Add(actor);
                else
                    Character.FlyingGarden.Furnitures[FurniturePlace.ROOM].Add(actor);
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_SETUP, actor.Name,
                    Character.FlyingGarden.Furnitures[FurniturePlace.GARDEN].Count +
                    Character.FlyingGarden.Furnitures[FurniturePlace.ROOM].Count,
                    Configuration.Configuration.Instance.MaxFurnitureCount));
            }
            else
            {
                SendSystemMessage(LocalManager.Instance.Strings.FG_FUTNITURE_MAX);
            }
        }

        public void OnFGardenEquipt(CSMG_FGARDEN_EQUIPT p)
        {
            if (Character.FlyingGarden == null)
                return;
            if (Character.MapID != Character.FlyingGarden.MapID && Character.MapID != Character.FlyingGarden.RoomMapID)
                return;
            if (p.InventorySlot != 0xFFFFFFFF)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item == null)
                    return;
                if (Character.FlyingGarden.FlyingGardenEquipments[p.Place] != 0)
                {
                    var itemID = Character.FlyingGarden.FlyingGardenEquipments[p.Place];
                    AddItem(ItemFactory.Instance.GetItem(itemID, true), false);
                    var p1 = new SSMG_FG_EQUIPT();
                    p1.ItemID = 0;
                    p1.Place = p.Place;
                    NetIo.SendPacket(p1);
                }

                if (p.Place == FlyingGardenSlot.GARDEN_MODELHOUSE &&
                    Character.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE] == 0)
                {
                    var p1 = new SSMG_NPC_SET_EVENT_AREA();
                    p1.EventID = 10000315;
                    p1.StartX = 6;
                    p1.StartY = 7;
                    p1.EndX = 6;
                    p1.EndY = 7;
                    NetIo.SendPacket(p1);
                }

                Character.FlyingGarden.FlyingGardenEquipments[p.Place] = item.ItemID;
                var p2 = new SSMG_FG_EQUIPT();
                p2.ItemID = item.ItemID;
                p2.Place = p.Place;
                NetIo.SendPacket(p2);
                DeleteItem(p.InventorySlot, 1, false);
            }
            else
            {
                var itemID = Character.FlyingGarden.FlyingGardenEquipments[p.Place];
                if (itemID != 0)
                    AddItem(ItemFactory.Instance.GetItem(itemID, true), false);
                Character.FlyingGarden.FlyingGardenEquipments[p.Place] = 0;
                var p1 = new SSMG_FG_EQUIPT();
                p1.ItemID = 0;
                p1.Place = p.Place;
                NetIo.SendPacket(p1);
                if (p.Place == FlyingGardenSlot.GARDEN_MODELHOUSE)
                {
                    var p2 = new SSMG_NPC_CANCEL_EVENT_AREA();
                    p2.StartX = 6;
                    p2.StartY = 7;
                    p2.EndX = 6;
                    p2.EndY = 7;
                    NetIo.SendPacket(p2);
                }
            }
        }
    }
}