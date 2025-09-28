using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     植物停頓（プラントヒーリング）
    /// </summary>
    public class PetPlantHealing : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = -(1.0f + 0.4f * level);
            SkillHandler.Instance.MagicAttack(sActor, sActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy, 50,
                factor);
        }

        #endregion
    }
}