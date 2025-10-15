using System;
using SagaDB.Actor;
using SagaLib.Tasks;
using SagaMap.Skill;

namespace SagaMap.Tasks.Mob
{
    public class TimingSpeak : MultiRunTask
    {
        private readonly Actor actor;
        private readonly string message;

        public TimingSpeak(Actor actor, int delay, string message)
        {
            DueTime = delay;
            Period = delay;
            this.actor = actor;
            this.message = message;
        }

        public override void CallBack()
        {
            try
            {
                if (actor != null) SkillHandler.Instance.ActorSpeak(actor, message);
                Deactivate();
            }
            catch (Exception exception)
            {
                SagaLib.Logger.getLogger().Error(exception, null);
            }
        }
    }
}