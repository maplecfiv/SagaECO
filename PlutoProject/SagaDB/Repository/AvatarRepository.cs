using System;
using SagaDB.Entities;

namespace SagaDB.Repository;

public class AvatarRepository {
    public static Avatar GetAvatar(uint accountId) {
        var avatars = SqlSugarHelper.Db.Queryable<Avatar>().Where(item => item.AccountId == accountId).ToList();
        return avatars.Count == 0 ? null : avatars[0];
    }

    public static bool SaveAvatar(uint accountId, byte[] values) {
        try {
            SqlSugarHelper.Db.BeginTran();

            var avatars = SqlSugarHelper.Db.Queryable<Avatar>().Where(item => item.AccountId == accountId)
                .ToList();

            switch (avatars.Count) {
                case 0:
                    SqlSugarHelper.Db.Insertable<Avatar>(new Avatar {
                        AccountId = accountId,
                        Valuess = values
                    }).ExecuteCommand();
                    break;
                case 1:
                    var avatar = avatars[0];
                    avatar.Valuess = values;
                    SqlSugarHelper.Db.Updateable<Avatar>().ExecuteCommand();
                    break;
                default:
                    throw new Exception($"more than avatars for {accountId} found!");
            }

            SqlSugarHelper.Db.CommitTran();
            return true;
        }
        catch (Exception ex) {
            SqlSugarHelper.Db.RollbackTran();
            SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            return false;
        }
    }
}