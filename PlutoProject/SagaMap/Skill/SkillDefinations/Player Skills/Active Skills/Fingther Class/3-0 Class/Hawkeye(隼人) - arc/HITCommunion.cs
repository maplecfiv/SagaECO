using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    public class HITCommunion : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Party != null) return 0;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 600000;
            var sPC = (ActorPC)sActor;
            foreach (var act in sPC.Party.Members.Values)
                if (act.Online)
                    if (act.Party.ID != 0 && !act.Buff.Dead && act.MapID == sActor.MapID)
                    {
                        var skill = new DefaultBuff(args.skill, act, "HITCommunion", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var hit_add = 100 + 20 * level;
            if (skill.Variable.ContainsKey("HITCommunion"))
                skill.Variable.Remove("HITCommunion");
            skill.Variable.Add("HITCommunion", hit_add);
            actor.Status.hit_melee_skill += (short)hit_add;
            actor.Status.hit_ranged_skill += (short)hit_add;
            actor.Buff.HitUp3RD = true;
            //if (actor.Status.Additions.ContainsKey("HITCommunion"))

            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_melee_skill -= (short)skill.Variable["HITCommunion"];
            actor.Status.hit_ranged_skill -= (short)skill.Variable["HITCommunion"];
            actor.Buff.HitUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}