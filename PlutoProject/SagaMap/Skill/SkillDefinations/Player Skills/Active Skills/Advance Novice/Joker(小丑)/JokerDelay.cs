using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     小丑延迟取消
    /// </summary>
    public class JokerDelay : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0; //不显示效果
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, true);
            var realAffected = new List<Actor>();
            var sPC = (ActorPC)sActor;
            if (sPC.Party != null)
            {
                foreach (var act in affected)
                    if (act.type == ActorType.PC)
                    {
                        var aPC = (ActorPC)act;
                        if (aPC.Party != null && sPC.Party != null)
                            if (aPC.Party.ID == sPC.Party.ID && aPC.Party.ID != 0 && !aPC.Buff.Dead &&
                                aPC.PossessionTarget == 0)
                            {
                                if (act.Buff.NoRegen) continue;

                                if (aPC.Party.ID == sPC.Party.ID) realAffected.Add(act);
                            }
                    }
            }
            else
            {
                realAffected.Add(sPC);
            }

            var life = 180000;
            foreach (var rAct in realAffected)
                if (rAct.ActorID == sActor.ActorID)
                {
                    Actor realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                    var skill = new DefaultBuff(args.skill, realdActor, "JokerDelay", life);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    skill.OnCheckValid += ValidCheck;
                    SkillHandler.ApplyAddition(realdActor, skill);
                }
                else
                {
                    var skill = new DefaultBuff(args.skill, rAct, "JokerDelay", life);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    skill.OnCheckValid += ValidCheck;
                    SkillHandler.ApplyAddition(rAct, skill);
                }
        }

        private void ValidCheck(ActorPC pc, Actor dActor, out int result)
        {
            result = TryCast(pc, dActor, null);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.aspd_skill_perc += 0.2f * skill.skill.Level;

            actor.Buff.JSpeed3RD = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var raspd_skill_perc_restore = 0.2f * skill.skill.Level;
            if (actor.Status.aspd_skill_perc > raspd_skill_perc_restore + 1)
                actor.Status.aspd_skill_perc -= raspd_skill_perc_restore;
            else
                actor.Status.aspd_skill_perc = 1;

            actor.Buff.JSpeed3RD = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}