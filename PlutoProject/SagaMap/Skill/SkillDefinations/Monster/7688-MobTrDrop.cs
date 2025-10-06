using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     解開憑依
    /// </summary>
    public class MobTrDrop : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 50; //也许设置概率改为50%更合适?
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (act.type == ActorType.PC)
                        if (SagaLib.Global.Random.Next(0, 99) < rate)
                            SkillHandler.Instance.PossessionCancel((ActorPC)act, PossessionPosition.NONE);
        }

        //#endregion
    }
}