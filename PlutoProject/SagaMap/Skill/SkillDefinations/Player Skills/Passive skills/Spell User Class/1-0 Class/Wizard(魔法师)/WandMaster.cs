using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    public class WandMaster : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                    pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.STAFF)
                    active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "WandMaster", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var rate = 0.02f * skill.skill.Level;
            var add = (int)(((ActorPC)actor).Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.matk * rate);
            if (skill.Variable.ContainsKey("WandMasterMatk"))
                skill.Variable.Remove("WandMasterMatk");
            skill.Variable.Add("WandMasterMatk", add);
            actor.Status.min_matk_skill += (short)add;
            actor.Status.max_matk_skill += (short)add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.min_matk_skill -= (short)skill.Variable["WandMasterMatk"];
            actor.Status.max_matk_skill -= (short)skill.Variable["WandMasterMatk"];
        }

        #endregion
    }
}