using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     吹き飛んでください
    /// </summary>
    public class BlowAway : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 5.0f;
            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                    affected.Add(item);
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}