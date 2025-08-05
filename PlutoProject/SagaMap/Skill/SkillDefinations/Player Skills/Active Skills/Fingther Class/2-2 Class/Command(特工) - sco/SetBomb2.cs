using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Command
{
    /// <summary>
    ///     定時炸彈（セットボム）[接續技能]
    /// </summary>
    public class SetBomb2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.5f + 0.5f * level;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var affected = map.GetActorsArea(dActor, 150, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    realAffected.Add(act);
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}