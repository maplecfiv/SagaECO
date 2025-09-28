using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    /// <summary>
    ///     魔法生物迴避
    /// </summary>
    public class ManAvoUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "柔和魔法", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
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