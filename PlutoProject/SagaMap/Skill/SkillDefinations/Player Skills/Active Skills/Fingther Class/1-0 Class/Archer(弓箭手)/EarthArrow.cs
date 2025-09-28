using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Archer_弓箭手_
{
    /// <summary>
    ///     大地箭
    /// </summary>
    public class EarthArrow : ISkill
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
            factor = 0.8f + 0.1f * level;
            var target = new List<Actor>();
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(dActor, 200, true);
            var affacted = new List<Actor>();
            byte MaxCount = 3;
            byte Count = 0;
            args.argType = SkillArg.ArgType.Attack;
            if (actors.Count > 0)
            {
                while (Count < MaxCount)
                    foreach (var i in actors)
                        if (Count < MaxCount)
                        {
                            Count += 1;
                            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                            {
                                affacted.Add(i);
                                map.SendEffect(i, 8034);
                            }
                        }

                SkillHandler.Instance.PhysicalAttack(sActor, affacted, args, Elements.Earth, factor);
            }
        }

        #endregion
    }
}