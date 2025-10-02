using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     追魂刃（追い討ちの刃）
    /// </summary>
    public class ChgstSwoDamUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] Values = { 0, 12, 14, 16, 18, 20 };
            var active = false;
            var value = Values[level];

            if (((ActorPC)sActor).Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                if (((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                    ItemType.SHORT_SWORD ||
                    ((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                    ItemType.SWORD ||
                    ((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                    ItemType.RAPIER)
                    active = true;

            //判斷對方異常狀態，傷害增加12% 14% 16% 18% 20% 
            var skill = new DefaultPassiveSkill(args.skill, sActor, "ChgstSwoDamUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        //#endregion
    }
}