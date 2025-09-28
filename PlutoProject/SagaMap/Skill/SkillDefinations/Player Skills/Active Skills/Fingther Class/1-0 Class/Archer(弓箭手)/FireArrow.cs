using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Archer_弓箭手_
{
    /// <summary>
    ///     火燄箭
    /// </summary>
    public class FireArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
                return 0;
            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW ||
                        SkillHandler.Instance.CheckDEMRightEquip(sActor, ItemType.PARTS_BLOW))
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = 1.0f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Fire, factor);
        }

        #endregion
    }
}