﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Marionette
{
    /// <summary>
    ///     亢奮一踢
    /// </summary>
    public class MCharge3 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PushBack(sActor, dActor, 6);
        }

        #endregion
    }
}