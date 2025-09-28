using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     極惡暴風雨
    /// </summary>
    public class MobWindRandcircleSeq2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var xy = map.GetRandomPosAroundActor(sActor);
            var actors = map.GetActorsArea(xy[0], xy[1], 300, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor);
            args.dActor = 0xffffffff;
            args.x = SagaLib.Global.PosX16to8(xy[0], map.Width);
            args.y = SagaLib.Global.PosY16to8(xy[1], map.Height);
        }

        #endregion
    }
}