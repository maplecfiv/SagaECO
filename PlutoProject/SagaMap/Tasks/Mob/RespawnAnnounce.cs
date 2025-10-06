using System;
using SagaDB.Actor;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Tasks.Mob
{
    public class RespawnAnnounce : MultiRunTask
    {
        private readonly ActorMob mob;

        public RespawnAnnounce(ActorMob mob, int delay)
        {
            DueTime = delay;
            Period = delay;
            this.mob = mob;
        }

        public override void CallBack()
        {
            try
            {
                var eh = (MobEventHandler)mob.e;
                if (eh.AI.Announce != "")
                    foreach (var i in MapClientManager.Instance.OnlinePlayer)
                        i.SendAnnounce(eh.AI.Announce);

                Deactivate();
            }
            catch (Exception)
            {
            }
        }
    }
}