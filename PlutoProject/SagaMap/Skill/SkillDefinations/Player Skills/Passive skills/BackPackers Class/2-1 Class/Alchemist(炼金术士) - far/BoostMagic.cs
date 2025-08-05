using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Alchemist
{
    /// <summary>
    ///     提升強化的成功率（魔法）（強化成功率上昇（魔力））
    /// </summary>
    public class BoostMagic : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "BoostMagic", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value = skill.skill.Level;
            if (skill.Variable.ContainsKey("BoostMagic"))
                skill.Variable.Remove("BoostMagic");
            skill.Variable.Add("BoostMagic", value);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("BoostMagic"))
                skill.Variable.Remove("BoostMagic");
        }

        #endregion
    }
}