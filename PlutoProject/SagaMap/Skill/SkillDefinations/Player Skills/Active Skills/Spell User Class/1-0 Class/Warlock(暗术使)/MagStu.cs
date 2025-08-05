using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_
{
    /// <summary>
    ///     マジックスタン
    /// </summary>
    public class MagStun : ISkill
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
                    rate = 42;
                    lifetime = 4000;
                    break;
                case 2:
                    rate = 44;
                    lifetime = 4500;
                    break;
                case 3:
                    rate = 46;
                    lifetime = 5000;
                    break;
                case 4:
                    rate = 48;
                    lifetime = 5500;
                    break;
                case 5:
                    rate = 50;
                    lifetime = 6000;
                    break;
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun, rate))
            {
                var skill = new Stun(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        #endregion
    }
}