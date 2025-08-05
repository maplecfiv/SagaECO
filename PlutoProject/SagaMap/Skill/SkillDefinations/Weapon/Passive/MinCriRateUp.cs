using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Weapon
{
    /// <summary>
    ///     クリティカル率アップ（小）
    /// </summary>
    public class MinCriRateUp : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "MinCriRateUp", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("MinCriRateUp"))
                skill.Variable.Remove("MinCriRateUp");
            skill.Variable.Add("MinCriRateUp", 5);
            actor.Status.cri_skill += 5;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.cri_skill -= (short)skill.Variable["MinCriRateUp"];
            if (skill.Variable.ContainsKey("MinCriRateUp"))
                skill.Variable.Remove("MinCriRateUp");
        }
    }
}