using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaMap.Scripting;
//所在地圖:飛空庭的庭院(30201001) NPC基本信息:舵輪(11001436) X:7 Y:13
namespace SagaScript.M30201001
{
    public class S11001436 : Event
    {
        public S11001436()
        {
            this.EventID = 11001436;
        }

        public override void OnEvent(ActorPC pc)
        {
            Say(pc, 11001435, 131, "那个是飞空庭的「舵轮」呀!$R;" +
                                   "$R从飞空庭下来，$R;" +
                                   "或改造飞空庭时使用的。$R;" +
                                   "$R放心吧，$R;" +
                                   "除了本人以外，不会让别人操作的。$R;", "玛莎");
        }
    }
}
