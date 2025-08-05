using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     こんどはエコの番！
    /// </summary>
    public class EcoDay : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.0f;
            SkillHandler.Instance.PhysicalAttack(sActor, new List<Actor> { dActor }, args, SkillHandler.DefType.Def,
                sActor.WeaponElement, 0, factor, false, 0, false, 0, 100);
        }
    }
}