﻿using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Enchanter
{
    /// <summary>
    ///     大地勢力（アースオーラ）
    /// </summary>
    public class SoulOfEarth : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.Status.Additions.ContainsKey("DevineBarrier")) return -12;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "SoulOfEarth", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            short LDef = 0, RDef = 0;
            int level = skill.skill.Level;
            switch (level)
            {
                case 1:
                    LDef = 7;
                    RDef = 20;
                    break;
                case 2:
                    LDef = 11;
                    RDef = 30;
                    break;
                case 3:
                    LDef = 15;
                    RDef = 40;
                    break;
            }

            SkillHandler.RemoveAddition(actor, "EnergyShield");

            //左防
            if (skill.Variable.ContainsKey("SoulOfEarth_LDef"))
                skill.Variable.Remove("SoulOfEarth_LDef");
            skill.Variable.Add("SoulOfEarth_LDef", LDef);
            //右防
            if (skill.Variable.ContainsKey("SoulOfEarth_RDef"))
                skill.Variable.Remove("SoulOfEarth_RDef");
            skill.Variable.Add("SoulOfEarth_RDef", RDef);

            //左防
            actor.Status.def_skill += LDef;
            //右防
            actor.Status.def_add_skill += RDef;

            actor.Buff.DefRateUp = true;
            actor.Buff.DefUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //左防
            actor.Status.def_skill -= (short)skill.Variable["SoulOfEarth_LDef"];
            //右防
            actor.Status.def_add_skill -= (short)skill.Variable["SoulOfEarth_RDef"];

            actor.Buff.DefRateUp = false;
            actor.Buff.DefUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}