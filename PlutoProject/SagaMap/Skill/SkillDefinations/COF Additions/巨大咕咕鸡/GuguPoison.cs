using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.巨大咕咕鸡
{
    internal class GuguPoison : MobISkill
    {
        //#region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 500, false, true);
            var realAffected = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var Poison = new Poison(args.skill, act, 10000);
                    SkillHandler.ApplyAddition(act, Poison);
                }
        }

        //#endregion
    }
}