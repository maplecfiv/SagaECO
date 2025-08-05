using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class SpLeech : DefaultBuff
    {
        public float rate;

        public SpLeech(SagaDB.Skill.Skill skill, Actor actor, int lifetime, float rate)
            : base(skill, actor, "SpLeech", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            this.rate = rate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            actor.Buff.SPDrain3RD = true;
            actor.Buff.HPDrain3RD = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            actor.Buff.SPDrain3RD = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}