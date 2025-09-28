using System.Collections.Generic;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     散彈射擊（バラージショット）
    /// </summary>
    public class PluralityShot : ISkill
    {
        #region ISkill Members

        private readonly int[] numdownmin = { 0, 2, 2, 3, 3, 4 };
        private readonly int[] numdownmax = { 0, 2, 3, 4, 5, 6 };
        private readonly int[] numdownmindouble = { 0, 3, 3, 4, 4, 5 };
        private readonly int[] numdownmaxdouble = { 0, 3, 4, 5, 6, 7 };
        private int numdown;

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            //numdown = SagaLib.Global.Random.Next();
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                        {
                            numdown = SagaLib.Global.Random.Next(numdownmin[args.skill.Level],
                                numdownmax[args.skill.Level]);
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown) return 0;

                            return -56;
                        }

                        return -35;
                    }

                    return -35;
                }

                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                        {
                            numdown = SagaLib.Global.Random.Next(numdownmindouble[args.skill.Level],
                                numdownmaxdouble[args.skill.Level]);
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown) return 0;

                            return -56;
                        }

                        return -35;
                    }

                    return -35;
                }

                return -5;
            }

            return -5;
        }

        //bool CheckPossible(Actor sActor)
        //{
        //    if (sActor.type == ActorType.PC)
        //    {
        //        ActorPC pc = (ActorPC)sActor;
        //        if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND))
        //        {
        //            if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.GUN ||
        //                pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.DUALGUN ||
        //                pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.RIFLE ||
        //                pc.Inventory.GetContainer(SagaDB.Item.ContainerType.RIGHT_HAND2).Count > 0)
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    else
        //        return true;
        //}
        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                    {
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);
                    }
                    else if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN)
                    {
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);
                    }
                }
            }

            var factor = 1.2f;
            args.argType = SkillArg.ArgType.Attack;
            var target = new List<Actor>();
            for (var i = 0; i < numdown; i++) target.Add(dActor);
            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, target, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}