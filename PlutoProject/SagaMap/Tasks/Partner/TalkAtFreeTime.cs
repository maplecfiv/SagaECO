using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.Partner
{
    public class TalkAtFreeTime : MultiRunTask
    {
        private readonly MapClient client;
        private int count;

        public TalkAtFreeTime(MapClient client)
        {
            DueTime = 50000;
            Period = 50000;
            this.client = client;
            count = 0;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                if (client.Character.Partner == null)
                    Deactivate();
                var partner = client.Character.Partner;
                if (count < 10)
                    if (!partner.Status.Additions.ContainsKey("NotAtFreeTime"))
                    {
                        count++;
                        if (Global.Random.Next(0, 100) < 20)
                            client.PartnerTalking(partner, MapClient.TALK_EVENT.NORMAL, 100);
                    }

                if (partner.Status.Additions.ContainsKey("NotAtFreeTime"))
                    count = 0;
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