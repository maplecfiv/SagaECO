using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR2_2
{
    /// <summary>
    ///     致殘攻擊(スロウブロウ)
    /// </summary>
    public class SlowArrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            args.type = ATTACK_TYPE.BLOW;
            factor = 1.4f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if ((args.flag[0] & AttackFlag.HP_DAMAGE) != 0)
            {
                var rate = 100;
                var lifetime = 0;
                switch (level)
                {
                    case 1:
                        lifetime = 6000;
                        break;
                    case 2:
                        lifetime = 7000;
                        break;
                    case 3:
                        lifetime = 8000;
                        break;
                    case 4:
                        lifetime = 9000;
                        break;
                    case 5:
                        lifetime = 10000;
                        break;
                }

                lifetime = SkillHandler.Instance.AdditionApply(sActor, dActor, rate, lifetime, SkillHandler.异常状态.迟钝);
                if (lifetime > 0)
                {
                    var skill = new MoveSpeedDown(args.skill, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }

        #endregion
    }
}