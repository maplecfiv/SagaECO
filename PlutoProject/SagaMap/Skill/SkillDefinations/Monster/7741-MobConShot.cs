using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     連續射擊 [接續技能]
    /// </summary>
    public class MobConShot : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.75f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}