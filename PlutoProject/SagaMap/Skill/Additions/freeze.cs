using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.Additions
{
    public class Freeze : DefaultBuff
    {
        public Freeze(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Frosen",
                lifetime = (int)(lifetime * (1f - actor.AbnormalStatus[AbnormalStatus.Frosen] / 100)), 1000)
        {
            if (SkillHandler.Instance.isBossMob(actor))
            {
                if (!actor.Status.Additions.ContainsKey("BOSSFrosen免疫"))
                {
                    var BOSSFrosen免疫 = new DefaultBuff(skill, actor, "BOSSFrosen免疫", 30000);
                    SkillHandler.ApplyAddition(actor, BOSSFrosen免疫);
                }
                else
                {
                    Enabled = false;
                }
            }

            if (actor.Status.Additions.ContainsKey("DebuffDef"))
                Enabled = false;

            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
            OnUpdate += UpdateEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Frosen = true;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            if (skill.Variable.ContainsKey("WaterFrosenElement"))
                skill.Variable.Remove("WaterFrosenElement");
            skill.Variable.Add("WaterFrosenElement", actor.Elements[Elements.Water]);
            actor.Elements[Elements.Water] += 100;
            SkillHandler.Instance.CancelSkillCast(actor);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            actor.SpeedCut = 0;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.Buff.Frosen = false;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            actor.Elements[Elements.Water] = skill.Variable["WaterFrosenElement"];
        }

        private void UpdateEvent(Actor actor, DefaultBuff skill)
        {
            /*int reduce = 10 / (skill.lifeTime / 10);
            if (skill.Variable["WaterFrosenElement"] > 0)
            {
                skill.Variable["WaterFrosenElement"] -= reduce;
                actor.Elements[SagaLib.Elements.Water] -= reduce;
                if(actor.type == ActorType.PC)
                SagaMap.Manager.MapClientManager.Instance.FindClient((ActorPC)actor).OnPlayerElements();
            }*/
        }
    }
}