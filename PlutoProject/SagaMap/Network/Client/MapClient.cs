using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SagaDB;
using SagaDB.Actor;
using SagaDB.Marionette;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Scripting;
using SagaMap.Tasks.Golem;

namespace SagaMap.Network.Client
{
    public partial class MapClient : SagaLib.Client
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
            netIO = new NetIO(mSock, mCommandTable, this);
            netIO.SetMode(NetIO.Mode.Server);
            netIO.FirstLevelLength = 2;
            if (netIO.sock.Connected) OnConnect();
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
                if (netIO != null)
                    ip = netIO.sock.RemoteEndPoint.ToString();
                if (Character != null) name = Character.Name;
                if (ip != "" || name != "") return string.Format("{0}({1})", name, ip);

                return "MapClient";
            }
            catch (Exception)
            {
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
                    //this.netIO.Disconnect();
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

                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.PLAYER_LOG_OUT, Character.Name));
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

                if (Character.FGarden != null)
                {
                    if (Character.FGarden.RopeActor != null)
                    {
                        var map = MapManager.Instance.GetMap(Character.FGarden.RopeActor.MapID);
                        map.DeleteActor(Character.FGarden.RopeActor);
                        if (ScriptManager.Instance.Events.ContainsKey(Character.FGarden.RopeActor.EventID))
                            ScriptManager.Instance.Events.Remove(Character.FGarden.RopeActor.EventID);
                        Character.FGarden.RopeActor = null;
                    }

                    if (Character.FGarden.RoomMapID != 0)
                    {
                        var roomMap = MapManager.Instance.GetMap(Character.FGarden.RoomMapID);
                        var gardenMap = MapManager.Instance.GetMap(Character.FGarden.MapID);
                        roomMap.ClientExitMap = gardenMap.ClientExitMap;
                        roomMap.ClientExitX = gardenMap.ClientExitX;
                        roomMap.ClientExitY = gardenMap.ClientExitY;
                        MapManager.Instance.DeleteMapInstance(roomMap.ID);
                        Character.FGarden.RoomMapID = 0;
                    }

                    if (Character.FGarden.MapID != 0)
                    {
                        MapManager.Instance.DeleteMapInstance(Character.FGarden.MapID);
                        Character.FGarden.MapID = 0;
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
            Character.FGarden = null;
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
    }
}