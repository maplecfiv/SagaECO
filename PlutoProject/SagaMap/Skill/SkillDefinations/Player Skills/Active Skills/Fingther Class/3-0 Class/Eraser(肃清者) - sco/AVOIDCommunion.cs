using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    public class AVOIDCommunion : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Party != null)
                return 0;
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
                        var skill = new DefaultBuff(args.skill, act, "AVOIDCommunion", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var avoid_add = 50 + 20 * level;
            var exercises = new[] { 0, 42, 57, 74, 90, 105, 200 };
            //pvp时 闪避共有效果修正
            if (actor.type == ActorType.PC)
            {
                var pc = actor as ActorPC;
                if (pc.Mode == PlayerMode.COLISEUM_MODE)
                    avoid_add = exercises[level];
            }


            if (skill.Variable.ContainsKey("AVOIDCommunionAdd"))
                skill.Variable.Remove("AVOIDCommunionAdd");
            skill.Variable.Add("AVOIDCommunionAdd", avoid_add);
            actor.Status.avoid_melee_skill += (short)avoid_add;
            actor.Status.avoid_ranged_skill += (short)avoid_add;
            actor.Buff.AvoidUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.avoid_melee_skill -= (short)skill.Variable["AVOIDCommunionAdd"];
            actor.Status.avoid_ranged_skill -= (short)skill.Variable["AVOIDCommunionAdd"];
            actor.Buff.AvoidUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}