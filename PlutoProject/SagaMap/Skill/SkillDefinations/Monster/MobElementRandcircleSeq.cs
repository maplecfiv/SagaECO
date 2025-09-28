using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     各種精靈的憤怒 [接續技能]
    /// </summary>
    public class MobElementRandcircleSeq : ISkill
    {
        private readonly Elements e;

        public MobElementRandcircleSeq(Elements element)
        {
            e = element;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.0f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            short[] xy =
            {
                (short)SagaLib.Global.Random.Next(sActor.X - 200, sActor.X + 200),
                (short)SagaLib.Global.Random.Next(sActor.Y - 200, sActor.Y + 200)
            };
            var actors = map.GetActorsArea(xy[0], xy[1], 100, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, e, factor);
            args.dActor = 0xffffffff;
            args.x = SagaLib.Global.PosX16to8(xy[0], map.Width);
            args.y = SagaLib.Global.PosY16to8(xy[1], map.Height);
        }

        #endregion
    }
}