using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class Rush : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.75f;
            //List<Actor> target = new List<Actor>();
            //target.Add(dActor);
            //target.Add(dActor);
            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.SLASH;
            SkillHandler.Instance.PushBack(sActor, dActor, 4);
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }
    }
}