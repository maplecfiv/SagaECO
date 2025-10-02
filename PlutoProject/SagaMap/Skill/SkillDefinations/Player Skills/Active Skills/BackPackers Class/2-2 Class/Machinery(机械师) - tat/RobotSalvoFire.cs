using System.Collections.Generic;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     爆炎煉獄（ミサイル一斉発射）
    /// </summary>
    public class RobotSalvoFire : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            //消耗：10026800
            uint ItemID = 10026800;
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
            {
                var ItemCount = SkillHandler.Instance.CountItem(sActor, ItemID);
                if (ItemCount > 0)
                {
                    SkillHandler.Instance.TakeItem(sActor, ItemID, 1);
                    return 0;
                }

                return -57;
            }

            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.28f + 0.08f * level;
            short[] range = { 0, 600, 600, 600, 700, 700 };
            int[] times = { 0, 6, 6, 7, 7, 8 };
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, range[level], true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    for (var i = 0; i < times[level]; i++)
                        realAffected.Add(act);

            if (realAffected.Count > 0)
            {
                var finalAffected = new List<Actor>();
                for (var i = 0; i < realAffected.Count; i++)
                    finalAffected.Add(realAffected[SagaLib.Global.Random.Next(0, realAffected.Count - 1)]);
                SkillHandler.Instance.PhysicalAttack(sActor, finalAffected, args, sActor.WeaponElement, factor);
            }
        }

        //#endregion
    }
}