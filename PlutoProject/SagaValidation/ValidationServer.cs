using System;
using SagaLib;
using SagaDB;
using SagaValidation.Manager;
using SagaLib.Properties;

namespace SagaValidation
{
    public class ValidationServer
    {
        /// <summary>
        /// The characterdatabase associated to this mapserver.
        /// </summary>
        public static ActorDB charDB;

        public static AccountDB accountDB;

        public static bool StartDatabase()
        {
            try
            {
                switch (Configuration.Instance.DBType)
                {
                    case 0:
                        charDB = new MySQLActorDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort,
                            Configuration.Instance.DBName, Configuration.Instance.DBUser,
                            Configuration.Instance.DBPass);
                        accountDB = new MySQLAccountDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort,
                            Configuration.Instance.DBName, Configuration.Instance.DBUser,
                            Configuration.Instance.DBPass);
                        charDB.Connect();
                        accountDB.Connect();
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception exception)
            {
                Logger.getLogger().Error(exception, null);
                return false;
            }
        }

        static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.getLogger().Information("======================================================================");
            //Console.ForegroundColor = ConsoleColor.Cyan;
            Logger.getLogger().Information("                     SagaECO Validation Server             ");
            Logger.getLogger()
                .Information("           (C)2013-2017 The Pluto ECO Project Development Team                  ");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.getLogger().Information("======================================================================");

            //Console.ForegroundColor = ConsoleColor.White;
            Logger.getLogger().Information("Version Informations:");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.getLogger().Information("SagaValidation");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.getLogger().Information(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.getLogger().Information("SagaLib");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.getLogger().Information(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            Logger.getLogger().Information("SagaDB");
            //Console.ForegroundColor = ConsoleColor.White;
            Logger.getLogger().Information(":SVN Rev." + GlobalInfo.Version + "(" + GlobalInfo.ModifyDate + ")");

            Logger.getLogger().Information("Starting Initialization...", null);

            Configuration.Instance.Initialization("./Config/SagaValidation.xml");
            Logger.CurrentLogger.LogLevel = (Logger.LogContent)Configuration.Instance.LogLevel;

            if (!StartDatabase())
            {
                Logger.getLogger().Error("cannot connect to dbserver", null);
                Logger.getLogger().Error("Shutting down in 20sec.", null);
                System.Threading.Thread.Sleep(20000);
                return;
            }

            ValidationClientManager.Instance.Start();
            if (!ValidationClientManager.Instance.StartNetwork(Configuration.Instance.Port))
            {
                Logger.getLogger().Error("cannot listen on port: " + Configuration.Instance.Port);
                Logger.getLogger().Information("Shutting down in 20sec.");
                System.Threading.Thread.Sleep(20000);
                return;
            }

            Global.clientMananger = (ClientManager)ValidationClientManager.Instance;

            Logger.getLogger().Information(string.Format("Accept Client Version at : {0}",
                Configuration.Instance.ClientGameVersion));

            Logger.getLogger().Information("Accepting clients.");

            while (true)
            {
                // keep the connections to the database servers alive
                EnsureAccountDB();
                // let new clients (max 10) connect

                ValidationClientManager.Instance.NetworkLoop(10);
                System.Threading.Thread.Sleep(1);
            }
        }

        public static void EnsureAccountDB()
        {
            bool connected = false;

            if (!accountDB.isConnected())
            {
                Logger.getLogger().Warning("LOST CONNECTION TO CHAR DB SERVER!", null);
                connected = false;
            }
            else
            {
                connected = true;
            }

            while (!connected)
            {
                Logger.getLogger().Information("Trying to reconnect to char db server ..", null);
                accountDB.Connect();
                if (!accountDB.isConnected())
                {
                    Logger.getLogger().Error("Failed.. Trying again in 10sec", null);
                    System.Threading.Thread.Sleep(10000);
                    connected = false;
                    continue;
                }

                Logger.getLogger().Information("SUCCESSFULLY RE-CONNECTED to char db server...", null);
                Logger.getLogger().Information("Clients can now connect again", null);
                connected = true;
            }
        }
    }
}