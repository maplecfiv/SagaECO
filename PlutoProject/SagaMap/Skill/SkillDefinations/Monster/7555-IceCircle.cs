using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class IceCircle : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000;
            args.dActor = 0;
            var rate = 30;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            foreach (var i in realAffected)
                if (SkillHandler.Instance.CanAdditionApply(sActor, i, SkillHandler.DefaultAdditions.Frosen, rate))
                {
                    var skill = new Freeze(args.skill, i, lifetime);
                    SkillHandler.ApplyAddition(i, skill);
                }
        }
    }
}