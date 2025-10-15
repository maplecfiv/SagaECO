using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.Partner
{
    public class DeleteCorpse : MultiRunTask
    {
        private readonly ActorPartner npc;

        public DeleteCorpse(ActorPartner partner)
        {
            DueTime = 5000;
            Period = 5000;
            npc = partner;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                npc.Tasks.Remove("DeleteCorpse");
                MapManager.Instance.GetMap(npc.MapID).DeleteActor(npc);
                Deactivate();
            }
            catch (Exception exception)
            {
                Logger.getLogger().Error(exception, null);
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}