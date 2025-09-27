using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    internal class WindExplosion : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            if (sActor.type != ActorType.PC) level = 5;
            switch (level)
            {
                case 1:
                    factor = 3.0f;
                    break;
                case 2:
                    factor = 6.0f;
                    break;
                case 3:
                    factor = 9.0f;
                    break;
                case 4:
                    factor = 12.0f;
                    break;
                case 5:
                    factor = 15.0f;
                    break;
            }

            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_1.ContainsKey(3261) || pc.DualJobSkill.Exists(x => x.ID == 3261))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3261))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3261).Level;

                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(3261))
                        mainlv = pc.Skills2_1[3261].Level;

                    factor += 0.5f * Math.Max(duallv, mainlv);
                }

                if (pc.Skills2_1.ContainsKey(3264) || pc.DualJobSkill.Exists(x => x.ID == 3264))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3264))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3264).Level;

                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(3264))
                        mainlv = pc.Skills2_1[3264].Level;

                    factor += 0.5f * Math.Max(duallv, mainlv);
                }
            }
            //ActorPC Me = (ActorPC)sActor;
            //if (Me.Skills2.ContainsKey(3261))//Caculate the factor according to skill FireStorm.
            //{
            //    TotalLv = Me.Skills2[3261].BaseData.lv;
            //    switch (level)
            //    {
            //        case 1:
            //            factor += 0.5f;
            //            break;
            //        case 2:
            //            factor += 1.0f;
            //            break;
            //        case 3:
            //            factor += 1.5f;
            //            break;
            //        case 4:
            //            factor += 2.0f;
            //            break;
            //        case 5:
            //            factor += 2.5f;
            //            break;

            //    }
            //}
            //if (Me.SkillsReserve.ContainsKey(3261))//Caculate the factor according to skill FireStorm.
            //{
            //    TotalLv = Me.SkillsReserve[3261].BaseData.lv;
            //    switch (level)
            //    {
            //        case 1:
            //            factor += 0.5f;
            //            break;
            //        case 2:
            //            factor += 1.0f;
            //            break;
            //        case 3:
            //            factor += 1.5f;
            //            break;
            //        case 4:
            //            factor += 2.0f;
            //            break;
            //        case 5:
            //            factor += 2.5f;
            //            break;

            //    }
            //}
            //if (Me.Skills2.ContainsKey(3264))//Caculate the factor according to skill FireStorm.
            //{
            //    TotalLv = Me.Skills2[3264].BaseData.lv;
            //    switch (level)
            //    {
            //        case 1:
            //            factor += 0.5f;
            //            break;
            //        case 2:
            //            factor += 1.0f;
            //            break;
            //        case 3:
            //            factor += 1.5f;
            //            break;
            //        case 4:
            //            factor += 2.0f;
            //            break;
            //        case 5:
            //            factor += 2.5f;
            //            break;

            //    }
            //}
            //if (Me.SkillsReserve.ContainsKey(3264))//Caculate the factor according to skill FireStorm.
            //{
            //    TotalLv = Me.SkillsReserve[3264].BaseData.lv;
            //    switch (level)
            //    {
            //        case 1:
            //            factor += 0.5f;
            //            break;
            //        case 2:
            //            factor += 1.0f;
            //            break;
            //        case 3:
            //            factor += 1.5f;
            //            break;
            //        case 4:
            //            factor += 2.0f;
            //            break;
            //        case 5:
            //            factor += 2.5f;
            //            break;

            //    }
            //}
            var actorS = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 350, null);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor);
        }

        #endregion
    }
}