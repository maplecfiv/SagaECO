using System;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     もっと褒めてもいいのよ！
    /// </summary>
    public class YouCanPraiseMore : ISkill
    {
        //#region ISkill 成員

        private int KillingMarkCounter;

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("Efuikasu"))
                return -1;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 420000;
            if (sActor.KillingMarkCounter != 0)
                KillingMarkCounter = sActor.KillingMarkCounter;
            var skill = new DefaultBuff(args.skill, sActor, "Efuikasu", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.KillingMarkCounter = Math.Min(KillingMarkCounter, 20);
            actor.Buff.KillingMark = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.KillingMarkCounter = 0;
            actor.Buff.KillingMark = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}