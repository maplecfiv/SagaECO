using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     範圍射擊
    /// </summary>
    public class MobCircleSelfAtk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, Elements.Neutral, factor);
        }

        #endregion
    }
}