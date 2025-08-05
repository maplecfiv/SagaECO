using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaDB.DefWar;
using SagaDB.FGarden;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Partner;
using SagaDB.Title;
using SagaLib;
using SagaMap.Dungeon;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;
using SagaMap.Scripting;
using SagaMap.Skill;
using SagaMap.Skill.Additions.Global;
using SagaMap.Tasks.PC;
using Item = SagaDB.Item.Item;
using Version = SagaLib.Version;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public DateTime hpmpspStamp = DateTime.Now;

        private uint masterpartner;
        public DateTime moveCheckStamp = DateTime.Now;
        public DateTime moveStamp = DateTime.Now;
        private uint pupilinpartner;

        //发送虚拟actor

        public void TitleProccess(ActorPC pc, uint ID, uint value)
        {
            if (CheckTitle((int)ID)) return;
            if (TitleFactory.Instance.Items.ContainsKey(ID))
            {
                var t = TitleFactory.Instance.Items[ID];
                //應該逐項條件檢查 先放著
                /*
                string name = "称号" + ID.ToString() + "完成度";
                pc.AInt[name] += (int)value;
                if (pc.ALong[name] >= (long)t.PrerequisiteCount)
                    UnlockTitle(pc, ID);
                */
            }
        }

        public void UnlockTitle(ActorPC pc, uint ID)
        {
            if (TitleFactory.Instance.Items.ContainsKey(ID))
            {
                var t = TitleFactory.Instance.Items[ID];
                var name = "称号" + ID + "完成度";
                if (Character.ALong[name] >= t.PrerequisiteCount)
                {
                    //應該逐項條件檢查 先放著
                    /*
                    if (!CheckTitle((int)ID))
                    {
                        SetTitle((int)ID, true);
                        SendSystemMessage("恭喜你！解锁了『" + t.name + "』称号！");
                        Skill.SkillHandler.Instance.ShowEffectOnActor(pc, 5420);
                    }
                    */
                }
            }
        }

        public void OnPlayerSetOption(CSMG_PLAYER_SETOPTION p)
        {
            if (Character == null)
                return;


            //SendSystemMessage("OPTION Result:" +  (Packets.Server.SSMG_ACTOR_OPTION.Options)p.GetOption);
            //SendSystemMessage("PACKET: " + p.DumpData());
            //SendSystemMessage("-----------");

            var unk = (SSMG_ACTOR_OPTION.Options)p.GetOption;

            foreach (var item in Enum.GetValues(typeof(SSMG_ACTOR_OPTION.Options)).Cast<Enum>()
                         .Where(item => unk.HasFlag(item)))
                switch ((SSMG_ACTOR_OPTION.Options)item)
                {
                    case SSMG_ACTOR_OPTION.Options.NONE:
                        ResetOption();
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_TRADE:
                        Character.canTrade = false;
                        Character.CInt["canTrade"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_EQUPMENT:
                        Character.showEquipment = false;
                        Character.CInt["showEquipment"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_FRIEND:
                        Character.canFriend = false;
                        Character.CInt["canFriend"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_BOND:
                        Character.canMentor = false;
                        Character.CInt["canMentor"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_PARTY:
                        Character.canParty = false;
                        Character.CInt["canParty"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_PATNER:
                        Character.canChangePartnerDisplay = false;
                        Character.CInt["canChangePartnerDisplay"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_REVIVE_MESSAGE:
                        Character.showRevive = false;
                        Character.CInt["showRevive"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_RING:
                        Character.canRing = false;
                        Character.CInt["canRing"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_SKILL:
                        Character.canWork = false;
                        Character.CInt["canWork"] = 0;
                        break;
                    case SSMG_ACTOR_OPTION.Options.NO_SPIRIT_POSSESSION:
                        Character.canPossession = false;
                        Character.CInt["canPossession"] = 0;
                        break;
                }
        }

        public void ResetOption()
        {
            if (Character == null)
                return;

            Character.canTrade = true;
            Character.canParty = true;
            Character.canWork = true;
            Character.canRing = true;
            Character.canPossession = true;
            Character.canFriend = true;
            Character.showEquipment = true;
            Character.showRevive = true;
            Character.canMentor = true;
            Character.canChangePartnerDisplay = true;
            Character.CInt["canTrade"] = 1;
            Character.CInt["canParty"] = 1;
            Character.CInt["canPossession"] = 1;
            Character.CInt["canRing"] = 1;
            Character.CInt["showRevive"] = 1;
            Character.CInt["canWork"] = 1;
            Character.CInt["canMentor"] = 1;
            Character.CInt["showEquipment"] = 1;
            Character.CInt["canChangePartnerDisplay"] = 1;
            Character.CInt["canFriend"] = 1;
        }

        public void OnPlayerSetTitle(CSMG_PLAYER_SETTITLE p)
        {
            if ((p.GetTSubID < 100000 || CheckTitle((int)p.GetTSubID)) &&
                (p.GetTPredID < 100000 || CheckTitle((int)p.GetTPredID)) &&
                (p.GetTBattleID < 100000 || CheckTitle((int)p.GetTBattleID)))
            {
                Character.AInt["称号_主语"] = (int)p.GetTSubID;
                Character.AInt["称号_连词"] = (int)p.GetTConjID;
                Character.AInt["称号_谓语"] = (int)p.GetTPredID;
                Character.AInt["称号_战斗"] = (int)p.GetTBattleID;
                StatusFactory.Instance.CalcStatus(Character);
                SendPCTitleInfo();
            }
            else
            {
                SendSystemMessage("非法的称号ID");
            }
        }

        public void OnPlayerOpenDailyStamp(CSMG_DAILY_STAMP_OPEN p2)
        {
            SendNPCPlaySound(3501, 0, 100, 50);

            //Hide Daily Stamp Icon
            var ds = new SSMG_PLAYER_SHOW_DAILYSTAMP();
            ds.Type = 0;
            netIO.SendPacket(ds);


            var days = Character.AInt["每日盖章"];
            var thisDay = DateTime.Today;

            if (Character.AStr["DailyStamp_DAY"] != thisDay.ToString("d"))
            {
                if (Character.AInt["每日盖章"] == 10) Character.AInt["每日盖章"] = 0;


                Character.AStr["DailyStamp_DAY"] = thisDay.ToString("d");
                Character.AInt["每日盖章"] += 1;


                var p = new SSMG_NPC_DAILY_STAMP();
                p.StampCount = (byte)Character.AInt["每日盖章"];
                p.Type = 2;
                netIO.SendPacket(p);

                if (Character.AInt["每日盖章"] == 5)
                {
                    EventActivate(19230002);
                    return;
                }

                if (Character.AInt["每日盖章"] == 10)
                {
                    //Character.AInt["每日盖章"] = 0;

                    EventActivate(19230003);
                    return;
                }

                //Normal Stamp
                EventActivate(19230001);
            }
            else
            {
                var p = new SSMG_NPC_DAILY_STAMP();
                p.StampCount = (byte)Character.AInt["每日盖章"];
                p.Type = 1;
                netIO.SendPacket(p);
            }
        }

        public void OnPlayerTitleRequire(CSMG_PLAYER_TITLE_REQUIRE p)
        {
            if (p.tID == 9 && Character.Gold >= 10000000)
                TitleProccess(Character, 9, 1);

            /*
            p2.tID = p.tID;
            p2.mark = 1;
            if (Character.AInt["称号" + p.tID.ToString() + "完成度"] != 0)
                p2.task = (ulong)Character.AInt["称号" + p.tID.ToString() + "完成度"];
            else p2.task = 0;
            */
            uint id = p.ID;
            if (TitleFactory.Instance.Items.ContainsKey(id))
            {
                var p2 = new SSMG_PLAYER_TITLE_REQ();
                p2.tID = TitleFactory.Instance.Items[id].ID;
                p2.PutPrerequisite(TitleFactory.Instance.Items[id].Prerequisites.Values.ToList());
                netIO.SendPacket(p2);
            }
        }

        public void OnBondRequestFromMaster(CSMG_BOND_REQUEST_FROM_MASTER p)
        {
            var pupilin = MapClientManager.Instance.FindClient(p.CharID);
            SendSystemMessage("师徒系统尚未实装。");

            var result = CheckMasterToPupilinInvitation(pupilin);
            var p1 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p1.Result = result;
            netIO.SendPacket(p1);
        }

        public int CheckMasterToPupilinInvitation(MapClient pupilin)
        {
            if (pupilin == null)
                return -2; //相手が見つかりませんでした
            if (!pupilin.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (pupilin.trading || pupilin.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Pupilins.Count >= Character.PupilinLimit)
                return -7; //これ以上弟子を取ることが出来ません
            if (pupilin.Character.Level >= 50 || pupilin.Character.JobLevel2T != 0 ||
                pupilin.Character.JobLevel2X != 0 || pupilin.Character.Rebirth)
                return -6; //そのキャラクターは弟子になれません
            if (pupilin.masterpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Pupilins.Contains(pupilin.Character.CharID))
                return -3; //既に師匠がいます
            try
            {
                var p2 = new SSMG_BOND_INVITE_TO_PUPILIN();
                p2.MasterID = Character.CharID;
                pupilin.netIO.SendPacket(p2);
                pupilinpartner = pupilin.Character.CharID;
                pupilin.masterpartner = Character.CharID;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return -11; //申請中です
        }

        public void OnBondRequestFromPupilin(CSMG_BOND_REQUEST_FROM_PUPILIN p)
        {
            var master = MapClientManager.Instance.FindClient(p.CharID);
            SendSystemMessage("师徒系统尚未实装。");

            var result = CheckPupilinToMasterInvitation(master);
            var p1 = new SSMG_BOND_INVITE_PUPILIN_RESULT();
            p1.Result = result;
            netIO.SendPacket(p1);
        }

        public int CheckPupilinToMasterInvitation(MapClient master)
        {
            if (master == null)
                return -2; //相手が見つかりませんでした
            if (!master.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (master.trading || master.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Master == master.Character.CharID)
                return -7; //既に師匠がいます
            if (master.Character.Level <= 55 || !master.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (master.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Master != 0)
                return -3; //既に違う人の弟子になっています
            try
            {
                var p2 = new SSMG_BOND_INVITE_TO_MASTER();
                p2.PupilinID = Character.CharID;
                master.netIO.SendPacket(p2);
                masterpartner = master.Character.CharID;
                master.pupilinpartner = Character.CharID;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return -11; //申請中です
        }

        public void OnBondPupilinAnswer(CSMG_BOND_REQUEST_PUPILIN_ANSWER p)
        {
            var master = MapClientManager.Instance.FindClient(p.MasterCharID);
            var result = CheckPupilinToMasterAnswer(p.Rejected, master);
            var p2 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p2.Result = result;
            master.netIO.SendPacket(p2);
        }

        public int CheckPupilinToMasterAnswer(bool rejected, MapClient master)
        {
            if (rejected)
                return -4; //拒否されました
            if (master == null)
                return -2; //相手が見つかりませんでした
            if (!master.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (master.trading || master.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Master == master.Character.CharID)
                return -7; //既に師匠がいます
            if (master.Character.Level <= 55 || !master.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (master.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Master != 0)
                return -3; //既に違う人の弟子になっています
            try
            {
                Character.Master = master.Character.CharID;
                master.Character.Pupilins.Add(Character.CharID);
                masterpartner = 0;
                master.pupilinpartner = 0;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //師匠になりました
        }

        public void OnBondMasterAnswer(CSMG_BOND_REQUEST_MASTER_ANSWER p)
        {
            var pupilin = MapClientManager.Instance.FindClient(p.PupilinCharID);
            var result = CheckMasterToPupilinAnswer(p.Rejected, pupilin);
            var p2 = new SSMG_BOND_INVITE_MASTER_RESULT();
            p2.Result = result;
            pupilin.netIO.SendPacket(p2);
        }

        public int CheckMasterToPupilinAnswer(bool rejected, MapClient pupilin)
        {
            if (rejected)
                return -4; //拒否されました
            if (pupilin == null)
                return -2; //相手が見つかりませんでした
            if (!pupilin.Character.canMentor)
                return -10; //相手の師弟関係設定が不許可になっています
            if (pupilin.trading || pupilin.scriptThread != null)
                return -9; //相手と師弟関係を結べる状態ではありません
            if (Character.Pupilins.Count >= Character.PupilinLimit)
                return -7; //これ以上弟子を取ることが出来ません
            if (pupilin.Character.Level <= 55 || !pupilin.Character.Rebirth)
                return -6; //そのキャラクターは師匠になれません
            if (pupilin.pupilinpartner != 0)
                return -5; //他の人から招待を受けています
            if (Character.Pupilins.Contains(pupilin.Character.CharID))
                return -3; //既に師匠がいます
            try
            {
                Character.Pupilins.Add(pupilin.Character.CharID);
                pupilin.Character.Master = Character.CharID;
                pupilinpartner = 0;
                pupilin.masterpartner = 0;
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //師匠になりました
        }

        public void OnBondBreak(CSMG_BOND_CANCEL p)
        {
            var target = MapClientManager.Instance.FindClient(p.TargetCharID);
            SendSystemMessage("師徒系統尚未實裝。");
            try
            {
                if (Character.Pupilins.Contains(target.Character.CharID))
                    Character.Pupilins.Remove(target.Character.CharID);
                if (Character.Master == target.Character.CharID)
                    Character.Master = 0;
                if (target.Character.Pupilins.Contains(Character.CharID))
                    target.Character.Pupilins.Remove(Character.CharID);
                if (target.Character.Master == Character.CharID)
                    target.Character.Master = 0;
            }
            catch
            {
            }

            var p1 = new SSMG_BOND_BREAK_RESULT();
            netIO.SendPacket(p1);
            var p2 = new SSMG_BOND_BREAK_RESULT();
            target.netIO.SendPacket(p2);
        }

        public void OnPlayerCancleTitleNew(CSMG_PLAYER_TITLE_CANCLENEW p)
        {
            var index = (int)p.tID;
            byte page = 1;
            var bounsflag = false;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var value = new BitMask_Long();
            var name = "N称号记录" + page;
            if (Character.AStr[name] == "")
            {
                value.Value = 0;
            }
            else
            {
                value.Value = ulong.Parse(Character.AStr[name]);
                if (value.Test((ulong)Math.Pow(2, index - 1)))
                    bounsflag = true;
                value.SetValueForNum(index, false);
            }

            Character.AStr[name] = value.Value.ToString();

            if (bounsflag)
            {
                var t = TitleFactory.Instance.Items[p.tID];
                foreach (var item in t.Bonus.Keys)
                {
                    var it = ItemFactory.Instance.GetItem(item);
                    it.Stack = t.Bonus[item];
                    AddItem(it, true);
                }

                if (t.Bonus.Count > 0)
                    SendSystemMessage("获得了称号『" + t.name + "』的奖励！");
                else
                    SendSystemMessage("称号『" + t.name + "』没有物品奖励。");
            }
        }

        public bool CheckTitle(int ID)
        {
            if (ID > 100000) return true;
            var index = ID;
            byte page = 1;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var value = new BitMask_Long();
            var name = "称号记录" + page;
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            var mark = (ulong)Math.Pow(2, index - 1);
            return value.Test(mark);
        }

        public void SendPCTitleInfo()
        {
            if (Character == null)
                return;
            var p = new SSMG_PLAYER_TITLE();
            var titles = new List<uint>();
            titles.Add((uint)Character.AInt["称号_主语"]);
            titles.Add((uint)Character.AInt["称号_连词"]);
            titles.Add((uint)Character.AInt["称号_谓语"]);
            titles.Add((uint)Character.AInt["称号_战斗"]);

            netIO.SendPacket(p);
            StatusFactory.Instance.CalcStatus(Character);
        }

        public void SendTitleList()
        {
            if (Character == null)
                return;
            var p = new SSMG_PLAYER_TITLE_LIST();
            var unknown1 = new List<ulong>();
            unknown1.Add(0);
            unknown1.Add(0);
            unknown1.Add(0);
            p.PutUnknown1(unknown1);

            var unknown2 = new List<ulong>();
            unknown2.Add(0);
            unknown2.Add(0);
            unknown2.Add(0);
            p.PutUnknown2(unknown2);

            var titles = new List<ulong>();
            if (Character.AStr["称号记录1"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录1"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录2"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录2"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录3"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录3"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录4"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录4"]));
            else
                titles.Add(0);
            if (Character.AStr["称号记录5"] != "")
                titles.Add(ulong.Parse(Character.AStr["称号记录5"]));
            else
                titles.Add(0);
            p.PutTitles(titles);

            var ntitles = new List<ulong>();
            if (Character.AStr["N称号记录1"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录1"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录2"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录2"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录3"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录3"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录4"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录4"]));
            else
                ntitles.Add(0);
            if (Character.AStr["N称号记录5"] != "")
                ntitles.Add(ulong.Parse(Character.AStr["N称号记录5"]));
            else
                ntitles.Add(0);
            p.PutTitles(ntitles);

            netIO.SendPacket(p);
        }

        public void SetTitle(int n, bool v)
        {
            var index = n;
            var value = new BitMask_Long();
            byte page = 1;
            if (index > 64)
            {
                page += (byte)(index / 64);
                index -= 64 * (page - 1);
            }

            var name = "称号记录" + page;
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            value.SetValueForNum(index, v);
            Character.AStr[name] = value.Value.ToString();


            name = "N" + name;
            value = new BitMask_Long();
            if (Character.AStr[name] == "")
                value.Value = 0;
            else
                value.Value = ulong.Parse(Character.AStr[name]);
            value.SetValueForNum(index, v);
            Character.AStr[name] = value.Value.ToString();

            SendTitleList();
        }

        public void SendPetInfo()
        {
            if (Character.Partner == null)
                return;
            Partner.StatusFactory.Instance.CalcPartnerStatus(Character.Partner);
            SendPetDetailInfo();
            SendPetBasicInfo();
        }

        public void SendPetDetailInfo()
        {
            if (Character.Partner != null)
            {
                var p = new SSMG_PARTNER_INFO_DETAIL();
                var pet = Character.Partner;
                p.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                p.MaxHP = pet.MaxHP;
                p.MaxMP = pet.MaxMP;
                p.MaxSP = pet.MaxSP;
                p.MoveSpeed = pet.Speed;
                p.MinPhyATK = pet.Status.min_atk1;
                p.MaxPhyATK = pet.Status.max_atk1;
                p.MinMAGATK = pet.Status.min_matk;
                p.MaxMAGATK = pet.Status.max_matk;
                p.DEF = pet.Status.def;
                p.DEFAdd = pet.Status.def_add;
                p.MDEF = pet.Status.mdef;
                p.MDEFAdd = pet.Status.mdef_add;
                p.ShortHit = pet.Status.hit_melee;
                p.LongHit = pet.Status.hit_ranged;
                p.ShortAvoid = pet.Status.avoid_melee;
                p.LongAvoid = pet.Status.avoid_ranged;
                p.ASPD = pet.Status.aspd;
                p.CSPD = pet.Status.cspd;
                netIO.SendPacket(p);
            }
        }

        public void SendPetBasicInfo()
        {
            if (Character.Partner != null)
            {
                var p = new SSMG_PARTNER_INFO_BASIC();
                var pet = Character.Partner;
                p.InventorySlot = Character.Inventory.Equipments[EnumEquipSlot.PET].Slot;
                p.Level = pet.Level;

                var bexp = ExperienceManager.Instance.GetExpForLevel(pet.Level, LevelType.CLEVEL2);
                var nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(pet.Level + 1), LevelType.CLEVEL2);
                var cexp = (uint)((float)(pet.exp - bexp) / (nextExp - bexp) * 1000);

                p.EXPPercentage = cexp;
                p.Rebirth = 0;
                if (pet.rebirth)
                    p.Rebirth = 1;
                var rank = (byte)(pet.BaseData.base_rank + pet.rank);
                p.Rank = rank;
                p.ReliabilityColor = pet.reliability;
                p.ReliabilityUpRate = pet.reliabilityuprate;

                if (pet.nextfeedtime > DateTime.Now)
                    p.NextFeedTime = (uint)(pet.nextfeedtime - DateTime.Now).TotalSeconds;
                else
                    p.NextFeedTime = 0;

                p.AIMode = pet.ai_mode;
                //p.MaxNextFeedTime = no data
                //p.CustomAISheet = no data
                //p.AICommandCount1 = no data
                //p.AICommandCount2 = no data
                p.PerkPoint = pet.perkpoint;
                //p.PerkListCount = no data
                p.Perk0 = pet.perk0;
                p.Perk1 = pet.perk1;
                p.Perk2 = pet.perk2;
                p.Perk3 = pet.perk3;
                p.Perk4 = pet.perk4;
                p.Perk5 = pet.perk5;
                if (pet.equipments.ContainsKey(EnumPartnerEquipSlot.WEAPON))
                    p.WeaponID = pet.equipments[EnumPartnerEquipSlot.WEAPON].ItemID;
                if (pet.equipments.ContainsKey(EnumPartnerEquipSlot.COSTUME))
                    p.ArmorID = pet.equipments[EnumPartnerEquipSlot.COSTUME].ItemID;
                netIO.SendPacket(p);
            }
        }

        public void OnAnoPaperEquip(CSMG_ANO_PAPER_EQUIP p)
        {
            if (Character.AnotherPapers.ContainsKey(p.paperID))
            {
                Character.UsingPaperID = p.paperID;
                StatusFactory.Instance.CalcStatus(Character);
                var p1 = new SSMG_ANO_EQUIP_RESULT();
                p1.PaperID = Character.UsingPaperID;
                SendPlayerInfo();
                netIO.SendPacket(p1);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PAPER_CHANGE, null, Character, true);
            }
        }

        public void OnAnoPaperTakeOff(CSMG_ANO_PAPER_TAKEOFF p)
        {
            Character.UsingPaperID = 0;
            StatusFactory.Instance.CalcStatus(Character);
            var p1 = new SSMG_ANO_TAKEOFF_RESULT();
            p1.PaperID = Character.UsingPaperID;
            netIO.SendPacket(p1);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PAPER_CHANGE, null, Character, true);
            SendPlayerInfo();
        }

        public void CreateAnotherPaper(uint paperID)
        {
            var detail = new AnotherDetail();
            detail.value = new BitMask_Long();
            detail.lv = 0;
            foreach (var item in AnotherFactory.Instance.AnotherPapers[paperID].Keys)
                if (!detail.skills.ContainsKey(item))
                    detail.skills.Add(item, 0);
            if (!Character.AnotherPapers.ContainsKey(paperID))
                Character.AnotherPapers.Add(paperID, detail);
        }

        public void OnAnoPaperCompound(CSMG_ANO_PAPER_COMPOUND p)
        {
            var penItem = Character.Inventory.GetItem(p.SlotID);
            var paperID = p.paperID;
            var lv = (byte)(Character.AnotherPapers[paperID].lv + 1);
            var value = (ulong)(0xff << (8 * (lv - 1)));
            if (lv == 1) value = 0xff;
            if (Character.AnotherPapers[paperID].value.Test(value))
            {
                if (lv > 1)
                {
                    uint penID = 0;
                    if (AnotherFactory.Instance.AnotherPapers[paperID][lv].requestItem2 == penItem.ItemID)
                        penID = penItem.ItemID;
                    else if (AnotherFactory.Instance.AnotherPapers[paperID][lv].requestItem1 == penItem.ItemID)
                        penID = penItem.ItemID;
                    else return;
                    if (CountItem(penID) < 1) return;
                    DeleteItemID(penID, 1, true);
                }

                Character.AnotherPapers[paperID].lv = lv;
                var p2 = new SSMG_ANO_PAPER_COMPOUND_RESULT();
                p2.lv = lv;
                p2.paperID = paperID;
                netIO.SendPacket(p2);
            }
            else
            {
                return;
            }
        }

        public void OnAnoPaperUse(CSMG_ANO_PAPER_USE p)
        {
            var paperItem = Character.Inventory.GetItem(p.slotID);
            if (paperItem != null)
            {
                var paperID = p.paperID;
                if (AnotherFactory.Instance.AnotherPapers[paperID][1].paperItems1.Contains(paperItem.ItemID))
                {
                    var lv = AnotherFactory.Instance.GetPaperLv(Character.AnotherPapers[paperID].value.Value);
                    var value = GetPaperValue(paperID, (byte)(lv + 1), paperItem.ItemID);
                    if (value == 0) return;
                    if (!Character.AnotherPapers[paperID].value.Test(value))
                        Character.AnotherPapers[paperID].value.SetValue(value, true);
                    else return;
                    DeleteItem(p.slotID, 1, true);
                    var p2 = new SSMG_ANO_PAPER_USE_RESULT();
                    p2.value = Character.AnotherPapers[paperID].value.Value;
                    p2.paperID = paperID;
                    netIO.SendPacket(p2);
                    MapServer.charDB.SavePaper(Character);
                }
            }
        }

        public ulong GetPaperValue(byte paperID, byte lv, uint ItemID)
        {
            ulong value = 0;
            if (!AnotherFactory.Instance.AnotherPapers.ContainsKey(paperID)) return 0;
            if (!AnotherFactory.Instance.AnotherPapers[paperID].ContainsKey(lv)) return 0;
            if (!AnotherFactory.Instance.AnotherPapers[paperID][lv].paperItems1.Contains(ItemID)) return 0;
            var index = AnotherFactory.Instance.AnotherPapers[paperID][lv].paperItems1.IndexOf(ItemID);
            switch (index)
            {
                case 0:
                    value = 0x1;
                    break;
                case 1:
                    value = 0x2;
                    break;
                case 2:
                    value = 0x4;
                    break;
                case 3:
                    value = 0x8;
                    break;
                case 4:
                    value = 0x10;
                    break;
                case 5:
                    value = 0x20;
                    break;
                case 6:
                    value = 0x40;
                    break;
                case 7:
                    value = 0x80;
                    break;
            }

            value = value << (8 * (lv - 1));
            return value;
        }

        public void OnAnoUIOpen(CSMG_ANO_UI_OPEN p)
        {
            try
            {
                CreateAnotherPaper(1);
                CreateAnotherPaper(2);
                CreateAnotherPaper(4);
                CreateAnotherPaper(6);
                CreateAnotherPaper(7);
                CreateAnotherPaper(8);
                CreateAnotherPaper(9);
                CreateAnotherPaper(10);
                CreateAnotherPaper(11);
                CreateAnotherPaper(13);
                var List1 = new List<ushort>();
                var List2 = new List<ulong>();
                var List3 = new List<byte>();
                var p2 = new SSMG_ANO_SHOW_INFOBOX();
                p2.index = p.index;
                p2.cexp = 0;
                p2.usingPaperID = Character.UsingPaperID;
                foreach (var item in Character.AnotherPapers.Keys)
                    if (AnotherFactory.Instance.AnotherPapers.ContainsKey(item))
                        if (AnotherFactory.Instance.AnotherPapers[item][1].type == p.index)
                            List1.Add((ushort)item);

                p2.papersID = List1;
                if (Character.UsingPaperID != 0)
                    p2.usingPaperValue = Character.AnotherPapers[Character.UsingPaperID].value.Value;
                for (var i = 0; i < List1.Count; i++) List2.Add(Character.AnotherPapers[List1[i]].value.Value);
                p2.paperValues = List2;
                if (Character.UsingPaperID != 0)
                    p2.usingLv =
                        Character.AnotherPapers[Character.UsingPaperID]
                            .lv; //AnotherFactory.Instance.GetPaperLv(this.Character.AnotherPapers[this.Character.UsingPaperID].value.Value);
                for (var i = 0; i < List1.Count; i++)
                    List3.Add(Character.AnotherPapers[List1[i]]
                        .lv); //AnotherFactory.Instance.GetPaperLv(this.Character.AnotherPapers[List1[i]].value.Value));
                p2.papersLv = List3;
                /*if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_1 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[0];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[1]);
                }
                p2.paperSkillsEXP_1 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_2 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[1];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[2]);
                }
                p2.paperSkillsEXP_2 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_3 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[2];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[3]);
                }
                p2.paperSkillsEXP_3 = List2;
                if (this.Character.UsingPaperID != 0)
                    p2.usingSkillEXP_4 = this.Character.AnotherPapers[this.Character.UsingPaperID].skills[3];
                List2 = new List<ulong>();
                for (int i = 0; i < List1.Count; i++)
                {
                    List2.Add(this.Character.AnotherPapers[List1[i]].skills[4]);
                }
                p2.paperSkillsEXP_4 = List2;*/
                netIO.SendPacket(p2);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnMirrorOpenRequire(CSMG_PLAYER_MIRROR_OPEN p)
        {
            var p1 = new SSMG_PLAYER_OPEN_MIRROR_WINDOW();
            p1.SetFace = new List<ushort>(15);
            p1.SetHairStyle = new List<ushort>(15);
            p1.SetHairColor = new List<ushort>(15);
            p1.SetUnknow = new List<uint>(15);
            p1.SetHairColorStorageSlot = new List<byte>(15);
            netIO.SendPacket(p1);
        }

        public void OnRequireRebirthReward(CSMG_PLAYER_REQUIRE_REBIRTHREWARD p)
        {
            var p1 = new SSMG_PLAYER_OPEN_REBIRTHREWARD_WINDOW();
            p1.SetOpen = 0x0A;
            netIO.SendPacket(p1);
        }

        public void OnCharFormChange(CSMG_CHAR_FORM p)
        {
            Character.TailStyle = p.tailstyle;
            Character.WingStyle = p.wingstyle;
            Character.WingColor = p.wingcolor;
            Character.e.PropertyUpdate(UpdateEvent.CHAR_INFO, 0);
        }

        public void OnPlayerFaceView(CSMG_ITEM_FACEVIEW p)
        {
            var p2 = new Packet(3);
            p2.ID = 0x1CF3;
            netIO.SendPacket(p2);
        }

        public void OnPlayerFaceChange(CSMG_ITEM_FACECHANGE p)
        {
            var itemID = Character.Inventory.GetItem(p.SlotID).ItemID;
            if (itemID == FaceFactory.Instance.Faces[p.FaceID])
            {
                DeleteItem(p.SlotID, 1, true);
                Character.Face = p.FaceID;
                SendPlayerInfo();
            }
        }

        /*public void SendNaviList(Packets.Client.CSMG_NAVI_OPEN p)
        {
            Packets.Server.SSMG_NAVI_LIST p1 = new Packets.Server.SSMG_NAVI_LIST();
            p1.CategoryId = p.CategoryId;
            p1.Count = 18;
            p1.Navi = this.Character.Navi;
            this.netIO.SendPacket(p1);
        }*/
        public void SendAnotherButton()
        {
            var p = new SSMG_ANO_BUTTON_APPEAR();
            p.Type = 1;
            netIO.SendPacket(p);
        }

        public void SendRingFF()
        {
            MapServer.charDB.GetFF(Character);
            if (Character.Ring != null)
                if (Character.Ring.FFarden != null)
                {
                    SendRingFFObtainMode();
                    SendRingFFHealthMode();
                    SendRingFFIsLock();
                    SendRingFFName();
                    SendRingFFMaterialPoint();
                    SendRingFFMaterialConsume();
                    SendRingFFLevel();
                    SendRingFFNextFeeTime();
                }
        }

        private void SendRingFFObtainMode()
        {
            var p = new SSMG_FF_OBTAIN_MODE();
            p.value = Character.Ring.FFarden.ObMode;
            netIO.SendPacket(p);
        }

        private void SendRingFFHealthMode()
        {
            var p = new SSMG_FF_HEALTH_MODE();
            p.value = Character.Ring.FFarden.HealthMode;
            netIO.SendPacket(p);
        }

        private void SendRingFFIsLock()
        {
            var p = new SSMG_FF_ISLOCK();
            if (Character.Ring.FFarden.IsLock)
                p.value = 1;
            else
                p.value = 0;
            netIO.SendPacket(p);
        }

        private void SendRingFFName()
        {
            var p = new SSMG_FF_RINGSELF();
            p.name = Character.Ring.FFarden.Name;
            netIO.SendPacket(p);
        }

        private void SendRingFFMaterialPoint()
        {
            var p = new SSMG_FF_MATERIAL_POINT();
            p.value = Character.Ring.FFarden.MaterialPoint;
            netIO.SendPacket(p);
        }

        private void SendRingFFMaterialConsume()
        {
            var p = new SSMG_FF_MATERIAL_CONSUME();
            p.value = Character.Ring.FFarden.MaterialConsume;
            netIO.SendPacket(p);
        }

        private void SendRingFFLevel()
        {
            var p = new SSMG_FF_LEVEL();
            p.level = Character.Ring.FFarden.Level;
            p.value = Character.Ring.FFarden.FFexp;
            netIO.SendPacket(p);
            var p1 = new SSMG_FF_F_LEVEL();
            p1.level = Character.Ring.FFarden.FLevel;
            p1.value = Character.Ring.FFarden.FFFexp;
            netIO.SendPacket(p1);
            var p2 = new SSMG_FF_SU_LEVEL();
            p2.level = Character.Ring.FFarden.SULevel;
            p2.value = Character.Ring.FFarden.FFSUexp;
            netIO.SendPacket(p2);
            var p3 = new SSMG_FF_BP_LEVEL();
            p3.level = Character.Ring.FFarden.BPLevel;
            p3.value = Character.Ring.FFarden.FFBPexp;
            netIO.SendPacket(p3);
            var p4 = new SSMG_FF_DEM_LEVEL();
            p4.level = Character.Ring.FFarden.DEMLevel;
            p4.value = Character.Ring.FFarden.FFDEMexp;
            netIO.SendPacket(p4);
        }

        private void SendRingFFNextFeeTime()
        {
            var p = new SSMG_FF_NEXTFEE_DATE();
            p.UpdateTime = DateTime.Now;
            netIO.SendPacket(p);
        }


        private void SendEffect(uint effect)
        {
            var arg = new EffectArg();
            arg.actorID = Character.ActorID;
            arg.effectID = effect;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Character, true);
        }

        public void ResetStatusPoint()
        {
            var setting = Configuration.Instance.StartupSetting[Character.Race];
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Str, Character.Str);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Dex, Character.Dex);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Int, Character.Int);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Vit, Character.Vit);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Agi, Character.Agi);
            Character.StatsPoint += StatusFactory.Instance.GetTotalBonusPointForStats(setting.Mag, Character.Mag);

            Character.Str = setting.Str;
            Character.Dex = setting.Dex;
            Character.Int = setting.Int;
            Character.Vit = setting.Vit;
            Character.Agi = setting.Agi;
            Character.Mag = setting.Mag;

            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        public void SendRange()
        {
            var p = new SSMG_ITEM_EQUIP();
            p.InventorySlot = 0xFFFFFFFF;
            p.Target = ContainerType.NONE;
            p.Result = 1;
            p.Range = Character.Range;
            netIO.SendPacket(p);
        }

        public void SendActorID()
        {
            var p2 = new SSMG_ACTOR_SPEED();
            p2.ActorID = Character.ActorID;
            p2.Speed = Character.Speed;
            //p2.Speed = 96;
            netIO.SendPacket(p2);
        }

        public void SendStamp()
        {
            var p1 = new SSMG_STAMP_INFO();
            p1.Page = 0;
            p1.Stamp = Character.Stamp;
            netIO.SendPacket(p1);
            var p2 = new SSMG_STAMP_INFO();
            p2.Page = 1;
            p2.Stamp = Character.Stamp;
            netIO.SendPacket(p2);
        }

        public void SendActorMode()
        {
            Character.e.OnPlayerMode(Character);
        }

        public void SendCharOption()
        {
            var sum = 0;
            if (Character.CInt["canTrade"] == 0) sum += 1;
            if (Character.CInt["canParty"] == 0) sum += 2;
            if (Character.CInt["canPossession"] == 0) sum += 4;
            if (Character.CInt["canRing"] == 0) sum += 8;
            if (Character.CInt["showRevive"] == 0) sum += 16;
            if (Character.CInt["canWork"] == 0) sum += 32;
            if (Character.CInt["canMentor"] == 0) sum += 256;
            if (Character.CInt["showEquipment"] == 0) sum += 512;
            if (Character.CInt["canChangePartnerDisplay"] == 0) sum += 1024;
            if (Character.CInt["canFriend"] == 0) sum += 2048;

            if (sum == 0)
            {
                var p4 = new SSMG_ACTOR_OPTION();
                p4.Option = SSMG_ACTOR_OPTION.Options.NONE;
                netIO.SendPacket(p4);
            }
            else
            {
                var p4 = new SSMG_ACTOR_OPTION();
                p4.RawOption = sum;
                netIO.SendPacket(p4);
            }
        }

        public void SendCharInfo()
        {
            if (Character.Online)
            {
                SkillHandler.Instance.CastPassiveSkills(Character);

                SendAttackType();
                var p1 = new SSMG_PLAYER_INFO();
                p1.Player = Character;
                netIO.SendPacket(p1);

                SendPlayerInfo();
            }
        }

        public void SendPlayerInfo()
        {
            if (Character.Online)
            {
                SendPlayerStatsBreak(Character);
                SendGoldUpdate();
                SendActorHPMPSP(Character);
                SendStatus();
                SendRange();
                SendStatusExtend();
                SendCapacity();
                //SendMaxCapacity();
                SendPlayerJob();
                SendSkillList();
                SendPlayerLevel();
                SendEXP();
                SendActorMode();
                SendCL();
                SendMotionList();
                SendPlayerEXPoints(Character);

                SendPlayerDualJobInfo();
                SendPlayerDualJobSkillList();
            }
        }

        private void SendPlayerStatsBreak(ActorPC actor)
        {
            var p = new SSMG_PLAYER_STATS_BREAK();
            p.STATS = (byte)(StatsBreakType.Str | StatsBreakType.Agi | StatsBreakType.Vit | StatsBreakType.Int |
                             StatsBreakType.Dex | StatsBreakType.Mag);
            netIO.SendPacket(p);
        }

        private void SendPlayerEXPoints(ActorPC actor)
        {
            var p = new SSMG_PLAYER_EXPOINT();
            p.EXStatPoint = actor.EXStatPoint;
            p.CanUseStatPoint = actor.StatsPoint;
            p.EXSkillPoint = actor.EXSkillPoint;
            netIO.SendPacket(p);
        }

        public void SendMotionList()
        {
            if (Character.Online)
            {
                var p = new SSMG_CHAT_EXPRESSION_UNLOCK();
                p.unlock = 0xffffffff;
                netIO.SendPacket(p);
                var p2 = new SSMG_CHAT_EXEMOTION_UNLOCK();
                p2.List1 = 0xffffffff;
                p2.List2 = 0xffffffff;
                p2.List3 = 0xffffffff;
                p2.List4 = 0xffffffff;
                p2.List5 = 0xffffffff;
                netIO.SendPacket(p2);
            }
        }

        public void SendAttackType()
        {
            if (Character.Online)
            {
                //去掉攻击类型消息显示
                Dictionary<EnumEquipSlot, Item> equips;
                if (Character.Form == DEM_FORM.NORMAL_FORM)
                    equips = Character.Inventory.Equipments;
                else
                    equips = Character.Inventory.Parts;
                if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND) || (equips.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                                                                     equips[EnumEquipSlot.LEFT_HAND].BaseData
                                                                         .itemType == ItemType.BOW))
                {
                    var item = new Item();
                    if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                        equips[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BOW)
                        item = equips[EnumEquipSlot.LEFT_HAND];
                    else
                        item = equips[EnumEquipSlot.RIGHT_HAND];
                    Character.Status.attackType = item.AttackType;
                    //switch (item.AttackType)
                    //{
                    //    case ATTACK_TYPE.BLOW:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREBLOW_TEXT);
                    //        break;
                    //    case ATTACK_TYPE.STAB:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTURESTAB_TEXT);
                    //        break;
                    //    case ATTACK_TYPE.SLASH:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTURESLASH_TEXT);
                    //        break;
                    //    default:
                    //        SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREERROR_TEXT);
                    //        break;
                    //}
                }
                else
                {
                    Character.Status.attackType = ATTACK_TYPE.BLOW;
                    //SendSystemMessage(SagaMap.Packets.Server.SSMG_SYSTEM_MESSAGE.Messages.GAME_SMSG_RECV_POSTUREBLOW_TEXT);
                }

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK_TYPE_CHANGE, null, Character, true);
            }
        }

        public void SendStatus()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_STATUS();
                if (Character.Form == DEM_FORM.MACHINA_FORM || Character.Race != PC_RACE.DEM)
                {
                    p.AgiBase = (ushort)(Character.Agi + Character.Status.m_agi_chip);
                    p.AgiRevide = (short)(Character.Status.agi_rev + Character.Status.agi_item +
                                          Character.Status.agi_mario + Character.Status.agi_skill +
                                          Character.Status.agi_iris);
                    p.AgiBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                    p.DexBase = (ushort)(Character.Dex + Character.Status.m_dex_chip);
                    p.DexRevide = (short)(Character.Status.dex_rev + Character.Status.dex_item +
                                          Character.Status.dex_mario + Character.Status.dex_skill +
                                          Character.Status.dex_iris);
                    p.DexBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                    p.IntBase = (ushort)(Character.Int + Character.Status.m_int_chip);
                    p.IntRevide = (short)(Character.Status.int_rev + Character.Status.int_item +
                                          Character.Status.int_mario + Character.Status.int_skill +
                                          Character.Status.int_iris);
                    p.IntBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                    p.VitBase = (ushort)(Character.Vit + Character.Status.m_vit_chip);
                    p.VitRevide = (short)(Character.Status.vit_rev + Character.Status.vit_item +
                                          Character.Status.vit_mario + Character.Status.vit_skill +
                                          Character.Status.vit_iris);
                    p.VitBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                    p.StrBase = (ushort)(Character.Str + Character.Status.m_str_chip);
                    p.StrRevide = (short)(Character.Status.str_rev + Character.Status.str_item +
                                          Character.Status.str_mario + Character.Status.str_skill +
                                          Character.Status.str_iris);
                    p.StrBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                    p.MagBase = (ushort)(Character.Mag + Character.Status.m_mag_chip);
                    p.MagRevide = (short)(Character.Status.mag_rev + Character.Status.mag_item +
                                          Character.Status.mag_mario + Character.Status.mag_skill +
                                          Character.Status.mag_iris);
                    p.MagBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                    netIO.SendPacket(p);
                }
                else
                {
                    p.AgiBase = (ushort)(Character.Agi + Character.Status.m_agi_chip);
                    p.AgiRevide = (short)(Character.Status.agi_rev - Character.Status.m_agi_chip +
                                          Character.Status.agi_item + Character.Status.agi_mario +
                                          Character.Status.agi_skill + Character.Status.agi_iris);
                    p.AgiBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                    p.DexBase = (ushort)(Character.Dex + Character.Status.m_dex_chip);
                    p.DexRevide = (short)(Character.Status.dex_rev - Character.Status.m_dex_chip +
                                          Character.Status.dex_item + Character.Status.dex_mario +
                                          Character.Status.dex_skill + Character.Status.dex_iris);
                    p.DexBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                    p.IntBase = (ushort)(Character.Int + Character.Status.m_int_chip);
                    p.IntRevide = (short)(Character.Status.int_rev - Character.Status.m_int_chip +
                                          Character.Status.int_item + Character.Status.int_mario +
                                          Character.Status.int_skill + Character.Status.int_iris);
                    p.IntBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                    p.VitBase = (ushort)(Character.Vit + Character.Status.m_vit_chip);
                    p.VitRevide = (short)(Character.Status.vit_rev - Character.Status.m_vit_chip +
                                          Character.Status.vit_item + Character.Status.vit_mario +
                                          Character.Status.vit_skill + Character.Status.vit_iris);
                    p.VitBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                    p.StrBase = (ushort)(Character.Str + Character.Status.m_str_chip);
                    p.StrRevide = (short)(Character.Status.str_rev - Character.Status.m_str_chip +
                                          Character.Status.str_item + Character.Status.str_mario +
                                          Character.Status.str_skill + Character.Status.str_iris);
                    p.StrBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                    p.MagBase = (ushort)(Character.Mag + Character.Status.m_mag_chip);
                    p.MagRevide = (short)(Character.Status.mag_rev - Character.Status.m_mag_chip +
                                          Character.Status.mag_item + Character.Status.mag_mario +
                                          Character.Status.mag_skill + Character.Status.mag_iris);
                    p.MagBonus = StatusFactory.Instance.RequiredBonusPoint(Character.Mag);

                    netIO.SendPacket(p);
                }
            }
        }

        public void SendStatusExtend()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_STATUS_EXTEND();

                switch (Character.Status.attackType)
                {
                    case ATTACK_TYPE.BLOW:
                        p.ATK1Max = Character.Status.max_atk1;
                        p.ATK1Min = Character.Status.min_atk1;
                        break;
                    case ATTACK_TYPE.SLASH:
                        p.ATK1Max = Character.Status.max_atk2;
                        p.ATK1Min = Character.Status.min_atk2;
                        break;
                    case ATTACK_TYPE.STAB:
                        p.ATK1Max = Character.Status.max_atk3;
                        p.ATK1Min = Character.Status.min_atk3;
                        break;
                }

                p.ATK2Max = Character.Status.max_atk2;
                p.ATK2Min = Character.Status.min_atk2;
                p.ATK3Max = Character.Status.max_atk3;
                p.ATK3Min = Character.Status.min_atk3;
                p.MATKMax = Character.Status.max_matk;
                p.MATKMin = Character.Status.min_matk;

                p.ASPD = Character.Status.aspd; // + this.Character.Status.aspd_skill);
                p.CSPD = Character.Status.cspd; // + this.Character.Status.cspd_skill);

                p.AvoidCritical = Character.Status.avoid_critical;
                p.AvoidMagic = Character.Status.avoid_magic;
                p.AvoidMelee = Character.Status.avoid_melee;
                p.AvoidRanged = Character.Status.avoid_ranged;

                p.DefAddition = (ushort)Character.Status.def_add;
                p.DefBase = Character.Status.def;
                p.MDefAddition = (ushort)Character.Status.mdef_add;
                p.MDefBase = Character.Status.mdef;

                p.HitCritical = Character.Status.hit_critical;
                p.HitMagic = Character.Status.hit_magic;
                p.HitMelee = Character.Status.hit_melee;
                p.HitRanged = Character.Status.hit_ranged;

                p.Speed = Character.Speed;

                netIO.SendPacket(p);
            }
        }

        public void SendCapacity()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_CAPACITY();
                /*p.CapacityBack = this.Character.Inventory.Volume[ContainerType.BACK_BAG];
                p.CapacityBody = this.Character.Inventory.Volume[ContainerType.BODY];
                p.CapacityLeft = this.Character.Inventory.Volume[ContainerType.LEFT_BAG];
                p.CapacityRight = this.Character.Inventory.Volume[ContainerType.RIGHT_BAG];
                p.PayloadBack = this.Character.Inventory.Payload[ContainerType.BACK_BAG];
                p.PayloadBody = this.Character.Inventory.Payload[ContainerType.BODY];
                p.PayloadLeft = this.Character.Inventory.Payload[ContainerType.LEFT_BAG];
                p.PayloadRight = this.Character.Inventory.Payload[ContainerType.RIGHT_BAG];*/
                p.Payload = Character.Inventory.Payload[ContainerType.BODY];
                p.Volume = Character.Inventory.Volume[ContainerType.BODY];
                p.MaxPayload = Character.Inventory.MaxPayload[ContainerType.BODY];
                p.MaxVolume = Character.Inventory.MaxVolume[ContainerType.BODY];
                netIO.SendPacket(p);
            }
        }

        public void SendMaxCapacity()
        {
            /*if (this.Character.Online)
            {
                Packets.Server.SSMG_PLAYER_MAX_CAPACITY p = new SagaMap.Packets.Server.SSMG_PLAYER_MAX_CAPACITY();
                /*p.CapacityBack = this.Character.Inventory.MaxVolume[ContainerType.BACK_BAG];
                p.CapacityBody = this.Character.Inventory.MaxVolume[ContainerType.BODY];
                p.CapacityLeft = this.Character.Inventory.MaxVolume[ContainerType.LEFT_BAG];
                p.CapacityRight = this.Character.Inventory.MaxVolume[ContainerType.RIGHT_BAG];
                p.PayloadBack = this.Character.Inventory.MaxPayload[ContainerType.BACK_BAG];
                p.PayloadBody = this.Character.Inventory.MaxPayload[ContainerType.BODY];
                p.PayloadLeft = this.Character.Inventory.MaxPayload[ContainerType.LEFT_BAG];
                p.PayloadRight = this.Character.Inventory.MaxPayload[ContainerType.RIGHT_BAG];
                p.Payload = this.Character.Inventory.MaxPayload[ContainerType.BODY];
                p.Volume = this.Character.Inventory.MaxVolume[ContainerType.BODY];
                this.netIO.SendPacket(p);
            }*/
        }

        public void SendChangeMap()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_CHANGE_MAP();
                if (map.returnori)
                    p.MapID = map.OriID;
                else
                    p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                if (map.IsDungeon)
                {
                    p.DungeonDir = map.DungeonMap.Dir;
                    p.DungeonX = map.DungeonMap.X;
                    p.DungeonY = map.DungeonMap.Y;
                    Character.Speed = Configuration.Instance.Speed;
                }

                if (fgTakeOff)
                {
                    p.FGTakeOff = fgTakeOff;
                    fgTakeOff = false;
                }

                //this.Character.Speed = Configuration.Instance.Speed;
                netIO.SendPacket(p);
            }
        }

        public void SendGotoFG()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (!fgMap.IsMapInstance)
                    Logger.ShowDebug(string.Format("MapID:{0} isn't a valid flying garden!"), Logger.defaultlogger);
                var owner = fgMap.Creator;
                var p = new SSMG_PLAYER_GOTO_FG();
                p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                p.Equiptments = owner.FGarden.FGardenEquipments;
                netIO.SendPacket(p);
            }
        }

        public void SendGotoFF()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (fgMap.ID == 90001999) //铲除一个神经病逻辑
                {
                    CustomMapManager.Instance.SendGotoSerFFMap(this);
                    return;
                }

                if (!fgMap.IsMapInstance)
                    Logger.ShowDebug(string.Format("MapID:{0} isn't a valid flying garden!"), Logger.defaultlogger);
                var owner = fgMap.Creator;
                var p = new SSMG_FF_ENTER();
                p.MapID = Character.MapID;
                p.X = Global.PosX16to8(Character.X, map.Width);
                p.Y = Global.PosY16to8(Character.Y, map.Height);
                p.Dir = (byte)(Character.Dir / 45);
                p.RingID = Character.Ring.ID;
                p.RingHouseID = 30250000;
                netIO.SendPacket(p);
            }
        }

        public void SendDungeonEvent()
        {
            if (Character.Online)
            {
                if (!map.IsMapInstance || !map.IsDungeon)
                    return;
                foreach (var i in map.DungeonMap.Gates.Keys)
                {
                    if (map.DungeonMap.Gates[i].NPCID != 0)
                    {
                        var p = new SSMG_NPC_SHOW();
                        p.NPCID = map.DungeonMap.Gates[i].NPCID;
                        netIO.SendPacket(p);
                    }

                    if (map.DungeonMap.Gates[i].ConnectedMap != null)
                    {
                        if (i != GateType.Central && i != GateType.Exit)
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;

                            switch (i)
                            {
                                case GateType.North:
                                    p1.EventID = 12001501;
                                    break;
                                case GateType.East:
                                    p1.EventID = 12001502;
                                    break;
                                case GateType.South:
                                    p1.EventID = 12001503;
                                    break;
                                case GateType.West:
                                    p1.EventID = 12001504;
                                    break;
                            }

                            switch (map.DungeonMap.Gates[i].Direction)
                            {
                                case Direction.In:
                                    p1.EffectID = 9002;
                                    break;
                                case Direction.Out:
                                    p1.EffectID = 9005;
                                    break;
                            }

                            netIO.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;
                            p1.EventID = 12001505;
                            p1.EffectID = 9005;
                            netIO.SendPacket(p1);
                        }

                        if (map.DungeonMap.Gates[i].NPCID != 0)
                        {
                            var p = new SSMG_CHAT_MOTION();
                            p.ActorID = map.DungeonMap.Gates[i].NPCID;
                            p.Motion = (MotionType)621;
                            netIO.SendPacket(p);
                        }
                    }
                    else
                    {
                        if (i == GateType.Entrance)
                        {
                            var p1 = new SSMG_NPC_SET_EVENT_AREA();
                            p1.StartX = map.DungeonMap.Gates[i].X;
                            p1.EndX = map.DungeonMap.Gates[i].X;
                            p1.StartY = map.DungeonMap.Gates[i].Y;
                            p1.EndY = map.DungeonMap.Gates[i].Y;
                            p1.EventID = 12001505;
                            p1.EffectID = 9003;
                            netIO.SendPacket(p1);
                        }
                    }
                }
            }
        }

        public void SendFGEvent()
        {
            if (Character.Online)
            {
                var fgMap = MapManager.Instance.GetMap(Character.MapID);
                if (!fgMap.IsMapInstance)
                    return;
                if (map.ID / 10 == 7000000)
                    if (map.Creator.FGarden.FGardenEquipments[FGardenSlot.GARDEN_MODELHOUSE] != 0)
                    {
                        var p1 = new SSMG_NPC_SET_EVENT_AREA();
                        p1.EventID = 10000315;
                        p1.StartX = 6;
                        p1.StartY = 7;
                        p1.EndX = 6;
                        p1.EndY = 7;
                        netIO.SendPacket(p1);
                    }
            }
        }

        public void SendGoldUpdate()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_GOLD_UPDATE();
                p.Gold = (ulong)Character.Gold;
                netIO.SendPacket(p);
            }
        }

        public void SendActorHPMPSP(Actor actor)
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_MAX_HPMPSP();
                p.ActorID = actor.ActorID;
                p.MaxHP = actor.MaxHP;
                p.MaxMP = actor.MaxMP;
                p.MaxSP = actor.MaxSP;
                p.MaxEP = actor.MaxEP;
                netIO.SendPacket(p);
                var p10 = new SSMG_PLAYER_HPMPSP();
                p10.ActorID = actor.ActorID;
                p10.HP = actor.HP;
                p10.MP = actor.MP;
                p10.SP = actor.SP;
                p10.EP = actor.EP;
                netIO.SendPacket(p10);
                if (actor == Character)
                    if (Character.Party != null)
                        PartyManager.Instance.UpdateMemberHPMPSP(Character.Party, Character);
                //if ((DateTime.Now - hpmpspStamp).TotalSeconds >= 2)
                //{
                //    hpmpspStamp = DateTime.Now;
                //}
            }
        }

        public void SendCharXY()
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_MOVE();
                p.ActorID = Character.ActorID;
                p.Dir = Character.Dir;
                p.X = Character.X;
                p.Y = Character.Y;
                p.MoveType = MoveType.WARP;
                netIO.SendPacket(p);
            }
        }

        public void SendPlayerLevel()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_LEVEL();
                p.Level = Character.Level;
                p.JobLevel = Character.JobLevel1;
                p.JobLevel2T = Character.JobLevel2T;
                p.JobLevel2X = Character.JobLevel2X;
                p.JobLevel3 = Character.JobLevel3;
                p.IsDualJob = Character.DualJobID != 0 ? (byte)1 : (byte)0;
                if (p.IsDualJob == 0x1)
                    p.DualjobLevel = Character.PlayerDualJobList[Character.DualJobID].DualJobLevel;

                p.UseableStatPoint = Character.StatsPoint;
                p.SkillPoint = Character.SkillPoint;
                p.Skill2XPoint = Character.SkillPoint2X;
                p.Skill2TPoint = Character.SkillPoint2T;
                p.Skill3Point = Character.SkillPoint3;
                netIO.SendPacket(p);
            }
        }

        public void SendPlayerJob()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_JOB();
                p.Job = Character.Job;
                if (Character.JobJoint != PC_JOB.NONE)
                    p.JointJob = Character.JobJoint;
                if (Character.DualJobID != 0)
                    p.DualJob = Character.DualJobID;
                netIO.SendPacket(p);
            }
        }


        public void SendCharInfoUpdate()
        {
            if (Character.Online)
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
        }

        public void SendAnnounce(string text)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_CHAT_PUBLIC();
                p11.ActorID = 0;
                p11.Message = text;
                netIO.SendPacket(p11);
            }
        }

        public void SendPkMode()
        {
            if (Character.Online)
            {
                Character.Mode = PlayerMode.COLISEUM_MODE;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, Character, true);
            }
        }

        public void SendNormalMode()
        {
            if (Character.Online)
            {
                Character.Mode = PlayerMode.NORMAL;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, Character, true);
            }
        }

        public void SendPlayerSizeUpdate()
        {
            if (Character.Online)
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_SIZE_UPDATE, null, Character, true);
        }

        public void OnMove(CSMG_PLAYER_MOVE p)
        {
            if (Character.Online)
            {
                if (state != SESSION_STATE.LOADED)
                    return;
                switch (p.MoveType)
                {
                    case MoveType.RUN:
                        Map.MoveActor(Map.MOVE_TYPE.START, Character, new short[2] { p.X, p.Y }, p.Dir,
                            Character.Speed);
                        moveCheckStamp = DateTime.Now;
                        break;
                    case MoveType.CHANGE_DIR:
                        Map.MoveActor(Map.MOVE_TYPE.STOP, Character, new short[2] { p.X, p.Y }, p.Dir, Character.Speed);
                        break;
                    case MoveType.WALK:
                        Map.MoveActor(Map.MOVE_TYPE.START, Character, new short[2] { p.X, p.Y }, p.Dir, Character.Speed,
                            false, MoveType.WALK);
                        moveCheckStamp = DateTime.Now;
                        break;
                }

                if (Character.CInt["NextMoveEventID"] != 0)
                {
                    EventActivate((uint)Character.CInt["NextMoveEventID"]);
                    Character.CInt["NextMoveEventID"] = 0;
                    Character.CInt.Remove("NextMoveEventID");
                }

                if (Character.TTime["特殊刀攻击间隔"] != DateTime.Now)
                    Character.TTime["特殊刀攻击间隔"] = DateTime.Now;
            }
        }

        public void SendActorSpeed(Actor actor, ushort speed)
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_SPEED();
                p.ActorID = actor.ActorID;
                p.Speed = speed;
                netIO.SendPacket(p);
            }
        }

        public void SendEXP()
        {
            if (Character.Online)
            {
                var p = new SSMG_PLAYER_EXP();
                ulong cexp, jexp;
                ulong bexp = 0, nextExp = 0;
                if (!Character.Rebirth || Character.Job != Character.Job3)
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.Level + 1), LevelType.CLEVEL);
                }
                else
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.Level, LevelType.CLEVEL2);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.Level + 1), LevelType.CLEVEL2);
                }

                cexp = (uint)((float)(Character.CEXP - bexp) / (nextExp - bexp) * 1000);
                if (Character.JobJoint == PC_JOB.NONE)
                {
                    if (Character.DualJobID != 0)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(
                            Character.PlayerDualJobList[Character.DualJobID].DualJobLevel, LevelType.DUALJ);
                        nextExp = ExperienceManager.Instance.GetExpForLevel(
                            (uint)(Character.PlayerDualJobList[Character.DualJobID].DualJobLevel + 1), LevelType.DUALJ);
                    }
                    else if (Character.Job == Character.JobBasic)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel1, LevelType.JLEVEL);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel1 + 1),
                            LevelType.JLEVEL);
                    }
                    else if (Character.Job == Character.Job2X)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel2X + 1),
                            LevelType.JLEVEL2);
                    }
                    else if (Character.Job == Character.Job2T)
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel2T + 1),
                            LevelType.JLEVEL2);
                    }
                    else
                    {
                        bexp = ExperienceManager.Instance.GetExpForLevel(Character.JobLevel3, LevelType.JLEVEL3);
                        nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JobLevel3 + 1),
                            LevelType.JLEVEL3);
                    }

                    if (Character.DualJobID != 0)
                        jexp = (uint)((float)(Character.PlayerDualJobList[Character.DualJobID].DualJobExp - bexp) /
                            (nextExp - bexp) * 1000);
                    else
                        jexp = (uint)((float)(Character.JEXP - bexp) / (nextExp - bexp) * 1000);
                }
                else
                {
                    bexp = ExperienceManager.Instance.GetExpForLevel(Character.JointJobLevel, LevelType.JLEVEL2);
                    nextExp = ExperienceManager.Instance.GetExpForLevel((uint)(Character.JointJobLevel + 1),
                        LevelType.JLEVEL2);

                    jexp = (uint)((float)(Character.JointJEXP - bexp) / (nextExp - bexp) * 1000);
                }

                p.EXPPercentage = (uint)cexp >= 1000 ? 999 : (uint)cexp;
                p.JEXPPercentage = (uint)jexp >= 1000 ? 999 : (uint)jexp;
                p.WRP = Character.WRP;
                p.ECoin = Character.ECoin;
                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    p.Exp = (uint)Character.DominionCEXP;
                    p.JExp = (uint)Character.DominionJEXP;
                }
                else
                {
                    p.Exp = (long)Character.CEXP;
                    if (Character.DualJobID != 0)
                        p.JExp = (long)Character.PlayerDualJobList[Character.DualJobID].DualJobExp;
                    else
                        p.JExp = (long)Character.JEXP;
                }

                netIO.SendPacket(p);
            }
        }

        public void SendEXPMessage(long exp, long jexp, long pexp, SSMG_PLAYER_EXP_MESSAGE.EXP_MESSAGE_TYPE type)
        {
            var p = new SSMG_PLAYER_EXP_MESSAGE();
            p.EXP = exp;
            p.JEXP = jexp;
            p.PEXP = pexp;
            p.Type = type;
            netIO.SendPacket(p);
        }

        public void SendLvUP(Actor pc, byte type)
        {
            if (Character.Online)
            {
                var p = new SSMG_ACTOR_LEVEL_UP();
                p.ActorID = pc.ActorID;
                p.Level = pc.Level;
                p.LvType = type;

                netIO.SendPacket(p);
            }
        }

        public void OnPlayerGreetings(CSMG_PLAYER_GREETINGS p)
        {
            if (Character.TTime["打招呼时间"] + new TimeSpan(0, 0, 10) > DateTime.Now)
            {
                SendSystemMessage("不可以频繁打招呼哦。");
                return;
            }

            var actor = map.GetActor(p.ActorID);
            if (actor.Buff.FishingState)
            {
                SendSystemMessage("对方正在钓鱼，不要打扰人家哦。");
                return;
            }

            if (actor != null)
                if (actor.type == ActorType.PC)
                {
                    var target = (ActorPC)actor;
                    if (target.Online && Character.Online)
                    {
                        var dir = map.CalcDir(Character.X, Character.Y, target.X, target.Y);
                        var dir2 = map.CalcDir(target.X, target.Y, Character.X, Character.Y);

                        var ys = new short[2];
                        ys[0] = Character.X;
                        ys[1] = Character.Y;
                        map.MoveActor(Map.MOVE_TYPE.START, Character, ys, dir, 500, true, MoveType.CHANGE_DIR);
                        if (Character.Partner != null)
                        {
                            ys = new short[2];
                            ys[0] = Character.Partner.X;
                            ys[1] = Character.Partner.Y;
                            map.MoveActor(Map.MOVE_TYPE.START, Character.Partner, ys, dir, 500, true,
                                MoveType.CHANGE_DIR);
                        }

                        ys = new short[2];
                        ys[0] = target.X;
                        ys[1] = target.Y;
                        map.MoveActor(Map.MOVE_TYPE.START, target, ys, dir2, 500, true, MoveType.CHANGE_DIR);
                        if (target.Partner != null)
                        {
                            ys = new short[2];
                            ys[0] = target.Partner.X;
                            ys[1] = target.Partner.Y;
                            map.MoveActor(Map.MOVE_TYPE.START, target.Partner, ys, dir, 500, true, MoveType.CHANGE_DIR);
                        }

                        ushort motionid = 163;
                        byte loop = 0;
                        switch (Global.Random.Next(0, 31))
                        {
                            case 0:
                                motionid = 113;
                                loop = 1;
                                break;
                            case 1:
                                motionid = 163;
                                break;
                            case 2:
                                motionid = 509;
                                break;
                            case 3:
                                motionid = 159;
                                break;
                            case 4:
                                motionid = 210;
                                break;
                            case 5:
                                motionid = 509;
                                break;
                            case 6:
                                motionid = 300;
                                break;
                            case 7:
                                motionid = 2035;
                                break;
                            case 8:
                                motionid = 2040;
                                break;
                            case 9:
                                motionid = 1520;
                                break;
                            case 10:
                                motionid = 1521;
                                break;
                            case 11:
                                motionid = 2020;
                                break;
                            case 12:
                                motionid = 2020;
                                break;
                            case 13:
                                motionid = 2064;
                                break;
                            case 14:
                                motionid = 2065;
                                break;
                            case 15:
                                motionid = 2066;
                                break;
                            case 16:
                                motionid = 2067;
                                break;
                            case 17:
                                motionid = 2069;
                                break;
                            case 18:
                                motionid = 2070;
                                break;
                            case 19:
                                motionid = 1524;
                                break;
                            case 20:
                                motionid = 2084;
                                break;
                            case 21:
                                motionid = 2095;
                                break;
                            case 22:
                                motionid = 2091;
                                break;
                            case 23:
                                motionid = 2085;
                                break;
                            case 24:
                                motionid = 2109;
                                break;
                            case 25:
                                motionid = 2125;
                                break;
                            case 26:
                                motionid = 2098;
                                break;
                            case 27:
                                motionid = 2079;
                                loop = 1;
                                break;
                            case 28:
                                motionid = 1523;
                                break;
                            case 29:
                                motionid = 2080;
                                break;
                            case 30:
                                motionid = 2138;
                                break;
                            case 31:
                                motionid = 2139;
                                break;
                        }

                        var tclient = FromActorPC(target);
                        SendMotion((MotionType)motionid, loop);
                        tclient.SendMotion((MotionType)motionid, loop);
                        SendSystemMessage("你问候了 " + target.Name);
                        tclient.SendSystemMessage(Character.Name + " 正在向你打招呼~");

                        if (target.AStr["打招呼每日重置2"] != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            target.AStr["打招呼每日重置2"] = DateTime.Now.ToString("yyyy-MM-dd");
                            if (target.CIDict["打招呼的玩家"].Count > 0)
                                target.CIDict["打招呼的玩家"] = new VariableHolderA<int, int>();
                            target.AInt["今日被打招呼次数"] = 0;
                        }

                        if (!target.CIDict["打招呼的玩家"].ContainsKey(Character.Account.AccountID))
                        {
                            target.CIDict["打招呼的玩家"][Character.Account.AccountID] = 0;
                            tclient.TitleProccess(target, 62, 1);
                            if (target.AInt["今日被打招呼次数"] < 1)
                            {
                                target.AInt["今日被打招呼次数"]++;
                                //int cp = Global.Random.Next(100, 280);
                                var ep = 10;
                                target.EP = Math.Min(target.EP + (uint)ep, target.MaxEP);
                                //tclient.SendSystemMessage("被亲切地问候了！获得" + cp + "CP。");
                            }
                        }

                        Character.TTime["打招呼时间"] = DateTime.Now;
                    }
                }
        }

        public void OnPlayerElements(CSMG_PLAYER_ELEMENTS p)
        {
            var p1 = new SSMG_PLAYER_ELEMENTS();
            var elements = new Dictionary<Elements, int>();
            foreach (var i in Character.AttackElements.Keys)
                elements.Add(i,
                    Character.AttackElements[i] + Character.Status.attackElements_item[i] +
                    Character.Status.attackelements_iris[i] + Character.Status.attackElements_skill[i]);
            p1.AttackElements = elements;
            elements.Clear();
            foreach (var i in Character.Elements.Keys)
                elements.Add(i,
                    Character.Elements[i] + Character.Status.elements_item[i] + Character.Status.elements_iris[i] +
                    Character.Status.elements_skill[i]);
            p1.DefenceElements = elements;
            netIO.SendPacket(p1);
        }

        public void OnPlayerElements()
        {
            var p1 = new SSMG_PLAYER_ELEMENTS();
            var elements = new Dictionary<Elements, int>();
            foreach (var i in Character.AttackElements.Keys)
                elements.Add(i,
                    Character.AttackElements[i] + Character.Status.attackElements_item[i] +
                    Character.Status.attackelements_iris[i] + Character.Status.attackElements_skill[i]);
            p1.AttackElements = elements;
            elements.Clear();
            foreach (var i in Character.Elements.Keys)
                elements.Add(i,
                    Character.Elements[i] + Character.Status.elements_item[i] + Character.Status.elements_iris[i] +
                    Character.Status.elements_skill[i]);
            p1.DefenceElements = elements;
            netIO.SendPacket(p1);
        }

        public void OnRequestPCInfo(CSMG_ACTOR_REQUEST_PC_INFO p)
        {
            var p1 = new SSMG_ACTOR_PC_INFO();
            var pc = map.GetActor(p.ActorID);

            if (pc == null) return;

            if (pc.type == ActorType.PC)
            {
                var a = (ActorPC)pc;
                a.WRPRanking = WRPRankingManager.Instance.GetRanking(a);
            }

            p1.Actor = pc;

            netIO.SendPacket(p1);
            if (pc.type == ActorType.PC)
            {
                var actor = (ActorPC)pc;
                if (actor.Ring != null) Character.e.OnActorRingUpdate(actor);
            }
        }

        public void OnStatsPreCalc(CSMG_PLAYER_STATS_PRE_CALC p)
        {
            //backup
            ushort str, dex, intel, agi, vit, mag;
            var p1 = new SSMG_PLAYER_STATS_PRE_CALC();
            str = Character.Str;
            dex = Character.Dex;
            intel = Character.Int;
            agi = Character.Agi;
            vit = Character.Vit;
            mag = Character.Mag;

            Character.Str = p.Str;
            Character.Dex = p.Dex;
            Character.Int = p.Int;
            Character.Agi = p.Agi;
            Character.Vit = p.Vit;
            Character.Mag = p.Mag;

            StatusFactory.Instance.CalcStatus(Character);

            p1.ASPD = Character.Status.aspd;
            p1.ATK1Max = Character.Status.max_atk1;
            p1.ATK1Min = Character.Status.min_atk1;
            p1.ATK2Max = Character.Status.max_atk2;
            p1.ATK2Min = Character.Status.min_atk2;
            p1.ATK3Max = Character.Status.max_atk3;
            p1.ATK3Min = Character.Status.min_atk3;
            p1.AvoidCritical = Character.Status.avoid_critical;
            p1.AvoidMagic = Character.Status.avoid_magic;
            p1.AvoidMelee = Character.Status.avoid_melee;
            p1.AvoidRanged = Character.Status.avoid_ranged;
            p1.CSPD = Character.Status.cspd;
            p1.DefAddition = (ushort)Character.Status.def_add;
            p1.DefBase = Character.Status.def;
            p1.HitCritical = Character.Status.hit_critical;
            p1.HitMagic = Character.Status.hit_magic;
            p1.HitMelee = Character.Status.hit_melee;
            p1.HitRanged = Character.Status.hit_ranged;
            p1.MATKMax = Character.Status.max_matk;
            p1.MATKMin = Character.Status.min_matk;
            p1.MDefAddition = (ushort)Character.Status.mdef_add;
            p1.MDefBase = Character.Status.mdef;
            p1.Speed = Character.Speed;
            p1.HP = (ushort)Character.MaxHP;
            p1.MP = (ushort)Character.MaxMP;
            p1.SP = (ushort)Character.MaxSP;
            uint count = 0;
            foreach (var i in Character.Inventory.MaxVolume.Values) count += i;
            p1.Capacity = (ushort)count;
            count = 0;
            foreach (var i in Character.Inventory.MaxPayload.Values) count += i;
            p1.Payload = (ushort)count;

            //resotre
            Character.Str = str;
            Character.Dex = dex;
            Character.Int = intel;
            Character.Agi = agi;
            Character.Vit = vit;
            Character.Mag = mag;

            StatusFactory.Instance.CalcStatus(Character);

            netIO.SendPacket(p1);
        }

        public void OnStatsUp(CSMG_PLAYER_STATS_UP p)
        {
            if (Configuration.Instance.Version < Version.Saga13)
            {
                switch (p.Type)
                {
                    case 0:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Str))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                            Character.Str += 1;
                        }

                        break;
                    case 1:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Dex))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                            Character.Dex += 1;
                        }

                        break;
                    case 2:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Int))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                            Character.Int += 1;
                        }

                        break;
                    case 3:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Vit))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                            Character.Vit += 1;
                        }

                        break;
                    case 4:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Agi))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                            Character.Agi += 1;
                        }

                        break;
                    case 5:
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Mag))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                            Character.Mag += 1;
                        }

                        break;
                }
            }
            else
            {
                if (p.Str > 0)
                    for (int i = p.Str; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Str))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Str);
                            Character.Str += 1;
                        }

                if (p.Dex > 0)
                    for (int i = p.Dex; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Dex))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Dex);
                            Character.Dex += 1;
                        }

                if (p.Int > 0)
                    for (int i = p.Int; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Int))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Int);
                            Character.Int += 1;
                        }

                if (p.Vit > 0)
                    for (int i = p.Vit; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Vit))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Vit);
                            Character.Vit += 1;
                        }

                if (p.Agi > 0)
                    for (int i = p.Agi; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Agi))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Agi);
                            Character.Agi += 1;
                        }

                if (p.Mag > 0)
                    for (int i = p.Mag; i > 0; i--)
                        if (Character.StatsPoint >= StatusFactory.Instance.RequiredBonusPoint(Character.Mag))
                        {
                            Character.StatsPoint -= StatusFactory.Instance.RequiredBonusPoint(Character.Mag);
                            Character.Mag += 1;
                        }
            }

            StatusFactory.Instance.CalcStatus(Character);
            SendActorHPMPSP(Character);
            SendStatus();
            SendStatusExtend();
            SendCapacity();
            //SendMaxCapacity();
            SendPlayerLevel();
        }

        public void SendWRPRanking(ActorPC pc)
        {
            var p = new SSMG_ACTOR_WRP_RANKING();
            p.ActorID = pc.ActorID;
            p.Ranking = pc.WRPRanking;
            netIO.SendPacket(p);
        }

        public void RevivePC(ActorPC pc)
        {
            pc.HP = pc.MaxHP;
            pc.MP = pc.MaxMP;
            pc.SP = pc.MaxSP;
            pc.EP = pc.MaxEP;

            if (pc.Job == PC_JOB.CARDINAL)
                pc.EP = 5000;

            if (pc.Job == PC_JOB.ASTRALIST) //魔法师
                pc.EP = 0;

            if (!pc.Status.Additions.ContainsKey("HolyVolition"))
            {
                var skill = new DefaultBuff(null, pc, "HolyVolition", 2000);
                SkillHandler.ApplyAddition(pc, skill);
            }

            if (pc.SaveMap == 0)
            {
                pc.SaveMap = 91000999;
                pc.SaveX = 21;
                pc.SaveY = 21;
            }

            pc.BattleStatus = 0;
            SendChangeStatus();

            pc.Buff.Dead = false;
            pc.Buff.TurningPurple = false;
            pc.Motion = MotionType.STAND;
            pc.MotionLoop = false;
            SkillHandler.Instance.ShowVessel(pc, (int)-pc.MaxHP);
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, pc, true);

            SkillHandler.Instance.ShowEffectByActor(pc, 5116);
            SkillHandler.Instance.CastPassiveSkills(pc);
            SendPlayerInfo();

            if (!pc.Tasks.ContainsKey("Recover")) //自然恢复
            {
                var reg = new Recover(FromActorPC(pc));
                pc.Tasks.Add("Recover", reg);
                reg.Activate();
            }

            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);

            if (scriptThread != null)
                ClientManager.RemoveThread(scriptThread.Name);
            scriptThread = null;
            currentEvent = null;

            /*Scripting.Event evnt = null;
            if (evnt != null)
            {
                evnt.CurrentPC = null;
                this.scriptThread = null;
                this.currentEvent = null;
                ClientManager.RemoveThread(System.Threading.Thread.CurrentThread.Name);
                //ClientManager.LeaveCriticalArea();
            }*/
        }

        public void OnPlayerReturnHome(CSMG_PLAYER_RETURN_HOME p)
        {
            if (Character.HP == 0)
            {
                Character.HP = 1;
                Character.MP = 1;
                Character.SP = 1;
            }

            if (Character.SaveMap == 0)
            {
                Character.SaveMap = 10023100;
                Character.SaveX = 242;
                Character.SaveY = 128;
            }

            Character.BattleStatus = 0;
            SendChangeStatus();
            Character.Buff.Dead = false;
            Character.Buff.TurningPurple = false;
            Character.Motion = MotionType.STAND;
            Character.MotionLoop = false;
            Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Character, true);

            SkillHandler.Instance.CastPassiveSkills(Character);
            SendPlayerInfo();

            if (map.ID == Character.SaveMap)
            {
                if (Map.Info.Healing)
                {
                    if (!Character.Tasks.ContainsKey("CityRecover"))
                    {
                        var task = new CityRecover(this);
                        Character.Tasks.Add("CityRecover", task);
                        task.Activate();
                    }
                }
                else
                {
                    if (Character.Tasks.ContainsKey("CityRecover"))
                    {
                        Character.Tasks["CityRecover"].Deactivate();
                        Character.Tasks.Remove("CityRecover");
                    }
                }

                if (Map.Info.Cold || map.Info.Hot || map.Info.Wet)
                {
                    if (!Character.Tasks.ContainsKey("CityDown"))
                    {
                        var task = new CityDown(this);
                        Character.Tasks.Add("CityDown", task);
                        task.Activate();
                    }
                }
                else
                {
                    if (Character.Tasks.ContainsKey("CityDown"))
                    {
                        Character.Tasks["CityDown"].Deactivate();
                        Character.Tasks.Remove("CityDown");
                    }
                }
            }

            if (Configuration.Instance.HostedMaps.Contains(Character.SaveMap))
            {
                var info = MapInfoFactory.Instance.MapInfo[Character.SaveMap];
                Map.SendActorToMap(Character, Character.SaveMap, Global.PosX8to16(Character.SaveX, info.width),
                    Global.PosY8to16(Character.SaveY, info.height));
            }

            Event evnt = null;
            if (evnt != null)
            {
                evnt.CurrentPC = null;
                scriptThread = null;
                currentEvent = null;
                ClientManager.RemoveThread(Thread.CurrentThread.Name);
                ClientManager.LeaveCriticalArea();
            }
        }


        public void SendDefWarChange(DefWar text)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_DEFWAR_SET();
                p11.MapID = map.ID;
                p11.Data = text;
                netIO.SendPacket(p11);
            }
        }

        public void SendDefWarResult(byte r1, byte r2, int exp, int jobexp, int cp, byte u = 0)
        {
            if (Character.Online)
            {
                var p11 = new SSMG_DEFWAR_RESULT();
                p11.Result1 = r1;
                p11.Result2 = r2;
                p11.EXP = exp;
                p11.JOBEXP = jobexp;
                p11.CP = cp;

                p11.Unknown = u;
                netIO.SendPacket(p11);
            }
        }

        public void SendDefWarState(byte rate)
        {
            if (Character.Online)
            {
                var p1 = new SSMG_DEFWAR_STATE();
                p1.MapID = map.ID;
                p1.Rate = rate;
                netIO.SendPacket(p1);
            }
        }

        public void SendDefWarStates(Dictionary<uint, byte> list)
        {
            if (Character.Online)
            {
                var p1 = new SSMG_DEFWAR_STATES();
                p1.List = list;
                netIO.SendPacket(p1);
            }
        }
    }
}