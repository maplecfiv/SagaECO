using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     DEM龙用灵魂审判
    /// </summary>
    public class HumanSeeleGuerrilla : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 50; //也许设置概率改为50%更合适?
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Silence,
                            rate))
                    {
                        var skill = new Silence(args.skill, act, 3000);
                        SkillHandler.ApplyAddition(act, skill);
                    }

                    if (act.type == ActorType.PC)
                        if (SagaLib.Global.Random.Next(0, 99) < rate)
                            SkillHandler.Instance.PossessionCancel((ActorPC)act, PossessionPosition.NONE);
                }
        }

        #endregion
    }
}