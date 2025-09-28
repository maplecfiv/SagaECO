namespace SagaMap.Skill.SkillDefinations.Event
{
    public class MobSkillCast : ISkill
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