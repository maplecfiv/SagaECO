﻿using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.SkillDefinations.Global;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    public class EnergyGroove : Groove, ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ProcSub(sActor, dActor, args, level, Elements.Neutral);
        }

        #endregion
    }
}