using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;
using SagaMap.Partner;

namespace SagaMap.Tasks.Partner
{
    public class Feed : MultiRunTask
    {
        private readonly MapClient mc;
        private readonly ActorPartner partner;

        public Feed(MapClient mc, ActorPartner partner, uint nextfeedtime)
        {
            dueTime = (int)(nextfeedtime * 1000);
            period = 5000;
            this.partner = partner;
            this.mc = mc;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                partner.reliabilityuprate = 100;

                partner.Tasks.Remove("Feed");
                Deactivate();

                StatusFactory.Instance.CalcPartnerStatus(partner);
                mc.SendPetBasicInfo();
                mc.SendPetDetailInfo();
                SagaMap.PC.StatusFactory.Instance.CalcStatus(mc.Character);
            }
            catch (Exception)
            {
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}