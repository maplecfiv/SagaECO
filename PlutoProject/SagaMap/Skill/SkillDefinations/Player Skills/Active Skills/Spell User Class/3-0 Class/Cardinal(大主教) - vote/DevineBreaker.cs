using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Cardinal
{
    internal class DevineBreaker : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 20.0f + 3.0f * level;
            if (!SkillHandler.Instance.isBossMob(dActor))
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                    factor);
            else
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
        }

        #endregion
    }
}