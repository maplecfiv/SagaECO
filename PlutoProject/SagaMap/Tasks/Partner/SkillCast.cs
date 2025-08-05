using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Partner;

namespace SagaMap.Tasks.Partner
{
    public class SkillCast : MultiRunTask
    {
        private readonly PartnerAI client;
        private readonly SkillArg skill;

        public SkillCast(PartnerAI ai, SkillArg skill)
        {
            if (skill.argType == SkillArg.ArgType.Cast)
            {
                dueTime = (int)skill.delay;
                period = (int)skill.delay;
            }

            client = ai;
            this.skill = skill;
        }

        public override void CallBack()
        {
            try
            {
                ClientManager.EnterCriticalArea();
                client.Partner.Tasks.Remove("SkillCast");
                if (skill.argType == SkillArg.ArgType.Cast)
                    client.OnSkillCastComplete(skill);
                Deactivate();
                ClientManager.LeaveCriticalArea();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                Deactivate();
            }
        }
    }
}