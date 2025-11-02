using System;
using System.Threading;
using SagaLib;
using SagaDB;
using SagaValidation.Manager;
using SagaLib.Properties;

namespace SagaValidation {
    public class ValidationServer {
        /// <summary>
        /// The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;

        public static bool StartDatabase() {
            //     try {
            //         switch (Configuration.Instance.DBType) {
            //             case 0:
            charDB = new MySqlActorDb();
            accountDB = new MySQLAccountDB( /*Configuration.Instance.DBHost, Configuration.Instance.DBPort,
                Configuration.Instance.DBName, Configuration.Instance.DBUser,
                Configuration.Instance.DBPass*/);
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


        static void Main(string[] args) {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.ShowInfo("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            Logger.ShowInfo("                     SagaECO Validation Server             ");
            Logger
                .ShowInfo("           (C)2013-2017 The Pluto ECO Project Development Team                  ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.ShowInfo("======================================================================");

            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.ShowInfo("SagaValidation");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.ShowInfo("SagaLib");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.ShowInfo("SagaDB");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.ShowInfo(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");

            Logger.ShowInfo("Starting Initialization...", null);

            Configuration.Instance.Initialization(
                $"{ConfigLoader.LoadConfigPath()}/SagaValidation.xml");
            //null.LogLevel = (Logger.LogContent)Configuration.Instance.LogLevel;

            StartDatabase();
            // if (!StartDatabase()) {
            //     Logger.ShowError("cannot connect to dbserver", null);
            //     Logger.ShowError("Shutting down in 20sec.", null);
            //     System.Threading.Thread.Sleep(20000);
            //     return;
            // }

            ValidationClientManager.Instance.Start();
            if (!ValidationClientManager.Instance.StartNetwork(Configuration.Instance.Port)) {
                Logger.ShowError("cannot listen on port: " + Configuration.Instance.Port);
                Logger.ShowInfo("Shutting down in 20sec.");
                System.Threading.Thread.Sleep(20000);
                return;
            }

            Global.clientMananger = (ClientManager)ValidationClientManager.Instance;

            Logger.ShowInfo(string.Format("Accept Client Version at : {0}",
                Configuration.Instance.ClientGameVersion));

            Logger.ShowInfo("Accepting clients.");

            while (!ConfigLoader.ShouldShutdown()) {
                // keep the connections to the database servers alive
                // EnsureAccountDB();
                // let new clients (max 10) connect

                ValidationClientManager.Instance.NetworkLoop(10);
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void EnsureAccountDB() {
            bool connected = false;

            if (!accountDB.isConnected()) {
                Logger.ShowWarning("LOST CONNECTION TO CHAR DB SERVER!", null);
                connected = false;
            }
            else {
                connected = true;
            }

            while (!connected) {
                Logger.ShowInfo("Trying to reconnect to char db server ..", null);
                accountDB.Connect();
                if (!accountDB.isConnected()) {
                    Logger.ShowError("Failed.. Trying again in 10sec", null);
                    System.Threading.Thread.Sleep(10000);
                    connected = false;
                    continue;
                }

                Logger.ShowInfo("SUCCESSFULLY RE-CONNECTED to char db server...", null);
                Logger.ShowInfo("Clients can now connect again", null);
                connected = true;
            }
        }
    }
}