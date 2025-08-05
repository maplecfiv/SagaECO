using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Royaldealer
{
    internal class CriUp : ISkill
    {
        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("Criavd_down"))
                skill.Variable.Remove("Criavd_down");
            skill.Variable.Add("Criavd_down", 30);
            actor.Status.criavd_skill -= 30;
            actor.Buff.三转指定对象被会心率UPクリティカルマーキング = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.criavd_skill += (short)skill.Variable["Criavd_down"];
            actor.Buff.三转指定对象被会心率UPクリティカルマーキング = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 140000 - 20000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "CriUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}