using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class SwordHitUP : ISkill
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
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND)) active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "SwordHitUp", active);
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
                    value = 2;
                    break;
                case 2:
                    value = 5;
                    break;
                case 3:
                    value = 8;
                    break;
                case 4:
                    value = 11;
                    break;
                case 5:
                    value = 15;
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