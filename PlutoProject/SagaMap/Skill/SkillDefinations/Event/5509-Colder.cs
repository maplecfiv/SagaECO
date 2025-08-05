using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     魅惑腳踢
    /// </summary>
    public class Colder : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            MapManager.Instance.GetMap(dActor.MapID).SendEffect(dActor, 5089);
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
            {
                var skill = new Freeze(args.skill, act, 5000);
                SkillHandler.ApplyAddition(act, skill);
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
                    realAffected.Add(act);
            }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Water, 2f);
        }

        #endregion
    }
}