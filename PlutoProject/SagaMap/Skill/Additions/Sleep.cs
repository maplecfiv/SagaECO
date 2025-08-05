using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class Sleep : DefaultBuff
    {
        public Sleep(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Sleep", (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Sleep] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSSleep免疫"))
                {
                    var BOSSSleep免疫 = new DefaultBuff(skill, actor, "BOSSSleep免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSSleep免疫);
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
            actor.Buff.Sleep = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Sleep = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}