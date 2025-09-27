using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     小丑的寂寞
    /// </summary>
    public class JokerArt : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 15.0f, 23.0f, 31.0f, 39.0f, 47.0f }[level];

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 400, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            var random = SagaLib.Global.Random.Next(0, 9999);
            if (random <= 7000)
            {
                SkillHandler.Instance.PhysicalAttack(sActor, sActor, args, sActor.WeaponElement, 1000.0f);
            }
            else
            {
                var dmg = (int)(sActor.HP - 1);
                SkillHandler.Instance.CauseDamage(sActor, sActor, dmg);
                SkillHandler.Instance.ShowVessel(dActor, dmg);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}