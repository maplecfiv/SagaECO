using System;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    /// <summary>
    ///     エフィカス
    /// </summary>
    public class Efuikasu : ISkill
    {
        //#region ISkill 成員

        private int KillingMarkCounter;
        private Actor skilluser;

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetimes = new[] { 0, 180000, 240000, 300000, 360000, 420000 };
            var lifetime = lifetimes[level];
            Actor realactor = SkillHandler.Instance.GetPossesionedActor(sActor as ActorPC);
            if (realactor.KillingMarkCounter != 0)
                KillingMarkCounter = realactor.KillingMarkCounter;
            if (realactor.ActorID != sActor.ActorID)
                skilluser = sActor;
            else
                skilluser = realactor;
            var skill = new DefaultBuff(args.skill, realactor, "Efuikasu", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realactor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.ActorID == skilluser.ActorID)
            {
                actor.KillingMarkSoulUse = false;
                actor.KillingMarkCounter = Math.Min(KillingMarkCounter, 20);
            }
            else
            {
                actor.KillingMarkSoulUse = true;
                actor.KillingMarkCounter = Math.Min(KillingMarkCounter, 10);
            }

            actor.Buff.KillingMark = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.KillingMarkSoulUse = false;
            actor.KillingMarkCounter = 0;
            actor.Buff.KillingMark = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}