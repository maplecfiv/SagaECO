using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     麻痺攻擊（スタンブロウ）
    /// </summary>
    public class StunBlow : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            args.type = ATTACK_TYPE.BLOW;
            factor = 1.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if (level > 1 && (args.flag[0] & AttackFlag.HP_DAMAGE) != 0)
            {
                var rate = 0;
                var lifetime = 0;
                switch (level)
                {
                    case 2:
                        rate = 10;
                        lifetime = 3000;
                        break;
                    case 3:
                        rate = 15;
                        lifetime = 4000;
                        break;
                    case 4:
                        rate = 22;
                        lifetime = 5000;
                        break;
                    case 5:
                        rate = 30;
                        lifetime = 6000;
                        break;
                }

                if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun, rate))
                {
                    var skill = new Stun(args.skill, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }

        //#endregion
    }
}