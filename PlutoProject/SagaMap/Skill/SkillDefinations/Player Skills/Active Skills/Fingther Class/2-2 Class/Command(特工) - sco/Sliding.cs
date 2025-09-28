using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     滑動追擊（スライディング）
    /// </summary>
    public class Sliding : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            /*
             * 無拍子中
             * 攻擊移動路線中的敵人
             * 如果攻擊範圍內有複數敵人，傷害會被分散
             * 命中率判定為角色遠距命中
             * 三格距離以外，會有傷害衰減
             * 在移動中途被攻擊時，會馬上停止移動
             * 此為設置系技能
             *
             */
            var pos = new short[2];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            pos[0] = SagaLib.Global.PosX8to16(args.x, map.Width);
            pos[1] = SagaLib.Global.PosY8to16(args.y, map.Height);
            map.MoveActor(Map.MOVE_TYPE.START, sActor, pos, sActor.Dir, 20000, true, MoveType.QUICKEN);
        }

        #endregion
    }
}