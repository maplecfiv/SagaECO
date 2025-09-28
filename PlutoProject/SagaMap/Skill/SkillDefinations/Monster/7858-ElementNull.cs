using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     属性攻击无效
    /// </summary>
    public class ElementNull : ISkill, MobISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000;
            var skill = new DefaultBuff(args.skill, dActor, "ElementNull", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.ElementDamegeDown_rate = 100;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.ElementDamegeDown_rate = 0;
        }
    }
}