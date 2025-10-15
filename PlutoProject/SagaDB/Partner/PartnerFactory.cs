using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Partner
{
    public class PartnerFactory : Singleton<PartnerFactory>
    {
        private readonly Dictionary<uint, ActCubeData> actcubes_db_itemID = new Dictionary<uint, ActCubeData>();
        private readonly Dictionary<uint, TalkInfo> actcubes_talks_db = new Dictionary<uint, TalkInfo>();

        private readonly Dictionary<uint, List<PartnerMotion>> partner_motion_info =
            new Dictionary<uint, List<PartnerMotion>>();

        private readonly Dictionary<uint, PartnerEquipment> partnerequips_db = new Dictionary<uint, PartnerEquipment>();
        private readonly Dictionary<uint, PartnerFood> partnerfoods_db = new Dictionary<uint, PartnerFood>();
        private readonly Dictionary<uint, PartnerData> partners_info = new Dictionary<uint, PartnerData>();
        public Dictionary<ushort, ActCubeData> actcubes_db_uniqueID = new Dictionary<ushort, ActCubeData>();

        public List<uint> PartnerPictList = new List<uint>();
        public List<uint> RankAPets = new List<uint>();

        public List<uint> RankBPets = new List<uint>();
        public List<uint> RankSPets = new List<uint>();
        public List<uint> RankSSPets = new List<uint>();
        public List<uint> RankSSSPets = new List<uint>();
        public Dictionary<uint, PartnerData> Partners { get; } = new Dictionary<uint, PartnerData>();

        public void ClearPartnerEquips()
        {
            partnerequips_db.Clear();
        }

        public List<PartnerMotion> GetPartnerMotion(uint itemid)
        {
            if (!partner_motion_info.ContainsKey(itemid)) return null;
            return partner_motion_info[itemid];
        }

        public TalkInfo GetPartnerTalks(uint parterid)
        {
            if (!actcubes_talks_db.ContainsKey(parterid)) return null;
            return actcubes_talks_db[parterid];
        }

        public PartnerData GetPartnerInfo(uint partnerid) //original csv data
        {
            return partners_info[partnerid];
        }

        public PartnerData GetPartnerData(uint partnerid) //custom csv data
        {
            return Partners[partnerid];
        }

        public PartnerFood GetPartnerFood(uint itemid)
        {
            return partnerfoods_db[itemid];
        }

        public PartnerEquipment GetPartnerEquip(uint itemid)
        {
            return partnerequips_db[itemid];
        }

        public ActCubeData GetCubeItemID(uint itemid)
        {
            return actcubes_db_itemID[itemid];
        }

        public ActCubeData GetCubeUniqueID(ushort uniqueid)
        {
            return actcubes_db_uniqueID[uniqueid];
        }

        public void InitPartnerInfo(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
#if !Web
            var label = "Loading partner info";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var partner = new PartnerData();
                    partner.id = uint.Parse(paras[0]);
                    partner.name = paras[1];
                    partner.pictid = uint.Parse(paras[2]);
                    partner.partnerSize = float.Parse(paras[3]);
                    partner.motionsetnumber = ushort.Parse(paras[4]);
                    try
                    {
                        var typeinfo = paras[6].Split('_').ToList();
                        partner.partnertypeid = ushort.Parse(typeinfo[typeinfo.Count - 1]);
                        typeinfo.RemoveAt(typeinfo.Count - 1);
                        var type = string.Join<string>("_", typeinfo);
                        partner.partnertype = (PartnerType)Enum.Parse(typeof(PartnerType), type);
                    }
                    catch (Exception exception)
                    {
                        Logger.getLogger().Error(exception, null);
                        while (paras[6].Substring(paras[6].Length - 1) != "_")
                            paras[6] = paras[6].Substring(0, paras[6].Length - 1);
                        paras[6] = paras[6].Substring(0, paras[6].Length - 1);
                        var typeinfo = paras[6].Split('_').ToList();
                        partner.partnertypeid = ushort.Parse(typeinfo[typeinfo.Count - 1]);
                        typeinfo.RemoveAt(typeinfo.Count - 1);
                        var type = string.Join<string>("_", typeinfo);
                        partner.partnertype = (PartnerType)Enum.Parse(typeof(PartnerType), type);
                    }

                    partner.partnersystemid = ushort.Parse(paras[7]);
                    partner.fly = toBool(paras[8]);
                    partner.speed = ushort.Parse(paras[9]);
                    partner.attackType = (ATTACK_TYPE)Enum.Parse(typeof(ATTACK_TYPE), paras[10]);
                    partner.isrange = toBool(paras[11]);
                    partner.range = 1; //float.Parse(paras[12]);

                    if (partners_info.ContainsKey(partner.id))
                    {
                        Logger.getLogger().Error("重复的PartnerID:" + partner.id + ",[" + partners_info[partner.id].name +
                                                 "]的已存在" + ",让[" + partner.name + "]没有被添加到Info。");
                    }
                    else
                    {
                        partners_info.Add(partner.id, partner);
                        count++;
                    }
#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.getLogger().Error("Error on parsing Partner Info!\r\nat line:" + line);
                    Logger.getLogger().Error(ex, ex.Message);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count + " partners infos loaded.");
#endif
            sr.Close();
        }

        public void InitPartnerRankDB(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
#if !Web
            var label = "Loading partner Rank database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    var id = uint.Parse(paras[0]);
                    var baserank = byte.Parse(paras[1]);
                    if (Partners.ContainsKey(id))
                        Partners[id].base_rank = baserank;
                    else
                        Logger.getLogger().Error("不存在ID为" + id + "的伙伴，设置初始RANK失败。");
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }

            foreach (var item in ItemFactory.Instance.Items.Values)
                if (item.petID != 0)
                    if (Partners.ContainsKey(item.petID) && item.itemType == ItemType.PARTNER)
                    {
                        var baserank = Partners[item.petID].base_rank;
                        var itemid2 = item.id;

                        switch (baserank)
                        {
                            case 61:
                                if (!RankBPets.Contains(itemid2))
                                    RankBPets.Add(itemid2);
                                break;
                            case 71:
                                if (!RankAPets.Contains(itemid2))
                                    RankAPets.Add(itemid2);
                                break;
                            case 81:
                                if (!RankSPets.Contains(itemid2))
                                    RankSPets.Add(itemid2);
                                break;
                            case 91:
                                if (!RankSSPets.Contains(itemid2))
                                    RankSSPets.Add(itemid2);
                                break;
                            case 101:
                                if (!RankSSSPets.Contains(itemid2))
                                    RankSSSPets.Add(itemid2);
                                break;
                        }
                    }
        }

        public void InitPartnerTalksInfo(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var label = "Loading Talks database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    var id = uint.Parse(paras[0]);
                    TalkInfo ti;
                    if (!actcubes_talks_db.ContainsKey(id))
                        ti = new TalkInfo();
                    else
                        ti = actcubes_talks_db[id];
                    if (paras[1] != "")
                        ti.Onsummoned.Add(paras[1]);
                    if (paras[2] != "")
                        ti.OnBattle.Add(paras[2]);
                    if (paras[3] != "")
                        ti.OnMasterDead.Add(paras[3]);
                    if (paras[4] != "")
                        ti.OnNormal.Add(paras[4]);
                    if (paras[5] != "")
                        ti.OnJoinParty.Add(paras[5]);
                    if (paras[6] != "")
                        ti.OnLeaveParty.Add(paras[6]);
                    if (paras[7] != "")
                        ti.OnMasterFighting.Add(paras[7]);
                    if (paras[8] != "")
                        ti.OnMasterLevelUp.Add(paras[8]);
                    if (paras[9] != "")
                        ti.OnMasterQuit.Add(paras[9]);
                    if (paras[10] != "")
                        ti.OnMasterSit.Add(paras[10]);
                    if (paras[11] != "")
                        ti.OnMasterRelax.Add(paras[11]);
                    if (paras[12] != "")
                        ti.OnMasterBow.Add(paras[12]);
                    if (paras[13] != "")
                        ti.OnMasterLogin.Add(paras[13]);
                    if (paras[14] != "")
                        ti.OnLevelUp.Add(paras[14]);
                    if (paras[15] != "")
                        ti.OnEquip.Add(paras[15]);
                    if (paras[16] != "")
                        ti.OnEat.Add(paras[16]);
                    if (paras[17] != "")
                        ti.OnEatReady.Add(paras[17]);
                    if (!actcubes_talks_db.ContainsKey(id))
                        actcubes_talks_db.Add(id, ti);
                    else
                        actcubes_talks_db[id] = ti;
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }
        }

        public void InitPartnerPicts(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var label = "Loading PartnerPict database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    var pictid = uint.Parse(paras[0]);
                    if (!PartnerPictList.Contains(pictid)) PartnerPictList.Add(pictid);
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }
        }

        public void InitPartnerMotions(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var label = "Loading Talks database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    var itemid = uint.Parse(paras[0]);
                    var pm = new PartnerMotion();
                    pm.ID = byte.Parse(paras[1]);
                    pm.MasterMotionID = uint.Parse(paras[6]);
                    pm.PartnerMotionID = uint.Parse(paras[4]);
                    if (!partner_motion_info.ContainsKey(itemid))
                        partner_motion_info.Add(itemid, new List<PartnerMotion>());
                    partner_motion_info[itemid].Add(pm);
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }
        }

        public void InitPartnerDB(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
#if !Web
            var label = "Loading partner database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var partner = new PartnerData();
                    partner.id = uint.Parse(paras[0]);
                    partner.name = paras[1];
                    partner.pictid = uint.Parse(paras[2]);
                    partner.partnerSize = float.Parse(paras[3]);
                    partner.motionsetnumber = ushort.Parse(paras[4]);
                    try
                    {
                        var typeinfo = paras[6].Split('_').ToList();
                        partner.partnertypeid = ushort.Parse(typeinfo[typeinfo.Count - 1]);
                        typeinfo.RemoveAt(typeinfo.Count - 1);
                        var type = string.Join<string>("_", typeinfo);
                        partner.partnertype = (PartnerType)Enum.Parse(typeof(PartnerType), type);
                    }
                    catch (Exception exception)
                    {
                        Logger.getLogger().Error(exception, null);
                        while (paras[6].Substring(paras[6].Length - 1) != "_")
                            paras[6] = paras[6].Substring(0, paras[6].Length - 1);
                        paras[6] = paras[6].Substring(0, paras[6].Length - 1);
                        var typeinfo = paras[6].Split('_').ToList();
                        partner.partnertypeid = ushort.Parse(typeinfo[typeinfo.Count - 1]);
                        typeinfo.RemoveAt(typeinfo.Count - 1);
                        var type = string.Join<string>("_", typeinfo);
                        partner.partnertype = (PartnerType)Enum.Parse(typeof(PartnerType), type);
                    }

                    partner.partnersystemid = ushort.Parse(paras[7]);
                    partner.fly = toBool(paras[8]);
                    partner.speed = ushort.Parse(paras[9]);
                    partner.attackType = (ATTACK_TYPE)Enum.Parse(typeof(ATTACK_TYPE), paras[10]);
                    partner.isrange = toBool(paras[11]);
                    partner.range = float.Parse(paras[12]);
                    partner.level_in = 1;
                    partner.hp_in = 100;
                    partner.mp_in = 0;
                    partner.sp_in = 0;
                    partner.hp_fn = 1000;
                    partner.mp_fn = 0;
                    partner.sp_fn = 0;
                    partner.hp_in_re = 100;
                    partner.mp_in_re = 100;
                    partner.sp_in_re = 100;
                    partner.hp_fn_re = 500;
                    partner.mp_fn_re = 0;
                    partner.sp_fn_re = 0;
                    partner.hp_rec_in = 0;
                    partner.mp_rec_in = 0;
                    partner.sp_rec_in = 0;
                    partner.hp_rec_fn = 0;
                    partner.mp_rec_fn = 0;
                    partner.sp_rec_fn = 0;
                    partner.hp_rec_in_re = 0;
                    partner.mp_rec_in_re = 0;
                    partner.sp_rec_in_re = 0;
                    partner.hp_rec_fn_re = 0;
                    partner.mp_rec_fn_re = 0;
                    partner.sp_rec_fn_re = 0;
                    partner.atk_min_in = 1;
                    partner.atk_max_in = 1;
                    partner.atk_min_fn = 1;
                    partner.atk_max_fn = 1;
                    partner.atk_min_in_re = 1;
                    partner.atk_max_in_re = 1;
                    partner.atk_min_fn_re = 1;
                    partner.atk_max_fn_re = 1;
                    partner.matk_min_in = 1;
                    partner.matk_max_in = 1;
                    partner.matk_min_fn = 1;
                    partner.matk_max_fn = 1;
                    partner.matk_min_in_re = 1;
                    partner.matk_max_in_re = 1;
                    partner.matk_min_fn_re = 1;
                    partner.matk_max_fn_re = 1;
                    partner.def_in = 1;
                    partner.def_add_in = 1;
                    partner.def_fn = 1;
                    partner.def_add_fn = 1;
                    partner.def_in_re = 1;
                    partner.def_add_in_re = 1;
                    partner.def_fn_re = 1;
                    partner.def_add_fn_re = 1;
                    partner.mdef_in = 1;
                    partner.mdef_add_in = 1;
                    partner.mdef_fn = 1;
                    partner.mdef_add_fn = 1;
                    partner.mdef_in_re = 1;
                    partner.mdef_add_in_re = 1;
                    partner.mdef_fn_re = 1;
                    partner.mdef_add_fn_re = 1;
                    partner.hit_melee_in = 1;
                    partner.hit_ranged_in = 1;
                    partner.hit_magic_in = 1;
                    partner.hit_critical_in = 1;
                    partner.hit_melee_fn = 1;
                    partner.hit_ranged_fn = 1;
                    partner.hit_magic_fn = 1;
                    partner.hit_critical_fn = 1;
                    partner.hit_melee_in_re = 1;
                    partner.hit_ranged_in_re = 1;
                    partner.hit_magic_in_re = 1;
                    partner.hit_critical_in_re = 1;
                    partner.hit_melee_fn_re = 1;
                    partner.hit_ranged_fn_re = 1;
                    partner.hit_magic_fn_re = 1;
                    partner.hit_critical_fn_re = 1;
                    partner.aspd_in = 1;
                    partner.cspd_in = 1;
                    partner.aspd_fn = 1;
                    partner.cspd_fn = 1;
                    partner.aspd_in_re = 1;
                    partner.cspd_in_re = 1;
                    partner.aspd_fn_re = 1;
                    partner.cspd_fn_re = 1;

                    partner.aiMode = 0;

                    if (Partners.ContainsKey(partner.id))
                    {
                        Logger.getLogger().Error("重复的PartnerID:" + partner.id + ",[" + Partners[partner.id].name +
                                                 "]的已存在" +
                                                 ",让[" + partner.name + "]没有被添加到DB。");
                    }
                    else
                    {
                        Partners.Add(partner.id, partner);
                        count++;
                    }
#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.getLogger().Error("Error on parsing Partner DB!\r\nat line:" + line);
                    Logger.getLogger().Error(ex, ex.Message);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count + " partners loaded.");
