using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    public class MaceMastery : ISkill
    {
        //#region ISkill Members

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
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.HAMMER ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.STAFF)
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "MaceMastery", active);
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

            if (skill.Variable.ContainsKey("MaceMasteryATK"))
                skill.Variable.Remove("MaceMasteryATK");
            skill.Variable.Add("MaceMasteryATK", value);
            actor.Status.min_atk1_skill += (short)value;

            value = skill.skill.Level * 2;

            if (skill.skill.Level == 5)
                value += 2;

            if (skill.Variable.ContainsKey("MaceMasteryHIT"))
                skill.Variable.Remove("MaceMasteryHIT");
            skill.Variable.Add("MaceMasteryHIT", value);
            actor.Status.hit_melee_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["MaceMasteryATK"];
            actor.Status.min_atk1_skill -= (short)value;
            value = skill.Variable["MaceMasteryHIT"];
            actor.Status.hit_melee_skill -= (short)value;
        }

        //#endregion
    }
}