using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     連發射擊
    /// </summary>
    public class MobConArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.8f;
            var realAffected = new List<Actor>();
            realAffected.Add(dActor);
            realAffected.Add(dActor);
            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, Elements.Neutral, factor);
        }

        #endregion
    }
}