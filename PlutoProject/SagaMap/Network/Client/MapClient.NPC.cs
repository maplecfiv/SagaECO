using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaDB.ECOShop;
using SagaDB.Item;
using SagaDB.Npc;
using SagaDB.Quests;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;
using SagaMap.Scripting;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public Event currentEvent;
        private uint currentEventID;
        public Shop currentShop;
        private uint currentVShopCategory;
        public string inputContent;
        public bool npcJobSwitch;
        public bool npcJobSwitchRes;
        public int npcSelectResult;
        public bool npcShopClosed;
        public Thread scriptThread;
        public uint selectedPet;
        public bool syntheseFinished;
        public Dictionary<uint, uint> syntheseItem;
        public bool vshopClosed = Configuration.Instance.VShopClosed;

        public void OnNPCPetSelect(CSMG_NPC_PET_SELECT p)
        {
            selectedPet = p.Result;
        }

        private string ff()
        {
            return Environment.CurrentDirectory;
        }

        public void OnVShopBuy(CSMG_VSHOP_BUY p)
        {
            if (!vshopClosed)
            {
                var items = p.Items;
                var counts = p.Counts;
                var points = new uint[items.Length];
                var rental = new int[items.Length];
                var k = 0;
                uint neededPoints = 0;
                for (var i = 0; i < items.Length; i++)
                {
                    var cat = from item in ECOShopFactory.Instance.Items.Values
                        where item.Items.ContainsKey(items[i])
                        select item;

                    if (cat.Count() > 0)
                    {
                        var category = cat.First();
                        if (counts[i] > 0)
                        {
                            var chip = category.Items[items[i]];
                            points[i] = chip.points;
                            rental[i] = chip.rental;
                        }
                    }
                }

                for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
                if (Character.VShopPoints >= neededPoints)
                {
                    Character.UsedVShopPoints += neededPoints;
                    Character.VShopPoints -= neededPoints;
                    for (k = 0; k < items.Length; k++)
                    {
                        if (counts[k] <= 0)
                            continue;
                        var item = ItemFactory.Instance.GetItem(items[k]);
                        item.Stack = (ushort)counts[k];
                        if (rental[k] > 0)
                        {
                            item.Rental = true;
                            item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                        }

                        Logger.LogItemGet(Logger.EventType.ItemVShopGet, Character.Name + "(" + Character.CharID + ")",
                            item.BaseData.name + "(" + item.ItemID + ")",
                            string.Format("VShopBuy Count:{0}", item.Stack), false);
                        AddItem(item, true);
                    }
                }
            }
        }

        public void OnNCShopBuy(CSMG_NCSHOP_BUY p)
        {
            switch (Character.UsingShopType)
            {
                case PlayerUsingShopType.None:
                    break;
                case PlayerUsingShopType.GShop:
                    HandleGShopBuy(p);
                    break;
                case PlayerUsingShopType.NCShop:
                    HandleNCShopBuy(p);
                    break;
            }
        }

        public void HandleNCShopBuy(CSMG_NCSHOP_BUY p)
        {
            var items = p.Items;
            var counts = p.Counts;
            var points = new uint[items.Length];
            var rental = new int[items.Length];
            var k = 0;
            uint neededPoints = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var cat = from item in NCShopFactory.Instance.Items.Values
                    where item.Items.ContainsKey(items[i])
                    select item;

                if (cat.Count() > 0)
                {
                    var category = cat.First();
                    if (counts[i] > 0)
                    {
                        var chip = category.Items[items[i]];
                        points[i] = chip.points;
                        rental[i] = chip.rental;
                    }
                }
            }

            for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
            if (Character.CP >= neededPoints)
            {
                Character.UsedVShopPoints += neededPoints;
                Character.CP -= neededPoints;
                for (k = 0; k < items.Length; k++)
                {
                    if (counts[k] <= 0)
                        continue;
                    var item = ItemFactory.Instance.GetItem(items[k]);
                    item.Stack = (ushort)counts[k];
                    if (rental[k] > 0)
                    {
                        item.Rental = true;
                        item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                    }

                    Logger.LogItemGet(Logger.EventType.ItemVShopGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("NCShopBuy Count:{0}", item.Stack), false);
                    AddItem(item, true);
                }
            }
        }

        public void HandleGShopBuy(CSMG_NCSHOP_BUY p)
        {
            var items = p.Items;
            var counts = p.Counts;
            var points = new uint[items.Length];
            var rental = new int[items.Length];
            var k = 0;
            uint neededPoints = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var cat = from item in GShopFactory.Instance.Items.Values
                    where item.Items.ContainsKey(items[i])
                    select item;

                if (cat.Count() > 0)
                {
                    var category = cat.First();
                    if (counts[i] > 0)
                    {
                        var chip = category.Items[items[i]];
                        points[i] = chip.points;
                        rental[i] = chip.rental;
                    }
                }
            }

            for (k = 0; k < items.Length; k++) neededPoints += points[k] * counts[k];
            if (Character.Gold >= neededPoints)
            {
                Character.Gold -= neededPoints;
                for (k = 0; k < items.Length; k++)
                {
                    if (counts[k] <= 0)
                        continue;
                    var item = ItemFactory.Instance.GetItem(items[k]);
                    item.Stack = (ushort)counts[k];
                    if (rental[k] > 0)
                    {
                        item.Rental = true;
                        item.RentalTime = DateTime.Now + new TimeSpan(0, rental[k], 0);
                    }

                    Logger.LogItemGet(Logger.EventType.ItemNPCGet, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("GShopBuy Count:{0}", item.Stack), false);
                    AddItem(item, true);
                }
            }
        }

        public void OnNCShopCategoryRequest(CSMG_NCSHOP_CATEGORY_REQUEST p)
        {
            var category = NCShopFactory.Instance.Items[p.Page + 1];
            var p1 = new SSMG_NCSHOP_INFO_HEADER();
            p1.Page = p.Page;
            netIO.SendPacket(p1);
            currentVShopCategory = p.Page + 1;
            foreach (var i in category.Items.Keys)
            {
                var p2 = new SSMG_NCSHOP_INFO();
                p2.Point = category.Items[i].points;
                p2.ItemID = i;
                p2.Comment = category.Items[i].comment;
                netIO.SendPacket(p2);
            }

            var p3 = new SSMG_NCSHOP_INFO_FOOTER();
            netIO.SendPacket(p3);
        }

        public void OnNCShopClose(CSMG_NCSHOP_CLOSE p)
        {
            Character.UsingShopType = PlayerUsingShopType.None;
            vshopClosed = true;
        }

        public void OnVShopClose(CSMG_VSHOP_CLOSE p)
        {
            vshopClosed = true;
        }

        public void OnVShopCategoryRequest(CSMG_VSHOP_CATEGORY_REQUEST p)
        {
            if (!vshopClosed)
            {
                var category = ECOShopFactory.Instance.Items[p.Page + 1];
                var p1 = new SSMG_VSHOP_INFO_HEADER();
                p1.Page = p.Page;
                netIO.SendPacket(p1);
                currentVShopCategory = p.Page + 1;
                foreach (var i in category.Items.Keys)
                {
                    var p2 = new SSMG_VSHOP_INFO();
                    p2.Point = category.Items[i].points;
                    p2.ItemID = i;
                    p2.Comment = category.Items[i].comment;
                    netIO.SendPacket(p2);
                }

                var p3 = new SSMG_VSHOP_INFO_FOOTER();
                netIO.SendPacket(p3);
            }
        }

        public void OnNPCJobSwitch(CSMG_NPC_JOB_SWITCH p)
        {
            if (!npcJobSwitch)
                return;
            npcJobSwitchRes = false;
            if (p.Unknown != 0)
            {
                npcJobSwitchRes = true;
                var item = Character.Inventory.GetItem(Configuration.Instance.JobSwitchReduceItem,
                    Inventory.SearchType.ITEM_ID);
                if (item != null || p.ItemUseCount == 0)
                {
                    if (item != null)
                    {
                        if (item.Stack >= p.ItemUseCount)
                            DeleteItem(item.Slot, (ushort)p.ItemUseCount, true);
                        else
                            return;
                    }

                    Character.SkillsReserve.Clear();
                    //check maximal reservalbe skill count
                    var count = 0;
                    if (Character.Job == Character.Job2X)
                        count = Character.JobLevel2X / 10;
                    if (Character.Job == Character.Job2T)
                        count = Character.JobLevel2T / 10;
                    if (count >= p.Skills.Length)
                        //set reserved skills
                        foreach (var i in p.Skills)
                            if (Character.Skills2.ContainsKey(i))
                                Character.SkillsReserve.Add(i, Character.Skills2[i]);

                    //clear skills
                    ResetSkill(2);

                    //change job and reduce job level
                    var levelLost = 0;
                    if (Character.Job == Character.Job2X)
                    {
                        Character.Job = Character.Job2T;
                        levelLost = (int)(Character.JobLevel2T / 5 - p.ItemUseCount);
                        if (levelLost <= 0)
                            levelLost = 0;
                        if (Character.SkillPoint2T > levelLost)
                            Character.SkillPoint2T -= (ushort)levelLost;
                        else
                            Character.SkillPoint2T = 0;
                        Character.JobLevel2T -= (byte)levelLost;
                        Character.JEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2T, LevelType.JLEVEL2T);
                    }
                    else
                    {
                        Character.Job = Character.Job2X;
                        levelLost = (int)(Character.JobLevel2X / 5 - p.ItemUseCount);
                        if (levelLost <= 0)
                            levelLost = 0;
                        if (Character.SkillPoint2X > levelLost)
                            Character.SkillPoint2X -= (ushort)levelLost;
                        else
                            Character.SkillPoint2X = 0;
                        Character.JobLevel2X -= (byte)levelLost;
                        Character.JEXP =
                            ExperienceManager.Instance.GetExpForLevel(Character.JobLevel2X, LevelType.JLEVEL2);
                    }

                    StatusFactory.Instance.CalcStatus(Character);
                    SendPlayerInfo();

                    var arg = new EffectArg();
                    arg.effectID = 4131;
                    arg.actorID = Character.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, Character, true);
                }
            }

            npcJobSwitch = false;
        }

        public void OnNPCInputBox(CSMG_NPC_INPUTBOX p)
        {
            inputContent = p.Content;
        }

        public void OnNPCShopBuy(CSMG_NPC_SHOP_BUY p)
        {
            var goods = p.Goods;
            var counts = p.Counts;
            if (Character.HP == 0) return;
            if (currentShop != null && goods.Length > 0)
            {
                uint gold = 0;
                switch (currentShop.ShopType)
                {
                    case ShopType.None:
                        gold = (uint)Character.Gold;
                        break;
                    case ShopType.CP:
                        gold = Character.CP;
                        break;
                    case ShopType.ECoin:
                        gold = Character.ECoin;
                        break;
                }

                for (var i = 0; i < goods.Length; i++)
                    if (currentShop.Goods.Contains(goods[i]))
                    {
                        var item = ItemFactory.Instance.GetItem(goods[i]);
                        item.Stack = (ushort)counts[i];
                        short buyrate = 0;
                        if (currentShop.ShopType == ShopType.None)
                            buyrate = Character.Status.buy_rate;
                        var price = (uint)(item.BaseData.price * ((float)(currentShop.SellRate + buyrate) / 200));
                        if (price == 0) price = 1;
                        price = price * item.Stack;
                        if (gold >= price)
                        {
                            var stack = item.Stack;
                            gold -= price;
                            Logger.LogItemGet(Logger.EventType.ItemNPCGet,
                                Character.Name + "(" + Character.CharID + ")",
                                item.BaseData.name + "(" + item.ItemID + ")",
                                string.Format("ShopBuy Count:{0}", item.Stack), false);
                            AddItem(item, true);
                        }
                    }

                switch (currentShop.ShopType)
                {
                    case ShopType.None:
                        Character.Gold = (int)gold;
                        break;
                    case ShopType.CP:
                        Character.CP = gold;
                        break;
                    case ShopType.ECoin:
                        Character.ECoin = gold;
                        break;
                }

                Character.Inventory.CalcPayloadVolume();
                SendCapacity();
            }
            else
            {
                if (currentEvent != null)
                {
                    var gold = Character.Gold;

                    switch (Character.TInt["ShopType"])
                    {
                        case 0:
                            gold = (uint)Character.Gold;
                            break;
                        case 1:
                            gold = Character.CP;
                            break;
                        case 2:
                            gold = Character.ECoin;
                            break;
                    }

                    for (var i = 0; i < goods.Length; i++)
                        if (currentEvent.Goods.Contains(goods[i]))
                        {
                            var item = ItemFactory.Instance.GetItem(goods[i]);
                            item.Stack = (ushort)counts[i];
                            var price = (int)(item.BaseData.price * ((float)Character.Status.buy_rate / 1000));
                            if (price == 0) price = 1;
                            price = price * item.Stack;
                            if (gold >= price)
                            {
                                var stack = item.Stack;
                                gold -= price;
                                Logger.LogItemGet(Logger.EventType.ItemNPCGet,
                                    Character.Name + "(" + Character.CharID + ")",
                                    item.BaseData.name + "(" + item.ItemID + ")",
                                    string.Format("AddItem Count:{0}", item.Stack), false);
                                AddItem(item, true);
                            }
                        }
                    //this.Character.Gold = gold;

                    switch (Character.TInt["ShopType"])
                    {
                        case 0:
                            Character.Gold = gold;
                            break;
                        case 1:
                            Character.CP = (uint)gold;
                            break;
                        case 2:
                            Character.ECoin = (uint)gold;
                            break;
                    }

                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnNPCShopSell(CSMG_NPC_SHOP_SELL p)
        {
            var goods = p.Goods;
            var counts = p.Counts;

            if (currentShop != null)
            {
                uint total = 0;
                for (var i = 0; i < goods.Length; i++)
                {
                    var itemDroped = Character.Inventory.GetItem(goods[i]);
                    if (itemDroped == null)
                        return;
                    if (counts[i] > itemDroped.Stack)
                        counts[i] = itemDroped.Stack;
                    Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                        itemDroped.BaseData.name + "(" + itemDroped.ItemID + ")",
                        string.Format("NPCShopSell Count:{0}", counts[i]), false);

                    DeleteItem(goods[i], (ushort)counts[i], true);

                    var price = (uint)(itemDroped.BaseData.price * counts[i] *
                                       ((float)(10 + Character.Status.sell_rate) / 100));
                    total += price;
                }

                Character.Gold += (int)total;
                Character.Inventory.CalcPayloadVolume();
                SendCapacity();
            }
            else
            {
                if (currentEvent != null)
                {
                    uint total = 0;
                    for (var i = 0; i < goods.Length; i++)
                    {
                        var itemDroped = Character.Inventory.GetItem(goods[i]);
                        if (itemDroped == null)
                            return;
                        Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                            itemDroped.BaseData.name + "(" + itemDroped.ItemID + ")",
                            string.Format("NPCShopSell Count:{0}", counts[i]), false);

                        DeleteItem(goods[i], (ushort)counts[i], true);

                        var price = (uint)(itemDroped.BaseData.price * counts[i] *
                                           ((float)(10 + Character.Status.sell_rate) / 100));

                        total += price;
                    }

                    Character.Gold += (int)total;
                    Character.Inventory.CalcPayloadVolume();
                    SendCapacity();
                }
            }
        }

        public void OnNPCShopClose(CSMG_NPC_SHOP_CLOSE p)
        {
            npcShopClosed = true;
        }

        public void OnNPCSelect(CSMG_NPC_SELECT p)
        {
            npcSelectResult = p.Result;
        }

        public void OnNPCSynthese(CSMG_NPC_SYNTHESE p)
        {
            var ids = p.SynIDs;
            foreach (var item in ids)
                if (!syntheseItem.ContainsKey(ids[item.Key]))
                    syntheseItem.Add(item.Key, item.Value);
        }

        public void OnNPCSyntheseFinish(CSMG_NPC_SYNTHESE_FINISH p)
        {
            syntheseFinished = true;
        }

        public void OnNPCEventStart(CSMG_NPC_EVENT_START p)
        {
            if (scriptThread == null)
            {
                if (tradingTarget != null || trading || Character.Buff.GetReadyPossession)
                {
                    SendEventStart(p.EventID);
                    SendCurrentEvent(p.EventID);
                    SendEventEnd();
                    return;
                }

                //if (p.EventID < 20000000 || p.EventID >= 0xF0000000)Unknow为啥要限制编号
                if (true)
                {
                    if (p.EventID >= 11000000)
                    {
                        if (NPCFactory.Instance.Items.ContainsKey(p.EventID))
                        {
                            var npc = NPCFactory.Instance.Items[p.EventID];
                            uint mapid;
                            if (map.IsMapInstance)
                            {
                                if (map.OriID != 0)
                                    mapid = map.OriID;
                                else
                                    mapid = map.ID * 100 / 1000;
                            }
                            else
                            {
                                mapid = map.ID;
                            }

                            if (npc.MapID == mapid)
                            {
                                if (Math.Abs(Character.X - Global.PosX8to16(npc.X, map.Width)) > 700 ||
                                    Math.Abs(Character.Y - Global.PosY8to16(npc.Y, map.Height)) > 700)
                                {
                                    SendEventStart(p.EventID);
                                    SendCurrentEvent(p.EventID);
                                    SendEventEnd();
                                    return;
                                }
                            }
                            else
                            {
                                SendEventStart(p.EventID);
                                SendCurrentEvent(p.EventID);
                                SendEventEnd();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (p.EventID != 10000315 && p.EventID != 10000316) //Exception for flying garden events
                        {
                            if (map.Info.events.ContainsKey(p.EventID))
                            {
                                var pos = map.Info.events[p.EventID];
                                byte x, y;
                                x = Global.PosX16to8(Character.X, map.Width);
                                y = Global.PosY16to8(Character.Y, map.Height);
                                var valid = false;
                                for (var i = 0; i < pos.Length / 2; i++)
                                    if (Math.Abs(pos[i * 2] - x) <= 3 && Math.Abs(pos[i * 2 + 1] - y) <= 3)
                                    {
                                        valid = true;
                                        break;
                                    }

                                if (!valid)
                                {
                                    SendHack();
                                    SendEventStart(p.EventID);
                                    SendCurrentEvent(p.EventID);
                                    SendEventEnd();
                                    return;
                                }
                            }
                            else
                            {
                                SendHack();
                                SendEventStart(p.EventID);
                                SendCurrentEvent(p.EventID);
                                SendEventEnd();
                                return;
                            }
                        }
                    }

                    EventActivate(p.EventID);
                }
                else
                {
                    SendEventStart(p.EventID);
                    SendCurrentEvent(p.EventID);
                    SendEventEnd();
                }
            }
            else
            {
                SendEventStart(p.EventID);
                SendCurrentEvent(p.EventID);
                SendEventEnd();
            }
        }

        public void EventActivate(uint EventID)
        {
            if (Character.Account.GMLevel > 100)
                SendSystemMessage("触发ID:" + EventID);
            if (ScriptManager.Instance.Events.ContainsKey(EventID))
            {
                var thread = new Thread(RunScript);
                thread.Name = string.Format("ScriptThread({0}) of player:{1}", thread.ManagedThreadId, Character.Name);
                ClientManager.AddThread(thread);
                if (scriptThread != null)
                {
                    Logger.ShowDebug("current script thread != null, currently running:" + currentEventID,
                        Logger.defaultlogger);
                    scriptThread.Abort();
                }

                currentEventID = EventID;
                scriptThread = thread;
                thread.Start();
            }
            else
            {
                SendEventStart(EventID);

                SendCurrentEvent(EventID);

                SendNPCMessageStart();
                if (account.GMLevel > 0)
                    SendNPCMessage(EventID, string.Format(LocalManager.Instance.Strings.NPC_EventID_NotFound, EventID),
                        131, "System Error");
                else
                    SendNPCMessage(EventID,
                        string.Format(LocalManager.Instance.Strings.NPC_EventID_NotFound_Msg, EventID), 131, "");
                SendNPCMessageEnd();
                SendEventEnd();
                Logger.ShowWarning("No script loaded for EventID:" + EventID);
            }
        }

        private void RunScript()
        {
            ClientManager.EnterCriticalArea();
            Event evnt = null;
            try
            {
                evnt = ScriptManager.Instance.Events[currentEventID];
                if (currentEventID < 0xFFFF0000)
                {
                    SendEventStart(currentEventID);
                    SendCurrentEvent(currentEventID);
                }

                currentEvent = evnt;
                currentEvent.CurrentPC = Character;
                var runscript = true;
                if (Character.Quest != null)
                {
                    if (Character.Quest.Detail.NPCSource == evnt.EventID)
                    {
                        if (Character.Quest.CurrentCount1 == 0 && Character.Quest.Status == QuestStatus.OPEN)
                        {
                            Character.Quest.CurrentCount1 = 1;
                            evnt.OnTransportSource(Character);
                            evnt.OnQuestUpdate(Character, Character.Quest);
                            runscript = false;
                        }
                        else
                        {
                            if (Character.Quest.CurrentCount2 == 1)
                            {
                                evnt.OnTransportCompleteSrc(Character);
                                runscript = false;
                            }
                        }
                    }

                    if (Character.Quest.Detail.NPCDestination == evnt.EventID)
                    {
                        if (Character.Quest.CurrentCount2 == 0 && Character.Quest.Status == QuestStatus.OPEN)
                        {
                            evnt.OnTransportDest(Character);
                            if (Character.Quest.CurrentCount3 == 0)
                            {
                                Character.Quest.CurrentCount2 = 1;
                                Character.Quest.Status = QuestStatus.COMPLETED;
                                evnt.OnQuestUpdate(Character, Character.Quest);
                                SendQuestStatus();
                                runscript = false;
                            }
                        }
                        else
                        {
                            evnt.OnTransportCompleteDest(Character);
                            runscript = false;
                        }
                    }
                }

                if (runscript) currentEvent.OnEvent(Character);
                if (currentEventID < 0xFFFF0000)
                    SendEventEnd();
            }
            catch (ThreadAbortException)
            {
                try
                {
                    ClientManager.RemoveThread(scriptThread.Name);
                    ClientManager.LeaveCriticalArea(scriptThread);
                    if (evnt != null)
                        evnt.CurrentPC = null;
                    currentEvent = null;
                    if (Character != null)
                        Logger.ShowWarning(string.Format(
                            "Player:{0} logged out while script thread is still running, terminating the script thread!",
                            Character.Name));
                }
                catch
                {
                }

                scriptThread = null;
            }
            catch (Exception ex)
            {
                try
                {
                    if (Character.Online)
                    {
                        if (Character.Account.GMLevel > 2)
                        {
                            SendNPCMessageStart();
                            SendNPCMessage(currentEventID,
                                "Script Error(" + ScriptManager.Instance.Events[currentEventID] + "):" + ex.Message,
                                131, "System Error");
                            SendNPCMessageEnd();
                        }

                        SendEventEnd();
                    }

                    Logger.ShowWarning("Script Error(" + ScriptManager.Instance.Events[currentEventID] + "):" +
                                       ex.Message + "\r\n" + ex.StackTrace);
                }
                catch
                {
                }
            }

            if (evnt != null)
                evnt.CurrentPC = null;
            scriptThread = null;
            currentEvent = null;
            ClientManager.RemoveThread(Thread.CurrentThread.Name);
            ClientManager.LeaveCriticalArea();
        }

        public void SendEventStart(uint id)
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_EVENT_START();
            netIO.SendPacket(p);
            var p2 = new SSMG_NPC_EVENT_START_RESULT();
            p2.NPCID = id;
            netIO.SendPacket(p2);
        }

        public void SendEventEnd()
        {
            if (!Character.Online)
                return;
            /*string args = "05 F4 00";
            byte[] buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            Packet ps1 = new Packet();
            ps1.data = buf;*/
            //this.netIO.SendPacket(ps1);
            var p = new SSMG_NPC_EVENT_END();
            netIO.SendPacket(p);
        }

        public void SendCurrentEvent(uint eventid)
        {
            var p = new SSMG_NPC_CURRENT_EVENT();
            p.EventID = eventid;
            netIO.SendPacket(p);
        }

        public void SendNPCMessageStart()
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_MESSAGE_START();
            netIO.SendPacket(p);
        }

        public void SendNPCMessageEnd()
        {
            if (!Character.Online)
                return;
            var p = new SSMG_NPC_MESSAGE_END();
            netIO.SendPacket(p);
        }

        public void SendNPCMessage(uint npcID, string message, ushort motion, string title)
        {
            try
            {
                if (!Character.Online)
                    return;
                var p = new SSMG_NPC_MESSAGE();
                if (message.Contains('%'))
                {
                    var newmessage = "";
                    var temp = "";
                    var paras = message.Split('%');
                    for (var i = 0; i < paras.Length; i++)
                    {
                        temp = temp + paras[i];
                        temp = temp.Replace("$P", "");
                        if (i != paras.Length - 1)
                            temp = temp + "$P";
                        newmessage += temp;
                    }

                    message = newmessage;
                }

                if (message.Length > 50)
                {
                    var count = message.Length / 50;
                    var messages = new List<string>();
                    for (var i = 0; i < count; i++)
                        messages.Add(message.Substring(50 * i, 50));
                    if (message.Length != count * 50)
                        messages.Add(message.Substring(count * 50, message.Length - count * 50));
                    foreach (var item in messages)
                    {
                        p = new SSMG_NPC_MESSAGE();
                        p.SetMessage(npcID, 1, item, motion, title);
                        netIO.SendPacket(p);
                    }
                }
                else
                {
                    p.SetMessage(npcID, 1, message, motion, title);
                    netIO.SendPacket(p);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void SendNPCWait(uint wait)
        {
            var p = new SSMG_NPC_WAIT();
            p.Wait = wait;
            netIO.SendPacket(p);
        }

        public void SendNPCPlaySound(uint soundID, byte loop, uint volume, byte balance)
        {
            SendNPCPlaySound(soundID, loop, volume, balance, false);
        }

        public void SendNPCPlaySound(uint soundID, byte loop, uint volume, byte balance, bool stopBGM)
        {
            var p = new SSMG_NPC_PLAY_SOUND();
            if (stopBGM) p.ID = 0x05EE;
            p.SoundID = soundID;
            p.Loop = loop;
            p.Volume = volume;
            p.Balance = balance;
            netIO.SendPacket(p);
        }

        public void SendChangeBGM(uint soundID, byte loop, uint volume, byte balance)
        {
            var p = new SSMG_NPC_CHANGE_BGM();
            p.SoundID = soundID;
            p.Loop = loop;
            p.Volume = volume;
            p.Balance = balance;
            netIO.SendPacket(p);
        }

        public void SendNPCShowEffect(uint actorID, byte x, byte y, ushort height, uint effectID, bool oneTime)
        {
            var p = new SSMG_NPC_SHOW_EFFECT();
            p.ActorID = actorID;
            p.EffectID = effectID;
            p.X = x;
            p.Y = y;
            p.height = height;
            p.OneTime = oneTime;
            netIO.SendPacket(p);
        }

        public void SendNPCStates()
        {
            var AllInvolvedNPCStates =
                (from npc in NPCFactory.Instance.Items.Values where npc.MapID == Character.MapID select npc)
                .ToDictionary(i => i.ID, i => false);
            for (var i = 0; i < AllInvolvedNPCStates.Count; i++)
            {
                var npcid = AllInvolvedNPCStates.Keys.ElementAt(i);
                if (Character.NPCStates.ContainsKey(npcid))
                    AllInvolvedNPCStates[npcid] = Character.NPCStates[npcid];
            }

            var unloadedCount = AllInvolvedNPCStates.Count;
            var loadedCount = 0;
            var pages = new List<Dictionary<uint, bool>>();
            while (unloadedCount > 0)
                if (unloadedCount > 100)
                {
                    pages.Add(AllInvolvedNPCStates.Skip(loadedCount).Take(100).ToDictionary(i => i.Key, i => i.Value));
                    loadedCount += 100;
                    unloadedCount -= 100;
                }
                else
                {
                    pages.Add(AllInvolvedNPCStates.Skip(loadedCount).Take(unloadedCount)
                        .ToDictionary(i => i.Key, i => i.Value));
                    loadedCount += unloadedCount;
                    unloadedCount = 0;
                }

            foreach (var subpage in pages)
            {
                var p = new SSMG_NPC_STATES();
                p.PutNPCStates(subpage);
                netIO.SendPacket(p);
            }
            /*
            if (this.Character.NPCStates.ContainsKey(this.map.ID))
            {
                foreach (uint i in this.chara.NPCStates[this.map.ID].Keys)
                {
                    if (this.chara.NPCStates[this.map.ID][i])
                    {
                        Packets.Server.SSMG_NPC_SHOW p = new SagaMap.Packets.Server.SSMG_NPC_SHOW();
                        p.NPCID = i;
                        this.netIO.SendPacket(p);
                    }
                    else
                    {
                        Packets.Server.SSMG_NPC_HIDE p = new SagaMap.Packets.Server.SSMG_NPC_HIDE();
                        p.NPCID = i;
                        this.netIO.SendPacket(p);
                    }
                }
            }
            */
        }
    }
}