using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     提升強化成功率（力量）（強化成功率上昇（力））
    /// </summary>
    public class BoostPower : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "BoostPower", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value = skill.skill.Level;
            if (skill.Variable.ContainsKey("BoostPower"))
                skill.Variable.Remove("BoostPower");
            skill.Variable.Add("BoostPower", value);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("BoostPower"))
                skill.Variable.Remove("BoostPower");
        }

        #endregion
    }
}