using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class Marionette : MultiRunTask
    {
        private readonly MapClient client;

        public Marionette(MapClient client, int duration)
        {
            dueTime = duration * 1000;
            period = duration * 1000;
            this.client = client;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                client.MarionetteDeactivate();
                if (client.Character.Tasks.ContainsKey("Marionette"))
                    client.Character.Tasks.Remove("Marionette");
                Deactivate();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}