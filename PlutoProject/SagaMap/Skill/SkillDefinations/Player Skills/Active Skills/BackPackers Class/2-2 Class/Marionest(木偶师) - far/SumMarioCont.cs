﻿using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Marionest
{
    /// <summary>
    ///     召喚活動木偶皇帝 [接續技能]
    /// </summary>
    public class SumMarioCont : ISkill
    {
        private readonly Elements element;

        public SumMarioCont(Elements e)
        {
            element = e;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.7f + 3f * level;
            if (sActor is ActorPC)
            {
                var pc = sActor as ActorPC;

                //不管是主职还是副职, 只要习得技能
                if (pc.Skills3.ContainsKey(993) || pc.DualJobSkill.Exists(x => x.ID == 993))
                {
                    //这里取副职的等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 993))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 993).Level;

                    //这里取主职的等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(993))
                        mainlv = pc.Skills3[993].Level;

                    //这里取等级最高的等级用来做倍率加成
                    factor += new[] { 0, 1.0f, 1.6f, 2.4f, 3.1f, 3.8f }[Math.Max(duallv, mainlv)];
                }
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(dActor, 200, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, element, factor);
            var mob = (ActorMob)sActor.Slave[0];
            map.DeleteActor(mob);
            sActor.Slave.RemoveAt(0);
        }

        #endregion
    }
}