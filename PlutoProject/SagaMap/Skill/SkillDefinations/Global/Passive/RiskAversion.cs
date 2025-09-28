using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    /// <summary>
    ///     迴避危險（危険回避）
    /// </summary>
    public class RiskAversion : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "RiskAversion", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.possessionCancel = (short)(10 * skill.skill.Level);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.possessionCancel = 0;
        }

        #endregion
    }
}