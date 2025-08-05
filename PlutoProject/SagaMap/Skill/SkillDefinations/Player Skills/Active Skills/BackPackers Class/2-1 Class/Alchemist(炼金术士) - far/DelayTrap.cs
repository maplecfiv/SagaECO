using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     計時炸彈（ディレイボム）
    /// </summary>
    public class DelayTrap : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            uint itemID = 10022307; //計時炸彈
            if (SkillHandler.Instance.CountItem(sActor, itemID) > 0)
            {
                SkillHandler.Instance.TakeItem(sActor, itemID, 1);
                return 0;
            }

            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 2000 + 2000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var skill = new DelayTrapBuff(args, sActor, lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class DelayTrapBuff : DefaultBuff
        {
            private readonly SkillArg args;
            private short x, y;

            public DelayTrapBuff(SkillArg args, Actor actor, int lifetime)
                : base(args.skill, actor, "DelayTrap", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.args = args.Clone();
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                x = actor.X;
                y = actor.Y;
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;

                var map = MapManager.Instance.GetMap(actor.MapID);
                var affected = map.GetActorsArea(x, y, 150, null);
                var factor = 1.0f + 1.0f * level;
                var realAffected = new List<Actor>();
                foreach (var act in affected)
                    if (SkillHandler.Instance.CheckValidAttackTarget(actor, act))
                        realAffected.Add(act);

                factor *= 1f / realAffected.Count;
                SkillHandler.Instance.PhysicalAttack(actor, realAffected, args, actor.WeaponElement, factor);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, args, actor, true);
            }
        }

        #endregion
    }
}