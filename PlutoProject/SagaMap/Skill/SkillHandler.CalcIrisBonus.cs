using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill
{
    public partial class SkillHandler
    {
        private float
            CalcIrisDmgFactor(Actor sActor, Actor dActor, Elements skillelement, int skilltype,
                bool heal) /*skilltype=0,物理；1，魔法*/
        {
            var res = 1f;
            if (sActor.type == ActorType.PC)
            {
                var spc = (ActorPC)sActor;
                foreach (var i in spc.IrisAbilityLevels.Keys)
                {
                    var a = false;
                    switch (i.ID)
                    {
                        //bool depend on sActor
                        case 2001: //初心者光环
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2021: //融会贯通
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2041: //巅峰武艺
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2101: //全力以赴
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2151: //底力
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2300: //无之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2310: //火之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2320: //水之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2330: //风之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2340: //地之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2350: //光之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2360: //暗之信仰
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2400: //短剑大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2405: //爪类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2410: //锤类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2415: //斧类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2420: //鞭类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2425: //剑类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2430: //细剑大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2435: //矛类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2440: //弓类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2445: //手枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2450: //步枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2455: //双枪大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2460: //书类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2465: //杖类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2470: //琴类大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2475: //包包大师
                            if (spc.Level <= 60) a = true;
                            break;
                        case 2480: //神奇大师
                            if (spc.Level <= 60) a = true;
                            break;
                        //bool depend on dActor
                    }
                }
            }

            if (dActor.type == ActorType.PC)
            {
            }

            return res;
        }

        private float CalcIrisHitFactor(Actor sActor, Actor dActor)
        {
            var res = 1f;
            if (sActor.type == ActorType.PC)
            {
            }

            if (dActor.type == ActorType.PC)
            {
            }

            return res;
        }

        private float CalcIrisSkillCastSpeedFactor(Actor sActor)
        {
            var res = 1f;
            if (sActor.type == ActorType.PC)
            {
            }

            return res;
        }

        private float CalcIrisSkillCostFactor(Actor sActor)
        {
            var res = 1f;
            if (sActor.type == ActorType.PC)
            {
            }

            return res;
        }

        private float CalcIrisExpFactor(Actor sActor, Actor dActor)
        {
            var res = 1f;
            if (sActor.type == ActorType.PC)
            {
            }

            return res;
        }
    }
}