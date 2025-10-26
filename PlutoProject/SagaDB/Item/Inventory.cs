using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SagaDB.Actor;
using SagaDB.DEMIC;
using SagaLib;

namespace SagaDB.Item {
    [Serializable]
    public class Inventory {
        public enum SearchType {
            ITEM_ID,
            SLOT_ID
        }

        private static ushort version = 6;
        private Dictionary<byte, DEMICPanel> ddemicChips = new Dictionary<byte, DEMICPanel>();
        private Dictionary<byte, DEMICPanel> demicChips = new Dictionary<byte, DEMICPanel>();
        private uint golemWareIndex = 300000001;

        private uint index = 1;

        private bool needSaveWare;

        [NonSerialized] private ActorPC owner;

        private Dictionary<WarehousePlace, List<Item>> ware = new Dictionary<WarehousePlace, List<Item>>();

        [NonSerialized] public uint wareIndex = 200000001;


        public Inventory(ActorPC owner) {
            this.owner = owner;
            Items.Add(ContainerType.BODY, new List<Item>());
            Items.Add(ContainerType.LEFT_BAG, new List<Item>());
            Items.Add(ContainerType.RIGHT_BAG, new List<Item>());
            Items.Add(ContainerType.BACK_BAG, new List<Item>());
            Items.Add(ContainerType.GOLEMWAREHOUSE, new List<Item>());

            ware.Add(WarehousePlace.Acropolis, new List<Item>());
            ware.Add(WarehousePlace.FederalOfIronSouth, new List<Item>());
            ware.Add(WarehousePlace.FarEast, new List<Item>());
            ware.Add(WarehousePlace.IronSouth, new List<Item>());
            ware.Add(WarehousePlace.KingdomOfNorthan, new List<Item>());
            ware.Add(WarehousePlace.MiningCamp, new List<Item>());
            ware.Add(WarehousePlace.Morg, new List<Item>());
            ware.Add(WarehousePlace.Northan, new List<Item>());
            ware.Add(WarehousePlace.RepublicOfFarEast, new List<Item>());
            ware.Add(WarehousePlace.Tonka, new List<Item>());
            ware.Add(WarehousePlace.ECOTown, new List<Item>());
            ware.Add(WarehousePlace.MaimaiCamp, new List<Item>());
            ware.Add(WarehousePlace.MermaidsHome, new List<Item>());
            ware.Add(WarehousePlace.TowerGoesToHeaven, new List<Item>());
            ware.Add(WarehousePlace.WestFord, new List<Item>());

            Payload.Add(ContainerType.BODY, 0);
            Payload.Add(ContainerType.LEFT_BAG, 0);
            Payload.Add(ContainerType.RIGHT_BAG, 0);
            Payload.Add(ContainerType.BACK_BAG, 0);
            MaxPayload.Add(ContainerType.BODY, 0);
            MaxPayload.Add(ContainerType.LEFT_BAG, 0);
            MaxPayload.Add(ContainerType.RIGHT_BAG, 0);
            MaxPayload.Add(ContainerType.BACK_BAG, 0);
            Volume.Add(ContainerType.BODY, 0);
            Volume.Add(ContainerType.LEFT_BAG, 0);
            Volume.Add(ContainerType.RIGHT_BAG, 0);
            Volume.Add(ContainerType.BACK_BAG, 0);
            MaxVolume.Add(ContainerType.BODY, 0);
            MaxVolume.Add(ContainerType.LEFT_BAG, 0);
            MaxVolume.Add(ContainerType.RIGHT_BAG, 0);
            MaxVolume.Add(ContainerType.BACK_BAG, 0);

            demicChips.Add(0, new DEMICPanel());
            demicChips.Add(100, new DEMICPanel());
            demicChips.Add(101, new DEMICPanel());
            ddemicChips.Add(0, new DEMICPanel());
            ddemicChips.Add(100, new DEMICPanel());
            ddemicChips.Add(101, new DEMICPanel());
        }

        public Dictionary<ContainerType, List<Item>> Items { get; } = new Dictionary<ContainerType, List<Item>>();

        public ActorPC Owner {
            get => owner;
            set => owner = value;
        }

        /// <summary>
        ///     负重
        /// </summary>
        public Dictionary<ContainerType, uint> Payload { get; } = new Dictionary<ContainerType, uint>();

        /// <summary>
        ///     最大负重
        /// </summary>
        public Dictionary<ContainerType, uint> MaxPayload { get; } = new Dictionary<ContainerType, uint>();

        /// <summary>
        ///     体积
        /// </summary>
        public Dictionary<ContainerType, uint> Volume { get; } = new Dictionary<ContainerType, uint>();

        /// <summary>
        ///     最大体积
        /// </summary>
        public Dictionary<ContainerType, uint> MaxVolume { get; } = new Dictionary<ContainerType, uint>();

        /// <summary>
        ///     仓库
        /// </summary>
        public Dictionary<WarehousePlace, List<Item>> WareHouse {
            get => ware;
            set => ware = value;
        }

        /// <summary>
        ///     玩家的DEMIC芯片组
        /// </summary>
        public Dictionary<byte, DEMICPanel> DemicChips {
            get {
                int validCL = owner.CL;
                var pageCount = validCL / 81 + 1;
                for (var i = 0; i < pageCount; i++)
                    if (!demicChips.ContainsKey((byte)i))
                        demicChips.Add((byte)i, new DEMICPanel());
                return demicChips;
            }
        }

