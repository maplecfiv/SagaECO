using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     エナジーボルト:α
    /// </summary>
    public class MagicSP : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, 3.0f);
        }

        #endregion
    }
}