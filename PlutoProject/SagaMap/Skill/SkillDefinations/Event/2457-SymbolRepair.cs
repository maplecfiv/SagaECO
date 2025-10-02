using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     シンボル修復
    /// </summary>
    public class SymbolRepair : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)dActor.e;
                if (eh.AI.Mode.Symbol)
                    return 0;
                return -14;
            }

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                float amount = pc.CurrentJobLevel * 10;
                if (pc.JobType == JobType.BACKPACKER)
                    amount = amount * 1.5f;
                if (amount > 320)
                    amount = 320;
                var list = new List<Actor>();
                list.Add(dActor);
                SkillHandler.Instance.MagicAttack(sActor, list, args, SkillHandler.DefType.IgnoreAll, Elements.Neutral,
                    -amount, 0, true);
            }
        }

        //#endregion
    }
}