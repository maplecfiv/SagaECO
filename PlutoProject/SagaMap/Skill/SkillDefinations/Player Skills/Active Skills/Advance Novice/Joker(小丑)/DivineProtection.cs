using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     ホーリーフェザー
    /// </summary>
    public class DivineProtection : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 2000, true);
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
                realAffected.Add(sActor);
            }

            args.affectedActors = realAffected;
            args.Init();
            int[] lifetimes = { 0, 60000, 75000, 90000, 105000, 120000 };
            var lifetime = lifetimes[level];
            foreach (var rAct in realAffected)
            {
                if (rAct.Status.Additions.ContainsKey("StyleChange"))
                    continue;
                var skill = new DefaultBuff(args.skill, rAct, "DivineProtection", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(rAct, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var status_add = new short[] { 0, 1, 2, 3, 5, 8 }[skill.skill.Level];
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                if (pc.Job == PC_JOB.JOKER)
                {
                    pc.Status.str_skill += status_add;
                    pc.Status.vit_skill += status_add;
                    pc.Status.dex_skill += status_add;
                    pc.Status.mag_skill += status_add;
                    pc.Status.int_skill += status_add;
                }
                else if (pc.JobBasic == PC_JOB.SWORDMAN ||
                         pc.JobBasic == PC_JOB.FENCER ||
                         pc.JobBasic == PC_JOB.SCOUT ||
                         pc.JobBasic == PC_JOB.ARCHER)
                {
                    pc.Status.str_skill += status_add;
                    pc.Status.vit_skill += status_add;
                }
                else if (pc.JobBasic == PC_JOB.WIZARD ||
                         pc.JobBasic == PC_JOB.SHAMAN ||
                         pc.JobBasic == PC_JOB.VATES ||
                         pc.JobBasic == PC_JOB.WARLOCK)
                {
                    pc.Status.mag_skill += status_add;
                    pc.Status.int_skill += status_add;
                }
                else if (pc.JobBasic == PC_JOB.TATARABE ||
                         pc.JobBasic == PC_JOB.FARMASIST ||
                         pc.JobBasic == PC_JOB.RANGER ||
                         pc.JobBasic == PC_JOB.MERCHANT)
                {
                    pc.Status.str_skill += status_add;
                    pc.Status.mag_skill += status_add;
                }
            }

            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var status_add = new short[] { 0, 1, 2, 3, 5, 8 }[skill.skill.Level];
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                if (pc.Job == PC_JOB.JOKER)
                {
                    pc.Status.str_skill -= status_add;
                    pc.Status.vit_skill -= status_add;
                    pc.Status.dex_skill -= status_add;
                    pc.Status.mag_skill -= status_add;
                    pc.Status.int_skill -= status_add;
                }
                else if (pc.JobBasic == PC_JOB.SWORDMAN ||
                         pc.JobBasic == PC_JOB.FENCER ||
                         pc.JobBasic == PC_JOB.SCOUT ||
                         pc.JobBasic == PC_JOB.ARCHER)
                {
                    pc.Status.str_skill -= status_add;
                    pc.Status.vit_skill -= status_add;
                }
                else if (pc.JobBasic == PC_JOB.WIZARD ||
                         pc.JobBasic == PC_JOB.SHAMAN ||
                         pc.JobBasic == PC_JOB.VATES ||
                         pc.JobBasic == PC_JOB.WARLOCK)
                {
                    pc.Status.mag_skill -= status_add;
                    pc.Status.int_skill -= status_add;
                }
                else if (pc.JobBasic == PC_JOB.TATARABE ||
                         pc.JobBasic == PC_JOB.FARMASIST ||
                         pc.JobBasic == PC_JOB.RANGER ||
                         pc.JobBasic == PC_JOB.MERCHANT)
                {
                    pc.Status.str_skill -= status_add;
                    pc.Status.mag_skill -= status_add;
                }
            }

            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHANGE_STATUS, null, actor, true);
        }

        //#endregion
    }
}