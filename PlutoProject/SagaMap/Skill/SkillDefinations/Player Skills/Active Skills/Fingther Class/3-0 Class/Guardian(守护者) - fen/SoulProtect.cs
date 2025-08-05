using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Guardian
{
    public class SoulProtect : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = (15 + level * 20) * 1000;
            var skill = new DefaultBuff(args.skill, sActor, "SoulProtect", lifetime);
            skill.OnAdditionStart += StartEvent;
            skill.OnAdditionEnd += EndEvent;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}