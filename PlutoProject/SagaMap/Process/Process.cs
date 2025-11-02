using System;
using System.Data;
using System.Linq;
using SagaDB;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SqlSugar;

namespace SagaMap.Process {
    internal class Process : MapServer {
        private string action;
        private uint charid, itemid;
        private ContainerType continer;
        private ushort qty;

        //MySQLActorDB sql = new MySQLActorDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort, Configuration.Instance.DBName, Configuration.Instance.DBUser, Configuration.Instance.DBPass);


        public void Action(uint charid, uint itemid, ushort qty) {
            this.charid = charid;
            this.qty = qty;
            this.itemid = itemid;
        }

        public void Query(uint charid) {
            this.charid = charid;
        }

        public void Announce(string msg) {
            try {
                foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendAnnounce(msg);
            }
            catch (Exception exception) {
                Logger.ShowError(exception);
            }
        }

        public int CheckAPIItem(uint charid, MapClient client) {
            //System.Threading.Thread.Sleep(2000);
            //MapClient client = MC(charid);

            var count = 0;

            try {
                SqlSugarHelper.Db.BeginTran();


                //MySQLActorDB sql = ConnectToDB();
                //MySQLActorDB sql = new MySQLActorDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort, Configuration.Instance.DBName, Configuration.Instance.DBUser, Configuration.Instance.DBPass);
                var result = SqlSugarHelper.Db.Queryable<SagaDB.Entities.ApiItem>().TranLock(DbLockType.Wait)
                    .Where(item => item.CharacterId == charid && item.Status == 0).ToList();

                foreach (SagaDB.Entities.ApiItem i in result) {
                    count++;

                    //Item Instance
                    var item = ItemFactory.Instance.GetItem(i.ItemId);
                    qty = i.Qty;
                    item.Stack = qty;


                    //Execute Add Item
                    client.AddItem(item, true);

                    //Save Record
                    i.ProcessTime = DateTime.Now;
                    i.Status = 1;
                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();

                    //sql.SQLExecuteNonQuery(str);
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                count = 0;
                SagaLib.Logger.ShowError(e);
            }

            return count;
        }

        public void SaveOfflineItem(uint charid, uint itemid, ushort qty) {
            //MySQLActorDB sql = ConnectToDB();
            //MySQLActorDB sql = new MySQLActorDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort, Configuration.Instance.DBName, Configuration.Instance.DBUser, Configuration.Instance.DBPass);


            SqlSugarHelper.Db.Insertable<SagaDB.Entities.ApiItem>(new SagaDB.Entities.ApiItem {
                CharacterId = charid, ItemId = itemid, Qty = qty, RequestTime = DateTime.Now, Status = 0
            }).ExecuteCommand();

            //sql.SQLExecuteNonQuery(str);
        }

        public void AddItem(MapClient i, uint itemid, ushort qty) {
            //Item Instance
            var item = ItemFactory.Instance.GetItem(itemid);
            item.Stack = qty;


            //Execute Add Item
            i.AddItem(item, true);


            //Save Record
            //MySQLActorDB sql = ConnectToDB();
            //MySQLActorDB sql = new MySQLActorDB(Configuration.Instance.DBHost, Configuration.Instance.DBPort, Configuration.Instance.DBName, Configuration.Instance.DBUser, Configuration.Instance.DBPass);

            SqlSugarHelper.Db.Insertable<SagaDB.Entities.ApiItem>(new SagaDB.Entities.ApiItem {
                CharacterId = charid, ItemId = itemid, Qty = qty, RequestTime = DateTime.Now,
                ProcessTime = DateTime.Now, Status = 1
            }).ExecuteCommand();

            //sql.SQLExecuteNonQuery(str);
        }

        public bool InvQuery() {
            //Client client = new Client();

            //Get Char Info
            var pc = charDB.GetChar(charid);

            pc.Inventory.GetContainer(ContainerType.BODY);


            //Check if Char is online
            MapClient i;
            var chr = from c in MapClientManager.Instance.OnlinePlayer
                where c.Character.Name == pc.Name
                select c;

            i = chr.First();
            AddItem(i, itemid, qty);
            Logger.ShowInfo("API Command execute successfully. (" + pc.Name + ")");


            return true;
        }

        public bool Load() {
            //Client client = new Client();

            //Get Char Info
            var pc = charDB.GetChar(charid);

            if (pc == null) {
                Logger.ShowError("NO SUCH CHARID" + charid);
                return false;
            }

            //Check if Char is online
            MapClient i;
            var chr = from c in MapClientManager.Instance.OnlinePlayer
                where c.Character.Name == pc.Name
                select c;
            if (chr.Count() == 0) {
                try {
                    SaveOfflineItem(charid, itemid, qty);
                }
                catch {
                    Logger.ShowError("ERROR ON SAVE OFFLINE APIITEM");
                }

                Logger
                    .ShowInfo("Player: " + pc.Name + " is offline, Item will be process on next login");
                return true;
            }

            i = chr.First();
            AddItem(i, itemid, qty);
            Logger.ShowInfo("API Command execute successfully. (" + pc.Name + ")");


            return true;
        }
    }
}