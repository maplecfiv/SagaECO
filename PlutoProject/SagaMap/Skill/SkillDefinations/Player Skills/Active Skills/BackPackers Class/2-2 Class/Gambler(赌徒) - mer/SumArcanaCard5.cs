using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Gambler
{
    /// <summary>
    ///     艾卡卡（アルカナカード）[接續技能]
    /// </summary>
    public class SumArcanaCard5 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 12000;
            uint MobID = 10330006; //艾卡納王
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(MobID,
                (short)(sActor.X + SagaLib.Global.Random.Next(1, 11)),
                (short)(sActor.Y + SagaLib.Global.Random.Next(1, 11)),
                2500, sActor);
            var mh = (MobEventHandler)mob.e;
            mh.AI.Mode = new AIMode(0);
            mh.AI.Mode.EventAttackingSkillRate = 0;
            var skill = new SumArcanaCardBuff(args, sActor, mob, lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class SumArcanaCardBuff : DefaultBuff
        {
            private readonly SkillArg arg;
            private readonly ActorMob mob;

            public SumArcanaCardBuff(SkillArg skill, Actor actor, ActorMob mob, int lifetime)
                : base(skill.skill, actor, "SumArcanaCard", lifetime, 6000)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                OnUpdate += TimeUpdate;
                this.mob = mob;
                arg = skill.Clone();
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                TimeUpdate(actor, skill);
            }

            private void TimeUpdate(Actor actor, DefaultBuff skill)
            {
                var rate = 60;
                var map = MapManager.Instance.GetMap(actor.MapID);
                var affected = map.GetActorsArea(mob, 600, false);
                foreach (var act in affected)
                    if (SkillHandler.Instance.CheckValidAttackTarget(actor, act))
                        if (SkillHandler.Instance.CanAdditionApply(actor, act, SkillHandler.DefaultAdditions.Confuse,
                                rate))
                        {
                            var skill2 = new Confuse(arg.skill, act, 3000);
                            SkillHandler.ApplyAddition(act, skill2);
                        }

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg, actor, false);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                mob.ClearTaskAddition();
                map.DeleteActor(mob);
            }
        }

        #endregion
    }
}