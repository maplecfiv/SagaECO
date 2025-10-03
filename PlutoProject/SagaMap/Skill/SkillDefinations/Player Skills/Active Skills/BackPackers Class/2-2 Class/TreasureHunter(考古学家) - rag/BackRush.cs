using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     纏繞捆綁（バックラッシュ）
    /// </summary>
    public class BackRush : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (dActor.type == ActorType.MOB)
                {
                    var actorMob = (ActorMob)dActor;
                    if (SkillHandler.Instance.isBossMob(actorMob)) return -14;
                }

                return 0;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 2000 + 1000 * level;
            var dskill = new Stiff(args.skill, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, dskill);
            var sskill = new Stiff(args.skill, sActor, lifetime);
            SkillHandler.ApplyAddition(sActor, sskill);
        }

        //#endregion
    }
}