using System;
using System.Linq;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     強運（一か八か）
    /// </summary>
    public class RandDamOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 0.1f, 3.4f, 4.8f, 6.2f, 7.77f }[level];
            var rate = 6000 + 500 * level;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Skills3.ContainsKey(1114) || pc.DualJobSkill.Exists(x => x.ID == 1114)) //幸运女神
                {
                    //这里取副职的等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 1114))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 1114).Level;

                    //这里取主职的等级
                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(1114))
                        mainlv = pc.Skills2_2[1114].Level;

                    rate += Math.Max(duallv, mainlv) * 300;
                    factor += Math.Max(duallv, mainlv) * 4.44f;
                }
            }

            if (SagaLib.Global.Random.Next(0, 10000) < rate)
            {
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
            else
            {
                sActor.HP = 1;
                var lifetime = 1000 + 1000 * level;
                var skill = new Stun(args.skill, sActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
                var map = MapManager.Instance.GetMap(sActor.MapID);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor, true);
            }
        }

        #endregion
    }
}