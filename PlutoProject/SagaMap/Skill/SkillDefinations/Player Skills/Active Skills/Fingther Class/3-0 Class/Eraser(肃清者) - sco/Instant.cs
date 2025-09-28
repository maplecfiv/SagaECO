using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    /// <summary>
    ///     刹那
    /// </summary>
    public class Instant : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 12.5f + 5.5f * level;
            var critbonus = new[] { 0, 10, 15, 20, 30, 40 }[level];
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

            SkillHandler.Instance.SetNextComboSkill(sActor, 2516);
            args.type = sActor.Status.attackType;
            SkillHandler.Instance.PhysicalAttack(sActor, new List<Actor> { dActor }, args, SkillHandler.DefType.Def,
                sActor.WeaponElement, 0, factor, false, 0, false, 0, critbonus);
        }
    }
}