using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Tasks.Mob
{
    public class DeleteCorpse : MultiRunTask
    {
        private readonly ActorMob npc;

        public DeleteCorpse(ActorMob mob)
        {
            dueTime = 5000;
            period = 5000;
            npc = mob;
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
            catch (Exception)
            {
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}