using SagaDB.Actor;
using SagaDB.Entities;
using SagaDB.Partner;

namespace SagaDB.Repository;

public class PartnerEquipRepository {
    public static void SavePartnerEquip(ActorPartner ap) {
        foreach (var equip in SqlSugarHelper.Db.Queryable<Entities.PartnerEquip>()
                     .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
            SqlSugarHelper.Db.Deleteable(equip).ExecuteCommand();
        }

        if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.COSTUME)) {
            SqlSugarHelper.Db.Insertable(new PartnerEquip {
                ActorPartnerId = ap.ActorPartnerID, Type = 1,
                ItemId = ap.equipments[EnumPartnerEquipSlot.COSTUME].ItemID,
                Count = ap.equipments[EnumPartnerEquipSlot.COSTUME].Stack
            }).ExecuteCommand();
        }

        if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.WEAPON)) {
            SqlSugarHelper.Db.Insertable(new PartnerEquip {
                ActorPartnerId = ap.ActorPartnerID, Type = 2,
                ItemId = ap.equipments[EnumPartnerEquipSlot.WEAPON].ItemID,
                Count = ap.equipments[EnumPartnerEquipSlot.WEAPON].Stack
            }).ExecuteCommand();
        }

        for (var i = 0; i < ap.foods.Count; i++) {
            SqlSugarHelper.Db.Insertable(new PartnerEquip {
                ActorPartnerId = ap.ActorPartnerID, Type = 3, ItemId = ap.foods[i].ItemID,
                Count = ap.foods[i].Stack
            }).ExecuteCommand();
        }
    }
}