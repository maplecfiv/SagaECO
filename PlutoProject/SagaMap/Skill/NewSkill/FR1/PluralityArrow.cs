using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Network.Client;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR1
{
    /// <summary>
    ///     多連箭（バラージアロー）
    /// </summary>
    public class PluralityArrow : ISkill
    {
        #region ISkill Members

        private int numdownmin = 0;
        private int numdownmax = 0;
        private int numdown;

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var numdownmin = new[] { 0, 3, 4, 4, 5, 5 };
            var numdownmax = new[] { 0, 2, 2, 3, 3, 4 };
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.ARROW)
                        {
                            numdown = SagaLib.Global.Random.Next(numdownmin[args.skill.Level],
                                numdownmin[args.skill.Level]);
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

            int[] times = { 0, 3, 4, 5, 5, 5 };
            var factor = 1.2f;
            var target = new List<Actor>();
            for (var i = 0; i < times[level]; i++) target.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, target, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}