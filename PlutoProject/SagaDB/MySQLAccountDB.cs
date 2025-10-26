using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using SagaDB.Actor;
using SagaLib;
using SqlSugar;

namespace SagaDB {
    public class MySQLAccountDB : MySQLConnectivity, AccountDB {
        private static readonly Serilog.Core.Logger _logger = Logger.InitLogger<MySQLAccountDB>();

        private readonly string database;
        private readonly string dbpass;
        private readonly string dbuser;
        private readonly string host;
        private readonly string port;
        private Encoding encoder = Encoding.UTF8;
        private bool isconnected;
        private DateTime tick = DateTime.Now;


        public MySQLAccountDB(string host, int port, string database, string user, string pass) {
            this.host = host;
            this.port = port.ToString();
            dbuser = user;
            dbpass = pass;
            this.database = database;
            isconnected = false;
            try {
                db = new MySqlConnection(string.Format("Server={1};Port={2};Uid={3};Pwd={4};Database={0};Charset=utf8;",
                    database, host, port, user, pass));
                dbinactive = new MySqlConnection(string.Format(
                    "Server={1};Port={2};Uid={3};Pwd={4};Database={0};Charset=utf8;", database, host, port, user,
                    pass));
                db.Open();
            }
            catch (MySqlException ex) {
                Logger.ShowSQL(ex, null);
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }

            if (db != null) {
                if (db.State != ConnectionState.Closed) isconnected = true;
                else _logger.Error("SQL Connection error");
            }
        }

        public bool Connect() {
            if (!isconnected) {
                if (db.State == ConnectionState.Open) {
                    isconnected = true;
                    return true;
                }

                try {
                    db.Open();
                }
                catch (Exception exception) {
                    Logger.ShowError(exception);
                }

                if (db != null) {
                    if (db.State != ConnectionState.Closed) return true;
                    return false;
                }
            }

            return true;
        }

        public bool isConnected() {
            if (isconnected) {
                var newtime = DateTime.Now - tick;
                if (newtime.TotalMinutes > 5) {
                    MySqlConnection tmp;
                    Logger.ShowSQL("AccountDB:Pinging SQL Server to keep the connection alive", null);
                    /* we actually disconnect from the mysql server, because if we keep the connection too long
                     * and the user resource of this mysql connection is full, mysql will begin to ignore our
                     * queries -_-
                     */
                    var criticalarea = ClientManager.Blocked;
                    if (criticalarea)
                        ClientManager.LeaveCriticalArea();
                    DatabaseWaitress.EnterCriticalArea();
                    tmp = dbinactive;
                    if (tmp.State == ConnectionState.Open) tmp.Close();
                    try {
                        tmp.Open();
                    }
                    catch (Exception exception) {
                        Logger.ShowError(exception);
                        tmp = new MySqlConnection(string.Format("Server={1};Port={2};Uid={3};Pwd={4};Database={0};",
                            database, host, port, dbuser, dbpass));
                        tmp.Open();
                    }

                    dbinactive = db;
                    db = tmp;
                    tick = DateTime.Now;
                    DatabaseWaitress.LeaveCriticalArea();
                    if (criticalarea)
                        ClientManager.EnterCriticalArea();
                }

                if (db.State == ConnectionState.Broken || db.State == ConnectionState.Closed) isconnected = false;
            }

            return isconnected;
        }


        //#region AccountDB Members

        private void SavePaper(ActorPC aChar) {
        }

        public void WriteUser(Account user) {
            if (user == null || !isConnected()) {
                return;
            }

            try {
                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.Login>().TranLock(DbLockType.Wait)
                             .Where(item => item.AccountId == user.AccountID).ToList()) {
                    i.Username = user.Name;
                    i.Password = user.Password;
                    i.DeletePassword = user.DeletePassword;
                    i.Bank = user.Bank;
                    i.Banned = user.Banned ? (byte)1 : (byte)0;
                    i.LastIp = user.LastIP;
                    i.QuestResetTime = user.questNextTime;
                    i.LastLoginTime = DateTime.Now;
                    i.MacAddress = user.MacAddress;
                    i.PlayerNames = user.PlayerNames;
                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public List<Account> GetAllAccount() {
            var accounts = new List<Account>();
            Account account;
            try {
                var result = SqlSugarHelper.Db.Queryable<Entities.Login>().ToList();


                if (result.Count == 0) {
                    return null;
                }

                for (var i = 0; i < result.Count; i++) {
                    account = new Account();
                    account.AccountID = result[i].AccountId;
                    account.Name = (string)result[i].Username;
                    account.Password = (string)result[i].Password;
                    account.DeletePassword = (string)result[i].DeletePassword;
                    account.GMLevel = (byte)result[i].GameMasterLevel;
                    account.Bank = (uint)result[i].Bank;
                    account.questNextTime = (DateTime)result[i].QuestResetTime;
                    account.lastLoginTime = (DateTime)result[i].LastLoginTime;
                    try {
                        account.LastIP = (string)result[i].LastIp;
                    }
                    catch (Exception ex) {
                        Logger.ShowError(ex);
                    }

                    accounts.Add(account);
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return null;
            }


            return accounts;
        }

        public Account GetUser(string name) {
            Account account = null;

            try {
                var result = SqlSugarHelper.Db.Queryable<Entities.Login>().Where(item => item.Username == name)
                    .ToList();


                if (result.Count == 0) {
                    return null;
                }

                account = new Account();
                account.AccountID = result[0].AccountId;
                account.Name = name;
                account.Password = (string)result[0].Password;
                account.DeletePassword = (string)result[0].DeletePassword;
                account.GMLevel = (byte)result[0].GameMasterLevel;
                account.Bank = (uint)result[0].Bank;
                account.questNextTime = (DateTime)result[0].QuestResetTime;
                try {
                    account.LastIP2 = (string)result[0].LastIp2;
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }

                account.Banned = ((byte)result[0].Banned == 1);
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return null;
            }


            return account;
        }

        public bool CheckPassword(string user, string password, uint frontword, uint backword) {
            try {
                var result = SqlSugarHelper.Db.Queryable<Entities.Login>().Where(item => item.Username == user)
                    .ToList();
                return (result.Count == 0)
                    ? false
                    : password == Conversions.bytes2HexString(SHA1.Create()
                        .ComputeHash(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}", frontword,
                            ((string)result[0].Password).ToLower(), backword)))).ToLower();
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return false;
            }
        }

        public int GetAccountID(string user) {
            try {
                var result = SqlSugarHelper.Db.Queryable<Entities.Login>().Where(item => item.Username == user)
                    .ToList();

                return (result.Count == 0) ? -1 : (int)result[0].AccountId;
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return -1;
            }
        }

        //#endregion
    }
}