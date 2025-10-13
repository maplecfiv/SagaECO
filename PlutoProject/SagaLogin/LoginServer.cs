//#define FreeVersion

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using SagaDB;
using SagaDB.Item;
using SagaLib;
using SagaLib.Properties;
using SagaLib.VirtualFileSytem;
using SagaLogin.Manager;

namespace SagaLogin
{
    public class LoginServer
    {
        private static readonly NLog.Logger _logger = Logger.InitLogger<LoginServer>();
        /// <summary>
        ///     The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;

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
            }catch (Exception exception)
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
            // var WINDOW_HANDLER = FindWindow(null, fullPath);
            // var CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            // var SC_CLOSE = 0xF060;
            // RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);

            Console.CancelKeyPress += ShutingDown;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var Log = new Logger("SagaLogin.log");
            Logger.defaultlogger = Log;
            Logger.CurrentLogger = Log;
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            _logger.Debug("                     SagaECO Login Server                ");
            _logger.Debug("         (C)2013-2017 The Pluto ECO Project Development Team              ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("======================================================================");
            //Console.ResetColor();

            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            _logger.Debug("SagaLogin");
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

            Logger.ShowInfo("Starting Initialization...", null);

            Configuration.Configuration.Instance.Initialization("./Config/SagaLogin.xml");

            Logger.CurrentLogger.LogLevel = (Logger.LogContent)Configuration.Configuration.Instance.LogLevel;

            Logger.ShowInfo("Initializing VirtualFileSystem...");
#if FreeVersion1
            VirtualFileSystemManager.Instance.Init(FileSystems.LPK, "./DB/DB.lpk");
#else
            VirtualFileSystemManager.Instance.Init(FileSystems.Real, ".");
#endif
            ItemFactory.Instance.Init(
                VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/", "item*.csv",
                    SearchOption.TopDirectoryOnly),
                Encoding.UTF8);

            //MapInfoFactory.Instance.Init("DB/MapInfo.zip", false);

            if (!StartDatabase())
            {
                Logger.ShowError("cannot connect to dbserver", null);
                Logger.ShowError("Shutting down in 20sec.", null);
                Thread.Sleep(20000);
                return;
            }

            LoginClientManager.Instance.Start();
            if (!LoginClientManager.Instance.StartNetwork(Configuration.Configuration.Instance.Port))
            {
                Logger.ShowError("cannot listen on port: " + Configuration.Configuration.Instance.Port);
                Logger.ShowInfo("Shutting down in 20sec.");
                Thread.Sleep(20000);
                return;
            }


            Global.clientMananger = LoginClientManager.Instance;

            _logger.Debug("Accepting clients.");

            while (true)
            {
                // keep the connections to the database servers alive
                EnsureCharDB();
                EnsureAccountDB();
                // let new clients (max 10) connect
#if FreeVersion
                if (LoginClientManager.Instance.Clients.Count < int.Parse("15"))
#endif
                LoginClientManager.Instance.NetworkLoop(10);

                Thread.Sleep(10);
            }
        }

        private static void ShutingDown(object sender, ConsoleCancelEventArgs args)
        {
            Logger.ShowInfo("Closing.....", null);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            Logger.ShowError("Fatal: An unhandled exception is thrown, terminating...");
            Logger.ShowError("Error Message:" + ex.Message);
            Logger.ShowError("Call Stack:" + ex.StackTrace);
        }
    }
}