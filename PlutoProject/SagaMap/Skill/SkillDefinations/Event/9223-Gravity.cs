﻿using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     スキル石?重力波（スキル石?グラヴィティ）
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

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            level = 5;
            var factor = 2.0f + 0.5f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                //還要去掉外圍的角色
                /*
                 * 範圍內有複數敵人時，傷害也不會分散
                 *
                 * 攻擊範圍：
                 *
                 * □□■□□　　☆：使用者
                 * □■■■□　　■：攻擊判定
                 * ■■☆■■
                 * □■■■□
                 * □□■□□
                 *
                 */
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

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, Elements.Neutral, factor);
        }

        #endregion
    }
}