using System;
using SagaDB.Actor;
using SagaDB.Entities;
using SqlSugar;

namespace SagaDB.Repository;

public class CharacterRepository {
    public static bool SaveCharacter(ActorPC aChar, uint mapId, byte x, byte y, uint questid, DateTime questRemainTime,
        byte status,
        int count1, int count2, int count3, uint partyId, uint ringId, uint golemId) {
        try {
            SqlSugarHelper.Db.BeginTran();

            foreach (var character in SqlSugarHelper.Db.Queryable<Character>().TranLock(DbLockType.Wait)
                         .Where(item => item.CharacterId == aChar.CharID).ToList()) {
                character.Name = (aChar.Name);
                character.Race = (byte)aChar.Race;
                character.Gender = (byte)aChar.Gender;
                character.HairStyle = aChar.HairStyle;
                character.HairColor = aChar.HairColor;
                character.Wig = aChar.Wig;
                character.Face = aChar.Face;
                character.Job = (byte)aChar.Job;
                character.MapId = mapId;
                character.Lv = aChar.Level;
                character.JobLv1 = aChar.JobLevel1;
                character.JobLv2x = aChar.JobLevel2X;
                character.JobLv2t = aChar.JobLevel2T;
                character.QuestRemaining = aChar.QuestRemaining;
                character.Slot = aChar.Slot;
                character.X = x;
                character.Y = y;
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
                character.SkillPoint3 = aChar.SkillPoint3;
                character.Gold = aChar.Gold;
                character.CExp = aChar.CEXP;
                character.JExp = aChar.JEXP;
                character.SaveMap = aChar.SaveMap;
                character.SaveX = aChar.SaveX;
                character.SaveY = aChar.SaveY;
                character.PossessionTarget = aChar.PossessionTarget;
                character.QuestId = questid;
                character.QuestEndTime = questRemainTime;
                character.QuestStatus = (byte)status;
                character.QuestCurrentCount1 = count1;
                character.QuestCurrentCount2 = count2;
                character.QuestCurrentCount3 = count3;
                character.QuestResetTime = aChar.QuestNextResetTime;
                character.Fame = aChar.Fame;
                character.Party = partyId;
                character.Ring = ringId;
                character.Golem = golemId;
                character.Cp = aChar.CP;
                character.ECoin = aChar.ECoin;
                character.JointJobLv = aChar.JointJobLevel;
                character.JointJobExp = aChar.JointJEXP;
                character.Wrp = aChar.WRP;
                character.Ep = aChar.EP;
                character.EpLoginDate = aChar.EPLoginTime;
                character.EpGreetingDate = aChar.EPGreetingTime;
                character.Cl = aChar.CL;
                character.EpUsed = aChar.EPUsed;
                character.TailStyle = aChar.TailStyle;
                character.WingStyle = aChar.WingStyle;
                character.WingColor = aChar.WingColor;
                character.Lv1 = aChar.Level1;
                character.JobLv3 = aChar.JobLevel3;
                character.ExplorerExp = aChar.ExplorerEXP;
                character.UsingPaperId = aChar.UsingPaperID;
                character.TitleId = aChar.PlayerTitleID;
                character.AbyssFloor = aChar.AbyssFloor;
                character.DualJobId = aChar.DualJobID;
                character.ExStatPoint = aChar.EXStatPoint;
                character.ExSkillPoint = aChar.EXSkillPoint;
                character.Online = false;

                SqlSugarHelper.Db.Updateable<Character>(character).ExecuteCommand();
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

    public static uint createCharacter(ActorPC aChar, uint accountId) {
        try {
            SqlSugarHelper.Db.BeginTran();


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


            uint charID = SqlSugarHelper.Db.Insertable<Character>(character).ExecuteReturnEntity().CharacterId;

            SqlSugarHelper.Db.CommitTran();
            return charID;
        }
        catch (Exception ex) {
            SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            return 0;
        }
    }

    public static bool AjiClear() {
        try {
            SqlSugarHelper.Db.BeginTran();
            foreach (var character in SqlSugarHelper.Db.Queryable<Character>().TranLock(DbLockType.Wait).ToList()) {
                character.Lv = 1;
                character.CExp = 0;
                SqlSugarHelper.Db.Updateable<Character>(character).ExecuteCommand();
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