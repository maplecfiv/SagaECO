using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Cabalist
{
    /// <summary>
    ///     幽靈術（オールターエゴ）
    /// </summary>
    public class HitAndAway : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "HitAndAway", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}