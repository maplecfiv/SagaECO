using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Archer_弓箭手_
{
    /// <summary>
    ///     寒冰箭
    /// </summary>
    public class WaterArrow : ISkill
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
            factor = 1.0f + 0.2f * level;
            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            //取得有效Actor（即怪物）
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, Elements.Water, factor);
            foreach (var i in affected)
                if (SkillHandler.Instance.CanAdditionApply(sActor, i, SkillHandler.DefaultAdditions.Frosen, 10 * level))
                {
                    var skill = new Freeze(args.skill, i, 3000);
                    SkillHandler.ApplyAddition(i, skill);
                }
        }

        #endregion
    }
}