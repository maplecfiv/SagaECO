﻿using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     一閃（一閃）
    /// </summary>
    public class MobIsSeN : ISkill
    {
        public Dictionary<SkillHandler.ActorDirection, List<int>> range =
            new Dictionary<SkillHandler.ActorDirection, List<int>>();

        public MobIsSeN()
        {
            init();
        }


        #region Init

        public void init()
        {
            //建立List
            for (var i = 0; i < 8; i++) range.Add((SkillHandler.ActorDirection)i, new List<int>());
            //塞入內容

            #region RangePos

            //North
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 2, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 2, 2));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 2));
            //NorthEast
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 2));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 2));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 2));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 2));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 2));
            //East
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 2));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 2));
            //SouthEast
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 2));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 2));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 2));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 2));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 2));
            //South
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -2, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -2, 2));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 2));
            //SouthWest
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 2));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 2));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 2));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 2));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 2));
            //West
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 2));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 2));
            //NorthWest
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 2));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 2));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 2));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 2));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 2));

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
            float factor = 0;
            factor = 3.0f;

            var rate = 30 + 10 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 250, false);
            var realAffected = new List<Actor>();
            var dir = SkillHandler.Instance.GetDirection(sActor);
            foreach (var act in affected)
                //需去掉不在範圍內的 - 完成
                /*
                 * ■■■■■　□■□□□　　☆：使用者
                 * □■■■□　□■■□□　　■：攻撃判定
                 * □□☆□□　□☆■■□
                 *
                 */
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    int XDiff, YDiff;
                    SkillHandler.Instance.GetXYDiff(map, sActor, act, out XDiff, out YDiff);
                    if (range[dir].Contains(SkillHandler.Instance.CalcPosHashCode(XDiff, YDiff, 2)))
                    {
                        realAffected.Add(act);
                        if (act.type == ActorType.PC)
                            if (SagaLib.Global.Random.Next(0, 99) < rate)
                                SkillHandler.Instance.PossessionCancel((ActorPC)act, PossessionPosition.NONE);
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}