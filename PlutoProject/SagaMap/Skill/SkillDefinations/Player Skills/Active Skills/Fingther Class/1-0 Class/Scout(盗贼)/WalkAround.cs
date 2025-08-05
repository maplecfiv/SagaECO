using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Scout_盗贼_
{
    /// <summary>
    ///     卑劣的襲擊（回り込み）
    /// </summary>
    public class WalkAround : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //SkillHandler.Instance.Warp(sActor, 2, 2000, SagaLib.MoveType.JUMP);
            byte x, y;
            SkillHandler.Instance.GetTBackPos(MapManager.Instance.GetMap(sActor.MapID), dActor, out x, out y);

            var pos = new short[2];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            pos[0] = SagaLib.Global.PosX8to16(x, map.Width);
            pos[1] = SagaLib.Global.PosY8to16(y, map.Height);
            map.MoveActor(Map.MOVE_TYPE.START, sActor, pos, (ushort)(dActor.Dir / 45), 20000, true, MoveType.QUICKEN);

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, 50))
            {
                var skill = new Stiff(args.skill, dActor, 3000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        #endregion
    }
}