using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class HPRecovery : DefaultBuff
    {
        private readonly bool isMarionette;

        public HPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period)
            : base(skill, actor, "HPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        public HPRecovery(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int period, bool isMarionette)
            : base(skill, actor, isMarionette ? "Marionette_HPRecovery" : "HPRecovery", lifetime, period)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
            this.isMarionette = isMarionette;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                if (pc.Marionette != null && isMarionette) pc.Status.hp_recover_skill += 15;
            }
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                if (pc.Marionette == null && isMarionette) pc.Status.hp_recover_skill -= 15;
            }
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (!actor.Buff.NoRegen)
                //测试去除技能同步锁ClientManager.EnterCriticalArea();
                try
                {
                    var map = MapManager.Instance.GetMap(actor.MapID);
                    uint hpadd = 0;
                    if (isMarionette)
                    {
                        var pc = (ActorPC)actor;
                        if (pc.Marionette == null) AdditionEnd();
                        hpadd = (uint)(pc.MaxHP * (100 + (pc.Vit +
                                                          pc.Status.vit_item + pc.Status.vit_mario +
                                                          pc.Status.vit_rev) / 3) / 1500);
                    }
                    else
                    {
                        hpadd = (uint)(actor.Status.hp_recover_skill / 100f * actor.MaxHP);
                    }

                    if (!actor.Buff.NoRegen)
                        actor.HP += hpadd;
                    if (actor.HP > actor.MaxHP) actor.HP = actor.MaxHP;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }
            //测试去除技能同步锁ClientManager.LeaveCriticalArea();
        }
    }
}