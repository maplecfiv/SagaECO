using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     防具解除（アーマーキャプチャー）
    /// </summary>
    public class ArmorRemove : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.01f;
            if (dActor.type == ActorType.PC)
            {
                var dePossessionRate = 10 + 10 * level;
                if (SagaLib.Global.Random.Next(0, 99) < dePossessionRate)
                {
                    var actor = (ActorPC)dActor;
                    SkillHandler.Instance.PossessionCancel(actor, PossessionPosition.CHEST);
                }
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}