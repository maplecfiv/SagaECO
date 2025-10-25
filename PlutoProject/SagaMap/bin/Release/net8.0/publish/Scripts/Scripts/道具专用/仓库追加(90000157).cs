using System;
using System.Collections.Generic;
using System.Text;
using SagaLib;
using SagaDB;
using SagaDB.Actor;
using SagaMap.Network.Client;
using SagaMap.Scripting;

using SagaScript.Chinese.Enums;
//アイテム名：いつでも仓库　アイテムID:10065100
namespace SagaMap
{
    public class S90000157 : Event
    {
        public S90000157()
        {
            this.EventID = 90000157;
        }

        public override void OnEvent(ActorPC pc)
        {
            switch (Select(pc, "选择追加地点", "", "阿克罗波利斯", "什么都不做"))
            {
                case 1:
                    if (pc.Account.AccountID == 914)
                    {
                        Packets.Server.SSMG_ITEM_WARE_PUT_RESULT p = new SagaMap.Packets.Server.SSMG_ITEM_WARE_PUT_RESULT();
                        p.Result = -7;
                        MapClient.FromActorPC(pc).netIO.SendPacket(p);

                    }
                    else
                    {
                        TakeItem(pc, 10065100, 1);
                        OpenWareHouse(pc, SagaDB.Item.WarehousePlace.Acropolis);
                    }
                    break;
            }
        }
    }
}