        /// <summary>
        ///     玩家恶魔界的ＤＥＭＩＣ芯片组
        /// </summary>
        public Dictionary<byte, DEMICPanel> DominionDemicChips {
            get {
                int validCL = owner.DominionCL;
                var pageCount = validCL / 81 + 1;
                for (var i = 0; i < pageCount; i++)
                    if (!ddemicChips.ContainsKey((byte)i))
                        ddemicChips.Add((byte)i, new DEMICPanel());
                return ddemicChips;
            }
        }

        /// <summary>
        ///     检查道具栏是否是空
        /// </summary>
        public bool IsEmpty {
            get {
                foreach (var i in Items.Values)
                    if (i.Count > 0)
                        return false;
                return true;
            }
        }

        public bool NeedSave { get; private set; }

        public bool NeedSaveWare => NeedSave;

        /// <summary>
        ///     检查仓库是否是空
        /// </summary>
        public bool IsWarehouseEmpty {
            get {
                foreach (var i in ware.Values)
                    if (i.Count > 0)
                        return false;
                return true;
            }
        }

        /// <summary>
        ///     仓库目前道具总数
        /// </summary>
        public int WareTotalCount {
            get {
                var count = 0;
                foreach (var i in ware.Values) count += i.Count;
                return count;
            }
        }

        public Dictionary<EnumEquipSlot, Item> Equipments { get; } = new Dictionary<EnumEquipSlot, Item>();

        public Dictionary<EnumEquipSlot, Item> Parts { get; } = new Dictionary<EnumEquipSlot, Item>();

        public Item LastItem { get; private set; }

        /// <summary>
        ///     计算目前道具栏中所有道具的空间和重量
        /// </summary>
        public void CalcPayloadVolume() {
            var list = Items[ContainerType.BODY];
            uint pal = 0, vol = 0;
            foreach (var i in list) {
                pal += i.BaseData.weight * i.Stack;
                vol += i.BaseData.volume * i.Stack;
            }

            if (owner.Form == DEM_FORM.NORMAL_FORM)
                foreach (var i in Equipments.Values) {
                    pal += i.BaseData.weight * i.Stack;
                    vol += i.BaseData.equipVolume * i.Stack;
                }
            else
                foreach (var i in Parts.Values) {
                    pal += i.BaseData.weight * i.Stack;
                    vol += i.BaseData.equipVolume * i.Stack;
                }

            Payload[ContainerType.BODY] = pal;
            Volume[ContainerType.BODY] = vol;

            for (var i = 3; i < 6; i++) {
                var type = (ContainerType)i;
                list = Items[type];
                pal = 0;
                vol = 0;
                foreach (var j in list) {
                    pal += j.BaseData.weight * j.Stack;
                    vol += j.BaseData.volume * j.Stack;
                }

                Payload[type] = pal;
                Volume[type] = vol;
            }
        }

        /// <summary>
        ///     向仓库添加道具
        /// </summary>
        /// <param name="place">仓库地点</param>
        /// <param name="item">要添加的道具</param>
        /// <returns>添加结果，需要注意的只有MIXED，MIXED的话，item则被改为叠加的道具，Inventory.LastItem则是多余的新道具</returns>
        public InventoryAddResult AddWareItem(WarehousePlace place, Item item) {
            try {
                needSaveWare = true;
                if (item.Stack > 0)
                    Logger.LogWarehousePut(owner.Name + "," + owner.CharID,
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("WarehousePlace:{0} Count:{1}", place, item.Stack));
                var query =
                    from it in ware[place]
                    where it.ItemID == item.ItemID && it.Stack < 9999
                    select it;
                if (query.Count() != 0 && item.Stackable) {
                    var oriItem = query.First();
                    oriItem.Stack += item.Stack;
                    if (oriItem.Stack <= 9999) {
                        item.Stack = oriItem.Stack;
                        item.Slot = oriItem.Slot;
                        return InventoryAddResult.STACKED;
                    }

                    var rest = (ushort)(oriItem.Stack - 9999);
                    if (rest > 9999) {
                        Logger.GetLogger().Warning("Adding too many item(" + item.BaseData.name + ":" + item.Stack +
                                                   "), setting count to the maximal value(9999)");
                        rest = 9999;
                    }

                    oriItem.Stack = 9999;
                    item.Stack = oriItem.Stack;
                    item.Slot = oriItem.Slot;
                    var newItem = item.Clone();
                    newItem.Stack = rest;
                    newItem.Slot = wareIndex;
                    wareIndex++;
                    ware[place].Add(newItem);
                    LastItem = newItem;
                    return InventoryAddResult.MIXED;
                }

                if (item.Stack > 9999) {
                    Logger.GetLogger().Warning("Adding too many item(" + item.BaseData.name + ":" + item.Stack +
                                               "), setting count to the maximal value(9999)");
                    item.Stack = 9999;
                }

                ware[place].Add(item);
                item.Slot = wareIndex;
                wareIndex++;

                return InventoryAddResult.NEW_INDEX;
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return InventoryAddResult.ERROR;
            }
        }

