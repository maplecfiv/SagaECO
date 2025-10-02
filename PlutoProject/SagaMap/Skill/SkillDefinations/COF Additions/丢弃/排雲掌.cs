using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.丢弃
{
    internal class Rowofcloudpalm : MobISkill
    {
        //#region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 1000, false, true);
            var realAffected = new List<Actor>();
            var FixActors = new List<Actor>();
            var NormalActors = new List<Actor>();
            foreach (var act in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            foreach (var item in realAffected)
                if (item.Status.Additions.ContainsKey("Frosen"))
                    FixActors.Add(item);
                else
                    NormalActors.Add(item);

            var arg2 = new SkillArg();
            arg2 = args.Clone();
            SkillHandler.Instance.FixAttack(sActor, FixActors, arg2, Elements.Neutral, 500);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg2, sActor, true);

            SkillHandler.Instance.PhysicalAttack(sActor, NormalActors, args, Elements.Neutral, 1f);
        }

        //#endregion
    }
}