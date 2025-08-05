using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Necromancer
{
    /// <summary>
    ///     死神召喚（死神召喚）[接續技能]
    /// </summary>
    public class SumDeath2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}