using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     自爆（自爆）
    /// </summary>
    public class MirrorSkill : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3000 * level;
            var skill = new MirrorSkillBuff(args, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        public class MirrorSkillBuff : DefaultBuff
        {
            private readonly SkillArg args;

            public MirrorSkillBuff(SkillArg args, Actor actor, int lifetime)
                : base(args.skill, actor, "MirrorSkill", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.args = args.Clone();
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var factor = SagaLib.Global.Random.Next(10, 400) / 100f;
                var map = MapManager.Instance.GetMap(actor.MapID);
                var affected = map.GetActorsArea(actor, 200, false);
                var realAffected = new List<Actor>();
                foreach (var act in affected)
                    if (SkillHandler.Instance.CheckValidAttackTarget(actor, act))
                        realAffected.Add(act);

                SkillHandler.Instance.PhysicalAttack(actor, realAffected, args, actor.WeaponElement, factor);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, args, actor, false);
            }
        }

        //#endregion
    }
}