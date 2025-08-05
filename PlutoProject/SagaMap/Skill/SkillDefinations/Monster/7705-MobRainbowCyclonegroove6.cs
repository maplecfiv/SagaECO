using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     彩虹黑暗子彈
    /// </summary>
    public class MobRainbowCyclonegroove6 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}