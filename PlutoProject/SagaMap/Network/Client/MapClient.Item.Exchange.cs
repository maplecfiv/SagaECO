using System.Linq;
using SagaDB.Item;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public bool itemexchange;

        public void OnItemExchangeConfirm(CSMG_ITEM_EXCHANGE_CONFIRM p)
        {
            var exchangetype = p.ExchangeType;
            var inventoryid = p.InventorySlot;
            var exchangetargetid = p.ExchangeTargetID;

            var item = Character.Inventory.GetItem(inventoryid);
            if (item == null || item.ItemID == 10000000)
            {
                var p2 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                netIO.SendPacket(p2);
                SendCapacity();
                return;
            }

            if (!ItemExchangeListFactory.Instance.ExchangeList.ContainsKey(item.ItemID))
            {
                var p3 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                netIO.SendPacket(p3);
                SendCapacity();
                return;
            }

            var oriitem = ItemExchangeListFactory.Instance.ExchangeList[item.ItemID].OriItemID;

            var canexchangelist = ExchangeFactory.Instance.ExchangeItems[oriitem];

            if (!canexchangelist.ItemsID.Contains(exchangetargetid) && canexchangelist.OriItemID != exchangetargetid)
            {
                var p4 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
                netIO.SendPacket(p4);
                SendCapacity();
                return;
            }

            var targetitem = ItemFactory.Instance.GetItem(exchangetargetid, true);

            DeleteItem(inventoryid, 1, true);
            AddItem(targetitem, true);
            //Logger.ShowInfo("Receive Item Exchange Request. Type:" + exchangetype + ", itemslot:" + inventoryid + ", targetid:" + exchangetargetid);

            var p1 = new SSMG_ITEM_EXCHANGE_WINDOW_RESET();
            netIO.SendPacket(p1);
            SendCapacity();
        }

        public void OnItemExchangeWindowClose(CSMG_ITEM_EXCHANGE_CLOSE p)
        {
            itemexchange = false;
        }
    }
}