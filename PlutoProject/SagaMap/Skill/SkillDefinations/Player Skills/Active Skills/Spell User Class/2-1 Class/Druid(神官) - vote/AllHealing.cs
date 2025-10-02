using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    public class AllHealing : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var recoverfactors = new[] { 0, -2.8f, -3.2f, -3.6f, -4.2f, -5.0f };
            var recoverfactor = recoverfactors[level];

            var damagefactors = new[] { 0, 3.0f, 3.4f, 4.2f, 5.2f, 6.0f };
            var damagefactor = damagefactors[level];

            var lifetimes = new[] { 0, 30000, 50000, 75000, 95000, 120000 };
            var lifetime = lifetimes[level];

            if (sActor.Status.Additions.ContainsKey("Cardinal")) //3转10技提升治疗量\
            {
                recoverfactor += sActor.Status.Cardinal_Rank;
                damagefactor -= sActor.Status.Cardinal_Rank;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 200, null);
            var recoveraffected = new List<Actor>();
            var damageaffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                {
                    var m = (ActorPC)act;
                    if (m.Buff.Dead != true)
                    {
                        recoveraffected.Add(act);
                    }
                    else if (m.Buff.TurningPurple != true)
                    {
                        m.Buff.TurningPurple = true;
                        MapClient.FromActorPC(m).Map
                            .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, m, true);
                        m.TInt["Revive"] = level;
                        MapClient.FromActorPC(m).EventActivate(0xF1000000);
                        m.TStr["Revive"] = sActor.Name;
                        MapClient.FromActorPC(m).SendSystemMessage(string.Format("玩家 {0} 正在请求你复活", sActor.Name));
                    }

                    if (!act.Status.Additions.ContainsKey("AllHealing") && !act.Buff.Undead)
                    {
                        var skill = new DefaultBuff(args.skill, act, "AllHealing", lifetime);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }

                    if (act.Buff.Undead) damageaffected.Add(act);
                }
                else if (act.type == ActorType.MOB)
                {
                    var m = (ActorMob)act;
                    if (m.BaseData.undead)
                        damageaffected.Add(act);
                }

            SkillHandler.Instance.MagicAttack(sActor, recoveraffected, args, SkillHandler.DefType.IgnoreAll,
                Elements.Holy, recoverfactor);
            SkillHandler.Instance.MagicAttack(sActor, damageaffected, args, Elements.Holy, damagefactor);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("AllHealing_MP"))
                skill.Variable.Remove("AllHealing_MP");
            skill.Variable.Add("AllHealing_MP", 15);
            actor.Status.mp_recover_skill += 15;
            if (skill.Variable.ContainsKey("AllHealing_HP"))
                skill.Variable.Remove("AllHealing_HP");
            skill.Variable.Add("AllHealing_HP", 15);
            actor.Status.hp_recover_skill += 15;
            if (skill.Variable.ContainsKey("AllHealing_SP"))
                skill.Variable.Remove("AllHealing_SP");
            skill.Variable.Add("AllHealing_SP", 15);
            actor.Status.sp_recover_skill += 15;
            actor.Buff.HPRegenUp = true;
            actor.Buff.SPRegenUp = true;
            actor.Buff.MPRegenUp = true;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.mp_recover_skill -= (short)skill.Variable["AllHealing_MP"];
            actor.Status.hp_recover_skill -= (short)skill.Variable["AllHealing_HP"];
            actor.Status.sp_recover_skill -= (short)skill.Variable["AllHealing_SP"];
            actor.Buff.HPRegenUp = false;
            actor.Buff.SPRegenUp = false;
            actor.Buff.MPRegenUp = false;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}