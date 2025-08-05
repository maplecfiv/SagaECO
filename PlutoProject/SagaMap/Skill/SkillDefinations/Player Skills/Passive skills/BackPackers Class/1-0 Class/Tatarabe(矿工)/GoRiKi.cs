using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Tatarabe
{
    /// <summary>
    ///     提升重量（ごうりき）
    /// </summary>
    public class GoRiKi : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "GoRiKi", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["GoRiKi"] = 10 * skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["GoRiKi"] = 0;
        }

        #endregion
    }
}