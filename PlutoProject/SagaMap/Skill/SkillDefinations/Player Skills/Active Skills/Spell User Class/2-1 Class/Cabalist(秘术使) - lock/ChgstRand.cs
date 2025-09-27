using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock
{
    /// <summary>
    ///     森羅萬象（アブレイション）
    /// </summary>
    public class ChgstRand : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.1f + 0.1f * level;
            var rate = 2 * level;
            var lifetime = 4500 + 1000 * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Dark, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun, rate))
            {
                var skill1 = new Stun(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill1);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.鈍足, rate))
            {
                var skill2 = new MoveSpeedDown(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill2);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Silence, rate))
            {
                var skill3 = new Silence(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill3);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.CannotMove, rate))
            {
                var skill4 = new CannotMove(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill4);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Confuse, rate))
            {
                var skill5 = new Confuse(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill5);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Frosen, rate))
            {
                var skill6 = new Freeze(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill6);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Poison, rate))
            {
                var skill7 = new Poison(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill7);
            }
        }

        #endregion
    }
}