using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class ParaizBan : ISkill
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
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, rate))
            {
                var skill = new Stiff(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{

            //    Additions.Global.Stiff skill = new SagaMap.Skill.Additions.Global.Stiff(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }
    }
}