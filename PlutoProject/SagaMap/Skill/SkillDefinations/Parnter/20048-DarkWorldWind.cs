using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     ダークワールウインド
    /// </summary>
    public class DarkWorldWind : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.3f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Dark, factor);
        }

        #endregion
    }
}