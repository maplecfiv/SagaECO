using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Advance_Novice.Joker_小丑_
{
    /// <summary>
    ///     ジョーカースタイル
    /// </summary>
    public class JokerStyle : ISkill
    {
        private void StartJokerStyle1(Actor actor, DefaultBuff skill)
        {
            actor.Status.str_skill -= 10;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndJokerStyle1(Actor actor, DefaultBuff skill)
        {
            actor.Status.str_skill += 10;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }


        private void StartJokerStyle3(Actor actor, DefaultBuff skill)
        {
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndJokerStyle3(Actor actor, DefaultBuff skill)
        {
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            switch (args.skill.Level)
            {
                case 1:
                    return 0;
                    break;
                case 2:
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.BOW ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE)
                        return 0;
                    break;
                case 3:
                    if ((pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SWORD ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.AXE ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.HAMMER ||
                         pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SPEAR) &&
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        return 0;
                    break;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.argType = SkillArg.ArgType.Attack;
            switch (level)
            {
                case 1:
                    var skill = new DefaultBuff(args.skill, dActor, "JokerStyle1", 5000);
                    skill.OnAdditionStart += StartJokerStyle1;
                    skill.OnAdditionEnd += EndJokerStyle1;
                    SkillHandler.ApplyAddition(dActor, skill);
                    SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, 10.5f);
                    break;
                case 2:
                    if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.鈍足, 50))
                    {
                        var skill2 = new 鈍足(args.skill, dActor, 750 + 250 * level);
                        SkillHandler.ApplyAddition(dActor, skill2);
                    }

                    var dest = new List<Actor>();
                    for (var i = 0; i < 3; i++)
                        dest.Add(dActor);
                    args.delayRate = 4.5f;
                    SkillHandler.Instance.PhysicalAttack(sActor, dest, args, sActor.WeaponElement, 3.0f);
                    break;
                case 3:
                    var skill3 = new DefaultBuff(args.skill, dActor, "JokerStyle3", 5000);
                    skill3.OnAdditionStart += StartJokerStyle3;
                    skill3.OnAdditionEnd += EndJokerStyle3;
                    SkillHandler.ApplyAddition(dActor, skill3);
                    SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, 10.5f);
                    break;
            }
        }

        #endregion
    }
}