using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class MoveSpeedDown2 : DefaultBuff
    {
        public MoveSpeedDown2(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "MoveSpeedDown2",
                (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.MoveSpeedDown] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSImmunitySpeedDown") &&
                    !actor.Status.Additions.ContainsKey("Frosen"))
                {
                    var BOSSImmunitySpeedDown = new DefaultBuff(skill, actor, "BOSSImmunitySpeedDown", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSImmunitySpeedDown);
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
            if (actor.SpeedCut < 7)
                actor.SpeedCut += 1;
            if (actor.SpeedCut >= 7)
            {
                var _Freeze = new Freeze(skill.skill, actor, 5000);
                SkillHandler.ApplyAddition(actor, _Freeze);
                actor.SpeedCut = 0;
            }

            var rate = 0.6f + 0.05f * actor.SpeedCut;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.SpeedDown = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (skill.Variable.ContainsKey("SpeedDown2"))
                skill.Variable.Remove("SpeedDown2");
            var value = (int)(actor.Speed * rate);
            skill.Variable.Add("SpeedDown2", value);
            actor.Speed -= (ushort)value;
            if (actor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)actor.e;
                //设置AI活动性可以更新AI的移动速度
                var act = eh.AI.AIActivity;
                eh.AI.AIActivity = act;
            }
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            if (skill.endTime < DateTime.Now)
            {
                actor.SpeedCut = 0;
                actor.Buff.SpeedDown = false;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }


            var value = skill.Variable["SpeedDown2"];
            actor.Speed = 600;
            if (actor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)actor.e;
                //设置AI活动性可以更新AI的移动速度
                var act = eh.AI.AIActivity;
                eh.AI.AIActivity = act;
            }
        }
    }
}