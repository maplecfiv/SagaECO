using System;
using System.Collections.Generic;
using SqlSugar;

namespace SagaDB.Repository;

public class LoginRepository {
    public static Entities.Login GetLoginByUsername(string username) {
        return SqlSugarHelper.Db.Queryable<Entities.Login>().Where(item => item.Username == username)
            .First();
    }

    public static List<Entities.Login> GetAllAccount() {
        return SqlSugarHelper.Db.Queryable<Entities.Login>().ToList();
    }

    public static Entities.Login WriteUser(Account user) {
        var i = SqlSugarHelper.Db.Queryable<Entities.Login>().TranLock(DbLockType.Wait)
            .Where(item => item.AccountId == user.AccountID).First();

        if (i == null) {
            return SqlSugarHelper.Db.Storageable(new Entities.Login {
                AccountId = user.AccountID,
                Username = user.Name,
                Password = user.Password,
                DeletePassword = user.DeletePassword,
                Bank = user.Bank,
                Banned = user.Banned ? (byte)1 : (byte)0,
                LastIp = user.LastIP,
                QuestResetTime = user.questNextTime,
                LastLoginTime = DateTime.Now,
                MacAddress = user.MacAddress,
                PlayerNames = user.PlayerNames,
            }).ExecuteReturnEntity();
        }
        else {
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
            return SqlSugarHelper.Db.Updateable(i).ExecuteReturnEntity();
        }
    }
}