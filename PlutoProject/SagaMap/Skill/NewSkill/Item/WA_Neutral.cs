﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Item
{
    public class WA_Neutral : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 3;

            //SkillHandler.Instance.WeaponMagicAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}