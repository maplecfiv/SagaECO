using SagaDB.Actor;
using SqlSugar;

namespace SagaDB.Repository;

public class PartnerRepository {
    public static Entities.Partner GetPartner(uint actorPartnerId) {
        return SqlSugarHelper.Db.Queryable<Entities.Partner>()
            .Where(item => item.ActorPartnerId == actorPartnerId).First();
    }

    public static Entities.Partner CreatePartner(ActorPartner ap) {
        return SqlSugarHelper.Db.Insertable<Entities.Partner>(new Entities.Partner {
            PartnerId = ap.partnerid,
            Name = ap.BaseData.name,
            Level = ap.Level,
            TrustLevel = ap.reliability, Rebirth = 0, Rank = ap.rank, PerkPoints = ap.perkpoint,
            Perk0 = ap.perk0,
            Perk1 = ap.perk1, Perk2 = ap.perk2, Perk3 = ap.perk3, Perk4 = ap.perk4, Perk5 = ap.perk5,
            AiMode = ap.ai_mode, BasicAiMode1 = ap.basic_ai_mode, BasicAiMode2 = ap.basic_ai_mode_2, Hp = ap.HP,
            MaxHp = ap.MaxHP, Mp = ap.MP, MaxMp = ap.MaxMP, Sp = ap.SP, MaxSp = ap.MaxSP
        }).ExecuteReturnEntity();
    }

    public static bool SavePartner(ActorPartner ap) {
        var i = SqlSugarHelper.Db.Queryable<Entities.Partner>().TranLock(DbLockType.Wait)
            .Where(item => item.PartnerId == ap.ActorPartnerID).First();
        if (i == null) {
            return false;
        }

        i.Name = ap.Name;
        i.Level = ap.Level;
        i.TrustLevel = ap.reliability;
        i.Rebirth = ap.rebirth ? (byte)1 : (byte)0;
        i.Rank = ap.rank;
        i.PerkPoints = ap.perkpoint;
        i.Hp = ap.HP;
        i.MaxHp = ap.MaxHP;
        i.Mp = ap.MP;
        i.MaxMp = ap.MaxMP;
        i.Sp = ap.SP;
        i.MaxSp = ap.MaxSP;
        i.Perk0 = ap.perk0;
        i.Perk1 = ap.perk1;
        i.Perk2 = ap.perk2;
        i.Perk3 = ap.perk3;
        i.Perk4 = ap.perk4;
        i.Perk5 = ap.perk5;
        i.AiMode = ap.ai_mode;
        i.BasicAiMode1 = ap.basic_ai_mode;
        i.BasicAiMode2 = ap.basic_ai_mode_2;
        i.Exp = ap.exp;
        i.PictId = ap.PictID;
        i.NextFeedTime = ap.nextfeedtime;
        i.ReliabilityUprate = ap.reliabilityuprate;
        i.TrustExp = ap.reliabilityexp;
        return SqlSugarHelper.Db.Updateable(i).ExecuteCommand() != 0;
    }
}