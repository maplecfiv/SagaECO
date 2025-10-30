using System;
using System.Collections.Generic;

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
}