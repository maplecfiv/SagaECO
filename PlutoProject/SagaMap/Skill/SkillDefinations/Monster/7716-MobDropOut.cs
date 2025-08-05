using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     退場！
    /// </summary>
    public class MobDropOut : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PushBack(sActor, dActor, 1);
        }

        #endregion
    }
}