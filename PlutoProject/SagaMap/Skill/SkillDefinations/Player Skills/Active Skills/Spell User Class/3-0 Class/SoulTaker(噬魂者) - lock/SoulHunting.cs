namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    /// <summary>
    ///     灵魂狩猎(ソウルハント)
    /// </summary>
    public class SoulHunting : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factor = { 0, 12.0f, 3.0f, 17.0f, 3.0f, 14.0f };

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor[level]);
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2545, level, 1000));
        }

        #endregion
    }
}