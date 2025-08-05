using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Trader
{
    /// <summary>
    ///     冥想
    /// </summary>
    public class PetMeditation : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Holy, -25);
        }

        #endregion
    }
}