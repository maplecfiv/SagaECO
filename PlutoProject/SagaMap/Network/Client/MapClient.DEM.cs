using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.DEMIC;
using SagaDB.Item;
using SagaDB.Map;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;
using SagaMap.Skill;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public bool chipShop;
        private uint currentChipCategory;
        public bool demCLBuy;

        public bool demic;
        public bool demParts;

        public void SendCL()
        {
            if (Character.Race == PC_RACE.DEM && state != SESSION_STATE.AUTHENTIFICATED)
            {
                var p1 = new SSMG_DEM_COST_LIMIT_UPDATE();
                p1.Result = 0;
                p1.CurrentEP = Character.EPUsed;
                p1.EPRequired = (short)(ExperienceManager.Instance.GetEPRequired(Character) - Character.EPUsed);
                p1.CL = Character.CL;
                netIO.SendPacket(p1);
            }
        }

        public void OnDEMCostLimitBuy(CSMG_DEM_COST_LIMIT_BUY p)
        {
            if (demCLBuy)
            {
                var ep = p.EP;
                var p1 = new SSMG_DEM_COST_LIMIT_UPDATE();
                if (Character.EP >= ep)
                {
                    Character.EP = (uint)(Character.EP - ep);
                    ExperienceManager.Instance.ApplyEP(Character, ep);
                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();
                    p1.Result = SSMG_DEM_COST_LIMIT_UPDATE.Results.OK;
                }
                else
                {
                    p1.Result = SSMG_DEM_COST_LIMIT_UPDATE.Results.NOT_ENOUGH_EP;
                }

                p1.CurrentEP = Character.EPUsed;
                p1.EPRequired = (short)(ExperienceManager.Instance.GetEPRequired(Character) - Character.EPUsed);
                p1.CL = Character.CL;
                netIO.SendPacket(p1);
            }
        }

        public void OnDEMCostLimitClose(CSMG_DEM_COST_LIMIT_CLOSE p)
        {
            demCLBuy = false;
        }

        public void OnDEMFormChange(CSMG_DEM_FORM_CHANGE p)
        {
            if (Character.Form != p.Form)
            {
                Character.Form = p.Form;

                SkillHandler.Instance.CastPassiveSkills(Character);
                StatusFactory.Instance.CalcStatus(Character);

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, Character, true);
                SendPlayerInfo();
                SendAttackType();

                var p1 = new SSMG_DEM_FORM_CHANGE();
                p1.Form = Character.Form;
                netIO.SendPacket(p1);
            }
        }

        public void OnDEMPartsUnequip(CSMG_DEM_PARTS_UNEQUIP p)
        {
            if (Character.Race == PC_RACE.DEM && demParts)
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null) return;
                var ifUnequip = Character.Inventory.IsContainerParts(Character.Inventory.GetContainerType(item.Slot));
                if (ifUnequip)
                {
                    var slots = item.EquipSlot;
                    if (slots.Count > 1)
                        for (var i = 1; i < slots.Count; i++)
                            Character.Inventory.Parts.Remove(slots[i]);

                    SSMG_ITEM_DELETE p2;
                    SSMG_ITEM_ADD p3;
                    var slot = item.Slot;

                    if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot,
                            ContainerType.BODY, 1))
                    {
                        if (item.Stack == 0)
                        {
                            if (slot == Character.Inventory.LastItem.Slot)
                            {
                                var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                                p1.InventorySlot = item.Slot;
                                p1.Target = ContainerType.BODY;
                                netIO.SendPacket(p1);
                                var p4 = new SSMG_ITEM_EQUIP();
                                p4.InventorySlot = 0xffffffff;
                                p4.Target = ContainerType.NONE;
                                p4.Result = 3;
                                StatusFactory.Instance.CalcRange(Character);
                                if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                                {
                                    SendAttackType();
                                    SkillHandler.Instance.CastPassiveSkills(Character);
                                }

                                p4.Range = Character.Range;
                                netIO.SendPacket(p4);
                                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character,
                                    true);

                                StatusFactory.Instance.CalcStatus(Character);
                                SendPlayerInfo();
                            }
                            else
                            {
                                p2 = new SSMG_ITEM_DELETE();
                                p2.InventorySlot = slot;
                                netIO.SendPacket(p2);
                                if (slot != item.Slot)
                                {
                                    item = Character.Inventory.GetItem(item.Slot);
                                    var p5 = new SSMG_ITEM_COUNT_UPDATE();
                                    p5.InventorySlot = item.Slot;
                                    p5.Stack = item.Stack;
                                    netIO.SendPacket(p5);
                                    item = Character.Inventory.LastItem;
                                    p3 = new SSMG_ITEM_ADD();
                                    p3.Container = ContainerType.BODY;
                                    p3.InventorySlot = item.Slot;
                                    p3.Item = item;
                                    netIO.SendPacket(p3);
                                }
                                else
                                {
                                    item = Character.Inventory.LastItem;
                                    var p4 = new SSMG_ITEM_COUNT_UPDATE();
                                    p4.InventorySlot = item.Slot;
                                    p4.Stack = item.Stack;
                                    netIO.SendPacket(p4);
                                }
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_ITEM_COUNT_UPDATE();
                            p1.InventorySlot = item.Slot;
                            p1.Stack = item.Stack;
                            netIO.SendPacket(p1);
                            if (Character.Inventory.LastItem.Stack == 1)
                            {
                                p3 = new SSMG_ITEM_ADD();
                                p3.Container = ContainerType.BODY;
                                p3.InventorySlot = Character.Inventory.LastItem.Slot;
                                p3.Item = Character.Inventory.LastItem;
                                netIO.SendPacket(p3);
                            }
                            else
                            {
                                item = Character.Inventory.LastItem;
                                var p4 = new SSMG_ITEM_COUNT_UPDATE();
                                p4.InventorySlot = item.Slot;
                                p4.Stack = item.Stack;
                                netIO.SendPacket(p4);
                            }
                        }
                    }

                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnDEMPartsEquip(CSMG_DEM_PARTS_EQUIP p)
        {
            if (Character.Race == PC_RACE.DEM && demParts)
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null) return;
                var result = CheckEquipRequirement(item);
                if (result < 0)
                {
                    var p4 = new SSMG_ITEM_EQUIP();
                    p4.InventorySlot = 0xffffffff;
                    p4.Target = ContainerType.NONE;
                    p4.Result = result;
                    p4.Range = Character.Range;
                    netIO.SendPacket(p4);
                    return;
                }

                foreach (var i in item.EquipSlot)
                    if (Character.Inventory.Parts.ContainsKey(i))
                    {
                        var oriItem = Character.Inventory.Parts[i];

                        foreach (var j in oriItem.EquipSlot)
                        {
                            if (!Character.Inventory.Parts.ContainsKey(j))
                                continue;
                            var dummyItem = Character.Inventory.Parts[j];
                            if (dummyItem.Stack == 0)
                            {
                                Character.Inventory.Parts.Remove(j);
                                continue;
                            }

                            var container = (ContainerType)Enum.Parse(typeof(ContainerType), j.ToString()) + 200;
                            if (Character.Inventory.MoveItem(container, (int)dummyItem.Slot, ContainerType.BODY,
                                    dummyItem.Stack))
                            {
                                var p1 = new SSMG_ITEM_CONTAINER_CHANGE();
                                p1.InventorySlot = dummyItem.Slot;
                                p1.Target = ContainerType.BODY;
                                netIO.SendPacket(p1);
                                var p4 = new SSMG_ITEM_EQUIP();
                                p4.InventorySlot = 0xffffffff;
                                p4.Target = ContainerType.NONE;
                                p4.Result = 1;
                                p4.Range = Character.Range;
                                netIO.SendPacket(p4);

                                StatusFactory.Instance.CalcStatus(Character);
                                SendPlayerInfo();
                            }
                        }
                    }

                var count = item.Stack;
                if (count == 0) return;

                var dst = (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString()) + 200;
                if (Character.Inventory.MoveItem(Character.Inventory.GetContainerType(item.Slot), (int)item.Slot, dst,
                        count))
                {
                    if (item.Stack == 0)
                    {
                        var p4 = new SSMG_ITEM_EQUIP();
                        dst = (ContainerType)Enum.Parse(typeof(ContainerType), item.EquipSlot[0].ToString());
                        p4.Target = dst;
                        p4.Result = 2;
                        p4.InventorySlot = item.Slot;
                        StatusFactory.Instance.CalcRange(Character);
                        p4.Range = Character.Range;
                        netIO.SendPacket(p4);
                    }
                    else
                    {
                        var p5 = new SSMG_ITEM_COUNT_UPDATE();
                        p5.InventorySlot = item.Slot;
                        p5.Stack = item.Stack;
                        netIO.SendPacket(p5);
                    }
                }

                var slots = item.EquipSlot;
                if (slots.Count > 1)
                    for (var i = 1; i < slots.Count; i++)
                    {
                        var dummy = item.Clone();
                        dummy.Stack = 0;
                        dst = (ContainerType)Enum.Parse(typeof(ContainerType), slots[i].ToString()) + 200;
                        Character.Inventory.AddItem(dst, dummy);
                    }

                if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                {
                    SendAttackType();
                    SkillHandler.Instance.CastPassiveSkills(Character);
                }

                //SkillHandler.Instance.CheckBuffValid(this.Character);

                StatusFactory.Instance.CalcStatus(Character);
                SendPlayerInfo();

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_EQUIP, null, Character, true);
            }
        }

        public void OnDEMPartsClose(CSMG_DEM_PARTS_CLOSE p)
        {
            demParts = false;
        }

        public void OnDEMDemicInitialize(CSMG_DEM_DEMIC_INITIALIZE p)
        {
            if (demic)
            {
                var page = p.Page;
                DEMICPanel panel = null;
                bool[,] table = null;
                if (map.Info.Flag.Test(MapFlags.Dominion))
                {
                    if (Character.Inventory.DominionDemicChips.ContainsKey(page))
                    {
                        panel = Character.Inventory.DominionDemicChips[page];
                        table = Character.Inventory.validTable(page, true);
                    }
                }
                else
                {
                    if (Character.Inventory.DemicChips.ContainsKey(page))
                    {
                        panel = Character.Inventory.DemicChips[page];
                        table = Character.Inventory.validTable(page, false);
                    }
                }

                var p1 = new SSMG_DEM_DEMIC_INITIALIZED();
                p1.Page = page;

                if (panel != null)
                {
                    if (Character.EP > 0)
                    {
                        Character.EP--;
                        foreach (var i in panel.Chips)
                        {
                            if (i.Data.skill1 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill1))
                                {
                                    if (Character.Skills[i.Data.skill1].Level > 1)
                                        Character.Skills[i.Data.skill1].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill1);
                                }

                            if (i.Data.skill2 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill2))
                                {
                                    if (Character.Skills[i.Data.skill2].Level > 1)
                                        Character.Skills[i.Data.skill2].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill2);
                                }

                            if (i.Data.skill3 != 0)
                                if (Character.Skills.ContainsKey(i.Data.skill3))
                                {
                                    if (Character.Skills[i.Data.skill3].Level > 1)
                                        Character.Skills[i.Data.skill3].Level--;
                                    else
                                        Character.Skills.Remove(i.Data.skill3);
                                }

                            AddItem(ItemFactory.Instance.GetItem(i.ItemID), true);
                        }

                        panel.Chips.Clear();
                        int engageTask;
                        var term = Global.Random.Next(0, 99);
                        if (term <= 10)
                            engageTask = 2;
                        else if (term <= 40)
                            engageTask = 1;
                        else
                            engageTask = 0;
                        panel.EngageTask1 = 255;
                        panel.EngageTask2 = 255;
                        for (var i = 0; i < engageTask; i++)
                        {
                            var valid = new List<byte[]>();
                            for (var j = 0; j < 9; j++)
                            for (var k = 0; k < 9; k++)
                                if (table[k, j])
                                    valid.Add(new[] { (byte)k, (byte)j });

                            var coord = valid[Global.Random.Next(0, valid.Count - 1)];
                            var task = (byte)(coord[0] + coord[1] * 9);
                            if (i == 0)
                                panel.EngageTask1 = task;
                            else
                                panel.EngageTask2 = task;
                        }

                        SendActorHPMPSP(Character);

                        p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.OK;
                        p1.EngageTask = panel.EngageTask1;
                        p1.EngageTask2 = panel.EngageTask2;

                        StatusFactory.Instance.CalcStatus(Character);
                        SendPlayerInfo();
                    }
                    else
                    {
                        p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.NOT_ENOUGH_EP;
                    }
                }
                else
                {
                    p1.Result = SSMG_DEM_DEMIC_INITIALIZED.Results.FAILED;
                }

                netIO.SendPacket(p1);
            }
        }

        public void OnDEMDemicConfirm(CSMG_DEM_DEMIC_CONFIRM p)
        {
            if (demic)
            {
                var chips = p.Chips;
                var page = p.Page;
                for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                {
                    var chipID = chips[j, i];
                    if (ChipFactory.Instance.ByChipID.ContainsKey(chipID))
                    {
                        var chip = new Chip(ChipFactory.Instance.ByChipID[chipID]);
                        if (CountItem(chip.ItemID) > 0)
                        {
                            chip.X = (byte)j;
                            chip.Y = (byte)i;
                            if (Character.Inventory.InsertChip(page, chip)) DeleteItemID(chip.ItemID, 1, true);
                        }
                    }
                }

                var p1 = new SSMG_DEM_DEMIC_CONFIRM_RESULT();
                p1.Page = page;
                p1.Result = SSMG_DEM_DEMIC_CONFIRM_RESULT.Results.OK;
                netIO.SendPacket(p1);

                StatusFactory.Instance.CalcStatus(Character);
                SkillHandler.Instance.CastPassiveSkills(Character);
                SendPlayerInfo();
            }
        }

        public void OnDEMDemicClose(CSMG_DEM_DEMIC_CLOSE p)
        {
            demic = false;
        }

        public void OnDEMStatsPreCalc(CSMG_DEM_STATS_PRE_CALC p)
        {
            //backup
            ushort str, dex, intel, agi, vit, mag;
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

            {
                var p1 = new SSMG_PLAYER_STATS_PRE_CALC();
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

                netIO.SendPacket(p1);
            }
            {
                var p1 = new SSMG_DEM_STATS_PRE_CALC();
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

                netIO.SendPacket(p1);
            }

            //resotre
            Character.Str = str;
            Character.Dex = dex;
            Character.Int = intel;
            Character.Agi = agi;
            Character.Vit = vit;
            Character.Mag = mag;

            StatusFactory.Instance.CalcStatus(Character);
        }

        public void OnDEMChipCategory(CSMG_DEM_CHIP_CATEGORY p)
        {
            if (chipShop)
                if (ChipShopFactory.Instance.Items.ContainsKey(p.Category))
                {
                    currentChipCategory = p.Category;
                    var category = ChipShopFactory.Instance.Items[p.Category];
                    var p1 = new SSMG_DEM_CHIP_SHOP_HEADER();
                    p1.CategoryID = p.Category;
                    netIO.SendPacket(p1);

                    foreach (var i in category.Items.Values)
                    {
                        var p2 = new SSMG_DEM_CHIP_SHOP_DATA();
                        p2.EXP = (uint)i.EXP;
                        p2.JEXP = (uint)i.JEXP;
                        p2.ItemID = i.ItemID;
                        p2.Description = i.Description;
                        netIO.SendPacket(p2);
                    }

                    var p3 = new SSMG_DEM_CHIP_SHOP_FOOTER();
                    netIO.SendPacket(p3);
                }
        }

        public void OnDEMChipClose(CSMG_DEM_CHIP_CLOSE p)
        {
            chipShop = false;
        }

        public void OnDEMChipBuy(CSMG_DEM_CHIP_BUY p)
        {
            if (chipShop)
            {
                var items = p.ItemIDs;
                var counts = p.Counts;

                for (var i = 0; i < items.Length; i++)
                {
                    var cat = from item in ChipShopFactory.Instance.Items.Values
                        where item.Items.ContainsKey(items[i])
                        select item;

                    if (cat.Count() > 0)
                    {
                        var category = cat.First();
                        if (counts[i] > 0)
                        {
                            var chip = category.Items[items[i]];
                            if (Character.CEXP > chip.EXP * (ulong)counts[i] &&
                                Character.JEXP > chip.JEXP * (ulong)counts[i])
                            {
                                Character.CEXP -= (uint)(chip.EXP * (ulong)counts[i]);
                                Character.JEXP -= (uint)(chip.JEXP * (ulong)counts[i]);
                                var item = ItemFactory.Instance.GetItem(items[i]);
                                item.Stack = (ushort)counts[i];
                                AddItem(item, true);
                            }
                        }
                    }

                    SendEXP();
                }
            }
        }
    }
}