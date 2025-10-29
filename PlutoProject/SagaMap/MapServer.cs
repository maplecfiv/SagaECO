//#define FreeVersion

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using SagaDB;
using SagaDB.Actor;
using SagaDB.DEMIC;
using SagaDB.DualJob;
using SagaDB.ECOShop;
using SagaDB.EnhanceTable;
using SagaDB.Experience;
using SagaDB.FictitiousActors;
using SagaDB.Fish;
using SagaDB.Iris;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Marionette;
using SagaDB.MasterEnchance;
using SagaDB.Mob;
using SagaDB.Npc;
using SagaDB.ODWar;
using SagaDB.Partner;
using SagaDB.Quests;
using SagaDB.Ring;
using SagaDB.Skill;
using SagaDB.Synthese;
using SagaDB.Tamaire;
using SagaDB.Theater;
using SagaDB.Title;
using SagaDB.Treasure;
using SagaLib;
using SagaLib.Properties;
using SagaLib.Tasks;
using SagaLib.VirtualFileSytem;
using SagaMap.Dungeon;
using SagaMap.FictitiousActors;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Network.LoginServer;
using SagaMap.Partner;
using SagaMap.Skill;
using SagaMap.Tasks.System;
using SqlSugar;
using AIThread = SagaMap.Mob.AIThread;

namespace SagaMap {
    public class MapServer {
        private static readonly Serilog.Core.Logger _logger = Logger.InitLogger<MapServer>();

        /// <summary>
        ///     The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;
        public static bool shutingdown;
        public static bool shouldRefreshStatistic = true;

        public static bool StartDatabase() {
            //     try {
            //         switch (Configuration.Configuration.Instance.DBType) {
            //             case 0:
            charDB = new MySqlActorDb();
            accountDB = new MySQLAccountDB( /*Configuration.Configuration.Instance.DBHost,
                Configuration.Configuration.Instance.DBPort,
                Configuration.Configuration.Instance.DBName, Configuration.Configuration.Instance.DBUser,
                Configuration.Configuration.Instance.DBPass*/);
            // accountDB.Connect();
            return true;
            //             default:
            //                 return false;
            //         }
            //     }
            //     catch (Exception exception) {
            //         Logger.ShowError(exception);
            //         return false;
            //     }
        }

        // public static void EnsureCharDB() {
        // }

        // public static void EnsureAccountDB() {
        //     var notConnected = false;
        //
        //     if (!accountDB.isConnected()) {
        //         Logger.GetLogger().Warning("LOST CONNECTION TO CHAR DB SERVER!", null);
        //         notConnected = true;
        //     }
        //
        //     while (notConnected) {
        //         Logger.GetLogger().Information("Trying to reconnect to char db server ..", null);
        //         accountDB.Connect();
        //         if (!accountDB.isConnected()) {
        //             Logger.ShowError("Failed.. Trying again in 10sec", null);
        //             Thread.Sleep(10000);
        //             notConnected = true;
        //         }
        //         else {
        //             Logger.GetLogger().Information("SUCCESSFULLY RE-CONNECTED to char db server...", null);
        //             Logger.GetLogger().Information("Clients can now connect again", null);
        //             notConnected = false;
        //         }
        //     }
        // }

        // [DllImport("User32.dll ", EntryPoint = "FindWindow")]

        // [DllImport("user32.dll ", EntryPoint = "GetSystemMenu")]

        // [DllImport("user32.dll ", EntryPoint = "RemoveMenu")]

