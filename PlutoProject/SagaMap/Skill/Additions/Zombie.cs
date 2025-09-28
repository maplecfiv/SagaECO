using System;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class Zombie : DefaultBuff
    {
        public Zombie(Actor actor)
            : base(null, actor, "Zombie", int.MaxValue, 10000)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += TimerUpdate;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Zombie = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Zombie = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            //测试去除技能同步锁ClientManager.EnterCriticalArea();
            try
            {
                if (actor.HP > 0 && !actor.Buff.Dead)
                {
                    var map = MapManager.Instance.GetMap(actor.MapID);
                    var amount = (int)(actor.MaxHP / 50);
                    if (amount < 1)
                        amount = 1;
                    if (actor.HP > amount)
                    {
                        actor.HP = (uint)(actor.HP - amount);
                    }
                    else
                    {
                        actor.HP = 0;
                        actor.Buff.Dead = true;
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
                        actor.e.OnDie();
                        SkillHandler.RemoveAddition(actor, "Zombie");
                    }

                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
            //测试去除技能同步锁ClientManager.LeaveCriticalArea();
        }
    }
}