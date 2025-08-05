﻿using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     發射導彈（アンチモブミサイル）
    /// </summary>
    public class RobotAmobm : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"

            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
            {
                if (sActor.type == ActorType.PC)
                {
                    uint itemID = 10026800; //飞弹
                    var pc = sActor;
                    if (SkillHandler.Instance.CountItem(pc, itemID) > 0)
                    {
                        SkillHandler.Instance.TakeItem(pc, itemID, 1);
                        return 0;
                    }

                    return -2;
                }

                return 0;
            }

            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.3f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}