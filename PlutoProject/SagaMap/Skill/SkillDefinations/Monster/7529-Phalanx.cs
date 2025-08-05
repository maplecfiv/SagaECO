using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class Phalanx : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.type = ATTACK_TYPE.BLOW;
            var factor = 1.1f;
            var lifetime = 5000;
            var rate = 30;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stiff, rate))
                    {
                        var skill = new Stiff(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{

            //    Additions.Global.Stiff skill = new SagaMap.Skill.Additions.Global.Stiff(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
        }
    }
}