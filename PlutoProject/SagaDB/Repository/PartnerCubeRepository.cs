using SagaDB.Actor;
using SagaDB.Entities;

namespace SagaDB.Repository;

public class PartnerCubeRepository {
    public static void SavePartnerCube(ActorPartner ap) {
        foreach (var partnerCube in SqlSugarHelper.Db.Queryable<PartnerCube>()
                     .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
            SqlSugarHelper.Db.Deleteable(partnerCube);
        }


        for (var i = 0; i < ap.equipcubes_condition.Count; i++) {
            SqlSugarHelper.Db.Insertable(new PartnerCube {
                ActorPartnerId = ap.ActorPartnerID, Type = 1, UniqueId = ap.equipcubes_condition[i]
            }).ExecuteCommand();
        }

        for (var i = 0; i < ap.equipcubes_action.Count; i++) {
            SqlSugarHelper.Db.Insertable(new PartnerCube {
                ActorPartnerId = ap.ActorPartnerID, Type = 2, UniqueId = ap.equipcubes_action[i]
            }).ExecuteCommand();
        }

        for (var i = 0; i < ap.equipcubes_activeskill.Count; i++) {
            SqlSugarHelper.Db.Insertable(new PartnerCube {
                ActorPartnerId = ap.ActorPartnerID, Type = 3, UniqueId = ap.equipcubes_activeskill[i]
            }).ExecuteCommand();
        }

        for (var i = 0; i < ap.equipcubes_passiveskill.Count; i++) {
            SqlSugarHelper.Db.Insertable(new PartnerCube {
                ActorPartnerId = ap.ActorPartnerID, Type = 4, UniqueId = ap.equipcubes_passiveskill[i]
            }).ExecuteCommand();
        }
    }
}