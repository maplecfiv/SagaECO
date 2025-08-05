using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Gambler
{
    /// <summary>
    ///     謎（SAGA6）（エニグマ）
    /// </summary>
    public class RandChgstateCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        #endregion
    }
}