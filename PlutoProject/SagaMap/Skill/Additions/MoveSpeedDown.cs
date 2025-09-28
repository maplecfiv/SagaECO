using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class MoveSpeedDown : DefaultBuff
    {
        public MoveSpeedDown(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "MoveSpeedDown",
                (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.MoveSpeedDown] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSImmunitySpeedDown"))
                {
                    var BOSS鈍足免疫 = new DefaultBuff(skill, actor, "BOSSImmunitySpeedDown", 30000);
                    SkillHandler.ApplyAddition(actor, BOSS鈍足免疫);
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
            actor.Buff.SpeedDown = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (skill.Variable.ContainsKey("SpeedDown"))
                skill.Variable.Remove("SpeedDown");
            var value = (int)(actor.Speed * 0.8f);
            skill.Variable.Add("SpeedDown", value);
            actor.Status.speed_skill -= (ushort)value;
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
            actor.Buff.SpeedDown = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            var value = skill.Variable["SpeedDown"];
            actor.Status.speed_skill += (ushort)value;
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