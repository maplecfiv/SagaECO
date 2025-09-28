namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     冥想（瞑想）
    /// </summary>
    public class PetMeditatioon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var HP_ADD = (uint)(sActor.MaxHP * 0.02f * level);
            SkillHandler.Instance.FixAttack(sActor, dActor, args, sActor.WeaponElement, -HP_ADD);
        }

        #endregion
    }
}