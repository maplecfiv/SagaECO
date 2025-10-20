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

            var name = CheckSQLString(ap.BaseData.name);
            try {
                ap.ActorPartnerID = SQLExecuteScalar(string.Format(
                    "INSERT INTO `partner`(`pid`,`name`,`lv`,`tlv`,`rb`,`rank`,`perkspoints`,`perk0`,`perk1`,`perk2`," +
                    " `perk3`,`perk4`,`perk5`,`aimode`,`basicai1`,`basicai2`,`hp`,`maxhp`,`mp`,`maxmp`,`sp`,`maxsp`)" +
                    "VALUES ('{0}','{1}','{2}','{3}','0','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'," +
                    "'{17}','{18}','{19}','{20}','{21}');",
                    ap.partnerid, ap.Name, ap.Level, ap.reliability, ap.rebirth, ap.rank, ap.perkpoint, ap.perk0,
                    ap.perk1, ap.perk2, ap.perk3, ap.perk4, ap.perk5, ap.ai_mode, ap.basic_ai_mode,
                    ap.basic_ai_mode_2,
                    ap.HP, ap.MaxHP, ap.MP, ap.MaxMP, ap.SP, ap.MaxSP)).Index;

                return ap.ActorPartnerID;
            }
            catch (Exception ex) {
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
                SQLExecuteNonQuery(string.Format(
                    "UPDATE `partner` SET `pid`='{1}',`name`='{2}',`lv`='{3}',`tlv`='{4}',`rb`='{5}',`rank`='{6}',`perkspoints`='{7}'," +
                    "`hp`='{8}',`maxhp`='{9}',`mp`='{10}',`maxmp`='{11}',`sp`='{12}',`maxsp`='{13}',`perk0`='{14}',`perk1`='{15}',`perk2`='{16}',`perk3`='{17}'" +
                    ",`perk4`='{18}',`perk5`='{19}',`aimode`='{20}',`basicai1`='{21}',`basicai2`='{22}',`exp` = '{23}',`pictid` = '{24}',`nextfeedtime` = '{25}'" +
                    ", `reliabilityuprate`='{26}',`texp`='{27}' WHERE apid='{0}' LIMIT 1",
                    ap.ActorPartnerID, ap.partnerid, ap.Name, ap.Level, ap.reliability, (ap.rebirth) ? 1 : 0, ap.rank,
                    ap.perkpoint, ap.HP, ap.MaxHP,
                    ap.MP, ap.MaxMP, ap.SP, ap.MaxSP,
                    ap.perk0, ap.perk1, ap.perk2, ap.perk3, ap.perk4, ap.perk5, ap.ai_mode, ap.basic_ai_mode,
                    ap.basic_ai_mode_2, ap.exp, ap.PictID, ToSQLDateTime(ap.nextfeedtime), ap.reliabilityuprate,
                    ap.reliabilityexp));
                /*SavePartnerEquip(ap);
                SavePartnerCube(ap);
                SavePartnerAI(ap);*/
                //暂时注释防止卡死
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerEquip(ActorPartner ap) {
            string sqlstr;
            if (ap == null) {
                return;
            }

            var apid = ap.ActorPartnerID;
            sqlstr = string.Format("DELETE FROM `partnerequip` WHERE `apid`='{0}';", apid);
            if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.COSTUME)) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerequip`(`apid`,`type`,`item_id`,`count`) VALUES ('{0}','1','{1}','{2}');",
                    apid, ap.equipments[EnumPartnerEquipSlot.COSTUME].ItemID,
                    ap.equipments[EnumPartnerEquipSlot.COSTUME].Stack);
            }

            if (ap.equipments.ContainsKey(EnumPartnerEquipSlot.WEAPON)) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerequip`(`apid`,`type`,`item_id`,`count`) VALUES ('{0}','2','{1}','{2}');",
                    apid, ap.equipments[EnumPartnerEquipSlot.WEAPON].ItemID,
                    ap.equipments[EnumPartnerEquipSlot.WEAPON].Stack);
            }

            for (var i = 0; i < ap.foods.Count; i++) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerequip`(`apid`,`type`,`item_id`,`count`) VALUES ('{0}','3','{1}','{2}');",
                    apid, ap.foods[i].ItemID, ap.foods[i].Stack);
            }

            try {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerCube(ActorPartner ap) {
            string sqlstr;
            if (ap == null) {
                return;
            }

            var apid = ap.ActorPartnerID;
            sqlstr = string.Format("DELETE FROM `partnercube` WHERE `apid`='{0}';", apid);
            if (ap.equipcubes_condition.Count > 0)
                for (var i = 0; i < ap.equipcubes_condition.Count; i++)
                    sqlstr += string.Format(
                        "INSERT INTO `partnercube`(`apid`,`type`,`unique_id`) VALUES ('{0}','1','{1}');",
                        apid, ap.equipcubes_condition[i]);

            if (ap.equipcubes_action.Count > 0)
                for (var i = 0; i < ap.equipcubes_action.Count; i++)
                    sqlstr += string.Format(
                        "INSERT INTO `partnercube`(`apid`,`type`,`unique_id`) VALUES ('{0}','2','{1}');",
                        apid, ap.equipcubes_action[i]);

            if (ap.equipcubes_activeskill.Count > 0)
                for (var i = 0; i < ap.equipcubes_activeskill.Count; i++)
                    sqlstr += string.Format(
                        "INSERT INTO `partnercube`(`apid`,`type`,`unique_id`) VALUES ('{0}','3','{1}');",
                        apid, ap.equipcubes_activeskill[i]);

            if (ap.equipcubes_passiveskill.Count > 0)
                for (var i = 0; i < ap.equipcubes_passiveskill.Count; i++)
                    sqlstr += string.Format(
                        "INSERT INTO `partnercube`(`apid`,`type`,`unique_id`) VALUES ('{0}','4','{1}');",
                        apid, ap.equipcubes_passiveskill[i]);

            try {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SavePartnerAI(ActorPartner ap) {
            if (ap == null) {
                return;
            }

            string sqlstr = string.Format("DELETE FROM `partnerai` WHERE `apid`='{0}';", ap.ActorPartnerID);

            foreach (var item in ap.ai_conditions) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerai`(`apid`,`type`,`index`,`value`) VALUES ('{0}','1','{1}','{2}');",
                    ap.ActorPartnerID, item.Key, item.Value);
            }


            foreach (var item in ap.ai_reactions) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerai`(`apid`,`type`,`index`,`value`) VALUES ('{0}','2','{1}','{2}');",
                    ap.ActorPartnerID, item.Key, item.Value);
            }

            foreach (var item in ap.ai_intervals) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerai`(`apid`,`type`,`index`,`value`) VALUES ('{0}','3','{1}','{2}');",
                    ap.ActorPartnerID, item.Key, item.Value);
            }


            foreach (var item in ap.ai_states) {
                sqlstr += string.Format(
                    "INSERT INTO `partnerai`(`apid`,`type`,`index`,`value`) VALUES ('{0}','4','{1}','{2}');",
                    ap.ActorPartnerID, item.Key, Convert.ToUInt16(item.Value));
            }

            try {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public ActorPartner GetActorPartner(uint ActorPartnerID, Item.Item partneritem) {
            var sqlstr = string.Format("SELECT * FROM `partner` WHERE `apid`='{0}' LIMIT 1;", ActorPartnerID);
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count == 0) {
                return null;
            }


            var partnerid = (uint)result[0]["pid"];
            var ap = new ActorPartner(partnerid, partneritem);
            ap.ActorPartnerID = ActorPartnerID;
            ap.Name = (string)result[0]["name"];
            ap.Level = (byte)result[0]["lv"];
            ap.reliability = (byte)result[0]["tlv"];
            ap.reliabilityexp = (ulong)result[0]["texp"];
            ap.rebirth = ((byte)result[0]["rb"] != 0);
            ap.rank = (byte)result[0]["rank"];
            ap.perkpoint = (ushort)result[0]["perkspoints"];
            ap.HP = (uint)result[0]["hp"];
            ap.MaxHP = (uint)result[0]["maxhp"];
            ap.MP = (uint)result[0]["mp"];
            ap.MaxMP = (uint)result[0]["maxmp"];
            ap.SP = (uint)result[0]["sp"];
            ap.MaxSP = (uint)result[0]["maxsp"];
            ap.perk0 = (byte)result[0]["perk0"];
            ap.perk1 = (byte)result[0]["perk1"];
            ap.perk2 = (byte)result[0]["perk2"];
            ap.perk3 = (byte)result[0]["perk3"];
            ap.perk4 = (byte)result[0]["perk4"];
            ap.perk5 = (byte)result[0]["perk5"];
            ap.ai_mode = (byte)result[0]["aimode"];
            ap.basic_ai_mode = (byte)result[0]["basicai1"];
            ap.basic_ai_mode_2 = (byte)result[0]["basicai2"];
            ap.exp = (ulong)result[0]["exp"];
            ap.nextfeedtime = (DateTime)result[0]["nextfeedtime"];
            ap.reliabilityuprate = (ushort)result[0]["reliabilityuprate"];

            GetPartnerEquip(ap);
            GetPartnerCube(ap);
            GetPartnerAI(ap);
            return ap;
            //暂时注释防止卡死
        }

        public void GetPartnerEquip(ActorPartner ap) {
            var sqlstr = string.Format("SELECT * FROM `partnerequip` WHERE `apid`='{0}';", ap.ActorPartnerID);
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count > 0)
                foreach (DataRow i in result) {
                    var item = ItemFactory.Instance.GetItem((uint)i["item_id"]);
                    item.Stack = (ushort)i["count"];
                    if (byte.Parse(i["type"].ToString()) == 0x1)
                        ap.equipments[EnumPartnerEquipSlot.COSTUME] = item;
                    if (byte.Parse(i["type"].ToString()) == 0x2)
                        ap.equipments[EnumPartnerEquipSlot.WEAPON] = item;
                    if (byte.Parse(i["type"].ToString()) == 0x3)
                        ap.foods.Add(item);
                }
        }

        public void GetPartnerCube(ActorPartner ap) {
            foreach (DataRow i in SQLExecuteQuery(string.Format("SELECT * FROM `partnercube` WHERE `apid`='{0}';",
                         ap.ActorPartnerID))) {
                if ((byte)i["type"] == 1)
                    ap.equipcubes_condition.Add((ushort)i["unique_id"]);
                if ((byte)i["type"] == 2)
                    ap.equipcubes_action.Add((ushort)i["unique_id"]);
                if ((byte)i["type"] == 3)
                    ap.equipcubes_activeskill.Add((ushort)i["unique_id"]);
                if ((byte)i["type"] == 4)
                    ap.equipcubes_passiveskill.Add((ushort)i["unique_id"]);
            }
        }

        public void GetPartnerAI(ActorPartner ap) {
            foreach (DataRow i in SQLExecuteQuery(string.Format("SELECT * FROM `partnerai` WHERE `apid`='{0}';",
                         ap.ActorPartnerID))) {
                if ((byte)i["type"] == 1) ap.ai_conditions.Add((byte)i["index"], (ushort)i["value"]);
                if ((byte)i["type"] == 2) ap.ai_reactions.Add((byte)i["index"], (ushort)i["value"]);
                if ((byte)i["type"] == 3) ap.ai_intervals.Add((byte)i["index"], (ushort)i["value"]);
                if ((byte)i["type"] == 4) ap.ai_states.Add((byte)i["index"], Convert.ToBoolean((ushort)i["value"]));
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
            string sqlstr;
            _ = GetAccountID((aChar.CharID));
            sqlstr = "DELETE FROM `char` WHERE char_id='" + aChar.CharID + "';";
            sqlstr += "DELETE FROM `inventory` WHERE char_id='" + aChar.CharID + "';";
            sqlstr += "DELETE FROM `skill` WHERE char_id='" + aChar.CharID + "';";
            sqlstr += "DELETE FROM `cvar` WHERE char_id='" + aChar.CharID + "';";
            sqlstr += "DELETE FROM `friend` WHERE `char_id`='" + aChar.CharID + "' OR `friend_char_id`='" +
                      aChar.CharID + "';";
            if (aChar.Party != null)
                if (aChar.Party.Leader != null)
                    if (aChar.Party.Leader.CharID == aChar.CharID)
                        DeleteParty(aChar.Party);

            if (aChar.Ring != null)
                if (aChar.Ring.Leader != null)
                    if (aChar.Ring.Leader.CharID == aChar.CharID)
                        DeleteRing(aChar.Ring);

            try {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public ActorPC GetChar(uint charID, bool fullinfo) {
            ActorPC pc = null;
            try {
                // GetAccountID(charID);
                var character = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == charID)
                    .First();
                if (character == null) {
                    return null;
                }

                pc = new ActorPC {
                    CharID = charID,
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
            var sqlstr = "SELECT `vshop_points`,`used_vshop_points` FROM `login` WHERE account_id='" + account +
                         "' LIMIT 1";
            var result = SQLExecuteQuery(sqlstr)[0];
            var eh = pc.e;
            pc.e = null;
            pc.VShopPoints = (uint)result["vshop_points"];
            pc.e = eh;
            pc.UsedVShopPoints = (uint)result["used_vshop_points"];
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
            var cmd = new MySqlCommand(string.Format(
                "REPLACE INTO `skill`(`char_id`,`skills`,`jobbasic`,`joblv`,`jobexp`,`skillpoint`,`skillpoint2x`," +
                "`skillpoint2t`,`skillpoint3`) VALUES ('{0}',?data,'{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                pc.CharID, (int)pc.JobBasic, pc.JobLevel3, pc.JEXP, pc.SkillPoint, pc.SkillPoint2X, pc.SkillPoint2T,
                pc.SkillPoint3));
            cmd.Parameters.Add("?data", MySqlDbType.Blob).Value = ms.ToArray();
            ms.Close();
            try {
                //SQLExecuteNonQuery(sqlstr);
                SQLExecuteNonQuery(cmd);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void SaveVShop(ActorPC pc) {
            var eh = pc.e;
            pc.e = null;
            var sqlstr = string.Format("UPDATE `login` SET `vshop_points`='{0}',`used_vshop_points`='{1}'" +
                                       " WHERE account_id='{2}' LIMIT 1",
                pc.VShopPoints, pc.UsedVShopPoints, pc.Account.AccountID);
            pc.e = eh;
            SQLExecuteNonQuery(sqlstr);
        }

        public void SaveServerVar(ActorPC fakepc) {
            var sqlstr = "TRUNCATE TABLE `sVar`;";
            foreach (var i in fakepc.AStr.Keys)
                sqlstr += string.Format("INSERT INTO `sVar`(`name`,`type`,`content`) VALUES " + "('{0}',0,'{1}');", i,
                    fakepc.AStr[i]);
            foreach (var i in fakepc.AInt.Keys)
                sqlstr += string.Format("INSERT INTO `sVar`(`name`,`type`,`content`) VALUES " + "('{0}',1,'{1}');", i,
                    fakepc.AInt[i]);
            foreach (var i in fakepc.AMask.Keys)
                sqlstr += string.Format("INSERT INTO `sVar`(`name`,`type`,`content`) VALUES " + "('{0}',2,'{1}');", i,
                    fakepc.AMask[i].Value);
            SQLExecuteNonQuery(sqlstr);
            sqlstr = "TRUNCATE TABLE `sList`;";
            foreach (var item in fakepc.Adict)
                foreach (var i in item.Value.Keys)
                    sqlstr += string.Format(
                        "INSERT INTO `sList`(`name`,`key`,`type`,`content`) VALUES " + "('{0}','{1}',1,'{2}');",
                        item.Key,
                        i, item.Value[i]);

            SQLExecuteNonQuery(sqlstr);
        }

        public ActorPC LoadServerVar() {
            var fakepc = new ActorPC();
            var sqlstr = "SELECT * FROM `sVar`;";
            DataRowCollection res;
            res = SQLExecuteQuery(sqlstr);
            foreach (DataRow i in res) {
                var type = (byte)i["type"];
                switch (type) {
                    case 0:
                        fakepc.AStr[(string)i["name"]] = (string)i["content"];
                        break;
                    case 1:
                        fakepc.AInt[(string)i["name"]] = int.Parse((string)i["content"]);
                        break;
                    case 2:
                        fakepc.AMask[(string)i["name"]] = new BitMask(int.Parse((string)i["content"]));
                        break;
                }
            }

            sqlstr = "SELECT * FROM `sList`;";
            res = SQLExecuteQuery(sqlstr);
            foreach (DataRow i in res) {
                var type = (byte)i["type"];
                switch (type) {
                    case 1:
                        fakepc.Adict[(string)i["name"]][(string)i["key"]] = int.Parse((string)i["content"]);
                        break;
                }
            }

            return fakepc;
        }

        public void SaveVar(ActorPC pc) {
            var account_id = this.GetAccountID(pc.CharID);

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

            var cmd = new MySqlCommand(string.Format("REPLACE `cvar`(`char_id`,`values`) VALUES ('{0}',?data);",
                pc.CharID));
            cmd.Parameters.Add("?data", MySqlDbType.Blob).Value = ms.ToArray();
            ms.Close();
            try {
                SQLExecuteNonQuery(cmd);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
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
            AvatarRepository.SaveAvatar(account_id, values);
        }

        public void GetSkill(ActorPC pc) {
            try {
                string sqlstr;
                DataRowCollection result = null;
                //sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' LIMIT 1;";
                sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' AND `jobbasic`='" +
                         (int)pc.JobBasic + "' LIMIT 1;";
                try {
                    result = SQLExecuteQuery(sqlstr);
                }
                catch (Exception ex) {
                    SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                    return;
                }

                if (result.Count > 0) {
                    var buf = (byte[])result[0]["skills"];
                    pc.JobLevel3 = (byte)result[0]["joblv"];
                    if (pc.JobLevel3 == 0) pc.JobLevel3 = 1;
                    pc.JEXP = (ulong)result[0]["jobexp"];
                    pc.SkillPoint = (ushort)result[0]["skillpoint"];
                    pc.SkillPoint2X = (ushort)result[0]["skillpoint2x"];
                    pc.SkillPoint2T = (ushort)result[0]["skillpoint2t"];
                    pc.SkillPoint3 = (ushort)result[0]["skillpoint3"];

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
            }
        }

        public void SaveNPCState(ActorPC pc, uint npcID) {
            if (pc.NPCStates.ContainsKey(npcID)) {
                var state = pc.NPCStates[npcID];
                byte value = 0;
                if (state)
                    value = 1;
                var sqlstr = $"SELECT * FROM `npcstates` WHERE `npc_id`='{npcID}',char_id='{pc.CharID}'";
                var result = SQLExecuteQuery(sqlstr);
                if (result.Count > 0)
                    sqlstr =
                        $"INSERT INTO `npcstates`(`char_id`,`npc_id`,`state`) VALUES ('{pc.CharID}','{npcID}','{value}')";
                else
                    sqlstr =
                        $"UPDATE `npcstates` SET `npc_id`='{npcID}',`state`='{value}' WHERE `char_id`='{pc.CharID}'";
                SQLExecuteNonQuery(sqlstr);
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
            var sqlstr = "SELECT `friend_char_id` FROM `friend` WHERE `char_id`='" + pc.CharID + "';";
            var result = SQLExecuteQuery(sqlstr);
            var list = new List<ActorPC>();
            for (var i = 0; i < result.Count; i++) {
                var friend = (uint)result[i]["friend_char_id"];


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

            return list;
        }

        public List<ActorPC> GetFriendList2(ActorPC pc) {
            var sqlstr = "SELECT `char_id` FROM `friend` WHERE `friend_char_id`='" + pc.CharID + "';";
            var result = SQLExecuteQuery(sqlstr);
            var list = new List<ActorPC>();
            for (var i = 0; i < result.Count; i++) {
                var friend = (uint)result[i]["char_id"];

                var res = SqlSugarHelper.Db.Queryable<Character>().Where(item => item.CharacterId == friend)
                    .ToList();
                if (res.Count == 0)
                    continue;
                var row = res[0];
                list.Add(new ActorPC {
                    CharID = friend,
                    Name = (string)row.Name,
                    Job = (PC_JOB)(byte)row.Job,
                    Level = (byte)row.Lv,
                    JobLevel1 = (byte)row.JobLv1,
                    JobLevel2X = (byte)row.JobLv2x,
                    JobLevel2T = (byte)row.JobLv2t,
                    JobLevel3 = (byte)row.JobLv3
                });
            }

            return list;
        }

        public void AddFriend(ActorPC pc, uint charID) {
            var sqlstr = string.Format("INSERT INTO `friend`(`char_id`,`friend_char_id`) VALUES " +
                                       "('{0}','{1}');", pc.CharID, charID);
            SQLExecuteNonQuery(sqlstr);
        }

        public bool IsFriend(uint char1, uint char2) {
            var sqlstr = "SELECT `char_id` FROM `friend` WHERE `friend_char_id`='" + char2 + "' AND `char_id`='" +
                         char1 + "';";
            var result = SQLExecuteQuery(sqlstr);
            return result.Count > 0;
        }

        public void DeleteFriend(uint char1, uint char2) {
            var sqlstr = "DELETE FROM `friend` WHERE `friend_char_id`='" + char2 + "' AND `char_id`='" + char1 + "';";
            SQLExecuteNonQuery(sqlstr);
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

            string sqlstr = "SELECT * FROM `ringmember` WHERE `ring_id`='" + id + "';";
            var result2 = SQLExecuteQuery(sqlstr);
            for (var i = 0; i < result2.Count; i++) {
                var rows = SqlSugarHelper.Db.Queryable<Character>()
                    .Where(item => item.CharacterId == (uint)result2[i]["char_id"])
                    .ToList();
                if (rows.Count > 0) {
                    var row = rows[0];
                    var pc = new ActorPC {
                        CharID = row.CharacterId,
                        Name = (string)row.Name,
                        Job = (PC_JOB)(byte)row.Job
                    };
                    var index = ring.NewMember(pc);
                    if (index >= 0)
                        ring.Rights[index].Value = (int)(uint)result2[i]["right"];
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
                    string sqlstr = string.Format("DELETE FROM `ringmember` WHERE `ring_id`='{0}';\r\n", ring.ID);
                    foreach (var i in ring.Members.Keys) {
                        sqlstr += string.Format(
                            "INSERT INTO `ringmember`(`ring_id`,`char_id`,`right`) VALUES ('{0}','{1}','{2}');\r\n",
                            ring.ID, ring.Members[i].CharID, ring.Rights[i].Value);
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

                var sqlstr = string.Format("DELETE FROM `ringmember` WHERE `ring_id`='{0}';", ring.ID);
                SQLExecuteNonQuery(sqlstr);


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
            var sqlstr = string.Format("SELECT * FROM `mails` WHERE `char_id`='{0}' ORDER BY `postdate` DESC;",
                pc.CharID);
            var list = new List<Mail>();
            var result = SQLExecuteQuery(sqlstr);

            foreach (DataRow i in result) {
                var post = new Mail();
                post.MailID = (uint)i["mail_id"];
                post.Name = (string)i["sender"];
                post.Title = (string)i["title"];
                post.Content = (string)i["content"];
                post.Date = (DateTime)i["postdate"];
                list.Add(post);
            }

            pc.Mails = list;
            return list;
        }

        public bool DeleteGift(Gift gift) {
            var sqlstr = "DELETE FROM `gifts` WHERE mail_id='" + gift.MailID + "';";
            try {
                return SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public bool DeleteMail(Mail mail) {
            var sqlstr = "DELETE FROM `mails` WHERE mail_id='" + mail.MailID + "';";
            try {
                return SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        public List<Gift> GetGifts(ActorPC pc) {
            if (pc == null) return null;
            var sqlstr = string.Format("SELECT * FROM `gifts` WHERE `a_id`='{0}' ORDER BY `postdate` DESC;",
                pc.Account.AccountID);
            var list = new List<Gift>();
            var result = SQLExecuteQuery(sqlstr);

            foreach (DataRow i in result) {
                var post = new Gift();
                post.MailID = (uint)i["mail_id"];
                post.AccountID = (uint)i["a_id"];
                post.Name = (string)i["sender"];
                post.Title = (string)i["title"];
                post.Date = (DateTime)i["postdate"];
                post.Items = new Dictionary<uint, ushort>();
                for (var y = 1; y < 11; y++) {
                    var itemid = (uint)i["itemid" + y];
                    var count = (ushort)i["count" + y];
                    if (!post.Items.ContainsKey(itemid) && itemid != 0)
                        post.Items.Add(itemid, count);
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

            var sqlstr = string.Format("INSERT INTO `gifts`(`a_id`,`sender`,`postdate`,`title`" +
                                       ",`itemid1`,`itemid2`,`itemid3`,`itemid4`,`itemid5`,`itemid6`,`itemid7`,`itemid8`,`itemid9`,`itemid10`" +
                                       ",`count1`,`count2`,`count3`,`count4`,`count5`,`count6`,`count7`,`count8`,`count9`,`count10`) VALUES " +
                                       "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'" +
                                       ",'{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}');",
                gift.AccountID, gift.Name, ToSQLDateTime(DateTime.Now.ToUniversalTime()), gift.Title, ids[0]
                , ids[1], ids[2], ids[3], ids[4], ids[5], ids[6], ids[7], ids[8], ids[9], counts[0], counts[1],
                counts[2], counts[3], counts[4], counts[5]
                , counts[6], counts[7], counts[8], counts[9]);
            return SQLExecuteScalar(sqlstr).Index;
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
                var sqlstr = string.Format("SELECT * FROM `ff_furniture` WHERE `ff_id`='{0}';", ring.FlyingCastle.ID);
                var result = SQLExecuteQuery(sqlstr);
                foreach (DataRow i in result) {
                    var place = (FurniturePlace)(byte)i["place"];
                    var actor = new ActorFurniture();
                    actor.ItemID = (uint)i["item_id"];
                    actor.PictID = (uint)i["pict_id"];
                    actor.X = (short)i["x"];
                    actor.Y = (short)i["y"];
                    actor.Z = (short)i["z"];
                    actor.Xaxis = (short)i["xaxis"];
                    actor.Yaxis = (short)i["yaxis"];
                    actor.Zaxis = (short)i["zaxis"];
                    actor.Motion = (ushort)i["motion"];
                    actor.Name = (string)i["name"];
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
            var sqlstr = "SELECT * FROM `ff_furniture_copy` WHERE `ff_id`='3';";
            var result = SQLExecuteQuery(sqlstr);
            foreach (DataRow i in result) {
                var place = (FurniturePlace)(byte)i["place"];
                var actor = new ActorFurniture();
                actor.ItemID = (uint)i["item_id"];
                actor.PictID = (uint)i["pict_id"];
                actor.X = (short)i["x"];
                actor.Y = (short)i["y"];
                actor.Z = (short)i["z"];
                actor.Xaxis = (short)i["xaxis"];
                actor.Yaxis = (short)i["yaxis"];
                actor.Zaxis = (short)i["zaxis"];
                actor.Motion = (ushort)i["motion"];
                actor.Name = (string)i["name"];
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

        public void SaveFlyCastleCopy(Dictionary<FurniturePlace, List<ActorFurniture>> Furnitures) {
            //uint account = GetAccountID(pc);
            string sqlstr;
            sqlstr = "DELETE FROM `ff_furniture_copy` WHERE `ff_id`='3';";
            if (Furnitures.ContainsKey(FurniturePlace.GARDEN))
                foreach (var i in Furnitures[FurniturePlace.GARDEN])
                    sqlstr += string.Format(
                        "INSERT INTO `ff_furniture_copy`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','0','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                        3, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);

            if (Furnitures.ContainsKey(FurniturePlace.ROOM))
                foreach (var i in Furnitures[FurniturePlace.ROOM])
                    sqlstr += string.Format(
                        "INSERT INTO `ff_furniture_copy`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','1','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                        3, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);

            if (Furnitures.ContainsKey(FurniturePlace.FARM))
                foreach (var i in Furnitures[FurniturePlace.FARM])
                    sqlstr += string.Format(
                        "INSERT INTO `ff_furniture_copy`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','2','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                        3, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);

            if (Furnitures.ContainsKey(FurniturePlace.FISHERY))
                foreach (var i in Furnitures[FurniturePlace.FISHERY])
                    sqlstr += string.Format(
                        "INSERT INTO `ff_furniture_copy`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','3','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                        3, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);

            if (Furnitures.ContainsKey(FurniturePlace.HOUSE))
                foreach (var i in Furnitures[FurniturePlace.HOUSE])
                    sqlstr += string.Format(
                        "INSERT INTO `ff_furniture_copy`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','4','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                        3, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);

            SQLExecuteNonQuery(sqlstr);
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
                string sqlstr;
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

                sqlstr = string.Format("DELETE FROM `ff_furniture` WHERE `ff_id`='{0}';", ring.FlyingCastle.ID);
                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.GARDEN))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.GARDEN])
                        sqlstr += string.Format(
                            "INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                            "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','0','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                            ring.FlyingCastle.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis,
                            i.Motion,
                            i.Name);

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.ROOM))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.ROOM])
                        sqlstr += string.Format(
                            "INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                            "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','1','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                            ring.FlyingCastle.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis,
                            i.Motion,
                            i.Name);

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.FARM))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.FARM])
                        sqlstr += string.Format(
                            "INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                            "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','2','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                            ring.FlyingCastle.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis,
                            i.Motion,
                            i.Name);

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.FISHERY))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.FISHERY])
                        sqlstr += string.Format(
                            "INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                            "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','3','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                            ring.FlyingCastle.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis,
                            i.Motion,
                            i.Name);

                if (ring.FlyingCastle.Furnitures.ContainsKey(FurniturePlace.HOUSE))
                    foreach (var i in ring.FlyingCastle.Furnitures[FurniturePlace.HOUSE])
                        sqlstr += string.Format(
                            "INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                            "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','4','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                            ring.FlyingCastle.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis,
                            i.Motion,
                            i.Name);

                SQLExecuteNonQuery(sqlstr);

                SqlSugarHelper.Db.CommitTran();
            }
            catch (Exception e) {
                SqlSugarHelper.Db.RollbackTran();
                SagaLib.Logger.ShowError(e);
            }
        }

        public void SaveSerFF(Server.Server ser) {
            var sqlstr = string.Format("DELETE FROM `ff_furniture` WHERE `ff_id`='{0}';", 99999);
            foreach (var i in ser.Furnitures[FurniturePlace.GARDEN])
                sqlstr += string.Format("INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','0','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                    99999, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);
            foreach (var i in ser.Furnitures[FurniturePlace.ROOM])
                sqlstr += string.Format("INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','1','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                    99999, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);
            foreach (var i in ser.Furnitures[FurniturePlace.HOUSE])
                sqlstr += string.Format("INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                                        "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','4','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                    99999, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);
            /*foreach (ActorFurniture i in ser.FurnituresofFG[SagaDB.FFGarden.FurniturePlace.GARDEN])
            {
                sqlstr += string.Format("INSERT INTO `ff_furniture`(`ff_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                   "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','5','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                  99999, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion, i.Name);
            }*/
            SQLExecuteNonQuery(sqlstr);
        }

        public void GetSerFFurniture(Server.Server ser) {
            if (!ser.Furnitures.ContainsKey(FurniturePlace.GARDEN) ||
                !ser.Furnitures.ContainsKey(FurniturePlace.ROOM)) {
                ser.Furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.FARM, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.FISHERY, new List<ActorFurniture>());
                ser.Furnitures.Add(FurniturePlace.HOUSE, new List<ActorFurniture>());
                var sqlstr = string.Format("SELECT * FROM `ff_furniture` WHERE `ff_id`='{0}';", 99999);
                var result = SQLExecuteQuery(sqlstr);
                foreach (DataRow i in result) {
                    var p = (byte)i["place"];
                    if (p < 5) {
                        var place = (FurniturePlace)p;
                        var actor = new ActorFurniture();
                        actor.ItemID = (uint)i["item_id"];
                        actor.PictID = (uint)i["pict_id"];
                        actor.X = (short)i["x"];
                        actor.Y = (short)i["y"];
                        actor.Z = (short)i["z"];
                        actor.Xaxis = (short)i["xaxis"];
                        actor.Yaxis = (short)i["yaxis"];
                        actor.Zaxis = (short)i["zaxis"];
                        actor.Motion = (ushort)i["motion"];
                        actor.Name = (string)i["name"];
                        actor.invisble = false;
                        ser.Furnitures[place].Add(actor);
                    }
                    else {
                        p -= 4;
                        var place = (FurniturePlace)p;
                        var actor = new ActorFurniture();
                        actor.ItemID = (uint)i["item_id"];
                        actor.PictID = (uint)i["pict_id"];
                        actor.X = (short)i["x"];
                        actor.Y = (short)i["y"];
                        actor.Z = (short)i["z"];
                        actor.Xaxis = (short)i["xaxis"];
                        actor.Yaxis = (short)i["yaxis"];
                        actor.Zaxis = (short)i["zaxis"];
                        actor.Motion = (ushort)i["motion"];
                        actor.Name = (string)i["name"];
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
                var sqlstr = string.Format(
                    "UPDATE `levellimit` SET `NowLevelLimit`='{0}',`NextLevelLimit`='{1}',`SetNextUpLevel`='{2}',`SetNextUpDays`='{3}',`ReachTime`='{4}',`NextTime`='{5}'"
                    + ",`FirstPlayer`='{6}',`SecondPlayer`='{7}',`ThirdPlayer`='{8}',`FourthPlayer`='{9}',`FifthPlayer`='{10}',`LastTimeLevelLimit`='{11}',`IsLock`='{12}'"
                    , LL.NowLevelLimit, LL.NextLevelLimit, LL.SetNextUpLevelLimit, LL.SetNextUpDays, LL.ReachTime,
                    LL.NextTime, LL.FirstPlayer, LL.SecondPlayer, LL.Thirdlayer
                    , LL.FourthPlayer, LL.FifthPlayer, LL.LastTimeLevelLimit, LL.IsLock);
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetLevelLimit() {
            var sqlstr =
                "SELECT `NowLevelLimit`,`NextLevelLimit`,`SetNextUpLevel`,`SetNextUpDays`,`ReachTime`,`NextTime`,`LastTimeLevelLimit`,`FirstPlayer`" +
                ",`SecondPlayer`,`ThirdPlayer`,`FourthPlayer`,`FifthPlayer`,`IsLock` FROM `levellimit`";
            var levellimit = LevelLimit.LevelLimit.Instance;
            var resule = SQLExecuteQuery(sqlstr);
            foreach (DataRow i in resule) {
                levellimit.NowLevelLimit = (uint)i["NowLevelLimit"];
                levellimit.NextLevelLimit = (uint)i["NextLevelLimit"];
                levellimit.SetNextUpLevelLimit = (uint)i["SetNextUpLevel"];
                levellimit.LastTimeLevelLimit = (uint)i["LastTimeLevelLimit"];
                levellimit.SetNextUpDays = (uint)i["SetNextUpDays"];
                levellimit.ReachTime = (DateTime)i["ReachTime"];
                levellimit.NextTime = (DateTime)i["NextTime"];
                levellimit.FirstPlayer = (uint)i["FirstPlayer"];
                levellimit.SecondPlayer = (uint)i["SecondPlayer"];
                levellimit.Thirdlayer = (uint)i["ThirdPlayer"];
                levellimit.FourthPlayer = (uint)i["FourthPlayer"];
                levellimit.FifthPlayer = (uint)i["FifthPlayer"];
                levellimit.IsLock = (byte)i["IsLock"];
            }
        }

        public void SaveStamp(ActorPC pc, StampGenre genre) {
            var sqlstr = $"SELECT * FROM `stamp` WHERE `stamp_id`='{(byte)genre}' AND `char_id`='{pc.CharID}' ";
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count > 0)
                sqlstr =
                    $"UPDATE `stamp` SET `value`='{pc.Stamp[genre].Value}' WHERE `char_id`='{pc.CharID}' AND `stamp_id`='{(byte)genre}'";
            else
                sqlstr =
                    $"INSERT INTO `stamp`(`char_id`,`stamp_id`,`value`) VALUES ('{pc.CharID}','{(byte)genre}','{pc.Stamp[genre].Value}')";
            SQLExecuteNonQuery(sqlstr);
        }

        public List<TamaireLending> GetTamaireLendings() {
            var sqlstr = "SELECT * FROM `tamairelending`;";
            var result = SQLExecuteQuery(sqlstr);
            var tamaireLendings = new List<TamaireLending>();
            for (var i = 0; i < result.Count; i++) {
                var tamaireLending = new TamaireLending();
                tamaireLending.Lender = (uint)result[0]["char_id"];
                tamaireLending.Baselv = (byte)result[0]["baselv"];
                tamaireLending.JobType = (byte)result[0]["jobtype"];
                tamaireLending.PostDue = (DateTime)result[0]["postdue"];
                tamaireLending.Comment = (string)result[0]["comment"];
                for (byte j = 1; j <= 4; j++) {
                    var renterid = (uint)result[0]["renter" + j];
                    if (renterid != 0)
                        tamaireLending.Renters.Add(renterid);
                }

                tamaireLendings.Add(tamaireLending);
            }

            return tamaireLendings;
        }

        public void GetTamaireLending(ActorPC pc) {
            var sqlstr = $"SELECT * FROM `tamairelending` WHERE `char_id`='{pc.CharID}' LIMIT 1;";
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count != 0) {
                if (pc.TamaireLending == null)
                    pc.TamaireLending = new TamaireLending();
                pc.TamaireLending.Lender = (uint)result[0]["char_id"];
                pc.TamaireLending.Comment = (string)result[0]["comment"];
                pc.TamaireLending.PostDue = (DateTime)result[0]["postdue"];
                pc.TamaireLending.JobType = (byte)result[0]["jobtype"];
                pc.TamaireLending.Baselv = (byte)result[0]["baselv"];
                for (byte j = 1; j <= 4; j++) {
                    var renterid = (uint)result[0]["renter" + j];
                    if (renterid != 0)
                        pc.TamaireLending.Renters.Add(renterid);
                }
            }
        }

        public void CreateTamaireLending(TamaireLending tamaireLending) {
            var comment = CheckSQLString(tamaireLending.Comment);
            var sqlstr = string.Format(
                "INSERT INTO `tamairelending`(`char_id`,`jobtype`,`baselv`,`postdue`,`comment`,`renter1`,`renter2`,`renter3`,`renter4`) VALUES " +
                "('{0}','{1}','{2}','{3}','{4}','0','0','0','0');", tamaireLending.Lender, tamaireLending.JobType,
                tamaireLending.Baselv, ToSQLDateTime(tamaireLending.PostDue), comment);
            SQLExecuteScalar(sqlstr);
        }

        public void SaveTamaireLending(TamaireLending tamaireLending) {
            uint renter1, renter2, renter3, renter4;
            var comment = CheckSQLString(tamaireLending.Comment);

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
            var sqlstr = string.Format(
                "UPDATE `tamairelending` SET `postdue`='{1}',`comment`='{2}', `renter1`='{3}',`renter2`='{4}'" +
                ",`renter3`='{5}',`renter4`='{6}' WHERE `char_id`='{0}' LIMIT 1;",
                tamaireLending.Lender, ToSQLDateTime(tamaireLending.PostDue), comment, renter1, renter2, renter3,
                renter4);
            SQLExecuteNonQuery(sqlstr);
        }

        public void DeleteTamaireLending(TamaireLending tamaireLending) {
            var sqlstr = string.Format("DELETE FROM `tamairelending` WHERE `char_id`='{0}';",
                tamaireLending.Lender);
            SQLExecuteNonQuery(sqlstr);
        }

        public void GetTamaireRental(ActorPC pc) {
            var sqlstr = $"SELECT * FROM `tamairerental` WHERE `char_id`='{pc.CharID}' LIMIT 1;";
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count != 0) {
                if (pc.TamaireRental == null)
                    pc.TamaireRental = new TamaireRental();
                pc.TamaireRental.Renter = (uint)result[0]["char_id"];
                pc.TamaireRental.RentDue = (DateTime)result[0]["rentdue"];
                pc.TamaireRental.CurrentLender = (uint)result[0]["currentlender"];
                pc.TamaireRental.LastLender = (uint)result[0]["lastlender"];
            }
        }

        public void CreateTamaireRental(TamaireRental tamaireRental) {
            var sqlstr = string.Format(
                "INSERT INTO `tamairerental`(`char_id`,`rentdue`,`currentlender`,`lastlender`) VALUES " +
                "('{0}','{1}','{2}','{3}');", tamaireRental.Renter, ToSQLDateTime(tamaireRental.RentDue),
                tamaireRental.CurrentLender, tamaireRental.LastLender);
            SQLExecuteScalar(sqlstr);
        }

        public void SaveTamaireRental(TamaireRental tamaireRental) {
            var sqlstr = string.Format(
                "UPDATE `tamairerental` SET `rentdue`='{1}',`currentlender`='{2}',`lastlender`='{3}' WHERE `char_id`='{0}' LIMIT 1;",
                tamaireRental.Renter, ToSQLDateTime(tamaireRental.RentDue), tamaireRental.CurrentLender,
                tamaireRental.LastLender);
            SQLExecuteNonQuery(sqlstr);
        }

        public void DeleteTamaireRental(TamaireRental tamaireRental) {
            var sqlstr = string.Format("DELETE FROM `tamairerental` WHERE `char_id`='{0}';", tamaireRental.Renter);
            SQLExecuteNonQuery(sqlstr);
        }

        public void GetMosterGuide(ActorPC pc) {
            var guide = new Dictionary<uint, bool>();
            var sqlstr = $"SELECT * FROM `mobstates` WHERE `char_id`='{pc.CharID}'";
            var results = SQLExecuteQuery(sqlstr);
            foreach (DataRow result in results) {
                //uint mobID = (uint)result["mob_id"];
                //bool state = (bool)result["state"];
                var mobID = Convert.ToUInt32(result["mob_id"]);
                var state = Convert.ToBoolean(result["state"]);

                if (MobFactory.Instance.Mobs.ContainsKey(mobID))
                    guide.Add(mobID, state);
            }

            pc.MosterGuide = guide;
        }

        public void SaveMosterGuide(ActorPC pc, uint mobID, bool state) {
            byte value = 0;
            if (state)
                value = 1;
            var sqlstr = $"SELECT * FROM `mobstates` WHERE `char_id`='{pc.CharID}' AND `mob_id`='{mobID}' ";
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count > 0)
                sqlstr =
                    $"UPDATE `mobstates` SET `state`='{value}' WHERE `char_id`='{pc.CharID}' AND `mob_id`='{mobID}'";
            else
                sqlstr =
                    $"REPLACE INTO `mobstates` (`char_id`,`mob_id`,`state`) VALUES ('{pc.CharID}','{mobID}','{value}')";
            SQLExecuteNonQuery(sqlstr);
        }

        public void SaveQuestInfo(ActorPC pc) {
            var sqlstr = string.Format("DELETE FROM `questinfo` WHERE `char_id`='{0}';", pc.CharID);
            foreach (var i in pc.KillList) {
                byte ss = 0;
                if (i.Value.isFinish)
                    ss = 1;
                sqlstr += string.Format(
                    "INSERT INTO `questinfo`(`char_id`,`object_id`,`count`,`totalcount`,`infinish`) VALUES ('{0}','{1}','{2}','{3}','{4}');",
                    pc.CharID, i.Key, i.Value.Count, i.Value.TotalCount, ss);
            }

            try {
                SQLExecuteNonQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetQuestInfo(ActorPC pc) {
            string sqlstr;
            DataRowCollection result = null;
            try {
                sqlstr = "SELECT * FROM `questinfo` WHERE `char_id`='" + pc.CharID + "'";
                try {
                    result = SQLExecuteQuery(sqlstr);
                    if (result.Count > 0)
                        for (var i = 0; i < result.Count; i++) {
                            var ki = new ActorPC.KillInfo();
                            var mobid = (uint)result[i]["object_id"];
                            var c = (uint)result[i]["count"];
                            ki.Count = (int)c;
                            c = (uint)result[i]["totalcount"];
                            ki.TotalCount = (int)c;
                            var s = (byte)result[i]["infinish"];
                            if (s == 1)
                                ki.isFinish = true;
                            else
                                ki.isFinish = false;
                            if (!pc.KillList.ContainsKey(mobid))
                                pc.KillList.Add(mobid, ki);
                        }
                }
                catch (Exception ex) {
                    SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                }
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        private void GetVar(ActorPC pc) {
            var sqlstr = "SELECT * FROM `cvar` WHERE `char_id`='" + pc.CharID + "' LIMIT 1;";
            var account_id = this.GetAccountID(pc.CharID);
            var enc = Encoding.UTF8;
            DataRowCollection res;
            res = SQLExecuteQuery(sqlstr);

            if (res.Count > 0) {
                var buf = (byte[])res[0]["values"];
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
            string sqlstr;
            DataRowCollection result = null;
            try {
                sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' AND `jobbasic`='" + 1 +
                         "' LIMIT 1;";
                result = SQLExecuteQuery(sqlstr);
                if (result.Count > 0)
                    pc.JobLV_GLADIATOR = (byte)result[0]["joblv"];
                sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' AND `jobbasic`='" + 31 +
                         "' LIMIT 1;";
                result = SQLExecuteQuery(sqlstr);
                if (result.Count > 0)
                    pc.JobLV_HAWKEYE = (byte)result[0]["joblv"];
                sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' AND `jobbasic`='" + 41 +
                         "' LIMIT 1;";
                result = SQLExecuteQuery(sqlstr);
                if (result.Count > 0)
                    pc.JobLV_FORCEMASTER = (byte)result[0]["joblv"];
                sqlstr = "SELECT * FROM `skill` WHERE `char_id`='" + pc.CharID + "' AND `jobbasic`='" + 61 +
                         "' LIMIT 1;";
                result = SQLExecuteQuery(sqlstr);
                if (result.Count > 0)
                    pc.JobLV_CARDINAL = (byte)result[0]["joblv"];
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        public void GetNPCStates(ActorPC pc) {
            string sqlstr;
            DataRowCollection result = null;
            sqlstr = $"SELECT * FROM `npcstates` WHERE `char_id`='{pc.CharID}';";
            try {
                result = SQLExecuteQuery(sqlstr);
            }
            catch (Exception ex) {
                SagaLib.Logger.GetLogger().Error(ex, ex.Message);
                return;
            }

            for (var i = 0; i < result.Count; i++) {
                var npcID = (uint)result[i]["npc_id"];
                var state = (bool)result[i]["state"];
                pc.NPCStates.Add(npcID, state);
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
            var result =
                SQLExecuteQuery(string.Format("SELECT * FROM `fgarden` WHERE `account_id`='{0}' LIMIT 1;",
                    account));
            if (result.Count > 0) {
                var garden = new FlyingGarden.FlyingGarden(pc);
                garden.ID = (uint)result[0]["fgarden_id"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE] = (uint)result[0]["part1"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL] = (uint)result[0]["part2"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR] = (uint)result[0]["part3"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE] = (uint)result[0]["part4"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall] = (uint)result[0]["part5"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof] = (uint)result[0]["part6"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR] = (uint)result[0]["part7"];
                garden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL] = (uint)result[0]["part8"];
                garden.Fuel = (uint)result[0]["fuel"];
                pc.FlyingGarden = garden;
            }

            if (pc.FlyingGarden == null) {
                return;
            }

            foreach (DataRow i in SQLExecuteQuery(string.Format(
                         "SELECT * FROM `fgarden_furniture` WHERE `fgarden_id`='{0}';",
                         pc.FlyingGarden.ID))) {
                var place = (FlyingGarden.FurniturePlace)(byte)i["place"];
                var actor = new ActorFurniture();
                actor.ItemID = (uint)i["item_id"];
                actor.PictID = (uint)i["pict_id"];
                actor.X = (short)i["x"];
                actor.Y = (short)i["y"];
                actor.Z = (short)i["z"];
                actor.Xaxis = (short)i["xaxis"];
                actor.Yaxis = (short)i["yaxis"];
                actor.Zaxis = (short)i["zaxis"];
                //actor.Dir = (ushort)i["dir"];
                actor.Motion = (ushort)i["motion"];
                actor.Name = (string)i["name"];
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

            var account = GetAccountID(pc);
            if (pc.FlyingGarden.ID > 0) {
                SQLExecuteNonQuery(string.Format(
                    "UPDATE `fgarden` SET `part1`='{0}',`part2`='{1}',`part3`='{2}',`part4`='{3}',`part5`='{4}'," +
                    "`part6`='{5}',`part7`='{6}',`part8`='{7}',`fuel`='{9}' WHERE `fgarden_id`='{8}';",
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL],
                    pc.FlyingGarden.ID,
                    pc.FlyingGarden.Fuel));
            }
            else {
                pc.FlyingGarden.ID = SQLExecuteScalar(string.Format(
                    "INSERT INTO `fgarden`(`account_id`,`part1`,`part2`,`part3`,`part4`,`part5`," +
                    "`part6`,`part7`,`part8`,`fuel`) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');",
                    account,
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_BASE],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.FLYING_SAIL],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_FLOOR],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.GARDEN_MODELHOUSE],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseOutSideWall],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.HouseRoof],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_FLOOR],
                    pc.FlyingGarden.FlyingGardenEquipments[FlyingGardenSlot.ROOM_WALL],
                    pc.FlyingGarden.Fuel)).Index;
            }

            string sqlstr = string.Format("DELETE FROM `fgarden_furniture` WHERE `fgarden_id`='{0}';",
                pc.FlyingGarden.ID);
            foreach (var i in pc.FlyingGarden.Furnitures[FlyingGarden.FurniturePlace.GARDEN])
                sqlstr += string.Format(
                    "INSERT INTO `fgarden_furniture`(`fgarden_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                    "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','0','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                    pc.FlyingGarden.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion,
                    i.Name);
            foreach (var i in pc.FlyingGarden.Furnitures[FlyingGarden.FurniturePlace.ROOM])
                sqlstr += string.Format(
                    "INSERT INTO `fgarden_furniture`(`fgarden_id`,`place`,`item_id`,`pict_id`,`x`,`y`," +
                    "`z`,`xaxis`,`yaxis`,`zaxis`,`motion`,`name`) VALUES ('{0}','1','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');",
                    pc.FlyingGarden.ID, i.ItemID, i.PictID, i.X, i.Y, i.Z, i.Xaxis, i.Yaxis, i.Zaxis, i.Motion,
                    i.Name);
            SQLExecuteNonQuery(sqlstr);
        }

        public void SaveStamps(ActorPC pc) {
            foreach (StampGenre genre in Enum.GetValues(typeof(StampGenre)))
                SaveStamp(pc, genre);
        }

        public void GetStamps(ActorPC pc) {
            var ret = SQLExecuteQuery($"SELECT * FROM `stamp` WHERE `char_id`='{pc.CharID}' ");
            for (var i = 0; i < ret.Count; i++) {
                var result = ret[i];
                pc.Stamp[(StampGenre)(byte)result["stamp_id"]].Value = (short)result["value"];
            }
        }

        //#region 副职相关

        public void GetDualJobInfo(ActorPC pc) {
            var result = SQLExecuteQuery($"select * from `dualjob` where `char_id` = '{pc.CharID}'");
            if (result.Count > 0) {
                pc.PlayerDualJobList = new Dictionary<byte, PlayerDualJobInfo>();
                foreach (DataRow item in result)
                    if (pc.PlayerDualJobList.ContainsKey(byte.Parse(item["series_id"].ToString()))) {
                        pc.PlayerDualJobList[byte.Parse(item["series_id"].ToString())].DualJobId =
                            byte.Parse(item["series_id"].ToString());
                        pc.PlayerDualJobList[byte.Parse(item["series_id"].ToString())].DualJobLevel =
                            byte.Parse(item["level"].ToString());
                        pc.PlayerDualJobList[byte.Parse(item["series_id"].ToString())].DualJobExp =
                            ulong.Parse(item["exp"].ToString());
                    }
                    else {
                        var pi = new PlayerDualJobInfo();
                        pi.DualJobId = byte.Parse(item["series_id"].ToString());
                        pi.DualJobLevel = byte.Parse(item["level"].ToString());
                        pi.DualJobExp = ulong.Parse(item["exp"].ToString());
                        pc.PlayerDualJobList.Add(byte.Parse(item["series_id"].ToString()), pi);
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
            var insertstr = $"delete from `dualjob` where `char_id` = {pc.CharID};";
            foreach (var item in dic.Keys)
                insertstr +=
                    $"insert into `dualjob` values ('',{pc.CharID}, {dic[item].DualJobId},{dic[item].DualJobLevel}, {dic[item].DualJobExp});";
            SQLExecuteNonQuery(insertstr);

            if (!allinfo) {
                return;
            }

            var delskillstr =
                $"delete from `dualjob_skill` where `char_id` = {pc.CharID} and `series_id` = {pc.DualJobID};";
            foreach (var item in pc.DualJobSkills)
                delskillstr +=
                    $"insert into `dualjob_skill` values ('',{pc.CharID}, {pc.DualJobID}, {item.ID}, {item.Level});";
            SQLExecuteNonQuery(delskillstr);
        }

        public void GetDualJobSkill(ActorPC pc) {
            var sqlstr =
                $"select * from `dualjob_skill` where `char_id` = '{pc.CharID}' and series_id={pc.DualJobID}";
            var result = SQLExecuteQuery(sqlstr);
            if (result.Count > 0) {
                pc.DualJobSkills = new List<Skill.Skill>();
                foreach (DataRow item in result) {
                    var id = uint.Parse(item["skill_id"].ToString());
                    var level = byte.Parse(item["skill_level"].ToString());
                    var s = SkillFactory.Instance.GetSkill(id, level);
                    if (s != null)
                        pc.DualJobSkills.Add(s);
                }
            }
            else {
                pc.DualJobSkills = new List<Skill.Skill>();
            }
        }

        public void SaveDualJobSkill(ActorPC pc) {
            var delskillstr =
                $"delete from `dualjob_skill` where `char_id` = {pc.CharID} and `series_id` = {pc.DualJobID};";
            foreach (var item in pc.DualJobSkills)
                delskillstr +=
                    $"insert into `dualjob_skill` values ('',{pc.CharID}, {pc.DualJobID}, {item.ID}, {item.Level});";
            SQLExecuteNonQuery(delskillstr);
        }
    }

//#endregion
}