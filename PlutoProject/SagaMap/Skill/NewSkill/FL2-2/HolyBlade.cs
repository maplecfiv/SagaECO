using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_2
{
    public class HolyBlade : ISkill
    {
        //#region ISkill Members

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
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD ||
                        SkillHandler.Instance.CheckDEMRightEquip(sActor, ItemType.PARTS_BLOW) ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RAPIER)
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
            if (CheckPossible(sActor))
            {
                args.type = ATTACK_TYPE.SLASH;
                factor = 1.3f + 0.3f * level;
                if (level == 6)
                {
                    factor = 5f;
                    var silence = new Silence(args.skill, dActor, 1500);
                    SkillHandler.ApplyAddition(dActor, silence);
                    SkillHandler.Instance.ShowEffect((ActorPC)sActor, dActor, 4282);
                }

                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Holy, factor);
            }
        }

        //#endregion
    }
}