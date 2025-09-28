using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Eraser_肃清者____sco
{
    public class EraserMaster : ISkill
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
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SHORT_SWORD ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.CLAW
                        || pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN
                       )
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "EraserMaster", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value;
            value = 15 + skill.skill.Level * 15;

            if (skill.Variable.ContainsKey("EraserMasteryATK"))
                skill.Variable.Remove("EraserMasteryATK");
            skill.Variable.Add("EraserMasteryATK", value);
            actor.Status.min_atk2_skill += (short)value;
            actor.Status.min_atk3_skill += (short)value;
            actor.Status.max_atk2_skill += (short)value;
            actor.Status.max_atk3_skill += (short)value;

            int value_hit;
            value_hit = 10 + 6 * skill.skill.Level;
            if (skill.Variable.ContainsKey("EraserMasteryHIT"))
                skill.Variable.Remove("EraserMasteryHIT");
            skill.Variable.Add("EraserMasteryHIT", value_hit);
            actor.Status.hit_melee_skill += (short)value_hit;

            short cri;
            cri = (short)(3 * skill.skill.Level);
            if (skill.Variable.ContainsKey("EraserCriUp"))
                skill.Variable.Remove("EraserCriUp");
            skill.Variable.Add("EraserCriUp", cri);
            actor.Status.cri_skill += cri;

            var MartialArtDamUp = 128 + 6 * skill.skill.Level;
            if (skill.Variable.ContainsKey("EraserMasteryMartialArtDamUp"))
                skill.Variable.Remove("EraserMasteryMartialArtDamUp");
            skill.Variable.Add("EraserMasteryMartialArtDamUp", MartialArtDamUp);
            if (actor.Status.Additions.ContainsKey("MartialArtDamUp"))
                (actor.Status.Additions["MartialArtDamUp"] as DefaultPassiveSkill).Variable["MartialArtDamUp"] +=
                    MartialArtDamUp;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.min_atk2_skill -= (short)skill.Variable["EraserMasteryATK"];
            actor.Status.min_atk3_skill -= (short)skill.Variable["EraserMasteryATK"];
            actor.Status.max_atk2_skill -= (short)skill.Variable["EraserMasteryATK"];
            actor.Status.max_atk3_skill -= (short)skill.Variable["EraserMasteryATK"];
            actor.Status.hit_melee_skill -= (short)skill.Variable["EraserMasteryHIT"];
            actor.Status.cri_skill -= (short)skill.Variable["EraserCriUp"];
            if (actor.Status.Additions.ContainsKey("MartialArtDamUp"))
                (actor.Status.Additions["MartialArtDamUp"] as DefaultPassiveSkill).Variable["MartialArtDamUp"] -=
                    (short)skill.Variable["EraserMasteryMartialArtDamUp"];
            if (skill.Variable.ContainsKey("EraserMasteryMartialArtDamUp"))
                skill.Variable.Remove("EraserMasteryMartialArtDamUp");
        }

        #endregion
    }
}