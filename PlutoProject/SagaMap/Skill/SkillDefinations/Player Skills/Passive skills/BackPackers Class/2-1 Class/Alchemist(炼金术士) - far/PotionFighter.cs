using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Alchemist
{
    /// <summary>
    ///     提升藥水效果（ポーション効果上昇）
    /// </summary>
    public class PotionFighter : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "PotionFighter", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var factor = new[] { 0, 0.14f, 0.17f, 0.2f, 0.25f, 0.33f }[skill.skill.Level];
            if (skill.Variable.ContainsKey("PotionFighter"))
                skill.Variable.Remove("PotionFighter");
            skill.Variable.Add("PotionFighter", (int)(factor * 100));
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("PotionFighter"))
                skill.Variable.Remove("PotionFighter");
        }

        #endregion
    }
}