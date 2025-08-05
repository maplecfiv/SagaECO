﻿using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Sorcerer
{
    /// <summary>
    ///     魔力吸收（ディジスト）
    /// </summary>
    public class Desist : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        /*
         * 以受到的魔法傷害恢復MP
         * 目標：自己
         * 施展時間：0.5秒
         * 冷卻時間：3秒
         * 憑依時使用：可
         * Lv       1    2     3     4     5
         * MP消耗   10   20    30    40    50
         * 吸收量   80%  100%  120%  140%  160%
         * 憑依時則恢復被憑依者
         */
        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 300000 + 300000 * level;
            Actor realactor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            var skill = new DefaultBuff(args.skill, realactor, "Desist", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var factor = 60 + 20 * level;
            if (skill.Variable.ContainsKey("Desist"))
                skill.Variable.Remove("Desist");
            skill.Variable.Add("Desist", factor);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("Desist"))
                skill.Variable.Remove("Desist");
        }

        #endregion
    }
}