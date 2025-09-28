using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    /// <summary>
    ///     假币（フォールスマネー）
    /// </summary>
    internal class FalseMoney : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.1f * level;
            var lifetime = 7000;
            var pc = sActor as ActorPC;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.鈍足, 50))
                    {
                        var skills = new MoveSpeedDown(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skills);
                    }

                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse, 50))
                    {
                        var skills = new Confuse(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skills);
                    }

                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Silence, 50))
                    {
                        var skills = new Silence(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skills);
                    }

                    if (SagaLib.Global.Random.Next(0, 99) > 50)
                    {
                        var skill = new DefaultBuff(args.skill, dActor, "FalseMoney", 180000);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(dActor, skill);
                    }

                    realAffected.Add(act);
                }

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //actor.Buff.DefRateUp = true;
            if (skill.Variable.ContainsKey("Agi_down"))
                skill.Variable.Remove("Agi_down");
            skill.Variable.Add("Agi_down", 25);
            actor.Status.agi_skill -= 25;
            if (skill.Variable.ContainsKey("Dex_down"))
                skill.Variable.Remove("Dex_down");
            skill.Variable.Add("Dex_down", 25);
            actor.Status.dex_skill -= 25;
            actor.Buff.AGIDown = true;
            actor.Buff.DEXDown = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.agi_skill += (short)skill.Variable["Agi_down"];
            actor.Status.dex_skill += (short)skill.Variable["Dex_down"];
            actor.Buff.AGIDown = false;
            actor.Buff.DEXDown = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}