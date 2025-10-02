﻿using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class DarkArrow : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = 1.3f + 0.2f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Dark, factor);
        }

        //#endregion
    }
}