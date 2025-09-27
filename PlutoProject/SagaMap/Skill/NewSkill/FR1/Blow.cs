using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR1
{
    /// <summary>
    ///     乾坤一擊
    /// </summary>
    public class Blow : ISkill
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
            if (CheckPossible(sActor))
            {
                args.type = ATTACK_TYPE.BLOW;

                factor = 1.1f + 0.3f * level;
                if (level == 6)
                    factor = 10f;
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
        }

        #endregion
    }
}