using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm
{
    /// <summary>
    ///     2段拔刀（二段抜刀）
    /// </summary>
    public class DoubleCutDown : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.SWORD, ItemType.SHORT_SWORD, ItemType.AXE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint NextSkillID = 2380;
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, level, 0));
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, level, 500));
        }

        //#endregion
    }
}