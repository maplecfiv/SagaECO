using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class Stun : DefaultBuff
    {
        public Stun(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Stun", (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Stun] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSStun免疫"))
                {
                    var BOSSStun免疫 = new DefaultBuff(skill, actor, "BOSSStun免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSStun免疫);
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
            actor.Buff.Stun = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Stun = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}