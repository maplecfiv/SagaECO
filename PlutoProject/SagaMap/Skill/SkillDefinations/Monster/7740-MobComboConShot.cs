using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     連續射擊
    /// </summary>
    public class MobComboConShot : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            for (var i = 0; i < 6; i++) args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(7741, level, 700));
        }

        #endregion
    }
}