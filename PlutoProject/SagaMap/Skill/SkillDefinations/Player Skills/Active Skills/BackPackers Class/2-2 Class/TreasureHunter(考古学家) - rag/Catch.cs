using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     鞭子拖拉（キャッチ）
    /// </summary>
    public class Catch : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                return (dActor.type == ActorType.MOB && SkillHandler.Instance.isBossMob((ActorMob)dActor)) ? -14 : 0;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pos = new short[2];
            pos[0] = sActor.X;
            pos[1] = sActor.Y;
            //SkillHandler.Instance.FixAttack(sActor, dActor, args, sActor.WeaponElement, 1f);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var lifetime = 1000;
            var dskill = new Stiff(args.skill, dActor, lifetime);
            //拖拽逻辑试修复
            map.MoveActor(Map.MOVE_TYPE.START, dActor, pos, dActor.Dir, 20000, true, MoveType.BATTLE_MOTION);
            SkillHandler.ApplyAddition(dActor, dskill);
        }

        #endregion
    }
}