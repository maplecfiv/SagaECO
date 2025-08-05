using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    /// <summary>
    ///     魔法衝擊波（エナジーショック）
    /// </summary>
    public class MobEnergyShock : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.9f + 0.2f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}