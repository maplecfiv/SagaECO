using System;
using SagaDB.Actor;
using SagaDB.Entities;
using SqlSugar;

namespace SagaDB.Repository;

public class CharacterRepository {
    public static bool SaveCharacter(ActorPC aChar, uint mapId, byte x, byte y, uint questid, DateTime questRemainTime,
        byte status,
        int count1, int count2, int count3, uint partyId, uint ringId, uint golemId) {
        SqlSugarHelper.Db.Storageable<Character>(new Entities.Character {
            CharacterId = aChar.CharID,
            Name = (aChar.Name),
            Race = (byte)aChar.Race,
            Gender = (byte)aChar.Gender,
            HairStyle = aChar.HairStyle,
            HairColor = aChar.HairColor,
            Wig = aChar.Wig,
            Face = aChar.Face,
            Job = (byte)aChar.Job,
            MapId = mapId,
            Lv = aChar.Level,
            JobLv1 = aChar.JobLevel1,
            JobLv2x = aChar.JobLevel2X,
            JobLv2t = aChar.JobLevel2T,
            QuestRemaining = aChar.QuestRemaining,
            Slot = aChar.Slot,
            X = x,
            Y = y,
            Dir = (byte)(aChar.Dir / 45),
            Hp = aChar.HP,
            MaxHp = aChar.MaxHP,
            Mp = aChar.MP,
            MaxMp = aChar.MaxMP,
            Sp = aChar.SP,
            MaxSp = aChar.MaxSP,
            Str = aChar.Str,
            Dex = aChar.Dex,
            Int = aChar.Int,
            Vit = aChar.Vit,
            Agi = aChar.Agi,
            Mag = aChar.Mag,
            StatsPoint = aChar.StatsPoint,
            SkillPoint = aChar.SkillPoint,
            SkillPoint2x = aChar.SkillPoint2X,
            SkillPoint2t = aChar.SkillPoint2T,
            SkillPoint3 = aChar.SkillPoint3,
            Gold = aChar.Gold,
            CExp = aChar.CEXP,
            JExp = aChar.JEXP,
            SaveMap = aChar.SaveMap,
            SaveX = aChar.SaveX,
            SaveY = aChar.SaveY,
            PossessionTarget = aChar.PossessionTarget,
            QuestId = questid,
            QuestEndTime = questRemainTime,
            QuestStatus = (byte)status,
            QuestCurrentCount1 = count1,
            QuestCurrentCount2 = count2,
            QuestCurrentCount3 = count3,
            QuestResetTime = aChar.QuestNextResetTime,
            Fame = aChar.Fame,
            Party = partyId,
            Ring = ringId,
            Golem = golemId,
            Cp = aChar.CP,
            ECoin = aChar.ECoin,
            JointJobLv = aChar.JointJobLevel,
            JointJobExp = aChar.JointJEXP,
            Wrp = aChar.WRP,
            Ep = aChar.EP,
            EpLoginDate = aChar.EPLoginTime,
            EpGreetingDate = aChar.EPGreetingTime,
            Cl = aChar.CL,
            EpUsed = aChar.EPUsed,
            TailStyle = aChar.TailStyle,
            WingStyle = aChar.WingStyle,
            WingColor = aChar.WingColor,
            Lv1 = aChar.Level1,
            JobLv3 = aChar.JobLevel3,
            ExplorerExp = aChar.ExplorerEXP,
            UsingPaperId = aChar.UsingPaperID,
            TitleId = aChar.PlayerTitleID,
            AbyssFloor = aChar.AbyssFloor,
            DualJobId = aChar.DualJobID,
            ExStatPoint = aChar.EXStatPoint,
            ExSkillPoint = aChar.EXSkillPoint,
            Online = false,
        }).ExecuteCommand();


        SqlSugarHelper.Db.CommitTran();
        return true;
    }

    public static uint CreateCharacter(ActorPC aChar, uint accountId) {
        Character character = new Character();

        character.AccountId = accountId;

        character.Name = aChar.Name;
        character.Race = (byte)aChar.Race;
        character.Gender = (byte)aChar.Gender;
        character.HairStyle = aChar.HairStyle;
        character.HairColor = aChar.HairColor;
        character.Wig = aChar.Wig;
        character.Face = aChar.Face;
        character.Job = (byte)aChar.Job;
        character.MapId = aChar.MapID;
        character.Lv = aChar.Level;
        character.JobLv1 = aChar.JobLevel1;
        character.JobLv2x = aChar.JobLevel2X;

        character.JobLv2t = aChar.JobLevel2T;

        character.QuestRemaining = aChar.QuestRemaining;
        character.Slot = aChar.Slot;
        character.X = aChar.X2;
        character.Y = aChar.Y2;
        character.Dir = (byte)(aChar.Dir / 45);
        character.Hp = aChar.HP;
        character.MaxHp = aChar.MaxHP;
        character.Mp = aChar.MP;
        character.MaxMp = aChar.MaxMP;
        character.Sp = aChar.SP;
        character.MaxSp = aChar.MaxSP;
        character.Str = aChar.Str;
        character.Dex = aChar.Dex;
        character.Int = aChar.Int;
        character.Vit = aChar.Vit;
        character.Agi = aChar.Agi;
        character.Mag = aChar.Mag;
        character.StatsPoint = aChar.StatsPoint;
        character.SkillPoint = aChar.SkillPoint;
        character.SkillPoint2x = aChar.SkillPoint2X;
        character.SkillPoint2t = aChar.SkillPoint2T;
        character.Gold = aChar.Gold;
        character.Ep = aChar.EP;
        character.EpLoginDate = aChar.EPLoginTime;
        character.TailStyle = aChar.TailStyle;
        character.WingStyle = aChar.WingStyle;
        character.WingColor = aChar.WingColor;
        character.Lv1 = aChar.Level1;
        character.JobLv3 = aChar.JobLevel3;
        character.SkillPoint3 = aChar.SkillPoint3;
        character.ExplorerExp = aChar.ExplorerEXP;


        return SqlSugarHelper.Db.Insertable<Character>(character).ExecuteReturnEntity().CharacterId;
    }

    public static bool AjiClear() {
        try {
            SqlSugarHelper.Db.BeginTran();
            foreach (var character in SqlSugarHelper.Db.Queryable<Character>().TranLock(DbLockType.Error).ToList()) {
                character.Lv = 1;
                character.CExp = 0;
                SqlSugarHelper.Db.Updateable<Character>(character).ExecuteCommand();
            }

            SqlSugarHelper.Db.CommitTran();
            return true;
        }
        catch (Exception ex) {
            SqlSugarHelper.Db.RollbackTran();
            SagaLib.Logger.ShowError(ex);
            return false;
        }
    }
}