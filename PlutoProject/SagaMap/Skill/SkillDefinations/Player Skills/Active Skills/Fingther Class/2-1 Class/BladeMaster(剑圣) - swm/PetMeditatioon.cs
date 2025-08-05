﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.BladeMaster
{
    /// <summary>
    ///     冥想（瞑想）
    /// </summary>
    public class PetMeditatioon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var HP_ADD = (uint)(sActor.MaxHP * 0.02f * level);
            SkillHandler.Instance.FixAttack(sActor, dActor, args, sActor.WeaponElement, -HP_ADD);
        }

        #endregion
    }
}