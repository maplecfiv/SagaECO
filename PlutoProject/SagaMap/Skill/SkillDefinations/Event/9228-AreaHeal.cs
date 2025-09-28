using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     範圍治癒（エリアヒール）
    /// </summary>
    public class AreaHeal : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            level = 5;
            var factor = -(1f + 0.4f * level);
            factor += sActor.Status.Cardinal_Rank;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factor);
        }

        #endregion
    }
}