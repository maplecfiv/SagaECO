using System.Collections.Generic;
using SagaDB.Actor;
using SqlSugar;

namespace SagaDB.Repository;

public class DualJobRepository {
    public static List<Entities.DualJob> GetDualJobInfo(uint characterId) {
        return SqlSugarHelper.Db.Queryable<Entities.DualJob>()
            .Where(item => item.CharacterId == characterId).ToList();
    }

    public static void SaveDualJobInfo(ActorPC pc) {
        var dic = pc.PlayerDualJobList;

        foreach (var item in dic.Keys) {
            SqlSugarHelper.Db.Storageable(new Entities.DualJob {
                CharacterId = pc.CharID,
                SeriesId = dic[item].DualJobId,
                Level = dic[item].DualJobLevel,
                Exp = dic[item].DualJobExp
            }).ExecuteCommand();
        }
    }
}