using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     這個傢伙！
    /// </summary>
    public class MobTalkSnmnpapa : ISkill
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