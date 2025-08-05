using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.BountyHunter
{
    /// <summary>
    ///     星辰亂舞（ディスラプション）
    /// </summary>
    public class BeatUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            var rate = 20 + 5 * level;
            factor = 2.75f + 0.25f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 150, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse,
                            rate))
                    {
                        var skill5 = new Confuse(args.skill, act, 4000);
                        SkillHandler.ApplyAddition(act, skill5);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}