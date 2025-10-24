using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using MySql.Data.MySqlClient;
using SagaDB.Actor;
using SagaDB.BBS;
using SagaDB.Entities;
using SagaDB.FlyingGarden;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Mob;
using SagaDB.Partner;
using SagaDB.Quests;
using SagaDB.Repository;
using SagaDB.Skill;
using SagaDB.Tamaire;
using SagaLib;
using SqlSugar;
using FurniturePlace = SagaDB.FlyingCastle.FurniturePlace;
using Inventory = SagaDB.Entities.Inventory;
using Logger = Serilog.Core.Logger;
using TamaireLending = SagaDB.Tamaire.TamaireLending;
using TamaireRental = SagaDB.Tamaire.TamaireRental;

namespace SagaDB {
    public class MySqlActorDb : MySQLConnectivity, ActorDB {
        public void AJIClear() {
            CharacterRepository.AjiClear();
        }

        public bool CreateChar(ActorPC aChar, int accountId) {
            if (aChar == null) {
                SagaLib.Logger.GetLogger().Error("aChar is null");
                return false;
            }

            //Map.MapInfo info = Map.MapInfoFactory.Instance.MapInfo[aChar.MapID];


            try {
                SqlSugarHelper.Db.BeginTran();

                aChar.CharID = CharacterRepository.createCharacter(aChar, (uint)accountId);


                SqlSugarHelper.Db.Insertable<Inventory>(new Inventory {
                    CharacterId = aChar.CharID,
                    Data = aChar.Inventory.ToBytes()
                }).ExecuteCommand();

                if (aChar.Inventory.WareHouse != null) {
                    if (SqlSugarHelper.Db.Queryable<Warehouse>().Where(item => item.AccountId == accountId).ToList()
                            .Count == 0) {
                        SqlSugarHelper.Db.Insertable<Warehouse>(new Warehouse {
                            AccountId = accountId,
                            Data = aChar.Inventory.WareToBytes()
                        });
                    }
                }

                SqlSugarHelper.Db.CommitTran();


                aChar.Inventory.WareHouse = null;
                //to avoid deleting items from warehouse
                SaveItem(aChar);
                return true;
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public uint CreatePartner(Item.Item partnerItem) {
            var ap = new ActorPartner(partnerItem.BaseData.petID, partnerItem);
            ap.HP = ap.BaseData.hp_in;
            ap.MaxHP = ap.BaseData.hp_in;
            ap.MP = ap.BaseData.mp_in;
            ap.MaxMP = ap.BaseData.mp_in;
            ap.SP = ap.BaseData.sp_in;
            ap.MaxSP = ap.BaseData.sp_in;
            ap.ai_mode = 1;

            try {
                SqlSugarHelper.Db.BeginTran();

                ap.ActorPartnerID = SqlSugarHelper.Db.Insertable<Entities.Partner>(new Entities.Partner {
                    PartnerId = ap.partnerid,
                    Name = ap.BaseData.name,
                    Level = ap.Level,
                    TrustLevel = ap.reliability, Rebirth = 0, Rank = ap.rank, PerkPoints = ap.perkpoint,
                    Perk0 = ap.perk0,
                    Perk1 = ap.perk1, Perk2 = ap.perk2, Perk3 = ap.perk3, Perk4 = ap.perk4, Perk5 = ap.perk5,
                    AiMode = ap.ai_mode, BasicAiMode1 = ap.basic_ai_mode, BasicAiMode2 = ap.basic_ai_mode_2, Hp = ap.HP,
                    MaxHp = ap.MaxHP, Mp = ap.MP, MaxMp = ap.MaxMP, Sp = ap.SP, MaxSp = ap.MaxSP
                }).ExecuteReturnEntity().ActorPartnerId;

                SqlSugarHelper.Db.CommitTran();
                return ap.ActorPartnerID;
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return 0;
            }
        }

        public void SavePartner(ActorPartner ap) {
            if (ap == null) {
                return;
            }

            //SagaLib.Logger.getLogger().Error(sqlstr);
            try {
                SqlSugarHelper.Db.BeginTran();


                foreach (var partner in SqlSugarHelper.Db.Queryable<Entities.Partner>().TranLock(DbLockType.Wait)
                             .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
                    partner.PartnerId = ap.partnerid;
                    partner.Name = ap.Name;
                    partner.Level = ap.Level;
                    partner.TrustLevel = ap.reliability;
                    partner.Rebirth = ap.rebirth ? (byte)1 : (byte)0;
                    partner.Rank = ap.rank;
                    partner.PerkPoints = ap.perkpoint;
                    partner.Hp = ap.HP;
                    partner.MaxHp = ap.MaxHP;
                    partner.Mp = ap.MP;
                    partner.MaxMp = ap.MaxMP;
                    partner.Sp = ap.SP;
                    partner.MaxSp = ap.MaxSP;
                    partner.Perk0 = ap.perk0;
                    partner.Perk1 = ap.perk1;
                    partner.Perk2 = ap.perk2;
                    partner.Perk3 = ap.perk3;
                    partner.Perk4 = ap.perk4;
                    partner.Perk5 = ap.perk5;
                    partner.AiMode = ap.ai_mode;
                    partner.BasicAiMode1 = ap.basic_ai_mode;
                    partner.BasicAiMode2 = ap.basic_ai_mode_2;
                    partner.Exp = ap.exp;
                    partner.PictId = ap.PictID;
                    partner.NextFeedTime = ap.nextfeedtime;
                    partner.ReliabilityUprate = ap.reliabilityuprate;
                    partner.TrustExp = ap.reliabilityexp;

                    SqlSugarHelper.Db.Updateable(partner).ExecuteCommand();
                }

                /*SavePartnerEquip(ap);
                SavePartnerCube(ap);
                SavePartnerAI(ap);*/
                //暂时注释防止卡死
                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerEquip(ActorPartner ap) {
            string sqlstr;
            if (ap == null) {
                return;
            }

            var apid = ap.ActorPartnerID;

            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var equip in SqlSugarHelper.Db.Queryable<Entities.PartnerEquip>().TranLock(DbLockType.Wait)
                             .Where(item => item.ActorPartnerId == apid).ToList()) {
                    SqlSugarHelper.Db.Deleteable(equip).ExecuteCommand();
                }

                if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.COSTUME)) {
                    SqlSugarHelper.Db.Insertable(new PartnerEquip {
                        ActorPartnerId = apid, Type = 1, ItemId = ap.equipments[EnumPartnerEquipSlot.COSTUME].ItemID,
                        Count = ap.equipments[EnumPartnerEquipSlot.COSTUME].Stack
                    }).ExecuteCommand();
                }

                if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.WEAPON)) {
                    SqlSugarHelper.Db.Insertable(new PartnerEquip {
                        ActorPartnerId = apid, Type = 2, ItemId = ap.equipments[EnumPartnerEquipSlot.WEAPON].ItemID,
                        Count = ap.equipments[EnumPartnerEquipSlot.WEAPON].Stack
                    }).ExecuteCommand();
                }

