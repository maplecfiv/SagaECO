using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    public class PoisonMaster : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, dActor, "PoisonMaster", true);
            skill.OnAdditionStart += skill_OnAdditionStart;
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void skill_OnAdditionStart(Actor actor, DefaultPassiveSkill skill)
        {
        }
    }
}