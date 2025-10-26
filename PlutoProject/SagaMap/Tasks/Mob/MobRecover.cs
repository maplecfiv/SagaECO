using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;

namespace SagaMap.Tasks.Mob {
    public class MobRecover : MultiRunTask {
        private readonly ActorMob mob;
        private byte count;

        public MobRecover(ActorMob mob) {
            DueTime = 1000;
            Period = 1000;
            this.mob = mob;
        }

        public override void CallBack() {
            //
            try {
                ClientManager.EnterCriticalArea();
                var hpadd = 0;
                hpadd = mob.BaseData.resilience;
                mob.HP += (uint)hpadd;
                if (mob.HP > mob.MaxHP) {
                    mob.HP = mob.MaxHP;
                    count = 0;
                }

                MapManager.Instance.GetMap(mob.MapID);
                //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, mob, false);
                ClientManager.LeaveCriticalArea();
                if (mob.HP == mob.MaxHP)
                    count++;
                if (count >= 100)
                    Deactivate();
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                mob.Tasks.Remove("MobRecover");
                Deactivate();
            }
            // 
        }
    }
}