        /// <summary>
        ///     从仓库删除物品
        /// </summary>
        /// <param name="place">仓库地点</param>
        /// <param name="slot">物品Slot</param>
        /// <param name="amount">数量</param>
        /// <returns>删除结果</returns>
        public InventoryDeleteResult DeleteWareItem(WarehousePlace place, uint slot, int amount) {
            needSaveWare = true;
            var query =
                from it in ware[place]
                where it.Slot == slot
                select it;
            if (query.Count() == 0)
                return InventoryDeleteResult.ERROR;
            var oriItem = query.First();
            if (oriItem.Stack > 0)
                Logger.LogWarehouseGet(owner.Name + "(" + owner.CharID + ")",
                    oriItem.BaseData.name + "(" + oriItem.ItemID + ")",
                    string.Format("WarehousePlace:{0} Count:{1}", place, oriItem.Stack));


            if (oriItem.Stack > amount) {
                oriItem.Stack -= (ushort)amount;
                return InventoryDeleteResult.STACK_UPDATED;
            }

            ware[place].Remove(oriItem);
            return InventoryDeleteResult.ALL_DELETED;
        }

        /// <summary>
        ///     添加道具
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="item">道具</param>
        /// <param name="newIndex">是否生成新索引</param>
        /// <returns>添加结果，需要注意的只有MIXED，MIXED的话，item则被改为叠加的道具，Inventory.LastItem则是多余的新道具</returns>
        public InventoryAddResult AddItem(ContainerType container, Item item, bool newIndex) {
            NeedSave = true;
            switch (container) {
                case ContainerType.BODY:
                case ContainerType.LEFT_BAG:
                case ContainerType.RIGHT_BAG:
                case ContainerType.BACK_BAG:
                case ContainerType.GOLEMWAREHOUSE:
                    var list = Items[container];
                    var query =
                        from it in list
                        where it.ItemID == item.ItemID && it.Stack < 9999
                        select it;
                    if (query.Count() != 0 && item.Stackable) {
                        var oriItem = query.First();
                        oriItem.Stack += item.Stack;
                        if (oriItem.Stack <= 9999) {
                            //感觉有问题
                            item.Stack = oriItem.Stack;
                            item.Slot = oriItem.Slot;
                            LastItem = oriItem;
                            if (oriItem.Identified)
                                item.identified = oriItem.identified;
                            return InventoryAddResult.STACKED;
                        }

                        var rest = (ushort)(oriItem.Stack - 9999);
                        if (rest > 9999) {
                            Logger.GetLogger().Warning("Adding too many item(" + item.BaseData.name + ":" + item.Stack +
                                                       "), setting count to the maximal value(9999)");
                            rest = 9999;
                        }

                        oriItem.Stack = 9999;
                        item.Stack = oriItem.Stack;
                        item.Slot = oriItem.Slot;
                        if (oriItem.Identified)
                            item.identified = oriItem.identified;
                        var newItem = item.Clone();
                        newItem.Stack = rest;
                        if (container == ContainerType.GOLEMWAREHOUSE) {
                            newItem.Slot = golemWareIndex;
                            golemWareIndex++;
                        }
                        else {
                            newItem.Slot = index;
                            index++;
                        }

                        list.Add(newItem);
                        LastItem = newItem;
                        return InventoryAddResult.MIXED;
                    }

                    if (item.Stack > 9999) {
                        Logger.GetLogger().Warning("Adding too many item(" + item.BaseData.name + ":" + item.Stack +
                                                   "), setting count to the maximal value(9999)");
                        item.Stack = 9999;
                    }

                    list.Add(item);
                    LastItem = item;
                    if (newIndex) {
                        if (container == ContainerType.GOLEMWAREHOUSE) {
                            item.Slot = golemWareIndex;
                            golemWareIndex++;
                        }
                        else {
                            item.Slot = index;
                            index++;
                        }
                    }

                    return InventoryAddResult.NEW_INDEX;
                case ContainerType.BACK:
                case ContainerType.CHEST_ACCE:
                case ContainerType.FACE:
                case ContainerType.FACE_ACCE:
                case ContainerType.HEAD:
                case ContainerType.HEAD_ACCE:
                case ContainerType.LEFT_HAND:
                case ContainerType.LOWER_BODY:
                case ContainerType.PET:
                case ContainerType.RIGHT_HAND:
                case ContainerType.SHOES:
                case ContainerType.SOCKS:
                case ContainerType.UPPER_BODY:
                case ContainerType.EFFECT:
                    if (Equipments.ContainsKey((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot),
                            container.ToString()))) {
                        if (item.BaseData.itemType != ItemType.BULLET && item.BaseData.itemType != ItemType.ARROW &&
                            item.BaseData.itemType != ItemType.CARD && item.BaseData.itemType != ItemType.THROW) {
                            Logger.ShowDebug("Container:" + container + " must be empty before adding item!",
                                null);
                        }
                        else {
                            if (Equipments[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString())]
                                    .ItemID == item.ItemID)
                                Equipments[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString())]
                                    .Stack += item.Stack;
                            else
                                Equipments[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString())] =
                                    item;
                        }
                    }
                    else {
                        Equipments.Add((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString()), item);
                        LastItem = item;
                        if (newIndex) {
                            item.Slot = index;
                            index++;
                        }
                    }

                    return InventoryAddResult.NEW_INDEX;
                case ContainerType.BACK2:
                case ContainerType.CHEST_ACCE2:
                case ContainerType.FACE2:
                case ContainerType.FACE_ACCE2:
                case ContainerType.HEAD2:
                case ContainerType.HEAD_ACCE2:
                case ContainerType.LEFT_HAND2:
                case ContainerType.LOWER_BODY2:
                case ContainerType.PET2:
                case ContainerType.RIGHT_HAND2:
                case ContainerType.SHOES2:
                case ContainerType.SOCKS2:
                case ContainerType.UPPER_BODY2:
                    var name = container.ToString();
                    name = name.Substring(0, name.Length - 1);
                    if (Parts.ContainsKey((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name))) {
                        if (item.BaseData.itemType != ItemType.BULLET && item.BaseData.itemType != ItemType.ARROW) {
                            Logger.ShowDebug("Container:" + container + " must be empty before adding item!",
                                null);
                        }
                        else {
                            if (Parts[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name)].ItemID == item.ItemID)
                                Parts[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name)].Stack += item.Stack;
                            else
                                Parts[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name)] = item;
                        }
                    }
                    else {
                        Parts.Add((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name), item);
                        LastItem = item;
                        if (newIndex) {
                            item.Slot = index;
                            index++;
                        }
                    }

                    return InventoryAddResult.NEW_INDEX;
                default:
                    throw new ArgumentException("Unsupported container!");
            }
        }

        /// <summary>
        ///     添加道具
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="item">道具</param>
        /// <returns>添加结果，需要注意的只有MIXED，MIXED的话，item则被改为叠加的道具，Inventory.LastItem则是多余的新道具</returns>
        public InventoryAddResult AddItem(ContainerType container, Item item) {
            return AddItem(container, item, true);
        }

        public Item GetItem2() {
            //Logger.getLogger().Error(string.Format("Get:{0}", DBID));
            for (var i = 2; i < 32; i++)
                if (i < 6 || i == 31) {
                    var list = Items[(ContainerType)i];
                    var result = new List<Item>();
                    var query1 =
                        from it in list
                        where it.ChangeMode
                        select it;
                    result = query1.ToList();
                    if (result.Count() == 0) continue;
                    return result.First();
                }

            for (var i = 0; i < 14; i++) {
                if (!Equipments.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Equipments[(EnumEquipSlot)i];
                if (item.ChangeMode)
                    //Logger.getLogger().Error(string.Format("Get1:{0}", item.DBID));
                    return item;
            }

            for (var i = 0; i < 14; i++) {
                if (!Parts.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Parts[(EnumEquipSlot)i];
                if (item.ChangeMode)
                    //Logger.getLogger().Error(string.Format("Get2:{0}", item.DBID));
                    return item;
            }

            return null;
        }

        public Item GetItem(uint ID, SearchType type) {
            for (var i = 2; i < 32; i++)
                if (i < 6 || i == 31) {
                    var list = Items[(ContainerType)i];
                    var result = new List<Item>();

                    switch (type) {
                        case SearchType.ITEM_ID:
                            var query =
                                from it in list
                                where it.ItemID == ID
                                select it;
                            result = query.ToList();
                            break;
                        case SearchType.SLOT_ID:
                            var query1 =
                                from it in list
                                where it.Slot == ID
                                select it;
                            result = query1.ToList();
                            break;
                    }

                    if (result.Count() == 0) continue;
                    return result.First();
                }

            for (var i = 0; i < 14; i++) {
                if (!Equipments.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Equipments[(EnumEquipSlot)i];
                if (type == SearchType.SLOT_ID)
                    if (item.Slot == ID)
                        return item;

                if (type == SearchType.ITEM_ID)
                    if (item.ItemID == ID)
                        return item;
            }

            for (var i = 0; i < 14; i++) {
                if (!Parts.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Parts[(EnumEquipSlot)i];
                if (type == SearchType.SLOT_ID)
                    if (item.Slot == ID)
                        return item;

                if (type == SearchType.ITEM_ID)
                    if (item.ItemID == ID)
                        return item;
            }

            return null;
        }

        public Item GetItem(uint slotID) {
            return GetItem(slotID, SearchType.SLOT_ID);
        }

        public Item GetItem(WarehousePlace place, uint slotID) {
            var query =
                from it in ware[place]
                where it.Slot == slotID
                select it;
            if (query.Count() == 0)
                return null;
            return query.First();
        }

        public InventoryDeleteResult DeleteItem(ContainerType container, uint itemID, int count) {
            return DeleteItem(container, (int)itemID, count, SearchType.ITEM_ID);
        }

        public InventoryDeleteResult DeleteItem(uint slotID, int count) {
            NeedSave = true;
            for (var i = 2; i < 32; i++)
                if (i < 6 || i == 31) {
                    var list = Items[(ContainerType)i];
                    var result = new List<Item>();

                    var query =
                        from it in list
                        where it.Slot == slotID
                        select it;
                    result = query.ToList();

                    if (result.Count() == 0) continue;
                    var item = result.First();
                    if (item.Stack == 0) {
                        list.Remove(item);
                        //Logger.getLogger().Error("0 "+list.Remove(item).ToString()); ;
                        return InventoryDeleteResult.ALL_DELETED;
                    }

                    if (item.Stack > count) {
                        item.Stack -= (ushort)count;
                        return InventoryDeleteResult.STACK_UPDATED;
                    }

                    list.Remove(item);
                    return InventoryDeleteResult.ALL_DELETED;
                }

            for (var i = 0; i < 14; i++) {
                if (!Equipments.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Equipments[(EnumEquipSlot)i];
                if (item.Slot == slotID) {
                    if (item.Stack > count) {
                        item.Stack -= (ushort)count;
                        return InventoryDeleteResult.STACK_UPDATED;
                    }

                    foreach (var j in item.EquipSlot) Equipments.Remove(j);
                    if (Equipments.ContainsKey((EnumEquipSlot)i))
                        Equipments.Remove((EnumEquipSlot)i);
                    return InventoryDeleteResult.ALL_DELETED;
                }
            }

            return InventoryDeleteResult.ALL_DELETED;
        }

        private InventoryDeleteResult DeleteItem(ContainerType container, int ID, int count, SearchType type) {
            switch (container) {
                case ContainerType.BODY:
                case ContainerType.LEFT_BAG:
                case ContainerType.RIGHT_BAG:
                case ContainerType.BACK_BAG:
                    var list = Items[container];
                    var result = new List<Item>();
                    switch (type) {
                        case SearchType.ITEM_ID:
                            var query =
                                from it in list
                                where it.ItemID == ID
                                select it;
                            result = query.ToList();
                            break;
                        case SearchType.SLOT_ID:
                            var query1 =
                                from it in list
                                where it.Slot == ID
                                select it;
                            result = query1.ToList();
                            break;
                    }

                    if (result.Count() == 0) throw new ArgumentException("No such item");
                    var item = result.First();
                    if (item.Stack > count) {
                        item.Stack -= (ushort)count;
                        return InventoryDeleteResult.STACK_UPDATED;
                    }

                    list.Remove(item);
                    return InventoryDeleteResult.ALL_DELETED;
                case ContainerType.BACK:
                case ContainerType.CHEST_ACCE:
                case ContainerType.FACE:
                case ContainerType.FACE_ACCE:
                case ContainerType.HEAD:
                case ContainerType.HEAD_ACCE:
                case ContainerType.LEFT_HAND:
                case ContainerType.LOWER_BODY:
                case ContainerType.PET:
                case ContainerType.RIGHT_HAND:
                case ContainerType.SHOES:
                case ContainerType.SOCKS:
                case ContainerType.UPPER_BODY:
                case ContainerType.EFFECT: {
                    var slotE = (EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString());
                    if (Equipments[slotE].Stack > 1) {
                        Equipments[slotE].Stack--;
                        return InventoryDeleteResult.STACK_UPDATED;
                    }

                    Equipments.Remove(slotE);
                    return InventoryDeleteResult.ALL_DELETED;
                }
                case ContainerType.BACK2:
                case ContainerType.CHEST_ACCE2:
                case ContainerType.FACE2:
                case ContainerType.FACE_ACCE2:
                case ContainerType.HEAD2:
                case ContainerType.HEAD_ACCE2:
                case ContainerType.LEFT_HAND2:
                case ContainerType.LOWER_BODY2:
                case ContainerType.PET2:
                case ContainerType.RIGHT_HAND2:
                case ContainerType.SHOES2:
                case ContainerType.SOCKS2:
                case ContainerType.UPPER_BODY2: {
                    var name = container.ToString();
                    name = name.Substring(0, name.Length - 1);
                    var slotE = (EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name);
                    if (Parts[slotE].Stack > 1) {
                        Parts[slotE].Stack--;
                        return InventoryDeleteResult.STACK_UPDATED;
                    }

                    Parts.Remove(slotE);
                    return InventoryDeleteResult.ALL_DELETED;
                }
            }

            return InventoryDeleteResult.ALL_DELETED;
        }

        public bool MoveItem(ContainerType src, uint itemID, ContainerType dst, int count) {
            return MoveItem(src, (int)itemID, dst, count, SearchType.ITEM_ID);
        }

        public bool MoveItem(ContainerType src, int slotID, ContainerType dst, int count) {
            return MoveItem(src, slotID, dst, count, SearchType.SLOT_ID);
        }

        private bool MoveItem(ContainerType src, int ID, ContainerType dst, int count, SearchType type) {
            try {
                List<Item> list;
                if (src == dst)
                    //Logger.ShowDebug("Source container is equal to Destination container! Transfer aborted!", null);
                    return false;
                switch (src) {
                    case ContainerType.BODY:
                    case ContainerType.LEFT_BAG:
                    case ContainerType.RIGHT_BAG:
                    case ContainerType.BACK_BAG:
                        list = Items[src];
                        break;
                    case ContainerType.BACK:
                    case ContainerType.CHEST_ACCE:
                    case ContainerType.FACE:
                    case ContainerType.FACE_ACCE:
                    case ContainerType.HEAD:
                    case ContainerType.HEAD_ACCE:
                    case ContainerType.LEFT_HAND:
                    case ContainerType.LOWER_BODY:
                    case ContainerType.PET:
                    case ContainerType.RIGHT_HAND:
                    case ContainerType.SHOES:
                    case ContainerType.SOCKS:
                    case ContainerType.UPPER_BODY:
                    case ContainerType.EFFECT:
                        list = new List<Item>();
                        list.Add(Equipments[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), src.ToString())]);
                        Equipments.Remove((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), src.ToString()));
                        break;
                    case ContainerType.BACK2:
                    case ContainerType.CHEST_ACCE2:
                    case ContainerType.FACE2:
                    case ContainerType.FACE_ACCE2:
                    case ContainerType.HEAD2:
                    case ContainerType.HEAD_ACCE2:
                    case ContainerType.LEFT_HAND2:
                    case ContainerType.LOWER_BODY2:
                    case ContainerType.PET2:
                    case ContainerType.RIGHT_HAND2:
                    case ContainerType.SHOES2:
                    case ContainerType.SOCKS2:
                    case ContainerType.UPPER_BODY2:
                        var name = src.ToString();
                        name = name.Substring(0, name.Length - 1);
                        list = new List<Item>();
                        list.Add(Parts[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name)]);
                        Parts.Remove((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name));
                        break;
                    default:
                        throw new ArgumentException("Unsupported Source Container!");
                }

                var result = new List<Item>();
                switch (type) {
                    case SearchType.ITEM_ID:
                        var query =
                            from it in list
                            where it.ItemID == ID
                            select it;
                        result = query.ToList();
                        break;
                    case SearchType.SLOT_ID:
                        var query1 =
                            from it in list
                            where it.Slot == ID
                            select it;
                        result = query1.ToList();
                        break;
                }

                if (result.Count == 0) throw new ArgumentException("The source container doesn't contain such item");

                var oldItem = result.First();
                var newItem = oldItem.Clone();
                if (count > oldItem.Stack || count == 0)
                    count = oldItem.Stack;
                newItem.Stack = (ushort)count;
                oldItem.Stack -= (ushort)count;

                if (oldItem.Stack == 0) {
                    list.Remove(oldItem);
                    newItem.Slot = oldItem.Slot;
                    AddItem(dst, newItem, false);
                    oldItem.Slot = newItem.Slot;
                }
                else {
                    if (oldItem.BaseData.itemType == ItemType.BULLET ||
                        oldItem.BaseData.itemType == ItemType.ARROW) //吞箭bug修复
                        Equipments.Add((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), src.ToString()), oldItem);
                    AddItem(dst, newItem, true);
                }

                return true;
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                return false;
            }
        }

        public List<Item> GetContainer(ContainerType container) {
            switch (container) {
                case ContainerType.BODY:
                case ContainerType.LEFT_BAG:
                case ContainerType.RIGHT_BAG:
                case ContainerType.BACK_BAG:
                case ContainerType.GOLEMWAREHOUSE:
                    return Items[container];
                case ContainerType.BACK:
                case ContainerType.CHEST_ACCE:
                case ContainerType.FACE:
                case ContainerType.FACE_ACCE:
                case ContainerType.HEAD:
                case ContainerType.HEAD_ACCE:
                case ContainerType.LEFT_HAND:
                case ContainerType.LOWER_BODY:
                case ContainerType.PET:
                case ContainerType.RIGHT_HAND:
                case ContainerType.SHOES:
                case ContainerType.SOCKS:
                case ContainerType.UPPER_BODY:
                case ContainerType.EFFECT: {
                    Item item;
                    var newList = new List<Item>();
                    if (Equipments.ContainsKey((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot),
                            container.ToString()))) {
                        item = Equipments[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), container.ToString())];
                        newList.Add(item);
                    }

                    return newList;
                }
                case ContainerType.BACK2:
                case ContainerType.CHEST_ACCE2:
                case ContainerType.FACE2:
                case ContainerType.FACE_ACCE2:
                case ContainerType.HEAD2:
                case ContainerType.HEAD_ACCE2:
                case ContainerType.LEFT_HAND2:
                case ContainerType.LOWER_BODY2:
                case ContainerType.PET2:
                case ContainerType.RIGHT_HAND2:
                case ContainerType.SHOES2:
                case ContainerType.SOCKS2:
                case ContainerType.UPPER_BODY2: {
                    Item item;
                    var newList = new List<Item>();
                    var name = container.ToString();
                    name = name.Substring(0, name.Length - 1);
                    if (Parts.ContainsKey((EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name))) {
                        item = Parts[(EnumEquipSlot)Enum.Parse(typeof(EnumEquipSlot), name)];
                        newList.Add(item);
                    }

                    return newList;
                }
                default:
                    return new List<Item>();
            }
        }

        public ContainerType GetContainerType(uint slotID) {
            for (var i = 2; i < 6; i++) {
                var list = Items[(ContainerType)i];
                var result = new List<Item>();

                var query =
                    from it in list
                    where it.Slot == slotID
                    select it;
                result = query.ToList();

                if (result.Count() == 0) continue;
                return (ContainerType)i;
            }

            for (var i = 0; i < 14; i++) {
                if (!Equipments.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Equipments[(EnumEquipSlot)i];
                if (item.Slot == slotID)
                    return (ContainerType)Enum.Parse(typeof(ContainerType), ((EnumEquipSlot)i).ToString());
            }

            for (var i = 0; i < 14; i++) {
                if (!Parts.ContainsKey((EnumEquipSlot)i))
                    continue;
                var item = Parts[(EnumEquipSlot)i];
                if (item.Slot == slotID)
                    return (ContainerType)Enum.Parse(typeof(ContainerType), ((EnumEquipSlot)i).ToString()) + 200;
            }

            return ContainerType.OTHER_WAREHOUSE;
        }

        public bool IsContainerEquip(ContainerType type) {
            if ((int)type >= (int)ContainerType.HEAD && (int)type <= (int)ContainerType.EFFECT)
                return true;
            return false;
        }

        public bool IsContainerParts(ContainerType type) {
            if ((int)type >= (int)ContainerType.HEAD2 && (int)type <= (int)ContainerType.PET2)
                return true;
            return false;
        }

        private int panelCount(byte page, bool dominion) {
            int cl;
            cl = owner.CL;
            var validCL = cl;
            if (validCL > page * 81) {
                var rest = validCL - page * 81;
                if (rest > 81)
                    return 81;
                return rest;
            }

            return 0;
        }

        public bool[,] validTable(byte page) {
            return validTable(page, owner.InDominionWorld);
        }

        public bool[,] validTable(byte page, bool dominion) {
            var validCount = panelCount(page, dominion);
            bool[,] table;
            int start;
            int x = 3, y = 0;
            int width = 3, height = 3;
            if (page == 0) {
                table = new bool[9, 9] {
                    { true, true, true, false, false, false, false, false, false },
                    { true, true, true, false, false, false, false, false, false },
                    { true, true, true, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false },
                    { false, false, false, false, false, false, false, false, false }
                };
                start = 9;
                x = 3;
                y = 0;
                width = 3;
                height = 3;
            }
            else {
                table = new bool[9, 9];
                start = 0;
                x = 0;
                y = 0;
                width = 0;
                height = 0;
            }

            for (var i = start; i < validCount; i++) {
                table[x, y] = true;
                if (y < height) y++;
                if (y == height)
                    if (x >= width) {
                        width++;
                        if (height == 0)
                            height++;
                        if (x > 0)
                            x = width - 1;
                        else
                            x++;
                        continue;
                    }

                if (x >= 0 && y >= height) x--;
                if (x == -1)
                    if (y >= height) {
                        x = width;
                        height++;
                        if (y > 0)
                            y = 0;
                        else
                            y++;
                    }
            }

            return table;
        }

        public short[,] GetChipList(byte page) {
            return GetChipList(page, owner.InDominionWorld);
        }

        public short[,] GetChipList(byte page, bool dominion) {
            Dictionary<byte, DEMICPanel> chips;
            if (dominion)
                chips = ddemicChips;
            else
                chips = demicChips;

            var res = new short[9, 9];
            if (chips.ContainsKey(page)) {
                if (chips[page].EngageTask1 != 255) {
                    int x, y;
                    x = chips[page].EngageTask1 % 9;
                    y = chips[page].EngageTask1 / 9;
                    res[x, y] = 10000;
                }

                if (chips[page].EngageTask2 != 255) {
                    int x, y;
                    x = chips[page].EngageTask2 % 9;
                    y = chips[page].EngageTask2 / 9;
                    res[x, y] = 10000;
                }

                foreach (var i in chips[page].Chips) res[i.X, i.Y] = i.ChipID;
            }

            return res;
        }

        private int CountChip(short chipID, bool dominion) {
            DEMICPanel[] chips;
            if (dominion)
                chips = ddemicChips.Values.ToArray();
            else
                chips = demicChips.Values.ToArray();

            var res = 0;
            foreach (var i in chips)
                foreach (var j in i.Chips)
                    if (j.ChipID == chipID)
                        res++;

            return res;
        }

        /// <summary>
        ///     尝试插入芯片，成功则返回true，否则返回false
        /// </summary>
        /// <param name="page">DEMIC页</param>
        /// <param name="chip">芯片</param>
        /// <returns>是否成功</returns>
        public bool InsertChip(byte page, Chip chip) {
            return InsertChip(page, chip, owner.InDominionWorld);
        }

        /// <summary>
        ///     尝试插入芯片，成功则返回true，否则返回false
        /// </summary>
        /// <param name="page">DEMIC页</param>
        /// <param name="chip">芯片</param>
        /// <param name="dominion">是否在恶魔界</param>
        /// <returns>是否成功</returns>
        public bool InsertChip(byte page, Chip chip, bool dominion) {
            return InsertChip(page, chip, validTable(page, dominion), dominion);
        }

        /// <summary>
        ///     尝试插入芯片，成功则返回true，否则返回false
        /// </summary>
        /// <param name="page">DEMIC页</param>
        /// <param name="chip">芯片</param>
        /// <param name="table">ＤＥＭＩＣ有效表</param>
        /// <param name="dominion">是否在恶魔界</param>
        /// <returns>是否成功</returns>
        public bool InsertChip(byte page, Chip chip, bool[,] table, bool dominion) {
            var check = false;
            Dictionary<byte, DEMICPanel> chips;
            var chipCount = CountChip(chip.ChipID, dominion);
            if (chipCount >= 10)
                return false;
            if (chip.Data.type == 30 && chipCount >= 1)
                return false;
            if (dominion)
                chips = ddemicChips;
            else
                chips = demicChips;
            if (chips.ContainsKey(page)) {
                byte x1 = 255, y1 = 255, x2 = 255, y2 = 255;

                if (chips[page].EngageTask1 != 255) {
                    x1 = (byte)(chips[page].EngageTask1 % 9);
                    y1 = (byte)(chips[page].EngageTask1 / 9);
                }

                if (chips[page].EngageTask2 != 255) {
                    x2 = (byte)(chips[page].EngageTask2 % 9);
                    y2 = (byte)(chips[page].EngageTask2 / 9);
                }

                foreach (var i in chips[page].Chips) {
                    foreach (var j in chip.Model.Cells) {
                        var X = chip.X + j[0] - chip.Model.CenterX;
                        var Y = chip.Y + j[1] - chip.Model.CenterY;

                        if (!check) {
                            if (x1 != 255 || y1 != 255)
                                if (X == x1 && Y == y1)
                                    return false;
                            if (x2 != 255 || y2 != 255)
                                if (X == x2 && Y == y2)
                                    return false;
                            if (!table[X, Y])
                                return false;
                        }

                        foreach (var k in i.Model.Cells) {
                            var X2 = i.X + k[0] - i.Model.CenterX;
                            var Y2 = i.Y + k[1] - i.Model.CenterY;
                            if (X2 == X && Y2 == Y)
                                return false;
                        }
                    }

                    check = true;
                }

                chips[page].Chips.Add(chip);
                return true;
            }

            return false;
        }

        private int countPossessionItem(List<Item> items) {
            var count = 0;
            foreach (var i in items)
                if (i.PossessionOwner != null)
                    if (i.PossessionOwner.CharID != owner.CharID)
                        count++;

            return count;
        }

        public byte[] ToBytes() {
            var names = Enum.GetNames(typeof(ContainerType));
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(version);
            bw.Write(names.Length);

            foreach (var i in names) {
                var container = (ContainerType)Enum.Parse(typeof(ContainerType), i);
                var list = GetContainer(container);
                bw.Write((int)container);
                bw.Write(list.Count - countPossessionItem(list));
                foreach (var j in list) {
                    if (j.PossessionOwner != null)
                        if (j.PossessionOwner.CharID != owner.CharID)
                            continue;
                    j.ToStream(ms);
                }
            }

            bw.Write((byte)demicChips.Count);
            foreach (var i in demicChips.Keys) {
                bw.Write(i);
                bw.Write(demicChips[i].EngageTask1);
                bw.Write(demicChips[i].EngageTask2);
                bw.Write((byte)demicChips[i].Chips.Count);
                foreach (var j in demicChips[i].Chips) {
                    bw.Write(j.ChipID);
                    bw.Write(j.X);
                    bw.Write(j.Y);
                }
            }

            bw.Write((byte)ddemicChips.Count);
            foreach (var i in ddemicChips.Keys) {
                bw.Write(i);
                bw.Write(ddemicChips[i].EngageTask1);
                bw.Write(ddemicChips[i].EngageTask2);
                bw.Write((byte)ddemicChips[i].Chips.Count);
                foreach (var j in ddemicChips[i].Chips) {
                    bw.Write(j.ChipID);
                    bw.Write(j.X);
                    bw.Write(j.Y);
                }
            }

            ms.Flush();
            return ms.ToArray();
        }

        public void FromStream(Stream ms) {
            try {
                var br = new BinaryReader(ms);
                var _version = br.ReadUInt16();
                if (_version >= 1) {
                    var count = br.ReadInt32();
                    for (var i = 0; i < count; i++) {
                        var type = (ContainerType)br.ReadInt32();
                        var count2 = br.ReadInt32();
                        for (var j = 0; j < count2; j++) {
                            var item = new Item();
                            item.FromStream(ms);
                            if (item.RentalTime > DateTime.Now || !item.Rental)
                                AddItem(type, item);
                        }
                    }
                }

                if (_version >= 2) {
                    demicChips.Clear();
                    ddemicChips.Clear();
                    var count = br.ReadByte();
                    for (var i = 0; i < count; i++) {
                        var page = br.ReadByte();
                        var panel = new DEMICPanel();
                        if (_version >= 3) {
                            panel.EngageTask1 = br.ReadByte();
                            panel.EngageTask2 = br.ReadByte();
                        }

                        var count2 = br.ReadByte();
                        var table = validTable(page, false);
                        demicChips.Add(page, panel);
                        for (var j = 0; j < count2; j++) {
                            Chip chip;
                            var chipID = br.ReadInt16();
                            var x = br.ReadByte();
                            var y = br.ReadByte();
                            if (ChipFactory.Instance.ByChipID.ContainsKey(chipID)) {
                                chip = new Chip(ChipFactory.Instance.ByChipID[chipID]);
                                chip.X = x;
                                chip.Y = y;
                                if (!InsertChip(page, chip, table, false))
                                    Logger.GetLogger().Warning(string.Format(
                                        "Cannot insert chip:{0} for character:{1}, droped!!!", chipID, owner.Name));
                            }
                        }
                    }

                    count = br.ReadByte();
                    for (var i = 0; i < count; i++) {
                        var page = br.ReadByte();
                        var panel = new DEMICPanel();
                        if (_version >= 3) {
                            panel.EngageTask1 = br.ReadByte();
                            panel.EngageTask2 = br.ReadByte();
                        }

                        var table = validTable(page, true);
                        var count2 = br.ReadByte();
                        ddemicChips.Add(page, panel);
                        for (var j = 0; j < count2; j++) {
                            Chip chip;
                            var chipID = br.ReadInt16();
                            var x = br.ReadByte();
                            var y = br.ReadByte();
                            if (ChipFactory.Instance.ByChipID.ContainsKey(chipID)) {
                                chip = new Chip(ChipFactory.Instance.ByChipID[chipID]);
                                chip.X = x;
                                chip.Y = y;
                                if (!InsertChip(page, chip, table, true))
                                    Logger.GetLogger().Warning(string.Format(
                                        "Cannot insert chip:{0} for character:{1}, droped!!!", chipID, owner.Name));
                            }
                        }
                    }

                    if (!demicChips.ContainsKey(100))
                        demicChips.Add(100, new DEMICPanel());
                    if (!demicChips.ContainsKey(101))
                        demicChips.Add(101, new DEMICPanel());
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public byte[] WareToBytes() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(version);
            bw.Write(ware.Count);
            foreach (var i in ware.Keys) {
                var list = ware[i];
                bw.Write((byte)i);
                bw.Write((ushort)list.Count);
                foreach (var j in list) j.ToStream(ms);
            }

            ms.Flush();
            return ms.ToArray();
        }

        public void WareFromSteam(Stream ms) {
            try {
                var br = new BinaryReader(ms);
                var _version = br.ReadUInt16();
                if (_version >= 1) {
                    var count = br.ReadInt32();
                    for (var i = 0; i < count; i++) {
                        var place = (WarehousePlace)br.ReadByte();
                        var count2 = br.ReadUInt16();
                        for (var j = 0; j < count2; j++) {
                            var item = new Item();
                            item.FromStream(ms);
                            AddWareItem(place, item);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }
    }
}