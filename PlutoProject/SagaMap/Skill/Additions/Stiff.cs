using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class Stiff : DefaultBuff
    {
        public Stiff(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Stiff", lifetime)
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSImmunityStiff"))
                {
                    var BOSSImmunityStiff = new DefaultBuff(skill, actor, "BOSSImmunityStiff", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSImmunityStiff);
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
            actor.Buff.Stiff = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Stiff = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}