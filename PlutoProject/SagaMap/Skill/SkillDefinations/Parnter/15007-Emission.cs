using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     エミッション
    /// </summary>
    internal class Emission : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 20.0f;
            if (!SkillHandler.Instance.isBossMob(dActor))
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll,
                    sActor.WeaponElement, factor);
            else
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}