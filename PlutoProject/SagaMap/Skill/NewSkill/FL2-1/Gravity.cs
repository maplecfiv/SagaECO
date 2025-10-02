using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_1
{
    /// <summary>
    ///     重力波（グラヴィティ）
    /// </summary>
    public class Gravity : ISkill
    {
        public List<int> range = new List<int>();

        public Gravity()
        {
            range.Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 2));
            range.Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 2));
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f + 0.5f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 250, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                //梦美：就先不改范围了
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    int XDiff, YDiff;
                    SkillHandler.Instance.GetXYDiff(map, sActor, act, out XDiff, out YDiff);
                    if (range.Contains(SkillHandler.Instance.CalcPosHashCode(XDiff, YDiff, 2)))
                    {
                        realAffected.Add(act);
                        SkillHandler.Instance.PushBack(sActor, act, 4);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
            var args2 = args.Clone();
            SkillHandler.Instance.MagicAttack(sActor, realAffected, args2, Elements.Earth, factor);
            args.AddSameActor(args2);
        }

        //#endregion
    }
}