using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Tatarabe_矿工_
{
    /// <summary>
    ///     丟石頭（石つぶて）
    /// </summary>
    public class StoneThrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.6f + 0.1f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 18 + 2 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, rate))
            {
                var skill1 = new Stiff(args.skill, dActor, 3000);
                SkillHandler.ApplyAddition(dActor, skill1);
            }
        }

        #endregion
    }
}