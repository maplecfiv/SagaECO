using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_1
{
    public class TwoHandMastery : ISkill
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
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) &&
                    pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD &&
                        pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SWORD)
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "TwoHandMastery", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = 0;
            switch (skill.skill.Level)
            {
                case 1:
                    value = 30;
                    break;
                case 2:
                    value = 35;
                    break;
                case 3:
                    value = 40;
                    break;
                case 4:
                    value = 45;
                    break;
                case 5:
                    value = 50;
                    break;
            }

            if (skill.Variable.ContainsKey("MasteryATK"))
                skill.Variable.Remove("MasteryATK");
            skill.Variable.Add("MasteryATK", value);
            actor.Status.min_atk2_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["MasteryATK"];
            actor.Status.min_atk2_skill -= (short)value;
        }

        #endregion
    }
}