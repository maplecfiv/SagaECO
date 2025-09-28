using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     化工廠（ケミカルプラント）[接續技能]
    /// </summary>
    public class SumChemicalPlant2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var factor = 1.0f + 3.0f * level;
            var mob = (ActorMob)sActor.Slave[0];
            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 150, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                    affected.Add(item);
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
            sActor.Slave.Remove(mob);
            map.DeleteActor(mob);
            //float factor = 0.5f + 1.5f * level;
            //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
            //ActorMob mob = (ActorMob)sActor.Slave[0];
            //List<Actor> affected = map.GetActorsArea(dActor, 150, false);
            //List<Actor> realAffected = new List<Actor>();
            //foreach (Actor act in affected)
            //{
            //    if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
            //    {
            //        realAffected.Add(act);
            //    }
            //}
            //SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
            //sActor.Slave.Remove(mob);
            //map.DeleteActor(mob);
        }

        #endregion
    }
}