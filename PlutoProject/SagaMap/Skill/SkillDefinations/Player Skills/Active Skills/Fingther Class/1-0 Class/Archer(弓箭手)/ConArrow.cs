using System.Collections.Generic;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Archer_弓箭手_
{
    public class ConArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var numdown = 2;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                        {
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown) return 0;

                            return -55;
                        }

                        return -34;
                    }

                    return -34;
                }

                return -5;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var numdown = 2;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);
            }

            var combo = 2;
            float factor = 0;
            args.argType = SkillArg.ArgType.Attack;
            switch (level)
            {
                case 1:
                    factor = 0.9f;
                    break;
                case 2:
                    factor = 1.0f;
                    break;
                case 3:
                    factor = 1.1f;
                    break;
                case 4:
                    factor = 1.2f;
                    break;
                case 5:
                    factor = 1.3f;
                    break;
                case 6:
                    factor = 2.2f;
                    break;
            }

            var target = new List<Actor>();
            args.delayRate = 2f;
            for (var i = 0; i < combo; i++) target.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, target, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}