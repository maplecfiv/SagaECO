﻿namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     彈飛
    /// </summary>
    public class MobCharge : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PushBack(sActor, dActor, 4);
        }

        #endregion
    }
}