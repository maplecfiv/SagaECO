using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    /// <summary>
    ///     經驗值上升
    /// </summary>
    public class ExpUp : DefaultBuff
    {
        public ExpUp(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "ExpUp", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.EXPUp = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.EXPUp = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}