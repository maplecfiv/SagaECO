using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Royaldealer
{
    internal class TimeIsMoney : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pc = sActor as ActorPC;
            var factor = new[] { 0, 5.0f, 6.0f, 7.0f, 8.5f, 10.0f }[level];
            if (pc.Gold >= 90000000)
                factor += 25.0f;
            else if (pc.Gold >= 50000000)
                factor += 22.0f;
            else if (pc.Gold >= 5000000)
                factor += 19.0f;
            else if (pc.Gold >= 500000)
                factor += 14.0f;
            else if (pc.Gold >= 50000)
                factor += 10.0f;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var acts = map.GetActorsArea(dActor, 250, true);
            var realaffected = new List<Actor>();
            foreach (var item in acts)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                    realaffected.Add(item);
            SkillHandler.Instance.PhysicalAttack(sActor, realaffected, args, SkillHandler.DefType.Def,
                sActor.WeaponElement, 0, factor, false, 0, false, 20, 0);
        }
    }
}