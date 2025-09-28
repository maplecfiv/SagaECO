using System;
using System.Collections.Generic;
using System.Linq;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     鞭子瞬連（ウィップラッシュ）
    /// </summary>
    public class ConthWhip : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 0.1f * level;

            int[] attackTimes = { 0, 2, 3, 4, 5, 6, 10 };
            if (sActor is ActorPC)
            {
                var pc = sActor as ActorPC;
                if (pc.Skills3.ContainsKey(992) || pc.DualJobSkill.Exists(x => x.ID == 992))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 992))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 992).Level;

                    //这里取主职的剑圣等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(992))
                        mainlv = pc.Skills3[992].Level;

                    //这里取等级最高的剑圣等级用来做居合的倍率加成
                    factor += 0.1f * Math.Max(duallv, mainlv);
                }
            }

            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.SLASH;
            var dest = new List<Actor>();
            for (var i = 0; i < attackTimes[level]; i++) dest.Add(dActor);

            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dest, args, sActor.WeaponElement, factor);
            //float factor = 0.9f;
            //int times = 1 + level;
            //List<Actor> realAffected = new List<Actor>();
            //for (int i = 0; i < times; i++)
            //{
            //    realAffected.Add(dActor);
            //}
            //SkillHandler.Instance.PhysicalAttack(sActor, realAffected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}