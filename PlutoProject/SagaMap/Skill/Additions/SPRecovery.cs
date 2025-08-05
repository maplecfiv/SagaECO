using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class SPRecovery : DefaultBuff
    {
        private readonly bool isMarionette;

        public SPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period)
            : base(skill, actor, "SPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        public SPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period, bool isMarionette)
            : base(skill, actor, isMarionette ? "Marionette_SPRecovery" : "SPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
            this.isMarionette = isMarionette;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            //Map map = Manager.MapManager.Instance.GetMap(actor.MapID);
            //map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
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
                uint spadd = 0;
                if (isMarionette)
                {
                    var pc = (ActorPC)actor;
                    if (pc.Marionette == null) AdditionEnd();
                    spadd = (uint)(pc.MaxSP * (100 + (pc.Int +
                                                      pc.Vit + pc.Status.int_item + pc.Status.int_mario +
                                                      pc.Status.int_rev + pc.Status.vit_rev + pc.Status.vit_mario +
                                                      pc.Status.vit_item) / 6) / 2000);
                }
                else
                {
                    var pc = (ActorPC)actor;
                    spadd = (uint)(pc.MaxSP * (100 + (pc.Int +
                                                      pc.Vit + pc.Status.int_item + pc.Status.int_mario +
                                                      pc.Status.int_rev + pc.Status.vit_rev + pc.Status.vit_mario +
                                                      pc.Status.vit_item) / 6) / 2000 + pc.Status.sp_recover_skill);
                }

                actor.SP += spadd;
                if (actor.SP > actor.MaxSP) actor.SP = actor.MaxSP;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
            }
        }
    }
}