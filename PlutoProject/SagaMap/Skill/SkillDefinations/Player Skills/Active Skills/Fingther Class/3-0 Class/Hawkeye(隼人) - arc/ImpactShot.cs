using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    /// <summary>
    ///     ジリオンブレイド
    /// </summary>
    public class ImpactShot : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcLongAttack(sActor);
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcArrowAndBulletDown(sActor);
            var factor = new[] { 0, 9f, 9.5f, 10f, 10.5f, 11f }[level];
            var movenum = new[] { 0, 0, 2, 3, 4, 5 }[level];
            var Stuntime = new[] { 0, 2500, 3500, 3500, 4500, 5000 }[level];

            var actors = MapManager.Instance.GetMap(sActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            foreach (var item in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, item))
                {
                    affected.Add(item);
                    if (SkillHandler.Instance.CanAdditionApply(sActor, item, SkillHandler.DefaultAdditions.Stun,
                            5 * level + 50))
                    {
                        var skill = new Stun(args.skill, item, Stuntime);
                        SkillHandler.ApplyAddition(item, skill);
                    }
                }

            args.type = ATTACK_TYPE.STAB;
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
            if (level > 1)
                SkillHandler.Instance.PushBack(dActor, sActor, movenum, 20000, MoveType.VANISH);
        }
    }
}