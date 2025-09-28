using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    public class SoulTakerMaster : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) ||
                    pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                {
                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                        if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.AXE ||
                            pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.HAMMER ||
                            pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD)
                            active = true;

                    if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                        if (pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.AXE ||
                            pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.HAMMER ||
                            pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SWORD)
                            active = true;
                }

                var skill = new DefaultPassiveSkill(args.skill, sActor, "SoulTakerMaster", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = 0;
            var value2 = 0;
            var value3 = 0;
            switch (skill.skill.Level)
            {
                case 1:
                    value = 30;
                    value2 = 16;
                    value3 = 6;
                    break;
                case 2:
                    value = 45;
                    value2 = 22;
                    value3 = 12;
                    break;
                case 3:
                    value = 60;
                    value2 = 28;
                    value3 = 18;
                    break;
                case 4:
                    value = 75;
                    value2 = 34;
                    value3 = 24;
                    break;
                case 5:
                    value = 90;
                    value2 = 40;
                    value3 = 30;
                    break;
            }

            if (skill.Variable.ContainsKey("SoulTakerMaster"))
                skill.Variable.Remove("SoulTakerMaster");
            skill.Variable.Add("SoulTakerMaster", value);
            actor.Status.min_atk1_skill += (short)value;
            actor.Status.min_atk2_skill += (short)value;
            actor.Status.min_atk3_skill += (short)value;
            if (skill.Variable.ContainsKey("SoulTakerMaster2"))
                skill.Variable.Remove("SoulTakerMaster2");
            skill.Variable.Add("SoulTakerMaster2", value2);
            actor.Status.hit_melee_skill += (short)value2;

            if (skill.Variable.ContainsKey("SoulTakerMaster3"))
                skill.Variable.Remove("SoulTakerMaster3");
            skill.Variable.Add("SoulTakerMaster3", value3);
            actor.Status.cri_skill += (short)value3;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (actor.type == ActorType.PC)
            {
                var value = skill.Variable["SoulTakerMaster"];
                actor.Status.min_atk1_skill -= (short)value;
                actor.Status.min_atk2_skill -= (short)value;
                actor.Status.min_atk3_skill -= (short)value;
                actor.Status.hit_melee_skill -= (short)skill.Variable["SoulTakerMaster2"];
                actor.Status.cri_skill -= (short)skill.Variable["SoulTakerMaster3"];
            }
        }

        #endregion
    }
}