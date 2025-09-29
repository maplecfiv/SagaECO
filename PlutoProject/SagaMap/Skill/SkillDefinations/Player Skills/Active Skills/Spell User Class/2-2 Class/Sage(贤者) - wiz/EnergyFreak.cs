using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     能量轉移（エナジーフリーク）
    /// </summary>
    public class EnergyFreak : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = new[] { 0, 4500, 5300, 6500, 7300, 8500 }[level];
            var factor = 1.0f + 0.4f * level;
            var rate = 10 * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Silence, rate))
            {
                var skill3 = new Silence(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill3);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Poison, rate))
            {
                var skill5 = new Poison(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill5);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stone, rate))
            {
                var skill4 = new Stone(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill4);
            }
        }

        #endregion
    }
}