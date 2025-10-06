using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.Mob
{
    public class DeleteCorpse : MultiRunTask
    {
        private readonly ActorMob npc;

        public DeleteCorpse(ActorMob mob)
        {
            DueTime = 5000;
            Period = 5000;
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