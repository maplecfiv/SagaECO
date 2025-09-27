using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    /// <summary>
    ///     光之箭（シャイニングアロー）、 黑暗箭（ダークネスアロー）
    /// </summary>
    public class LightDarkArrow : ISkill
    {
        private readonly Elements ArrowElement = Elements.Neutral;

        public LightDarkArrow(Elements e)
        {
            ArrowElement = e;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcBowAndArrow(sActor);
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcArrowDown(sActor);
            var factor = 2.0f + 0.7f * level;
            args.argType = SkillArg.ArgType.Attack;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, ArrowElement, factor);
        }

        #endregion
    }
}