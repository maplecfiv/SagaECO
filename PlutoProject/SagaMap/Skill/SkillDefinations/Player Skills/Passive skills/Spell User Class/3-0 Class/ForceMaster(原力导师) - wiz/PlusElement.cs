using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._3_0_Class.
    ForceMaster_原力导师____wiz
{
    public class PlusElement : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //bool active = true;
            //if (sActor.type == ActorType.PC)
            //{
            //    ActorPC pc = (ActorPC)sActor;
            //    if (pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.RIGHT_HAND) ||
            //        pc.Inventory.Equipments.ContainsKey(SagaDB.Item.EnumEquipSlot.LEFT_HAND))
            //    {
            //        if (pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.STAFF ||
            //            pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.itemType == SagaDB.Item.ItemType.STAFF)
            //        {
            //            active = true;
            //        }
            //    }
            //该技能没有任何武器要求    
            //}
            var skill = new DefaultPassiveSkill(args.skill, sActor, "PlusElement", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //ActorPC pc = (ActorPC)actor;
            //float rate = 0.02f+0.002f*skill.skill.Level;
            //int elements = pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Dark] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Earth] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Fire] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Holy] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Neutral] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Water] +
            //    pc.Inventory.Equipments[SagaDB.Item.EnumEquipSlot.RIGHT_HAND].BaseData.element[SagaLib.Elements.Wind];
            //pc.Status.PlusElement_rate += elements * rate;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //actor.Status.PlusElement_rate = 0;
        }

        #endregion
    }
}