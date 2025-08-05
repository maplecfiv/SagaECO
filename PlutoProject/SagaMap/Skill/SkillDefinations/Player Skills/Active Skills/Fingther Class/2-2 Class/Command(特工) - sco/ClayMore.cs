using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;
using SagaMap.Skill.SkillDefinations.Global;

namespace SagaMap.Skill.SkillDefinations.Command
{
    /// <summary>
    ///     大型地雷（クレイモアトラップ）
    /// </summary>
    public class ClayMore : Trap
    {
        public ClayMore()
            : base(true, 300, PosType.sActor)
        {
        }

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            uint itemID = 10022308;
            if (SkillHandler.Instance.CountItem(sActor, itemID) > 0)
            {
                SkillHandler.Instance.TakeItem(sActor, itemID, 1);
                return 0;
            }

            return -12;
        }

        public override void BeforeProc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            LifeTime = 20000 + 1000 * level;
        }

        public override void ProcSkill(Actor sActor, Actor mActor, ActorSkill actor, SkillArg args, Map map, int level,
            float factor)
        {
            var lifetime = 1500;

            var skill = new ClayMoreBuff(args, sActor, actor, lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class ClayMoreBuff : DefaultBuff
        {
            private readonly SkillArg args;
            private readonly Actor sActor;

            public ClayMoreBuff(SkillArg skill, Actor sActor, ActorSkill actor, int lifetime)
                : base(skill.skill, actor, "ClayMore", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.sActor = sActor;
                args = skill.Clone();
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var factor = 2.5f + 0.5f * skill.skill.Level;
                var map = MapManager.Instance.GetMap(sActor.MapID);
                var affected = map.GetActorsArea(sActor, 350, false);
                var realAffected = new List<Actor>();
                foreach (var act in affected)
                    if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                        realAffected.Add(act);

                SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, args, actor, false);
            }
        }
    }
}