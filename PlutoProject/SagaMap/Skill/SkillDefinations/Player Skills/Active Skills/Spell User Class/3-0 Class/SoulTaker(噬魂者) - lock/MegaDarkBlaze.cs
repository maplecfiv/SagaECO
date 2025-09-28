using System;
using System.Collections.Generic;
using System.Linq;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    public class MegaDarkBlaze : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.PossessionTarget > 0) return -25;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2f + 2f * level;
            //不管是主职还是副职, 只要习得黑暗火焰技能，进行判断
            if (sActor.type == ActorType.PC)
            {
                var pc = sActor as ActorPC;
                if (pc.Skills2_1.ContainsKey(3310) || pc.DualJobSkill.Exists(x => x.ID == 3310))
                {
                    //这里取副职的黑暗火焰等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3310))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3310).Level;

                    //这里取主职的黑暗火焰等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(3310))
                        mainlv = pc.Skills2_1[3310].Level;

                    //这里取等级最高的黑暗火焰等级用来倍率加成
                    factor += 2.5f + 0.5f * Math.Max(duallv, mainlv);
                }
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 550, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Dark, factor);
        }
    }
}