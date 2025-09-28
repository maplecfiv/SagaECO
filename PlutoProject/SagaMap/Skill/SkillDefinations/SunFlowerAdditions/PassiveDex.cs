using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    public class PassiveDex : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, dActor, "PassiveDex", true);
            skill.OnAdditionStart += skill_OnAdditionStart;
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void skill_OnAdditionStart(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.dex_skill += 20;
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.dex_skill -= 20;
        }
    }
}