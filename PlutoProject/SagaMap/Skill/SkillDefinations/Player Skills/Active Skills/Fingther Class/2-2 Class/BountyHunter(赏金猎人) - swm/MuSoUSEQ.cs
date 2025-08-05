using System;
using System.Linq;
using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    public class MuSoUSEQ : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0, 0.2f, 0.45f, 0.7f, 0.95f, 1.2f };
            var factor = factors[level];
            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.SLASH;
            if (sActor is ActorPC)
            {
                var lv = 0;
                var pc = sActor as ActorPC;
                //不管是主职还是副职, 只要习得剑圣技能, 都会导致combo成立, 这里一步就行了
                if (pc.Skills3.ContainsKey(1117) || pc.DualJobSkill.Exists(x => x.ID == 1117))
                {
                    //lv = pc.Skills3[1117].Level;
                    //这里取副职的剑圣等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 1117))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 1117).Level;

                    //这里取主职的剑圣等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(1117))
                        mainlv = pc.Skills3[1117].Level;

                    //这里取等级最高的剑圣等级用来做倍率加成
                    factor += 0.1f + 0.1f * Math.Max(duallv, mainlv);
                    //factor += (0.1f + 0.1f *level);

                    //神速斩
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2527);
                }
            }

            if (sActor.MuSoUCount == 10) args.skill.BaseData.nAnim1 = args.skill.BaseData.nAnim2 = 332;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var skill = new Stiff(args.skill, dActor, 1000);
            SkillHandler.ApplyAddition(dActor, skill);
            sActor.MuSoUCount++;
            if (sActor.MuSoUCount == 10)
                SkillHandler.Instance.PushBack(sActor, dActor, 2);
        }
    }
}