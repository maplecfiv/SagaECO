using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     機銃砲・マシンナーズアリア
    /// </summary>
    public class MachineNurseAria : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 6.0f;

            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                    affected.Add(item);
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }
    }
}