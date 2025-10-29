using System.Collections.Generic;
using SagaDB.Actor;
using SqlSugar;

namespace SagaDB.Repository;

public class DualJobSkillRepository {
    public static void SaveDualJobSkillInfo(ActorPC pc) {
        foreach (var item in pc.DualJobSkills) {
            SqlSugarHelper.Db.Storageable(new Entities.DualJobSkill {
                CharacterId = pc.CharID,
                SeriesId = pc.DualJobID,
                SkillId = item.ID,
                SkillLevel = item.Level
            }).ExecuteCommand();
        }
    }

    public static void SaveDualJobSkill(ActorPC pc) {
        foreach (var item in pc.DualJobSkills) {
            SqlSugarHelper.Db.Storageable(new Entities.DualJobSkill {
                CharacterId = pc.CharID,
                SeriesId = pc.DualJobID,
                SkillId = item.ID,
                SkillLevel = item.Level
            }).ExecuteCommand();
        }
    }

    public static List<Entities.DualJobSkill> GetDualJobSkill(ActorPC pc) {
        return SqlSugarHelper.Db.Queryable<Entities.DualJobSkill>()
            .Where(item => item.CharacterId == pc.CharID && item.SeriesId == pc.DualJobID).ToList();
    }
}