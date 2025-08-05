using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class SpRecover : MultiRunTask
    {
        private readonly MapClient client;

        public SpRecover(MapClient client)
        {
            dueTime = 3000;
            period = 3000;
            this.client = client;
        }

        public override void CallBack()
        {
            if (client != null && client.Character != null)
            {
                ClientManager.EnterCriticalArea();
                try
                {
                    if (!(client.Character.Job == PC_JOB.CARDINAL || client.Character.Job == PC_JOB.FORCEMASTER))
                    {
                        client.Character.Tasks.Remove("EpRecover");
                        Deactivate();
                    }
                    else if (client.Character.EP != client.Character.MaxEP)
                    {
                        client.Character.EP += 100;
                        if (client.Character.EP > client.Character.MaxEP)
                            client.Character.EP = client.Character.MaxEP;
                        client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null,
                            client.Character, true);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                    client.Character.Tasks.Remove("EpRecover");
                    Deactivate();
                }

                ClientManager.LeaveCriticalArea();
            }
        }
    }
}