using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.NewBoss
{
    internal class B1 : ISkill
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
            //7个！
            /*3水光4
             2地无风5
              7暗火6*/
            short effectrange = 200;
            short damagerange = 300;
            var effectx1 = 0;
            var effecty1 = 0;
            var effectx2 = -2 * effectrange;
            var effecty2 = 0;
            var effectx3 = -effectrange;
            var effecty3 = (int)(effectrange * Math.Sqrt(3));
            int effectx4 = effectrange;
            var effecty4 = (int)(effectrange * Math.Sqrt(3));
            var effectx5 = 2 * effectrange;
            var effecty5 = 0;
            int effectx6 = effectrange;
            var effecty6 = (int)(-effectrange * Math.Sqrt(3));
            var effectx7 = -effectrange;
            var effecty7 = (int)(-effectrange * Math.Sqrt(3));
            var damagex1 = (short)(effectx1 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey1 = (short)(effecty1 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex2 = (short)(effectx2 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey2 = (short)(effecty2 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex3 = (short)(effectx3 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey3 = (short)(effecty3 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex4 = (short)(effectx4 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey4 = (short)(effecty4 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex5 = (short)(effectx5 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey5 = (short)(effecty5 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex6 = (short)(effectx6 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey6 = (short)(effecty6 + SagaLib.Global.PosY8to16(args.y, map.Height));
            var damagex7 = (short)(effectx7 + SagaLib.Global.PosX8to16(args.x, map.Width));
            var damagey7 = (short)(effecty7 + SagaLib.Global.PosY8to16(args.y, map.Height));
            /*
            SkillArg args1 = args.Clone();
            SkillArg args2 = args.Clone();
            SkillArg args3 = args.Clone();
            SkillArg args4 = args.Clone();
            SkillArg args5 = args.Clone();
            SkillArg args6 = args.Clone();
            SkillArg args7 = args.Clone();
            args1.x = SagaLib.Global.PosX16to8((short)damagex1, map.Width);
            args1.y = SagaLib.Global.PosY16to8((short)damagey1, map.Height);
            args2.x = SagaLib.Global.PosX16to8((short)damagex2, map.Width);
            args2.y = SagaLib.Global.PosY16to8((short)damagey2, map.Height);
            args3.x = SagaLib.Global.PosX16to8((short)damagex3, map.Width);
            args3.y = SagaLib.Global.PosY16to8((short)damagey3, map.Height);
            args4.x = SagaLib.Global.PosX16to8((short)damagex4, map.Width);
            args4.y = SagaLib.Global.PosY16to8((short)damagey4, map.Height);
            args5.x = SagaLib.Global.PosX16to8((short)damagex5, map.Width);
            args5.y = SagaLib.Global.PosY16to8((short)damagey5, map.Height);
            args6.x = SagaLib.Global.PosX16to8((short)damagex6, map.Width);
            args6.y = SagaLib.Global.PosY16to8((short)damagey6, map.Height);
            args7.x = SagaLib.Global.PosX16to8((short)damagex7, map.Width);
            args7.y = SagaLib.Global.PosY16to8((short)damagey7, map.Height);
            */
            var list1 = map.GetRoundAreaActors(damagex1, damagey1, damagerange);
            var list2 = map.GetRoundAreaActors(damagex2, damagey2, damagerange);
            var list3 = map.GetRoundAreaActors(damagex3, damagey3, damagerange);
            var list4 = map.GetRoundAreaActors(damagex4, damagey4, damagerange);
            var list5 = map.GetRoundAreaActors(damagex5, damagey5, damagerange);
            var list6 = map.GetRoundAreaActors(damagex6, damagey6, damagerange);
            var list7 = map.GetRoundAreaActors(damagex7, damagey7, damagerange);
            var affected1 = new List<Actor>();
            var affected2 = new List<Actor>();
            var affected3 = new List<Actor>();
            var affected4 = new List<Actor>();
            var affected5 = new List<Actor>();
            var affected6 = new List<Actor>();
            var affected7 = new List<Actor>();
            foreach (var i in list1)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected1.Add(i);
            foreach (var i in list2)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected2.Add(i);
            foreach (var i in list3)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected3.Add(i);
            foreach (var i in list4)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected4.Add(i);
            foreach (var i in list5)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected5.Add(i);
            foreach (var i in list6)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected6.Add(i);
            foreach (var i in list7)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected7.Add(i);

            ////应该乘个旋转矩阵
            //SkillArg args1 = args.Clone();
            var args2 = args.Clone();
            var args3 = args.Clone();
            var args4 = args.Clone();
            var args5 = args.Clone();
            var args6 = args.Clone();
            var args7 = args.Clone();
            SkillHandler.Instance.MagicAttack(sActor, affected1, args, sActor.WeaponElement, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected2, args2, Elements.Earth, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected3, args3, Elements.Water, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected4, args4, Elements.Holy, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected5, args5, Elements.Wind, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected6, args6, Elements.Fire, factor);
            SkillHandler.Instance.MagicAttack(sActor, affected7, args7, Elements.Dark, factor);

            args.Add(args2);
            args.Add(args3);
            args.Add(args4);
            args.Add(args5);
            args.Add(args6);
            args.Add(args7);

            var arg = new EffectArg();

            arg.effectID = 5622;
            arg.x = SagaLib.Global.PosX16to8(damagex1, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey1, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);

            arg.effectID = 5225;
            arg.x = SagaLib.Global.PosX16to8(damagex2, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey2, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            arg.effectID = 5613;
            arg.x = SagaLib.Global.PosX16to8(damagex3, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey3, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            arg.effectID = 5065;
            arg.x = SagaLib.Global.PosX16to8(damagex4, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey4, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            arg.effectID = 5604;
            arg.x = SagaLib.Global.PosX16to8(damagex5, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey5, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            arg.effectID = 5307;
            arg.x = SagaLib.Global.PosX16to8(damagex6, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey6, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            arg.effectID = 5619;
            arg.x = SagaLib.Global.PosX16to8(damagex7, map.Width);
            arg.y = SagaLib.Global.PosY16to8(damagey7, map.Height);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
        }

        #endregion
    }
}