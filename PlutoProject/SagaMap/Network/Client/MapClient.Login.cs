using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.ODWar;
using SagaDB.Skill;
using SagaDB.Theater;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;
using SagaMap.Scripting;
using SagaMap.Tasks.PC;
using SagaMap.Tasks.System;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
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
            netIO.SendPacket(p2);
        }

        public void OnSendVersion(CSMG_SEND_VERSION p)
        {
            if (Configuration.Instance.ClientVersion == null || Configuration.Instance.ClientVersion == p.GetVersion())
            {
                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.CLIENT_CONNECTING, p.GetVersion()));
                client_Version = p.GetVersion();

                var p1 = new SSMG_VERSION_ACK();
                p1.SetResult(SSMG_VERSION_ACK.Result.OK);
                p1.SetVersion(client_Version);
                netIO.SendPacket(p1);
                //Official HK server will now request for Hackshield GUID check , we don't know its algorithms, so not implemented
                var p2 = new SSMG_LOGIN_ALLOWED();
                frontWord = (uint)Global.Random.Next();
                backWord = (uint)Global.Random.Next();
                p2.FrontWord = frontWord;
                p2.BackWord = backWord;
                netIO.SendPacket(p2);
            }
            else
            {
                var p2 = new SSMG_VERSION_ACK();
                p2.SetResult(SSMG_VERSION_ACK.Result.VERSION_MISSMATCH);
                netIO.SendPacket(p2);
            }
        }

        public void OnLogin(CSMG_LOGIN p)
        {
            p.GetContent();
            if (MapServer.shutingdown)
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                netIO.SendPacket(p1);
                return;
            }

            if (AJImode.Instance.StopLogin)
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                netIO.SendPacket(p1);
                return;
            }

            if (MapServer.accountDB.CheckPassword(p.UserName, p.Password, frontWord, backWord))
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.OK;
                p1.Unknown1 = 0x100;
                p1.TimeStamp = (uint)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

                netIO.SendPacket(p1);
                /*if(MapClientManager.Instance.OnlinePlayer.Count > 3)
                    System.Environment.Exit(System.Environment.ExitCode);*/


                account = MapServer.accountDB.GetUser(p.UserName);
                var check = from acc in MapClientManager.Instance.OnlinePlayer
                    where acc.account.Name == account.Name
                    select acc;
                foreach (var i in check) i.netIO.Disconnect();

                account.LastIP = netIO.sock.RemoteEndPoint.ToString().Split(':')[0];
                account.MacAddress = p.MacAddress;

                //这里检查同mac的已在线玩家, 如果大于或等于2个. 则断开当前请求的连接
                var players = MapClientManager.Instance.OnlinePlayer;
                var insamemac = players.Count(x => x.account.MacAddress == account.MacAddress);
                var insameip = players.Count(x => x.account.LastIP == account.LastIP);
                var onlinecount = Math.Max(insamemac, insameip);
                if (onlinecount >= Configuration.Instance.MaxCharacterInMapServer)
                {
                    netIO.Disconnect();
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
                netIO.SendPacket(p1);
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

                if (Character.DominionStr < Configuration.Instance.StartupSetting[Character.Race].Str)
                    Character.DominionStr = Configuration.Instance.StartupSetting[Character.Race].Str;
                if (Character.DominionDex < Configuration.Instance.StartupSetting[Character.Race].Dex)
                    Character.DominionDex = Configuration.Instance.StartupSetting[Character.Race].Dex;
                if (Character.DominionInt < Configuration.Instance.StartupSetting[Character.Race].Int)
                    Character.DominionInt = Configuration.Instance.StartupSetting[Character.Race].Int;
                if (Character.DominionVit < Configuration.Instance.StartupSetting[Character.Race].Vit)
                    Character.DominionVit = Configuration.Instance.StartupSetting[Character.Race].Vit;
                if (Character.DominionAgi < Configuration.Instance.StartupSetting[Character.Race].Agi)
                    Character.DominionAgi = Configuration.Instance.StartupSetting[Character.Race].Agi;
                if (Character.DominionMag < Configuration.Instance.StartupSetting[Character.Race].Mag)
                    Character.DominionMag = Configuration.Instance.StartupSetting[Character.Race].Mag;

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
                    netIO.SendPacket(p);
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
                foreach (var i in Configuration.Instance.MonitorAccounts)
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
                netIO.SendPacket(p3);
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
                    netIO.SendPacket(p2);
                }

                if (golem.GolemType == GolemType.Buy)
                {
                    var p2 = new SSMG_GOLEM_SHOP_BUY_RESULT();
                    p2.BoughtItems = golem.BoughtItem;
                    netIO.SendPacket(p2);
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
                    netIO.SendPacket(p3);
                    p3 = new SSMG_THEATER_INFO();
                    p3.MessageType = SSMG_THEATER_INFO.Type.MOVIE_ADDRESS;
                    p3.Message = nextMovie.URL;
                    netIO.SendPacket(p3);
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
            netIO.SendPacket(p1);
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
                netIO.SendPacket(p2);
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
                var pr = new Process();
                pr.CheckAPIItem(Character.CharID, this);
                CheckAPI = true;
            }


            //Send Daily Stamp
            var thisDay = DateTime.Today;
            if (Character.AStr["DailyStamp_DAY"] != thisDay.ToString("d"))
            {
                var ds = new SSMG_PLAYER_SHOW_DAILYSTAMP();
                ds.Type = 1;
                netIO.SendPacket(ds);
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
            netIO.SendPacket(p1);
        }

        public void OnSSOLogout(CSMG_SSO_LOGOUT p)
        {
            //竟然不清状态。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            //Packets.Server.SSMG_SSO_LOGOUT p1 = new Packets.Server.SSMG_SSO_LOGOUT();
            //this.netIO.SendPacket(p1);
            netIO.Disconnect();
        }
    }
}