//#define FreeVersion

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
using SagaLib.Properties;
using SagaMap.Skill;
using SagaMap.Tasks.System;
using AIThread = SagaMap.Mob.AIThread;

namespace SagaMap
{
    public class MapServer
    {
        private static readonly NLog.Logger _logger = Logger.InitLogger<MapServer>();

        /// <summary>
        ///     The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;
        public static bool shutingdown;
        public static bool shouldRefreshStatistic = true;

        public static bool StartDatabase()
        {
            try
            {
                switch (Configuration.Configuration.Instance.DBType)
                {
                    case 0:
                        charDB = new MySQLActorDB(Configuration.Configuration.Instance.DBHost,
                            Configuration.Configuration.Instance.DBPort,
                            Configuration.Configuration.Instance.DBName, Configuration.Configuration.Instance.DBUser,
                            Configuration.Configuration.Instance.DBPass);
                        accountDB = new MySQLAccountDB(Configuration.Configuration.Instance.DBHost,
                            Configuration.Configuration.Instance.DBPort,
                            Configuration.Configuration.Instance.DBName, Configuration.Configuration.Instance.DBUser,
                            Configuration.Configuration.Instance.DBPass);
                        charDB.Connect();
                        accountDB.Connect();
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception exception)
            {
                Logger.ShowError(exception, null);
                return false;
            }
        }

        public static void EnsureCharDB()
        {
            var notConnected = false;

            if (!charDB.isConnected())
            {
                Logger.ShowWarning("LOST CONNECTION TO CHAR DB SERVER!", null);
                notConnected = true;
            }

            while (notConnected)
            {
                Logger.ShowInfo("Trying to reconnect to char db server ..", null);
                charDB.Connect();
                if (!charDB.isConnected())
                {
                    Logger.ShowError("Failed.. Trying again in 10sec", null);
                    Thread.Sleep(10000);
                    notConnected = true;
                }
                else
                {
                    Logger.ShowInfo("SUCCESSFULLY RE-CONNECTED to char db server...", null);
                    Logger.ShowInfo("Clients can now connect again", null);
                    notConnected = false;
                }
            }
        }

        public static void EnsureAccountDB()
        {
            var notConnected = false;

            if (!accountDB.isConnected())
            {
                Logger.ShowWarning("LOST CONNECTION TO CHAR DB SERVER!", null);
                notConnected = true;
            }

            while (notConnected)
            {
                Logger.ShowInfo("Trying to reconnect to char db server ..", null);
                accountDB.Connect();
                if (!accountDB.isConnected())
                {
                    Logger.ShowError("Failed.. Trying again in 10sec", null);
                    Thread.Sleep(10000);
                    notConnected = true;
                }
                else
                {
                    Logger.ShowInfo("SUCCESSFULLY RE-CONNECTED to char db server...", null);
                    Logger.ShowInfo("Clients can now connect again", null);
                    notConnected = false;
                }
            }
        }

        // [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        // [DllImport("user32.dll ", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        // [DllImport("user32.dll ", EntryPoint = "RemoveMenu")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPos, int flags);

        private static void Main(string[] args)
        {
            var time = DateTime.Now;
            // var fullPath = Environment.CurrentDirectory + "\\SagaMap.exe";
            // var WINDOW_HANDLER = FindWindow(null, fullPath);
            // var CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            // var SC_CLOSE = 0xF060;
            // RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);
            Console.CancelKeyPress += ShutingDown;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var Log = new Logger("SagaMap.log");
            Logger.defaultlogger = Log;
            Logger.CurrentLogger = Log;
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            _logger.Debug("                         SagaECO Map Server                ");
            _logger.Debug("         (C)2013-2017 The Pluto ECO Project Development Team                ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("======================================================================");
            //Console.ResetColor();
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("SagaMap");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("SagaLib");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(":SVN Rev." + GlobalInfo.Version + "(" +
                          GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("SagaDB");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(":SVN Rev." + GlobalInfo.Version + "(" +
                          GlobalInfo.ModifyDate + ")");

            Logger.ShowInfo(LocalManager.Instance.Strings.INITIALIZATION, null);

            Configuration.Configuration.Instance.Initialization("./Config/SagaMap.xml");

            //Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug("[Info]");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("Current Packet Version:[");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(Configuration.Configuration.Instance.Version.ToString());
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("]");

            // LocalManager.Instance.CurrentLanguage =
            //     (LocalManager.Languages)Enum.Parse(typeof(LocalManager.Languages),
            //         Configuration.Configuration.Instance.Language);

            //Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug("[Info]");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("Current Language:[");
            //Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(LocalManager.Instance.ToString());
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("]");
            //Console.ResetColor();

            //int item = (int)ContainerType.HEAD_ACCE2;

            Logger.CurrentLogger.LogLevel = (Logger.LogContent)Configuration.Configuration.Instance.LogLevel;

            Logger.ShowInfo("Initializing VirtualFileSystem...");
#if FreeVersion1
            VirtualFileSystemManager.Instance.Init(FileSystems.LPK, "./DB/DB.lpk");
#else
            VirtualFileSystemManager.Instance.Init(FileSystems.Real, ".");
#endif
            ItemAdditionFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/", "Addition*.csv",
                    SearchOption.TopDirectoryOnly),
                Encoding.UTF8);
            ItemFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/", "item*.csv",
                    SearchOption.TopDirectoryOnly),
                Encoding.UTF8);
            ItemReleaseFactory.Instance.Init("DB/equipment_release.csv",
                Encoding.UTF8);
            FurnitureFactory.Instance.Init("DB/furniture.csv",
                Encoding.UTF8);
            HairFactory.Instance.Init("DB/hair_info.csv",
                Encoding.UTF8);
            FaceFactory.Instance.Init("DB/face_info.csv",
                Encoding.UTF8);
            ItemExchangeListFactory.Instance.Init("DB/exchange_list.csv",
                Encoding.UTF8);
            ExchangeFactory.Instance.Init("DB/exchange.csv",
                Encoding.UTF8);
            PacketManager.Instance.LoadPacketFiles("./Packers");

