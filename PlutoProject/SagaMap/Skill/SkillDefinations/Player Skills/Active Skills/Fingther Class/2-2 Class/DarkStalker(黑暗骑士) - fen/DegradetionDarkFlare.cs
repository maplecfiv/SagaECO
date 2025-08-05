using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     魔王之火（ダークフレア）
    /// </summary>
    public class DegradetionDarkFlare : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 0.94f, 1.04f, 1.09f };
            var factor = factors[level];
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 5 + 10 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "DegradetionDarkFlare", rate) &&
                !SkillHandler.Instance.isBossMob(dActor))
            {
                var skill = new DegradetionDarkFlareBuff(sActor, args, dActor, 20000, 2000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        public class DegradetionDarkFlareBuff : DefaultBuff
        {
            private readonly SkillArg args;
            private readonly Actor sActor;

            public DegradetionDarkFlareBuff(Actor sActor, SkillArg args, Actor actor, int lifetime, int period)
                : base(args.skill, actor, "DegradetionDarkFlareBuff", lifetime, period)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                OnUpdate += TimerUpdate;
                this.args = args.Clone();
                this.sActor = sActor;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void TimerUpdate(Actor actor, DefaultBuff skill)
            {
                var HP_Lost = (uint)(actor.MaxHP * 0.015f);
                SkillHandler.Instance.FixAttack(sActor, actor, args, sActor.WeaponElement, HP_Lost);
                var map = MapManager.Instance.GetMap(actor.MapID);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, args, actor, false);
            }
        }

        #endregion
    }
}