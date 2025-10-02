﻿using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     赞歌（Ragnarok）
    /// </summary>
    public class Adoramus : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 15.0f;
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
        }

        //#endregion
    }
}