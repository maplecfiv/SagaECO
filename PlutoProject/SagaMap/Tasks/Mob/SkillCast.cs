using System;
using SagaLib;
using SagaMap.Mob;

namespace SagaMap.Tasks.Mob
{
    public class SkillCast : MultiRunTask
    {
        private readonly MobAI client;
        private readonly SkillArg skill;

        public SkillCast(MobAI ai, SkillArg skill)
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
                client.Mob.Tasks.Remove("SkillCast");
                if (skill.argType == SkillArg.ArgType.Cast)
                    client.OnSkillCastComplete(skill);
                client.Mob.TInt["CanNotInterrupted"] = 0;
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