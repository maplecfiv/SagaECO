using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Cabalist
{
    /// <summary>
    ///     黑暗火焰（ダークブレイズ）
    /// </summary>
    public class SelfDarkStorm : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.5f + 0.5f * level;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            var affected = map.GetActorsArea(dActor, (short)(100 * level + 50), false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Dark, factor);
        }

        #endregion
    }
}