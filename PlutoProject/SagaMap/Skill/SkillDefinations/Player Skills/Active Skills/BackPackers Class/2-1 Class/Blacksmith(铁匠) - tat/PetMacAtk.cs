﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Blacksmith
{
    /// <summary>
    ///     集中射擊（集中射撃）[接續技能]
    /// </summary>
    public class PetMacAtk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}