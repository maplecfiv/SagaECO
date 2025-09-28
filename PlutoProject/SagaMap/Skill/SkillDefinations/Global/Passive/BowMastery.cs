using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    public class BowMastery : ISkill
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
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW)
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "BowMastery", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value;
            value = skill.skill.Level * 5;
            if (skill.skill.Level == 5)
                value += 5;

            if (skill.Variable.ContainsKey("MasteryATK"))
                skill.Variable.Remove("MasteryATK");
            skill.Variable.Add("MasteryATK", value);
            actor.Status.min_atk3_skill += (short)value;

            var value2 = skill.skill.Level * 2;
            if (skill.skill.Level == 5)
                value2 += 2;

            if (skill.Variable.ContainsKey("MasteryHIT"))
                skill.Variable.Remove("MasteryHIT");
            skill.Variable.Add("MasteryHIT", value2);
            actor.Status.hit_ranged_skill += (short)value2;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["MasteryATK"];
            actor.Status.min_atk3_skill -= (short)value;
            var value2 = skill.Variable["MasteryHIT"];
            actor.Status.hit_ranged_skill -= (short)value2;
        }

        #endregion
    }
}