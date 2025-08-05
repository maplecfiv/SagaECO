﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Striker
{
    /// <summary>
    ///     兇狠威脅（バイオレントピック）[接續技能]
    /// </summary>
    public class PetBirdAtk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.7f + 0.3f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}