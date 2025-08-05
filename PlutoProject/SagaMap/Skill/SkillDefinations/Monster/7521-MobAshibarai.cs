using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     倒下！
    /// </summary>
    public class MobAshibarai : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.4f; /*0.9f + 0.1f * level;*/
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
            var rate = 80; /*{ 0, 32, 44, 56, 68, 80 };*/
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, rate))
            {
                var skill = new Stiff(args.skill, dActor, 3000); //10秒太恐怖了
                SkillHandler.ApplyAddition(dActor, skill);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}