using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class MagSlow : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.8f;
            var lifetime = 60000;
            var rate = 20;

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.CannotMove, rate))
            {
                var skill = new MoveSpeedDown(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
            //if (SagaLib.Global.Random.Next(0, 99) < rate)
            //{
            //    SkillHandler.Instance.MagicAttack(sActor, dActor, args, SagaLib.Elements.Neutral, factor);
            //    Additions.Global.MoveSpeedDown skill = new SagaMap.Skill.Additions.Global.MoveSpeedDown(args.skill, dActor, lifetime);
            //    SkillHandler.ApplyAddition(dActor, skill);
            //}
        }
    }
}