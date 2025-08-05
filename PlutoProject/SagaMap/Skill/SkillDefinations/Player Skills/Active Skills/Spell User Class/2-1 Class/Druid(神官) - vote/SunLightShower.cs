using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Druid
{
    /// <summary>
    ///     溫暖陽光（サンライトシャワー）
    /// </summary>
    public class SunLightShower : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f;
            for (var i = 1; i <= level; i++) factor += level / 10;
            factor *= -1;

            if (sActor.Status.Additions.ContainsKey("Cardinal")) //3转10技提升治疗量
                factor = factor + sActor.Status.Cardinal_Rank;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            short range = 350;
            var sActorPC = (ActorPC)sActor;
            if (sActorPC.PossessionTarget != 0) range = 150;
            var affected = map.GetActorsArea(sActor, range, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.PC && !SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Holy, factor);
        }

        #endregion
    }
}