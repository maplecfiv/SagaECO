using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     Joker_2
    /// </summary>
    public class JokerTwoHead : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 8.5f, 10.5f, 12.5f, 14.5f, 16.5f }[level];

            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            int elements;
            if (sActor.WeaponElement != Elements.Neutral)
                elements = sActor.Status.attackElements_item[sActor.WeaponElement]
                           + sActor.Status.attackElements_skill[sActor.WeaponElement]
                           + sActor.Status.attackelements_iris[sActor.WeaponElement];
            else
                elements = 0;
            var rete = new[] { 0, 0.01f, 0.03f, 0.05f, 0.07f, 0.1f }[level];
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                {
                    var dmg = SkillHandler.Instance.CalcDamage(true, sActor, dActor, args, SkillHandler.DefType.Def,
                        sActor.WeaponElement, elements, factor);
                    SkillHandler.Instance.CauseDamage(sActor, dActor, dmg);
                    SkillHandler.Instance.ShowVessel(dActor, dmg);
                }
            //args.type = ATTACK_TYPE.SLASH;
            //SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }
    }
}