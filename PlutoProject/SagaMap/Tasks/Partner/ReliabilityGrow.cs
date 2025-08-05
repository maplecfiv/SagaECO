using System;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Tasks.Partner
{
    public class ReliabilityGrow : MultiRunTask
    {
        private ActorPartner partner;

        public ReliabilityGrow(ActorPartner partner)
        {
            dueTime = 60 * 1000;
            period = 60 * 1000;
            this.partner = partner;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                //Manager.ExperienceManager.Instance.ApplyPartnerReliabilityEXP(partner, 60);
            }
            catch (Exception)
            {
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}