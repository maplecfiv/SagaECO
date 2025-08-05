using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Bard
{
    /// <summary>
    ///     軍隊音樂會（ゲリラライブ）
    /// </summary>
    public class ChangeMusic : ISkill
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
        }

        #endregion
    }
}