        private static void Main(string[] args) {
            var time = DateTime.Now;
            // var fullPath = Environment.CurrentDirectory + "\\SagaMap.exe";
            // var WINDOW_HANDLER = FindWindow(null, fullPath);
            // var CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            // var SC_CLOSE = 0xF060;
            // RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);
            Console.CancelKeyPress += ShutingDown;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            _logger.Information("                         SagaECO Map Server                ");
            _logger.Information("         (C)2013-2017 The Pluto ECO Project Development Team                ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("======================================================================");
            //Console.ResetColor();
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.GetLogger().Information("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("SagaMap");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Information(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("SagaLib");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Information(":SVN Rev." + GlobalInfo.Version + "(" +
                                GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("SagaDB");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Information(":SVN Rev." + GlobalInfo.Version + "(" +
                                GlobalInfo.ModifyDate + ")");

            Logger.GetLogger().Information(LocalManager.Instance.Strings.INITIALIZATION, null);

            Configuration.Configuration.Instance.Initialization(
                $"{ConfigLoader.LoadConfigPath()}/SagaMap.xml");

            //Console.ForegroundColor = ConsoleColor.Green;
            _logger.Information("[Info]");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("Current Packet Version:[");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Information(Configuration.Configuration.Instance.Version.ToString());
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("]");

            // LocalManager.Instance.CurrentLanguage =
            //     (LocalManager.Languages)Enum.Parse(typeof(LocalManager.Languages),
            //         Configuration.Configuration.Instance.Language);

            //Console.ForegroundColor = ConsoleColor.Green;
            _logger.Information("[Info]");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("Current Language:[");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Information(LocalManager.Instance.ToString());
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Information("]");
            //Console.ResetColor();

            //int item = (int)ContainerType.HEAD_ACCE2;

            //null.LogLevel = (Logger.LogContent)Configuration.Configuration.Instance.LogLevel;

            Logger.GetLogger().Information("Initializing VirtualFileSystem...");
// #if FreeVersion1
            // VirtualFileSystemManager.Instance.Init(FileSystems.LPK, $"{ConfigLoader.LoadDbPath()}/DB.lpk");
// #else
            VirtualFileSystemManager.Instance.Init(FileSystems.Real, ".");
// #endif
            ItemAdditionFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile(ConfigLoader.LoadDbPath(), "Addition*.csv"),
                Encoding.UTF8);
            ItemFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile(ConfigLoader.LoadDbPath(), "item*.csv"),
                Encoding.UTF8);
            ItemReleaseFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/equipment_release.csv",
                Encoding.UTF8);
            FurnitureFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/furniture.csv",
                Encoding.UTF8);
            HairFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/hair_info.csv",
                Encoding.UTF8);
            FaceFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/face_info.csv",
                Encoding.UTF8);
            ItemExchangeListFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/exchange_list.csv",
                Encoding.UTF8);
            ExchangeFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/exchange.csv",
                Encoding.UTF8);
            PacketManager.Instance.LoadPacketFiles(ConfigLoader.LoadPacketPath());

            EnhanceTableFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/enhancetable.csv",
                Encoding.UTF8);
            MasterEnhanceMaterialFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/MasterEnhanceMaterial.csv",
                Encoding.UTF8);

            IrisAbilityFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/iris_ability_vector_info.csv",
                Encoding.UTF8);
            IrisCardFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/iris_card.csv",
                Encoding.UTF8);
            IrisGachaFactory.Instance.InitBlack($"{ConfigLoader.LoadDbPath()}/iris_gacha_blank.csv",
                Encoding.UTF8);
            IrisGachaFactory.Instance.InitWindow($"{ConfigLoader.LoadDbPath()}/iris_gacha_window.csv",
                Encoding.UTF8);
            IrisDrawRateFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/IrisDrawRate.csv",
                Encoding.UTF8);

            ModelFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/demic_chip_model.csv",
                Encoding.UTF8);
            ChipFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/demic_chip.csv",
                Encoding.UTF8);

            SyntheseFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/synthe1.csv",
                Encoding.UTF8);
            TreasureFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile($"{ConfigLoader.LoadDbPath()}/Treasure",
                    "*.xml",
                    SearchOption.AllDirectories), null);
            FishFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/FishList.xml",
                Encoding.UTF8);
            DropGroupFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/monsterdrop.csv",
                Encoding.UTF8);
            MobFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/monster.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPet($"{ConfigLoader.LoadDbPath()}/pet.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPartner($"{ConfigLoader.LoadDbPath()}/partner_info.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPetLimit($"{ConfigLoader.LoadDbPath()}/pet_limit.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerDB($"{ConfigLoader.LoadDbPath()}/partner_info.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerRankDB($"{ConfigLoader.LoadDbPath()}/partner_base_rank.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerFoodDB($"{ConfigLoader.LoadDbPath()}/partner_food.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerEquipDB($"{ConfigLoader.LoadDbPath()}/partner_equip.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerTalksInfo($"{ConfigLoader.LoadDbPath()}/partner_talks_db.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerMotions($"{ConfigLoader.LoadDbPath()}/partner_motion_together.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitActCubeDB($"{ConfigLoader.LoadDbPath()}/partner_actcube.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerPicts($"{ConfigLoader.LoadDbPath()}/monsterpict.csv",
                Encoding.UTF8);


            MarionetteFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/marionette.csv",
                Encoding.UTF8);

            SkillFactory.Instance.InitSSP($"{ConfigLoader.LoadDbPath()}/effect.ssp", Encoding.Unicode);
            SkillFactory.Instance.LoadSkillList($"{ConfigLoader.LoadDbPath()}/SkillList.xml");
            //SkillFactory.Instance.LoadSkillList2("DB/SkillDB");

            RingFameTable.Instance.Init($"{ConfigLoader.LoadDbPath()}/RingFame.xml",
                Encoding.UTF8);

            QuestFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/Quests", null, true);

            NPCFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/npc.csv",
                Encoding.UTF8);
            ShopFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/ShopDB.xml",
                Encoding.UTF8);
            ECOShopFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/ECOShop.xml",
                Encoding.UTF8);
            ChipShopFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/ChipShop.xml",
                Encoding.UTF8);
            NCShopFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/NCShop.xml",
                Encoding.UTF8);
            GShopFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/GShop.xml",
                Encoding.UTF8);
            KujiListFactory.Instance.InitXML($"{ConfigLoader.LoadDbPath()}/KujiList.xml",
                Encoding.UTF8);
            KujiListFactory.Instance.BuildNotInKujiItemsList();
            //KujiListFactory.Instance.InitEventKujiList("DB/EventKujiList.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));
            KujiListFactory.Instance.InitTransformList($"{ConfigLoader.LoadDbPath()}/item_transform.csv",
                Encoding.UTF8);

            //加载副职信息
            DualJobInfoFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/dualjob_info.csv",
                Encoding.UTF8);
            DualJobSkillFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/dualjob_skill_learn.csv",
                Encoding.UTF8);

            MapInfoFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/MapInfo.zip");
            MapNameFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/mapname.csv",
                Encoding.UTF8);
            MapInfoFactory.Instance.LoadMapFish($"{ConfigLoader.LoadDbPath()}/CanFish.xml");
            MapInfoFactory.Instance.LoadFlags($"{ConfigLoader.LoadDbPath()}/MapFlags.xml");
            MapInfoFactory.Instance.LoadGatherInterval($"{ConfigLoader.LoadDbPath()}/pick_interval.csv",
                Encoding.UTF8);
            MapInfoFactory.Instance.LoadMapObjects($"{ConfigLoader.LoadDbPath()}/MapObjects.dat");
            MapInfoFactory.Instance.ApplyMapObject();
            MapInfoFactory.Instance.MapObjects.Clear();

            MapManager.Instance.MapInfos = MapInfoFactory.Instance.MapInfo;
            MapManager.Instance.LoadMaps();

            DungeonMapsFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/Dungeon/DungeonMaps.xml",
                Encoding.UTF8);
            DungeonFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/Dungeon/Dungeons.xml",
                Encoding.UTF8);

            MobAIFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/MobAI.xml",
                Encoding.UTF8);
            MobAIFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile($"{ConfigLoader.LoadDbPath()}/TTMobAI", "*.xml",
                    SearchOption.AllDirectories),
                Encoding.UTF8);
            MobAIFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile($"{ConfigLoader.LoadDbPath()}/AnMobAI", "*.xml",
                    SearchOption.AllDirectories),
                Encoding.UTF8);
            // PartnerAIFactory.Instance.Init(
            //     VirtualFileSystemManager.Instance.FileSystem.SearchFile($"{ConfigLoader.LoadDbPath()}/PartnerAI",
            //         "*.xml",
            //         SearchOption.AllDirectories),
            // Encoding.UTF8);
            //MobSpawnManager.Instance.LoadAnAI("DB/AnMobAI");
            MobSpawnManager.Instance.LoadSpawn($"{ConfigLoader.LoadDbPath()}/Spawns");
            FictitiousActorsFactory.Instance.LoadActorsList($"{ConfigLoader.LoadDbPath()}/Actors");
            //SagaDB.FictitiousActors.FictitiousActorsFactory.Instance.LoadShopLists("DB/GolemShop");
            FictitiousActorsManager.Instance.regionFictitiousActors();
            TheaterFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/TheaterSchedule.xml",
                Encoding.UTF8);
            ODWarFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/ODWar.xml",
                Encoding.UTF8);
            Theater.Instance.Activate();
            //Tasks.System.AutoRunSystemScript runscript = new Tasks.System.AutoRunSystemScript(3235125);
            //runscript.Activate();

            AnotherFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/another_page.csv",
                Encoding.UTF8);
            //意义不明暂时关闭
            //PlayerTitleFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/playertitle.csv", System.Text.Encoding.GetEncoding(Configuration.Instance.DBEncoding));
            //KujiListFactory.Instance.InitZeroCPList("DB/CP0List.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            //title db
            TitleFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/title_info.csv",
                Encoding.UTF8);

            //Experience Reward table for tamaire
            TamaireExpRewardFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/tamairereward.csv",
                Encoding.UTF8);
            //Status table for tamaire
            TamaireStatusFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/soul_status_param.csv",
                Encoding.UTF8);

            //称号的奖励暂时被过滤掉了
            //TitleFactory.Instance.InitB("DB/title_Bounds.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            // SkillHandler.Instance.LoadSkill("./Skills");
            // SkillHandler.Instance.Init();

            Global.clientMananger = MapClientManager.Instance;

            //目前无用
            //DefWarFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/odwar_order_info.csv", System.Text.Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            AtCommand.Instance.LoadCommandLevelSetting($"{ConfigLoader.LoadConfigPath()}/GMCommand.csv");

            // var login = new LoginSession(); //Make connection to the Login server.
            //
            // while (login.state != LoginSession.SESSION_STATE.IDENTIFIED &&
            //        login.state != LoginSession.SESSION_STATE.REJECTED)
            //     Thread.Sleep(1000);
            //
            // if (login.state == LoginSession.SESSION_STATE.REJECTED) {
            //     Logger.ShowError("Shutting down in 20sec.", null);
            //     MapClientManager.Instance.Abort();
            //     Thread.Sleep(20000);
            //     return;
            // }

            StartDatabase();
            // if (!StartDatabase()) {
            //     Logger.ShowError("cannot connect to dbserver", null);
            //     Logger.ShowError("Shutting down in 20sec.", null);
            //     MapClientManager.Instance.Abort();
            //     Thread.Sleep(20000);
            //     return;
            // }

            if (Configuration.Configuration.Instance.SQLLog)
                Logger.defaultSql = charDB as MySqlActorDb;

            ScriptManager.Instance.LoadScript($"{ConfigLoader.LoadScriptPath()}/Scripts");
            ScriptManager.Instance.VariableHolder = charDB.LoadServerVar();

            //Starting API
            var pre = Configuration.Configuration.Instance.Prefixes + ":" +
                      Configuration.Configuration.Instance.APIPort + "/";
            var ws = new WebServer.WebServer(SendResponse, pre);
            ws.Run();
            Logger.GetLogger().Information("Accepting API Clients from: " + pre);


            MapClientManager.Instance.Start();
            if (!MapClientManager.Instance.StartNetwork(Configuration.Configuration.Instance.Port)) {
                Logger.ShowError("cannot listen on port: " + Configuration.Configuration.Instance.Port);
                Logger.GetLogger().Information("Shutting down in 20sec.");
                MapClientManager.Instance.Abort();
                Thread.Sleep(20000);
                return;
            }

            if (Logger.defaultSql != null) {
                try {
                    SqlSugarHelper.Db.BeginTran();
                    SqlSugarHelper.Db.Queryable<SagaDB.Entities.Character>()
                        .ForEach(item => {
                            SagaDB.Entities.Character onlinePlayer = SqlSugarHelper.Db
                                .Queryable<SagaDB.Entities.Character>().TranLock(DbLockType.Wait)
                                .Where(onlinePlayer => onlinePlayer.CharacterId == item.CharacterId).First();

                            onlinePlayer.Online = false;

                            SqlSugarHelper.Db.Updateable(onlinePlayer);
                        });

                    SqlSugarHelper.Db.CommitTran();
                }
                catch (Exception e) {
                    SqlSugarHelper.Db.RollbackTran();
                    SagaLib.Logger.ShowError(e);
                }


                Logger.GetLogger().Information("Clearing SQL Logs");
                Logger.ShowInfo(string.Format("DELETE FROM `log` WHERE `eventTime` < '{0}';",
                    Logger.defaultSql.ToSqlDateTime(DateTime.Now - new TimeSpan(15, 0, 0, 0))));
            }

            //激活AJIMODE線程
            //SagaMap.Tasks.System.AJImode aji = new Tasks.System.AJImode();
            //aji.Activate();
            //激活自動保存系統變量線程
            var asss = new AutoSaveServerSvar();
            asss.Activate();

            //关攻防
            //Tasks.System.ODWar.Instance.Activate();

            Configuration.Configuration.Instance.InitAnnounce($"{ConfigLoader.LoadDbPath()}/Announce.csv");
            //OD War related

            //蓝莓活动，记得关！！
            //Tasks.System.BlueBerryActivity.Instance.Activate();

            //Tasks.System.南牢列车.Instance.Activate();


            foreach (var i in ODWarFactory.Instance.Items.Values) {
                //ODWarManager.Instance.StartODWar(i.MapID);
            }
            //SagaMap.LevelLimit.LevelLimitManager.Instance.LoadLevelLimit();


            //Experience table
            //SagaMap.Manager.ExperienceManager.Instance.LoadTable("DB/exp.xml");
            PCExperienceFactory.Instance.Init($"{ConfigLoader.LoadDbPath()}/EXP.csv",
                Encoding.UTF8);

            //CustomMapManager.Instance.CreateFF();
            //MapManager.Instance.CreateFFInstanceOfSer();

            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var mapId in Configuration.Configuration.Instance.HostedMaps) {
                    SqlSugarHelper.Db.Storageable(new SagaDB.Entities.Server {
                        ServerIp = Dns.GetHostAddresses(Configuration.Configuration.Instance.Host)[0]
                            .ToString(),
                        Port = Configuration.Configuration.Instance.Port,
                        Type = mapId.ToString()
                    }).ExecuteCommand();
                }

                foreach (var mapId in MapInfoFactory.Instance.MapInfo.Keys) {
                    SqlSugarHelper.Db.Storageable(new SagaDB.Entities.Server {
                        ServerIp = Dns.GetHostAddresses(Configuration.Configuration.Instance.Host)[0]
                            .ToString(),
                        Port = Configuration.Configuration.Instance.Port,
                        Type = mapId.ToString()
                    }).ExecuteCommand();
                }

                SqlSugarHelper.Db.Storageable<SagaDB.Entities.Server>(new SagaDB.Entities.Server {
                    ServerIp = Dns.GetHostAddresses(Configuration.Configuration.Instance.Host)[0].ToString(),
                    Port = Configuration.Configuration.Instance.Port,
                    Type = "PORTAL"
                }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                throw;
            }


            Logger.ProgressBarHide("加载总耗时：" + (DateTime.Now - time).TotalMilliseconds + "ms");
            Logger.GetLogger().Information(LocalManager.Instance.Strings.ACCEPTING_CLIENT);

            var console = new Thread(ConsoleThread);
            console.Start();

            new Thread(ThreadShutdownWatcher).Start();

            while (true)
                try {
                    if (shouldRefreshStatistic && Configuration.Configuration.Instance.OnlineStatistics) {
                        try {
                            string content;
                            var sr = new StreamReader($"{ConfigLoader.LoadConfigPath()}/OnlineStatisticTemplate.htm",
                                true);
                            content = sr.ReadToEnd();
                            sr.Close();
                            var header = content.Substring(0, content.IndexOf("<template for one row>"));
                            content = content.Substring(content.IndexOf("<template for one row>") +
                                                        "<template for one row>".Length);
                            var footer = content.Substring(content.IndexOf("</template for one row>") +
                                                           "</template for one row>".Length);
                            content = content.Substring(0, content.IndexOf("</template for one row>"));
                            var res = "";
                            foreach (var i in MapClientManager.Instance.OnlinePlayer)
                                try {
                                    var tmp = content;
                                    tmp = tmp.Replace("{CharName}", i.Character.Name);
                                    tmp = tmp.Replace("{Job}", i.Character.Job.ToString());
                                    tmp = tmp.Replace("{BaseLv}", i.Character.Level.ToString());
                                    if (i.Character.Job == i.Character.JobBasic)
                                        tmp = tmp.Replace("{JobLv}", i.Character.JobLevel1.ToString());
                                    else if (i.Character.Job == i.Character.Job2X)
                                        tmp = tmp.Replace("{JobLv}", i.Character.JobLevel2X.ToString());
                                    else if (i.Character.Job == i.Character.Job2T)
                                        tmp = tmp.Replace("{JobLv}", i.Character.JobLevel2T.ToString());
                                    else
                                        tmp = tmp.Replace("{JobLv}", i.Character.JobLevel3.ToString());
                                    tmp = tmp.Replace("{Map}", i.map.Info.name);
                                    res += tmp;
                                }
                                catch {
                                }

                            var sw = new StreamWriter(Configuration.Configuration.Instance.StatisticsPagePath, false,
                                Global.Unicode);
                            sw.Write(header);
                            sw.Write(res);
                            sw.Write(footer);
                            sw.Flush();
                            sw.Close();
                        }
                        catch (Exception ex) {
                            Logger.ShowError(ex);
                        }

                        shouldRefreshStatistic = false;
                    }

                    // keep the connections to the database servers alive
                    // let new clients (max 10) connect
#if FreeVersion
                if (MapClientManager.Instance.OnlinePlayer.Count < int.Parse("15"))
#endif
                    if (!shutingdown)
                        MapClientManager.Instance.NetworkLoop(10);
                    Thread.Sleep(1);
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e) {
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e) {
            throw new NotImplementedException();
        }

        private static void ProcessShutdown() {
            Logger.GetLogger().Information("Closing.....", null);
            shutingdown = true;
            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
            var clients = new MapClient[MapClientManager.Instance.Clients.Count];
            MapClientManager.Instance.Clients.CopyTo(clients);
            Logger.GetLogger().Information("Saving player's data.....", null);

            foreach (var i in clients)
                try {
                    if (i.Character == null) continue;
                    i.NetIo.Disconnect();
                }
                catch (Exception exception) {
                    Logger.ShowError(exception);
                }

            Logger.GetLogger().Information("Saving golem's data.....", null);

            foreach (var i in MapManager.Instance.Maps.Values) {
                foreach (var j in i.Actors.Values)
                    if (j.type == ActorType.GOLEM)
                        try {
                            var golem = (ActorGolem)j;
                            charDB.SaveChar(golem.Owner, false);
                        }
                        catch {
                        }

                if (i.IsMapInstance)
                    i.OnDestrory();
            }

            Environment.Exit(Environment.ExitCode);
        }

        private static void ThreadShutdownWatcher() {
            while (!ConfigLoader.ShouldShutdown()) {
                Thread.Sleep(1000);
            }

            ShutingDown(null, null);
        }

        private static void ConsoleThread() {
            while (true) {
                try {
                    var cmd = Console.ReadLine();
                    var args = cmd.Split(' ');
                    switch (args[0].ToLower()) {
                        case "printthreads":
                            ClientManager.PrintAllThreads();
                            break;
                        case "printtaskinfo":
                            Logger.GetLogger().Warning("Active AI count:" + AIThread.Instance.ActiveAI);
                            var tasks = TaskManager.Instance.RegisteredTasks;
                            Logger.GetLogger().Warning("Active Tasks:" + tasks.Count);
                            foreach (var i in tasks) Logger.GetLogger().Warning(i);
                            break;
                        case "printband":
                            var sendTotal = 0;
                            var receiveTotal = 0;
                            Logger.GetLogger().Warning("Bandwidth usage information:");
                            try {
                                foreach (var i in MapClientManager.Instance.OnlinePlayer) {
                                    sendTotal += i.NetIo.UpStreamBand;
                                    receiveTotal += i.NetIo.DownStreamBand;
                                    Logger.GetLogger().Warning(string.Format(
                                        "Client:{0} Receive:{1:0.##}KB/s Send:{2:0.##}KB/s",
                                        i,
                                        (float)i.NetIo.DownStreamBand / 1024,
                                        (float)i.NetIo.UpStreamBand / 1024));
                                }
                            }
                            catch {
                            }

                            Logger.GetLogger().Warning(string.Format("Total: Receive:{0:0.##}KB/s Send:{1:0.##}KB/s",
                                (float)receiveTotal / 1024,
                                (float)sendTotal / 1024));
                            break;
                        case "announce":
                            if (args.Length > 1) {
                                var tmsg = new StringBuilder(args[1]);
                                for (var i = 2; i < args.Length; i++) tmsg.Append(" " + args[i]);
                                var msg = tmsg.ToString();
                                try {
                                    foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendAnnounce(msg);
                                }
                                catch (Exception exception) {
                                    Logger.ShowError(exception);
                                }
                            }

                            break;
                        case "kick":
                            if (args.Length > 1)
                                try {
                                    MapClient client;
                                    var chr =
                                        from c in MapClientManager.Instance.OnlinePlayer
                                        where c.Character.Name == args[1]
                                        select c;
                                    client = chr.First();
                                    client.NetIo.Disconnect();
                                }
                                catch (Exception exception) {
                                    Logger.ShowError(exception);
                                }

                            break;
                        case "savevar":
                            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
                            Logger.GetLogger().Information("Saving ....", null);
                            break;
                        case "quit":
                            ProcessShutdown();
                            break;
                        case "who":
                            foreach (var i in MapClientManager.Instance.OnlinePlayer) {
                                byte x, y;

                                x = Global.PosX16to8(i.Character.X, i.map.Width);
                                y = Global.PosY16to8(i.Character.Y, i.map.Height);
                                Logger.GetLogger().Information(i.Character.Name + "(CharID:" + i.Character.CharID +
                                                               ")[" + i.Map.Name +
                                                               " " + x + "," + y + "] IP：" +
                                                               i.Character.Account.LastIP);
                            }

                            Logger.GetLogger().Information(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                                           MapClientManager.Instance.OnlinePlayer.Count);
                            Logger.GetLogger()
                                .Information("当前IP在线：" + MapClientManager.Instance.OnlinePlayerOnlyIP.Count);
                            break;
                        case "kick2":
                            if (args.Length > 1)
                                try {
                                    MapClient client;
                                    var chr =
                                        from c in MapClientManager.Instance.OnlinePlayer
                                        where c.Character.CharID == uint.Parse(args[1])
                                        select c;
                                    if (chr.Count() > 0) {
                                        client = chr.First();
                                        client.NetIo.Disconnect();
                                    }
                                }
                                catch (Exception exception) {
                                    Logger.ShowError(exception);
                                }

                            break;
                    }
                }
                catch {
                }
            }
        }

        private static void ShutingDown(object sender, ConsoleCancelEventArgs args) {
            Logger.GetLogger().Information("Closing.....", null);
            shutingdown = true;
            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
            var clients = new MapClient[MapClientManager.Instance.Clients.Count];
            MapClientManager.Instance.Clients.CopyTo(clients);
            Logger.GetLogger().Information("Saving golem's data.....", null);

            var maps = MapManager.Instance.Maps.Values.ToArray();
            foreach (var i in maps) {
                var actors = i.Actors.Values.ToArray();
                foreach (var j in actors)
                    if (j == null)
                        continue;
                /*if (j.type == ActorType.GOLEM)取消石像
                    {
                        try
                        {
                            ActorGolem golem = (ActorGolem)j;
                            charDB.SaveChar(golem.Owner, true, false);
                        }
                        catch (Exception ex) { Logger.getLogger().Error(ex, ex.Message); }
                    }*/
                if (i.IsMapInstance)
                    try {
                        i.OnDestrory();
                    }
                    catch {
                    }
            }

            Logger.GetLogger().Information("Saving player's data.....", null);

            foreach (var i in clients)
                try {
                    if (i.Character == null) continue;
                    i.NetIo.Disconnect();
                }
                catch (Exception exception) {
                    Logger.ShowError(exception);
                }


            // Logger.GetLogger().Information("Closing MySQL connection....");
            // if (charDB.GetType() == typeof(MySQLConnectivity)) {
            //     var con = (MySQLConnectivity)charDB;
            //     while (!con.CanClose)
            //         Thread.Sleep(100);
            // }
            //
            // if (accountDB.GetType() == typeof(MySQLConnectivity)) {
            //     var con = (MySQLConnectivity)accountDB;
            //     while (!con.CanClose)
            //         Thread.Sleep(100);
            // }
        }

        public static string SendResponse(HttpListenerRequest request) {
            return null;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            shutingdown = true;
            Logger.ShowError("Fatal: An unhandled exception is thrown, terminating...");
            Logger.ShowError("Error Message:" + ex);
            Logger.ShowError("Call Stack:" + ex.StackTrace);
            Logger.ShowError("Trying to save all player's data");
            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);

            var clients = new MapClient[MapClientManager.Instance.Clients.Count];
            MapClientManager.Instance.Clients.CopyTo(clients);
            foreach (var i in clients)
                try {
                    if (i.Character == null) continue;
                    i.NetIo.Disconnect();
                }
                catch (Exception exception) {
                    Logger.ShowError(exception);
                }

            Logger.ShowError("Trying to clear golem actor");

            var maps = MapManager.Instance.Maps.Values.ToArray();
            foreach (var i in maps) {
                foreach (var j in i.Actors.Values)
                    if (j.type == ActorType.GOLEM)
                        try {
                            var golem = (ActorGolem)j;
                            charDB.SaveChar(golem.Owner, true, false);
                        }
                        catch {
                        }

                if (i.IsMapInstance)
                    i.OnDestrory();
            }

            // Logger.GetLogger().Information("Closing MySQL connection....");
            // if (charDB.GetType() == typeof(MySQLConnectivity)) {
            //     var con = (MySQLConnectivity)charDB;
            //     while (!con.CanClose)
            //         Thread.Sleep(100);
            // }
            //
            // if (accountDB.GetType() == typeof(MySQLConnectivity)) {
            //     var con = (MySQLConnectivity)accountDB;
            //     while (!con.CanClose)
            //         Thread.Sleep(100);
            // }
        }
    }
}