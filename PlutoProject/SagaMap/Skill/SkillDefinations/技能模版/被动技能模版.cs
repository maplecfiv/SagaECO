using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.技能模版
{
    public class 被动 : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "MaxHealMpForWeapon", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //if (actor.type == ActorType.PC)
            //    ((ActorPC)actor).MaxHealMpForWeapon = (short)(20 + 10 * skill.skill.Level);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //if (actor.type == ActorType.PC)
            //    ((ActorPC)actor).MaxHealMpForWeapon = 0;
        }

        //#endregion
    }
}