using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     刀背擊（みね打ち）
    /// </summary>
    public class MiNeUChi : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] StunRate = { 0, 30, 40, 50, 55, 60 };
            int[] lifetime = { 0, 8000, 7500, 7000, 6500, 6000 };
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun,
                    StunRate[level]))
            {
                var skill = new Stun(args.skill, dActor, lifetime[level]);
                SkillHandler.ApplyAddition(dActor, skill);
            }

            var factor = 1f + 0.1f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}