﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Explorer
{
    /// <summary>
    ///     犬牙撕裂（ファングアタック）
    /// </summary>
    public class PetDogSlash : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.5f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}