using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     精靈命中
    /// </summary>
    public class EleHitUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] Values = { 0, 3, 6, 9, 12, 15 }; //%
            var active = false;
            var value = Values[level];
            if (sActor.type == ActorType.PC)
                if (((ActorPC)sActor).Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                        ItemType.SHORT_SWORD ||
                        ((ActorPC)sActor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType ==
                        ItemType.HAMMER)
                        active = true;

            var skill = new DefaultPassiveSkill(args.skill, dActor, "破戒", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}