#endif
            sr.Close();
        }

        public void InitPartnerFoodDB(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
#if !Web
            var label = "Loading partner food database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var food = new PartnerFood();
                    food.itemID = uint.Parse(paras[0]);
                    food.partnerrank_min = byte.Parse(paras[1]);
                    food.partnerrank_max = byte.Parse(paras[2]);
                    food.systemID = byte.Parse(paras[3]);
                    food.nextfeedtime = 60 * uint.Parse(paras[4]); //from mins to seconds
                    food.rankexp = uint.Parse(paras[5]);
                    food.reliabilityuprate = ushort.Parse(paras[6]);

                    if (partnerfoods_db.ContainsKey(food.itemID))
                    {
                        Logger.getLogger().Error("重复的PartnerFoodID:" + food.itemID + ",已存在,没有被添加到DB。");
                    }
                    else
                    {
                        partnerfoods_db.Add(food.itemID, food);
                        count++;
                    }
#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.getLogger().Error("Error on parsing Partner Food DB!\r\nat line:" + line);
                    Logger.getLogger().Error(ex, ex.Message);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count + " partner foods loaded.");
#endif
            sr.Close();
        }

        public void InitPartnerEquipDB(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
#if !Web
            var label = "Loading partner equip database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var equip = new PartnerEquipment();
                    equip.itemID = uint.Parse(paras[0]); //[1] is item name
                    var item = ItemFactory.Instance.GetItem(equip.itemID);

                    item.BaseData.hp = short.Parse(paras[2]);
                    item.BaseData.mp = short.Parse(paras[3]);
                    item.BaseData.sp = short.Parse(paras[4]);
                    item.BaseData.atk1 = short.Parse(paras[5]);
                    item.BaseData.atk2 = short.Parse(paras[5]);
                    item.BaseData.atk3 = short.Parse(paras[5]);
                    item.BaseData.matk = short.Parse(paras[6]);
                    item.BaseData.def = short.Parse(paras[7]);
                    item.BaseData.mdef = short.Parse(paras[7]);
                    item.BaseData.hitMelee = short.Parse(paras[9]);
                    item.BaseData.hitRanged = short.Parse(paras[10]);
                    item.BaseData.hitMagic = short.Parse(paras[11]);
                    item.BaseData.avoidMelee = short.Parse(paras[12]);
                    item.BaseData.avoidRanged = short.Parse(paras[13]);
                    item.BaseData.avoidMagic = short.Parse(paras[14]);
                    item.BaseData.hitCritical = short.Parse(paras[15]);
                    item.BaseData.avoidCritical = short.Parse(paras[16]);

                    equip.hp_up = int.Parse(paras[2]);
                    equip.mp_up = int.Parse(paras[3]);
                    equip.sp_up = int.Parse(paras[4]);
                    equip.atk = short.Parse(paras[5]);
                    equip.matk = short.Parse(paras[6]);
                    equip.def = short.Parse(paras[7]);
                    equip.mdef = short.Parse(paras[8]);
                    equip.Shit = short.Parse(paras[9]);
                    equip.Lhit = short.Parse(paras[10]);
                    equip.Mhit = short.Parse(paras[11]);
                    equip.Savd = short.Parse(paras[12]);
                    equip.Lavd = short.Parse(paras[13]);
                    equip.Mavd = short.Parse(paras[14]);
                    equip.Chit = short.Parse(paras[15]);
                    equip.Cavd = short.Parse(paras[16]);
                    equip.hp_rec = int.Parse(paras[17]);
                    equip.mp_rec = int.Parse(paras[18]);
                    equip.sp_rec = int.Parse(paras[19]);
                    for (var i = 0; i < 7; i++) equip.elements.Add((Elements)i, int.Parse(paras[20 + i]));
                    for (var i = 0; i < 9; i++) equip.abnormalStatus.Add((AbnormalStatus)i, short.Parse(paras[27 + i]));
                    equip.partnerrank = byte.Parse(paras[36]); //[37]is attacktype null
                    equip.systemID = byte.Parse(paras[38]);

                    if (partnerequips_db.ContainsKey(equip.itemID))
                    {
                        Logger.getLogger().Error("重复的PartnerEquipID:" + equip.itemID + ",已存在,没有被添加到DB。");
                    }
                    else
                    {
                        partnerequips_db.Add(equip.itemID, equip);
                        count++;
                    }
#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.getLogger().Error("Error on parsing Partner Equip DB!\r\nat line:" + line);
                    Logger.getLogger().Error(ex, ex.Message);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count + " partner equips loaded.");
