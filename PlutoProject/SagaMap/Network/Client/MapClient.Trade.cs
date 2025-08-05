using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        private bool confirmed;
        public bool npcTrade;
        public List<Item> npcTradeItem;
        private bool performed;
        private List<ushort> tradeCounts;
        private List<uint> tradeItems;
        private bool trading;
        private long tradingGold;
        private ActorPC tradingTarget;

        public void OnTradeRequest(CSMG_TRADE_REQUEST p)
        {
            var actor = Map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.PC)
                return;
            var pc = (ActorPC)actor;
            var client = FromActorPC(pc);
            var result = CheckTradeRequest(client);

            if (result == 0)
            {
                tradingTarget = pc;
                client.SendTradeRequest(Character);
            }

            var p1 = new SSMG_TRADE_REQUEST_RESULT();
            p1.Result = result;
            netIO.SendPacket(p1);
        }

        private int CheckTradeRequest(MapClient client)
        {
            if (trading)
                return -1; //トレード中です
            if (scriptThread != null)
                return -2; //イベント中です
            if (client.trading)
                return -3; //相手がトレード中です
            if (client.scriptThread != null)
                return -4; //相手がイベント中です
            if (Character.Golem != null)
                return -7; //ゴーレムショップ起動中です
            if (client.Character.Golem != null)
                return -8; //相手がゴーレムショップ起動中です
            if (Character.PossessionTarget != 0)
                return -9; //憑依中です
            if (client.Character.PossessionTarget != 0)
                return -10; //相手が憑依中です
            if (!client.Character.canTrade)
                return -11; //相手のトレード設定が不許可になっています
            if (Character.Buff.FishingState || Character.Buff.Dead || Character.Buff.Confused ||
                Character.Buff.Frosen || Character.Buff.Paralysis || Character.Buff.Sleep || Character.Buff.Stone ||
                Character.Buff.Stun
                || client.Character.Buff.Dead || client.Character.Buff.Confused || client.Character.Buff.Frosen ||
                client.Character.Buff.Paralysis || client.Character.Buff.Sleep || client.Character.Buff.Stone ||
                client.Character.Buff.Stun)
                return -12; //トレードを行える状態ではありません
            if (Math.Abs(Character.X - client.Character.X) > 300 || Math.Abs(Character.Y - client.Character.Y) > 300)
                return -13; //トレード相手との距離が離れすぎています
            return 0;
        }


        public void OnTradeRequestAnswer(CSMG_TRADE_REQUEST_ANSWER p)
        {
            if (tradingTarget == null)
                return;
            if (tradingTarget.MapID != Character.MapID)
                return;
            var client = FromActorPC(tradingTarget);
            switch (p.Answer)
            {
                case 1:
                    trading = true;
                    client.trading = true;

                    confirmed = false;
                    performed = false;
                    client.confirmed = false;
                    client.performed = false;

                    SendTradeStart();
                    SendTradeStatus(true, false);
                    client.SendTradeStart();
                    client.SendTradeStatus(true, false);
                    break;
                default:
                    tradingTarget = null;
                    client.tradingTarget = null;
                    var p1 = new SSMG_TRADE_REQUEST_RESULT();
                    p1.Result = -6;
                    client.netIO.SendPacket(p1);
                    break;
            }
        }

        private List<ItemType> CP10TypeList()
        {
            var list = new List<ItemType>();
            list.Add(ItemType.FURNITURE);

            return list;
        }

        private long GetGoldForRecycle(List<uint> tradeItems, List<ushort> tradeCounts)
        {
            var zero = ZeroPriceList();
            long gold = 0;
            for (var i = 0; i < this.tradeItems.Count; i++)
            {
                var item = Character.Inventory.GetItem(this.tradeItems[i]).Clone();
                if (item.Stack < this.tradeCounts[i])
                {
                    this.tradeCounts[i] = item.Stack;
                    //SendSystemMessage("你试图通过某种方法修改交易数量！你已经被记录于系统，请联系管理员接受处理。");
                    //Character.Account.Banned = true;
                    var log = new Logger("玩家异常.txt");
                    var logtext = "\r\n" + Character.Name + "使用了交易，修改了数量：" + this.tradeCounts[i] + "/" + item.Stack;
                    log.WriteLog(logtext);
                }

                item.Stack = this.tradeCounts[i];

                var g = item.BaseData.price;
                if (g < 5) g = 10;
                if (zero.Contains(item.ItemID))
                    g = 0;
                if (g > 500) g = 500;
                if (item.BaseData.itemType == ItemType.FURNITURE) //家具类
                    g = 2000;
                if (item.EquipSlot.Count >= 1) //装备类
                {
                    g = (uint)(1000 + 1000 * item.EquipSlot.Count);
                    if (g > 5000)
                        g = 5000;
                }

                gold += g * item.Stack / 100;
            }

            return gold;
        }

        private void OnTradeItemNPC(CSMG_TRADE_ITEM p)
        {
            if (tradeItems != null)
                if (tradeItems.Count != 0)
                {
                    confirmed = false;
                    performed = false;
                    SendTradeStatus(true, false);
                }

            tradeItems = p.InventoryID;
            tradeCounts = p.Count;
            tradingGold = p.Gold;


            var gold = GetGoldForRecycle(tradeItems, tradeCounts);
            var p3 = new SSMG_TRADE_GOLD();
            p3.Gold = 0; // gold;
            netIO.SendPacket(p3);
            tradingGold = gold; // gold;
        }

        public void OnTradeItem(CSMG_TRADE_ITEM p)
        {
            if (npcTrade)
            {
                OnTradeItemNPC(p);
                return;
            }

            if (tradingTarget == null)
                return;
            var client = FromActorPC(tradingTarget);
            if (tradeItems != null)
                if (tradeItems.Count != 0)
                {
                    confirmed = false;
                    client.confirmed = false;
                    performed = false;
                    client.performed = false;
                    SendTradeStatus(true, false);
                    client.SendTradeStatus(true, false);
                }

            tradeItems = p.InventoryID;
            tradeCounts = p.Count;
            tradingGold = p.Gold;
            //if (Character.Account.GMLevel < 200)
            //    tradingGold = 0;
            var p1 = new SSMG_TRADE_ITEM_HEAD();
            client.netIO.SendPacket(p1);
            for (var i = 0; i < tradeItems.Count; i++)
            {
                var p2 = new SSMG_TRADE_ITEM_INFO();
                var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                if (Character.Account.GMLevel < 100)
                    if (item.BaseData.noTrade || item.BaseData.itemType == ItemType.DEMIC_CHIP)
                    {
                        tradeItems[i] = 0;
                        tradeCounts[i] = 0;
                        continue;
                    }

                if (item.PossessionOwner != null)
                    if (item.PossessionOwner.CharID != Character.CharID)
                    {
                        tradeItems[i] = 0;
                        tradeCounts[i] = 0;
                        continue;
                    }

                if (item.Stack < tradeCounts[i])
                    tradeCounts[i] = item.Stack;
                item.Stack = tradeCounts[i];
                p2.Item = item;
                p2.InventorySlot = tradeItems[i];
                p2.Container = ContainerType.BODY;
                Logger.ShowInfo("尝试交易道具:" + item.ItemID + "[" + item.Name + "] " + item.Stack + "个  , 道具栏ID: " +
                                tradeItems[i]);
                client.netIO.SendPacket(p2);
            }

            var p3 = new SSMG_TRADE_GOLD();
            p3.Gold = tradingGold;
            client.netIO.SendPacket(p3);
            var p4 = new SSMG_TRADE_ITEM_FOOT();
            client.netIO.SendPacket(p4);
        }

        public void OnTradeConfirm(CSMG_TRADE_CONFIRM p)
        {
            if (npcTrade)
            {
                switch (p.State)
                {
                    case 0:
                        confirmed = false;
                        break;
                    case 1:
                        confirmed = true;
                        break;
                }

                if (confirmed) SendTradeStatus(false, true);
            }

            if (tradingTarget == null)
                return;
            switch (p.State)
            {
                case 0:
                    confirmed = false;
                    break;
                case 1:
                    confirmed = true;
                    break;
            }

            if (confirmed && FromActorPC(tradingTarget).confirmed)
            {
                SendTradeStatus(false, true);
                FromActorPC(tradingTarget).SendTradeStatus(false, true);
            }
        }

        public void OnTradePerform(CSMG_TRADE_PERFORM p)
        {
            if (npcTrade)
            {
                PerformTradeNPC();
                return;
            }

            if (tradingTarget == null)
                return;
            switch (p.State)
            {
                case 0:
                    performed = false;
                    break;
                case 1:
                    performed = true;
                    break;
            }

            var client = FromActorPC(tradingTarget);
            if (performed && client.performed)
            {
                if (Character.Gold >= tradingGold &&
                    client.Character.Gold >= client.tradingGold &&
                    Character.Gold + client.tradingGold < 10000000000 && //金钱上限为1亿
                    client.Character.Gold + tradingGold < 10000000000
                   )
                {
                    SendTradeEnd(2);
                    PerformTrade();
                    client.SendTradeEnd(2);
                    client.PerformTrade();
                }

                SendTradeEnd(1);
                client.SendTradeEnd(1);
            }
        }

        public void OnTradeCancel(CSMG_TRADE_CANCEL p)
        {
            if (npcTrade)
            {
                npcTradeItem = new List<Item>();
                npcTrade = false;
                SendTradeEnd(3);
                return;
            }

            if (tradingTarget == null)
                return;

            FromActorPC(tradingTarget).SendTradeEnd(3);
            SendTradeEnd(3);
        }

        public List<uint> ZeroPriceList()
        {
            var l = KujiListFactory.Instance.ZeroPriceList;
            return l;
        }

        private void PerformTradeNPC()
        {
            npcTradeItem = new List<Item>();
            SendTradeEnd(2);
            if (tradeItems != null)
                for (var i = 0; i < tradeItems.Count; i++)
                {
                    var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                    item.Stack = tradeCounts[i];
                    Logger.LogItemLost(Logger.EventType.ItemNPCLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("NPCTrade Count:{0}", item.Stack), false);
                    DeleteItem(tradeItems[i], tradeCounts[i], true);
                    npcTradeItem.Add(item);
                }

            //this.Character.Gold -= (int)this.tradingGold;
            if (Character.TInt["垃圾箱记录"] == 1)
            {
                Character.CP += (uint)tradingGold;
                Character.TInt["垃圾箱记录"] = 0;
            }

            //国庆活动
            /*Character.AInt["国庆CP_个人收集"] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.AInt["国庆CP_全服收集"] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.Adict["国庆CP_排行榜"]["国庆CP_" + Character.Account.AccountID.ToString()] += (int)tradingGold;
            ScriptManager.Instance.VariableHolder.AStr["国庆CP_" + Character.Account.AccountID.ToString()] = Character.Name;*/

            SendGoldUpdate();
            performed = true;
            npcTrade = false;

            SendTradeEnd(1);
        }

        public void PerformTrade()
        {
            if (tradingTarget == null)
                return;
            var client = FromActorPC(tradingTarget);
            if (tradeItems != null)
                for (var i = 0; i < tradeItems.Count; i++)
                {
                    if (tradeItems[i] == 0)
                        continue;
                    var item = Character.Inventory.GetItem(tradeItems[i]).Clone();
                    item.Stack = tradeCounts[i];
                    Logger.LogItemLost(Logger.EventType.ItemTradeLost, Character.Name + "(" + Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("Trade Count:{0} To:{1}({2})", tradeCounts[i], client.Character.Name,
                            client.Character.CharID), false);
                    DeleteItem(tradeItems[i], tradeCounts[i], true);
                    Logger.LogItemGet(Logger.EventType.ItemTradeGet,
                        client.Character.Name + "(" + client.Character.CharID + ")",
                        item.BaseData.name + "(" + item.ItemID + ")",
                        string.Format("Trade Count:{0} From:{1}({2})", item.Stack, Character.Name, Character.CharID),
                        false);
                    client.AddItem(item, true);
                }

            Character.Gold -= (int)tradingGold;
            SendGoldUpdate();
            client.Character.Gold += (int)tradingGold;
            client.SendGoldUpdate();
        }

        /// <summary>
        ///     结束交易
        /// </summary>
        /// <param name="type">1，清空变量，2，发送结束封包，3，两个都执行</param>
        public void SendTradeEnd(int type)
        {
            if (type == 1 || type == 3)
            {
                tradeCounts = null;
                tradeItems = null;
                trading = false;
                tradingGold = 0;
                tradingTarget = null;
                confirmed = false;
                performed = false;
            }

            if (type == 2 || type == 3)
            {
                var p = new SSMG_TRADE_END();
                netIO.SendPacket(p);
            }
        }

        public void SendTradeStatus(bool canConfirm, bool canPerform)
        {
            var p = new SSMG_TRADE_STATUS();
            p.Confirm = canConfirm;
            p.Perform = canPerform;
            netIO.SendPacket(p);
        }

        public void SendTradeStart()
        {
            if (tradingTarget == null)
                return;
            var p = new SSMG_TRADE_START();
            p.SetPara(tradingTarget.Name, 0);
            netIO.SendPacket(p);
        }

        public void SendTradeStartNPC(string name)
        {
            if (npcTrade)
            {
                var p = new SSMG_TRADE_START();
                p.SetPara(name, 1);
                netIO.SendPacket(p);
                SendTradeStatus(true, false);
            }
        }

        public void SendTradeRequest(ActorPC pc)
        {
            var p = new SSMG_TRADE_REQUEST();
            p.Name = pc.Name;
            netIO.SendPacket(p);

            tradingTarget = pc;
        }
    }
}