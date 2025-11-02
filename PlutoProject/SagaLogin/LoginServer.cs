//#define FreeVersion

using System;
using System.IO;
using System.Text;
using System.Threading;
using SagaDB;
using SagaDB.Item;
using SagaLib;
using SagaLib.Properties;
using SagaLib.VirtualFileSytem;
using SagaLogin.Manager;

namespace SagaLogin {
    public class LoginServer {
        /// <summary>
        ///     The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;

        public static bool StartDatabase() {
            //     try {
            //         switch (Configuration.Configuration.Instance.DBType) {
            //             case 0:
            charDB = new MySqlActorDb();
            accountDB = new MySQLAccountDB( /*Configuration.Configuration.Instance.DBHost,
                Configuration.Configuration.Instance.DBPort,
                Configuration.Configuration.Instance.DBName, Configuration.Configuration.Instance.DBUser,
                Configuration.Configuration.Instance.DBPass*/);
            //                 // charDB.Connect();
            //                 accountDB.Connect();
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

        public static void EnsureCharDB() {
            var notConnected = false;

            while (notConnected) {
                Logger.ShowInfo("Trying to reconnect to char db server ..", null);
            }
        }

        public static void EnsureAccountDB() {
            var notConnected = false;

            if (!accountDB.isConnected()) {
                Logger.ShowWarning("LOST CONNECTION TO CHAR DB SERVER!", null);
                notConnected = true;
            }

            while (notConnected) {
                Logger.ShowInfo("Trying to reconnect to char db server ..", null);
                accountDB.Connect();
                if (!accountDB.isConnected()) {
                    Logger.ShowError("Failed.. Trying again in 10sec", null);
                    Thread.Sleep(10000);
                    notConnected = true;
                }
                else {
                    Logger.ShowInfo("SUCCESSFULLY RE-CONNECTED to char db server...", null);
                    Logger.ShowInfo("Clients can now connect again", null);
                    notConnected = false;
                }
            }
        }

        // [DllImport("User32.dll ", EntryPoint = "FindWindow")]

        // [DllImport("user32.dll ", EntryPoint = "GetSystemMenu")]

        // [DllImport("user32.dll ", EntryPoint = "RemoveMenu")]


        private static void ThreadShutdownWatcher() {
            while (!ConfigLoader.ShouldShutdown()) {
                Thread.Sleep(1000);
            }

            ShutingDown(null, null);
        }

        private static void Main(string[] args) {
            // var WINDOW_HANDLER = FindWindow(null, fullPath);
            // var CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            // var SC_CLOSE = 0xF060;
            // RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);

            Console.CancelKeyPress += ShutingDown;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Console.ForegroundColor = ConsoleColor.Yellow;
            SagaLib.Logger.ShowInfo("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            SagaLib.Logger.ShowInfo("                     SagaECO Login Server                ");
            SagaLib.Logger.ShowInfo("         (C)2013-2017 The Pluto ECO Project Development Team              ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            SagaLib.Logger.ShowInfo("======================================================================");
            //Console.ResetColor();

            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            SagaLib.Logger.ShowInfo("SagaLogin");
            //Console.ForegroundColor = ConsoleColor.White;
            SagaLib.Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            SagaLib.Logger.ShowInfo("SagaLib");
            //Console.ForegroundColor = ConsoleColor.White;
            SagaLib.Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" +
                                    GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            SagaLib.Logger.ShowInfo("SagaDB");
            //Console.ForegroundColor = ConsoleColor.White;
            SagaLib.Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" +
                                    GlobalInfo.ModifyDate + ")");


            Logger.ShowInfo("Starting Initialization...", null);

            Configuration.Configuration.Instance.Initialization(
                $"{ConfigLoader.LoadConfigPath()}/SagaLogin.xml");

            //null.LogLevel = (Logger.LogContent)Configuration.Configuration.Instance.LogLevel;

            Logger.ShowInfo("Initializing VirtualFileSystem...");
// #if FreeVersion1
            // VirtualFileSystemManager.Instance.Init(FileSystems.LPK, $"{ConfigLoader.LoadDbPath()}/DB.lpk");
// #else
            VirtualFileSystemManager.Instance.Init(FileSystems.Real, ".");
// #endif
            ItemFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile(ConfigLoader.LoadDbPath(), "item*.csv"),
                Encoding.UTF8);

            //MapInfoFactory.Instance.Init("DB/MapInfo.zip", false);
            StartDatabase();
            // if (!StartDatabase()) {
            //     Logger.ShowError("cannot connect to dbserver", null);
            //     Logger.ShowError("Shutting down in 20sec.", null);
            //     Thread.Sleep(20000);
            //     return;
            // }

            LoginClientManager.Instance.Start();
            if (!LoginClientManager.Instance.StartNetwork(Configuration.Configuration.Instance.Port)) {
                Logger.ShowError("cannot listen on port: " + Configuration.Configuration.Instance.Port);
                Logger.ShowInfo("Shutting down in 20sec.");
                Thread.Sleep(20000);
                return;
            }

            Global.clientMananger = LoginClientManager.Instance;

            SagaLib.Logger.ShowInfo("Accepting clients.");

            new Thread(ThreadShutdownWatcher).Start();

            while (true) {
                // keep the connections to the database servers alive
                // EnsureCharDB();
                // EnsureAccountDB();
                // let new clients (max 10) connect
#if FreeVersion
                if (LoginClientManager.Instance.Clients.Count < int.Parse("15"))
#endif
                LoginClientManager.Instance.NetworkLoop(10);

                System.Threading.Thread.Sleep(10);
            }
        }

        private static void ShutingDown(object sender, ConsoleCancelEventArgs args) {
            Logger.ShowInfo("Closing.....", null);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            Logger.ShowError("Fatal: An unhandled exception is thrown, terminating...");
            Logger.ShowError("Error Message:" + ex.Message);
            Logger.ShowError("Call Stack:" + ex.StackTrace);
        }
    }
}