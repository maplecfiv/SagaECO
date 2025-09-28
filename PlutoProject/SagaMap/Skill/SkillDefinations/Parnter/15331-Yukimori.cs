namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     黄泉還り
    /// </summary>
    public class Yukimori : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.5f;
            var dmg = SkillHandler.Instance.CalcDamage(true, sActor, dActor, args, SkillHandler.DefType.Def,
                sActor.WeaponElement, sActor.AttackElements[sActor.WeaponElement], factor);
            SkillHandler.Instance.ApplyDamage(sActor, dActor, dmg);
            SkillHandler.Instance.ShowVessel(dActor, dmg);
            SkillHandler.Instance.ApplyDamage(sActor, sActor, -dmg);
            SkillHandler.Instance.ShowVessel(sActor, -dmg);
        }

        #endregion
    }
}