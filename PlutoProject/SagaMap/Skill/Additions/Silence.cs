using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class Silence : DefaultBuff
    {
        public Silence(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Silence", (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Silence] / 100)))
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSSilence免疫"))
                {
                    var BOSSSilence免疫 = new DefaultBuff(skill, actor, "BOSSSilence免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSSilence免疫);
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
            actor.Buff.Silence = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Silence = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}