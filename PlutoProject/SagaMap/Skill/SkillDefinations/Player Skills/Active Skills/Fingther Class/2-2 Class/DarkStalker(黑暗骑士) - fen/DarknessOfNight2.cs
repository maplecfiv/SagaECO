﻿using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     黑暗騎士（ダークネスオブナイト）[接續技能]
    /// </summary>
    public class DarknessOfNight2 : ISkill
    {
        public Dictionary<SkillHandler.ActorDirection, List<int>> range =
            new Dictionary<SkillHandler.ActorDirection, List<int>>();

        #region Init

        public DarknessOfNight2()
        {
            //建立List
            for (var i = 0; i < 8; i++) range.Add((SkillHandler.ActorDirection)i, new List<int>());
            //塞入內容

            #region RangePos

            //North
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 2, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 2, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 3, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 3, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 3, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 4, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 4, 4));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 4, 4));
            //NorthEast
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 2, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, 2, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 3, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, 3, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(4, 3, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, 4, 4));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(4, 4, 4));
            //East
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 0, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, -1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(4, 1, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(4, 0, 4));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(4, -1, 4));
            //SouthEast
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, -2, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, -2, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -3, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, -3, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(4, -3, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, -4, 4));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(4, -4, 4));
            //South
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -2, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -2, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -3, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -3, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -3, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -4, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -4, 4));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -4, 4));
            //SouthWest
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, -2, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, -2, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -3, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, -3, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-4, -3, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, -4, 4));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-4, -4, 4));
            //West
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 0, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, -1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-4, 1, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-4, 0, 4));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-4, -1, 4));
            //NorthWest
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 2, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, 2, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 3, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, 3, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-4, 3, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, 4, 4));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-4, 4, 4));

            #endregion
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 0.76f, 1.20f, 1.68f, 2.16f, 2.64f };
            var factor = factors[level];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 400, false);
            var realAffected = new List<Actor>();
            var dir = SkillHandler.Instance.GetDirection(sActor);
            foreach (var act in affected)
                //需去掉不在範圍內的 - 完成
                /*
                 * □□□□□□　□□□■■□
                 * □□■■■□　□□■■■□
                 * □☆■■■□　□■■■□□
                 * □□■■■□　□☆■□□□
                 * □□□□□□　□□□□□□
                 *
                 */
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    int XDiff, YDiff;
                    SkillHandler.Instance.GetXYDiff(map, sActor, act, out XDiff, out YDiff);
                    if (range[dir].Contains(SkillHandler.Instance.CalcPosHashCode(XDiff, YDiff, 4)))
                        realAffected.Add(act);
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}