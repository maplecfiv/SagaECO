using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions.Global
{
    public class Stone : DefaultBuff
    {
        public Stone(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Stone", (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Stone] / 100)),
                100)
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSStone免疫"))
                {
                    var BOSSStone免疫 = new DefaultBuff(skill, actor, "BOSSStone免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSStone免疫);
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
            actor.Buff.Stone = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (skill.Variable.ContainsKey("StoneFrosenElement"))
                skill.Variable.Remove("StoneFrosenElement");
            skill.Variable.Add("StoneFrosenElement", 100 - actor.Elements[Elements.Earth]);
            actor.Elements[Elements.Earth] = 100;
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Stone = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            actor.Elements[Elements.Earth] -= skill.Variable["StoneFrosenElement"];
        }

        private void UpdateEvent(Actor actor, DefaultBuff skill)
        {
            //int reduce = 100 / (skill.lifeTime / 100);
            //if (skill.Variable["StoneFrosenElement"] > 0)
            //{
            //    skill.Variable["StoneFrosenElement"] -= reduce;
            //    actor.Elements[SagaLib.Elements.Earth] -= reduce;
            //    if (actor.type == ActorType.PC)
            //        SagaMap.Manager.MapClientManager.Instance.FindClient((ActorPC)actor).OnPlayerElements();
            //}
        }
    }
}