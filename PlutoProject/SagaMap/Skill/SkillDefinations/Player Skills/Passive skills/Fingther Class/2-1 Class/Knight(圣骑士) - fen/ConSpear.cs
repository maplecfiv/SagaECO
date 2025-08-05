using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    public class ConSpear : ISkill
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
                    pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                {
                    if ((pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SPEAR &&
                         pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SPEAR) ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RAPIER)
                        active = true;
                    var skill = new DefaultPassiveSkill(args.skill, sActor, "ConSpear", active);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(sActor, skill);
                }
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill += (short)(2 * level);
            actor.Status.combo_skill = 2;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill -= (short)(2 * level);
            actor.Status.combo_skill = 1;
        }

        #endregion
    }
}