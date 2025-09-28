using System.Collections.Generic;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     3連發精密射擊（バーストショット）
    /// </summary>
    public class BurstShot : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            var numdown = 3;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
            {
                if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    {
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                        {
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

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var numdown = 3;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                            if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.BULLET)
                                if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Stack >= numdown)
                                    for (var i = 0; i < numdown; i++)
                                        MapClient.FromActorPC(pc)
                                            .DeleteItem(pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].Slot, 1,
                                                false);
            }

            var factor = 1.0f + 0.1f * level;
            var rate = 5 * level;
            args.argType = SkillArg.ArgType.Attack;
            var affected = new List<Actor>();
            for (var i = 0; i < 3; i++) affected.Add(dActor);
            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "BurstShot", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "BurstShot", 15000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //近命中
            actor.Status.hit_melee_skill = -20;

            //遠命中
            actor.Status.hit_ranged_skill = -20;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.hit_melee_skill -= -20;

            //遠命中
            actor.Status.hit_ranged_skill -= -20;
        }

        #endregion
    }
}