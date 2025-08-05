using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_2
{
    public class ShieldBash : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1 + 0.2f * level;
            args.type = ATTACK_TYPE.BLOW;

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if ((args.flag[0] & AttackFlag.HP_DAMAGE) != 0)
            {
                var rate = 100;
                var lifetime = 0;
                switch (level)
                {
                    case 1:
                        lifetime = 8000;
                        break;
                    case 2:
                        lifetime = 9000;
                        break;
                    case 3:
                        lifetime = 10000;
                        break;
                    case 4:
                        lifetime = 11000;
                        break;
                    case 5:
                        lifetime = 12000;
                        break;
                }

                lifetime = SkillHandler.Instance.AdditionApply(sActor, dActor, rate, lifetime, SkillHandler.异常状态.昏迷,
                    ((ActorPC)sActor).Str);
                if (lifetime > 0)
                {
                    var skill = new Stun(args.skill, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }
    }
}