﻿using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     巨人遊戲
    /// </summary>
    public class EventMaximum : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3000000;
            var skill = new DefaultBuff(args.skill, dActor, "EventMaximum", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC) SkillHandler.Instance.ChangePlayerSize((ActorPC)actor, 2000);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC) SkillHandler.Instance.ChangePlayerSize((ActorPC)actor, 1000);
        }

        #endregion
    }
}