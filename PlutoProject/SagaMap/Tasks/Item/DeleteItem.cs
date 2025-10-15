using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.Item
{
    public class DeleteItem : MultiRunTask
    {
        private readonly ActorItem npc;

        public DeleteItem(ActorItem item)
        {
            DueTime = 60000;
            Period = 60000;
            npc = item;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                npc.Tasks.Remove("DeleteItem");
                MapManager.Instance.GetMap(npc.MapID).DeleteActor(npc);
                Deactivate();
            }
            catch (Exception exception)
            {
                Logger.getLogger().Error(exception, null);
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}