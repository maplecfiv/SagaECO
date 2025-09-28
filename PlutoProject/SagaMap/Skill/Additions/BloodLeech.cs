using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class BloodLeech : DefaultBuff
    {
        public float rate;

        public BloodLeech(SagaDB.Skill.Skill skill, Actor actor, int lifetime, float rate)
            : base(skill, actor, "BloodLeech", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            this.rate = rate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            actor.Buff.HPDrain3RD = true;
            actor.Buff.SPDrain3RD = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            actor.Buff.HPDrain3RD = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}