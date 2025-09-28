using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     憤怒的錘子
    /// </summary>
    public class MobHpPerDown : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = dActor.HP * 0.1f;
            SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}