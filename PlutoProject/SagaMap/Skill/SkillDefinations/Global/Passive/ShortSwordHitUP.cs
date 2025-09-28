using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    public class ShortSwordHitUP : ISkill
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
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SHORT_SWORD)
                        active = true;

                var skill = new DefaultPassiveSkill(args.skill, sActor, "ShortSwordHitUp", active);
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
                    value = 3;
                    break;
                case 2:
                    value = 6;
                    break;
                case 3:
                    value = 9;
                    break;
                case 4:
                    value = 13;
                    break;
                case 5:
                    value = 18;
                    break;
            }

            if (skill.Variable.ContainsKey("MasteryHIT"))
                skill.Variable.Remove("MasteryHIT");
            skill.Variable.Add("MasteryHIT", value);
            actor.Status.hit_melee_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (actor.type == ActorType.PC)
            {
                var value = skill.Variable["MasteryHIT"];
                actor.Status.hit_melee_skill -= (short)value;
            }
        }

        #endregion
    }
}