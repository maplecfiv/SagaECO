using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;

namespace SagaMap.Tasks.Partner
{
    public class ReliabilityGrow : MultiRunTask
    {
        private ActorPartner partner;

        public ReliabilityGrow(ActorPartner partner)
        {
            DueTime = 60 * 1000;
            Period = 60 * 1000;
            this.partner = partner;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                //Manager.ExperienceManager.Instance.ApplyPartnerReliabilityEXP(partner, 60);
            }
            catch (Exception exception)
            {
                Logger.GetLogger().Error(exception, null);
                Deactivate();
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}