using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class MPRecovery : DefaultBuff
    {
        private readonly bool isMarionette;

        public MPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period)
            : base(skill, actor, "MPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        public MPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period, bool isMarionette)
            : base(skill, actor, isMarionette ? "Marionette_MPRecovery" : "MPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
            this.isMarionette = isMarionette;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            // Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
            // map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            //Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
            //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (!actor.Buff.NoRegen)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                uint mpadd = 0;
                if (isMarionette)
                {
                    var pc = (ActorPC)actor;
                    if (pc.Marionette == null) AdditionEnd();
                    mpadd = (uint)(pc.MaxMP * (100 + (pc.Mag +
                                                      pc.Status.mag_item + pc.Status.mag_mario +
                                                      pc.Status.mag_rev) / 3) / 2000);
                }
                else
                {
                    var pc = (ActorPC)actor;
                    mpadd = mpadd = (uint)(pc.MaxMP * (100 + (pc.Mag +
                                                              pc.Status.mag_item + pc.Status.mag_mario +
                                                              pc.Status.mag_rev) / 3) / 2000 +
                                           pc.Status.mp_recover_skill);
                }

                actor.MP += mpadd;
                if (actor.MP > actor.MaxMP) actor.MP = actor.MaxMP;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
            }
        }
    }
}