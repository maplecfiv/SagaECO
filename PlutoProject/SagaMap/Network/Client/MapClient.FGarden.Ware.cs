using System.Collections.Generic;
using SagaDB.Item;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnFGardenWareClose(CSMG_FG_WARE_CLOSE p)
        {
            currentWarehouse = WarehousePlace.Current;
        }

        public void OnFGardenWareGet(CSMG_FG_WARE_GET p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = 1;
            }
            else
            {
                var item = Character.Inventory.GetItem(currentWarehouse, p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else
                {
                    Item newItem;
                    switch (Character.Inventory.DeleteWareItem(currentWarehouse, item.Slot, p.Count))
                    {
                        case InventoryDeleteResult.ALL_DELETED:
                            var p1 = new SSMG_ITEM_DELETE();
                            p1.InventorySlot = item.Slot;
                            netIO.SendPacket(p1);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.STACK_UPDATED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            netIO.SendPacket(p2);
                            newItem = item.Clone();
                            newItem.Stack = p.Count;
                            Logger.LogItemGet(Logger.EventType.ItemWareGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("WareGet Count:{0}", item.Stack), false);
                            AddItem(newItem, false);
                            SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_GET,
                                item.BaseData.name, p.Count));
                            break;
                        case InventoryDeleteResult.ERROR:
                            result = -99;
                            break;
                    }
                }
            }

            var p5 = new SSMG_ITEM_WARE_GET_RESULT();
            p5.Result = result;
            netIO.SendPacket(p5);
        }

        public void OnFGardenWarePut(CSMG_FG_WARE_PUT p)
        {
            var result = 0;
            if (currentWarehouse == WarehousePlace.Current)
            {
                result = -1;
            }
            else
            {
                var item = Character.Inventory.GetItem(p.InventoryID);
                if (item == null)
                {
                    result = -2;
                }
                else if (item.Stack < p.Count)
                {
                    result = -3;
                }
                else if (Character.Inventory.WareTotalCount >= Configuration.Instance.WarehouseLimit)
                {
                    result = -4;
                }
                else
                {
                    Logger.LogItemLost(Logger.EventType.ItemWareLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")", string.Format("WarePut Count:{0}", p.Count),
                        false);
                    DeleteItem(p.InventoryID, p.Count, false);
                    var newItem = item.Clone();
                    newItem.Stack = p.Count;
                    switch (Character.Inventory.AddWareItem(currentWarehouse, newItem))
                    {
                        case InventoryAddResult.NEW_INDEX:
                            var p1 = new SSMG_FG_WARE_ITEM();
                            p1.InventorySlot = newItem.Slot;
                            p1.Item = newItem;
                            netIO.SendPacket(p1);
                            break;
                        case InventoryAddResult.STACKED:
                            var p2 = new SSMG_ITEM_COUNT_UPDATE();
                            p2.InventorySlot = item.Slot;
                            p2.Stack = item.Stack;
                            netIO.SendPacket(p2);
                            break;
                        case InventoryAddResult.MIXED:
                            var p3 = new SSMG_ITEM_COUNT_UPDATE();
                            p3.InventorySlot = item.Slot;
                            p3.Stack = item.Stack;
                            netIO.SendPacket(p3);
                            var p4 = new SSMG_FG_WARE_ITEM();
                            p4.InventorySlot = Character.Inventory.LastItem.Slot;
                            p4.Item = Character.Inventory.LastItem;
                            netIO.SendPacket(p4);
                            break;
                    }

                    SendSystemMessage(string.Format(LocalManager.Instance.Strings.ITEM_WARE_PUT, item.BaseData.name,
                        p.Count));
                }
            }

            var p5 = new SSMG_ITEM_WARE_PUT_RESULT();
            p5.Result = result;
            netIO.SendPacket(p5);
        }

        public void SendFGardenWareItems()
        {
            currentWarehouse = WarehousePlace.FGarden;
            var p0 = new SSMG_FG_WARE_SENDCOUNT();
            Character.Inventory.WareHouse[currentWarehouse] = new List<Item>(300);
            p0.CurrentCount = Character.Inventory.WareHouse[currentWarehouse].Capacity;
            netIO.SendPacket(p0);

            var p1 = new SSMG_FG_WARE_HEADER();
            netIO.SendPacket(p1);

            foreach (var j in Character.Inventory.WareHouse[currentWarehouse])
            {
                //if (j.Refine == 0)
                //    j.Clear();

                var p2 = new SSMG_FG_WARE_ITEM();
                p2.Item = j;
                p2.InventorySlot = j.Slot;
                //if (i == place)
                //    p2.Place = WarehousePlace.Current;
                //else
                //    p2.Place = i;
                netIO.SendPacket(p2);
            }

            var p3 = new SSMG_FG_WARE_FOOTER();
            netIO.SendPacket(p3);
        }
    }
}