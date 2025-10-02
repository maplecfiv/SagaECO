using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     蜂針
    /// </summary>
    public class MobPerfectcritical : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            var actors = new List<Actor>();
            actors.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, actors, args, SkillHandler.DefType.IgnoreAll, Elements.Neutral,
                0, factor, false);
        }

        //#endregion
    }
}