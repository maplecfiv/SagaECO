using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     煙幕彈射擊（スロウグレネード）
    /// </summary>
    public class GrenadeSlow : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor) && CheckPossible(sActor))
            {
                if (SkillHandler.Instance.CountItem(sActor, 10053300) < 1)
                    return -2;
                return 0;
            }

            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.GUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.DUALGUN ||
                        pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.RIFLE ||
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.2f + 0.1f * level;
            var rate = 20 + 10 * level;
            args.argType = SkillArg.ArgType.Attack;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 100, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    if (SkillHandler.Instance.CanAdditionApply(sActor, i, SkillHandler.DefaultAdditions.鈍足, rate))
                    {
                        var skill1 = new MoveSpeedDown(args.skill, i, 6000);
                        SkillHandler.ApplyAddition(i, skill1);
                    }
        }

        //#endregion
    }
}