using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC {
    public class CityRecover : MultiRunTask {
        private readonly MapClient client;

        public CityRecover(MapClient client) {
            DueTime = 5000;
            Period = 5000;
            this.client = client;
        }

        public override void CallBack() {
            ClientManager.EnterCriticalArea();
            try {
                if (client.Character.HP < client.Character.MaxHP || client.Character.MP < client.Character.MaxMP ||
                    client.Character.SP < client.Character.MaxSP) {
                    client.Character.HP += (uint)(client.Character.MaxHP * (100 + (client.Character.Vit +
                        client.Character.Status.vit_item +
                        client.Character.Status.vit_rev) / 3) / 2000);
                    if (client.Character.HP > client.Character.MaxHP)
                        client.Character.HP = client.Character.MaxHP;
                    client.Character.MP += (uint)(client.Character.MaxMP * (100 + (client.Character.Mag +
                        client.Character.Status.mag_item +
                        client.Character.Status.mag_rev) / 3) / 2000);
                    if (client.Character.MP > client.Character.MaxMP)
                        client.Character.MP = client.Character.MaxMP;
                    client.Character.SP += (uint)(client.Character.MaxSP * (100 + (client.Character.Int +
                        client.Character.Vit + client.Character.Status.int_item +
                        client.Character.Status.int_rev + client.Character.Status.vit_rev +
                        client.Character.Status.vit_item) / 6) / 2000);
                    if (client.Character.SP > client.Character.MaxSP)
                        client.Character.SP = client.Character.MaxSP;
                    client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, client.Character,
                        true);
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
                client.Character.Tasks.Remove("CityRecover");
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}