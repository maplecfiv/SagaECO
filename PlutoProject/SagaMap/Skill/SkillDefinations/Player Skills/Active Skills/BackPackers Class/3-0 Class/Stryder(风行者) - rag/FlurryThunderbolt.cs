using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    public class FlurryThunderbolt : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] lifetime = { 0, 120, 120, 120, 180, 180 };
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //更改作用范围,为3*3而不是之前的5*5
            var actors = map.GetActorsArea(sActor, 100, true);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (i.type == ActorType.PC)
                {
                    var skill = new DefaultBuff(args.skill, i, "FlurryThunderbolt", lifetime[level] * 1000);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(i, skill);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var add = 7 + skill.skill.Level;
            if (skill.Variable.ContainsKey("FlurryThunderbolt_DEX"))
                skill.Variable.Remove("FlurryThunderbolt_DEX");
            skill.Variable.Add("FlurryThunderbolt_DEX", add);
            actor.Status.dex_skill += (short)add;
            actor.Status.agi_skill += (short)add;

            actor.Buff.DEXUp = true;
            actor.Buff.AGIUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.dex_skill -= (short)skill.Variable["FlurryThunderbolt_DEX"];
            actor.Status.agi_skill -= (short)skill.Variable["FlurryThunderbolt_DEX"];
            actor.Buff.DEXUp = false;
            actor.Buff.AGIUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}