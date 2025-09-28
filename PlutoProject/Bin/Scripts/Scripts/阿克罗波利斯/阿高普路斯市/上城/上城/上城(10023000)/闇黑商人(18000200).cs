using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaMap.Scripting;
//所在地圖:上城(10023000) NPC基本信息:闇黑商人(18000200) X:133 Y:147
namespace SagaScript.M10023000
{
    public class S11001692 : Event
    {
        public S11001692()
        {
            this.EventID = 11001692;
        }

        public override void OnEvent(ActorPC pc)
        {
            switch (Select(pc, "买东西吗？", "", "买", "不买"))
            {
                case 1:
                    OpenShopBuy(pc, 414);
                    break;
            }
        }
    }
}
