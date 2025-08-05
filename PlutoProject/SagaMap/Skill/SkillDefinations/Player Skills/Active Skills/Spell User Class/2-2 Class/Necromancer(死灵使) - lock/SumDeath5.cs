using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Necromancer
{
    /// <summary>
    ///     死神召喚（死神召喚）[接續技能]
    /// </summary>
    public class SumDeath5 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 6.0f;
            var rate = 50;
            var lifetime = 1000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(dActor, 200, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse,
                            rate))
                    {
                        var skill = new Confuse(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }

                    realAffected.Add(act);
                }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Neutral, factor);
            if (sActor.type == ActorType.MOB)
                try
                {
                    var mob = (ActorMob)sActor;
                    var mobe = (MobEventHandler)mob.e;
                    var Master = mobe.AI.Master;
                    Master.Slave.Remove(mob);
                    mob.ClearTaskAddition();
                    map.DeleteActor(mob);
                }
                catch (Exception)
                {
                }
        }

        #endregion
    }
}