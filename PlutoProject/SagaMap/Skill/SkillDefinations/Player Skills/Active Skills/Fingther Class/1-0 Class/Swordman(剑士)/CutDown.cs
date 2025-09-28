using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     橫砍（なぎ払い）
    /// </summary>
    public class CutDown : ISkill
    {
        public Dictionary<SkillHandler.ActorDirection, List<int>> range =
            new Dictionary<SkillHandler.ActorDirection, List<int>>();

        #region Init

        public CutDown()
        {
            //建立List
            for (var i = 0; i < 8; i++) range.Add((SkillHandler.ActorDirection)i, new List<int>());
            //塞入內容

            #region RangePos

            //North
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 1));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 1));
            range[SkillHandler.ActorDirection.North].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 1));
            //NorthEast
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, 1));
            range[SkillHandler.ActorDirection.NorthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 1));
            //East
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 1, 1));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 1));
            range[SkillHandler.ActorDirection.East].Add(SkillHandler.Instance.CalcPosHashCode(1, -1, 1));
            //SouthEast
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(0, 1, -1));
            range[SkillHandler.ActorDirection.SouthEast].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 1));
            //South
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 1));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 1));
            range[SkillHandler.ActorDirection.South].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 1));
            //SouthWest
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 1));
            range[SkillHandler.ActorDirection.SouthWest].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 1));
            //West
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 1, 1));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, 0, 1));
            range[SkillHandler.ActorDirection.West].Add(SkillHandler.Instance.CalcPosHashCode(-1, -1, 1));
            //NorthWest
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(0, -1, 1));
            range[SkillHandler.ActorDirection.NorthWest].Add(SkillHandler.Instance.CalcPosHashCode(1, 0, 1));

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
            float factor = factor = 1.3f + 0.3f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            var realAffected = new List<Actor>();
            var dir = SkillHandler.Instance.GetDirection(sActor);
            foreach (var act in affected)
                //需去掉部分人物 - 完成
                /*
                 * 有效範圍（正面）
                 * ■■■
                 * □☆□
                 * □□□
                 *
                 * 有效範圍（側面）
                 * □■□
                 * □☆■
                 * □□□
                 *
                 */
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    int XDiff, YDiff;
                    SkillHandler.Instance.GetXYDiff(map, sActor, act, out XDiff, out YDiff);
                    if (range[dir].Contains(SkillHandler.Instance.CalcPosHashCode(XDiff, YDiff, 1)))
                        realAffected.Add(act);
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}