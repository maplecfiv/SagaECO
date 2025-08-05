﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Explorer
{
    /// <summary>
    ///     合攻（挟撃）
    /// </summary>
    public class PetDogLineatk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.5f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}