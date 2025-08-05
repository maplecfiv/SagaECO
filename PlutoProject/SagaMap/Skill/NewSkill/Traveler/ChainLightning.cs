using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Traveler
{
    internal class ChainLightning : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 1;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //向右的判定矩型
            short ox1 = 0;
            short oy1 = 100;
            short ox2 = 0;
            short oy2 = -100;
            short ox3 = 700;
            short oy3 = -100;
            short ox4 = 700;
            short oy4 = 100;
            //矩阵旋转
            var angel = map.DirChange(sActor.Dir) * Math.PI / 180;
            //double angel=sActor.Dir * Math.PI / 180;
            //Logger.ShowError(angel.ToString());
            var x1 = (short)(ox1 * Math.Cos(angel) - oy1 * Math.Sin(angel));
            var y1 = (short)(ox1 * Math.Sin(angel) + oy1 * Math.Cos(angel));
            var x2 = (short)(ox2 * Math.Cos(angel) - oy2 * Math.Sin(angel));
            var y2 = (short)(ox2 * Math.Sin(angel) + oy2 * Math.Cos(angel));
            var x3 = (short)(ox3 * Math.Cos(angel) - oy3 * Math.Sin(angel));
            var y3 = (short)(ox3 * Math.Sin(angel) + oy3 * Math.Cos(angel));
            var x4 = (short)(ox4 * Math.Cos(angel) - oy4 * Math.Sin(angel));
            var y4 = (short)(ox4 * Math.Sin(angel) + oy4 * Math.Cos(angel));
            //Logger.ShowError(x1.ToString() + "," + y1.ToString() + " " + 
            //  x2.ToString() + "," + y2.ToString() + " " + 
            //x3.ToString() + "," + y3.ToString() + " " + 
            //x4.ToString() + "," + y4.ToString());
            var actors = map.GetRectAreaActors(
                (short)(x1 + sActor.X), (short)(y1 + sActor.Y),
                (short)(x2 + sActor.X), (short)(y2 + sActor.Y),
                (short)(x3 + sActor.X), (short)(y3 + sActor.Y),
                (short)(x4 + sActor.X), (short)(y4 + sActor.Y));

            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            //SkillHandler.Instance.MagicAttack(sActor, affected, args, SagaLib.Elements.Holy, factor);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor);
        }

        #endregion
    }
}