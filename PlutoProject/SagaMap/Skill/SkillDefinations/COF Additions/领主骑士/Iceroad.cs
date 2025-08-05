using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.X
{
    public class Iceroad : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
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
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Water, 3f);

            //SkillHandler.Instance.FixAttack(sActor, affected, args, SagaLib.Elements.Water, 980);

            foreach (var item in affected)
            {
                var freeze = new Freeze(args.skill, item, 5000);
                SkillHandler.ApplyAddition(item, freeze);
            }


            for (var i = 0; i < 2; i++)
            {
                var xy = map.GetRandomPosAroundActor2(sActor);
                xy[0] = SagaLib.Global.PosX8to16(16, map.Width);
                xy[1] = SagaLib.Global.PosY8to16(79, map.Height);
                if (sActor.Slave.Count < 2)
                    sActor.Slave.Add(map.SpawnMob(82000003, xy[0], xy[1], 2500, sActor));
                else if (sActor.Slave.Count < 4)
                    sActor.Slave.Add(map.SpawnMob(82000004, xy[0], xy[1], 2500, sActor));
                else if (sActor.Slave[0].Buff.Dead)
                    sActor.Slave[0] = map.SpawnMob(82000003, xy[0], xy[1], 2500, sActor);
                else if (sActor.Slave[1].Buff.Dead)
                    sActor.Slave[1] = map.SpawnMob(82000003, xy[0], xy[1], 2500, sActor);
                else if (sActor.Slave[2].Buff.Dead)
                    sActor.Slave[2] = map.SpawnMob(82000004, xy[0], xy[1], 2500, sActor);
                else if (sActor.Slave[3].Buff.Dead)
                    sActor.Slave[3] = map.SpawnMob(82000004, xy[0], xy[1], 2500, sActor);
            }
        }

        #endregion
    }
}