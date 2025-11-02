using System;
using SagaDB.Actor;
using SagaDB.Entities;
using SqlSugar;

namespace SagaDB.Repository;

public class PartnerAiRepository {
    public static void SavePartnerAi(ActorPartner ap) {
        foreach (var ai in SqlSugarHelper.Db.Queryable<PartnerAi>().TranLock(DbLockType.Wait)
                     .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
            SqlSugarHelper.Db.Deleteable((ai)).ExecuteCommand();
        }

        foreach (var item in ap.ai_conditions) {
            SqlSugarHelper.Db.Insertable(new PartnerAi {
                ActorPartnerId = ap.ActorPartnerID, Type = 1, Index = item.Key, Value = item.Value
            }).ExecuteCommand();
        }


        foreach (var item in ap.ai_reactions) {
            SqlSugarHelper.Db.Insertable(new PartnerAi {
                ActorPartnerId = ap.ActorPartnerID, Type = 2, Index = item.Key, Value = item.Value
            }).ExecuteCommand();
        }

        foreach (var item in ap.ai_intervals) {
            SqlSugarHelper.Db.Insertable(new PartnerAi {
                ActorPartnerId = ap.ActorPartnerID, Type = 3, Index = item.Key, Value = item.Value
            }).ExecuteCommand();
        }


        foreach (var item in ap.ai_states) {
            SqlSugarHelper.Db.Insertable(new PartnerAi {
                ActorPartnerId = ap.ActorPartnerID, Type = 4, Index = item.Key,
                Value = Convert.ToUInt16(item.Value)
            }).ExecuteCommand();
        }
    }
}