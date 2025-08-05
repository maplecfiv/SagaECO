using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class Confuse : DefaultBuff
    {
        public Confuse(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Confuse",
                (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Confused] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSConfuse免疫"))
                {
                    var BOSSConfuse免疫 = new DefaultBuff(skill, actor, "BOSSConfuse免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSConfuse免疫);
                }
                else
                {
                    Enabled = false;
                }
            }

            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Confused = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Confused = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}