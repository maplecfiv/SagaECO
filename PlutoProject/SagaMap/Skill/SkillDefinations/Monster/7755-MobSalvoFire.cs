using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     解除霍汀克導彈!
    /// </summary>
    public class MobSalvoFire : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.58f;
            short range = 300;
            var times = 8;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, range, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    for (var i = 0; i < times; i++)
                        realAffected.Add(act);

            if (realAffected.Count > 0)
            {
                var finalAffected = new List<Actor>();
                for (var i = 0; i < realAffected.Count; i++)
                    finalAffected.Add(realAffected[SagaLib.Global.Random.Next(0, realAffected.Count - 1)]);
                SkillHandler.Instance.PhysicalAttack(sActor, finalAffected, args, Elements.Neutral, factor);
            }
        }

        //#endregion
    }
}