#endif
            sr.Close();
        }

        public void InitActCubeDB(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count_itemID = 0;
            var count_uniqueID = 0;
#if !Web
            var label = "Loading partner cube database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var cube = new ActCubeData();
                    cube.uniqueID = ushort.Parse(paras[0]);
                    cube.itemID = uint.Parse(paras[1]);
                    cube.cubetype = (PartnerCubeType)int.Parse(paras[2]); //[3] is cube name
                    cube.cubename = paras[3];
                    cube.systemID = byte.Parse(paras[4]); //[5]-[15] is system related 0s
                    cube.reliability = byte.Parse(paras[16]);
                    cube.rebirth = byte.Parse(paras[17]); //[18] is description
                    cube.skillID = uint.Parse(paras[19]); //[20] is partner restriction
                    cube.actionID = int.Parse(paras[21]);
                    cube.parameter1 = int.Parse(paras[22]);
                    cube.parameter2 = int.Parse(paras[23]);
                    cube.parameter3 = int.Parse(paras[24]); //[25] is unknown

                    if (actcubes_db_itemID.ContainsKey(cube.itemID))
                    {
                        Logger.getLogger().Error("重复的PartnerCubeItemID:" + cube.itemID + ",已存在,没有被添加到DB。");
                    }
                    else
                    {
                        actcubes_db_itemID.Add(cube.itemID, cube);
                        count_itemID++;
                    }

                    if (actcubes_db_uniqueID.ContainsKey(cube.uniqueID))
                    {
                        Logger.getLogger().Error("重复的PartnerCubeUniqueID:" + cube.uniqueID + ",已存在,没有被添加到DB。");
                    }
                    else
                    {
                        actcubes_db_uniqueID.Add(cube.uniqueID, cube);
                        count_uniqueID++;
                    }
#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.getLogger().Error("Error on parsing Partner Cube DB!\r\nat line:" + line);
                    Logger.getLogger().Error(ex, ex.Message);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count_itemID + "/" + count_uniqueID + " partner cubes loaded.");
#endif
            sr.Close();
        }

        private bool toBool(string input)
        {
            if (input == "1") return true;
            return false;
        }
    }
}