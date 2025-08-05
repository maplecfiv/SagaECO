using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    public class DancingSword : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2f + level * 0.5f;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.Def, Elements.Neutral, factor);
        }

        #endregion
    }
}