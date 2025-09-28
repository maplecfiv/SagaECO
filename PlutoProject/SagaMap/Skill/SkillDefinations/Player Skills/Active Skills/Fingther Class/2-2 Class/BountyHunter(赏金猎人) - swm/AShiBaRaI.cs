using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     絆腿（足払い）
    /// </summary>
    public class AShiBaRaI : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.9f + 0.1f * level;
            uint MartialArtDamUp_SkillID = 125;
            var actorPC = (ActorPC)sActor;
            if (actorPC.Skills2.ContainsKey(MartialArtDamUp_SkillID))
                if (actorPC.Skills2[MartialArtDamUp_SkillID].Level == 3)
                    factor = 1.32f + 0.1f * level;

            if (actorPC.SkillsReserve.ContainsKey(MartialArtDamUp_SkillID))
                if (actorPC.SkillsReserve[MartialArtDamUp_SkillID].Level == 3)
                    factor = 1.32f + 0.1f * level;

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            int[] rate = { 0, 5, 10, 15, 20, 25, 30 };
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff,
                    rate[level]))
            {
                var skill = new Stiff(args.skill, dActor, 10000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        #endregion
    }
}