using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Guardian
{
    /// <summary>
    ///     リフレクトシールド
    /// </summary>
    public class ReflectionShield : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "Reflex", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.reflex_odds = 0.1f * skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.reflex_odds = 0;
        }
    }
}