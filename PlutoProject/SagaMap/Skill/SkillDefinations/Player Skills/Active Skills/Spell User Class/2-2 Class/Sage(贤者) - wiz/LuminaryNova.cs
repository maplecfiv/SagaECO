using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Sage
{
    /// <summary>
    ///     恆星磒落（ルミナリィノヴァ）
    /// </summary>
    public class LuminaryNova : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.2f + 1.0f * level;
            var rate = 20 + 10 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 300, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    if (SkillHandler.Instance.CanAdditionApply(sActor, i, "LuminaryNova", 40) &&
                        !SkillHandler.Instance.isBossMob(i))
                    {
                        var skill = new LuminaryNovaBuff(args, sActor, i, 20000, 2000);
                        SkillHandler.ApplyAddition(i, skill);
                    }

                    affected.Add(i);
                }

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Neutral, factor);
        }

        public class LuminaryNovaBuff : DefaultBuff
        {
            private readonly SkillArg args;
            private readonly Actor sActor;

            public LuminaryNovaBuff(SkillArg args, Actor sActor, Actor actor, int lifetime, int period)
                : base(args.skill, actor, "LuminaryNova", lifetime, period)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                OnUpdate += UpdateTimeHandler;
                this.args = args.Clone();
                this.sActor = sActor;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void UpdateTimeHandler(Actor actor, DefaultBuff skill)
            {
                if (actor.HP > 0 && !actor.Buff.Dead)
                {
                    var map = MapManager.Instance.GetMap(actor.MapID);
                    var demage = Math.Min((int)(actor.MaxHP * (0.015f + 0.002f * skill.skill.Level)), 3000);
                    var arg2 = new EffectArg();
                    arg2.effectID = 5059;
                    arg2.actorID = actor.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, actor, true);
                    SkillHandler.Instance.FixAttack(sActor, actor, args, Elements.Neutral, demage);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, args, actor, true);
                }
                else
                {
                    skill.AdditionEnd();
                }
            }
        }

        #endregion
    }
}