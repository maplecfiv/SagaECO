using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class WarCry : ISkill
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
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.CannotMove,
                            rate))
                    {
                        var skill = new CannotMove(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{
            //    SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, SagaLib.Elements.Neutral, factor);
            //    Additions.Global.CannotMove skill = new SagaMap.Skill.Additions.Global.CannotMove(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
        }
    }
}