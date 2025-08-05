﻿using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Guardian
{
    public class Guardian : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = (30 + level * 30) * 1000;
            var DEFUP = 50 + 5 * level;
            var skill = new DefaultBuff(args.skill, sActor, "Guardian", lifetime, 1000);
            skill.OnAdditionStart += StartEvent;
            skill.OnAdditionEnd += EndEvent;
            skill.OnUpdate += UpdateEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            //X
            if (skill.Variable.ContainsKey("Save_X"))
                skill.Variable.Remove("Save_X");
            skill.Variable.Add("Save_X", actor.X);

            //Y
            if (skill.Variable.ContainsKey("Save_Y"))
                skill.Variable.Remove("Save_Y");
            skill.Variable.Add("Save_Y", actor.Y);
            int level = skill.skill.Level;
            var rate = new[] { 0, 0.5f, 0.55f, 0.6f, 0.7f, 0.8f }[level];
            var def_add = (int)(actor.Status.def_add * rate);
            var mdef_add = (int)(actor.Status.mdef_add * rate);
            if (skill.Variable.ContainsKey("Guardian_def"))
                skill.Variable.Remove("Guardian_def");
            skill.Variable.Add("Guardian_def", def_add);
            if (skill.Variable.ContainsKey("Guardian_mdef"))
                skill.Variable.Remove("Guardian_mdef");
            skill.Variable.Add("Guardian_mdef", mdef_add);
            actor.Status.def_add_skill += (short)def_add;
            actor.Status.mdef_add_skill += (short)mdef_add;
            actor.Buff.MainSkillPowerUp3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_add_skill -= (short)skill.Variable["Guardian_def"];
            actor.Status.mdef_add_skill -= (short)skill.Variable["Guardian_mdef"];
            actor.Buff.MainSkillPowerUp3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void UpdateEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.X != (short)skill.Variable["Save_X"] || actor.Y != (short)skill.Variable["Save_Y"])
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.Status.Additions["Guardian"].AdditionEnd();
                actor.Status.Additions.Remove("Guardian");
                actor.Buff.MainSkillPowerUp3RD = false;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, false);
            }
        }

        #endregion
    }
}