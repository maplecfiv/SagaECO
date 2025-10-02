using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     X 領域（エキスパンド）
    /// </summary>
    public class ExpUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 300000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                {
                    var skill = new Additions.ExpUp(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        //#endregion
    }
}