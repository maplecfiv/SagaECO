using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     芥末問候
    /// </summary>
    public class MobChargeCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 30;
            var lifetime = 3000;
            var factor = 1.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    SkillHandler.Instance.PushBack(sActor, act, 4);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse,
                            rate))
                    {
                        var skill = new Confuse(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, Elements.Neutral, factor);
        }

        #endregion
    }
}