﻿namespace SagaMap.Skill.SkillDefinations.Hawkeye
{
    /// <summary>
    /// リベンジトリガー
    /// </summary>
    public class MissRevenge : ISkill
    {
        #region ISkill 成員

        public int TryCast(SagaDB.Actor.ActorPC sActor, SagaDB.Actor.Actor dActor, SkillArg args)
        {
            return 0;
        }
        public void Proc(SagaDB.Actor.Actor sActor, SagaDB.Actor.Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            DefaultPassiveSkill skill = new DefaultPassiveSkill(args.skill, sActor, "MissRevenge", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += this.StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += this.EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }
        void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.MissRevenge_rate = (byte)(25 + 5 * skill.skill.Level);
        }
        void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.MissRevenge_rate = 0;
        }
        #endregion
    }
}
