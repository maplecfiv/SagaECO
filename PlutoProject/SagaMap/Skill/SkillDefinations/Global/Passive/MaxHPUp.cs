using System;
using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class MaxHPUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill = new DefaultPassiveSkill(args.skill, sActor, "MaxHPUp", true);
            //设置OnAdditionStart事件处理过程
            skill.OnAdditionStart += StartEventHandler;
            //设置OnAdditionEnd事件处理过程
            skill.OnAdditionEnd += EndEventHandler;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actorParam, DefaultPassiveSkill skill)
        {
            var actor = actorParam as ActorPC;
            int value;
            var factor = new[] { 0, 0.01f, 0.03f, 0.06f, 0.10f, 0.15f }[skill.skill.Level];

            value = (int)(actor.MaxHP * factor);

            var minhpup = new ushort[] { 0, 10, 20, 30, 40, 60 };

            value = Math.Max(value, minhpup[skill.skill.Level]);
            var hpBonus = 1.00f + (double)((actor.Vit + actor.Status.vit_chip + actor.Status.vit_item +
                                            actor.Status.vit_mario + actor.Status.vit_rev + actor.Status.vit_skill) /
                                           200);
            value = (int)Math.Floor(value * hpBonus);
            //将此被动技能所增加的值存到临时变量中，以便取消效果时调用
            if (skill.Variable.ContainsKey("HPValue"))
                skill.Variable.Remove("HPValue");
            skill.Variable.Add("HPValue", value);
            actor.Status.hp_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["HPValue"];
            actor.Status.hp_skill -= (short)value;
        }

        #endregion
    }
}