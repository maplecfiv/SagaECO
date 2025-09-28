using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm
{
    /// <summary>
    ///     サヴァイブ,GLJOB35
    /// </summary>
    public class Sewaibu : ISkill
    {
        #region ISkill 成員

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "Sewaibu", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.Pressure_lv = skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.Pressure_lv = 0;
        }

        #endregion
    }
}