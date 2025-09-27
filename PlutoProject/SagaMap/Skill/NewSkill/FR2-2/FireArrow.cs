using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR2_2
{
    /// <summary>
    ///     火燄箭
    /// </summary>
    public class FireArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcBowAndArrow(pc);
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcArrowDown(sActor);
            var args2 = args.Clone();
            float factor = 0;
            factor = 1.0f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Fire, factor);
            //SkillHandler.Instance.MagicAttack(sActor, dActor, args2, SagaLib.Elements.Fire, factor);
            args.AddSameActor(args2);
        }

        #endregion
    }
}