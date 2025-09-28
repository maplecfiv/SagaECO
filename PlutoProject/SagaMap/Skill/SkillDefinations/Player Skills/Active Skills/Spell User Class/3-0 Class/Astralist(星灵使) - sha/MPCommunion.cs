using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    public class MPCommunion : ISkill
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
                        var skill = new DefaultBuff(args.skill, act, "MPCommunion", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var mp_add = 400 + 120 * level;
            if (skill.Variable.ContainsKey("MPCommunion"))
                skill.Variable.Remove("MPCommunion");
            skill.Variable.Add("MPCommunion", mp_add);
            actor.MaxMP += (uint)mp_add;
            actor.Buff.MaxMPUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.MaxMP -= (uint)skill.Variable["MPCommunion"];
            actor.Buff.MaxMPUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}