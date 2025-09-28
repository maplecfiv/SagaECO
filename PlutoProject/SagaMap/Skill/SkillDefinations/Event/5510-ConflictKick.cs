using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     魅惑腳踢
    /// </summary>
    public class ConflictKick : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, 2f);
        }

        #endregion
    }
}