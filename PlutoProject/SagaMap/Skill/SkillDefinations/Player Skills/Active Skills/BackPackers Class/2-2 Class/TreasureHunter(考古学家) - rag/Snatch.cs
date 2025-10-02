using System;
using System.Linq;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     偷天換日（スナッチ）
    /// </summary>
    public class Snatch : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.8f + 0.2f * level;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
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

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 3 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate && !dActor.Status.Additions.ContainsKey("Snatch"))
                if (dActor.type == ActorType.MOB)
                {
                    var mob = (ActorMob)dActor;
                    if (!SkillHandler.Instance.isBossMob(mob))
                    {
                        //偷東西!!
                        var skill = new DefaultBuff(args.skill, dActor, "Snatch", int.MaxValue);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(dActor, skill);
                    }
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        //#endregion
    }
}