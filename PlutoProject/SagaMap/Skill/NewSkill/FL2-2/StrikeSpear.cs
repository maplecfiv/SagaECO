using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_2
{
    /// <summary>
    ///     比卡利之槍（ストライクスピア）
    /// </summary>
    public class StrikeSpear : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.0f + 0.4f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //向右的判定矩型
            short ox1 = 0;
            short oy1 = 100;
            short ox2 = 0;
            short oy2 = -100;
            short ox3 = 400;
            short oy3 = -100;
            short ox4 = 400;
            short oy4 = 100;
            //矩阵旋转
            var angel = map.DirChange(sActor.Dir) * Math.PI / 180;
            var x1 = (short)(ox1 * Math.Cos(angel) - oy1 * Math.Sin(angel));
            var y1 = (short)(ox1 * Math.Sin(angel) + oy1 * Math.Cos(angel));
            var x2 = (short)(ox2 * Math.Cos(angel) - oy2 * Math.Sin(angel));
            var y2 = (short)(ox2 * Math.Sin(angel) + oy2 * Math.Cos(angel));
            var x3 = (short)(ox3 * Math.Cos(angel) - oy3 * Math.Sin(angel));
            var y3 = (short)(ox3 * Math.Sin(angel) + oy3 * Math.Cos(angel));
            var x4 = (short)(ox4 * Math.Cos(angel) - oy4 * Math.Sin(angel));
            var y4 = (short)(ox4 * Math.Sin(angel) + oy4 * Math.Cos(angel));

            var actors = map.GetRectAreaActors(
                (short)(x1 + sActor.X), (short)(y1 + sActor.Y),
                (short)(x2 + sActor.X), (short)(y2 + sActor.Y),
                (short)(x3 + sActor.X), (short)(y3 + sActor.Y),
                (short)(x4 + sActor.X), (short)(y4 + sActor.Y));
            //Logger.getLogger().Error(actors.Count.ToString());
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);


            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }
    }
}