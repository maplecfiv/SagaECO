using System;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    internal class SkillForbid : ISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.三转禁言レストスキル = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.三转禁言レストスキル = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isBossMob(dActor))
                return -1;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] lifetime = { 0, 3000, 4000, 4000, 4000, 5000 };
            if (dActor.type == ActorType.PC)
                lifetime = new[] { 0, 3000, 4000, 4000, 4000, 5000 };
            if (dActor.type == ActorType.MOB)
                lifetime = new[] { 0, 15000, 18000, 21000, 24000, 30000 };
            if (SkillHandler.Instance.isBossMob(sActor))
                lifetime = new[] { 0, 2000, 2000, 2000, 2000, 2000 };


            int[] rate = { 0, 95, 90, 85, 80, 75 };

            var rand = new Random();
            var rVal = rand.Next(0, 100);
            if (rVal > rate[level]) return;


            var skill = new DefaultBuff(args.skill, dActor, "SkillForbid", lifetime[level]);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}