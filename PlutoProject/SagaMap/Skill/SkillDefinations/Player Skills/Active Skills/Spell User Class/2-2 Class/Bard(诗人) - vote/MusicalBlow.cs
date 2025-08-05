using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Bard
{
    /// <summary>
    ///     雄壯音樂演奏（ミュージカルブロウ）
    /// </summary>
    public class MusicalBlow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.2f + 0.2f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
        }

        #endregion
    }
}