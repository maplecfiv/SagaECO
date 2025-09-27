using System;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     音速鞭子（ソニックウィップ）最终阶段(弹飞)
    /// </summary>
    public class SonicWhipSEQ2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (!SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return -5;
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
            {
                var factor = 0.3f + 0.25f * level;
                if (sActor.type == ActorType.PC)
                {
                    var pc = (ActorPC)sActor;
                    if (pc.Skills3.ContainsKey(992) || pc.DualJobSkill.Exists(x => x.ID == 992))
                    {
                        var duallv = 0;
                        if (pc.DualJobSkill.Exists(x => x.ID == 992))
                            duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 992).Level;
                        var mainlv = 0;
                        if (pc.Skills3.ContainsKey(992))
                            mainlv = pc.Skills3[992].Level;
                        factor += 0.1f * Math.Max(duallv, mainlv);
                    }
                }

                args.argType = SkillArg.ArgType.Attack;
                args.type = ATTACK_TYPE.SLASH;
                //List<Actor> dest = new List<Actor>();
                SkillHandler.Instance.PushBack(sActor, dActor, 4);
                //dest.Add(dActor);

                args.delayRate = 4.5f;
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
        }

        #endregion
    }
}