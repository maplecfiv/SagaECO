using SagaMap.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SagaDB.Actor;

namespace SagaScript.Scripts
{
    public class TestMapName : Event
    {
        public TestMapName()
        {
            this.EventID = 8500001;
        }

        public override void OnEvent(ActorPC pc)
        {
            Logger.getLogger().Information(pc.SaveMap);
            Logger.getLogger().Information(GetMapName(pc.SaveMap));
        }
    }
}
