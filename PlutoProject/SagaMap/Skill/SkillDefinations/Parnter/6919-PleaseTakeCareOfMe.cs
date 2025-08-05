using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     お引取り下さい
    /// </summary>
    public class PleaseTakeCareOfMe : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 10.0f;
            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                {
                    affected.Add(item);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, item, SkillHandler.DefaultAdditions.Stun, 40))
                    {
                        var skill = new Stun(args.skill, item, 2000);
                        SkillHandler.ApplyAddition(item, skill);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}