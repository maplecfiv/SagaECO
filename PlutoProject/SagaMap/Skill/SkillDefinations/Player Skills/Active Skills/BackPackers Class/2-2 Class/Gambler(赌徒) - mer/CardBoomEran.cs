using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Gambler
{
    /// <summary>
    ///     卡飛旋鏢（カードブーメラン）
    /// </summary>
    public class CardBoomEran : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.CARD) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.05f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}