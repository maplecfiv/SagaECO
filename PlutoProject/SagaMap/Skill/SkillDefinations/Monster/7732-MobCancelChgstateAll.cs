using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     驅逐
    /// </summary>
    public class MobCancelChgstateAll : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 30;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (SagaLib.Global.Random.Next(0, 99) < rate)
                    {
                        var WillBeRemove = new List<Addition>();

                        foreach (var s in act.Status.Additions)
                            if (!(s.Value is DefaultPassiveSkill))
                            {
                                var addition = s.Value;
                                WillBeRemove.Add(addition);
                            }

                        foreach (var i in WillBeRemove)
                            if (i.Activated)
                                SkillHandler.RemoveAddition(act, i);
                    }
        }

        #endregion
    }
}