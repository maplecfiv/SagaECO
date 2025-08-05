using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Command
{
    /// <summary>
    ///     ラッシュボム（ラッシュボム）[接續技能]
    /// </summary>
    public class RushBomSeq2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 0.25f, 0.45f, 0.65f, 0.85f, 1.05f };
            var factor = factors[level];
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var affected = map.GetActorsArea(sActor, 150, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}