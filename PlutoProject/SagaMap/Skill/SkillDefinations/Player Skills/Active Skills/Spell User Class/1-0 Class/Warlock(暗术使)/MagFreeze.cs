using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Warlock
{
    /// <summary>
    ///     マジックフリーズ
    /// </summary>
    public class MagFreeze : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 0;
            var lifetime = 0;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, 0);
            args.flag[0] = AttackFlag.NONE;
            switch (level)
            {
                case 1:
                    rate = 40;
                    lifetime = 3000;
                    break;
                case 2:
                    rate = 40;
                    lifetime = 4000;
                    break;
                case 3:
                    rate = 40;
                    lifetime = 5000;
                    break;
                case 4:
                    rate = 40;
                    lifetime = 5500;
                    break;
                case 5:
                    rate = 40;
                    lifetime = 6000;
                    break;
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Frosen, rate))
            {
                var skill = new Freeze(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        #endregion
    }
}