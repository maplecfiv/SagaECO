using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat
{
    public class ATKCommunion : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Party != null) return 0;
            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 600000;
            var realAffected = new List<Actor>();
            var sPC = (ActorPC)sActor;
            foreach (var act in sPC.Party.Members.Values)
                if (act.Online)
                    if (act.Party.ID != 0 && !act.Buff.Dead && act.MapID == sActor.MapID)
                    {
                        var skill = new DefaultBuff(args.skill, act, "ATKCommunion", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var atk_add = 120 + 36 * level;
            if (skill.Variable.ContainsKey("ATKCommunion"))
                skill.Variable.Remove("ATKCommunion");
            skill.Variable.Add("ATKCommunion", atk_add);
            actor.Status.max_atk1_skill += (short)atk_add;
            actor.Status.max_atk2_skill += (short)atk_add;
            actor.Status.max_atk3_skill += (short)atk_add;
            actor.Buff.AtkUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.max_atk1_skill -= (short)skill.Variable["ATKCommunion"];
            actor.Status.max_atk2_skill -= (short)skill.Variable["ATKCommunion"];
            actor.Status.max_atk3_skill -= (short)skill.Variable["ATKCommunion"];
            actor.Buff.AtkUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}