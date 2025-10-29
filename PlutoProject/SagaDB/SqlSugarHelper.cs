using System;
using SagaLib;
using SqlSugar;

namespace SagaDB;

public class SqlSugarHelper // Cannot be a generic class
{
    // Instructions for multi-database usage:
    // For fixed multiple databases, you can pass new SqlSugarScope(List<ConnectionConfig>, db => {}). See the documentation on multi-tenancy.
    // For variable multiple databases, refer to the SaaS sub-database documentation.
    // Use singleton pattern
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig() {
            ConnectionString = $"datasource={ConfigLoader.LoadDbPath()}/eco.db", // Connection string
            DbType = DbType.Sqlite, // Database type
            IsAutoCloseConnection = true, // If not set to true, manual close is required
            LanguageType = LanguageType.Default
        },
        db => {
            db.Ado.ExecuteCommand("PRAGMA journal_mode = WAL;");
            // (A) Global effect configuration point, generally used for AOP and program startup configurations, effective for all contexts
            // Debugging SQL event, can be removed
            db.Aop.OnLogExecuting = (sql, pars) => {
                // Recommended to get native SQL, performance is OK for version 5.1.4.63
                SagaLib.Logger.ShowInfo(UtilMethods.GetNativeSql(sql, pars));

                // Get non-parameterized SQL, affects performance especially with large SQL and many parameters, use for debugging
                // Console.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars))
            };

            // Add multiple configurations below
            // db.Ado.IsDisableMasterSlaveSeparation = true;

            // Note: For multi-tenancy, configure as many as needed
            // db.GetConnection(i).Aop

            db.DbMaintenance.CreateDatabase();

            db.CodeFirst.InitTables<Entities.AnotherPaper>();
            db.CodeFirst.InitTables<Entities.Avatar>();
            db.CodeFirst.InitTables<Entities.Bbs>();
            db.CodeFirst.InitTables<Entities.Character>();
            db.CodeFirst.InitTables<Entities.Warehouse>();
            db.CodeFirst.InitTables<Entities.Inventory>();
            db.CodeFirst.InitTables<Entities.PartyMember>();
            db.CodeFirst.InitTables<Entities.Party>();
            db.CodeFirst.InitTables<Entities.FlyingCastle>();
            db.CodeFirst.InitTables<Entities.Ring>();
            db.CodeFirst.InitTables<Entities.RingMember>();
            db.CodeFirst.InitTables<Entities.DualJob>();
            db.CodeFirst.InitTables<Entities.DualJobSkill>();
            db.CodeFirst.InitTables<Entities.Gifts>();
            db.CodeFirst.InitTables<Entities.Mails>();
            db.CodeFirst.InitTables<Entities.Stamp>();
            db.CodeFirst.InitTables<Entities.PartnerEquip>();
            db.CodeFirst.InitTables<Entities.PartnerCube>();
            db.CodeFirst.InitTables<Entities.Partner>();
            db.CodeFirst.InitTables<Entities.PartnerAi>();
            db.CodeFirst.InitTables<Entities.FlyingCastleFurniture>();
            db.CodeFirst.InitTables<Entities.FlyingCastleFurnitureCopy>();
            db.CodeFirst.InitTables<Entities.TamaireLending>();
            db.CodeFirst.InitTables<Entities.TamaireRental>();
            db.CodeFirst.InitTables<Entities.QuestInfo>();
            db.CodeFirst.InitTables<Entities.NpcStates>();
            db.CodeFirst.InitTables<Entities.Friend>();
            db.CodeFirst.InitTables<Entities.ServerVariable>();
            db.CodeFirst.InitTables<Entities.Server>();
            db.CodeFirst.InitTables<Entities.SettingList>();
            db.CodeFirst.InitTables<Entities.FlyingGarden>();
            db.CodeFirst.InitTables<Entities.FlyingGardenFurniture>();
            db.CodeFirst.InitTables<Entities.Skill>();
            db.CodeFirst.InitTables<Entities.CharacterVariable>();
            db.CodeFirst.InitTables<Entities.LevelLimit>();
            db.CodeFirst.InitTables<Entities.MobStates>();
            db.CodeFirst.InitTables<Entities.Login>();
            db.CodeFirst.InitTables<Entities.ApiItem>();


            if (db.Queryable<Entities.Login>().Where(item => item.Username == "barrysm").ToList().Count == 0) {
                var i = new Entities.Login();
                i.Username = "barrysm";
                i.Password = Encryption.HashPassword("jsteam");
                i.DeletePassword = Encryption.HashPassword("jsteam");
                i.Bank = 0;
                i.Banned = 0;
                i.LastIp = "172.16.160.1";
                i.LastIp2 = "172.16.160.1";
                i.QuestResetTime = DateTime.Now;
                i.LastLoginTime = DateTime.Now;
                i.MacAddress = "0";
                i.PlayerNames = "barrysm";

                db.Insertable<Entities.Login>(i).ExecuteCommand();
            }
        });
}