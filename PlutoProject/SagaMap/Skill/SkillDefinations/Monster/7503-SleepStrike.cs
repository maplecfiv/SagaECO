using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class SleepStrike : ISkill
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
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Silence, rate))
            {
                var skill = new Silence(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{

            //    Additions.Global.Silence skill = new SagaMap.Skill.Additions.Global.Silence(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
        }
    }
}