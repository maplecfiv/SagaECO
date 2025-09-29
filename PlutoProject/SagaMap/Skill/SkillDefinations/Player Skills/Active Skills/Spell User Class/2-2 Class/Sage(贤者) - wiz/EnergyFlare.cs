using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     燃燒生命（エナジーフレア）
    /// </summary>
    public class EnergyFlare : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 6.0f + 0.7f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "EnergyFlare", 40) &&
                !SkillHandler.Instance.isBossMob(dActor))
            {
                var skill = new EnergyFlareBuff(args, sActor, dActor, 20000, 2000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        public class EnergyFlareBuff : DefaultBuff
        {
            private readonly SkillArg args;
            private readonly Actor sActor;

            public EnergyFlareBuff(SkillArg args, Actor sActor, Actor actor, int lifetime, int period)
                : base(args.skill, actor, "EnergyFlare", lifetime, period)
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
                    var demage = Math.Min((int)(actor.MaxHP * (0.013f + 0.004f * skill.skill.Level)), 3000);
                    SkillHandler.Instance.FixAttack(sActor, actor, args, Elements.Neutral, demage);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, args, actor, false);
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