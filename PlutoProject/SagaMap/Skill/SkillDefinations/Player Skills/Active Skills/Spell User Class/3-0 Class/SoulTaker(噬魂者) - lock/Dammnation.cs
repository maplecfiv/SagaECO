using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    /// <summary>
    ///     3431 ダムネイション (Dammnation) 天谴
    /// </summary>
    public class Dammnation : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 9.0f + 4.5f * level;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                //检测3392可能存在的位置
                if (pc.Skills3.ContainsKey(3392) || pc.DualJobSkill.Exists(x => x.ID == 3392))
                {
                    //这里取副职的3392等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3392))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3392).Level;

                    //这里取主职的3392等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(3392))
                        mainlv = pc.Skills3[3392].Level;

                    //这里取等级最高的3392等级用来参与运算
                    factor += 0.5f * Math.Max(duallv, mainlv);
                    //factor += pc.Skills3[3392].Level * 0.5f;
                }

                //检测3420可能存在的位置
                if (pc.Skills3.ContainsKey(3420) || pc.DualJobSkill.Exists(x => x.ID == 3420))
                {
                    //这里取副职的3420等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3420))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3420).Level;

                    //这里取主职的3420等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(3420))
                        mainlv = pc.Skills3[3420].Level;

                    //这里取等级最高的3420等级用来参与运算
                    factor += 0.5f * Math.Max(duallv, mainlv);
                    //factor += pc.Skills3[3420].Level * 0.5f;
                }
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (sActor.type != ActorType.PC)
            {
                var actorszero = map.GetActorsArea(sActor, 700, true);
                foreach (var i in actorszero)
                    if (SagaLib.Global.Random.Next(1, actorszero.Count()) == 1)
                    {
                        dActor = i;
                        break;
                    }
            }

            var actors = map.GetActorsArea(dActor, 300, true);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Dark, factor);
        }
    }
}