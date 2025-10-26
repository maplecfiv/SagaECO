using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC {
    public class PossessionRecover : MultiRunTask {
        private readonly MapClient client;

        public PossessionRecover(MapClient client) {
            DueTime = 10000;
            Period = 10000;
            this.client = client;
        }

        public override void CallBack() {
            ClientManager.EnterCriticalArea();
            try {
                if (client.Character.PossessionTarget == 0) {
                    client.Character.Tasks.Remove("PossessionRecover");
                    Deactivate();
                    ClientManager.LeaveCriticalArea();
                    return;
                }

                client.Character.HP += (uint)(client.Character.MaxHP * (100 + (client.Character.Vit +
                                                                               client.Character.Status.vit_item +
                                                                               client.Character.Status.vit_rev) / 3) /
                                              1000);
                if (client.Character.HP > client.Character.MaxHP)
                    client.Character.HP = client.Character.MaxHP;
                client.Character.MP += (uint)(client.Character.MaxMP * (100 + (client.Character.Mag +
                                                                               client.Character.Status.mag_item +
                                                                               client.Character.Status.mag_rev) / 3) /
                                              1000);
                if (client.Character.MP > client.Character.MaxMP)
                    client.Character.MP = client.Character.MaxMP;
                client.Character.SP += (uint)(client.Character.MaxSP * (100 + (client.Character.Int +
                                                                               client.Character.Vit +
                                                                               client.Character.Status.int_item +
                                                                               client.Character.Status.int_rev +
                                                                               client.Character.Status.vit_rev +
                                                                               client.Character.Status.vit_item) / 6) /
                                              1000);
                if (client.Character.SP > client.Character.MaxSP)
                    client.Character.SP = client.Character.MaxSP;
                client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, client.Character,
                    true);
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                client.Character.Tasks.Remove("PossessionRecover");
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}