            EnhanceTableFactory.Instance.Init("DB/enhancetable.csv",
                Encoding.UTF8);
            MasterEnhanceMaterialFactory.Instance.Init("DB/MasterEnhanceMaterial.csv",
                Encoding.UTF8);

            IrisAbilityFactory.Instance.Init("DB/iris_ability_vector_info.csv",
                Encoding.UTF8);
            IrisCardFactory.Instance.Init("DB/iris_card.csv",
                Encoding.UTF8);
            IrisGachaFactory.Instance.InitBlack("DB/iris_gacha_blank.csv",
                Encoding.UTF8);
            IrisGachaFactory.Instance.InitWindow("DB/iris_gacha_window.csv",
                Encoding.UTF8);
            IrisDrawRateFactory.Instance.Init("DB/IrisDrawRate.csv",
                Encoding.UTF8);

            ModelFactory.Instance.Init("DB/demic_chip_model.csv",
                Encoding.UTF8);
            ChipFactory.Instance.Init("DB/demic_chip.csv",
                Encoding.UTF8);

            SyntheseFactory.Instance.Init("DB/synthe1.csv",
                Encoding.UTF8);
            TreasureFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/Treasure", "*.xml",
                    SearchOption.AllDirectories), null);
            FishFactory.Instance.Init("DB/FishList.xml",
                Encoding.UTF8);
            DropGroupFactory.Instance.Init("DB/monsterdrop.csv",
                Encoding.UTF8);
            MobFactory.Instance.Init("DB/monster.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPet("DB/pet.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPartner("DB/partner_info.csv",
                Encoding.UTF8);
            MobFactory.Instance.InitPetLimit("DB/pet_limit.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerDB("DB/partner_info.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerRankDB("DB/partner_base_rank.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerFoodDB("DB/partner_food.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerEquipDB("DB/partner_equip.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerTalksInfo("DB/partner_talks_db.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerMotions("DB/partner_motion_together.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitActCubeDB("DB/partner_actcube.csv",
                Encoding.UTF8);
            PartnerFactory.Instance.InitPartnerPicts("DB/monsterpict.csv",
                Encoding.UTF8);


            MarionetteFactory.Instance.Init("DB/marionette.csv",
                Encoding.UTF8);

            SkillFactory.Instance.InitSSP("DB/effect.ssp", Encoding.Unicode);
            SkillFactory.Instance.LoadSkillList("DB/SkillList.xml");
            //SkillFactory.Instance.LoadSkillList2("DB/SkillDB");

            RingFameTable.Instance.Init("DB/RingFame.xml",
                Encoding.UTF8);

            QuestFactory.Instance.Init("DB/Quests", null, true);

            NPCFactory.Instance.Init("DB/npc.csv",
                Encoding.UTF8);
            ShopFactory.Instance.Init("DB/ShopDB.xml",
                Encoding.UTF8);
            ECOShopFactory.Instance.Init("DB/ECOShop.xml",
                Encoding.UTF8);
            ChipShopFactory.Instance.Init("DB/ChipShop.xml",
                Encoding.UTF8);
            NCShopFactory.Instance.Init("DB/NCShop.xml",
                Encoding.UTF8);
            GShopFactory.Instance.Init("DB/GShop.xml",
                Encoding.UTF8);
            KujiListFactory.Instance.InitXML("DB/KujiList.xml",
                Encoding.UTF8);
            KujiListFactory.Instance.BuildNotInKujiItemsList();
            //KujiListFactory.Instance.InitEventKujiList("DB/EventKujiList.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));
            KujiListFactory.Instance.InitTransformList("DB/item_transform.csv",
                Encoding.UTF8);

            //加载副职信息
            DualJobInfoFactory.Instance.Init("DB/dualjob_info.csv",
                Encoding.UTF8);
            DualJobSkillFactory.Instance.Init("DB/dualjob_skill_learn.csv",
                Encoding.UTF8);

            MapInfoFactory.Instance.Init("DB/MapInfo.zip");
            MapNameFactory.Instance.Init("DB/mapname.csv",
                Encoding.UTF8);
            MapInfoFactory.Instance.LoadMapFish("DB/CanFish.xml");
            MapInfoFactory.Instance.LoadFlags("DB/MapFlags.xml");
            MapInfoFactory.Instance.LoadGatherInterval("DB/pick_interval.csv",
                Encoding.UTF8);
            MapInfoFactory.Instance.LoadMapObjects("DB/MapObjects.dat");
            MapInfoFactory.Instance.ApplyMapObject();
            MapInfoFactory.Instance.MapObjects.Clear();

            MapManager.Instance.MapInfos = MapInfoFactory.Instance.MapInfo;
            MapManager.Instance.LoadMaps();

            DungeonMapsFactory.Instance.Init("DB/Dungeon/DungeonMaps.xml",
                Encoding.UTF8);
            DungeonFactory.Instance.Init("DB/Dungeon/Dungeons.xml",
                Encoding.UTF8);

            MobAIFactory.Instance.Init("DB/MobAI.xml",
                Encoding.UTF8);
            MobAIFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/TTMobAI", "*.xml",
                    SearchOption.AllDirectories),
                Encoding.UTF8);
            MobAIFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/AnMobAI", "*.xml",
                    SearchOption.AllDirectories),
                Encoding.UTF8);
            PartnerAIFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/PartnerAI", "*.xml",
                    SearchOption.AllDirectories),
                Encoding.UTF8);
            //MobSpawnManager.Instance.LoadAnAI("DB/AnMobAI");
            MobSpawnManager.Instance.LoadSpawn("DB/Spawns");
            FictitiousActorsFactory.Instance.LoadActorsList("DB/Actors");
            //SagaDB.FictitiousActors.FictitiousActorsFactory.Instance.LoadShopLists("DB/GolemShop");
            FictitiousActorsManager.Instance.regionFictitiousActors();
            TheaterFactory.Instance.Init("DB/TheaterSchedule.xml",
                Encoding.UTF8);
            ODWarFactory.Instance.Init("DB/ODWar.xml",
                Encoding.UTF8);
            Theater.Instance.Activate();
            //Tasks.System.AutoRunSystemScript runscript = new Tasks.System.AutoRunSystemScript(3235125);
            //runscript.Activate();

            AnotherFactory.Instance.Init("DB/another_page.csv",
                Encoding.UTF8);
            //意义不明暂时关闭
            //PlayerTitleFactory.Instance.Init("DB/playertitle.csv", System.Text.Encoding.GetEncoding(Configuration.Instance.DBEncoding));
            //KujiListFactory.Instance.InitZeroCPList("DB/CP0List.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            //title db
            TitleFactory.Instance.Init("DB/title_info.csv",
                Encoding.UTF8);

            //Experience Reward table for tamaire
            TamaireExpRewardFactory.Instance.Init("DB/tamairereward.csv",
                Encoding.UTF8);
            //Status table for tamaire
            TamaireStatusFactory.Instance.Init("DB/soul_status_param.csv",
                Encoding.UTF8);

            //称号的奖励暂时被过滤掉了
            //TitleFactory.Instance.InitB("DB/title_Bounds.csv", Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            SkillHandler.Instance.LoadSkill("./Skills");
            SkillHandler.Instance.Init();

            Global.clientMananger = MapClientManager.Instance;

            //目前无用
            //DefWarFactory.Instance.Init("DB/odwar_order_info.csv", System.Text.Encoding.GetEncoding(Configuration.Instance.DBEncoding));

            AtCommand.Instance.LoadCommandLevelSetting("./Config/GMCommand.csv");

            var login = new LoginSession(); //Make connection to the Login server.

            while (login.state != LoginSession.SESSION_STATE.IDENTIFIED &&
                   login.state != LoginSession.SESSION_STATE.REJECTED)
                Thread.Sleep(1000);

            if (login.state == LoginSession.SESSION_STATE.REJECTED)
            {
                Logger.ShowError("Shutting down in 20sec.", null);
                MapClientManager.Instance.Abort();
                Thread.Sleep(20000);
                return;
            }

            if (!StartDatabase())
            {
                Logger.ShowError("cannot connect to dbserver", null);
                Logger.ShowError("Shutting down in 20sec.", null);
                MapClientManager.Instance.Abort();
                Thread.Sleep(20000);
                return;
            }

            if (Configuration.Configuration.Instance.SQLLog)
                Logger.defaultSql = charDB as MySQLActorDB;

            ScriptManager.Instance.LoadScript("./Scripts");
            ScriptManager.Instance.VariableHolder = charDB.LoadServerVar();

            //Starting API
            var pre = Configuration.Configuration.Instance.Prefixes + ":" +
                      Configuration.Configuration.Instance.APIPort + "/";
            var ws = new WebServer.WebServer(SendResponse, pre);
            ws.Run();
            Logger.ShowInfo("Accepting API Clients from: " + pre);


            MapClientManager.Instance.Start();
            if (!MapClientManager.Instance.StartNetwork(Configuration.Configuration.Instance.Port))
            {
                Logger.ShowError("cannot listen on port: " + Configuration.Configuration.Instance.Port);
                Logger.ShowInfo("Shutting down in 20sec.");
                MapClientManager.Instance.Abort();
                Thread.Sleep(20000);
                return;
            }

            if (Logger.defaultSql != null)
            {
                Logger.defaultSql.SQLExecuteNonQuery("UPDATE `char` SET `online`=0;");
                Logger.ShowInfo("Clearing SQL Logs");
                Logger.defaultSql.SQLExecuteNonQuery(string.Format("DELETE FROM `log` WHERE `eventTime` < '{0}';",
                    Logger.defaultSql.ToSQLDateTime(DateTime.Now - new TimeSpan(15, 0, 0, 0))));
            }

            Logger.ProgressBarHide("加载总耗时：" + (DateTime.Now - time).TotalMilliseconds + "ms");
            Logger.ShowInfo(LocalManager.Instance.Strings.ACCEPTING_CLIENT);

            //激活AJIMODE線程
            //SagaMap.Tasks.System.AJImode aji = new Tasks.System.AJImode();
            //aji.Activate();
            //激活自動保存系統變量線程
            var asss = new AutoSaveServerSvar();
            asss.Activate();

            //关攻防
            //Tasks.System.ODWar.Instance.Activate();

            Configuration.Configuration.Instance.InitAnnounce("./DB/Announce.csv");
            //OD War related

            //蓝莓活动，记得关！！
            //Tasks.System.BlueBerryActivity.Instance.Activate();

            //Tasks.System.南牢列车.Instance.Activate();


            foreach (var i in ODWarFactory.Instance.Items.Values)
            {
                //ODWarManager.Instance.StartODWar(i.MapID);
            }
            //SagaMap.LevelLimit.LevelLimitManager.Instance.LoadLevelLimit();


            //Experience table
            //SagaMap.Manager.ExperienceManager.Instance.LoadTable("DB/exp.xml");
            PCExperienceFactory.Instance.Init("DB/EXP.csv",
                Encoding.UTF8);

            //CustomMapManager.Instance.CreateFF();
            //MapManager.Instance.CreateFFInstanceOfSer();


            var console = new Thread(ConsoleThread);
            console.Start();

            while (true)
                try
                {
                    if (shouldRefreshStatistic && Configuration.Configuration.Instance.OnlineStatistics)
                    {
                        try
                        {
                            string content;
                            var sr = new StreamReader("Config/OnlineStatisticTemplate.htm", true);
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
                                try
                                {
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
                                catch
                                {
                                }

                            var sw = new StreamWriter(Configuration.Configuration.Instance.StatisticsPagePath, false,
                                Global.Unicode);
                            sw.Write(header);
                            sw.Write(res);
                            sw.Write(footer);
                            sw.Flush();
                            sw.Close();
                        }
                        catch (Exception ex)
                        {
                            Logger.ShowError(ex);
                        }

                        shouldRefreshStatistic = false;
                    }

                    // keep the connections to the database servers alive
                    EnsureCharDB();
                    EnsureAccountDB();
                    // let new clients (max 10) connect
#if FreeVersion
                if (MapClientManager.Instance.OnlinePlayer.Count < int.Parse("15"))
#endif
                    if (!shutingdown)
                        MapClientManager.Instance.NetworkLoop(10);
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void ConsoleThread()
        {
            while (true)
                try
                {
                    var cmd = Console.ReadLine();
                    var args = cmd.Split(' ');
                    switch (args[0].ToLower())
                    {
                        case "printthreads":
                            ClientManager.PrintAllThreads();
                            break;
                        case "printtaskinfo":
                            Logger.ShowWarning("Active AI count:" + AIThread.Instance.ActiveAI);
                            var tasks = TaskManager.Instance.RegisteredTasks;
                            Logger.ShowWarning("Active Tasks:" + tasks.Count);
                            foreach (var i in tasks) Logger.ShowWarning(i);
                            break;
                        case "printband":
                            var sendTotal = 0;
                            var receiveTotal = 0;
                            Logger.ShowWarning("Bandwidth usage information:");
                            try
                            {
                                foreach (var i in MapClientManager.Instance.OnlinePlayer)
                                {
                                    sendTotal += i.NetIo.UpStreamBand;
                                    receiveTotal += i.NetIo.DownStreamBand;
                                    Logger.ShowWarning(string.Format(
                                        "Client:{0} Receive:{1:0.##}KB/s Send:{2:0.##}KB/s",
                                        i,
                                        (float)i.NetIo.DownStreamBand / 1024,
                                        (float)i.NetIo.UpStreamBand / 1024));
                                }
                            }
                            catch
                            {
                            }

                            Logger.ShowWarning(string.Format("Total: Receive:{0:0.##}KB/s Send:{1:0.##}KB/s",
                                (float)receiveTotal / 1024,
                                (float)sendTotal / 1024));
                            break;
                        case "announce":
                            if (args.Length > 1)
                            {
                                var tmsg = new StringBuilder(args[1]);
                                for (var i = 2; i < args.Length; i++) tmsg.Append(" " + args[i]);
                                var msg = tmsg.ToString();
                                try
                                {
                                    foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendAnnounce(msg);
                                }
                                catch (Exception exception)
                                {
                                    Logger.ShowError(exception, null);
                                }
                            }

                            break;
                        case "kick":
                            if (args.Length > 1)
                                try
                                {
                                    MapClient client;
                                    var chr =
                                        from c in MapClientManager.Instance.OnlinePlayer
                                        where c.Character.Name == args[1]
                                        select c;
                                    client = chr.First();
                                    client.NetIo.Disconnect();
                                }
                                catch (Exception exception)
                                {
                                    Logger.ShowError(exception, null);
                                }

                            break;
                        case "savevar":
                            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
                            Logger.ShowInfo("Saving ....", null);
                            break;
                        case "quit":
                            Logger.ShowInfo("Closing.....", null);
                            shutingdown = true;
                            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
                            var clients = new MapClient[MapClientManager.Instance.Clients.Count];
                            MapClientManager.Instance.Clients.CopyTo(clients);
                            Logger.ShowInfo("Saving player's data.....", null);

                            foreach (var i in clients)
                                try
                                {
                                    if (i.Character == null) continue;
                                    i.NetIo.Disconnect();
                                }
                                catch (Exception exception)
                                {
                                    Logger.ShowError(exception, null);
                                }

                            Logger.ShowInfo("Saving golem's data.....", null);

                            foreach (var i in MapManager.Instance.Maps.Values)
                            {
                                foreach (var j in i.Actors.Values)
                                    if (j.type == ActorType.GOLEM)
                                        try
                                        {
                                            var golem = (ActorGolem)j;
                                            charDB.SaveChar(golem.Owner, false);
                                        }
                                        catch
                                        {
                                        }

                                if (i.IsMapInstance)
                                    i.OnDestrory();
                            }

                            Environment.Exit(Environment.ExitCode);
                            break;
                        case "who":
                            foreach (var i in MapClientManager.Instance.OnlinePlayer)
                            {
                                byte x, y;

                                x = Global.PosX16to8(i.Character.X, i.map.Width);
                                y = Global.PosY16to8(i.Character.Y, i.map.Height);
                                Logger.ShowInfo(i.Character.Name + "(CharID:" + i.Character.CharID + ")[" + i.Map.Name +
                                                " " + x + "," + y + "] IP：" + i.Character.Account.LastIP);
                            }

                            Logger.ShowInfo(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                            MapClientManager.Instance.OnlinePlayer.Count);
                            Logger.ShowInfo("当前IP在线：" + MapClientManager.Instance.OnlinePlayerOnlyIP.Count);
                            break;
                        case "kick2":
                            if (args.Length > 1)
                                try
                                {
                                    MapClient client;
                                    var chr =
                                        from c in MapClientManager.Instance.OnlinePlayer
                                        where c.Character.CharID == uint.Parse(args[1])
                                        select c;
                                    if (chr.Count() > 0)
                                    {
                                        client = chr.First();
                                        client.NetIo.Disconnect();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    Logger.ShowError(exception, null);
                                }

                            break;
                    }
                }
                catch
                {
                }
        }

        private static void ShutingDown(object sender, ConsoleCancelEventArgs args)
        {
            Logger.ShowInfo("Closing.....", null);
            shutingdown = true;
            charDB.SaveServerVar(ScriptManager.Instance.VariableHolder);
            var clients = new MapClient[MapClientManager.Instance.Clients.Count];
            MapClientManager.Instance.Clients.CopyTo(clients);
            Logger.ShowInfo("Saving golem's data.....", null);

            var maps = MapManager.Instance.Maps.Values.ToArray();
            foreach (var i in maps)
            {
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
                        catch (Exception ex) { Logger.ShowError(ex); }
                    }*/
                if (i.IsMapInstance)
                    try
                    {
                        i.OnDestrory();
                    }
                    catch
                    {
                    }
            }

            Logger.ShowInfo("Saving player's data.....", null);

            foreach (var i in clients)
                try
                {
                    if (i.Character == null) continue;
                    i.NetIo.Disconnect();
                }
                catch (Exception exception)
                {
                    Logger.ShowError(exception, null);
                }


            Logger.ShowInfo("Closing MySQL connection....");
            if (charDB.GetType() == typeof(MySQLConnectivity))
            {
                var con = (MySQLConnectivity)charDB;
                while (!con.CanClose)
                    Thread.Sleep(100);
            }

            if (accountDB.GetType() == typeof(MySQLConnectivity))
            {
                var con = (MySQLConnectivity)accountDB;
                while (!con.CanClose)
                    Thread.Sleep(100);
            }
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            return null;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
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
                try
                {
                    if (i.Character == null) continue;
                    i.NetIo.Disconnect();
                }
                catch (Exception exception)
                {
                    Logger.ShowError(exception, null);
                }

            Logger.ShowError("Trying to clear golem actor");

            var maps = MapManager.Instance.Maps.Values.ToArray();
            foreach (var i in maps)
            {
                foreach (var j in i.Actors.Values)
                    if (j.type == ActorType.GOLEM)
                        try
                        {
                            var golem = (ActorGolem)j;
                            charDB.SaveChar(golem.Owner, true, false);
                        }
                        catch
                        {
                        }

                if (i.IsMapInstance)
                    i.OnDestrory();
            }

            Logger.ShowInfo("Closing MySQL connection....");
            if (charDB.GetType() == typeof(MySQLConnectivity))
            {
                var con = (MySQLConnectivity)charDB;
                while (!con.CanClose)
                    Thread.Sleep(100);
            }

            if (accountDB.GetType() == typeof(MySQLConnectivity))
            {
                var con = (MySQLConnectivity)accountDB;
                while (!con.CanClose)
                    Thread.Sleep(100);
            }
        }
    }
}