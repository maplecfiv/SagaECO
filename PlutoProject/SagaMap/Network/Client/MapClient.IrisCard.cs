using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Iris;
using SagaDB.Item;
using SagaLib;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public bool irisAddSlot;
        private uint irisAddSlotItem;
        private uint irisAddSlotMaterial;
        public bool irisCardAssemble;
        private uint irisCardItem;
        public bool irisGacha;


        public void OnIrisGachaCancel(CSMG_IRIS_GACHA_CANCEL p)
        {
            irisGacha = false;
        }


        private DrawType GetDrawTypeFromItem(uint itemID)
        {
            var drawtype = DrawType.Random;
            switch (itemID)
            {
                case 10067300:
                case 10067310:
                case 10067320:
                case 16003300:
                case 16003310:
                case 16003313:
                    break;
                case 10067301:
                    drawtype = DrawType.NomalOnly;
                    break;
                case 10067302:
                    drawtype = DrawType.UnCommonOnly;
                    break;
                case 10067303:
                    drawtype = DrawType.RarityOnly;
                    break;
                case 10067304:
                    drawtype = DrawType.SuperRarityOnly;
                    break;
                case 16003311:
                    drawtype = DrawType.AtleastOneSuperRarity;
                    break;
                default:
                    drawtype = DrawType.Random;
                    break;
            }

            return drawtype;
        }

        public void OnIrisGacha(CSMG_IRIS_GACHA_DRAW p)
        {
            var itemID = p.ItemID;

            var key = string.Format("{0},{1},{2}", p.PayFlag, p.SessionID, p.ItemID);

            if (CountItem(itemID) > 0)
                if (IrisGachaFactory.Instance.IrisGacha.ContainsKey(key))
                {
                    //根据使用的抽卡道具获取抽卡方式
                    var drawType = GetDrawTypeFromItem(itemID);

                    var gacha = IrisGachaFactory.Instance.IrisGacha[key];
                    var cards = new Dictionary<uint, byte>();
                    DeleteItemID(itemID, 1, true);

                    //这里获取本页所有的18张卡片
                    var selectedcards = IrisCardFactory.Instance.Items.Values.Where(x => x.Page == gacha.PageID)
                        .ToList();

                    //加入字典? 意义不明
                    foreach (var item in selectedcards)
                        cards.Add(item.ID, (byte)item.Rarity);

                    //声明结果对象
                    var results = new List<uint>();

                    var retitems = new List<Item>();


                    IrisDrawRate drawrate = null;

                    if (IrisDrawRateFactory.Instance.DrawRate.ContainsKey(key))
                        drawrate = IrisDrawRateFactory.Instance.DrawRate[key];

                    //先把所有的卡片抽出来
                    for (var i = 0; i < gacha.Count; i++)
                    {
                        var lottery = Global.Random.Next(0, 1000);
                        var Lcards = new List<uint>();
                        byte rank = 1;

                        if (lottery < (drawrate != null ? drawrate.SuperRatityRate : 5)) rank = 4;
                        else if (lottery < (drawrate != null ? drawrate.RatityRate : 55)) rank = 3;
                        else if (lottery < (drawrate != null ? drawrate.UnCommonRate : 185)) rank = 2;
                        else rank = 1;


                        while (cards.Count(x => x.Value == rank) == 0)
                            if (rank - 1 > 0)
                                rank -= 1;
                            else
                                rank = 4;

                        //不存在保底
                        //if (i == 9)
                        //{
                        //    rank = 2;//保底变R
                        //    if (lottery < 50) rank = 3;
                        //    if (lottery < 8) rank = 4;
                        //}
                        foreach (var i2 in cards)
                            if (i2.Value == rank)
                                Lcards.Add(i2.Key);

                        uint itemid = 0;

                        if (Lcards.Count == 1)
                            itemid = Lcards[0];
                        else
                            itemid = Lcards[Global.Random.Next(0, Lcards.Count - 1)];

                        var item = ItemFactory.Instance.GetItem(itemid);
                        item.Stack = 1;
                        item.Identified = true;
                        retitems.Add(item);
                    }

                    //这里根据drawtype 对已经抽到的10张卡片进行加工
                    var idx = 0;
                    switch (drawType)
                    {
                        case DrawType.Random:
                            break;
                        case DrawType.NomalOnly:
                            var uncommon = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Common).ToList();
                            for (var i = 0; i < uncommon.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Common).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(uncommon[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.UnCommonOnly:
                            var ununcommon = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Uncommon).ToList();
                            for (var i = 0; i < ununcommon.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Uncommon).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(ununcommon[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.RarityOnly:
                            var unrarity = retitems
                                .Where(x => IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.Rare).ToList();
                            for (var i = 0; i < unrarity.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.Rare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(unrarity[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.SuperRarityOnly:
                            var unsuperrarity = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity != Rarity.SuperRare).ToList();
                            for (var i = 0; i < unsuperrarity.Count; i++)
                            {
                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.SuperRare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                retitems.Remove(unsuperrarity[i]);
                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                        case DrawType.AtleastOneSuperRarity:
                            var superrarity = retitems.Where(x =>
                                IrisCardFactory.Instance.Items[x.ItemID].Rarity == Rarity.SuperRare).ToList();
                            if (superrarity.Count == 0)
                            {
                                idx = Global.Random.Next(0, retitems.Count - 1);
                                retitems.RemoveAt(idx);

                                var nomalcards = selectedcards.Where(x => x.Rarity == Rarity.SuperRare).ToList();
                                idx = Global.Random.Next(0, nomalcards.Count - 1);

                                var item = ItemFactory.Instance.GetItem(nomalcards[idx].ID);
                                item.Stack = 1;
                                item.Identified = true;
                                retitems.Add(item);
                            }

                            break;
                    }


                    //这里把卡片给出去
                    foreach (var item in retitems)
                    {
                        results.Add(item.ItemID);
                        AddItem(item, true);
                    }

                    var p2 = new SSMG_IRIS_GACHA_RESULT();
                    p2.ItemIDs = results;
                    netIO.SendPacket(p2);
                }
        }

        public void OnIrisCardAssembleCancel(CSMG_IRIS_CARD_ASSEMBLE_CANCEL p)
        {
            irisCardAssemble = false;
        }

        public void OnIrisCardAssemble(CSMG_IRIS_CARD_ASSEMBLE_CONFIRM p)
        {
            var cardID = p.CardID;
            if (CountItem(cardID) > 0)
            {
                if (IrisCardFactory.Instance.Items.ContainsKey(cardID))
                {
                    var card = IrisCardFactory.Instance.Items[cardID];
                    if (card.NextCard != 0)
                    {
                        var rates = new int[4] { 90, 60, 30, 5 };
                        var counts = new int[4] { 10, 2, 2, 2 };
                        var SupportItem = p.SupportItem;
                        var ProtectItem = p.ProtectItem;

                        var rate = rates[card.Rank];
                        var count = counts[card.Rank];
                        if (SupportItem == 10087101 || SupportItem == 10087100)
                            rate += 100;
                        else if (SupportItem != 0)
                            rate += 5;
                        if (CountItem(cardID) >= count)
                        {
                            if (Character.Gold >= 5000)
                            {
                                Character.Gold -= 5000;

                                if (SupportItem != 0)
                                    DeleteItemID(SupportItem, 1, true);

                                if (ProtectItem != 0)
                                    DeleteItemID(ProtectItem, 1, true);

                                if (Global.Random.Next(0, 99) < rate)
                                {
                                    DeleteItemID(cardID, (ushort)count, true);
                                    AddItem(ItemFactory.Instance.GetItem(card.NextCard), true);
                                    var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                    p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.OK;
                                    netIO.SendPacket(p1);
                                }
                                else
                                {
                                    if (ProtectItem == 0)
                                        DeleteItemID(cardID, (ushort)count, true);

                                    var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                    p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.FAILED;
                                    netIO.SendPacket(p1);
                                    irisCardAssemble = false;
                                }
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                                p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.NOT_ENOUGH_GOLD;
                                netIO.SendPacket(p1);
                                irisCardAssemble = false;
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                            p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.SUCCESS_NOT_ENOUGH_ITEM;
                            netIO.SendPacket(p1);
                            irisCardAssemble = false;
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.SUCCESS_NOT_ENOUGH_ITEM;
                        netIO.SendPacket(p1);
                        irisCardAssemble = false;
                    }
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_ASSEMBLE_RESULT();
                p1.Result = SSMG_IRIS_CARD_ASSEMBLE_RESULT.Results.NO_ITEM;
                netIO.SendPacket(p1);
                irisCardAssemble = false;
            }
        }

        public void OnIrisCardClose(CSMG_IRIS_CARD_CLOSE p)
        {
            irisCardItem = 0;
        }

        public void OnIrisCardLock(CSMG_IRIS_CARD_LOCK p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                item.Locked = true;
                SendItemIdentify(item.Slot);
                var p1 = new SSMG_IRIS_CARD_LOCK_RESULT();
                netIO.SendPacket(p1);
            }
        }

        public void OnIrisCardUnlock(CSMG_IRIS_CARD_UNLOCK p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                item.Locked = false;
                SendItemIdentify(item.Slot);

                var p1 = new SSMG_IRIS_CARD_UNLOCK_RESULT();
                p1.Result = (byte)(CountItem(16003400u) > 0 ? 0x00 : 0x01);
                if (CountItem(16003400u) > 0)
                    DeleteItem(GetItem(16003400u)[0].Slot, 1, true);
                netIO.SendPacket(p1);
            }
        }

        /// <summary>
        ///     给武器打洞
        /// </summary>
        /// <param name="pc"></param>
        protected void ItemAddSlot(ActorPC pc)
        {
            var items = new List<uint>();
            foreach (var i in pc.Inventory.GetContainer(ContainerType.BODY))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.BACK_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.LEFT_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            foreach (var i in pc.Inventory.GetContainer(ContainerType.RIGHT_BAG))
                if (i.IsEquipt)
                {
                    if (i.CurrentSlot >= 10 || (i.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && i.CurrentSlot >= 5))
                        continue;
                    if (i.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE ||
                        i.EquipSlot[0] == EnumEquipSlot.UPPER_BODY ||
                        i.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                        items.Add(i.Slot);
                }

            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].CurrentSlot < 10)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].Slot);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.UPPER_BODY))
                if (pc.Inventory.Equipments[EnumEquipSlot.UPPER_BODY].CurrentSlot < 10)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.UPPER_BODY].Slot);
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.CHEST_ACCE))
                if (pc.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].CurrentSlot < 5)
                    items.Add(pc.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE].Slot);

            //思念的结晶
            if (CountItem(10073000) > 0)
                items.Insert(0, GetItem(10073000)[0].Slot);

            //大的思念结晶
            if (CountItem(10073100) > 0)
                items.Insert(0, GetItem(10073100)[0].Slot);

            //真实的思念结晶
            if (CountItem(10073200) > 0)
                items.Insert(0, GetItem(10073200)[0].Slot);

            //插槽用钻孔机3
            if (CountItem(6001400) > 0)
                items.Add(GetItem(16001400)[0].Slot);

            //插槽用钻孔机4
            if (CountItem(16001401) > 0)
                items.Add(GetItem(16001401)[0].Slot);

            //插槽用钻孔机5
            if (CountItem(16001402) > 0)
                items.Add(GetItem(16001402)[0].Slot);

            //插槽用钻孔机6
            if (CountItem(16001403) > 0)
                items.Add(GetItem(16001403)[0].Slot);

            //插槽用钻孔机7
            if (CountItem(16001404) > 0)
                items.Add(GetItem(16001404)[0].Slot);

            //插槽用钻孔机8
            if (CountItem(16001405) > 0)
                items.Add(GetItem(16001405)[0].Slot);

            //插槽用钻孔机9
            if (CountItem(16001407) > 0)
                items.Add(GetItem(16001407)[0].Slot);

            //插槽用钻孔机10
            if (CountItem(16001408) > 0)
                items.Add(GetItem(16001408)[0].Slot);

            //武具保险书·扩展插槽
            if (CountItem(16001500) > 0)
                items.Add(GetItem(16001500)[0].Slot);

            //∽スロット用ドリル（ビギナー）
            if (CountItem(16001406) > 0)
                items.Add(GetItem(16001406)[0].Slot);

            var p = new SSMG_IRIS_ADD_SLOT_ITEM_LIST();
            p.Items = items;
            netIO.SendPacket(p);
        }

        public void OnIrisCardRemove(CSMG_IRIS_CARD_REMOVE p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                if (!item.Locked)
                {
                    if (p.CardSlot < item.Cards.Count)
                    {
                        var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.OK;
                        netIO.SendPacket(p1);

                        var card = item.Cards[p.CardSlot];
                        AddItem(ItemFactory.Instance.GetItem(card.ID), true);
                        item.Cards.RemoveAt(p.CardSlot);
                        SendItemCardInfo(item);
                        SendItemCardAbility(item);
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                        p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                        netIO.SendPacket(p1);
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                    p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                    netIO.SendPacket(p1);
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_REMOVE_RESULT();
                p1.Result = SSMG_IRIS_CARD_REMOVE_RESULT.Results.FAILED;
                netIO.SendPacket(p1);
            }
        }

        public void OnIrisCardInsert(CSMG_IRIS_CARD_INSERT p)
        {
            var item = Character.Inventory.GetItem(irisCardItem);
            if (item != null)
            {
                if (item.Cards.Count < item.CurrentSlot)
                {
                    var card = Character.Inventory.GetItem(p.InventorySlot);
                    if (card != null)
                        if (card.BaseData.itemType == ItemType.IRIS_CARD)
                        {
                            if (IrisCardFactory.Instance.Items.ContainsKey(card.BaseData.id))
                            {
                                DeleteItem(card.Slot, 1, true);
                                var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                                p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.OK;
                                netIO.SendPacket(p1);
                                var cardInfo = IrisCardFactory.Instance.Items[card.BaseData.id];
                                item.Cards.Add(cardInfo);
                                SendItemCardInfo(item);
                                SendItemCardAbility(item);
                                StatusFactory.Instance.CalcStatus(Character);
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                                p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.CANNOT_SET;
                                netIO.SendPacket(p1);
                            }
                        }
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_INSERT_RESULT();
                    p1.Result = SSMG_IRIS_CARD_INSERT_RESULT.Results.SLOT_OVER;
                    netIO.SendPacket(p1);
                }
            }
        }

        public void OnIrisCardOpen(CSMG_IRIS_CARD_OPEN p)
        {
            var item = Character.Inventory.GetItem(p.InventorySlot);
            if (item != null)
            {
                if (item.CurrentSlot > 0)
                {
                    irisCardItem = item.Slot;
                    var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                    p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.OK;
                    netIO.SendPacket(p1);

                    SendItemCardAbility(item);
                }
                else
                {
                    var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                    p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.NO_SLOT;
                    netIO.SendPacket(p1);
                }
            }
            else
            {
                var p1 = new SSMG_IRIS_CARD_OPEN_RESULT();
                p1.Result = SSMG_IRIS_CARD_OPEN_RESULT.Results.NO_ITEM;
                netIO.SendPacket(p1);
            }
        }

        public void OnIrisAddSlotConfirm(CSMG_IRIS_ADD_SLOT_CONFIRM p)
        {
            if (irisAddSlot)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item != null)
                {
                    var gold = item.BaseData.possibleLv * 1000;

                    var material = p.Material;
                    var protectitem = p.ProtectItem;
                    var supportitem = p.SupportItem;
                    if (CountItem(material) > 0)
                    {
                        if (Character.Gold > gold)
                        {
                            if ((!item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && item.CurrentSlot < 10) ||
                                (item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE) && item.CurrentSlot < 5))
                            {
                                Character.Gold -= gold;

                                DeleteItemID(material, 1, true);


                                var baseRate = 0;
                                if (!item.EquipSlot.Contains(EnumEquipSlot.CHEST_ACCE))
                                    baseRate = 100 - item.CurrentSlot * 10;
                                else
                                    baseRate = 100 - item.CurrentSlot * 20;

                                if (baseRate < 0)
                                    baseRate = 5;

                                if (supportitem == 16001406 && item.CurrentSlot < 2)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 100;
                                }
                                else if (supportitem == 16001400 && item.CurrentSlot < 3)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001401 && item.CurrentSlot < 4)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001402 && item.CurrentSlot < 5)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001403 && item.CurrentSlot < 6)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001404 && item.CurrentSlot < 7)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001405 && item.CurrentSlot < 8)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001407 && item.CurrentSlot < 9)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }
                                else if (supportitem == 16001408 && item.CurrentSlot < 10)
                                {
                                    DeleteItemID(supportitem, 1, true);
                                    baseRate += 5;
                                }

                                if (protectitem != 0)
                                    DeleteItemID(protectitem, 1, true);


                                if (Global.Random.Next(1, 100) < baseRate)
                                {
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.OK;
                                    netIO.SendPacket(p1);
                                    SendEffect(5145);
                                    item.CurrentSlot++;
                                    SendItemInfo(item);

                                    ItemAddSlot(Character);
                                    //this.irisAddSlot = false;
                                }
                                else if (protectitem != 0)
                                {
                                    //DeleteItemID(p.ProtectItem, 1, true);
                                    SendSystemMessage("装备打洞失败！使用了一本防爆书（打洞）。");
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                    netIO.SendPacket(p1);

                                    ItemAddSlot(Character);
                                    //this.irisAddSlot = false;
                                }
                                else
                                {
                                    DeleteItem(item.Slot, 1, true);
                                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                    netIO.SendPacket(p1);

                                    irisAddSlot = false;
                                }
                            }
                            else
                            {
                                var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                                p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                                netIO.SendPacket(p1);

                                irisAddSlot = false;
                            }
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                            p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NOT_ENOUGH_GOLD;
                            netIO.SendPacket(p1);

                            irisAddSlot = false;
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                        p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_RIGHT_MATERIAL;
                        netIO.SendPacket(p1);

                        irisAddSlot = false;
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_ITEM;
                    netIO.SendPacket(p1);

                    irisAddSlot = false;
                }
            }
        }

        public void OnIrisAddSlotCancel(CSMG_IRIS_ADD_SLOT_CANCEL p)
        {
            irisAddSlot = false;
        }

        public void OnIrisAddSlotItemSelect(CSMG_IRIS_ADD_SLOT_ITEM_SELECT p)
        {
            if (irisAddSlot)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item != null)
                {
                    var gold = item.BaseData.possibleLv * 1000;
                    uint material = 0;
                    if (item.BaseData.possibleLv <= 30)
                        material = 10073000;
                    else if (item.BaseData.possibleLv <= 70)
                        material = 10073100;
                    else
                        material = 10073200;
                    if (Character.Gold > gold)
                    {
                        if (item.CurrentSlot < 5)
                        {
                            irisAddSlotMaterial = material;
                            irisAddSlotItem = item.Slot;

                            var p1 = new SSMG_IRIS_ADD_SLOT_MATERIAL();
                            p1.Slot = 1;
                            p1.Material = material;
                            p1.Gold = gold;
                            netIO.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                            p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.Failed;
                            netIO.SendPacket(p1);
                        }
                    }
                    else
                    {
                        var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                        p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NOT_ENOUGH_GOLD;
                        netIO.SendPacket(p1);
                    }
                }
                else
                {
                    var p1 = new SSMG_IRIS_ADD_SLOT_RESULT();
                    p1.Result = SSMG_IRIS_ADD_SLOT_RESULT.Results.NO_ITEM;
                    netIO.SendPacket(p1);
                    irisAddSlot = false;
                }
            }
        }

        public void SendItemCardInfo(Item item)
        {
            var p = new SSMG_ITEM_IRIS_CARD_INFO();
            p.Item = item;
            netIO.SendPacket(p);
        }

        public void SendItemCardAbility(Item item)
        {
            var p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Deck;
            p.AbilityVectors = item.AbilityVectors(true);
            p.VectorValues = item.VectorValues(true, false).Values.ToList();
            p.VectorLevels = item.VectorValues(true, true).Values.ToList();
            var release = item.ReleaseAbilities(true);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                p.ElementsAttack = item.IrisElements(true);
            else
                p.ElementsAttack = Item.ElementsZero();
            if (item.EquipSlot[0] == EnumEquipSlot.UPPER_BODY || item.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE)
                p.ElementsDefence = item.IrisElements(true);
            else
                p.ElementsDefence = Item.ElementsZero();
            netIO.SendPacket(p);

            p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Max;
            p.AbilityVectors = item.AbilityVectors(false);
            p.VectorValues = item.VectorValues(false, false).Values.ToList();
            p.VectorLevels = item.VectorValues(false, true).Values.ToList();
            release = item.ReleaseAbilities(false);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            if (item.EquipSlot[0] == EnumEquipSlot.RIGHT_HAND)
                p.ElementsAttack = item.IrisElements(false);
            else
                p.ElementsAttack = Item.ElementsZero();
            if (item.EquipSlot[0] == EnumEquipSlot.UPPER_BODY || item.EquipSlot[0] == EnumEquipSlot.CHEST_ACCE)
                p.ElementsDefence = item.IrisElements(false);
            else
                p.ElementsDefence = Item.ElementsZero();
            netIO.SendPacket(p);

            p = new SSMG_IRIS_CARD_ITEM_ABILITY();
            p.Type = SSMG_IRIS_CARD_ITEM_ABILITY.Types.Total;
            p.AbilityVectors = Character.IrisAbilityValues.Keys.ToList();
            p.VectorValues = Character.IrisAbilityValues.Values.ToList();
            p.VectorLevels = Character.IrisAbilityLevels.Values.ToList();
            release = Item.ReleaseAbilities(Character.IrisAbilityLevels);
            p.ReleaseAbilities = release.Keys.ToList();
            p.AbilityValues = release.Values.ToList();
            p.ElementsAttack = Character.Status.attackelements_iris;
            p.ElementsDefence = Character.Status.elements_iris;
            netIO.SendPacket(p);
        }
    }
}