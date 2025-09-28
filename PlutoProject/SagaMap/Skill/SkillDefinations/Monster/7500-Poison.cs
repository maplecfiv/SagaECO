using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class PoisonPerfume : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.type = ATTACK_TYPE.BLOW;
            var factor = 0.8f;
            var lifetime = 5000;
            var rate = 50;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Poison, rate))
            {
                var skill = new Poison(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{

            //    Additions.Global.Poison skill = new SagaMap.Skill.Additions.Global.Poison(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }
    }
}