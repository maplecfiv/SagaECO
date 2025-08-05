using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Druid
{
    /// <summary>
    ///     封印（封印）
    /// </summary>
    public class Seal : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3000 + 1000 * level;
            var skill = new MoveSpeedDown(args.skill, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}