using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Gardener_庭园师_
{
    /// <summary>
    ///     庭師の手腕
    /// </summary>
    public class GardenerSkill : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 10f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                {
                    var mob = (ActorMob)act;
                    if (SkillHandler.Instance.CheckMobType(mob, "plant")) realAffected.Add(act);
                }

            SkillHandler.Instance.FixAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}