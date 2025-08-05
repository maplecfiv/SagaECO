﻿using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Fencer
{
    /// <summary>
    ///     重裝鎧化（ディフェンス・バースト）
    /// </summary>
    public class MobDefUpSelf : ISkill
    {
        private readonly bool MobUse;

        public MobDefUpSelf()
        {
            MobUse = false;
        }

        public MobDefUpSelf(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = sActor.ActorID;
            //int[] life = { 0, 5000 };
            var lifetime = 280000 - level * 20000;
            //DefaultBuff skill = new DefaultBuff(args.skill, dActor, "铁壁", life[level]);
            var skill = new DefaultBuff(args.skill, dActor, "重装铠化", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;

            if (MobUse)
                level = 5;

            var leftdef = new[] { 0, 5, 5, 8, 8, 11 };
            var rightdef = 8 + 2 * skill.skill.Level;
            var leftmdef = new[] { 0, 4, 4, 7, 7, 10 };
            var rightmdef = 5 + 2 * skill.skill.Level;

            if (skill.Variable.ContainsKey("重装铠化_leftdef"))
                skill.Variable.Remove("重装铠化_leftdef");
            skill.Variable.Add("重装铠化_leftdef", leftdef[skill.skill.Level]);
            actor.Status.def_skill += (short)skill.Variable["重装铠化_leftdef"];

            if (skill.Variable.ContainsKey("重装铠化_rightdef"))
                skill.Variable.Remove("重装铠化_rightdef");
            skill.Variable.Add("重装铠化_rightdef", rightdef);
            actor.Status.def_add_skill += (short)skill.Variable["重装铠化_rightdef"];

            if (skill.Variable.ContainsKey("重装铠化_leftmdef"))
                skill.Variable.Remove("重装铠化_leftmdef");
            skill.Variable.Add("重装铠化_leftmdef", leftmdef[skill.skill.Level]);
            actor.Status.mdef_skill += (short)skill.Variable["重装铠化_leftmdef"];

            if (skill.Variable.ContainsKey("重装铠化_rightmdef"))
                skill.Variable.Remove("重装铠化_rightmdef");
            skill.Variable.Add("重装铠化_rightmdef", rightmdef);
            actor.Status.mdef_add_skill += (short)skill.Variable["重装铠化_rightmdef"];

            actor.Buff.DefUp = true;
            actor.Buff.MagicDefUp = true;
            if (actor is ActorMob)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mdef_add_skill -= (short)skill.Variable["重装铠化_rightmdef"];
            skill.Variable.Remove("重装铠化_rightmdef");
            actor.Status.mdef_skill -= (short)skill.Variable["重装铠化_leftmdef"];
            skill.Variable.Remove("重装铠化_leftmdef");

            actor.Status.def_add_skill -= (short)skill.Variable["重装铠化_rightdef"];
            skill.Variable.Remove("重装铠化_rightdef");
            actor.Status.def_skill -= (short)skill.Variable["重装铠化_leftdef"];
            skill.Variable.Remove("重装铠化_leftdef");

            actor.Buff.DefUp = false;
            actor.Buff.MagicDefUp = false;
            if (actor is ActorMob)
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            else
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}