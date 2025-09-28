using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    public class DelayOut : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("DelayOut"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultBuff(args.skill, sActor, "DelayOut", 10000);
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            skill.OnAdditionStart += skill_OnAdditionStart;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void skill_OnAdditionStart(Actor actor, DefaultBuff skill)
        {
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultBuff skill)
        {
        }
    }
}