using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     無屬性攻擊
    /// </summary>
    public class NormalAttack : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, 1.5f);
        }

        #endregion
    }
}