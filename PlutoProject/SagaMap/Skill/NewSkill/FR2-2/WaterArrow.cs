using SagaLib;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR2_2
{
    /// <summary>
    ///     寒冰箭
    /// </summary>
    public class WaterArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcBowAndArrow(pc);
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcArrowDown(sActor);
            float factor = 0;
            factor = 1.3f + 0.2f * level;

            if (level == 6)
            {
                factor = 3.5f;
                var f = new Freeze(args.skill, dActor, 3000);
                SkillHandler.ApplyAddition(dActor, f);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Water, factor);
        }

        #endregion
    }
}