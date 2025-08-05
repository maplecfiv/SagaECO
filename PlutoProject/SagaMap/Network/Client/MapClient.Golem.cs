using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnGolemWarehouse(CSMG_GOLEM_WAREHOUSE p)
        {
            var p1 = new SSMG_GOLEM_WAREHOUSE();
            p1.ActorID = Character.ActorID;
            p1.Title = Character.Golem.Title;
            netIO.SendPacket(p1);

            foreach (var i in Character.Inventory.GetContainer(ContainerType.GOLEMWAREHOUSE))
            {
                var p2 = new SSMG_GOLEM_WAREHOUSE_ITEM();
                p2.InventorySlot = i.Slot;
                p2.Container = ContainerType.GOLEMWAREHOUSE;
                p2.Item = i;
                netIO.SendPacket(p2);
            }

            var p3 = new SSMG_GOLEM_WAREHOUSE_ITEM_FOOTER();
            netIO.SendPacket(p3);
        }

        public void OnGolemWarehouseSet(CSMG_GOLEM_WAREHOUSE_SET p)
        {
            if (Character.Golem != null)
                Character.Golem.Title = p.Title;
        }

        public void OnGolemWarehouseGet(CSMG_GOLEM_WAREHOUSE_GET p)
        {
            var item = Character.Inventory.GetItem(p.InventoryID);
            if (item != null)
            {
                var count = p.Count;
                if (item.Stack >= count)
                {
                    var newItem = item.Clone();
                    newItem.Stack = count;
                    if (newItem.Stack > 0)
                        Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                            Character.Name + "(" + Character.CharID + ")",
                            newItem.BaseData.name + "(" + newItem.ItemID + ")",
                            string.Format("GolemWarehouseGet Count:{0}", count), false);

                    Character.Inventory.DeleteItem(p.InventoryID, count);
                    Logger.LogItemGet(Logger.EventType.ItemGolemGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("GolemWarehouse Count:{0}", item.Stack), false);
                    AddItem(newItem, false);
                    var p1 = new SSMG_GOLEM_WAREHOUSE_GET();
                    netIO.SendPacket(p1);
                }
            }
        }

        public void OnGolemShopBuySell(CSMG_GOLEM_SHOP_BUY_SELL p)
        {
            var actor = map.GetActor(p.ActorID);
            var items = p.Items;
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;
                uint gold = 0;
                foreach (var i in items.Keys)
                {
                    var item = Character.Inventory.GetItem(i);
                    if (item == null)
                        continue;
                    if (items[i] == 0)
                        continue;
                    //if (item.BaseData.noTrade)
                    //continue;
                    var newItem = item.Clone();
                    if (item.Stack >= items[i])
                    {
                        uint inventoryID = 0;
                        foreach (var j in golem.BuyShop.Keys)
                            if (golem.BuyShop[j].ItemID == newItem.ItemID)
                            {
                                inventoryID = j;
                                break;
                            }

                        gold += golem.BuyShop[inventoryID].Price * items[i];
                        if (golem.BuyLimit < gold)
                        {
                            gold -= golem.BuyShop[inventoryID].Price * items[i];
                            break;
                        }

                        newItem.Stack = items[i];
                        Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                            Character.Name + "(" + Character.CharID + ")", item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("GolemSell Count:{0}", items[i]), false);
                        DeleteItem(i, items[i], true);
                        golem.BuyShop[inventoryID].Count -= items[i];

                        if (golem.BoughtItem.ContainsKey(item.ItemID))
                        {
                            golem.BoughtItem[item.ItemID].Count += items[i];
                        }
                        else
                        {
                            golem.BoughtItem.Add(item.ItemID, new GolemShopItem());
                            golem.BoughtItem[item.ItemID].Price = golem.BuyShop[inventoryID].Price;
                            golem.BoughtItem[item.ItemID].Count += items[i];
                        }

                        if (newItem.Stack > 0)
                            Logger.LogItemGet(Logger.EventType.ItemGolemGet,
                                Character.Name + "(" + Character.CharID + ")",
                                newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                string.Format("GolemBuy Count:{0}", newItem.Stack), false);
                        if (golem.BuyShop[inventoryID].Count == 0) //新加
                            golem.BuyShop.Remove(inventoryID);
                        //golem.Owner.Inventory.AddItem(ContainerType.BODY, newItem);
                    }
                }

                //golem.Owner.Gold -= (int)gold;
                golem.BuyLimit -= gold;
                Character.Gold += (int)gold;
            }
        }

        public void OnGolemShopSellBuy(CSMG_GOLEM_SHOP_SELL_BUY p)
        {
            var actor = map.GetActor(p.ActorID);
            var items = p.Items;
            var p1 = new SSMG_GOLEM_SHOP_SELL_ANSWER();
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;
                uint gold = 0;
                foreach (var i in items.Keys)
                {
                    var item = ItemFactory.Instance.GetItem(golem.SellShop[i].ItemID);
                    if (item == null)
                    {
                        p1.Result = -4;
                        netIO.SendPacket(p1);
                        return;
                    }

                    if (items[i] == 0)
                    {
                        p1.Result = -2;
                        netIO.SendPacket(p1);
                        return;
                    }

                    /*if (item.BaseData.noTrade)
                    {
                        p1.Result = -1;
                        this.netIO.SendPacket(p1);
                        return;
                    }*/
                    if (golem.SellShop[i].Count >= items[i])
                    {
                        gold += golem.SellShop[i].Price * items[i];
                        if (Character.Gold < gold)
                        {
                            p1.Result = -7;
                            netIO.SendPacket(p1);
                            return;
                        }

                        /*if (gold + golem.Owner.Gold >= 99999999)
                        {
                            p1.Result = -9;
                            this.netIO.SendPacket(p1);
                            return;
                        }*/
                        var newItem = item.Clone();
                        newItem.Stack = items[i];
                        if (newItem.Stack > 0)
                            Logger.LogItemLost(Logger.EventType.ItemGolemLost,
                                Character.Name + "(" + Character.CharID + ")",
                                newItem.BaseData.name + "(" + newItem.ItemID + ")",
                                string.Format("GolemSell Count:{0}", items[i]), false);
                        //golem.Owner.Inventory.DeleteItem(i, items[i]);
                        golem.SellShop[i].Count -= items[i];
                        if (golem.SoldItem.ContainsKey(item.ItemID))
                        {
                            golem.SoldItem[item.ItemID].Count += items[i];
                        }
                        else
                        {
                            golem.SoldItem.Add(item.ItemID, new GolemShopItem());
                            golem.SoldItem[item.ItemID].Price = golem.SellShop[i].Price;
                            golem.SoldItem[item.ItemID].Count += items[i];
                        }

                        if (golem.SellShop[i].Count == 0) golem.SellShop.Remove(i);

                        if (golem.SellShop.Count == 0)
                        {
                            golem.invisble = true;
                            map.OnActorVisibilityChange(golem);
                        }

                        Logger.LogItemGet(Logger.EventType.ItemGolemGet, Character.Name + "(" + Character.CharID + ")",
                            item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("GolemBuy Count:{0}", item.Stack), false);
                        AddItem(newItem, true);
                    }
                    else
                    {
                        p1.Result = -5;
                        netIO.SendPacket(p1);
                        return;
                    }
                }

                //golem.Owner.Gold += (int)gold;
                Character.Gold -= (int)gold;
                /*try
                {
                     SagaMap.MapServer. charDB.SaveChar(golem.Owner, true, false);
                }
                catch (Exception ex) { Logger.ShowError(ex); }*/
            }
        }

        public void OnGolemShopOpen(CSMG_GOLEM_SHOP_OPEN p)
        {
            var actor = map.GetActor(p.ActorID);
            if (actor.type == ActorType.GOLEM)
            {
                var golem = (ActorGolem)actor;

                if (golem.GolemType == GolemType.Sell)
                {
                    var p1 = new SSMG_GOLEM_SHOP_OPEN_OK();
                    p1.ActorID = p.ActorID;
                    netIO.SendPacket(p1);
                    var p2 = new SSMG_GOLEM_SHOP_HEADER();
                    p2.ActorID = p.ActorID;
                    netIO.SendPacket(p2);
                    foreach (var i in golem.SellShop.Keys)
                    {
                        var item = golem.Owner.Inventory.GetItem(i);
                        if (item == null)
                            continue;
                        var p3 = new SSMG_GOLEM_SHOP_ITEM();
                        p3.InventorySlot = i;
                        p3.Container = ContainerType.BODY;
                        p3.Price = golem.SellShop[i].Price;
                        p3.ShopCount = golem.SellShop[i].Count;
                        p3.Item = item;
                        netIO.SendPacket(p3);
                    }

                    var p4 = new SSMG_GOLEM_SHOP_FOOTER();
                    netIO.SendPacket(p4);
                }

                if (golem.GolemType == GolemType.Buy)
                {
                    var p2 = new SSMG_GOLEM_SHOP_BUY_HEADER();
                    p2.ActorID = p.ActorID;
                    netIO.SendPacket(p2);

                    var p3 = new SSMG_GOLEM_SHOP_BUY_ITEM();
                    p3.Items = golem.BuyShop;
                    p3.BuyLimit = golem.BuyLimit;
                    netIO.SendPacket(p3);
                }
            }
        }

        public void OnGolemShopSellClose(CSMG_GOLEM_SHOP_SELL_CLOSE p)
        {
            var p1 = new SSMG_GOLEM_SHOP_SELL_SET();
            netIO.SendPacket(p1);
        }

        public void OnGolemShopSellSetup(CSMG_GOLEM_SHOP_SELL_SETUP p)
        {
            var ids = p.InventoryIDs;
            var counts = p.Counts;
            var prices = p.Prices;
            if (ids.Length != 0)
                for (var i = 0; i < ids.Length; i++)
                {
                    if (!Character.Golem.SellShop.ContainsKey(ids[i]))
                    {
                        var item = new GolemShopItem();
                        item.InventoryID = ids[i];
                        item.ItemID = Character.Inventory.GetItem(ids[i]).ItemID;
                        Character.Golem.SellShop.Add(ids[i], item);
                    }

                    if (counts[i] == 0)
                    {
                        Character.Golem.SellShop.Remove(ids[i]);
                    }
                    else
                    {
                        Character.Golem.SellShop[ids[i]].Count = counts[i];
                        Character.Golem.SellShop[ids[i]].Price = prices[i];
                    }
                }

            Character.Golem.Title = p.Comment;
        }

        public void OnGolemShopSell(CSMG_GOLEM_SHOP_SELL p)
        {
            var p1 = new SSMG_GOLEM_SHOP_SELL_SETUP();
            p1.Comment = Character.Golem.Title;
            netIO.SendPacket(p1);
        }

        public void OnGolemShopBuyClose(CSMG_GOLEM_SHOP_BUY_CLOSE p)
        {
            var p1 = new SSMG_GOLEM_SHOP_BUY_SET();
            netIO.SendPacket(p1);
        }

        public void OnGolemShopBuySetup(CSMG_GOLEM_SHOP_BUY_SETUP p)
        {
            var ids = p.InventoryIDs;
            var counts = p.Counts;
            var prices = p.Prices;
            if (ids.Length != 0)
                for (var i = 0; i < ids.Length; i++)
                {
                    if (!Character.Golem.BuyShop.ContainsKey(ids[i]))
                    {
                        var item = new GolemShopItem();
                        item.InventoryID = ids[i];
                        item.ItemID = Character.Inventory.GetItem(ids[i]).ItemID;
                        Character.Golem.BuyShop.Add(ids[i], item);
                    }

                    if (counts[i] == 0)
                    {
                        Character.Golem.BuyShop.Remove(ids[i]);
                    }
                    else
                    {
                        Character.Golem.BuyShop[ids[i]].Count = counts[i];
                        Character.Golem.BuyShop[ids[i]].Price = prices[i];
                    }
                }

            Character.Golem.BuyLimit = p.BuyLimit;
            Character.Golem.Title = p.Comment;
        }

        public void OnGolemShopBuy(CSMG_GOLEM_SHOP_BUY p)
        {
            var p1 = new SSMG_GOLEM_SHOP_BUY_SETUP();
            p1.BuyLimit = Character.Golem.BuyLimit;
            p1.Comment = Character.Golem.Title;
            Character.Golem.BuyShop.Clear();
            netIO.SendPacket(p1);
        }
    }
}