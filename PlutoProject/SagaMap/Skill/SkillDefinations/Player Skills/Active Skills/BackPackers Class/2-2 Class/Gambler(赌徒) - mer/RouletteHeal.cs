using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     幸福輪盤（ルーレットヒーリング）
    /// </summary>
    public class RouletteHeal : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = -(1.0f + 0.6f * level);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            var healCount = SagaLib.Global.Random.Next(0, realAffected.Count - 1);
            affected.Clear();
            for (var i = 0; i < healCount; i++) affected.Add(realAffected[i]);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Holy, factor);
        }

        #endregion
    }
}