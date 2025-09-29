using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.DEMIC;
using SagaDB.Iris;
using SagaDB.Item;
using SagaDB.Skill;
using SagaDB.Title;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.PC
{
    public class StatusFactory : Singleton<StatusFactory>
    {
        /// <summary>
        ///     获取强化奖励数值
        /// </summary>
        /// <param name="item">道具本身</param>
        /// <param name="Type">0: 生命 1: ATK DEF MATK 2: MDEF 3: 爆击相关</param>
        /// <returns></returns>
        public short GetEnhanceBonus(Item item, int Type)
        {
            short value = 0;
            var hps = new short[31]
            {
                0,
                100, 20, 70, 30, 80, 40, 90, 50, 100, 150,
                150, 60, 110, 70, 200, 200, 120, 80, 130, 250,
                250, 90, 140, 100, 250, 250, 150, 110, 160, 400
            };
            var atk_def_matk = new short[31]
            {
                0,
                10, 3, 5, 3, 6, 3, 7, 3, 8, 13,
                13, 3, 9, 3, 15, 15, 10, 3, 11, 20,
                20, 3, 12, 3, 22, 22, 13, 3, 14, 25
            };
            var mdef = new short[31]
            {
                0,
                10, 2, 5, 2, 6, 3, 6, 3, 6, 15,
                15, 4, 7, 4, 10, 10, 7, 4, 7, 15,
                15, 5, 8, 5, 15, 15, 8, 5, 8, 25
            };
            var cris = new short[31]
            {
                0,
                5, 1, 3, 2, 4, 3, 4, 3, 5, 9,
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4,
                5, 1, 2, 3, 4, 5, 1, 2, 3, 5
            };
            switch (Type)
            {
                case 0:
                    for (var i = 0; i <= item.LifeEnhance; i++)
                        value += hps[i];
                    break;
                case 1:
                    for (var i = 0; i <= item.PowerEnhance; i++)
                        value += atk_def_matk[i];
                    break;
                case 2:
                    for (var i = 0; i <= item.CritEnhance; i++)
                        value += cris[i];
                    break;
                case 3:
                    for (var i = 0; i <= item.MagEnhance; i++)
                        value += mdef[i];
                    break;
                case 4:
                    for (var i = 0; i <= item.MagEnhance; i++)
                        value += atk_def_matk[i];
                    break;
                default:
                    Logger.ShowError("未知的附魔类型");
                    value = 0;
                    break;
            }

            return value;
        }

        private void CalcPlayerTitleBouns(ActorPC pc)
        {
            var TitleID = (uint)pc.AInt["称号_战斗"];
            if (MapClient.FromActorPC(pc).CheckTitle((int)TitleID))
                if (TitleID != 0 && TitleFactory.Instance.Items.ContainsKey(TitleID))
                {
                    var item = TitleFactory.Instance.Items[TitleID];
                    pc.Status.hp_tit = (short)item.hp;
                    pc.Status.mp_tit = (short)item.mp;
                    pc.Status.sp_tit = (short)item.sp;
                    pc.Status.min_atk1_tit = (short)item.atk_min;
                    pc.Status.min_atk2_tit = (short)item.atk_min;
                    pc.Status.min_atk3_tit = (short)item.atk_min;
                    pc.Status.max_atk1_tit = (short)item.atk_max;
                    pc.Status.max_atk2_tit = (short)item.atk_max;
                    pc.Status.max_atk3_tit = (short)item.atk_max;
                    pc.Status.min_matk_tit = (short)item.matk_min;
                    pc.Status.max_matk_tit = (short)item.matk_max;
                    pc.Status.def_add_tit = (short)item.def;
                    pc.Status.mdef_add_tit = (short)item.mdef;
                    pc.Status.cri_tit = (short)item.cri;
                    pc.Status.cri_avoid_tit = (short)item.cri_avoid;
                    pc.Status.hit_melee_tit = (short)item.hit_melee;
                    pc.Status.hit_ranged_tit = (short)item.hit_range;
                    pc.Status.hit_magic_tit = (short)item.hit_range;
                    pc.Status.avoid_melee_tit = (short)item.avoid_melee;
                    pc.Status.avoid_ranged_tit = (short)item.avoid_range;
                    pc.Status.avoid_magic_tit = (short)item.avoid_range;
                    pc.Status.aspd_tit = (short)item.aspd;
                    pc.Status.cspd_tit = (short)item.cspd;
                    MapClient.FromActorPC(pc).SendCharInfoUpdate();
                    return;
                }

            pc.Status.hit_magic_tit = 0;
            pc.Status.cri_avoid_tit = 0;
            pc.Status.avoid_magic_tit = 0;
            pc.Status.aspd_tit = 0;
            pc.Status.cspd_tit = 0;


            pc.PlayerTitleID = 0;
            pc.PlayerTitle = "";
            pc.FirstName = "";
            pc.Status.str_tit = 0;
            pc.Status.mag_tit = 0;
            pc.Status.agi_tit = 0;
            pc.Status.dex_tit = 0;
            pc.Status.vit_tit = 0;
            pc.Status.int_tit = 0;
            pc.Status.hp_tit = 0;
            pc.Status.mp_tit = 0;
            pc.Status.sp_tit = 0;
            pc.Status.hprecov_tit = 0;
            pc.Status.mprecov_tit = 0;
            pc.Status.sprecov_tit = 0;
            pc.Status.min_atk1_tit = 0;
            pc.Status.min_atk2_tit = 0;
            pc.Status.min_atk3_tit = 0;
            pc.Status.max_atk1_tit = 0;
            pc.Status.max_atk2_tit = 0;
            pc.Status.max_atk3_tit = 0;
            pc.Status.min_matk_tit = 0;
            pc.Status.max_matk_tit = 0;
            pc.Status.def_add_tit = 0;
            pc.Status.mdef_add_tit = 0;
            pc.Status.cri_tit = 0;
            pc.Status.hit_melee_tit = 0;
            pc.Status.hit_ranged_tit = 0;
            pc.Status.avoid_melee_tit = 0;
            pc.Status.avoid_ranged_tit = 0;
            MapClient.FromActorPC(pc).SendCharInfoUpdate();
        }

        private void CalcTamaireBonus(ActorPC pc)
        {
            pc.Status.hp_tamaire = pc.TamaireRental.hp;
            pc.Status.mp_tamaire = pc.TamaireRental.mp;
            pc.Status.sp_tamaire = pc.TamaireRental.sp;
            pc.Status.min_atk1_tamaire = pc.TamaireRental.atk_min;
            pc.Status.min_atk2_tamaire = pc.TamaireRental.atk_min;
            pc.Status.min_atk3_tamaire = pc.TamaireRental.atk_min;
            pc.Status.max_atk1_tamaire = pc.TamaireRental.atk_max;
            pc.Status.max_atk2_tamaire = pc.TamaireRental.atk_max;
            pc.Status.max_atk3_tamaire = pc.TamaireRental.atk_max;
            pc.Status.min_matk_tamaire = pc.TamaireRental.matk_min;
            pc.Status.max_matk_tamaire = pc.TamaireRental.matk_max;
            pc.Status.def_add_tamaire = pc.TamaireRental.def;
            pc.Status.mdef_add_tamaire = pc.TamaireRental.mdef;
            pc.Status.hit_melee_tamaire = pc.TamaireRental.hit_melee;
            pc.Status.hit_ranged_tamaire = pc.TamaireRental.hit_range;
            pc.Status.avoid_melee_tamaire = pc.TamaireRental.avoid_melee;
            pc.Status.avoid_ranged_tamaire = pc.TamaireRental.avoid_range;
            pc.Status.aspd_tamaire = pc.TamaireRental.aspd;
            pc.Status.cspd_tamaire = pc.TamaireRental.cspd;
            MapClient.FromActorPC(pc).SendCharInfoUpdate();
        }

        private void CalcAnotherPaperBonus(ActorPC pc)
        {
            if (pc.UsingPaperID != 0 && pc.AnotherPapers.ContainsKey(pc.UsingPaperID))
            {
                var paper = pc.AnotherPapers[pc.UsingPaperID];
                var rate = 1;
                if (pc.Buff.Unknow13)
                    rate = 2;
                if (AnotherFactory.Instance.AnotherPapers[pc.UsingPaperID].ContainsKey(paper.lv))
                {
                    var paperstatus = AnotherFactory.Instance.AnotherPapers[pc.UsingPaperID][paper.lv];
                    pc.Status.str_anthor = (short)(paperstatus.str * rate);
                    pc.Status.mag_anthor = (short)(paperstatus.mag * rate);
                    pc.Status.agi_anthor = (short)(paperstatus.agi * rate);
                    pc.Status.dex_anthor = (short)(paperstatus.dex * rate);
                    pc.Status.vit_anthor = (short)(paperstatus.vit * rate);
                    pc.Status.int_anthor = (short)(paperstatus.ing * rate);
                    pc.Status.hp_anthor = (short)(paperstatus.hp_add * rate);
                    pc.Status.mp_anthor = (short)(paperstatus.mp_add * rate);
                    pc.Status.sp_anthor = (short)(paperstatus.sp_add * rate);
                    pc.Status.min_atk1_anthor = (short)(paperstatus.min_atk_add * rate);
                    pc.Status.min_atk2_anthor = (short)(paperstatus.min_atk_add * rate);
                    pc.Status.min_atk3_anthor = (short)(paperstatus.min_atk_add * rate);
                    pc.Status.max_atk1_anthor = (short)(paperstatus.max_atk_add * rate);
                    pc.Status.max_atk2_anthor = (short)(paperstatus.max_atk_add * rate);
                    pc.Status.max_atk3_anthor = (short)(paperstatus.max_atk_add * rate);
                    pc.Status.min_matk_anthor = (short)(paperstatus.min_matk_add * rate);
                    pc.Status.max_matk_anthor = (short)(paperstatus.max_matk_add * rate);
                    pc.Status.def_add_anthor = (short)(paperstatus.def_add * rate);
                    pc.Status.mdef_add_anthor = (short)(paperstatus.mdef_add * rate);
                    pc.Status.hit_melee_anthor = (short)(paperstatus.hit_melee_add * rate);
                    pc.Status.hit_ranged_anthor = (short)(paperstatus.hit_magic_add * rate);
                    pc.Status.avoid_melee_anthor = (short)(paperstatus.avoid_melee_add * rate);
                    pc.Status.avoid_ranged_anthor = (short)(paperstatus.avoid_magic_add * rate);
                    return;
                }
            }

            pc.Status.str_anthor = 0;
            pc.Status.mag_anthor = 0;
            pc.Status.agi_anthor = 0;
            pc.Status.dex_anthor = 0;
            pc.Status.vit_anthor = 0;
            pc.Status.int_anthor = 0;
            pc.Status.hp_anthor = 0;
            pc.Status.mp_anthor = 0;
            pc.Status.sp_anthor = 0;
            pc.Status.min_atk1_anthor = 0;
            pc.Status.min_atk2_anthor = 0;
            pc.Status.min_atk3_anthor = 0;
            pc.Status.max_atk1_anthor = 0;
            pc.Status.max_atk2_anthor = 0;
            pc.Status.max_atk3_anthor = 0;
            pc.Status.min_matk_anthor = 0;
            pc.Status.max_matk_anthor = 0;
            pc.Status.def_add_anthor = 0;
            pc.Status.mdef_add_anthor = 0;
            pc.Status.hit_melee_anthor = 0;
            pc.Status.hit_ranged_anthor = 0;
            pc.Status.avoid_melee_anthor = 0;
            pc.Status.avoid_ranged_anthor = 0;
        }

        private void CalcEquipBonus(ActorPC pc)
        {
            pc.Status.ClearItem();
            pc.ClearIrisAbilities();

            pc.Inventory.MaxPayload[ContainerType.BODY] = 0;
            pc.Inventory.MaxPayload[ContainerType.BACK_BAG] = 0;
            pc.Inventory.MaxPayload[ContainerType.LEFT_BAG] = 0;
            pc.Inventory.MaxPayload[ContainerType.RIGHT_BAG] = 0;
            pc.Inventory.MaxVolume[ContainerType.BODY] = 0;
            pc.Inventory.MaxVolume[ContainerType.BACK_BAG] = 0;
            pc.Inventory.MaxVolume[ContainerType.LEFT_BAG] = 0;
            pc.Inventory.MaxVolume[ContainerType.RIGHT_BAG] = 0;

            Dictionary<EnumEquipSlot, Item> equips;
            if (pc.Form == DEM_FORM.NORMAL_FORM)
                equips = pc.Inventory.Equipments;
            else
                equips = pc.Inventory.Parts;
            foreach (var j in equips.Keys)
            {
                if (equips[j] == null)
                    continue;
                var i = equips[j];
                if (i.Stack == 0)
                    continue;

                if (j == EnumEquipSlot.LEFT_HAND)
                    if (i.BaseData.itemType == ItemType.GUN || i.BaseData.itemType == ItemType.DUALGUN ||
                        i.BaseData.itemType == ItemType.RIFLE || i.BaseData.itemType == ItemType.BOW)
                        continue;

                //去掉左手武器判定
                if ((j != EnumEquipSlot.PET || i.BaseData.itemType == ItemType.BACK_DEMON)
                    && !(j == EnumEquipSlot.LEFT_HAND && i.EquipSlot.Contains(EnumEquipSlot.RIGHT_HAND) &&
                         i.EquipSlot.Count == 1))
                {
                    //int weapon_atk1_add = 0, weapon_atk2_add = 0, weapon_atk3_add = 0, weapon_matk_add = 0;
                    //float rate = pc.Status.weapon_rate;

                    ItemFactory.Instance.CalcRefineBouns(i);

                    //weapon_atk1_add += (int)(i.BaseData.atk1 * rate);
                    //weapon_atk2_add += (int)(i.BaseData.atk2 * rate);
                    //weapon_atk3_add += (int)(i.BaseData.atk3 * rate);
                    //weapon_matk_add += (int)(i.BaseData.matk * rate);

                    //pc.Status.atk1_item = (short)((pc.Status.atk1_item + i.BaseData.atk1 + i.Atk1 + weapon_atk1_add + pc.Status.weapon_add_iris));
                    //pc.Status.atk2_item = (short)((pc.Status.atk2_item + i.BaseData.atk2 + i.Atk2 + weapon_atk2_add + pc.Status.weapon_add_iris));
                    //pc.Status.atk3_item = (short)((pc.Status.atk3_item + i.BaseData.atk3 + i.Atk3 + weapon_atk3_add + pc.Status.weapon_add_iris));
                    pc.Status.atk1_item = (short)(pc.Status.atk1_item + i.BaseData.atk1 + i.Atk1);
                    pc.Status.atk2_item = (short)(pc.Status.atk2_item + i.BaseData.atk2 + i.Atk2);
                    pc.Status.atk3_item = (short)(pc.Status.atk3_item + i.BaseData.atk3 + i.Atk3);
                    pc.Status.matk_item = (short)(pc.Status.matk_item + i.BaseData.matk + i.MAtk);

                    pc.Status.def_add_item = (short)(pc.Status.def_add_item + i.BaseData.def + i.Def);
                    pc.Status.mdef_add_item = (short)(pc.Status.mdef_add_item + i.BaseData.mdef + i.MDef);

                    pc.Status.hit_melee_item = (short)(pc.Status.hit_melee_item + i.BaseData.hitMelee + i.HitMelee);
                    pc.Status.hit_ranged_item = (short)(pc.Status.hit_ranged_item + i.BaseData.hitRanged + i.HitRanged);
                    pc.Status.avoid_melee_item =
                        (short)(pc.Status.avoid_melee_item + i.BaseData.avoidMelee + i.AvoidMelee);
                    pc.Status.avoid_ranged_item =
                        (short)(pc.Status.avoid_ranged_item + i.BaseData.avoidRanged + i.AvoidRanged);

                    pc.Status.str_item = (short)(pc.Status.str_item + i.BaseData.str + i.Str);
                    pc.Status.agi_item = (short)(pc.Status.agi_item + i.BaseData.agi + i.Agi);
                    pc.Status.dex_item = (short)(pc.Status.dex_item + i.BaseData.dex + i.Dex);
                    pc.Status.vit_item = (short)(pc.Status.vit_item + i.BaseData.vit + i.Vit);
                    pc.Status.int_item = (short)(pc.Status.int_item + i.BaseData.intel + i.Int);
                    pc.Status.mag_item = (short)(pc.Status.mag_item + i.BaseData.mag + i.Mag);

                    pc.Status.hp_item = pc.Status.hp_item + i.BaseData.hp + i.HP;
                    pc.Status.sp_item = pc.Status.sp_item + i.BaseData.sp + i.SP;
                    pc.Status.mp_item = pc.Status.mp_item + i.BaseData.mp + i.MP;

                    pc.Status.cri_item = (short)(pc.Status.cri_item + i.BaseData.hitCritical + i.HitCritical);
                    pc.Status.criavd_item = (short)(pc.Status.criavd_item + i.BaseData.avoidCritical + i.AvoidCritical);
                    pc.Status.hit_magic_item = (short)(pc.Status.hit_magic_item + i.BaseData.hitMagic + i.HitMagic);
                    pc.Status.avoid_magic_item =
                        (short)(pc.Status.avoid_magic_item + i.BaseData.avoidMagic + i.AvoidMagic);
                    pc.Status.hit_magic_item = (short)(pc.Status.hit_magic_item + i.BaseData.hitMagic + i.HitMagic);

                    pc.Status.speed_item = pc.Status.speed_item + i.BaseData.speedUp + i.SpeedUp;


                    if (i.BaseData.speedUp != 0 || i.SpeedUp != 0)
                        if (pc.Online)
                            pc.e.PropertyUpdate(UpdateEvent.SPEED, 0);

                    if (i.IsWeapon)
                        foreach (var k in i.BaseData.element.Keys)
                            if (i.Element.ContainsKey(k) && i.Element[k] != 0)
                                pc.Status.attackElements_item[k] += i.Element[k];
                            else
                                pc.Status.attackElements_item[k] += i.BaseData.element[k];
                    else if ((i.IsArmor || i.IsEquipt) && !i.IsAmmo)
                        foreach (var k in i.BaseData.element.Keys)
                            if (i.Element.ContainsKey(k) && i.Element[k] != 0)
                                pc.Status.elements_item[k] += i.Element[k];
                            else
                                pc.Status.elements_item[k] += i.BaseData.element[k];

                    if (i.BaseData.itemType == ItemType.ARROW || i.BaseData.itemType == ItemType.BULLET)
                        foreach (var k in i.BaseData.element.Keys)
                            if (i.Element.ContainsKey(k) && i.Element[k] != 0)
                                pc.Status.attackElements_item[k] += i.Element[k];
                            else
                                pc.Status.attackElements_item[k] += i.BaseData.element[k];
                }

                if (i.BaseData.weightUp != 0)
                    switch (j)
                    {
                        case EnumEquipSlot.PET:
                            pc.Inventory.MaxPayload[ContainerType.BODY] =
                                (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] + i.BaseData.weightUp + i.WeightUp);
                            break;
                        case EnumEquipSlot.BACK:
                            pc.Inventory.MaxPayload[ContainerType.BACK_BAG] =
                                (uint)(pc.Inventory.MaxPayload[ContainerType.BACK_BAG] + i.BaseData.weightUp +
                                       i.WeightUp);
                            break;
                        case EnumEquipSlot.LEFT_HAND:
                            pc.Inventory.MaxPayload[ContainerType.LEFT_BAG] =
                                (uint)(pc.Inventory.MaxPayload[ContainerType.LEFT_BAG] + i.BaseData.weightUp +
                                       i.WeightUp);
                            break;
                        case EnumEquipSlot.RIGHT_HAND:
                            pc.Inventory.MaxPayload[ContainerType.RIGHT_BAG] =
                                (uint)(pc.Inventory.MaxPayload[ContainerType.RIGHT_BAG] + i.BaseData.weightUp +
                                       i.WeightUp);
                            break;
                    }

                if (i.BaseData.volumeUp != 0)
                {
                    var rate = 0f;
                    if (pc.Status.Additions.ContainsKey("Packing"))
                    {
                        var skill = (DefaultPassiveSkill)pc.Status.Additions["Packing"];
                        rate = (float)skill["Packing"] / 100;
                    }

                    switch (j)
                    {
                        case EnumEquipSlot.PET:
                            pc.Inventory.MaxVolume[ContainerType.BODY] =
                                (uint)(pc.Inventory.MaxVolume[ContainerType.BODY] +
                                       (i.BaseData.volumeUp + i.VolumeUp) * (1f + rate));
                            break;
                        case EnumEquipSlot.BACK:
                            pc.Inventory.MaxVolume[ContainerType.BACK_BAG] =
                                (uint)(pc.Inventory.MaxVolume[ContainerType.BACK_BAG] +
                                       (i.BaseData.volumeUp + i.VolumeUp) * (1f + rate));
                            break;
                        case EnumEquipSlot.LEFT_HAND:
                            pc.Inventory.MaxVolume[ContainerType.LEFT_BAG] =
                                (uint)(pc.Inventory.MaxVolume[ContainerType.LEFT_BAG] +
                                       (i.BaseData.volumeUp + i.VolumeUp) * (1f + rate));
                            break;
                        case EnumEquipSlot.RIGHT_HAND:
                            pc.Inventory.MaxVolume[ContainerType.RIGHT_BAG] =
                                (uint)(pc.Inventory.MaxVolume[ContainerType.RIGHT_BAG] +
                                       (i.BaseData.volumeUp + i.VolumeUp) * (1f + rate));
                            break;
                    }
                }

                ApplyIrisCardAbilities(pc, i);
                AddItemAddition(pc, i);
            }

            ApplyIrisRes(pc);

            var pcclient = MapClientManager.Instance.FindClient(pc);
            if (pcclient != null)
                pcclient.OnPlayerElements();
            CalcDemicChips(pc);
        }

        public void AddItemAddition(ActorPC pc, Item item)
        {
            var add = item.BaseData.ItemAddition;
            if (add == null)
                return;

            if (add.BonusList == null)
                return;

            var lst = add.BonusList.Where(x => x.EffectType == 0).ToList();
            foreach (var bonus in lst)
            {
                if (bonus.BonusType == 0)
                {
                    var b = bonus.Attribute.Substring(1).ToLower();
                    switch (b)
                    {
                        case "str":
                            pc.Status.str_item += (short)bonus.Values1;
                            break;
                        case "agi":
                            pc.Status.agi_item += (short)bonus.Values1;
                            break;
                        case "vit":
                            pc.Status.vit_item += (short)bonus.Values1;
                            break;
                        case "int":
                            pc.Status.int_item += (short)bonus.Values1;
                            break;
                        case "dex":
                            pc.Status.dex_item += (short)bonus.Values1;
                            break;
                        case "mag":
                            pc.Status.mag_item += (short)bonus.Values1;
                            break;
                        case "def":
                            pc.Status.def_item += (short)bonus.Values1;
                            break;
                        case "def2":
                            pc.Status.def_add_item += (short)bonus.Values1;
                            break;
                        case "mdef":
                            pc.Status.mdef_item += (short)bonus.Values1;
                            break;
                        case "mdef2":
                            pc.Status.mdef_add_item += (short)bonus.Values1;
                            break;
                        case "cri":
                            pc.Status.cri_item += (short)bonus.Values1;
                            break;
                        case "avoidcri":
                            pc.Status.avoid_critical_item += (short)bonus.Values1;
                            break;
                        case "speed":
                            pc.Status.speed_item += (short)bonus.Values1;
                            break;
                        case "aspd":
                            pc.Status.aspd_item += (short)bonus.Values1;
                            break;
                        case "cspd":
                            pc.Status.cspd_item += (short)bonus.Values1;
                            break;
                        case "atk":
                            pc.Status.atk1_item += (short)bonus.Values1;
                            pc.Status.atk2_item += (short)bonus.Values1;
                            pc.Status.atk3_item += (short)bonus.Values1;
                            break;
                        case "atkrate":
                            pc.Status.atk1_rate_item += (short)bonus.Values1;
                            pc.Status.atk2_rate_item += (short)bonus.Values1;
                            pc.Status.atk3_rate_item += (short)bonus.Values1;
                            break;
                        case "matk":
                            pc.Status.matk_item += (short)bonus.Values1;
                            break;
                        case "matkrate":
                            pc.Status.matk_rate_item += (short)bonus.Values1;
                            break;
                        case "defratioatk":
                            pc.Status.DefRatioAtk = bonus.Values1 == 1 ? true : false;
                            break;
                    }
                }

                if (bonus.BonusType == 1)
                {
                    var b = bonus.Attribute.Substring(1).ToLower();
                    switch (b)
                    {
                        case "skillatk":
                            if (pc.Status.SkillRate.ContainsKey((uint)bonus.Values1))
                                pc.Status.SkillRate[(uint)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.SkillRate.Add((uint)bonus.Values1, bonus.Values2);
                            break;
                        case "skilldamage":
                            if (pc.Status.SkillDamage.ContainsKey((uint)bonus.Values1))
                                pc.Status.SkillDamage[(uint)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.SkillDamage.Add((uint)bonus.Values1, bonus.Values2);
                            break;
                        case "addrace":
                            if (pc.Status.AddRace.ContainsKey((byte)bonus.Values1))
                                pc.Status.AddRace[(byte)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.AddRace.Add((byte)bonus.Values1, bonus.Values2);
                            break;
                        case "subrace":
                            if (pc.Status.SubRace.ContainsKey((byte)bonus.Values1))
                                pc.Status.SubRace[(byte)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.SubRace.Add((byte)bonus.Values1, bonus.Values2);
                            break;
                        case "addelement":
                            if (pc.Status.AddElement.ContainsKey((byte)bonus.Values1))
                                pc.Status.AddElement[(byte)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.AddElement.Add((byte)bonus.Values1, bonus.Values2);
                            break;
                        case "subelement":
                            if (pc.Status.SubElement.ContainsKey((byte)bonus.Values1))
                                pc.Status.SubElement[(byte)bonus.Values1] += bonus.Values2;
                            else
                                pc.Status.SubElement.Add((byte)bonus.Values1, bonus.Values2);
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     Calculate item's card ability values on PC
        /// </summary>
        /// <param name="pc"></param>
        /// <param name="item"></param>
        private void ApplyIrisCardAbilities(ActorPC pc, Item item)
        {
            if (!item.Locked)
                return;

            #region Iris Card Ability Calculation

            var IrisCardAbilityValues = item.VectorValues(false, false);
            foreach (var i in IrisCardAbilityValues.Keys)
                if (!pc.IrisAbilityValues.ContainsKey(i))
                    pc.IrisAbilityValues.Add(i, IrisCardAbilityValues[i]);
                else
                    pc.IrisAbilityValues[i] += IrisCardAbilityValues[i];

            #endregion

            var elements = item.IrisElements(false);
            if (item.IsArmor || item.IsWeapon)
            {
                if (item.IsWeapon)
                    foreach (var i in elements.Keys)
                        pc.Status.attackelements_iris[i] += elements[i];

                if (item.IsArmor)
                    foreach (var i in elements.Keys)
                        pc.Status.elements_iris[i] += elements[i];
            }
        }

        /// <summary>
        ///     Calculate item's card ability levels on PC and basic(original) RAs
        /// </summary>
        /// <param name="pc"></param>
        private void ApplyIrisRes(ActorPC pc)
        {
            #region Iris Card Ability Level Calculation

            var lvs = new int[10] { 1, 30, 80, 150, 250, 370, 510, 660, 820, 999 }; //old/original settings
            //int[] lvs = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; new settings
            foreach (var i in pc.IrisAbilityValues.Keys)
            {
                var lv = 0;
                foreach (var item in lvs)
                    if (pc.IrisAbilityValues[i] >= item)
                        lv++;
                pc.IrisAbilityLevels.Add(i, lv);
            }

            #endregion

            #region ReleaseAbility for UI

            var releaseabilities = UIStatusModRAs(pc, pc.IrisAbilityLevels);

            foreach (var i in releaseabilities.Keys)
            {
                var value = releaseabilities[i];
                var Status = pc.Status;
                switch (i)
                {
                    case ReleaseAbility.EXP_HUMAN:
                        break;
                    case ReleaseAbility.EXP_BIRD:
                        break;
                    case ReleaseAbility.EXP_ANIMAL:
                        break;
                    case ReleaseAbility.EXP_INSECT:
                        break;
                    case ReleaseAbility.EXP_ELEMENT:
                        break;
                    case ReleaseAbility.EXP_UNDEAD:
                        break;
                    case ReleaseAbility.STR_UP:
                        pc.Status.str_iris += (short)value;
                        break;
                    case ReleaseAbility.DEX_UP:
                        pc.Status.dex_iris += (short)value;
                        break;
                    case ReleaseAbility.INT_UP:
                        pc.Status.int_iris += (short)value;
                        break;
                    case ReleaseAbility.VIT_UP:
                        pc.Status.vit_iris += (short)value;
                        break;
                    case ReleaseAbility.AGI_UP:
                        pc.Status.agi_iris += (short)value;
                        break;
                    case ReleaseAbility.MAG_UP:
                        pc.Status.mag_iris += (short)value;
                        break;
                    case ReleaseAbility.HP_MAX_UP:
                        pc.Status.hp_iris += (short)value;
                        break;
                    case ReleaseAbility.HP_PER_UP:
                        pc.Status.hp_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.MP_MAX_UP:
                        pc.Status.mp_iris += (short)value;
                        break;
                    case ReleaseAbility.MP_PER_UP:
                        pc.Status.mp_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.SP_MAX_UP:
                        pc.Status.sp_iris += (short)value;
                        break;
                    case ReleaseAbility.SP_PER_UP:
                        pc.Status.sp_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.SKILL_SP_PER_DOWN:
                        pc.Status.sp_rate_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.SKILL_MP_PER_DOWN:
                        pc.Status.mp_rate_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.ATK_FIX_UP:
                        pc.Status.min_atk1_iris += (short)value;
                        pc.Status.min_atk2_iris += (short)value;
                        pc.Status.min_atk3_iris += (short)value;
                        pc.Status.max_atk1_iris += (short)value;
                        pc.Status.max_atk2_iris += (short)value;
                        pc.Status.max_atk3_iris += (short)value;
                        break;
                    case ReleaseAbility.ATK_PER_UP:
                        pc.Status.min_atk1_rate_iris += (short)value;
                        pc.Status.min_atk2_rate_iris += (short)value;
                        pc.Status.min_atk3_rate_iris += (short)value;
                        pc.Status.max_atk1_rate_iris += (short)value;
                        pc.Status.max_atk2_rate_iris += (short)value;
                        pc.Status.max_atk3_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.MATK_FIX_UP:
                        pc.Status.min_matk_iris += (short)value;
                        pc.Status.max_matk_iris += (short)value;
                        break;
                    case ReleaseAbility.MATK_PER_UP:
                        pc.Status.min_matk_rate_iris += (short)value;
                        pc.Status.max_matk_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.SHIT_FIX_UP:
                        pc.Status.hit_melee_iris += (short)value;
                        break;
                    case ReleaseAbility.SHIT_PER_UP:
                        pc.Status.hit_melee_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.LHIT_FIX_UP:
                        pc.Status.hit_ranged_iris += (short)value;
                        break;
                    case ReleaseAbility.LHIT_PER_UP:
                        pc.Status.hit_ranged_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.SAVOID_FIX_UP:
                        pc.Status.avoid_melee_iris += (short)value;
                        break;
                    case ReleaseAbility.SAVOID_PER_UP:
                        pc.Status.avoid_melee_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.LAVOID_FIX_UP:
                        pc.Status.avoid_ranged_iris += (short)value;
                        break;
                    case ReleaseAbility.LAVOID_PER_UP:
                        pc.Status.avoid_ranged_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.DEF_UP:
                        pc.Status.def_iris += (short)value;
                        break;
                    case ReleaseAbility.DEF_PER_UP:
                        //已不使用的属性
                        break;
                    case ReleaseAbility.MDEF_FIX_UP:
                        pc.Status.mdef_iris += (short)value;
                        break;
                    case ReleaseAbility.MDEF_PER_UP:
                        //已不使用的属性
                        break;
                    case ReleaseAbility.WEAPON_FIX_UP:
                        pc.Status.weapon_add_iris += (short)value;
                        break;
                    case ReleaseAbility.WEAPON_PER_UP:
                        pc.Status.weapon_add_rate_iris += (short)value;
                        break;
                    case ReleaseAbility.EQUIP_DEF_FIX_UP:
                        pc.Status.def_add_iris += (short)value;
                        break;
                    case ReleaseAbility.EQUIP_DEF_UP:
                        pc.Status.def_add_iris += (short)(pc.Status.def_add_item * (value / 100));
                        break;
                    case ReleaseAbility.HIT_UP:
                        //已不使用的属性
                        break;
                    case ReleaseAbility.AVOID_UP: //回避成功率
                        pc.Status.avoid_end_rate += (short)value;
                        break;
                    case ReleaseAbility.BDAMAGE_DOWN:
                        pc.Status.physice_rate_iris -= (short)value;
                        break;
                    case ReleaseAbility.MDAMAGE_DOWN:
                        pc.Status.magic_rate_iris -= (short)value;
                        break;
                    case ReleaseAbility.ELMDMG_PER_DWON:
                        pc.Status.Element_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.PT_DAMUP:
                        pc.Status.pt_dmg_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.PT_DAMDOWN:
                        pc.Status.pt_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.HELL_DAMUP:
                        //tbc
                        break;
                    case ReleaseAbility.HELL_DAMDOWN:
                        //tbc
                        break;
                    case ReleaseAbility.CRITICAL_UP:
                        pc.Status.hit_critical_iris += (short)value;
                        break;
                    case ReleaseAbility.CRIAVOID_UP:
                        pc.Status.avoid_critical_iris += (short)value;
                        break;
                    case ReleaseAbility.GUARD_UP:
                        pc.Status.guard_iris += (short)value;
                        break;
                    case ReleaseAbility.PAYLOAD_UP:
                        pc.Status.payl_iris += (short)value;
                        break;
                    case ReleaseAbility.CAPACITY_UP:
                        pc.Status.volume_iris += (short)value;
                        break;
                    case ReleaseAbility.PAYLOAD_FIX_UP:
                        pc.Status.payl_add_iris += (short)value;
                        break;
                    case ReleaseAbility.CAPACITY_FIX_UP:
                        pc.Status.volume_add_iris += (short)value;
                        break;
                    case ReleaseAbility.LV_DIFF_DOWN:
                        pc.Status.level_hit_iris += (short)value;
                        break;
                    case ReleaseAbility.REGI_POISON_UP:
                        pc.AbnormalStatus[AbnormalStatus.Poisen] += (short)value;
                        break;
                    case ReleaseAbility.REGI_STONE_UP:
                        pc.AbnormalStatus[AbnormalStatus.Stone] += (short)value;
                        break;
                    case ReleaseAbility.REGI_SLEEP_UP:
                        pc.AbnormalStatus[AbnormalStatus.Sleep] += (short)value;
                        break;
                    case ReleaseAbility.REGI_SILENCE_UP:
                        pc.AbnormalStatus[AbnormalStatus.Silence] += (short)value;
                        break;
                    case ReleaseAbility.REGI_SLOW_UP:
                        pc.AbnormalStatus[AbnormalStatus.MoveSpeedDown] += (short)value;
                        break;
                    case ReleaseAbility.REGI_CONFUSION_UP:
                        pc.AbnormalStatus[AbnormalStatus.Confused] += (short)value;
                        break;
                    case ReleaseAbility.REGI_ICE_UP:
                        pc.AbnormalStatus[AbnormalStatus.Frosen] += (short)value;
                        break;
                    case ReleaseAbility.REGI_STUN_UP:
                        pc.AbnormalStatus[AbnormalStatus.Stun] += (short)value;
                        break;
                    case ReleaseAbility.CAN_BTPDOWN_PER:
                        //tbc
                        break;
                    case ReleaseAbility.P_CSPD_PER_DOWN:
                        //tbc
                        break;
                    case ReleaseAbility.P_CSPD_PER_UP:
                        //tbc
                        break;
                    case ReleaseAbility.M_CSPD_PER_DOWN:
                        //tbc
                        break;
                    case ReleaseAbility.M_CSPD_PER_UP:
                        //tbc
                        break;
                    case ReleaseAbility.LV_DIFF_UP:
                        pc.Status.level_avoid_iris += (short)value;
                        break;

                    //part related
                    case ReleaseAbility.PART_R_HPMAX_FIX_UP:
                        //tbc
                        if (pc.Partner != null)
                            pc.Partner.MaxHP += (uint)value;
                        else if (pc.Pet != null) pc.Pet.MaxHP += (uint)value;
                        break;
                    case ReleaseAbility.PART_R_HPHEAL_UP:
                        //tbc
                        break;
                    case ReleaseAbility.PART_R_HPMAX_UP:
                        if (pc.Partner != null)
                            pc.Partner.MaxHP += (uint)(pc.Partner.MaxHP * ((float)value / 100));
                        else if (pc.Pet != null) pc.Pet.MaxHP += (uint)(pc.Pet.MaxHP * ((float)value / 100));
                        break;

                    case ReleaseAbility.DUR_DAMAGE_DOWN:
                        //tbc
                        break;
                    case ReleaseAbility.CHGSTATE_RATE_UP:
                        //tbc
                        break;
                    case ReleaseAbility.FOOD_UP:
                        pc.Status.foot_iris += (short)value;
                        break;
                    case ReleaseAbility.POTION_UP:
                        pc.Status.potion_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_HUMAN:
                        pc.Status.human_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_BIRD:
                        pc.Status.bird_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_ANIMAL:
                        pc.Status.animal_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_INSECT:
                        pc.Status.insect_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_MAGIC_CREATURE:
                        pc.Status.magic_c_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_PLANT:
                        pc.Status.plant_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_WATER_ANIMAL:
                        pc.Status.water_a_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_MACHINE:
                        pc.Status.machine_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_ROCK:
                        pc.Status.rock_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_ELEMENT:
                        pc.Status.element_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMUP_UNDEAD:
                        pc.Status.undead_dmg_up_iris += (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_HUMAN:
                        pc.Status.human_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_BIRD:
                        pc.Status.bird_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_ANIMAL:
                        pc.Status.animal_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_INSECT:
                        pc.Status.insect_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_MAGIC_CREATURE:
                        pc.Status.magic_c_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_PLANT:
                        pc.Status.plant_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_WATER_ANIMAL:
                        pc.Status.water_a_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_MACHINE:
                        pc.Status.machine_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_ROCK:
                        pc.Status.rock_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_ELEMENT:
                        pc.Status.element_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.DAMDOWN_UNDEAD:
                        pc.Status.undead_dmg_down_iris -= (short)value;
                        break;
                    case ReleaseAbility.EXP_MAGIC_CREATURE:
                        //tbc
                        break;
                    case ReleaseAbility.EXP_PLANT:
                        //tbc
                        break;
                    case ReleaseAbility.EXP_WATER_ANIMAL:
                        //tbc
                        break;
                    case ReleaseAbility.EXP_MACHINE:
                        //tbc
                        break;
                    case ReleaseAbility.EXP_ROCK:
                        //tbc
                        break;
                    case ReleaseAbility.HIT_HUMAN:
                        pc.Status.human_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_BIRD:
                        pc.Status.bird_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_ANIMAL:
                        pc.Status.animal_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_INSECT:
                        pc.Status.insect_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_MAGIC_CREATURE:
                        pc.Status.magic_c_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_PLANT:
                        pc.Status.plant_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_WATER_ANIMAL:
                        pc.Status.water_a_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_MACHINE:
                        pc.Status.machine_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_ROCK:
                        pc.Status.rock_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_ELEMENT:
                        pc.Status.element_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.HIT_UNDEAD:
                        pc.Status.undead_hit_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_HUMAN:
                        pc.Status.human_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_BIRD:
                        pc.Status.bird_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_ANIMAL:
                        pc.Status.animal_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_INSECT:
                        pc.Status.insect_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_MAGIC_CREATURE:
                        pc.Status.magic_c_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_PLANT:
                        pc.Status.plant_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_WATER_ANIMAL:
                        pc.Status.water_a_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_MACHINE:
                        pc.Status.machine_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_ROCK:
                        pc.Status.rock_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_ELEMENT:
                        pc.Status.element_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.AVOID_UNDEAD:
                        pc.Status.undead_avoid_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_HUMAN:
                        pc.Status.human_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_BIRD:
                        pc.Status.bird_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_ANIMAL:
                        pc.Status.animal_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_INSECT:
                        pc.Status.insect_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_MAGIC_CREATURE:
                        pc.Status.magic_c_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_PLANT:
                        pc.Status.plant_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_WATER_ANIMAL:
                        pc.Status.water_a_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_MACHINE:
                        pc.Status.machine_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_ROCK:
                        pc.Status.rock_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_ELEMENT:
                        pc.Status.element_cri_up_iris -= (short)value;
                        break;
                    case ReleaseAbility.CRITICAL_UNDEAD:
                        pc.Status.undead_cri_up_iris -= (short)value;
                        break;
                }
            }

            #endregion
        }

        /// <summary>
        ///     Get all RAs for ui display
        /// </summary>
        /// <param name="irislevels"></param>
        /// <returns></returns>
        private Dictionary<ReleaseAbility, int> UIStatusModRAs(ActorPC pc, Dictionary<AbilityVector, int> irislevels)
        {
            var list = new Dictionary<ReleaseAbility, int>();
            var genderlist = new Dictionary<PC_GENDER, ushort>();
            genderlist.Add(PC_GENDER.FEMALE, 0);
            genderlist.Add(PC_GENDER.MALE, 0);
            genderlist.Add(PC_GENDER.NONE, 0);
            var joblist = new Dictionary<PC_JOB, ushort>();
            joblist.Add(PC_JOB.GLADIATOR, 0);
            joblist.Add(PC_JOB.HAWKEYE, 0);
            joblist.Add(PC_JOB.FORCEMASTER, 0);
            joblist.Add(PC_JOB.CARDINAL, 0);
            joblist.Add(PC_JOB.NOVICE, 0);
            var racelist = new Dictionary<PC_RACE, ushort>();
            racelist.Add(PC_RACE.EMIL, 0);
            racelist.Add(PC_RACE.TITANIA, 0);
            racelist.Add(PC_RACE.DOMINION, 0);
            racelist.Add(PC_RACE.DEM, 0);
            racelist.Add(PC_RACE.NONE, 0);

            if (pc.Party != null)
                foreach (var j in pc.Party.Members.Values)
                {
                    if (genderlist.ContainsKey(j.Gender))
                        genderlist[j.Gender]++;
                    else
                        genderlist.Add(j.Gender, 1);
                    if (joblist.ContainsKey(j.Job))
                        joblist[j.Job]++;
                    else
                        joblist.Add(j.Job, 1);
                    if (racelist.ContainsKey(j.Race))
                        racelist[j.Race]++;
                    else
                        racelist.Add(j.Race, 1);
                }

            foreach (var i in irislevels.Keys)
            {
                var RAstate = false;
                if (i.ID < 1000) //原版能力
                {
                    RAstate = true;
                }
                else if (i.ID >= 1000 && i.ID < 2000) //组队条件触发的面板显示的能力
                {
                    if (i.ID < 1100) //单人or无队伍条件
                    {
                        if (pc.Party == null)
                            RAstate = true;
                    }
                    else //多人队伍条件
                    {
                        if (pc.Party != null)
                            switch (i.ID)
                            {
                                case 1101:
                                    if (pc.Party.Members.Count == 2 && genderlist[PC_GENDER.FEMALE] == 1)
                                        RAstate = true;
                                    break;
                                case 1102:
                                    if (pc.Party.Members.Count == 2 && genderlist[PC_GENDER.MALE] == 2) RAstate = true;
                                    break;
                                case 1103:
                                    if (pc.Party.Members.Count == 2 && genderlist[PC_GENDER.FEMALE] == 2)
                                        RAstate = true;
                                    break;
                                case 1801:
                                case 1802:
                                    if (joblist[PC_JOB.GLADIATOR] > 0 && joblist[PC_JOB.HAWKEYE] > 0 &&
                                        joblist[PC_JOB.FORCEMASTER] > 0 && joblist[PC_JOB.CARDINAL] > 0) RAstate = true;
                                    break;
                                case 1804:
                                    if (joblist[PC_JOB.GLADIATOR] > 0 && joblist[PC_JOB.HAWKEYE] > 0 &&
                                        joblist[PC_JOB.FORCEMASTER] > 0 && joblist[PC_JOB.CARDINAL] > 0)
                                        if (joblist[PC_JOB.GLADIATOR] < 3 && joblist[PC_JOB.HAWKEYE] < 3 &&
                                            joblist[PC_JOB.FORCEMASTER] < 3 && joblist[PC_JOB.CARDINAL] < 3)
                                            RAstate = true;
                                    break;
                                case 1901:
                                case 1902:
                                    if (racelist[PC_RACE.EMIL] > 0 && racelist[PC_RACE.TITANIA] > 0 &&
                                        racelist[PC_RACE.DOMINION] > 0) RAstate = true;
                                    break;
                            }
                    }
                }

                if (RAstate)
                {
                    var ability = i.ReleaseAbilities[(byte)irislevels[i]];
                    foreach (var j in ability.Keys)
                        if (list.ContainsKey(j))
                            list[j] = Math.Max(list[j], ability[j]);
                        //list[j] += ability[j];
                        else
                            list.Add(j, ability[j]);
                }
            }

            return list;
        }

        private void CalcDemicChips(ActorPC pc)
        {
            Dictionary<byte, DEMICPanel> chips;
            if (pc.InDominionWorld)
                chips = pc.Inventory.DominionDemicChips;
            else
                chips = pc.Inventory.DemicChips;
            var skills = new Dictionary<uint, int>();

            #region CalcChips

            foreach (var i in chips.Keys)
                foreach (var j in chips[i].Chips)
                {
                    byte x1 = 255, y1 = 255, x2 = 255, y2 = 255;
                    if (chips[i].EngageTask1 != 255)
                    {
                        x1 = (byte)(chips[i].EngageTask1 % 9);
                        y1 = (byte)(chips[i].EngageTask1 / 9);
                    }

                    if (chips[i].EngageTask2 != 255)
                    {
                        x2 = (byte)(chips[i].EngageTask2 % 9);
                        y2 = (byte)(chips[i].EngageTask2 / 9);
                    }

                    var nearEngage = j.IsNear(x1, y1) || j.IsNear(x2, y2);

                    if (j.Data.type < 20)
                    {
                        var rate = 1;
                        if (nearEngage)
                            rate = 2;
                        pc.Status.m_str_chip += (short)(rate * j.Data.str);
                        pc.Status.m_agi_chip += (short)(rate * j.Data.agi);
                        pc.Status.m_vit_chip += (short)(rate * j.Data.vit);
                        pc.Status.m_dex_chip += (short)(rate * j.Data.dex);
                        pc.Status.m_int_chip += (short)(rate * j.Data.intel);
                        pc.Status.m_mag_chip += (short)(rate * j.Data.mag);
                    }
                    else if (j.Data.type < 30)
                    {
                        var level = 1;
                        if (nearEngage)
                            level = 2;
                        if (skills.ContainsKey(j.Data.skill1))
                            skills[j.Data.skill1] += level;
                        else if (j.Data.skill1 != 0)
                            skills.Add(j.Data.skill1, level);

                        if (skills.ContainsKey(j.Data.skill2))
                            skills[j.Data.skill2] += level;
                        else if (j.Data.skill2 != 0)
                            skills.Add(j.Data.skill2, level);

                        if (skills.ContainsKey(j.Data.skill3))
                            skills[j.Data.skill3] += level;
                        else if (j.Data.skill3 != 0)
                            skills.Add(j.Data.skill3, level);
                    }
                    else
                    {
                        Chip next = null;
                        if (ChipFactory.Instance.ByChipID.ContainsKey(j.Data.engageTaskChip) && nearEngage)
                            next = new Chip(ChipFactory.Instance.ByChipID[j.Data.engageTaskChip]);
                        else
                            next = j;
                        pc.Status.m_str_chip += next.Data.str;
                        pc.Status.m_agi_chip += next.Data.agi;
                        pc.Status.m_vit_chip += next.Data.vit;
                        pc.Status.m_dex_chip += next.Data.dex;
                        pc.Status.m_int_chip += next.Data.intel;
                        pc.Status.m_mag_chip += next.Data.mag;
                        pc.Status.hp_rate_item += next.Data.hp;
                        pc.Status.sp_rate_item += next.Data.sp;
                        pc.Status.mp_rate_item += next.Data.mp;
                    }
                }

            #endregion

            foreach (var i in skills.Keys)
            {
                var level = 0;
                if (pc.Form != DEM_FORM.MACHINA_FORM)
                    level = 0;
                else
                    level = skills[i];
                if (pc.Skills.ContainsKey(i))
                {
                    if (pc.Skills[i].Level != level)
                    {
                        pc.Skills[i].Level = (byte)level;
                        if (pc.Skills[i].Level > pc.Skills[i].MaxLevel)
                            pc.Skills[i].Level = pc.Skills[i].MaxLevel;
                    }
                }
                else
                {
                    var skill = SkillFactory.Instance.GetSkill(i, 1);
                    skill.Level = (byte)level;
                    if (skill.Level > skill.MaxLevel)
                        skill.Level = skill.MaxLevel;
                    skill.NoSave = true;
                    pc.Skills.Add(i, skill);
                }
            }
        }

        public void CalcStatus(ActorPC pc)
        {
            //bool blocked = ClientManager.Blocked;
            //if (!blocked)
            //    ClientManager.EnterCriticalArea();
            CalcEquipBonus(pc);
            CalcAnotherPaperBonus(pc);
            //CalcPlayerTitleBouns(pc);
            CalcRange(pc);
            CalcStatsRev(pc);
            CalcPayV(pc);
            CalcHPMPSP(pc);
            CalcStats(pc);
            pc.Inventory.CalcPayloadVolume();
            //if (blocked)
            //    ClientManager.LeaveCriticalArea();    
        }

        public void CalcStatusOnSkillEffect(ActorPC pc)
        {
            CalcHPMPSP(pc);
            CalcStats(pc);
        }

        public void CalcRange(ActorPC pc)
        {
            Dictionary<EnumEquipSlot, Item> equips;

            if (pc.Form == DEM_FORM.NORMAL_FORM)
                equips = pc.Inventory.Equipments;
            else
                equips = pc.Inventory.Parts;

            if (equips.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                var item = equips[EnumEquipSlot.RIGHT_HAND];
                pc.Range = item.BaseData.range;
            }
            else
            {
                if (equips.ContainsKey(EnumEquipSlot.LEFT_HAND))
                {
                    var item = equips[EnumEquipSlot.LEFT_HAND];
                    pc.Range = item.BaseData.range;
                }
                else
                {
                    pc.Range = 1;
                }
            }
        }

        private ushort checkPositive(double num)
        {
            if (num > 0)
                return (ushort)num;
            return 0;
        }

        private ushort checkHighVitBonus(ActorPC pc)
        {
            var vitcount = pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev + pc.Status.vit_mario +
                           pc.Status.vit_skill + pc.Status.vit_iris;
            if (vitcount >= 120 && vitcount < 150)
                return 2;
            if (vitcount >= 150)
                return 5;
            return 0;
        }

        private ushort checkHighIntBonus(ActorPC pc)
        {
            var intcount = pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev + pc.Status.int_mario +
                           pc.Status.int_skill + pc.Status.int_iris;
            if (intcount >= 120 && intcount < 150)
                return 2;
            if (intcount >= 150)
                return 5;
            return 0;
        }

        /// <summary>
        ///     计算素质属性能力
        /// </summary>
        /// <param name="pc"></param>
        private void CalcStats(ActorPC pc)
        {
            //获取玩家基础能力
            var pcstr = checkPositive(pc.Str + pc.Status.str_item + pc.Status.str_chip + pc.Status.str_rev +
                                      pc.Status.str_mario + pc.Status.str_skill + pc.Status.str_iris);
            var pcdex = checkPositive(pc.Dex + pc.Status.dex_item + pc.Status.dex_chip + pc.Status.dex_rev +
                                      pc.Status.dex_mario + pc.Status.dex_skill + pc.Status.dex_iris);
            var pcint = checkPositive(pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev +
                                      pc.Status.int_mario + pc.Status.int_skill + pc.Status.int_iris);
            var pcvit = checkPositive(pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev +
                                      pc.Status.vit_mario + pc.Status.vit_skill + pc.Status.vit_iris);
            var pcagi = checkPositive(pc.Agi + pc.Status.agi_item + pc.Status.agi_chip + pc.Status.agi_rev +
                                      pc.Status.agi_mario + pc.Status.agi_skill + pc.Status.agi_iris);
            var pcmag = checkPositive(pc.Mag + pc.Status.mag_item + pc.Status.mag_chip + pc.Status.mag_rev +
                                      pc.Status.mag_mario + pc.Status.mag_skill + pc.Status.mag_iris);
            if (pc.Status.Additions.ContainsKey("ModeChange"))
            {
                #region 物魔互换模块(基础属性和buff装备分离部分)

                pcstr = checkPositive(pc.Str + pc.Status.mag_item + pc.Status.mag_chip + pc.Status.str_rev +
                                      pc.Status.mag_mario + pc.Status.mag_skill + pc.Status.mag_iris);
                pcdex = checkPositive(pc.Dex + pc.Status.agi_item + pc.Status.agi_chip + pc.Status.dex_rev +
                                      pc.Status.agi_mario + pc.Status.agi_skill + pc.Status.agi_iris);
                pcint = checkPositive(pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev +
                                      pc.Status.int_mario + pc.Status.int_skill + pc.Status.int_iris);
                pcvit = checkPositive(pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev +
                                      pc.Status.vit_mario + pc.Status.vit_skill + pc.Status.vit_iris);
                pcagi = checkPositive(pc.Agi + pc.Status.dex_item + pc.Status.dex_chip + pc.Status.agi_rev +
                                      pc.Status.dex_mario + pc.Status.dex_skill + pc.Status.dex_iris);
                pcmag = checkPositive(pc.Mag + pc.Status.str_item + pc.Status.str_chip + pc.Status.mag_rev +
                                      pc.Status.str_mario + pc.Status.str_skill + pc.Status.str_iris);

                #endregion
            }

            if (pc.Pet != null && pc.Pet.Ride)
            {
                pc.Status.min_atk1 = pc.Pet.Status.min_atk1;
                pc.Status.min_atk2 = pc.Pet.Status.min_atk2;
                pc.Status.min_atk3 = pc.Pet.Status.min_atk3;
                pc.Status.max_atk1 = pc.Pet.Status.max_atk1;
                pc.Status.max_atk2 = pc.Pet.Status.max_atk2;
                pc.Status.max_atk3 = pc.Pet.Status.max_atk3;
                pc.Status.min_matk = pc.Pet.Status.min_matk;
                pc.Status.max_matk = pc.Pet.Status.min_matk;
                pc.Status.def = pc.Pet.Status.def;
                pc.Status.def_add = pc.Pet.Status.def_add;
                pc.Status.mdef = pc.Pet.Status.mdef;
                pc.Status.mdef_add = pc.Pet.Status.mdef_add;
                pc.Status.aspd = pc.Pet.Status.aspd;
                //pc.Status.cspd = pc.Pet.Status.cspd;
                pc.Status.hit_melee = pc.Pet.Status.hit_melee;
                pc.Status.hit_ranged = pc.Pet.Status.hit_ranged;
                pc.Status.avoid_melee = pc.Pet.Status.avoid_melee;
                pc.Status.avoid_ranged = pc.Pet.Status.avoid_ranged;
                pc.Status.hit_critical = pc.Pet.Status.hit_critical;
                pc.Status.avoid_critical = pc.Pet.Status.avoid_critical;
                pc.Speed = pc.Pet.Speed;
            }
            else
            {
                if (pc.Status.Additions.ContainsKey("ModeChange"))
                {
                    #region 物魔互换模块(攻击力计算部分)

                    //攻击力计算
                    var minmatk = (ushort)Math.Floor(pcstr + Math.Pow(Math.Floor((double)(pcstr / 9)), 2));
                    var minatk = (ushort)Math.Floor(pcmag + Math.Pow(Math.Floor((float)(pcmag + 9) / 8), 2) *
                        (1.0f + Math.Floor(pcint * 1.2f) / 320.0f));
                    var maxmatk = (ushort)(pcstr + Math.Pow(Math.Floor((pcstr + 14) / 5.0f), 2));
                    var maxatk = (ushort)(pcmag + Math.Pow(Math.Floor((pcmag + 17) / 6.0f), 2));

                    minatk = (ushort)(minatk * CalcATKRate(pc));

                    int weapon_atk1_add = 0, weapon_atk2_add = 0, weapon_atk3_add = 0, weapon_matk_add = 0;
                    float rate = pc.Status.weapon_add_rate_iris;
                    weapon_atk1_add += (int)(pc.Status.atk1_item * (rate / 100.0f));
                    weapon_atk2_add += (int)(pc.Status.atk2_item * (rate / 100.0f));
                    weapon_atk3_add += (int)(pc.Status.atk3_item * (rate / 100.0f));
                    weapon_matk_add += (int)(pc.Status.matk_item * (rate / 100.0f));
                    //pc.Status.min_atk_bs = (ushort)checkPositive((float)((pc.Str + pc.Status.str_item + pc.Status.str_rev) * 2f + (pc.Dex + pc.Status.dex_item + pc.Status.dex_rev) * 1f));
                    pc.Status.min_atk_bs = minatk;

                    pc.Status.min_atk1 = checkPositive(
                        (minatk + pc.Status.atk1_item + pc.Status.min_atk1_mario) *
                        (float)(pc.Status.min_atk1_rate_iris / 100) * (pc.Status.min_atk1_rate_skill / 100) +
                        pc.Status.min_atk1_skill + pc.Status.min_atk1_iris + pc.Status.weapon_add_iris +
                        weapon_atk1_add);
                    pc.Status.min_atk2 = checkPositive(
                        (minatk + pc.Status.atk2_item + pc.Status.min_atk2_mario) *
                        (float)(pc.Status.min_atk2_rate_iris / 100) * (pc.Status.min_atk2_rate_skill / 100) +
                        pc.Status.min_atk2_skill + pc.Status.min_atk2_iris + pc.Status.weapon_add_iris +
                        weapon_atk2_add);
                    pc.Status.min_atk3 = checkPositive(
                        (minatk + pc.Status.atk3_item + pc.Status.min_atk3_mario) *
                        (float)(pc.Status.min_atk3_rate_iris / 100) * (pc.Status.min_atk3_rate_skill / 100) +
                        pc.Status.min_atk3_skill + pc.Status.min_atk3_iris + pc.Status.weapon_add_iris +
                        weapon_atk3_add);


                    //pc.Status.max_atk_bs = (ushort)checkPositive((float)((pc.Str + pc.Status.str_item + pc.Status.str_rev) * 5f + (pc.Dex + pc.Status.dex_item + pc.Status.dex_rev) * 1f));
                    pc.Status.max_atk_bs = maxatk;

                    pc.Status.max_atk1 = checkPositive(
                        (maxatk + pc.Status.atk1_item + pc.Status.max_atk1_mario) *
                        (float)(pc.Status.max_atk1_rate_iris / 100) * (pc.Status.max_atk1_rate_skill / 100) +
                        pc.Status.max_atk1_skill + pc.Status.max_atk1_iris + pc.Status.weapon_add_iris +
                        weapon_atk1_add);
                    pc.Status.max_atk2 = checkPositive(
                        (maxatk + pc.Status.atk2_item + pc.Status.max_atk2_mario) *
                        (float)(pc.Status.max_atk2_rate_iris / 100) * (pc.Status.max_atk2_rate_skill / 100) +
                        pc.Status.max_atk2_skill + pc.Status.max_atk2_iris + pc.Status.weapon_add_iris +
                        weapon_atk2_add);
                    pc.Status.max_atk3 = checkPositive(
                        (maxatk + pc.Status.atk3_item + pc.Status.max_atk3_mario) *
                        (float)(pc.Status.max_atk3_rate_iris / 100) * (pc.Status.max_atk3_rate_skill / 100) +
                        pc.Status.max_atk3_skill + pc.Status.max_atk3_iris + pc.Status.weapon_add_iris +
                        weapon_atk3_add);


                    //pc.Status.max_matk_bs = (ushort)checkPositive((float)((pc.Mag + pc.Status.mag_item + pc.Status.mag_rev) * 2f + (pc.Int + pc.Status.int_item + pc.Status.int_rev) * 1f));
                    pc.Status.min_matk_bs = minmatk;

                    pc.Status.min_matk = checkPositive(
                        (minmatk + pc.Status.matk_item + pc.Status.min_matk_mario) *
                        (float)(pc.Status.min_matk_rate_iris / 100) * (pc.Status.min_matk_rate_skill / 100) +
                        pc.Status.min_matk_skill + pc.Status.min_matk_iris + pc.Status.weapon_add_iris +
                        weapon_matk_add);


                    //pc.Status.max_matk_bs = (ushort)checkPositive((float)((pc.Mag + pc.Status.mag_item + pc.Status.mag_rev) * 5f + (pc.Int + pc.Status.int_item + pc.Status.int_rev) * 1f));
                    pc.Status.max_matk_bs = maxmatk;
                    pc.Status.max_matk = checkPositive(
                        (maxmatk + pc.Status.matk_item + pc.Status.max_matk_mario) *
                        (float)(pc.Status.min_matk_rate_iris / 100) * (pc.Status.min_matk_rate_skill / 100) +
                        pc.Status.max_matk_skill + pc.Status.max_matk_iris + pc.Status.weapon_add_iris +
                        weapon_matk_add);

                    #endregion
                }
                else
                {
                    //攻击力计算
                    var minatk = (ushort)Math.Floor(pcstr + Math.Pow(Math.Floor((double)(pcstr / 9)), 2));
                    minatk = (ushort)(minatk * CalcATKRate(pc));

                    int weapon_atk1_add = 0, weapon_atk2_add = 0, weapon_atk3_add = 0, weapon_matk_add = 0;
                    float rate = pc.Status.weapon_add_rate_iris;
                    weapon_atk1_add += (int)(pc.Status.atk1_item * (rate / 100.0f));
                    weapon_atk2_add += (int)(pc.Status.atk2_item * (rate / 100.0f));
                    weapon_atk3_add += (int)(pc.Status.atk3_item * (rate / 100.0f));
                    weapon_matk_add += (int)(pc.Status.matk_item * (rate / 100.0f));
                    //pc.Status.min_atk_bs = (ushort)checkPositive((float)((pc.Str + pc.Status.str_item + pc.Status.str_rev) * 2f + (pc.Dex + pc.Status.dex_item + pc.Status.dex_rev) * 1f));
                    pc.Status.min_atk_bs = minatk;

                    pc.Status.min_atk1 = checkPositive(
                        (minatk + pc.Status.atk1_item + pc.Status.min_atk1_mario) *
                        (pc.Status.min_atk1_rate_iris / 100.0f) * (pc.Status.min_atk1_rate_skill / 100.0f) +
                        pc.Status.min_atk1_skill + pc.Status.min_atk1_iris + pc.Status.weapon_add_iris +
                        weapon_atk1_add);
                    pc.Status.min_atk2 = checkPositive(
                        (minatk + pc.Status.atk2_item + pc.Status.min_atk1_mario) *
                        (pc.Status.min_atk2_rate_iris / 100.0f) * (pc.Status.min_atk2_rate_skill / 100.0f) +
                        pc.Status.min_atk2_skill + pc.Status.min_atk2_iris + pc.Status.weapon_add_iris +
                        weapon_atk2_add);
                    pc.Status.min_atk3 = checkPositive(
                        (minatk + pc.Status.atk3_item + pc.Status.min_atk1_mario) *
                        (pc.Status.min_atk3_rate_iris / 100.0f) * (pc.Status.min_atk3_rate_skill / 100.0f) +
                        pc.Status.min_atk3_skill + pc.Status.min_atk3_iris + pc.Status.weapon_add_iris +
                        weapon_atk3_add);
                    //pc.Status.min_atk2 = (ushort)checkPositive((minatk + pc.Status.atk2_item + pc.Status.min_atk2_mario + pc.Status.min_atk2_skill + pc.Status.min_atk2_iris + pc.Status.weapon_add_iris + weapon_atk2_add) * (float)(pc.Status.min_atk2_rate_iris / 100.0f) * (float)(pc.Status.min_atk2_rate_skill / 100.0f));
                    //pc.Status.min_atk3 = (ushort)checkPositive((minatk + pc.Status.atk3_item + pc.Status.min_atk3_mario + pc.Status.min_atk3_skill + pc.Status.min_atk3_iris + pc.Status.weapon_add_iris + weapon_atk3_add) * (float)(pc.Status.min_atk3_rate_iris / 100.0f) * (float)(pc.Status.min_atk3_rate_skill / 100.0f));

                    var maxatk = (ushort)(pcstr + Math.Pow(Math.Floor((pcstr + 14) / 5.0f), 2));
                    //pc.Status.max_atk_bs = (ushort)checkPositive((float)((pc.Str + pc.Status.str_item + pc.Status.str_rev) * 5f + (pc.Dex + pc.Status.dex_item + pc.Status.dex_rev) * 1f));
                    pc.Status.max_atk_bs = maxatk;

                    pc.Status.max_atk1 = checkPositive(
                        (maxatk + pc.Status.atk1_item + pc.Status.max_atk1_mario) *
                        (pc.Status.max_atk1_rate_iris / 100.0f) * (pc.Status.max_atk1_rate_skill / 100.0f) +
                        pc.Status.max_atk1_skill + pc.Status.max_atk1_iris + pc.Status.weapon_add_iris +
                        weapon_atk1_add);
                    pc.Status.max_atk2 = checkPositive(
                        (maxatk + pc.Status.atk2_item + pc.Status.max_atk1_mario) *
                        (pc.Status.max_atk2_rate_iris / 100.0f) * (pc.Status.max_atk2_rate_skill / 100.0f) +
                        pc.Status.max_atk2_skill + pc.Status.max_atk2_iris + pc.Status.weapon_add_iris +
                        weapon_atk2_add);
                    pc.Status.max_atk3 = checkPositive(
                        (maxatk + pc.Status.atk3_item + pc.Status.max_atk1_mario) *
                        (pc.Status.max_atk3_rate_iris / 100.0f) * (pc.Status.max_atk3_rate_skill / 100.0f) +
                        pc.Status.max_atk3_skill + pc.Status.max_atk3_iris + pc.Status.weapon_add_iris +
                        weapon_atk3_add);
                    //pc.Status.max_atk1 = (ushort)checkPositive((maxatk + pc.Status.atk1_item + pc.Status.max_atk1_mario + pc.Status.max_atk1_skill + pc.Status.max_atk1_iris + pc.Status.weapon_add_iris + weapon_atk1_add) * (float)(pc.Status.max_atk1_rate_iris / 100.0f) * (float)(pc.Status.max_atk1_rate_skill / 100.0f));
                    //pc.Status.max_atk2 = (ushort)checkPositive((maxatk + pc.Status.atk2_item + pc.Status.max_atk2_mario + pc.Status.max_atk2_skill + pc.Status.max_atk2_iris + pc.Status.weapon_add_iris + weapon_atk2_add) * (float)(pc.Status.max_atk2_rate_iris / 100.0f) * (float)(pc.Status.max_atk2_rate_skill / 100.0f));
                    //pc.Status.max_atk3 = (ushort)checkPositive((maxatk + pc.Status.atk3_item + pc.Status.max_atk3_mario + pc.Status.max_atk3_skill + pc.Status.max_atk3_iris + pc.Status.weapon_add_iris + weapon_atk3_add) * (float)(pc.Status.max_atk3_rate_iris / 100.0f) * (float)(pc.Status.max_atk3_rate_skill / 100.0f));

                    var minmatk = (ushort)Math.Floor(pcmag + Math.Pow(Math.Floor((float)(pcmag + 9) / 8), 2) *
                        (1.0f + Math.Floor(pcint * 1.2f) / 320.0f));

                    //pc.Status.max_matk_bs = (ushort)checkPositive((float)((pc.Mag + pc.Status.mag_item + pc.Status.mag_rev) * 2f + (pc.Int + pc.Status.int_item + pc.Status.int_rev) * 1f));
                    pc.Status.min_matk_bs = minmatk;

                    pc.Status.min_matk = checkPositive(
                        (minmatk + pc.Status.matk_item + pc.Status.min_matk_mario) *
                        (pc.Status.min_matk_rate_iris / 100.0f) * (pc.Status.min_matk_rate_skill / 100.0f) +
                        pc.Status.min_matk_skill + pc.Status.min_matk_iris + pc.Status.weapon_add_iris +
                        weapon_matk_add);

                    var maxmatk = (ushort)(pcmag + Math.Pow(Math.Floor((pcmag + 17) / 6.0f), 2));

                    //pc.Status.max_matk_bs = (ushort)checkPositive((float)((pc.Mag + pc.Status.mag_item + pc.Status.mag_rev) * 5f + (pc.Int + pc.Status.int_item + pc.Status.int_rev) * 1f));
                    pc.Status.max_matk_bs = maxmatk;

                    pc.Status.max_matk = checkPositive(
                        (maxmatk + pc.Status.matk_item + pc.Status.max_matk_mario) *
                        (pc.Status.min_matk_rate_iris / 100.0f) * (pc.Status.min_matk_rate_skill / 100.0f) +
                        pc.Status.max_matk_skill + pc.Status.max_matk_iris + pc.Status.weapon_add_iris +
                        weapon_matk_add);
                }

                #region 最小攻击力大于最大攻击力的修正部分

                if (pc.Status.min_atk1 > pc.Status.max_atk1)
                    pc.Status.min_atk1 = pc.Status.max_atk1;
                if (pc.Status.min_atk2 > pc.Status.max_atk2)
                    pc.Status.min_atk2 = pc.Status.max_atk2;
                if (pc.Status.min_atk3 > pc.Status.max_atk3)
                    pc.Status.min_atk3 = pc.Status.max_atk3;
                if (pc.Status.min_matk > pc.Status.max_matk)
                    pc.Status.min_matk = pc.Status.max_matk;

                #endregion

                //命中计算
                var hit_melee = (ushort)(pcdex + (short)Math.Floor(pcdex / 10.0f) * 11 + pc.Level + 3);
                pc.Status.hit_melee =
                    checkPositive(
                        (hit_melee + pc.Status.hit_melee_item + pc.Status.hit_melee_skill + pc.Status.hit_melee_iris) *
                        (pc.Status.hit_melee_rate_iris / 100.0f));

                var hit_ranged = (ushort)(pcint + (short)Math.Floor(pcint / 10.0f) * 11 + pc.Level + 3);
                pc.Status.hit_ranged =
                    checkPositive(
                        (hit_ranged + pc.Status.hit_ranged_item + pc.Status.hit_ranged_skill +
                         pc.Status.hit_ranged_iris) * (pc.Status.hit_ranged_rate_iris / 100.0f));

                //防御计算
                pc.Status.def = Math.Min(checkPositive(pcvit / 3 + (int)(pcvit / 4.5f)),
                    Configuration.Configuration.Instance.BasePhysicDef);
                pc.Status.def_bs = pc.Status.def;
                pc.Status.def += checkPositive(pc.Status.def_skill + checkHighVitBonus(pc) + pc.Status.def_item +
                                               pc.Status.def_iris);

                pc.Status.def_add = (short)checkPositive(pc.Status.def_add_mario + pc.Status.def_add_skill +
                                                         pc.Status.def_add_item + pc.Status.def_add_iris);

                pc.Status.mdef = Math.Min(checkPositive(pcint / 3 + (int)(pcvit / 4.0f)),
                    Configuration.Configuration.Instance.BaseMagicDef);
                pc.Status.mdef_bs = pc.Status.mdef;
                pc.Status.mdef += checkPositive(pc.Status.mdef_skill + checkHighIntBonus(pc) + pc.Status.mdef_item +
                                                pc.Status.mdef_iris);

                pc.Status.mdef_add = (short)checkPositive(pc.Status.mdef_add_mario + pc.Status.mdef_add_skill +
                                                          pc.Status.mdef_add_item + pc.Status.mdef_add_iris);

                //闪避计算
                var avoid_melee = (ushort)(pcagi + Math.Pow(Math.Floor((pcagi + 18) / 9.0f), 2) +
                    Math.Floor(pc.Level / 3.0f) - 1);
                pc.Status.avoid_melee =
                    checkPositive((avoid_melee + pc.Status.avoid_melee_item + pc.Status.avoid_melee_skill) *
                                  (pc.Status.avoid_melee_rate_iris / 100.0f));

                var avoid_ranged = (ushort)(Math.Floor(pcint * 5.0f / 3.0f) + pcagi + Math.Floor(pc.Level / 3.0f) + 3);
                pc.Status.avoid_ranged =
                    checkPositive((avoid_ranged + pc.Status.avoid_ranged_item + pc.Status.avoid_ranged_skill) *
                                  (pc.Status.avoid_ranged_rate_iris / 100.0f));

                //会心计算
                pc.Status.hit_critical = (ushort)Math.Max(Math.Floor((double)((pcdex + 1) / 8)), 1);
                pc.Status.avoid_critical = (ushort)((pcagi + 8) / 6.0f);
                //pc.Status.hit_magic = (ushort)((pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev + pc.Status.int_mario + pc.Status.int_skill) * 0.2f);
                //calculate the possession spirit status
                foreach (var i in pc.PossesionedActors)
                {
                    if (i == pc)
                        continue;

                    pc.Status.min_atk1 = checkPositive(pc.Status.min_atk1 + i.Status.min_atk1_possession);
                    pc.Status.min_atk2 = checkPositive(pc.Status.min_atk2 + i.Status.min_atk2_possession);
                    pc.Status.min_atk3 = checkPositive(pc.Status.min_atk3 + i.Status.min_atk3_possession);
                    pc.Status.max_atk1 = checkPositive(pc.Status.max_atk1 + i.Status.max_atk1_possession);
                    pc.Status.max_atk2 = checkPositive(pc.Status.max_atk2 + i.Status.max_atk2_possession);
                    pc.Status.max_atk3 = checkPositive(pc.Status.max_atk3 + i.Status.max_atk3_possession);
                    pc.Status.min_matk = checkPositive(pc.Status.min_matk + i.Status.min_matk_possession);
                    pc.Status.max_matk = checkPositive(pc.Status.max_matk + i.Status.max_matk_possession);
                    pc.Status.hit_melee = checkPositive(pc.Status.hit_melee + i.Status.hit_melee_possession);
                    pc.Status.hit_ranged = checkPositive(pc.Status.hit_ranged + i.Status.hit_ranged_possession);
                    pc.Status.avoid_melee = checkPositive(pc.Status.avoid_melee + i.Status.avoid_melee_possession);
                    pc.Status.avoid_ranged = checkPositive(pc.Status.avoid_ranged + i.Status.avoid_ranged_possession);
                    pc.Status.def = checkPositive(pc.Status.def + i.Status.def_possession);
                    pc.Status.def_add = (short)checkPositive(pc.Status.def_add + i.Status.def_add_possession);
                    pc.Status.mdef = checkPositive(pc.Status.mdef + i.Status.mdef_possession);
                    pc.Status.mdef_add = (short)checkPositive(pc.Status.mdef_add + i.Status.mdef_add_possession);
                }

                pc.Status.def = Math.Min(pc.Status.def, Configuration.Configuration.Instance.MaxPhysicDef);
                pc.Status.mdef = Math.Min(pc.Status.mdef, Configuration.Configuration.Instance.MaxMagicDef);

                //攻速计算
                if (pc.Status.Additions.ContainsKey("ModeChange"))
                    pc.Status.aspd = (short)(pcdex * 3 + Math.Floor(Math.Pow((short)((pcdex + 63) / 9.0f), 2)) + 129 +
                                             pc.Status.aspd_skill);
                else
                    pc.Status.aspd = (short)(pcagi * 3 + Math.Floor(Math.Pow((short)((pcagi + 63) / 9.0f), 2)) + 129 +
                                             pc.Status.aspd_skill);
            }

            //唱速计算
            if (pc.Status.Additions.ContainsKey("ModeChange"))
                pc.Status.cspd = (short)(pcagi * 3 + Math.Floor(Math.Pow((short)((pcagi + 63) / 9.0f), 2)) + 129 +
                                         pc.Status.cspd_skill);
            else
                pc.Status.cspd = (short)(pcdex * 3 + Math.Floor(Math.Pow((short)((pcdex + 63) / 9.0f), 2)) + 129 +
                                         pc.Status.cspd_skill);


            //移动速度
            pc.Speed = (ushort)(Configuration.Configuration.Instance.Speed + pc.Status.speed_item +
                                pc.Status.speed_skill);

            //爪子和双枪的攻速惩罚
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                var item = pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                if (item.BaseData.itemType == ItemType.DUALGUN || item.BaseData.itemType == ItemType.CLAW)
                    pc.Status.aspd = (short)(pc.Status.aspd * 0.70f);
            }

            //修正dex爆表的情况下 cspd变负数导致咏唱时间超标的问题
            pc.Status.aspd = Math.Max((short)1, pc.Status.aspd);
            pc.Status.cspd = Math.Max((short)1, pc.Status.cspd);

            pc.Status.avoid_melee = Math.Min((ushort)500, pc.Status.avoid_melee);
            pc.Status.avoid_ranged = Math.Min((ushort)500, pc.Status.avoid_ranged);

            pc.Status.hit_melee = Math.Min((ushort)500, pc.Status.hit_melee);
            pc.Status.hit_ranged = Math.Min((ushort)500, pc.Status.hit_ranged);

            pc.Status.aspd = Math.Min((short)800, pc.Status.aspd);

            pc.Status.cspd =
                Math.Min((short)(pc.Status.speedenchantcspdbonus > 0 || pc.Status.communioncspdbonus > 0 ? 850 : 800),
                    (short)(pc.Status.cspd + pc.Status.speedenchantcspdbonus + pc.Status.communioncspdbonus));
        }

        public ushort RequiredBonusPoint(ushort current)
        {
            return (ushort)Math.Ceiling((current + 1) / 6.0f);
        }

        public ushort GetTotalBonusPointForStats(ushort start, ushort stat)
        {
            var points = 0;
            for (var i = start; i < stat; i++) points += RequiredBonusPoint(i);
            return (ushort)points;
        }

        private float CalcATKRate(ActorPC pc)
        {
            var ifRanged = false;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                var item = pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND];
                if (item.BaseData.itemType == ItemType.BOW || item.BaseData.itemType == ItemType.GUN ||
                    item.BaseData.itemType == ItemType.DUALGUN ||
                    item.BaseData.itemType == ItemType.RIFLE || item.BaseData.itemType == ItemType.THROW)
                    ifRanged = true;
            }

            if (!ifRanged)
                return 1.0f + (float)((pc.Dex + pc.Status.dex_item + pc.Status.dex_rev + pc.Status.dex_mario +
                                       pc.Status.dex_skill) * 1.5 / 160);

            return 1.0f +
                   (float)((pc.Int + pc.Status.int_item + pc.Status.int_rev + pc.Status.int_mario +
                            pc.Status.int_skill) * 1.5 / 160);
        }

        public void CalcPayV(ActorPC pc)
        {
            CalcPayl(pc);
            CalcVolume(pc);
        }

        private void CalcVolume(ActorPC pc)
        {
            //CAPA = floor[ (floor[DEX/5] + floor[INT/10] + 200)×職業係数×スキルパッキングによる倍率 ]
            var VOLU =
                (uint)((Math.Max(
                            pc.Dex + pc.Status.dex_item + pc.Status.dex_chip + pc.Status.dex_rev + pc.Status.dex_skill,
                            0) / 5.0f +
                        Math.Max(
                            pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev + pc.Status.int_skill,
                            0) / 10.0f + 200)
                       * VolumeJobFactor(pc)
                       * Configuration.Configuration.Instance.VolumeRate * 10);

            if (pc.Status.volume_iris > 0) VOLU += (uint)(VOLU * (pc.Status.volume_iris / 100.0f));
            if (pc.Status.volume_add_iris > 0) VOLU += (uint)pc.Status.volume_add_iris;
            pc.Inventory.MaxVolume[ContainerType.BODY] = VOLU;
            //pc.Inventory.MaxVolume[ContainerType.BODY] = (uint)((float)(2 * pc.JobLevel1 + 4 * pc.JobLevel2X + 4 * pc.JobLevel2T + 6 * pc.JobLevel3 + 300) * VolumeJobFactor(pc) * Configuration.Instance.VolumeRate * 10);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.volumeUp > 0)
                pc.Inventory.MaxVolume[ContainerType.BODY] = (uint)(pc.Inventory.MaxVolume[ContainerType.BODY] +
                                                                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]
                                                                        .BaseData.volumeUp);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.volumeUp > 0)
                pc.Inventory.MaxVolume[ContainerType.BODY] = (uint)(pc.Inventory.MaxVolume[ContainerType.BODY] +
                                                                    pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND]
                                                                        .BaseData.volumeUp);

            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.BACK) &&
                pc.Inventory.Equipments[EnumEquipSlot.BACK].BaseData.volumeUp > 0)
                pc.Inventory.MaxVolume[ContainerType.BODY] = (uint)(pc.Inventory.MaxVolume[ContainerType.BODY] +
                                                                    pc.Inventory.Equipments[EnumEquipSlot.BACK].BaseData
                                                                        .volumeUp);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                pc.Inventory.MaxVolume[ContainerType.BODY] = (uint)(pc.Inventory.MaxVolume[ContainerType.BODY] +
                                                                    pc.Inventory.Equipments[EnumEquipSlot.PET].BaseData
                                                                        .volumeUp);
            if (pc.Account.GMLevel > 50)
                pc.Inventory.MaxVolume[ContainerType.BODY] += 1000;
        }

        private void CalcPayl(ActorPC pc)
        {
            //PAYL = floor[(X + 400)×種族係数×職業係数]
            //ただし X = floor[STR×2 / 3] + floor[VIT / 3]
            //旧公式废除
            //pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)((float)((pc.Str + pc.Status.str_item + pc.Status.str_chip + pc.Status.str_rev + pc.Status.str_skill) * 3 + (2 * pc.JobLevel1 + 4 * pc.JobLevel2X + 4 * pc.JobLevel2T + 6 * pc.JobLevel3 + 350)) *
            //    PayLoadRaceFactor(pc.Race) * Configuration.Instance.PayloadRate * PayLoadJobFactor(pc) * 10);
            var PCPAYL =
                (uint)((Math.Max(
                            pc.Str + pc.Status.str_item + pc.Status.str_chip + pc.Status.str_rev + pc.Status.str_skill,
                            0) * 2.0f / 3.0f +
                        Math.Max(
                            pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev + pc.Status.vit_skill,
                            0) / 3.0f + 400) *
                       Configuration.Configuration.Instance.PayloadRate *
                       PayLoadRaceFactor(pc.Race) * PayLoadJobFactor(pc) * 10);
            if (pc.Status.payl_iris > 0) PCPAYL += (uint)(PCPAYL * (pc.Status.payl_iris / 100.0f));
            if (pc.Status.payl_add_iris > 0) PCPAYL += (uint)pc.Status.payl_add_iris;
            pc.Inventory.MaxPayload[ContainerType.BODY] = PCPAYL;
            if (pc.Status.Additions.ContainsKey("GoRiKi"))
            {
                var skill = (DefaultPassiveSkill)pc.Status.Additions["GoRiKi"];
                pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] *
                                                                     (1f + (float)skill["GoRiKi"] / 100));
            }

            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.weightUp > 0)
                pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] +
                                                                     pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND]
                                                                         .BaseData.weightUp);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND) &&
                pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.weightUp > 0)
                pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] +
                                                                     pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND]
                                                                         .BaseData.weightUp);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.BACK) &&
                pc.Inventory.Equipments[EnumEquipSlot.BACK].BaseData.weightUp > 0)
                pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] +
                                                                     pc.Inventory.Equipments[EnumEquipSlot.BACK]
                                                                         .BaseData.weightUp);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
                pc.Inventory.MaxPayload[ContainerType.BODY] = (uint)(pc.Inventory.MaxPayload[ContainerType.BODY] +
                                                                     pc.Inventory.Equipments[EnumEquipSlot.PET].BaseData
                                                                         .weightUp);
            if (pc.Account.GMLevel > 50)
                pc.Inventory.MaxPayload[ContainerType.BODY] += 1000;
        }

        private float PayLoadRaceFactor(PC_RACE race)
        {
            switch (race)
            {
                case PC_RACE.EMIL:
                    return 1.3f;
                case PC_RACE.TITANIA:
                    return 0.9f;
                case PC_RACE.DOMINION:
                    return 1.1f;
                default:
                    return 1;
            }
        }

        private float PayLoadJobFactor(ActorPC pc)
        {
            PC_JOB job;
            if (pc.JobJoint == PC_JOB.NONE)
                job = pc.Job;
            else
                job = pc.JobJoint;
            switch (job)
            {
                //□初期：ノービス
                case PC_JOB.NOVICE:
                    return 0.7f;
                //□F系：ファイター
                //┣■ソードマン
                //┃┣ ブレイドマスター
                //┃┗ バウンティハンター
                //┣■フェンサー
                //┃┣ ナイト
                //┃┗ ダークストーカー
                //┣■スカウト
                //┃┣ アサシン
                //┃┗ コマンド
                //┣■アーチャー
                //┃┣ ストライカー
                //┃┗ ガンナー
                case PC_JOB.SWORDMAN:
                case PC_JOB.BLADEMASTER:
                case PC_JOB.BOUNTYHUNTER:
                case PC_JOB.GLADIATOR:
                case PC_JOB.FENCER:
                case PC_JOB.KNIGHT:
                case PC_JOB.DARKSTALKER:
                case PC_JOB.GUARDIAN:
                case PC_JOB.SCOUT:
                case PC_JOB.ASSASSIN:
                case PC_JOB.COMMAND:
                case PC_JOB.ERASER:
                case PC_JOB.ARCHER:
                case PC_JOB.STRIKER:
                case PC_JOB.GUNNER:
                case PC_JOB.HAWKEYE:
                    return 1.0f;
                //□SU系：スペルユーザー
                //┣■ウィザード
                //┃┣ ソーサラー
                //┃┗ セージ
                //┣■シャーマン
                //┃┣ エレメンタラー
                //┃┗ エンチャンター
                //┣■ウァテス
                //┃┣ ドルイド
                //┃┗ バード
                //┣■ウォーロック
                //┃┣ カバリスト
                //┃┗ ネクロマンサー
                case PC_JOB.WIZARD:
                case PC_JOB.SORCERER:
                case PC_JOB.SAGE:
                case PC_JOB.FORCEMASTER:
                case PC_JOB.SHAMAN:
                case PC_JOB.ELEMENTER:
                case PC_JOB.ENCHANTER:
                case PC_JOB.ASTRALIST:
                case PC_JOB.VATES:
                case PC_JOB.DRUID:
                case PC_JOB.BARD:
                case PC_JOB.CARDINAL:
                case PC_JOB.WARLOCK:
                case PC_JOB.CABALIST:
                case PC_JOB.NECROMANCER:
                case PC_JOB.SOULTAKER:
                    return 0.8f;
                //□BP系：バックパッカー
                //┣■タタラベ
                //┃┣ ブラックスミス
                //┃┗ マシンナリー
                //┣■ファーマー
                //┃┣ アルケミスト
                //┃┗ マリオネスト
                //┣■レンジャー
                //┃┣ エクスプローラー
                //┃┗ トレジャーハンター
                //┃■マーチャント
                //┃┣ トレーダー
                //┃┗ ギャンブラー
                case PC_JOB.BLACKSMITH:
                case PC_JOB.MACHINERY:
                case PC_JOB.MAESTRO:
                case PC_JOB.ALCHEMIST:
                case PC_JOB.HARVEST:
                case PC_JOB.RANGER:
                case PC_JOB.EXPLORER:
                case PC_JOB.TREASUREHUNTER:
                case PC_JOB.STRIDER:
                case PC_JOB.MERCHANT:
                case PC_JOB.TRADER:
                case PC_JOB.GAMBLER:
                case PC_JOB.ROYALDEALER:
                    return 1.3f;
                default:
                    return 1;
            }
        }

        private float VolumeJobFactor(ActorPC pc)
        {
            PC_JOB job;
            if (pc.JobJoint == PC_JOB.NONE)
                job = pc.Job;
            else
                job = pc.JobJoint;
            switch (job)
            {
                //□初期：ノービス
                case PC_JOB.NOVICE:
                    return 0.85f;
                //□F系：ファイター
                //┣■ソードマン
                //┃┣ ブレイドマスター
                //┃┗ バウンティハンター
                //┣■フェンサー
                //┃┣ ナイト
                //┃┗ ダークストーカー
                //┣■スカウト
                //┃┣ アサシン
                //┃┗ コマンド
                //┣■アーチャー
                //┃┣ ストライカー
                //┃┗ ガンナー
                case PC_JOB.SWORDMAN:
                case PC_JOB.BLADEMASTER:
                case PC_JOB.BOUNTYHUNTER:
                case PC_JOB.GLADIATOR:
                case PC_JOB.FENCER:
                case PC_JOB.KNIGHT:
                case PC_JOB.DARKSTALKER:
                case PC_JOB.GUARDIAN:
                case PC_JOB.SCOUT:
                case PC_JOB.ASSASSIN:
                case PC_JOB.COMMAND:
                case PC_JOB.ERASER:
                case PC_JOB.ARCHER:
                case PC_JOB.STRIKER:
                case PC_JOB.GUNNER:
                case PC_JOB.HAWKEYE:
                    return 1.0f;
                //□SU系：スペルユーザー
                //┣■ウィザード
                //┃┣ ソーサラー
                //┃┗ セージ
                //┣■シャーマン
                //┃┣ エレメンタラー
                //┃┗ エンチャンター
                //┣■ウァテス
                //┃┣ ドルイド
                //┃┗ バード
                //┣■ウォーロック
                //┃┣ カバリスト
                //┃┗ ネクロマンサー
                case PC_JOB.WIZARD:
                case PC_JOB.SORCERER:
                case PC_JOB.SAGE:
                case PC_JOB.FORCEMASTER:
                case PC_JOB.SHAMAN:
                case PC_JOB.ELEMENTER:
                case PC_JOB.ENCHANTER:
                case PC_JOB.ASTRALIST:
                case PC_JOB.VATES:
                case PC_JOB.DRUID:
                case PC_JOB.BARD:
                case PC_JOB.CARDINAL:
                case PC_JOB.WARLOCK:
                case PC_JOB.CABALIST:
                case PC_JOB.NECROMANCER:
                case PC_JOB.SOULTAKER:
                    return 1.0f;
                //□BP系：バックパッカー
                //┣■タタラベ
                //┃┣ ブラックスミス
                //┃┗ マシンナリー
                //┣■ファーマー
                //┃┣ アルケミスト
                //┃┗ マリオネスト
                //┣■レンジャー
                //┃┣ エクスプローラー
                //┃┗ トレジャーハンター
                //┃■マーチャント
                //┃┣ トレーダー
                //┃┗ ギャンブラー
                case PC_JOB.BLACKSMITH:
                case PC_JOB.MACHINERY:
                case PC_JOB.MAESTRO:
                case PC_JOB.ALCHEMIST:
                case PC_JOB.HARVEST:
                case PC_JOB.RANGER:
                case PC_JOB.EXPLORER:
                case PC_JOB.TREASUREHUNTER:
                case PC_JOB.STRIDER:
                case PC_JOB.MERCHANT:
                case PC_JOB.TRADER:
                case PC_JOB.GAMBLER:
                case PC_JOB.ROYALDEALER:
                    return 1.13f;
                default:
                    return 1;
            }
        }

        public void CalcHPMPSP(ActorPC pc)
        {
            pc.MaxHP = CalcMaxHP(pc);
            pc.MaxMP = CalcMaxMP(pc);
            pc.MaxSP = CalcMaxSP(pc);
            pc.MaxEP = CalcMaxEP(pc);
            if (pc.Status.Additions.ContainsKey("ModeChange"))
            {
                var Maxtmp = pc.MaxMP;
                pc.MaxMP = pc.MaxSP;
                pc.MaxSP = Maxtmp;
            }

            if (pc.HP > pc.MaxHP) pc.HP = pc.MaxHP;
            if (pc.MP > pc.MaxMP) pc.MP = pc.MaxMP;
            if (pc.SP > pc.MaxSP) pc.SP = pc.MaxSP;
            if (pc.EP > pc.MaxEP) pc.EP = pc.MaxEP;
        }

        private uint CalcMaxEP(ActorPC pc)
        {
            if (pc.Ring == null)
                return 30;
            return Math.Min((uint)(30 + pc.Ring.MemberCount * 2), 110);
            //return 100;
        }

        private uint CalcMaxHP(ActorPC pc)
        {
            short possession = 0;
            var lv = pc.Level;
            if (pc.Pet != null)
                if (pc.Pet.Ride)
                    if (pc.Pet.MaxHP != 0)
                        return pc.Pet.MaxHP;

            foreach (var i in pc.PossesionedActors)
            {
                if (i == pc) continue;
                if (i.Status == null)
                    continue;
                possession += i.Status.hp_possession;
            }

            var pcvit = checkPositive(pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev +
                                      pc.Status.vit_mario + pc.Status.vit_skill + pc.Status.vit_iris);

            var basehp = (uint)(Math.Floor(pcvit * 3 + Math.Pow((int)Math.Floor(pcvit / 5.0f), 2) + lv * 2 +
                                           Math.Pow((int)Math.Floor(lv / 5.0f), 2) + 50) * HPJobFactor(pc));
            basehp = (uint)Math.Max(1,
                basehp + pc.Status.hp_item + pc.Status.hp_mario + possession + pc.Status.hp_skill + pc.Status.hp_iris);
            var itembonus = pc.Status.hp_rate_item / 100.0f - 1.0f;
            var irisbonus = pc.Status.hp_rate_iris / 100.0f - 1.0f;
            basehp = (uint)(basehp * (1.0f + itembonus + irisbonus));
            return Math.Min(basehp, 70000);
        }

        private uint CalcMaxMP(ActorPC pc)
        {
            short possession = 0;
            byte lv = 0;
            var map = MapManager.Instance.GetMap(pc.MapID);
            lv = pc.Level;
            if (pc.Pet != null)
                if (pc.Pet.Ride)
                    if (pc.Pet.MaxMP != 0)
                        return pc.Pet.MaxMP;

            foreach (var i in pc.PossesionedActors)
            {
                if (i == pc) continue;
                if (i.Status == null)
                    continue;
                possession += i.Status.mp_possession;
            }

            var pcmag = checkPositive(pc.Mag + pc.Status.mag_item + pc.Status.mag_chip + pc.Status.mag_rev +
                                      pc.Status.mag_mario + pc.Status.mag_skill + pc.Status.mag_iris);

            var basemp = (uint)Math.Floor((float)(pcmag * 3 + lv + Math.Pow((float)Math.Floor(lv / 9.0f), 2) + 30) *
                                          MPJobFactor(pc));
            basemp = (uint)Math.Max(1,
                basemp + pc.Status.mp_item + pc.Status.mp_mario + possession + pc.Status.mp_skill + pc.Status.mp_iris);
            var itembonus = pc.Status.mp_rate_item / 100.0f - 1.0f;
            var irisbonus = pc.Status.mp_rate_iris / 100.0f - 1.0f;
            basemp = (uint)(basemp * (1.0f + itembonus + irisbonus));
            return Math.Min(basemp, 40000);
        }

        private uint CalcMaxSP(ActorPC pc)
        {
            short possession = 0;
            byte lv = 0;
            var map = MapManager.Instance.GetMap(pc.MapID);
            lv = pc.Level;
            if (pc.Pet != null)
                if (pc.Pet.Ride)
                    if (pc.Pet.MaxSP != 0)
                        return pc.Pet.MaxSP;

            foreach (var i in pc.PossesionedActors)
            {
                if (i == pc) continue;
                possession += i.Status.sp_possession;
            }

            var pcint = checkPositive(pc.Int + pc.Status.int_item + pc.Status.int_chip + pc.Status.int_rev +
                                      pc.Status.int_mario + pc.Status.int_skill + pc.Status.int_iris);
            var pcvit = checkPositive(pc.Vit + pc.Status.vit_item + pc.Status.vit_chip + pc.Status.vit_rev +
                                      pc.Status.vit_mario + pc.Status.vit_skill + pc.Status.int_iris);

            //最大SP = floor[ (INT + VIT + LV + floor[LV/9]^2 + 20)×SP係数 ] + スキルによる補正 + 装備による補正
            var basesp = (uint)Math.Floor((float)(pcint + pcvit + lv + Math.Pow((int)Math.Floor(lv / 9.0f), 2) + 20) *
                                          SPJobFactor(pc));
            basesp = (uint)Math.Max(1,
                basesp + pc.Status.sp_item + pc.Status.sp_mario + possession + pc.Status.sp_skill + pc.Status.sp_iris);
            var itembonus = pc.Status.sp_rate_item / 100.0f - 1.0f;
            var irisbonus = pc.Status.sp_rate_iris / 100.0f - 1.0f;
            basesp = (uint)(basesp * (1.0f + itembonus + irisbonus));
            return Math.Min(basesp, 40000);
        }

        private float HPJobFactor(ActorPC pc)
        {
            PC_JOB job;
            if (pc.JobJoint == PC_JOB.NONE)
                job = pc.Job;
            else
                job = pc.JobJoint;

            switch (job)
            {
                //1次職
                case PC_JOB.NOVICE:
                    return 1.00f;

                case PC_JOB.SWORDMAN:
                    return 1.80f;

                case PC_JOB.FENCER:
                    return 1.65f;

                case PC_JOB.SCOUT:
                    return 1.45f;

                case PC_JOB.ARCHER:
                    return 1.35f;

                case PC_JOB.WIZARD:
                    return 1.10f;

                case PC_JOB.SHAMAN:
                    return 1.05f;

                case PC_JOB.VATES:
                    return 1.15f;

                case PC_JOB.WARLOCK:
                    return 1.30f;

                case PC_JOB.RANGER:
                    return 1.25f;

                case PC_JOB.MERCHANT:
                    return 1.20f;

                case PC_JOB.TATARABE:
                    return 1.50f;

                case PC_JOB.FARMASIST:
                    return 1.40f;

                //2次職エキスパート
                case PC_JOB.BLADEMASTER:
                    return 3.05f;

                case PC_JOB.KNIGHT:
                    return 3.30f;

                case PC_JOB.ASSASSIN:
                    return 2.45f; //(2.20-2.45)

                case PC_JOB.STRIKER:
                    return 2.30f; //(2.07-2.25)

                case PC_JOB.SORCERER:
                    return 1.85f;

                case PC_JOB.ELEMENTER:
                    return 1.80f;

                case PC_JOB.DRUID:
                    return 1.95f;

                case PC_JOB.CABALIST:
                    return 2.60f;

                case PC_JOB.BREEDER:
                case PC_JOB.GARDNER:
                case PC_JOB.BLACKSMITH:
                    return 3;

                case PC_JOB.ALCHEMIST:
                    return 2.50f;

                case PC_JOB.EXPLORER:
                    return 2.80f;

                case PC_JOB.TRADER:
                    return 2.40f;

                //2次職テクニカル
                case PC_JOB.BOUNTYHUNTER:
                    return 2.90f; //?

                case PC_JOB.DARKSTALKER:
                    return 3;

                case PC_JOB.COMMAND:
                    return 2.50f;

                case PC_JOB.GUNNER:
                    return 2.15f;

                case PC_JOB.SAGE:
                    return 1.95f;

                case PC_JOB.ENCHANTER:
                    return 1.85f; //(1.62-1.85)

                case PC_JOB.BARD:
                    return 2.15f; //(2.00-2.15)

                case PC_JOB.NECROMANCER:
                    return 2.30f;

                case PC_JOB.MACHINERY:
                    return 2.60f;

                case PC_JOB.TREASUREHUNTER:
                    return 2.30f;

                case PC_JOB.GAMBLER:
                    return 2.40f;

                //3转职业补正
                case PC_JOB.GLADIATOR:
                    return 5.17f;
                case PC_JOB.GUARDIAN:
                    return 6.6f;
                case PC_JOB.ERASER:
                    return 4.31f;
                case PC_JOB.HAWKEYE:
                    return 3.92f;
                case PC_JOB.FORCEMASTER:
                    return 3.46f;
                case PC_JOB.ASTRALIST:
                    return 3.26f;
                case PC_JOB.CARDINAL:
                    return 4.02f;
                case PC_JOB.SOULTAKER:
                    return 5.20f;
                case PC_JOB.MAESTRO:
                    return 6f;
                case PC_JOB.HARVEST:
                    return 5f;
                case PC_JOB.STRIDER:
                    return 5.6f;
                case PC_JOB.ROYALDEALER:
                    return 4.8f;
                case PC_JOB.JOKER:
                    return 3f;

                default:
                    return 1;
            }
        }

        private float MPJobFactor(ActorPC pc)
        {
            PC_JOB job;
            if (pc.JobJoint == PC_JOB.NONE)
                job = pc.Job;
            else
                job = pc.JobJoint;

            switch (job)
            {
                //1次職
                case PC_JOB.NOVICE:
                    return 1.00f;

                case PC_JOB.SWORDMAN:
                    return 1.05f;

                case PC_JOB.FENCER:
                    return 1.05f;

                case PC_JOB.SCOUT:
                    return 1.05f;

                case PC_JOB.ARCHER:
                    return 1.10f;

                case PC_JOB.WIZARD:
                    return 1.20f;

                case PC_JOB.SHAMAN:
                    return 1.25f; //?

                case PC_JOB.VATES:
                    return 1.15f;

                case PC_JOB.WARLOCK:
                    return 1.15f;

                case PC_JOB.RANGER:
                    return 1.05f;

                case PC_JOB.MERCHANT:
                    return 1.10f;

                case PC_JOB.FARMASIST:
                    return 1.10f;

                case PC_JOB.TATARABE:
                    return 1.10f;

                //2次職エキスパート
                case PC_JOB.BLADEMASTER:
                    return 1.25f;

                case PC_JOB.KNIGHT:
                    return 1.30f;

                case PC_JOB.ASSASSIN:
                    return 1.30f;

                case PC_JOB.STRIKER:
                    return 1.40f;

                case PC_JOB.SORCERER:
                    return 2.35f;

                case PC_JOB.ELEMENTER:
                    return 2.40f;

                case PC_JOB.DRUID:
                    return 2.20f;

                case PC_JOB.CABALIST:
                    return 2;

                case PC_JOB.BREEDER:
                case PC_JOB.GARDNER:
                case PC_JOB.BLACKSMITH:
                    return 1.20f;

                case PC_JOB.ALCHEMIST:
                    return 1.50f;

                case PC_JOB.EXPLORER:
                    return 1.30f;

                case PC_JOB.TRADER:
                    return 1.30f;

                //2次職テクニカル
                case PC_JOB.BOUNTYHUNTER:
                    return 1.25f; //?

                case PC_JOB.DARKSTALKER:
                    return 1.25f;

                case PC_JOB.COMMAND:
                    return 1.25f; //?

                case PC_JOB.GUNNER:
                    return 1.25f;

                case PC_JOB.SAGE:
                    return 2.30f;

                case PC_JOB.ENCHANTER:
                    return 2.35f;

                case PC_JOB.BARD:
                    return 2.10f;

                case PC_JOB.NECROMANCER:
                    return 2.30f;

                case PC_JOB.MACHINERY:
                    return 1.50f;

                case PC_JOB.TREASUREHUNTER:
                    return 1.50f;

                case PC_JOB.GAMBLER:
                    return 1.90f;

                //3转职业补正
                case PC_JOB.GLADIATOR:
                    return 1.49f;
                case PC_JOB.GUARDIAN:
                    return 1.61f;
                case PC_JOB.ERASER:
                    return 1.61f;
                case PC_JOB.HAWKEYE:
                    return 1.78f;
                case PC_JOB.FORCEMASTER:
                    return 4.60f;
                case PC_JOB.ASTRALIST:
                    return 4.61f;
                case PC_JOB.CARDINAL:
                    return 4.21f;
                case PC_JOB.SOULTAKER:
                    return 4.60f;
                case PC_JOB.MAESTRO:
                    return 2.14f;
                case PC_JOB.HARVEST:
                    return 4.20f;
                case PC_JOB.STRIDER:
                    return 2.05f;
                case PC_JOB.ROYALDEALER:
                    return 3.28f;
                case PC_JOB.JOKER:
                    return 3f;

                default:
                    return 1;
            }
        }

        private float SPJobFactor(ActorPC pc)
        {
            PC_JOB job;
            if (pc.JobJoint == PC_JOB.NONE)
                job = pc.Job;
            else
                job = pc.JobJoint;
            switch (job)
            {
                //1次職
                case PC_JOB.NOVICE:
                    return 1.00f;

                case PC_JOB.SWORDMAN:
                    return 1.10f;

                case PC_JOB.FENCER:
                    return 1.15f;

                case PC_JOB.SCOUT:
                    return 1.20f;

                case PC_JOB.ARCHER:
                    return 1.15f;

                case PC_JOB.WIZARD:
                    return 1.05f;

                case PC_JOB.SHAMAN:
                    return 1.10f;

                case PC_JOB.VATES:
                    return 1.10f;

                case PC_JOB.WARLOCK:
                    return 1.10f;

                case PC_JOB.RANGER:
                    return 1.15f;

                case PC_JOB.MERCHANT:
                    return 1.10f;

                case PC_JOB.FARMASIST:
                    return 1.10f;

                case PC_JOB.TATARABE:
                    return 1.15f;

                //2次職エキスパート
                case PC_JOB.BLADEMASTER:
                    return 1.75f;

                case PC_JOB.KNIGHT:
                    return 1.50f;

                case PC_JOB.ASSASSIN:
                    return 1.70f;

                case PC_JOB.STRIKER:
                    return 1.60f;

                case PC_JOB.SORCERER:
                    return 1.25f;

                case PC_JOB.ELEMENTER:
                    return 1.20f;

                case PC_JOB.DRUID:
                    return 1.35f;

                case PC_JOB.CABALIST:
                    return 1.40f;

                case PC_JOB.BREEDER:
                case PC_JOB.GARDNER:
                case PC_JOB.BLACKSMITH:
                    return 1.60f;

                case PC_JOB.ALCHEMIST:
                    return 1.80f;

                case PC_JOB.EXPLORER:
                    return 1.85f;

                case PC_JOB.TRADER:
                    return 1.90f;

                //2次職テクニカル
                case PC_JOB.BOUNTYHUNTER:
                    return 1.80f; //?

                case PC_JOB.DARKSTALKER:
                    return 1.70f;

                case PC_JOB.COMMAND:
                    return 1.80f; //?

                case PC_JOB.GUNNER:
                    return 2.30f; //?

                case PC_JOB.SAGE:
                    return 1.25f; //?

                case PC_JOB.ENCHANTER:
                    return 1.25f;

                case PC_JOB.BARD:
                    return 1.25f;

                case PC_JOB.NECROMANCER:
                    return 1.35f; //?

                case PC_JOB.MACHINERY:
                    return 1.90f;

                case PC_JOB.TREASUREHUNTER:
                    return 2.10f;

                case PC_JOB.GAMBLER:
                    return 1.70f;

                //3转职业补正
                case PC_JOB.GLADIATOR:
                    return 2.95f;
                case PC_JOB.GUARDIAN:
                    return 2.82f;
                case PC_JOB.ERASER:
                    return 3.85f;
                case PC_JOB.HAWKEYE:
                    return 4.6f;
                case PC_JOB.FORCEMASTER:
                    return 1.49f;
                case PC_JOB.ASTRALIST:
                    return 1.56f;
                case PC_JOB.CARDINAL:
                    return 1.66f;
                case PC_JOB.SOULTAKER:
                    return 2.05f;
                case PC_JOB.MAESTRO:
                    return 3.14f;
                case PC_JOB.HARVEST:
                    return 2.82f;
                case PC_JOB.STRIDER:
                    return 4.01f;
                case PC_JOB.ROYALDEALER:
                    return 3.28f;
                case PC_JOB.JOKER:
                    return 3f;

                default:
                    return 1;
            }
        }

        private void CalcStatsRev(ActorPC pc)
        {
            if (pc.JobJoint == PC_JOB.NONE)
            {
                byte joblv1 = 0;
                byte joblv2x = 0;
                byte joblv2t = 0;
                byte joblv3 = 0;
                var map = MapManager.Instance.GetMap(pc.MapID);
                joblv1 = pc.JobLevel1;
                joblv2x = pc.JobLevel2X;
                joblv2t = pc.JobLevel2T;
                joblv3 = pc.JobLevel3;

                switch (pc.Job)
                {
                    case PC_JOB.NOVICE:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.07f);
                        break;
                    case PC_JOB.SWORDMAN:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.26f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.03f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.19f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.FENCER:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.21f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.18f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.05f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.18f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.16f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.SCOUT:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.20f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.20f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.05f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.26f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.ARCHER:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.12f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.26f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.10f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.WIZARD:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.02f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.10f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.21f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.07f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.10f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.30f);
                        break;
                    case PC_JOB.SHAMAN:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.02f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.13f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.18f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.06f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.13f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.28f);
                        break;
                    case PC_JOB.VATES:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.05f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.11f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.25f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.02f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.11f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.26f);
                        break;
                    case PC_JOB.WARLOCK:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.14f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.13f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.08f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.12f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.18f);
                        break;
                    case PC_JOB.RANGER:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.10f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.24f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.13f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.08f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.22f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.03f);
                        break;
                    case PC_JOB.MERCHANT:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.12f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.21f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.25f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.05f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.15f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.TATARABE:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.14f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.24f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.06f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.18f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.14f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    case PC_JOB.FARMASIST:
                        pc.Status.str_rev = (ushort)(joblv1 * 0.14f);
                        pc.Status.dex_rev = (ushort)(joblv1 * 0.18f);
                        pc.Status.int_rev = (ushort)(joblv1 * 0.16f);
                        pc.Status.vit_rev = (ushort)(joblv1 * 0.20f);
                        pc.Status.agi_rev = (ushort)(joblv1 * 0.08f);
                        pc.Status.mag_rev = (ushort)(joblv1 * 0.02f);
                        break;
                    // 2次エキスパート職補正値 = FLOOR((JobLv + 30) * 補正率)
                    case PC_JOB.BLADEMASTER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.30f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.24f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.04f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.20f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.02f);
                        break;
                    case PC_JOB.KNIGHT:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.17f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.20f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.08f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.32f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.09f);
                        break;
                    case PC_JOB.ASSASSIN:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.21f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.22f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.11f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.30f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.02f);
                        break;
                    case PC_JOB.STRIKER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.21f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.30f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.21f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.02f);
                        break;
                    case PC_JOB.SORCERER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.03f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.16f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.24f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.15f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.30f);
                        break;
                    case PC_JOB.ELEMENTER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.03f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.25f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.20f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.08f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.15f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.29f);
                        break;
                    case PC_JOB.DRUID:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.16f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.26f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.06f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.28f);
                        break;
                    case PC_JOB.CABALIST:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.21f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.17f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.09f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.25f);
                        break;
                    case PC_JOB.BLACKSMITH:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.19f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.28f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.23f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.06f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.12f);
                        break;
                    case PC_JOB.ALCHEMIST:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.16f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.25f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.24f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.21f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.08f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.06f);
                        break;
                    case PC_JOB.EXPLORER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.14f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.27f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.16f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.26f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.05f);
                        break;
                    case PC_JOB.TRADER:
                        pc.Status.str_rev = (ushort)((joblv2x + 30) * 0.15f);
                        pc.Status.dex_rev = (ushort)((joblv2x + 30) * 0.26f);
                        pc.Status.int_rev = (ushort)((joblv2x + 30) * 0.30f);
                        pc.Status.vit_rev = (ushort)((joblv2x + 30) * 0.06f);
                        pc.Status.agi_rev = (ushort)((joblv2x + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv2x + 30) * 0.03f);
                        break;
                    //2次テクニカル職
                    case PC_JOB.BOUNTYHUNTER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.23f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.23f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.12f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.15f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.25f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.02f);
                        break;
                    case PC_JOB.DARKSTALKER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.18f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.23f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.08f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.24f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.17f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.10f);
                        break;
                    case PC_JOB.COMMAND:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.22f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.24f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.23f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.03f);
                        break;
                    case PC_JOB.GUNNER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.20f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.30f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.06f);
                        break;
                    case PC_JOB.SAGE:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.06f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.24f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.24f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.06f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.14f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.26f);
                        break;
                    case PC_JOB.ENCHANTER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.05f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.27f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.25f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.06f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.10f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.27f);
                        break;
                    case PC_JOB.BARD:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.08f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.20f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.22f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.10f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.20f);
                        break;
                    case PC_JOB.NECROMANCER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.14f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.13f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.18f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.06f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.22f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.27f);
                        break;
                    case PC_JOB.MACHINERY:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.23f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.21f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.04f);
                        break;
                        ;
                    case PC_JOB.TREASUREHUNTER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.08f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.16f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.25f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.27f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.08f);
                        break;
                    case PC_JOB.GAMBLER:
                        pc.Status.str_rev = (ushort)((joblv2t + 30) * 0.10f);
                        pc.Status.dex_rev = (ushort)((joblv2t + 30) * 0.20f);
                        pc.Status.int_rev = (ushort)((joblv2t + 30) * 0.21f);
                        pc.Status.vit_rev = (ushort)((joblv2t + 30) * 0.14f);
                        pc.Status.agi_rev = (ushort)((joblv2t + 30) * 0.26f);
                        pc.Status.mag_rev = (ushort)((joblv2t + 30) * 0.09f);
                        break;
                    //3转职业属性补正
                    case PC_JOB.GLADIATOR:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.36f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.27f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.121f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.23f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.3f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.0204f);
                        break;
                    case PC_JOB.GUARDIAN:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.27f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.26f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.08f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.38f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.21f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.10f);
                        break;
                    case PC_JOB.ERASER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.33f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.28f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.16f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.14f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.36f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.03f);
                        break;
                    case PC_JOB.HAWKEYE:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.30f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.16f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.37f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.13f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.28f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.06f);
                        break;
                    case PC_JOB.FORCEMASTER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.06f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.30f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.27f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.15f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.16f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.36f);
                        break;
                    case PC_JOB.ASTRALIST:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.05f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.33f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.28f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.18f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.34f);
                        break;
                    case PC_JOB.CARDINAL:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.12f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.28f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.26f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.12f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.32f);
                        break;
                    case PC_JOB.SOULTAKER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.23f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.19f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.21f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.15f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.22f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.30f);
                        break;
                    case PC_JOB.MAESTRO:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.25f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.29f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.21f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.23f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.12f);
                        break;
                    case PC_JOB.HARVEST:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.18f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.25f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.24f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.22f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.21f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.20f);
                        break;
                    case PC_JOB.STRIDER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.21f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.33f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.16f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.25f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.27f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.08f);
                        break;
                    case PC_JOB.ROYALDEALER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.23f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.26f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.30f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.15f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.27f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.09f);
                        break;
                    case PC_JOB.JOKER:
                        pc.Status.str_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.dex_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.int_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.vit_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.agi_rev = (ushort)((joblv3 + 30) * 0.20f);
                        pc.Status.mag_rev = (ushort)((joblv3 + 30) * 0.20f);
                        break;
                }
            }
            else
            {
                switch (pc.JobJoint)
                {
                    case PC_JOB.BREEDER:
                        pc.Status.str_rev = (ushort)(3 + (pc.JointJobLevel + 3) * 0.143);
                        pc.Status.dex_rev = (ushort)(6 + (pc.JointJobLevel + 1) * 0.25);
                        pc.Status.int_rev = (ushort)(1 + pc.JointJobLevel * 0.04);
                        pc.Status.vit_rev = (ushort)(6 + (pc.JointJobLevel + 1) * 0.25);
                        pc.Status.agi_rev = (ushort)(7 + pc.JointJobLevel * 0.28);
                        pc.Status.mag_rev = (ushort)(1 + pc.JointJobLevel * 0.04);
                        break;
                    case PC_JOB.GARDNER:
                        pc.Status.str_rev = (ushort)(3 + (pc.JointJobLevel + 2) * 0.125);
                        pc.Status.dex_rev = (ushort)(6 + (pc.JointJobLevel + 1) * 0.25);
                        pc.Status.int_rev = (ushort)(6 + (pc.JointJobLevel + 1) * 0.25);
                        pc.Status.vit_rev = (ushort)(5 + (pc.JointJobLevel + 2) * 0.25);
                        pc.Status.agi_rev = (ushort)(3 + (pc.JointJobLevel - 1) * 0.125);
                        pc.Status.mag_rev = (ushort)(pc.JointJobLevel * 0.04);
                        break;
                }
            }
        }

        private void CalcMarionetteBonus(ActorPC pc)
        {
        }
    }
}