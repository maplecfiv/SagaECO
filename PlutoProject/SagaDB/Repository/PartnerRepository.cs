using SagaDB.Actor;

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
        return SqlSugarHelper.Db.Storageable(new Entities.Partner {
            PartnerId = ap.ActorPartnerID,
            Name = ap.Name,
            Level = ap.Level,
            TrustLevel = ap.reliability,
            Rebirth = ap.rebirth ? (byte)1 : (byte)0,
            Rank = ap.rank,
            PerkPoints = ap.perkpoint,
            Hp = ap.HP,
            MaxHp = ap.MaxHP,
            Mp = ap.MP,
            MaxMp = ap.MaxMP,
            Sp = ap.SP,
            MaxSp = ap.MaxSP,
            Perk0 = ap.perk0,
            Perk1 = ap.perk1,
            Perk2 = ap.perk2,
            Perk3 = ap.perk3,
            Perk4 = ap.perk4,
            Perk5 = ap.perk5,
            AiMode = ap.ai_mode,
            BasicAiMode1 = ap.basic_ai_mode,
            BasicAiMode2 = ap.basic_ai_mode_2,
            Exp = ap.exp,
            PictId = ap.PictID,
            NextFeedTime = ap.nextfeedtime,
            ReliabilityUprate = ap.reliabilityuprate, TrustExp = ap.reliabilityexp
        }).ExecuteCommand() != 0;
    }
}