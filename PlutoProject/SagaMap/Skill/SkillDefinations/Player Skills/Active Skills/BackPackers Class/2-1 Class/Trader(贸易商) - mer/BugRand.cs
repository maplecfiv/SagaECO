using System.Collections.Generic;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     連續重擊（ビートスマッシュ）
    /// </summary>
    public class BugRand : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.HANDBAG) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.5f + 0.2f * level;
            int[] min_Times = { 1, 1, 1, 2, 2 };
            int[] max_Times = { 2, 2, 3, 3, 3 };
            var times = SagaLib.Global.Random.Next(0, 1) == 0 ? min_Times[level] : max_Times[level];
            var dest = new List<Actor>();
            for (var i = 0; i < times; i++) dest.Add(dActor);
            SkillHandler.Instance.PhysicalAttack(sActor, dest, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}