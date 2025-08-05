using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Gunner
{
    /// <summary>
    ///     榴彈射擊（グレネードショット）
    /// </summary>
    public class GrenadeShot : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (CheckPossible(sActor))
            {
                if (SkillHandler.Instance.CountItem(sActor, 10053300) < 1) //榴弹
                    return -2;
                return 0;
            }

            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE ||
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 3.6f, 3.8f, 4.6f }[level];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(dActor, 100, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}