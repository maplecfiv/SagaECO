using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Merchant
{
    /// <summary>
    ///     提升體積（パッキング）
    /// </summary>
    public class Packing : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "Packing", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["Packing"] = 10 * skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["Packing"] = 10 * skill.skill.Level;
        }

        #endregion
    }
}