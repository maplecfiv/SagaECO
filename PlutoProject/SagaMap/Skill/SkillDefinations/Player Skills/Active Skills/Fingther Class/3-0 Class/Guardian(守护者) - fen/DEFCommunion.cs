using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Guardian_守护者____fen
{
    public class def_addCommunion : ISkill
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
                        var skill = new DefaultBuff(args.skill, act, "DEFCommunion", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var def_add_add = 68 + 30 * level;
            //暂时不考虑pvp状态
            //int def_add_pvp = 25 + level == 5 ? (level - 1) * 11 + 12 : level * 11;
            if (skill.Variable.ContainsKey("DEFCommunion"))
                skill.Variable.Remove("DEFCommunion");
            skill.Variable.Add("DEFCommunion", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;
            actor.Buff.DefUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_add_skill -= (short)skill.Variable["DEFCommunion"];
            actor.Buff.DefUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}