                for (var i = 0; i < ap.foods.Count; i++) {
                    SqlSugarHelper.Db.Insertable(new PartnerEquip {
                        ActorPartnerId = apid, Type = 3, ItemId = ap.foods[i].ItemID,
                        Count = ap.foods[i].Stack
                    }).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerCube(ActorPartner ap) {
            string sqlstr;
            if (ap == null) {
                return;
            }

            var apid = ap.ActorPartnerID;


            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var partnerCube in SqlSugarHelper.Db.Queryable<PartnerCube>().TranLock(DbLockType.Wait)
                             .Where(item => item.ActorPartnerId == apid).ToList()) {
                    SqlSugarHelper.Db.Deleteable(partnerCube);
                }


                for (var i = 0; i < ap.equipcubes_condition.Count; i++) {
                    SqlSugarHelper.Db.Insertable(new PartnerCube {
                        ActorPartnerId = apid, Type = 1, UniqueId = ap.equipcubes_condition[i]
                    }).ExecuteCommand();
                }

                for (var i = 0; i < ap.equipcubes_action.Count; i++) {
                    SqlSugarHelper.Db.Insertable(new PartnerCube {
                        ActorPartnerId = apid, Type = 2, UniqueId = ap.equipcubes_action[i]
                    }).ExecuteCommand();
                }

                for (var i = 0; i < ap.equipcubes_activeskill.Count; i++) {
                    SqlSugarHelper.Db.Insertable(new PartnerCube {
                        ActorPartnerId = apid, Type = 3, UniqueId = ap.equipcubes_activeskill[i]
                    }).ExecuteCommand();
                }

                for (var i = 0; i < ap.equipcubes_passiveskill.Count; i++) {
                    SqlSugarHelper.Db.Insertable(new PartnerCube {
                        ActorPartnerId = apid, Type = 4, UniqueId = ap.equipcubes_passiveskill[i]
                    }).ExecuteCommand();
                }


                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerAI(ActorPartner ap) {
            if (ap == null) {
                return;
            }


            try {
                SqlSugarHelper.Db.BeginTran();

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

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public ActorPartner GetActorPartner(uint actorPartnerId, Item.Item partneritem) {
            var result = SqlSugarHelper.Db.Queryable<Entities.Partner>()
                .Where(item => item.ActorPartnerId == actorPartnerId).ToList();
            if (result.Count == 0) {
                return null;
            }


            var partnerid = (uint)result[0].PartnerId;
            var ap = new ActorPartner(partnerid, partneritem);
            ap.ActorPartnerID = actorPartnerId;
            ap.Name = (string)result[0].Name;
            ap.Level = (byte)result[0].Level;
            ap.reliability = (byte)result[0].TrustLevel;
            ap.reliabilityexp = (ulong)result[0].TrustExp;
            ap.rebirth = result[0].Rebirth != 0;
            ap.rank = (byte)result[0].Rank;
            ap.perkpoint = (ushort)result[0].PerkPoints;
            ap.HP = (uint)result[0].Hp;
            ap.MaxHP = (uint)result[0].MaxHp;
            ap.MP = (uint)result[0].Mp;
            ap.MaxMP = (uint)result[0].MaxMp;
            ap.SP = (uint)result[0].Sp;
            ap.MaxSP = (uint)result[0].MaxSp;
            ap.perk0 = (byte)result[0].Perk0;
            ap.perk1 = (byte)result[0].Perk1;
            ap.perk2 = (byte)result[0].Perk2;
            ap.perk3 = (byte)result[0].Perk3;
            ap.perk4 = (byte)result[0].Perk4;
            ap.perk5 = (byte)result[0].Perk5;
            ap.ai_mode = (byte)result[0].AiMode;
            ap.basic_ai_mode = (byte)result[0].BasicAiMode1;
            ap.basic_ai_mode_2 = (byte)result[0].BasicAiMode2;
            ap.exp = (ulong)result[0].Exp;
            ap.nextfeedtime = (DateTime)result[0].NextFeedTime;
            ap.reliabilityuprate = (ushort)result[0].ReliabilityUprate;

            GetPartnerEquip(ap);
            GetPartnerCube(ap);
            GetPartnerAI(ap);
            return ap;
            //暂时注释防止卡死
        }

        public void GetPartnerEquip(ActorPartner ap) {
            foreach (var i in SqlSugarHelper.Db.Queryable<Entities.PartnerEquip>()
                         .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
                var item = ItemFactory.Instance.GetItem(i.ItemId);
                item.Stack = i.Count;
                if (i.Type == 1)
                    ap.equipments[EnumPartnerEquipSlot.COSTUME] = item;
                if (i.Type == 2)
                    ap.equipments[EnumPartnerEquipSlot.WEAPON] = item;
                if (i.Type == 3)
                    ap.foods.Add(item);
            }
        }

        public void GetPartnerCube(ActorPartner ap) {
            foreach (var i in SqlSugarHelper.Db.Queryable<PartnerCube>()
                         .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
                if (i.Type == 1)
                    ap.equipcubes_condition.Add(i.UniqueId);
                if (i.Type == 2)
                    ap.equipcubes_action.Add(i.UniqueId);
                if (i.Type == 3)
                    ap.equipcubes_activeskill.Add(i.UniqueId);
                if (i.Type == 4)
                    ap.equipcubes_passiveskill.Add(i.UniqueId);
            }
        }

        public void GetPartnerAI(ActorPartner ap) {
            foreach (var i in SqlSugarHelper.Db.Queryable<PartnerAi>()
                         .Where(item => item.ActorPartnerId == ap.ActorPartnerID).ToList()) {
                if ((byte)i.Type == 1) ap.ai_conditions.Add((byte)i.Index, (ushort)i.Value);
                if ((byte)i.Type == 2) ap.ai_reactions.Add((byte)i.Index, (ushort)i.Value);
                if ((byte)i.Type == 3) ap.ai_intervals.Add((byte)i.Index, (ushort)i.Value);
                if ((byte)i.Type == 4) ap.ai_states.Add((byte)i.Index, Convert.ToBoolean((ushort)i.Value));
            }
        }

        public void SaveChar(ActorPC aChar) {
            SaveChar(aChar, true);
        }

        public void SaveChar(ActorPC aChar, bool fullinfo) {
            SaveChar(aChar, true, fullinfo);
        }

        public void SaveChar(ActorPC aChar, bool itemInfo, bool fullinfo) {
            string sqlstr;
            MapInfo info = null;
            if (MapInfoFactory.Instance.MapInfo.ContainsKey(aChar.MapID)) {
                info = MapInfoFactory.Instance.MapInfo[aChar.MapID];
            }
            else {
                if (MapInfoFactory.Instance.MapInfo.ContainsKey(aChar.MapID / 1000 * 1000)) {
                    info = MapInfoFactory.Instance.MapInfo[aChar.MapID / 1000 * 1000];
                }
            }

            if (aChar == null) {
                return;
            }

            uint questid = 0;
            uint partyid = 0;
            uint ringid = 0;
            uint golemid = 0;
            uint mapid = 0;
            byte x = 0, y = 0;
            int count1 = 0, count2 = 0, count3 = 0;
            var questtime = DateTime.Now;
            var status = QuestStatus.OPEN;
            if (aChar.Quest != null) {
                questid = aChar.Quest.ID;
                count1 = aChar.Quest.CurrentCount1;
                count2 = aChar.Quest.CurrentCount2;
                count3 = aChar.Quest.CurrentCount3;
                questtime = aChar.Quest.EndTime;
                status = aChar.Quest.Status;
            }

            if (aChar.Party != null)
                partyid = aChar.Party.ID;
            if (aChar.Ring != null)
                ringid = aChar.Ring.ID;
            if (aChar.Golem != null)
                golemid = aChar.Golem.ActorID;
            if (info != null) {
                mapid = aChar.MapID;
                x = Global.PosX16to8(aChar.X, info.width);
                y = Global.PosY16to8(aChar.Y, info.height);
            }
            else {
                mapid = aChar.SaveMap;
                x = aChar.SaveX;
                y = aChar.SaveY;
            }

            CharacterRepository.SaveCharacter(aChar, mapid, x, y, questid, questtime, (byte)status,
                count1, count2, count3, partyid, ringid, golemid);

            SaveVar(aChar);

            SavePaper(aChar);
            SaveFGarden(aChar);
            //SaveFlyCastle(aChar.Ring);
            //SaveNavi(aChar);
            if (itemInfo)
                SaveItem(aChar);
            if (fullinfo) {
                SaveSkill(aChar);
                SaveDualJobInfo(aChar, true);
                //SaveNPCStates(aChar);
            }

            SaveQuestInfo(aChar);
            SaveStamps(aChar);
        }

        /*
        public void SaveNavi(ActorPC pc)
        {
            string sqlstr = string.Format("DELETE FROM `navi` WHERE `account_id`='{0}';", pc.Account.AccountID);
            foreach (SagaDB.Navi.Category c in pc.Navi.Categories.Values)
            {
                foreach (SagaDB.Navi.Event e in c.Events.Values)
                {
                    foreach (SagaDB.Navi.Step s in e.Steps.Values)
                    {
                        if (e.Show)
                            sqlstr += string.Format("INSERT INTO `navi`(`account_id`,`CategoryID`,`EventID`,`StepID`,`EventState`,`StepDisplay`, `StepFinished`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}');", pc.Account.AccountID,
                                c.ID, e.ID, s.ID, e.State, s.Display, s.Finished);
                    }
                }
            }
            try
            {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex)
            {
                Logger.getLogger().Error(ex, ex.Message);
            }
        }
        public void GetNavi(ActorPC pc)
        {
            string sqlstr;
            DataRow result = null;
            try
            {
                sqlstr = "SELECT * FROM `navi` WHERE `anncount_id`='" + pc.Account.AccountID + "' LIMIT 1";
                try
                {
                    result = SQLExecuteQuery(sqlstr)[0];
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                    return;
                }
                uint categoryId, eventId, stepId; ;

                categoryId = (uint)result["CategoryID"];
                eventId = (uint)result["EventID"];
                stepId = (uint)result["StepID"];
                pc.Navi.Categories[categoryId].Events[eventId].State = (byte)result["EventState"];
                pc.Navi.Categories[categoryId].Events[eventId].Steps[stepId].Display = (bool)result["StepDisplay"];
                pc.Navi.Categories[categoryId].Events[eventId].Steps[stepId].Finished = (bool)result["StepFinished"];
            }
            catch (Exception ex)
            {
                Logger.getLogger().Error(ex, ex.Message);
                return;
            }
        }
        */
        public void SaveWRP(ActorPC pc) {
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var character in SqlSugarHelper.Db.Queryable<Character>()
                             .Where(item => item.CharacterId == pc.CharID).ToList()) {
                    character.Wrp = pc.WRP;
                    SqlSugarHelper.Db.Updateable<Character>(character).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void DeleteChar(ActorPC aChar) {
            string sqlstr = "";
            _ = GetAccountID((aChar.CharID));


            try {
                SqlSugarHelper.Db.BeginTran();

                SqlSugarHelper.Db.Deleteable<Entities.Character>(new Entities.Character { CharacterId = aChar.CharID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.Deleteable<Entities.Inventory>(new Entities.Inventory { CharacterId = aChar.CharID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.Deleteable<Entities.Skill>(new Entities.Skill { CharacterId = aChar.CharID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.Deleteable<Entities.CharacterVariable>(new Entities.CharacterVariable
                        { CharacterId = aChar.CharID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.Deleteable<Entities.Friend>(new Entities.Friend { CharacterId = aChar.CharID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.Deleteable<Entities.Friend>(new Entities.Friend { FriendCharacterId = aChar.CharID })
                    .ExecuteCommand();
                if (aChar.Party != null)
                    if (aChar.Party.Leader != null)
                        if (aChar.Party.Leader.CharID == aChar.CharID)
                            DeleteParty(aChar.Party);

                if (aChar.Ring != null)
                    if (aChar.Ring.Leader != null)
                        if (aChar.Ring.Leader.CharID == aChar.CharID)
                            DeleteRing(aChar.Ring);

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public ActorPC GetChar(uint charId, bool fullinfo) {
            ActorPC pc = null;
            try {
                // GetAccountID(charID);
                var character = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == charId)
                    .First();
                if (character == null) {
                    return null;
                }

                pc = new ActorPC {
                    CharID = charId,
                    Account = null,
                    Name = character.Name,
                    Race = (PC_RACE)character.Race,
                    UsingPaperID = character.UsingPaperId,
                    PlayerTitleID = character.TitleId,
                    Gender = (PC_GENDER)character.Gender,
                    TailStyle = character.TailStyle,
                    WingStyle = character.WingStyle,
                    WingColor = character.WingColor,
                    HairStyle = character.HairStyle,
                    HairColor = character.HairColor,
                    Wig = character.Wig,
                    Face = character.Face,
                    Job = (PC_JOB)character.Job,
                    MapID = character.MapId,
                    Level = character.Lv,
                    Level1 = character.Lv1,
                    JobLevel1 = character.JobLv1,
                    JobLevel2X = character.JobLv2x,
                    JobLevel2T = character.JobLv2t,
                    JobLevel3 = character.JobLv3,
                    JointJobLevel = character.JointJobLv,
                    QuestRemaining = character.QuestRemaining,
                    QuestNextResetTime = character.QuestResetTime,
                    Fame = character.Fame,
                    Slot = character.Slot
                };

                if (fullinfo) {
                    MapInfo info = null;
                    if (MapInfoFactory.Instance.MapInfo.ContainsKey(pc.MapID)) {
                        info = MapInfoFactory.Instance.MapInfo[pc.MapID];
                    }
                    else {
                        if (MapInfoFactory.Instance.MapInfo.ContainsKey(pc.MapID / 1000 * 1000))
                            info = MapInfoFactory.Instance.MapInfo[pc.MapID / 1000 * 1000];
                    }

                    pc.X = Global.PosX8to16(character.X, info.width);
                    pc.Y = Global.PosY8to16(character.Y, info.height);
                }

                pc.Dir = (ushort)(character.Dir * 45);

                pc.SaveMap = character.SaveMap;
                pc.SaveX = character.SaveX;
                pc.SaveY = character.SaveY;
                pc.HP = character.Hp;
                pc.MP = character.Mp;
                pc.SP = character.Sp;
                pc.MaxHP = character.MaxHp;
                pc.MaxMP = character.MaxMp;
                pc.MaxSP = character.MaxSp;
                pc.EP = character.Ep;
                pc.EPLoginTime = character.EpLoginDate;
                pc.EPGreetingTime = character.EpGreetingDate;
                pc.EPUsed = character.EpUsed;
                pc.CL = character.Cl;
                pc.Str = character.Str;
                pc.Dex = character.Dex;
                pc.Int = character.Int;
                pc.Vit = character.Vit;
                pc.Agi = character.Agi;
                pc.Mag = character.Mag;
                pc.StatsPoint = character.StatsPoint;
                pc.SkillPoint = character.SkillPoint;
                pc.SkillPoint2X = character.SkillPoint2x;
                pc.SkillPoint2T = character.SkillPoint2t;
                pc.SkillPoint3 = character.SkillPoint3;
                pc.EXStatPoint = character.ExStatPoint;
                pc.EXSkillPoint = character.ExSkillPoint;
                //pc.DefLv = (byte)result["deflv"];
                //pc.MDefLv = (byte)result["mdeflv"];
                //pc.DefPoint = (uint)result["defpoint"];
                //pc.MDefPoint = (uint)result["mdefpoint"];
                lock (this) {
                    var old = SagaLib.Logger.SQLLogLevel.Value;
                    SagaLib.Logger.SQLLogLevel.Value = 0;
                    pc.Gold = character.Gold;
                    SagaLib.Logger.SQLLogLevel.Value = old;
                }

                pc.CP = character.Cp;
                pc.ECoin = character.ECoin;
                pc.CEXP = character.CExp;
                //pc.JEXP = (ulong)result["jexp"];
                pc.JointJEXP = character.JointJobExp;
                pc.ExplorerEXP = character.ExplorerExp;
                pc.WRP = character.Wrp;
                pc.PossessionTarget = character.PossessionTarget;
                pc.AbyssFloor = character.AbyssFloor;
                var party = new Party.Party();
                party.ID = character.Party;
                var ring = new Ring.Ring();
                ring.ID = character.Ring;
                if (party.ID != 0)
                    pc.Party = party;
                if (ring.ID != 0)
                    pc.Ring = ring;
                var golem = character.Golem;
                if (golem != 0) {
                    pc.Golem = new ActorGolem();
                    pc.Golem.ActorID = golem;
                }

                pc.DualJobID = character.DualJobId == null ? (byte)0 : character.DualJobId;

                if (fullinfo) {
                    var questid = character.QuestId;
                    if (questid != 0) {
                        try {
                            var quest = new Quest(questid);
                            quest.EndTime = character.QuestEndTime;
                            quest.Status = (QuestStatus)character.QuestStatus;
                            quest.CurrentCount1 = character.QuestCurrentCount1;
                            quest.CurrentCount2 = character.QuestCurrentCount2;
                            quest.CurrentCount3 = character.QuestCurrentCount3;
                            pc.Quest = quest;
                        }
                        catch (Exception e) {
                            SagaLib.Logger.ShowError(e);
                        }
                    }

                    GetSkill(pc);
                    GetJobLV(pc);
                    GetNPCStates(pc);
                    GetFlyingGarden(pc);
                    GetVar(pc);
                    //GetNavi(pc);
                    //GetFlyCastle(pc);
                }

                GetPaper(pc);
                GetQuestInfo(pc);
                GetItem(pc);
                GetVShop(pc);
                GetStamps(pc);
                //GetGifts(pc);
                //GetMail(pc);
                GetTamaireLending(pc);
                GetTamaireRental(pc);
                GetMosterGuide(pc);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }

            return pc;
        }

        public ActorPC GetChar(uint charID) {
            return GetChar(charID, true);
        }

        public void GetVShop(ActorPC pc) {
            var account = GetAccountID(pc);
            var records = SqlSugarHelper.Db.Queryable<Login>().Where(item => item.AccountId == account).ToList();
            if (records.Count != 0) {
                var result = records[0];
                var eh = pc.e;
                pc.e = null;
                pc.VShopPoints = (uint)result.VShopPoints;
                pc.e = eh;
                pc.UsedVShopPoints = (uint)result.UsedVShopPoints;
            }
        }

        public void SaveSkill(ActorPC pc) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var count = pc.Skills.Count + pc.Skills2.Count + pc.SkillsReserve.Count;
            if (pc.Rebirth || pc.Job == pc.Job3)
                count = pc.Skills.Count + pc.Skills2_1.Count + pc.Skills2_2.Count + pc.Skills3.Count;
            else
                count = pc.Skills.Count + pc.Skills2.Count + pc.SkillsReserve.Count;
            var nosave = 0;
            foreach (var i in pc.Skills.Values)
                if (i.NoSave)
                    nosave++;
            if (pc.Rebirth || pc.Job == pc.Job3) {
                foreach (var i in pc.Skills2_1.Values)
                    if (i.NoSave)
                        nosave++;
                foreach (var i in pc.Skills2_2.Values)
                    if (i.NoSave)
                        nosave++;
                foreach (var i in pc.Skills3.Values)
                    if (i.NoSave)
                        nosave++;
            }
            else {
                foreach (var i in pc.Skills2.Values)
                    if (i.NoSave)
                        nosave++;
                foreach (var i in pc.SkillsReserve.Values)
                    if (i.NoSave)
                        nosave++;
            }

            count -= nosave;
            bw.Write(count);
            foreach (var j in pc.Skills.Keys) {
                if (pc.Skills[j].NoSave)
                    continue;
                bw.Write(j);
                bw.Write(pc.Skills[j].Level);
            }

            if (pc.Rebirth || pc.Job == pc.Job3) {
                foreach (var j in pc.Skills2_1.Keys) {
                    if (pc.Skills2_1[j].NoSave)
                        continue;
                    bw.Write(j);
                    bw.Write(pc.Skills2_1[j].Level);
                }

                foreach (var j in pc.Skills2_2.Keys) {
                    if (pc.Skills2_2[j].NoSave)
                        continue;
                    bw.Write(j);
                    bw.Write(pc.Skills2_2[j].Level);
                }

                foreach (var j in pc.Skills3.Keys) {
                    if (pc.Skills3[j].NoSave)
                        continue;
                    bw.Write(j);
                    bw.Write(pc.Skills3[j].Level);
                }
            }
            else {
                foreach (var j in pc.Skills2.Keys) {
                    if (pc.Skills2[j].NoSave)
                        continue;
                    bw.Write(j);
                    bw.Write(pc.Skills2[j].Level);
                }

                foreach (var j in pc.SkillsReserve.Keys) {
                    if (pc.SkillsReserve[j].NoSave)
                        continue;
                    bw.Write(j);
                    bw.Write(pc.SkillsReserve[j].Level);
                }
            }

            ms.Flush();
            //MySqlCommand cmd = new MySqlCommand(string.Format("REPLACE INTO `skill`(`char_id`,`skills`) VALUES ('{0}',?data);", pc.CharID));
            //string sqlstr = string.Format("DELETE FROM `skill` WHERE `char_id`='{0}' AND `jobbasic`='{1}';", pc.CharID, (int)pc.JobBasic);

            var data = ms.ToArray();
            ms.Close();
            try {
                SqlSugarHelper.Db.BeginTran();


                SqlSugarHelper.Db.Storageable(new Entities.Skill {
                    Skills = data,
                    CharacterId = pc.CharID,
                    JobBasic = (int)pc.JobBasic,
                    JobLevel = pc.JobLevel3,
                    JobExp = pc.JEXP,
                    SkillPoint = pc.SkillPoint,
                    SkillPoint2X = pc.SkillPoint2X,
                    SkillPoint2T = pc.SkillPoint2T,
                    SkillPoint3 = pc.SkillPoint3
                }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SaveVShop(ActorPC pc) {
            var eh = pc.e;
            pc.e = null;
            foreach (var i in SqlSugarHelper.Db.Queryable<Login>().TranLock(DbLockType.Wait)
                         .Where(item => item.AccountId == pc.Account.AccountID).ToList()) {
                i.VShopPoints = pc.VShopPoints;
                i.UsedVShopPoints = pc.UsedVShopPoints;
                SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
            }

            pc.e = eh;
        }

        public void SaveServerVar(ActorPC fakepc) {
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var i in SqlSugarHelper.Db.Queryable<ServerVariable>().TranLock(DbLockType.Wait).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                foreach (var i in fakepc.AStr.Keys)
                    SqlSugarHelper.Db.Insertable<ServerVariable>(new ServerVariable {
                        Name = i, Type = 0, Content = fakepc.AStr[i]
                    }).ExecuteCommand();
                foreach (var i in fakepc.AInt.Keys)
                    SqlSugarHelper.Db.Insertable<ServerVariable>(new ServerVariable {
                        Name = i, Type = 1, Content = fakepc.AInt[i].ToString()
                    }).ExecuteCommand();

                foreach (var i in fakepc.AMask.Keys)
                    SqlSugarHelper.Db.Insertable<ServerVariable>(new ServerVariable {
                        Name = i, Type = 2, Content = fakepc.AMask[i].Value.ToString()
                    }).ExecuteCommand();


                foreach (var i in SqlSugarHelper.Db.Queryable<SettingList>().TranLock(DbLockType.Wait).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }


                foreach (var item in fakepc.Adict)
                    foreach (var i in item.Value.Keys)
                        SqlSugarHelper.Db.Insertable<SettingList>(new SettingList {
                            Name = item.Key, Key = i, Type = 1, Content = item.Value[i]
                        }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public ActorPC LoadServerVar() {
            var fakepc = new ActorPC();
            foreach (var i in SqlSugarHelper.Db.Queryable<ServerVariable>().ToList()) {
                switch (i.Type) {
                    case 0:
                        fakepc.AStr[(string)i.Name] = i.Content;
                        break;
                    case 1:
                        fakepc.AInt[(string)i.Name] = int.Parse(i.Content);
                        break;
                    case 2:
                        fakepc.AMask[i.Name] = new BitMask(int.Parse(i.Content));
                        break;
                }
            }

            foreach (var i in SqlSugarHelper.Db.Queryable<SettingList>().ToList()) {
                switch (i.Type) {
                    case 1:
                        fakepc.Adict[i.Name][i.Key] = i.Content;
                        break;
                }
            }

            return fakepc;
        }

        public void SaveVar(ActorPC pc) {
            var accountId = this.GetAccountID(pc.CharID);

            var enc = Encoding.UTF8;

            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(pc.CInt.Count);
            foreach (var j in pc.CInt.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                bw.Write(pc.CInt[j]);
            }

            bw.Write(pc.CMask.Count);
            foreach (var j in pc.CMask.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                bw.Write(pc.CMask[j].Value);
            }

            bw.Write(pc.CStr.Count);
            foreach (var j in pc.CStr.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                buf = enc.GetBytes(pc.CStr[j]);
                bw.Write(buf.Length);
                bw.Write(buf);
            }

            ms.Flush();
            byte[] data = ms.ToArray();
            ms.Close();
            try {
                SqlSugarHelper.Db.Storageable(new CharacterVariable { CharacterId = pc.CharID, Values = data })
                    .ExecuteCommand();
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
            finally {
                ms.Close();
            }

            ms = new MemoryStream();
            bw = new BinaryWriter(ms);
            bw.Write(pc.AInt.Count);
            foreach (var j in pc.AInt.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                bw.Write(pc.AInt[j]);
            }

            bw.Write(pc.AMask.Count);
            foreach (var j in pc.AMask.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                bw.Write(pc.AMask[j].Value);
            }

            bw.Write(pc.AStr.Count);
            foreach (var j in pc.AStr.Keys) {
                var buf = enc.GetBytes(j);
                bw.Write(buf.Length);
                bw.Write(buf);
                buf = enc.GetBytes(pc.AStr[j]);
                bw.Write(buf.Length);
                bw.Write(buf);
            }

            ms.Flush();

            byte[] values = ms.ToArray();
            ms.Close();
            AvatarRepository.SaveAvatar(accountId, values);
        }

        public void GetSkill(ActorPC pc) {
            try {
                foreach (var skillRecord in SqlSugarHelper.Db.Queryable<Entities.Skill>().Where(item =>
                             item.CharacterId == pc.CharID && item.JobBasic == (int)pc.JobBasic).ToList()) {
                    var buf = (byte[])skillRecord.Skills;
                    pc.JobLevel3 = (byte)skillRecord.JobLevel;
                    if (pc.JobLevel3 == 0) pc.JobLevel3 = 1;
                    pc.JEXP = (ulong)skillRecord.JobExp;
                    pc.SkillPoint = (ushort)skillRecord.SkillPoint;
                    pc.SkillPoint2X = (ushort)skillRecord.SkillPoint2X;
                    pc.SkillPoint2T = (ushort)skillRecord.SkillPoint2T;
                    pc.SkillPoint3 = (ushort)skillRecord.SkillPoint3;

                    var skills = SkillFactory.Instance.SkillList(pc.JobBasic);
                    var skills2x = SkillFactory.Instance.SkillList(pc.Job2X);
                    var skills2t = SkillFactory.Instance.SkillList(pc.Job2T);
                    var skills3 = SkillFactory.Instance.SkillList(pc.Job3);

                    var ms = new MemoryStream(buf);
                    var br = new BinaryReader(ms);
                    var count = br.ReadInt32();
                    for (var i = 0; i < count; i++) {
                        var skillID = br.ReadUInt32();
                        var lv = br.ReadByte();
                        var skill = SkillFactory.Instance.GetSkill(skillID, lv);
                        if (skill == null)
                            continue;
                        if (skills.ContainsKey(skill.ID)) {
                            if (!pc.Skills.ContainsKey(skill.ID))
                                pc.Skills.Add(skill.ID, skill);
                        }
                        else if (skills2x.ContainsKey(skill.ID)) {
                            if (!pc.Rebirth || pc.Job != pc.Job3) {
                                if (pc.Job == pc.Job2X) {
                                    if (!pc.Skills2.ContainsKey(skill.ID))
                                        pc.Skills2.Add(skill.ID, skill);
                                }
                                else {
                                    if (!pc.SkillsReserve.ContainsKey(skill.ID))
                                        pc.SkillsReserve.Add(skill.ID, skill);
                                }
                            }
                            else {
                                if (!pc.Skills2_1.ContainsKey(skill.ID))
                                    pc.Skills2_1.Add(skill.ID, skill);
                                if (!pc.Skills2.ContainsKey(skill.ID))
                                    pc.Skills2.Add(skill.ID, skill);
                            }
                        }
                        else if (skills2t.ContainsKey(skill.ID)) {
                            if (!pc.Rebirth || pc.Job != pc.Job3) {
                                if (pc.Job == pc.Job2T) {
                                    if (!pc.Skills2.ContainsKey(skill.ID))
                                        pc.Skills2.Add(skill.ID, skill);
                                }
                                else {
                                    if (!pc.SkillsReserve.ContainsKey(skill.ID))
                                        pc.SkillsReserve.Add(skill.ID, skill);
                                }
                            }
                            else {
                                if (!pc.Skills2_2.ContainsKey(skill.ID))
                                    pc.Skills2_2.Add(skill.ID, skill);
                                if (!pc.Skills2.ContainsKey(skill.ID))
                                    pc.Skills2.Add(skill.ID, skill);
                            }
                        }
                        else if (skills3.ContainsKey(skill.ID)) {
                            if (!pc.Skills3.ContainsKey(skill.ID))
                                pc.Skills3.Add(skill.ID, skill);
                        }
                        else {
                            if (!pc.Skills.ContainsKey(skill.ID))
                                pc.Skills.Add(skill.ID, skill);
                        }
                    }
                }
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return;
            }
        }

        public void SaveNPCState(ActorPC pc, uint npcId) {
            if (!pc.NPCStates.ContainsKey(npcId)) {
                return;
            }

            try {
                SqlSugarHelper.Db.BeginTran();
                bool isFound = false;
                foreach (var npcState in SqlSugarHelper.Db.Queryable<NpcStates>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == pc.CharID).Where(item => item.NpcId == npcId)
                             .ToList()) {
                    npcState.State = pc.NPCStates[npcId];
                    SqlSugarHelper.Db.Updateable(npcState).ExecuteCommand();
                    isFound = true;
                }

                if (!isFound) {
                    SqlSugarHelper.Db.Insertable(new NpcStates {
                        CharacterId = pc.CharID,
                        NpcId = npcId,
                        State = pc.NPCStates[npcId]
                    }).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public bool CharExists(string name) {
            return SqlSugarHelper.Db.Queryable<Character>().Where(item => item.Name.Equals((name))).Any();
        }

        public uint GetAccountID(uint charID) {
            var character = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == charID)
                .First();
            return character == null ? 0 : character.AccountId;
        }

        public uint GetAccountID(ActorPC pc) {
            if (pc.Account != null)
                return (uint)pc.Account.AccountID;
            return GetAccountID(pc.CharID);
        }

        public uint[] GetCharIDs(int account_id) {
            try {
                var characters = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.AccountId == account_id)
                    .ToList();
                if (characters.Count == 0) {
                    return new uint[0];
                }

                uint[] buf = new uint[characters.Count];
                for (var i = 0; i < buf.Length; i++) {
                    buf[i] = characters[i].CharacterId;
                }

                return buf;
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return new uint[0];
            }
        }

        public string GetCharName(uint id) {
            var characters = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == id)
                .ToList();
            if (characters.Count == 0)
                return null;
            return characters[0].Name;
        }

        public List<ActorPC> GetFriendList(ActorPC pc) {
            var list = new List<ActorPC>();

            var exp = Expressionable.Create<Friend>();
            exp.Or(item => item.CharacterId == pc.CharID);
            exp.Or(item => item.FriendCharacterId == pc.CharID);

            try {
                var result = SqlSugarHelper.Db.Queryable<Friend>()
                    .Where(exp.ToExpression()).ToList();

                for (var i = 0; i < result.Count; i++) {
                    var friend = (uint)result[i].CharacterId == pc.CharID
                        ? result[i].FriendCharacterId
                        : result[i].CharacterId;

                    if (friend == pc.CharID) {
                        continue;
                    }

                    var res = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == friend)
                        .ToList();

                    if (res.Count == 0)
                        continue;
                    var row = res[0];
                    list.Add(new ActorPC {
                        CharID = row.CharacterId,
                        Name = (string)row.Name,
                        Job = (PC_JOB)(byte)row.Job,
                        Level = (byte)row.Lv,
                        JobLevel1 = (byte)row.JobLv1,
                        JobLevel2X = (byte)row.JobLv2x,
                        JobLevel2T = (byte)row.JobLv2t
                    });
                }
            }
            catch (Exception e) {
                SagaLib.Logger.ShowError(e);
            }

            return list;
        }

        public List<ActorPC> GetFriendList2(ActorPC pc) {
            return GetFriendList(pc);
        }

        public void AddFriend(ActorPC pc, uint charID) {
            try {
                SqlSugarHelper.Db.BeginTran();
                SqlSugarHelper.Db.Insertable<Friend>(new Friend { CharacterId = pc.CharID, FriendCharacterId = charID })
                    .ExecuteCommand();
                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public bool IsFriend(uint char1, uint char2) {
            var exp = Expressionable.Create<Friend>();
            exp.Or(item => item.CharacterId == char1 && item.FriendCharacterId == char2);
            exp.Or(item => item.CharacterId == char2 && item.FriendCharacterId == char1);

            return SqlSugarHelper.Db.Queryable<Friend>().Where(exp.ToExpression()).Count() > 0;
        }

        public void DeleteFriend(uint char1, uint char2) {
            var exp = Expressionable.Create<Friend>();
            exp.Or(item => item.CharacterId == char1 && item.FriendCharacterId == char2);
            exp.Or(item => item.CharacterId == char2 && item.FriendCharacterId == char1);

            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var i in SqlSugarHelper.Db.Queryable<Friend>().TranLock(DbLockType.Wait)
                             .Where(exp.ToExpression()).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public Party.Party GetParty(uint id) {
            var result = SqlSugarHelper.Db.Queryable<SagaDB.Entities.Party>().Where(item => item.PartyId == id)
                .ToList();
            if (result.Count == 0) {
                return null;
            }


            var party = new Party.Party();
            party.ID = id;
            party.Name = (string)result[0].Name;
            return party;
        }

        public void NewParty(Party.Party party) {
            try {
                SqlSugarHelper.Db.BeginTran();

                uint index = SqlSugarHelper.Db.Insertable<SagaDB.Entities.Party>(new SagaDB.Entities.Party {
                    Name = party.Name, Leader = party.Leader.CharID
                }).ExecuteReturnEntity().PartyId;
                SqlSugarHelper.Db.Insertable<PartyMember>(new PartyMember {
                    PartyId = index, CharId = party.Leader.CharID
                }).ExecuteCommand();

                party.ID = (uint)index;

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveParty(Party.Party party) {
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var _party in SqlSugarHelper.Db.Queryable<SagaDB.Entities.Party>().TranLock(DbLockType.Wait)
                             .Where(item => item.PartyId == party.ID).ToList()) {
                    _party.Name = party.Name;
                    _party.Leader = party.Leader.CharID;
                    SqlSugarHelper.Db.Updateable(_party).ExecuteCommand();
                }


                SqlSugarHelper.Db.Deleteable<PartyMember>(new List<PartyMember>() {
                    new PartyMember() { PartyId = party.ID }
                }).ExecuteCommand();

                foreach (var i in party.Members.Keys) {
                    SqlSugarHelper.Db.Insertable<PartyMember>(
                        new PartyMember() { PartyId = party.ID, CharId = party.Members[i].CharID }
                    ).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void DeleteParty(Party.Party party) {
            try {
                SqlSugarHelper.Db.BeginTran();
                SqlSugarHelper.Db.Deleteable<PartyMember>(new List<PartyMember>() {
                    new PartyMember() { PartyId = party.ID }
                }).ExecuteCommand();

                SqlSugarHelper.Db.Deleteable<SagaDB.Entities.Party>(new SagaDB.Entities.Party() { PartyId = party.ID }
                ).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public Ring.Ring GetRing(uint id) {
            var result = SqlSugarHelper.Db.Queryable<Entities.Ring>().Where(item => item.RingId == id).ToList();

            var ring = new Ring.Ring();
            if (result.Count == 0) {
                return null;
            }

            ring.ID = id;
            var leader = (uint)result[0].Leader;
            ring.Name = (string)result[0].Name;
            ring.Fame = (uint)result[0].Fame;
            if (result[0].FfId != null) {
                ring.FF_ID = (uint)result[0].FfId;
            }


            var result2 = SqlSugarHelper.Db.Queryable<RingMember>()
                .Where(item => item.RingId == ring.ID).ToList();

            for (var i = 0; i < result2.Count; i++) {
                var rows = SqlSugarHelper.Db.Queryable<Character>()
                    .Where(item => item.CharacterId == (uint)result2[i].CharacterId)
                    .ToList();
                if (rows.Count > 0) {
                    var row = rows[0];
                    var pc = new ActorPC {
                        CharID = row.CharacterId,
                        Name = (string)row.Name,
                        Job = (PC_JOB)(byte)row.Job
                    };
                    var index = ring.NewMember(pc);
                    if (index >= 0) {
                        ring.Rights[index].Value = result2[i].Right;
                    }

                    if (leader == pc.CharID)
                        ring.Leader = pc;
                }
            }

            if (ring.Leader == null)
                return null;
            return ring;
        }

        public void NewRing(Ring.Ring ring) {
            if (SqlSugarHelper.Db.Queryable<Entities.Ring>().Where(item => item.Name.Equals(ring.Name)).ToList().Count >
                0) {
                ring.ID = 0xFFFFFFFF;
                return;
            }

            try {
                SqlSugarHelper.Db.BeginTran();

                ring.ID = SqlSugarHelper.Db.Insertable<Entities.Ring>(new Entities.Ring {
                    Leader = 0,
                    Name = ring.Name
                }).ExecuteReturnEntity().RingId;

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveRing(Ring.Ring ring, bool saveMembers) {
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var _ring in SqlSugarHelper.Db.Queryable<Entities.Ring>().TranLock(DbLockType.Wait)
                             .Where(item => item.RingId == ring.ID).ToList()) {
                    _ring.Leader = ring.Leader.CharID;
                    _ring.Name = ring.Name;
                    _ring.Fame = ring.Fame;
                    _ring.FfId = ring.FF_ID;
                    SqlSugarHelper.Db.Updateable(_ring).ExecuteCommand();
                }

                if (saveMembers) {
                    foreach (var ringMember in SqlSugarHelper.Db.Queryable<RingMember>().TranLock(DbLockType.Wait)
                                 .Where(item => item.RingId == ring.ID).ToList()) {
                        SqlSugarHelper.Db.Deleteable((ringMember));
                    }

                    foreach (var i in ring.Members.Keys) {
                        SqlSugarHelper.Db.Insertable<RingMember>(new RingMember {
                                RingId = ring.ID, CharacterId = ring.Members[i].CharID, Right = ring.Rights[i].Value
                            })
                            .ExecuteCommand();
                    }
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void DeleteRing(Ring.Ring ring) {
            try {
                SqlSugarHelper.Db.BeginTran();


                foreach (var ringMember in SqlSugarHelper.Db.Queryable<RingMember>().TranLock(DbLockType.Wait)
                             .Where(item => item.RingId == ring.ID).ToList()) {
                    SqlSugarHelper.Db.Deleteable((ringMember));
                }


                foreach (var _ring in SqlSugarHelper.Db.Queryable<Entities.Ring>().TranLock(DbLockType.Wait)
                             .Where(item => item.RingId == ring.ID).ToList()) {
                    SqlSugarHelper.Db.Deleteable<Entities.Ring>(_ring).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void RingEmblemUpdate(Ring.Ring ring, byte[] buf) {
            try {
                SqlSugarHelper.Db.BeginTran();
                foreach (var _ring in SqlSugarHelper.Db.Queryable<Entities.Ring>().TranLock(DbLockType.Wait)
                             .Where(item => item.RingId == ring.ID).ToList()) {
                    _ring.Emblem = buf;
                    _ring.EmblemDate = DateTime.Now.ToUniversalTime();
                    SqlSugarHelper.Db.Updateable(_ring).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public GetRingEmblemResult GetRingEmblem(uint ring_id, DateTime date) {
            var result = SqlSugarHelper.Db.Queryable<Entities.Ring>().TranLock(DbLockType.Wait)
                .Where(item => item.RingId == ring_id).ToList();
            if (result.Count == 0) {
                return new GetRingEmblemResult(null, false, DateTime.Now);
            }

            if (result[0].Emblem == null) {
                return new GetRingEmblemResult(null, false, DateTime.Now);
            }

            var newTime = (DateTime)result[0].EmblemDate;
            if (date < newTime) {
                return new GetRingEmblemResult((byte[])result[0].Emblem, true, newTime);
            }

            return new GetRingEmblemResult(new byte[0], false, newTime);
        }

        public List<Post> GetBBS(uint bbsId) {
            var list = new List<Post>();

            foreach (Bbs bbs in BbsRepository.GetBbs(bbsId)) {
                list.Add(new Post {
                    Name = bbs.Name,
                    Title = bbs.Title,
                    Content = bbs.Content,
                    Date = bbs.PostDate
                });
            }

            return list;
        }

        public List<Post> GetBBSPage(uint bbsId, int page) {
            var list = new List<Post>();

            foreach (Bbs bbs in BbsRepository.GetBbs(bbsId, page, 5)) {
                list.Add(new Post {
                    Name = bbs.Name,
                    Title = bbs.Title,
                    Content = bbs.Content,
                    Date = bbs.PostDate
                });
            }

            return list;
        }

        public List<Mail> GetMail(ActorPC pc) {
            var list = new List<Mail>();

            foreach (var i in SqlSugarHelper.Db.Queryable<Mails>().Where(item => item.CharacterId == pc.CharID)
                         .OrderByDescending(item => item.PostDate).ToList()) {
                var post = new Mail();
                post.MailID = (uint)i.MailId;
                post.Name = (string)i.Sender;
                post.Title = (string)i.Title;
                post.Content = (string)i.Content;
                post.Date = (DateTime)i.PostDate;
                list.Add(post);
            }

            pc.Mails = list;
            return list;
        }

        public bool DeleteGift(Gift gift) {
            bool isSuccess = false;
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var i in SqlSugarHelper.Db.Queryable<Gifts>().TranLock(DbLockType.Wait)
                             .Where(item => item.MailId == gift.MailID)
                             .ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
                isSuccess = true;
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }

            return isSuccess;
        }

        public bool DeleteMail(Mail mail) {
            bool isSuccess = false;
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var i in SqlSugarHelper.Db.Queryable<Mails>().TranLock(DbLockType.Wait)
                             .Where(item => item.MailId == mail.MailID)
                             .ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
                isSuccess = true;
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }

            return isSuccess;
        }

        public List<Gift> GetGifts(ActorPC pc) {
            if (pc == null) {
                return null;
            }

            var list = new List<Gift>();

            foreach (var i in SqlSugarHelper.Db.Queryable<Gifts>().TranLock(DbLockType.Wait)
                         .Where(item => item.AccountId == pc.Account.AccountID)
                         .ToList()) {
                var post = new Gift();
                post.MailID = (uint)i.MailId;
                post.AccountID = (uint)i.AccountId;
                post.Name = (string)i.Sender;
                post.Title = (string)i.Title;
                post.Date = (DateTime)i.PostDate;
                post.Items = new Dictionary<uint, ushort>();
                if (!post.Items.ContainsKey(i.ItemId1) && i.ItemId1 != 0) {
                    post.Items.Add(i.ItemId1, i.ItemCount1);
                }

                if (!post.Items.ContainsKey(i.ItemId2) && i.ItemId2 != 0) {
                    post.Items.Add(i.ItemId2, i.ItemCount2);
                }

                if (!post.Items.ContainsKey(i.ItemId3) && i.ItemId3 != 0) {
                    post.Items.Add(i.ItemId3, i.ItemCount3);
                }

                if (!post.Items.ContainsKey(i.ItemId4) && i.ItemId4 != 0) {
                    post.Items.Add(i.ItemId4, i.ItemCount4);
                }

                if (!post.Items.ContainsKey(i.ItemId5) && i.ItemId5 != 0) {
                    post.Items.Add(i.ItemId5, i.ItemCount5);
                }

                if (!post.Items.ContainsKey(i.ItemId6) && i.ItemId6 != 0) {
                    post.Items.Add(i.ItemId6, i.ItemCount6);
                }

                if (!post.Items.ContainsKey(i.ItemId7) && i.ItemId7 != 0) {
                    post.Items.Add(i.ItemId7, i.ItemCount7);
                }

                if (!post.Items.ContainsKey(i.ItemId8) && i.ItemId8 != 0) {
                    post.Items.Add(i.ItemId8, i.ItemCount8);
                }

                if (!post.Items.ContainsKey(i.ItemId9) && i.ItemId9 != 0) {
                    post.Items.Add(i.ItemId9, i.ItemCount9);
                }

                if (!post.Items.ContainsKey(i.ItemId10) && i.ItemId10 != 0) {
                    post.Items.Add(i.ItemId10, i.ItemCount10);
                }

                list.Add(post);
            }

            pc.Gifts = list;
            return list;
        }

        public uint AddNewGift(Gift gift) {
            var ids = new List<uint> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var counts = new List<ushort> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte index = 0;
            foreach (var item in gift.Items.Keys) {
                if (item != 0) {
                    ids[index] = item;
                    counts[index] = gift.Items[item];
                }

                index++;
            }

            try {
                SqlSugarHelper.Db.BeginTran();

                var gifts = SqlSugarHelper.Db.Insertable<Gifts>(new Gifts {
                    AccountId = gift.AccountID,
                    Sender = gift.Name,
                    PostDate = DateTime.Now.ToUniversalTime(),
                    Title = gift.Title,
                    ItemId1 = ids[0],
                    ItemId2 = ids[1],
                    ItemId3 = ids[2],
                    ItemId4 = ids[3],
                    ItemId5 = ids[4],
                    ItemId6 = ids[5],
                    ItemId7 = ids[6],
                    ItemId8 = ids[7],
                    ItemId9 = ids[8],
                    ItemId10 = ids[9],
                    ItemCount1 = counts[0],
                    ItemCount2 = counts[1],
                    ItemCount3 = counts[2],
                    ItemCount4 = counts[3],
                    ItemCount5 = counts[4],
                    ItemCount6 = counts[5],
                    ItemCount7 = counts[6],
                    ItemCount8 = counts[7],
                    ItemCount9 = counts[8],
                    ItemCount10 = counts[9],
                }).ExecuteReturnEntity();


                SqlSugarHelper.Db.CommitTran();

                return gifts.GiftId;
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);

                return 0;
            }
        }

        public bool BBSNewPost(ActorPC poster, uint bbsId, string title, string content) {
            return BbsRepository.Insert(poster, bbsId, title, content);
        }

        public uint GetFlyCastleRindID(uint ffid) {
            foreach (var flyingCastle in SqlSugarHelper.Db.Queryable<Entities.FlyingCastle>()
                         .Where(item => item.FfId == ffid).ToList()) {
                return flyingCastle.RingId;
            }

            return 0;
        }

        public void GetFlyCastle(ActorPC pc) {
            if (pc.Ring == null) {
                return;
            }

            var result = SqlSugarHelper.Db.Queryable<Entities.FlyingCastle>().Where(item => item.FfId == pc.Ring.FF_ID)
                .ToList();
            if (result.Count == 0) {
                return;
            }

            if (pc.Ring.FlyingCastle == null) {
                pc.Ring.FlyingCastle = new FlyingCastle.FlyingCastle();
                pc.Ring.FlyingCastle.ID = (uint)result[0].FfId;
                pc.Ring.FlyingCastle.Name = (string)result[0].Name;
                pc.Ring.FlyingCastle.RingID = (uint)result[0].RingId;
                pc.Ring.FlyingCastle.ObMode = 3;
                pc.Ring.FlyingCastle.Content = (string)result[0].Content;
                pc.Ring.FlyingCastle.Level = (uint)result[0].Level;
            }
        }

        public void GetFlyingCastleFurniture(Ring.Ring ring) {
            if (ring.FlyingCastle == null)
                return;
            if (!ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.GARDEN) ||
                !ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.ROOM)) {
                ring.FlyingCastle.Furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
                ring.FlyingCastle.Furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
                ring.FlyingCastle.Furnitures.Add(FurniturePlace.FARM, new List<ActorFurniture>());
                ring.FlyingCastle.Furnitures.Add(FurniturePlace.FISHERY, new List<ActorFurniture>());
                ring.FlyingCastle.Furnitures.Add(FurniturePlace.HOUSE, new List<ActorFurniture>());

                foreach (var i in SqlSugarHelper.Db.Queryable<FlyingCastleFurniture>().TranLock(DbLockType.Wait)
                             .Where(item => item.FlyingCastleId == ring.FlyingCastle.ID).ToList()) {
                    var place = (FurniturePlace)i.Place;
                    var actor = new ActorFurniture();
                    actor.ItemID = (uint)i.ItemId;
                    actor.PictID = (uint)i.PictId;
                    actor.X = (short)i.X;
                    actor.Y = (short)i.Y;
                    actor.Z = (short)i.Z;
                    actor.Xaxis = (short)i.AxisX;
                    actor.Yaxis = (short)i.AxisY;
                    actor.Zaxis = (short)i.AxisZ;
                    actor.Motion = (ushort)i.Motion;
                    actor.Name = (string)i.Name;
                    actor.invisble = false;


                    ring.FlyingCastle.Furnitures[place].Add(actor);
                }
            }
        }

        public void GetFlyingCastleFurnitureCopy(Dictionary<FurniturePlace, List<ActorFurniture>> Furnitures) {
            Furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
            Furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
            Furnitures.Add(FurniturePlace.FARM, new List<ActorFurniture>());
            Furnitures.Add(FurniturePlace.FISHERY, new List<ActorFurniture>());
            Furnitures.Add(FurniturePlace.HOUSE, new List<ActorFurniture>());

            foreach (var i in SqlSugarHelper.Db.Queryable<FlyingCastleFurnitureCopy>().TranLock(DbLockType.Wait)
                         .Where(item => item.FlyingCastleId == 3).ToList()) {
                var place = (FurniturePlace)i.Place;
                var actor = new ActorFurniture();
                actor.ItemID = (uint)i.ItemId;
                actor.PictID = (uint)i.PictId;
                actor.X = (short)i.X;
                actor.Y = (short)i.Y;
                actor.Z = (short)i.Z;
                actor.Xaxis = (short)i.AxisX;
                actor.Yaxis = (short)i.AxisY;
                actor.Zaxis = (short)i.AxisZ;
                actor.Motion = (ushort)i.Motion;
                actor.Name = (string)i.Name;
                actor.invisble = false;
                Furnitures[place].Add(actor);
            }
        }

        public List<FlyingCastle.FlyingCastle> GetFlyingCastles() {
            ;

            var list = new List<FlyingCastle.FlyingCastle>();

            foreach (Entities.FlyingCastle i in SqlSugarHelper.Db.Queryable<Entities.FlyingCastle>().ToList()) {
                var ff = new FlyingCastle.FlyingCastle();
                ff.Name = (string)i.Name;
                ff.Content = (string)i.Content;
                ff.ID = (uint)i.FfId;
                ff.RingID = (uint)i.RingId;
                ff.Level = (uint)i.Level;
                list.Add(ff);
            }

            return list;
        }

        public void SaveFlyCastleCopy(Dictionary<FurniturePlace, List<ActorFurniture>> furnitures) {
            //uint account = GetAccountID(pc);
            try {
                SqlSugarHelper.Db.BeginTran();


                foreach (var i in SqlSugarHelper.Db.Queryable<FlyingCastleFurnitureCopy>().TranLock(DbLockType.Wait)
                             .Where(item => item.FlyingCastleId == 3).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                if (furnitures.ContainsKey(FurniturePlace.GARDEN))
                    foreach (var i in furnitures[FurniturePlace.GARDEN])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurnitureCopy>(new FlyingCastleFurnitureCopy {
                            FlyingCastleId = 3,
                            Place = 0,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (furnitures.ContainsKey(FurniturePlace.ROOM))
                    foreach (var i in furnitures[FurniturePlace.ROOM])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurnitureCopy>(new FlyingCastleFurnitureCopy {
                            FlyingCastleId = 3,
                            Place = 1,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (furnitures.ContainsKey(FurniturePlace.FARM))
                    foreach (var i in furnitures[FurniturePlace.FARM])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurnitureCopy>(new FlyingCastleFurnitureCopy {
                            FlyingCastleId = 3,
                            Place = 2,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (furnitures.ContainsKey(FurniturePlace.FISHERY))
                    foreach (var i in furnitures[FurniturePlace.FISHERY])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurnitureCopy>(new FlyingCastleFurnitureCopy {
                            FlyingCastleId = 3,
                            Place = 3,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (furnitures.ContainsKey(FurniturePlace.HOUSE))
                    foreach (var i in furnitures[FurniturePlace.HOUSE])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurnitureCopy>(new FlyingCastleFurnitureCopy {
                            FlyingCastleId = 3,
                            Place = 4,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SavePaper(ActorPC pc) {
            if (pc.AnotherPapers == null) {
                return;
            }

            try {
                SqlSugarHelper.Db.BeginTran();

                SqlSugarHelper.Db.Deleteable<AnotherPaper>().Where(item => item.CharId == pc.CharID).ExecuteCommand();
                foreach (var i in pc.AnotherPapers) {
                    AnotherPaper anotherPaper = new AnotherPaper();
                    anotherPaper.CharId = pc.CharID;
                    anotherPaper.PaperId = i.Key;
                    anotherPaper.PaperLv = i.Value.lv;
                    anotherPaper.PaperValue = i.Value.value.Value;
                    SqlSugarHelper.Db.Insertable<AnotherPaper>(anotherPaper).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SaveFlyCastle(Ring.Ring ring) {
            if (ring == null) {
                return;
            }

            if (ring.FlyingCastle == null) {
                return;
            }


            try {
                SqlSugarHelper.Db.BeginTran();

                //uint account = GetAccountID(pc);
                if (ring.FlyingCastle.ID > 0) {
                    foreach (var flyingCastle in SqlSugarHelper.Db.Queryable<Entities.FlyingCastle>()
                                 .TranLock(DbLockType.Wait)
                                 .Where(item => item.FfId == ring.FlyingCastle.ID).ToList()) {
                        flyingCastle.Level = ring.FlyingCastle.Level;
                        flyingCastle.Content = ring.FlyingCastle.Content;
                        flyingCastle.Name = ring.FlyingCastle.Name;
                        SqlSugarHelper.Db.Updateable(flyingCastle).ExecuteCommand();
                    }
                }

                // sqlstr = string.Format("DELETE FROM `ff_furniture` WHERE `ff_id`='{0}';", ring.FlyingCastle.ID);
                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.GARDEN))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.GARDEN])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                            FlyingCastleId = ring.FlyingCastle.ID,
                            Place = 0,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.ROOM))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.ROOM])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                            FlyingCastleId = ring.FlyingCastle.ID,
                            Place = 1,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.FARM))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.FARM])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                            FlyingCastleId = ring.FlyingCastle.ID,
                            Place = 2,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.FISHERY))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.FISHERY])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                            FlyingCastleId = ring.FlyingCastle.ID,
                            Place = 3,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.HOUSE))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.HOUSE])
                        SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                            FlyingCastleId = ring.FlyingCastle.ID,
                            Place = 4,
                            ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                            AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                        }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveSerFF(Server.Server ser) {
            try {
                SqlSugarHelper.Db.BeginTran();


                foreach (var i in SqlSugarHelper.Db.Queryable<FlyingCastleFurniture>().TranLock(DbLockType.Wait)
                             .Where(item => item.FlyingCastleId == 99999).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                foreach (var i in ser.Furnitures[FurniturePlace.GARDEN])
                    SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                        FlyingCastleId = 99999,
                        Place = 0,
                        ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                        AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                    }).ExecuteCommand();
                foreach (var i in ser.Furnitures[FurniturePlace.ROOM])
                    SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                        FlyingCastleId = 99999,
                        Place = 1,
                        ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                        AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                    }).ExecuteCommand();
                foreach (var i in ser.Furnitures[FurniturePlace.HOUSE])
                    SqlSugarHelper.Db.Insertable<FlyingCastleFurniture>(new FlyingCastleFurniture {
                        FlyingCastleId = 99999,
                        Place = 4,
                        ItemId = i.ItemID, PictId = i.PictID, X = i.X, Y = i.Y, Z = i.Z, AxisX = i.Xaxis,
                        AxisY = i.Yaxis, AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                    }).ExecuteCommand();
                /*foreach (ActorFurniture i in ser.FurnituresofFG[SagaDB.FFGarden.FurniturePlace.GARDEN])
                {
                    sqlstr += string.Format("INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                       "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','5','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                      99999, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);
                }*/

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void GetSerFFurniture(Server.Server ser) {
            if (!ser.Furnitures.ContainsKey(FurniturePlace.GARDEN) ||
                !ser.Furnitures.ContainsKey(FurniturePlace.ROOM)) {
                ser.Furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.FARM, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.FISHERY, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.HOUSE, new List<ActorFurniture>());
                foreach (var i in SqlSugarHelper.Db.Queryable<FlyingCastleFurniture>().TranLock(DbLockType.Wait)
                             .Where(item => item.FlyingCastleId == 99999).ToList()) {
                    var p = (byte)i.Place;
                    if (p < 5) {
                        var place = (FurniturePlace)p;
                        var actor = new ActorFurniture();
                        actor.ItemID = (uint)i.ItemId;
                        actor.PictID = (uint)i.PictId;
                        actor.X = (short)i.X;
                        actor.Y = (short)i.Y;
                        actor.Z = (short)i.Z;
                        actor.Xaxis = (short)i.AxisX;
                        actor.Yaxis = (short)i.AxisY;
                        actor.Zaxis = (short)i.AxisZ;
                        actor.Motion = (ushort)i.Motion;
                        actor.Name = (string)i.Name;
                        actor.invisble = false;
                        ser.Furnitures[place].Add(actor);
                    }
                    else {
                        p -= 4;
                        var place = (FurniturePlace)p;
                        var actor = new ActorFurniture();
                        actor.ItemID = (uint)i.ItemId;
                        actor.PictID = (uint)i.PictId;
                        actor.X = (short)i.X;
                        actor.Y = (short)i.Y;
                        actor.Z = (short)i.Z;
                        actor.Xaxis = (short)i.AxisX;
                        actor.Yaxis = (short)i.AxisY;
                        actor.Zaxis = (short)i.AxisZ;
                        actor.Motion = (ushort)i.Motion;
                        actor.Name = (string)i.Name;
                        actor.invisble = false;
                        ser.FurnituresofFG[place].Add(actor);
                    }
                }
            }
        }

        public void CreateFF(ActorPC pc) {
            if (pc.Ring.FlyingCastle == null) {
                return;
            }

            GetAccountID(pc);
            string sqlstr;

            try {
                SqlSugarHelper.Db.BeginTran();

                uint id = SqlSugarHelper.Db.Insertable<Entities.FlyingCastle>(new Entities.FlyingCastle {
                    RingId = pc.Ring.ID,
                    Name = pc.Ring.FlyingCastle.Name,
                    Content = pc.Ring.FlyingCastle.Content,
                    Level = pc.Ring.FlyingCastle.Level
                }).ExecuteReturnEntity().FfId;

                pc.Ring.FlyingCastle.ID = id;
                pc.Ring.FF_ID = id;

                foreach (var ring in SqlSugarHelper.Db.Queryable<Entities.Ring>().TranLock(DbLockType.Wait)
                             .Where(item => item.RingId == pc.Ring.ID).ToList()) {
                    ring.FfId = pc.Ring.FF_ID;
                    SqlSugarHelper.Db.Updateable(ring).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public List<ActorPC> GetWRPRanking() {
            var characters = SqlSugarHelper.Db.Queryable<Character>().OrderByDescending(item => item.Wrp).Take(100)
                .ToList();
            var res = new List<ActorPC>();
            uint count = 1;
            foreach (Character character in characters) {
                var pc = new ActorPC();
                pc.CharID = character.CharacterId;
                pc.Name = character.Name;
                pc.Level = character.Lv;
                pc.JobLevel3 = character.JobLv3;
                pc.Job = (PC_JOB)character.Job;
                pc.WRP = character.Wrp;
                pc.WRPRanking = count;
                res.Add(pc);
                count++;
            }

            return res;
        }

        public void SavaLevelLimit() {
            var LL = LevelLimit.LevelLimit.Instance;
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.LevelLimit>().TranLock(DbLockType.Wait)
                             .ToList()) {
                    i.NowLevelLimit = LL.NowLevelLimit;
                    i.NextLevelLimit = LL.NextLevelLimit;
                    i.SetNextUpLevelLimit = LL.SetNextUpLevelLimit;
                    i.SetNextUpDays = LL.SetNextUpDays;
                    i.ReachTime = LL.ReachTime;
                    i.NextTime = LL.NextTime;
                    i.FirstPlayer = LL.FirstPlayer;
                    i.SecondPlayer = LL.SecondPlayer;
                    i.Thirdlayer = LL.Thirdlayer;
                    i.FourthPlayer = LL.FourthPlayer;
                    i.FifthPlayer = LL.FifthPlayer;
                    i.LastTimeLevelLimit = LL.LastTimeLevelLimit;
                    i.IsLock = LL.IsLock;
                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetLevelLimit() {
            var levellimit = LevelLimit.LevelLimit.Instance;
            foreach (var i in SqlSugarHelper.Db.Queryable<Entities.LevelLimit>().ToList()) {
                levellimit.NowLevelLimit = (uint)i.NowLevelLimit;
                levellimit.NextLevelLimit = (uint)i.NextLevelLimit;
                levellimit.SetNextUpLevelLimit = (uint)i.SetNextUpLevelLimit;
                levellimit.LastTimeLevelLimit = (uint)i.LastTimeLevelLimit;
                levellimit.SetNextUpDays = (uint)i.SetNextUpDays;
                levellimit.ReachTime = (DateTime)i.ReachTime;
                levellimit.NextTime = (DateTime)i.NextTime;
                levellimit.FirstPlayer = (uint)i.FirstPlayer;
                levellimit.SecondPlayer = (uint)i.SecondPlayer;
                levellimit.Thirdlayer = (uint)i.Thirdlayer;
                levellimit.FourthPlayer = (uint)i.FourthPlayer;
                levellimit.FifthPlayer = (uint)i.FifthPlayer;
                levellimit.IsLock = (byte)i.IsLock;
            }
        }

        public void SaveStamp(ActorPC pc, StampGenre genre) {
            try {
                SqlSugarHelper.Db.BeginTran();

                var result = SqlSugarHelper.Db.Queryable<Entities.Stamp>().TranLock(DbLockType.Wait)
                    .Where(item => item.CharacterId == pc.CharID).Where(item => item.StampId == (byte)genre).ToList();

                if (result.Count > 0) {
                    foreach (var i in result) {
                        i.Value = pc.Stamp[genre].Value;
                        SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                    }
                }
                else {
                    SqlSugarHelper.Db.Insertable<Entities.Stamp>(new Entities.Stamp {
                        CharacterId = pc.CharID,
                        StampId = (byte)genre,
                        Value = pc.Stamp[genre].Value
                    }).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public List<TamaireLending> GetTamaireLendings() {
            var result = SqlSugarHelper.Db.Queryable<Entities.TamaireLending>().ToList();
            var tamaireLendings = new List<TamaireLending>();
            for (var i = 0; i < result.Count; i++) {
                var tamaireLending = new TamaireLending();
                tamaireLending.Lender = (uint)result[i].CharacterId;
                tamaireLending.Baselv = (byte)result[i].BaseLevel;
                tamaireLending.JobType = (byte)result[i].JobType;
                tamaireLending.PostDue = (DateTime)result[i].PostDue;
                tamaireLending.Comment = (string)result[i].Comment;
                if (result[i].Renter1 != 0)
                    tamaireLending.Renters.Add(result[i].Renter1);
                if (result[i].Renter2 != 0)
                    tamaireLending.Renters.Add(result[i].Renter2);
                if (result[i].Renter3 != 0)
                    tamaireLending.Renters.Add(result[i].Renter3);
                if (result[i].Renter4 != 0)
                    tamaireLending.Renters.Add(result[i].Renter4);

                tamaireLendings.Add(tamaireLending);
            }

            return tamaireLendings;
        }

        public void GetTamaireLending(ActorPC pc) {
            var result = SqlSugarHelper.Db.Queryable<Entities.TamaireLending>()
                .Where(item => item.CharacterId == pc.CharID).ToList();
            if (result.Count == 0) {
                return;
            }

            if (pc.TamaireLending == null)
                pc.TamaireLending = new TamaireLending();
            pc.TamaireLending.Lender = (uint)result[0].CharacterId;
            pc.TamaireLending.Comment = (string)result[0].Comment;
            pc.TamaireLending.PostDue = (DateTime)result[0].PostDue;
            pc.TamaireLending.JobType = (byte)result[0].JobType;
            pc.TamaireLending.Baselv = (byte)result[0].BaseLevel;
            if (result[0].Renter1 != 0)
                pc.TamaireLending.Renters.Add(result[0].Renter1);
            if (result[0].Renter2 != 0)
                pc.TamaireLending.Renters.Add(result[0].Renter2);
            if (result[0].Renter3 != 0)
                pc.TamaireLending.Renters.Add(result[0].Renter3);
            if (result[0].Renter4 != 0)
                pc.TamaireLending.Renters.Add(result[0].Renter4);
        }

        public void CreateTamaireLending(TamaireLending tamaireLending) {
            try {
                SqlSugarHelper.Db.BeginTran();

                SqlSugarHelper.Db.Insertable<Entities.TamaireLending>(new Entities.TamaireLending {
                    CharacterId = tamaireLending.Lender,
                    JobType = tamaireLending.JobType,
                    BaseLevel = tamaireLending.Baselv,
                    PostDue = tamaireLending.PostDue,
                    Comment = tamaireLending.Comment,
                    Renter1 = 0,
                    Renter2 = 0,
                    Renter3 = 0,
                    Renter4 = 0,
                }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveTamaireLending(TamaireLending tamaireLending) {
            uint renter1, renter2, renter3, renter4;
            var comment = CheckSqlString(tamaireLending.Comment);

            if (tamaireLending.Renters.Count > 0)
                renter1 = tamaireLending.Renters[0];
            else
                renter1 = 0;
            if (tamaireLending.Renters.Count > 1)
                renter2 = tamaireLending.Renters[1];
            else
                renter2 = 0;
            if (tamaireLending.Renters.Count > 2)
                renter3 = tamaireLending.Renters[2];
            else
                renter3 = 0;
            if (tamaireLending.Renters.Count > 3)
                renter4 = tamaireLending.Renters[3];
            else
                renter4 = 0;

            try {
                SqlSugarHelper.Db.BeginTran();
                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.TamaireLending>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == tamaireLending.Lender).ToList()) {
                    i.PostDue = tamaireLending.PostDue;
                    i.Comment = tamaireLending.Comment;
                    i.Renter1 = renter1;
                    i.Renter2 = renter2;
                    i.Renter3 = renter3;
                    i.Renter4 = renter4;

                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void DeleteTamaireLending(TamaireLending tamaireLending) {
            try {
                SqlSugarHelper.Db.BeginTran();
                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.TamaireLending>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == tamaireLending.Lender).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void GetTamaireRental(ActorPC pc) {
            var result = SqlSugarHelper.Db.Queryable<Entities.TamaireRental>()
                .Where(item => item.CharacterId == pc.CharID).ToList();
            if (result.Count == 0) {
                return;
            }

            if (pc.TamaireRental == null)
                pc.TamaireRental = new TamaireRental();
            pc.TamaireRental.Renter = (uint)result[0].CharacterId;
            pc.TamaireRental.RentDue = (DateTime)result[0].RentDue;
            pc.TamaireRental.CurrentLender = (uint)result[0].CurrentLender;
            pc.TamaireRental.LastLender = (uint)result[0].LastLender;
        }

        public void CreateTamaireRental(TamaireRental tamaireRental) {
            try {
                SqlSugarHelper.Db.BeginTran();

                SqlSugarHelper.Db.Insertable<Entities.TamaireRental>(new Entities.TamaireRental {
                    CharacterId = tamaireRental.Renter,
                    RentDue = tamaireRental.RentDue,
                    CurrentLender = tamaireRental.CurrentLender,
                    LastLender = tamaireRental.LastLender
                }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveTamaireRental(TamaireRental tamaireRental) {
            try {
                SqlSugarHelper.Db.BeginTran();
                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.TamaireRental>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == tamaireRental.Renter).ToList()) {
                    i.RentDue = tamaireRental.RentDue;
                    i.CurrentLender = tamaireRental.CurrentLender;
                    i.LastLender = tamaireRental.LastLender;

                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void DeleteTamaireRental(TamaireRental tamaireRental) {
            try {
                SqlSugarHelper.Db.BeginTran();
                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.TamaireRental>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == tamaireRental.Renter).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void GetMosterGuide(ActorPC pc) {
            var guide = new Dictionary<uint, bool>();
            foreach (var result in SqlSugarHelper.Db.Queryable<Entities.MobStates>()
                         .Where(item => item.CharacterId == pc.CharID).ToList()) {
                //uint mobID = (uint)result["mob_id"];
                //bool state = (bool)result["state"];

                if (MobFactory.Instance.Mobs.ContainsKey(result.MobId))
                    guide.Add(result.MobId, result.State);
            }

            pc.MosterGuide = guide;
        }

        public void SaveMosterGuide(ActorPC pc, uint mobId, bool state) {
            SqlSugarHelper.Db.Storageable(new MobStates
                    { CharacterId = pc.CharID, MobId = mobId, State = state })
                .ExecuteCommand();
        }

        public void SaveQuestInfo(ActorPC pc) {
            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var questInfo in SqlSugarHelper.Db.Queryable<Entities.QuestInfo>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == pc.CharID).ToList()) {
                    SqlSugarHelper.Db.Deleteable(questInfo).ExecuteCommand();
                }

                foreach (var i in pc.KillList) {
                    SqlSugarHelper.Db.Insertable<Entities.QuestInfo>(new Entities.QuestInfo {
                        CharacterId = pc.CharID, ObjectId = i.Key, Count = i.Value.Count,
                        TotalCount = i.Value.TotalCount, Status = (i.Value.isFinish) ? (byte)1 : (byte)0
                    }).ExecuteCommand();
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetQuestInfo(ActorPC pc) {
            try {
                foreach (var questInfo in SqlSugarHelper.Db.Queryable<Entities.QuestInfo>()
                             .Where(item => item.CharacterId == pc.CharID).ToList()) {
                    if (pc.KillList.ContainsKey(questInfo.ObjectId)) {
                        continue;
                    }

                    var ki = new ActorPC.KillInfo();
                    ki.Count = questInfo.Count;
                    ki.TotalCount = questInfo.TotalCount;
                    ki.isFinish = (questInfo.Status == 1);
                    pc.KillList.Add(questInfo.ObjectId, ki);
                }
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        private void GetVar(ActorPC pc) {
            var account_id = this.GetAccountID(pc.CharID);
            var enc = Encoding.UTF8;
            var res = SqlSugarHelper.Db.Queryable<Entities.CharacterVariable>()
                .Where(item => item.CharacterId == pc.CharID).ToList();

            if (res.Count > 0) {
                var buf = (byte[])res[0].Values;
                var ms = new MemoryStream(buf);
                var br = new BinaryReader(ms);
                var count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.CInt[name] = br.ReadInt32();
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.CMask[name] = new BitMask(br.ReadInt32());
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.CStr[name] = enc.GetString(br.ReadBytes(br.ReadInt32()));
                }
            }

            var avatar = AvatarRepository.GetAvatar(account_id);

            if (avatar != null) {
                var br = new BinaryReader(new MemoryStream(avatar.Valuess));
                var count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.AInt[name] = br.ReadInt32();
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.AMask[name] = new BitMask(br.ReadInt32());
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++) {
                    var name = enc.GetString(br.ReadBytes(br.ReadInt32()));
                    pc.AStr[name] = enc.GetString(br.ReadBytes(br.ReadInt32()));
                }
            }
        }

        public void SaveItem(ActorPC pc) {
            try {
                SqlSugarHelper.Db.BeginTran();
                var account = GetAccountID(pc);
                if ((!pc.Inventory.IsEmpty || pc.Inventory.NeedSave) &&
                    pc.Inventory.Items[ContainerType.BODY].Count < 1000) {
                    if (pc.Account != null) {
                        SagaLib.Logger.GetLogger().Information(
                            "存储玩家(" + pc.Account.AccountID + ")：" + pc.Name + "道具信息...大小：" +
                            pc.Inventory.ToBytes().Length);
                    }

                    foreach (var inventory in SqlSugarHelper.Db.Queryable<Inventory>().TranLock(DbLockType.Wait)
                                 .Where(item => item.CharacterId == pc.CharID).ToList()) {
                        inventory.Data = pc.Inventory.ToBytes();
                        SqlSugarHelper.Db.Updateable<Inventory>(inventory).ExecuteCommand();
                    }
                }


                if (pc.Inventory.WareHouse != null) {
                    if (!pc.Inventory.IsWarehouseEmpty || pc.Inventory.NeedSaveWare) {
                        foreach (var warehouse in SqlSugarHelper.Db.Queryable<Warehouse>().TranLock(DbLockType.Wait)
                                     .Where(item => item.AccountId == account).ToList()) {
                            warehouse.Data = pc.Inventory.WareToBytes();

                            SqlSugarHelper.Db.Updateable<Warehouse>(warehouse).ExecuteCommand();
                        }
                    }
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception ex) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetJobLV(ActorPC pc) {
            try {
                var result = SqlSugarHelper.Db.Queryable<Entities.Skill>()
                    .Where(item => item.CharacterId == pc.CharID && item.JobBasic == 1).ToList();
                if (result.Count > 0)
                    pc.JobLV_GLADIATOR = (byte)result[0].JobLevel;
                result = SqlSugarHelper.Db.Queryable<Entities.Skill>()
                    .Where(item => item.CharacterId == pc.CharID && item.JobBasic == 31).ToList();
                if (result.Count > 0)
                    pc.JobLV_HAWKEYE = (byte)result[0].JobLevel;
                result = SqlSugarHelper.Db.Queryable<Entities.Skill>()
                    .Where(item => item.CharacterId == pc.CharID && item.JobBasic == 41).ToList();
                if (result.Count > 0)
                    pc.JobLV_FORCEMASTER = (byte)result[0].JobLevel;
                result = SqlSugarHelper.Db.Queryable<Entities.Skill>()
                    .Where(item => item.CharacterId == pc.CharID && item.JobBasic == 61).ToList();
                if (result.Count > 0)
                    pc.JobLV_CARDINAL = (byte)result[0].JobLevel;
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetNPCStates(ActorPC pc) {
            try {
                var result = SqlSugarHelper.Db.Queryable<NpcStates>().Where(item => item.CharacterId == pc.CharID)
                    .ToList();
                for (var i = 0; i < result.Count; i++) {
                    pc.NPCStates.Add(result[i].NpcId, result[i].State);
                }
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetItem(ActorPC pc) {
            try {
                var account = GetAccountID(pc);
                var inventories = SqlSugarHelper.Db.Queryable<Inventory>()
                    .Where(item => item.CharacterId == pc.CharID)
                    .ToList();


                if (inventories.Count > 0) {
                    SagaDB.Item.Inventory inv = null;

                    var buf = (byte[])inventories[0].Data;
                    SagaLib.Logger.GetLogger()
                        .Information("获取玩家(" + account + ")：" + pc.Name + "道具信息...大小：" + buf.Length);
                    var ms = new MemoryStream(buf);
                    if (buf[0] == 0x42 && buf[1] == 0x5A) {
                        var ms2 = new MemoryStream();
                        BZip2.Decompress(ms, ms2, true);
                        ms = new MemoryStream(ms2.ToArray());
#pragma warning disable SYSLIB0011
                        var bf = new BinaryFormatter();
                        inv = (SagaDB.Item.Inventory)bf.Deserialize(ms);

                        pc.Inventory = inv;
                        pc.Inventory.Owner = pc;
                    }
                    else {
                        inv = new SagaDB.Item.Inventory(pc);
                        inv.FromStream(ms);
                        pc.Inventory = inv;
                    }
                }

                var warehouse = SqlSugarHelper.Db.Queryable<Warehouse>().Where(item => item.AccountId == account)
                    .ToList();

                if (warehouse.Count > 0) {
                    Dictionary<WarehousePlace, List<Item.Item>> inv = null;
                    var buf = (byte[])warehouse[0].Data;
                    var ms = new MemoryStream(buf);
                    if (buf[0] == 0x42 && buf[1] == 0x5A) {
                        pc.Inventory.WareHouse = new Dictionary<WarehousePlace, List<Item.Item>>();
                        var ms2 = new MemoryStream();
                        BZip2.Decompress(ms, ms2, true);
                        ms = new MemoryStream(ms2.ToArray());
#pragma warning disable SYSLIB0011
                        var bf = new BinaryFormatter();
                        inv = (Dictionary<WarehousePlace, List<Item.Item>>)bf.Deserialize(ms);
                        if (inv != null) {
                            pc.Inventory.wareIndex = 200000001;
                            foreach (var i in inv.Keys) {
                                pc.Inventory.WareHouse.Add(i, new List<Item.Item>());
                                foreach (var j in inv[i]) pc.Inventory.AddWareItem(i, j);
                            }
                        }
                    }
                    else {
                        if (pc.Inventory.WareHouse == null) {
                            pc.Inventory.WareHouse = new SagaDB.Item.Inventory(pc).WareHouse;
                        }

                        pc.Inventory.WareFromSteam(ms);
                    }
                }

                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.Acropolis))
                    pc.Inventory.WareHouse.Add(WarehousePlace.Acropolis, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.FederalOfIronSouth))
                    pc.Inventory.WareHouse.Add(WarehousePlace.FederalOfIronSouth, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.FarEast))
                    pc.Inventory.WareHouse.Add(WarehousePlace.FarEast, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.IronSouth))
                    pc.Inventory.WareHouse.Add(WarehousePlace.IronSouth, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.KingdomOfNorthan))
                    pc.Inventory.WareHouse.Add(WarehousePlace.KingdomOfNorthan, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.MiningCamp))
                    pc.Inventory.WareHouse.Add(WarehousePlace.MiningCamp, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.Morg))
                    pc.Inventory.WareHouse.Add(WarehousePlace.Morg, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.Northan))
                    pc.Inventory.WareHouse.Add(WarehousePlace.Northan, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.RepublicOfFarEast))
                    pc.Inventory.WareHouse.Add(WarehousePlace.RepublicOfFarEast, new List<Item.Item>());
                if (!pc.Inventory.WareHouse.ContainsKey(WarehousePlace.Tonka))
                    pc.Inventory.WareHouse.Add(WarehousePlace.Tonka, new List<Item.Item>());
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetPartyMember(Party.Party party) {
            var result = SqlSugarHelper.Db.Queryable<PartyMember>().Where(item => item.PartyId == party.ID)
                .ToList();

            for (byte index = 0; index < 7; index++) {
                var memberCharId = (uint)result[index].CharId;
                if (memberCharId == 0) {
                    continue;
                }

                var members = SqlSugarHelper.Db.Queryable<Character>()
                    .Where(item => item.CharacterId == memberCharId)
                    .ToList();
                if (members.Count == 0) {
                    continue;
                }

                var member = members[0];
                party.Members.Add(index, new ActorPC {
                    CharID = member.CharacterId,
                    Name = member.Name,
                    Job = (PC_JOB)member.Job
                });
            }
        }

        private void GetFlyingGarden(ActorPC pc) {
            var account = GetAccountID(pc);
            var result = SqlSugarHelper.Db.Queryable<Entities.FlyingGarden>().Where(item => item.AccountId == account)
                .ToList();
            if (result.Count > 0) {
                var garden = new FlyingGarden.FlyingGarden(pc);
                garden.ID = (uint)result[0].FlyingGardenId;
                garden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE] = (uint)result[0].Part1;
                garden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL] = (uint)result[0].Part2;
                garden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR] = (uint)result[0].Part3;
                garden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE] = (uint)result[0].Part4;
                garden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall] = (uint)result[0].Part5;
                garden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof] = (uint)result[0].Part6;
                garden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR] = (uint)result[0].Part7;
                garden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL] = (uint)result[0].Part8;
                garden.Fuel = (uint)result[0].Fuel;
                pc.FlyingGarden = garden;
            }

            if (pc.FlyingGarden == null) {
                return;
            }

            foreach (var i in SqlSugarHelper.Db.Queryable<Entities.FlyingGardenFurniture>()
                         .Where(item => item.FlyingGardenId == pc.FlyingGarden.ID).ToList()) {
                var place = (FlyingGarden.FurniturePlace)(byte)i.Place;
                var actor = new ActorFurniture();

                actor.ItemID = (uint)i.ItemId;
                actor.PictID = (uint)i.PictId;
                actor.X = (short)i.X;
                actor.Y = (short)i.Y;
                actor.Z = (short)i.Z;
                actor.Xaxis = (short)i.AxisX;
                actor.Yaxis = (short)i.AxisY;
                actor.Zaxis = (short)i.AxisZ;
                actor.Motion = (ushort)i.Motion;
                actor.Name = (string)i.Name;
                pc.FlyingGarden.Furnitures[place].Add(actor);
            }
        }

        public void GetPaper(ActorPC pc) {
            foreach (var anotherPaper in SqlSugarHelper.Db.Queryable<AnotherPaper>()
                         .Where(item => item.CharId == pc.CharID).ToList()) {
                if (pc.AnotherPapers.ContainsKey(anotherPaper.PaperId)) {
                    continue;
                }

                var detail = new AnotherDetail();
                detail.value = new BitMask_Long();
                detail.value.Value = anotherPaper.PaperValue;
                detail.lv = anotherPaper.PaperLv;

                pc.AnotherPapers.Add(anotherPaper.PaperId, detail);
            }
        }

        private void SaveFGarden(ActorPC pc) {
            if (pc.FlyingGarden == null) {
                return;
            }

            try {
                SqlSugarHelper.Db.BeginTran();

                var account = GetAccountID(pc);

                bool isFound = false;

                foreach (var i in SqlSugarHelper.Db.Queryable<Entities.FlyingGarden>().TranLock(DbLockType.Wait)
                             .Where(item => item.AccountId == account).ToList()) {
                    i.Part1 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE];
                    i.Part2 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL];
                    i.Part3 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR];
                    i.Part4 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE];
                    i.Part5 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall];
                    i.Part6 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof];
                    i.Part7 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR];
                    i.Part8 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL];
                    i.Fuel = pc.FlyingGarden.Fuel;
                    SqlSugarHelper.Db.Updateable(i).ExecuteCommand();
                    isFound = true;
                }

                if (!isFound) {
                    pc.FlyingGarden.ID = SqlSugarHelper.Db.Insertable<Entities.FlyingGarden>(new Entities.FlyingGarden {
                            AccountId = account,
                            Part1 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE],
                            Part2 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL],
                            Part3 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR],
                            Part4 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE],
                            Part5 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall],
                            Part6 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof],
                            Part7 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR],
                            Part8 = pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL],
                            Fuel = pc.FlyingGarden.Fuel,
                        }).ExecuteReturnEntity()
                        .FlyingGardenId;
                }

                foreach (var i in SqlSugarHelper.Db.Queryable<FlyingGardenFurniture>().TranLock(DbLockType.Wait)
                             .Where(item => item.FlyingGardenId == pc.FlyingGarden.ID).ToList()) {
                    SqlSugarHelper.Db.Deleteable(i).ExecuteCommand();
                }

                foreach (var i in pc.FlyingGarden.Furnitures[FlyingGarden.FurniturePlace.GARDEN])
                    SqlSugarHelper.Db.Insertable<FlyingGardenFurniture>(new FlyingGardenFurniture {
                        FlyingGardenId = pc.FlyingGarden.ID,
                        Place = 0,
                        ItemId = i.ItemID,
                        PictId = i.PictID,
                        X = i.X,
                        Y = i.Y,
                        Z = i.Z,
                        AxisX = i.Xaxis,
                        AxisY = i.Yaxis,
                        AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                    }).ExecuteCommand();
                foreach (var i in pc.FlyingGarden.Furnitures[FlyingGarden.FurniturePlace.ROOM])
                    SqlSugarHelper.Db.Insertable<FlyingGardenFurniture>(new FlyingGardenFurniture {
                        FlyingGardenId = pc.FlyingGarden.ID,
                        Place = 1,
                        ItemId = i.ItemID,
                        PictId = i.PictID,
                        X = i.X,
                        Y = i.Y,
                        Z = i.Z,
                        AxisX = i.Xaxis,
                        AxisY = i.Yaxis,
                        AxisZ = i.Zaxis, Motion = i.Motion, Name = i.Name
                    }).ExecuteCommand();

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveStamps(ActorPC pc) {
            foreach (StampGenre genre in Enum.GetValues(typeof(StampGenre)))
                SaveStamp(pc, genre);
        }

        public void GetStamps(ActorPC pc) {
            foreach (var i in SqlSugarHelper.Db.Queryable<Entities.Stamp>()
                         .Where(item => item.CharacterId == pc.CharID).ToList()
                    ) {
                pc.Stamp[(StampGenre)(byte)i.StampId].Value = i.Value;
            }
        }

        //#region 副职相关

        public void GetDualJobInfo(ActorPC pc) {
            var result = SqlSugarHelper.Db.Queryable<Entities.DualJob>().TranLock(DbLockType.Wait)
                .Where(item => item.CharacterId == pc.CharID).ToList();
            if (result.Count > 0) {
                pc.PlayerDualJobList = new Dictionary<byte, PlayerDualJobInfo>();
                foreach (Entities.DualJob item in result)
                    if (pc.PlayerDualJobList.ContainsKey(item.SeriesId)) {
                        pc.PlayerDualJobList[item.SeriesId].DualJobId = item.SeriesId;
                        pc.PlayerDualJobList[item.SeriesId].DualJobLevel = item.Level;
                        pc.PlayerDualJobList[item.SeriesId].DualJobExp = item.Exp;
                    }
                    else {
                        var pi = new PlayerDualJobInfo();
                        pi.DualJobId = item.SeriesId;
                        pi.DualJobLevel = item.Level;
                        pi.DualJobExp = item.Exp;
                        pc.PlayerDualJobList.Add(item.SeriesId, pi);
                    }
            }

            GetDualJobSkill(pc);
            //else
            //{
            //    pc.PlayerDualJobList = new Dictionary<byte, PlayerDualJobInfo>();

            //    var initstr = $"";
            //    for (byte i = 1; i <= 12; i++)
            //    {
            //        initstr += $"insert into `dualjob` values ('',{pc.CharID}, {i}, 1, 0);";
            //    }
            //    SQLExecuteNonQuery(initstr);
            //    GetDualJobInfo(pc);
            //}
        }

        public void SaveDualJobInfo(ActorPC pc, bool allinfo) {
            var dic = pc.PlayerDualJobList;


            try {
                SqlSugarHelper.Db.BeginTran();

                foreach (var dualJob in SqlSugarHelper.Db.Queryable<Entities.DualJob>().TranLock(DbLockType.Wait)
                             .Where(item => item.CharacterId == pc.CharID).ToList()) {
                    SqlSugarHelper.Db.Deleteable(dualJob).ExecuteCommand();
                }

                foreach (var item in dic.Keys) {
                    SqlSugarHelper.Db.Insertable(new Entities.DualJob {
                        CharacterId = pc.CharID,
                        SeriesId = dic[item].DualJobId,
                        Level = dic[item].DualJobLevel,
                        Exp = dic[item].DualJobExp
                    }).ExecuteCommand();
                }

                if (!allinfo) {
                    foreach (var dualJobSkill in SqlSugarHelper.Db.Queryable<Entities.DualJobSkill>()
                                 .TranLock(DbLockType.Wait)
                                 .Where(item => item.CharacterId == pc.CharID)
                                 .Where(item => item.SeriesId == pc.DualJobID).ToList()) {
                        SqlSugarHelper.Db.Deleteable(dualJobSkill).ExecuteCommand();
                    }

                    foreach (var item in pc.DualJobSkills) {
                        SqlSugarHelper.Db.Insertable(new Entities.DualJobSkill {
                            CharacterId = pc.CharID,
                            SeriesId = pc.DualJobID,
                            SkillId = item.ID,
                            SkillLevel = item.Level
                        }).ExecuteCommand();
                    }

                    ;
                }

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void GetDualJobSkill(ActorPC pc) {
            pc.DualJobSkills = new List<Skill.Skill>();
            foreach (Entities.DualJobSkill item in SqlSugarHelper.Db.Queryable<Entities.DualJobSkill>()
                         .Where(item => item.CharacterId == pc.CharID)
                         .Where(item => item.SeriesId == pc.DualJobID).ToList()) {
                var id = item.SkillId;
                var level = item.SkillLevel;
                var s = SkillFactory.Instance.GetSkill(id, level);
                if (s != null) {
                    pc.DualJobSkills.Add(s);
                }
            }
        }

        public void SaveDualJobSkill(ActorPC pc) {
            foreach (var dualJobSkill in SqlSugarHelper.Db.Queryable<Entities.DualJobSkill>()
                         .TranLock(DbLockType.Wait)
                         .Where(item => item.CharacterId == pc.CharID)
                         .Where(item => item.SeriesId == pc.DualJobID).ToList()) {
                SqlSugarHelper.Db.Deleteable(dualJobSkill).ExecuteCommand();
            }

            foreach (var item in pc.DualJobSkills) {
                SqlSugarHelper.Db.Insertable(new Entities.DualJobSkill {
                    CharacterId = pc.CharID,
                    SeriesId = pc.DualJobID,
                    SkillId = item.ID,
                    SkillLevel = item.Level
                }).ExecuteCommand();
            }
        }
    }

//#endregion
}