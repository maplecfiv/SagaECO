using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class CityDown : MultiRunTask
    {
        private readonly MapClient client;

        public CityDown(MapClient client)
        {
            DueTime = 5000;
            Period = 5000;
            this.client = client;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                if (client.Character.HP > 5)
                    client.Character.HP -= 5;
                else
                    client.Character.HP = 1;
                client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, client.Character,
                    true);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                client.Character.Tasks.Remove("CityDown");
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}