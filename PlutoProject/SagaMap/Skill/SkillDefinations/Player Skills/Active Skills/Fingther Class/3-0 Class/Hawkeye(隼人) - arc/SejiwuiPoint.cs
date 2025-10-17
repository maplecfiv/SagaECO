using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    public class SejiwuiPoint : ISkill
    {
        private Actor pc;

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            pc = sActor;
            var lifetime = 30000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 400, true);
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                {
                    var pc = (ActorPC)act;
                    if (pc.Online)
                    {
                        var skill = new DefaultBuff(args.skill, act, "SejiwuiPoint", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var hit_add = (int)(actor.Status.hit_melee * 0.02f * skill.skill.Level);
            var cri_add_player = 15 + 2 * skill.skill.Level;
            var cri_add_team = 3 * skill.skill.Level;

            if (skill.Variable.ContainsKey("SejiwuiPoint_hit"))
                skill.Variable.Remove("SejiwuiPoint_hit");
            skill.Variable.Add("SejiwuiPoint_hit", hit_add);
            actor.Status.hit_melee_skill += (short)hit_add;

            if (actor == pc)
            {
                if (skill.Variable.ContainsKey("SejiwuiPoint_cri"))
                    skill.Variable.Remove("SejiwuiPoint_cri");
                skill.Variable.Add("SejiwuiPoint_cri", cri_add_player);
                actor.Status.cri_skill += (short)cri_add_player;
            }
            else
            {
                if (skill.Variable.ContainsKey("SejiwuiPoint_cri"))
                    skill.Variable.Remove("SejiwuiPoint_cri");
                skill.Variable.Add("SejiwuiPoint_cri", cri_add_team);
                actor.Status.cri_skill += (short)cri_add_team;
            }


            actor.Buff.三转せーチウィークポイント = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_melee_skill -= (short)skill.Variable["SejiwuiPoint_hit"];
            actor.Status.cri_skill -= (short)skill.Variable["SejiwuiPoint_cri"];
            actor.Buff.三转せーチウィークポイント = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}