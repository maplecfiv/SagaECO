using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class Corona : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 20.0f;
            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                    affected.Add(item);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Fire, factor);
        }
    }
}