using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions {
    public class Invisible : DefaultBuff {
        /// <summary>
        ///     隐身，每2秒会向周围队友发送自己的位置（显示特效）
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="actor"></param>
        /// <param name="lifetime">持续时间</param>
        public Invisible(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Invisible", lifetime, 2000) {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += UpdateEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill) {
            var map = MapManager.Instance.GetMap(actor.MapID);
            /*---------移除仇恨---------*/
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.DISAPPEAR, null, actor, false);

            SkillHandler.Instance.ShowEffectByActor(actor, 4102);
            actor.Buff.Transparent = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill) {
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.APPEAR, null, actor, false);

            actor.Buff.Transparent = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEvent(Actor actor, DefaultBuff skill) {
            try {
                if (actor.HP > 0 && !actor.Buff.Dead)
                    if (actor.type == ActorType.PC) {
                        var pc = (ActorPC)actor;
                        if (pc.Party != null) {
                            var map = MapManager.Instance.GetMap(actor.MapID);
                            var target = map.GetActorsArea(pc, 1000, true);
                            foreach (var item in target)
                                if (item.type == ActorType.PC) {
                                    var pm = (ActorPC)item;
                                    if (pm.Online && pm.Party == pc.Party)
                                        SkillHandler.Instance.ShowEffectByActor(pc, 4238);
                                }
                        }
                    }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }
    }
}