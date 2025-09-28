namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     デモリッション
    /// </summary>
    public class Demolition : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 18.0f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}