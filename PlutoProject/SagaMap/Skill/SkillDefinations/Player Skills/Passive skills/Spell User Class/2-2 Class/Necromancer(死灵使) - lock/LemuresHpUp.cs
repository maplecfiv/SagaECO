using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Necromancer
{
    /// <summary>
    ///     提升召喚對象的HP（召喚対象HP上昇）
    /// </summary>
    public class LemuresHpUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "LemuresHpUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}