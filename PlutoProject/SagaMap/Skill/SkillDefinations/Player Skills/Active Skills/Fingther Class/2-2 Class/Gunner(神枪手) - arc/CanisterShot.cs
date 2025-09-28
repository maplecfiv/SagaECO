﻿using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     散彈（散弾）
    /// </summary>
    public class CanisterShot : ISkill, MobISkill
    {
        private bool MobUse;

        public Dictionary<SkillHandler.ActorDirection, List<int>> range =
            new Dictionary<SkillHandler.ActorDirection, List<int>>();

        public CanisterShot(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region Init

        public CanisterShot()
        {
            MobUse = false;
        }

        private void init()
        {
            range.Clear();
            //建立List
            for (var i = 0; i < 8; i++) range.Add((SkillHandler.ActorDirection)i, new List<int>());
            //塞入內容

            #region RangePos

            //North
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 2, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 2, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(3, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(2, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(0, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-1, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-2, 3, 3));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(-3, 3, 3));
            //NorthEast
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, 0, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 2, 3));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 3, 3));
            //East
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 2, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 3, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 2, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, 0, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, -1, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, -2, 3));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(3, -3, 3));
            //SouthEast
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, 0, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(3, 0, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(2, -1, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, -2, 3));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, -3, 3));
            //South
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(2, -2, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -2, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -2, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -2, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(3, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(2, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(1, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(0, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-2, -3, 3));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-3, -3, 3));
            //SouthWest
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, 0, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-0, -1, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-0, -2, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, -2, 3));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-0, -3, 3));
            //West
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 2, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, -1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-2, -2, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 3, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 2, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, 0, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, -1, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, -2, 3));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-3, -3, 3));
            //NorthWest
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 0, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-3, 0, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-2, 1, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, 2, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 2, 3));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, 3, 3));

            #endregion
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcGunAndBullet(sActor);
        }

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcBulletDown(sActor);
            init();
            var factor = 1.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            var dir = SkillHandler.Instance.GetDirection(sActor);
            foreach (var act in affected)
                //需去掉不在範圍內的 - 完成
                /*
                 * ■■■■■■■　　□■□□□□　☆：使用者
                 * □■■■■■□　　□■■□□□　■：效果範圍
                 * □□■■■□□　　□■■■□□
                 * □□□☆□□□　　□☆■■■□
                 *
                 */
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    int XDiff, YDiff;
                    SkillHandler.Instance.GetXYDiff(map, sActor, act, out XDiff, out YDiff);
                    if (range[dir].Contains(SkillHandler.Instance.CalcPosHashCode(XDiff, YDiff, 3)))
                        realAffected.Add(act);
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}