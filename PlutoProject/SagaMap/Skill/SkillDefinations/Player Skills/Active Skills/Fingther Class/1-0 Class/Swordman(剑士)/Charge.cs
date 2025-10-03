using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     彈飛
    /// </summary>
    public class Charge : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD)
                    return 0;
            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) ||
                    pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                    return true;
                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            args.dActor = sActor.ActorID;
            if (CheckPossible(sActor))
            {
                args.type = ATTACK_TYPE.BLOW;
                factor = 1.0f + 0.3f * level;

                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);

                //推开1格
                SkillHandler.Instance.PushBack(sActor, dActor, 2);
            }
        }

        //#endregion
    }
}