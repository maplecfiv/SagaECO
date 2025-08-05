using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.X
{
    internal class X1 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        #endregion
    }
}