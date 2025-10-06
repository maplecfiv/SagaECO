using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    /// <summary>
    ///     ピレッジアクト
    /// </summary>
    public class PillageAct : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            factor = new[] { 0, 0.5f, 3.0f, 8.0f, 13.0f, 18.0f }[level];

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    realAffected.Add(act);
                    if (act.type == ActorType.MOB)
                    {
                        //预留偷窃逻辑位置,等级越低成功率越高
                    }
                }

            SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}