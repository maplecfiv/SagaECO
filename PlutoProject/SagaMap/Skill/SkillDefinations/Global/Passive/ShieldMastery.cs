using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    /// <summary>
    ///     盾術修練（シールドマスタリー）
    /// </summary>
    public class ShieldMastery : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                if (sActor.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD)
                    return 0;
            return -1;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var pc = sActor as ActorPC;
            if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.LEFT_HAND))
                active = pc.Inventory.Equipments[EnumEquipSlot.LEFT_HAND].BaseData.itemType == ItemType.SHIELD;

            var skill = new DefaultPassiveSkill(args.skill, sActor, "ShieldMastery", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var guradadd = 5 * skill.skill.Level;
            if (skill.Variable.ContainsKey("ShieldMastery"))
                skill.Variable.Remove("ShieldMastery");
            skill.Variable.Add("ShieldMastery", guradadd);
            actor.Status.guard_skill += (short)guradadd;
        }

        public void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value2 = skill.Variable["ShieldMastery"];
            actor.Status.guard_skill -= (short)value2;
        }

        //#endregion
    }
}