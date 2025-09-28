namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     各種精靈的憤怒
    /// </summary>
    public class MobElementRandcircle : ISkill
    {
        private readonly int Count;
        private readonly uint NextSkillID;

        public MobElementRandcircle(uint NextID, int count)
        {
            NextSkillID = NextID;
            Count = count;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            for (var i = 0; i < Count; i++)
                args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, level, 0));
        }

        #endregion
    }
}