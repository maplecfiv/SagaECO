﻿using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;

namespace SagaMap.Skill.SkillDefinations.Vates
{
    public class SmallHealing : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Status.Additions.ContainsKey("Spell")) return -7;
            if (dActor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)dActor.e;
                if (eh.AI.Mode.Symbol)
                    return -14;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.5f + 0.05f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                -factor);
        }

        #endregion
    }
}