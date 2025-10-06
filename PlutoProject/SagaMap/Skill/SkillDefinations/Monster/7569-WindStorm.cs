using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     魔物用劫雷灭世
    /// </summary>
    internal class WindStorm : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.8f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //Logger.ShowDebug(string.Format("劫雷滅世=>({0},{1})",args.x, args.y), Logger.defaultlogger);
            //Logger.ShowDebug(string.Format("劫雷滅世=>({0},{1})", SagaLib.Global.PosX8to16(args.x, map.Width), SagaLib.Global.PosY8to16(args.y, map.Height)), Logger.defaultlogger);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 300, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor);
        }

        //#endregion
    }
}