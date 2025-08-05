using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Swordman
{
    /// <summary>
    ///     暈眩攻擊
    /// </summary>
    public class ConfuseBlow : ISkill
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
                var lifetime = 6000;
                switch (level)
                {
                    case 2:
                        rate = 120;
                        lifetime = 7000;
                        break;
                    case 3:
                        rate = 140;
                        lifetime = 8000;
                        break;
                    case 4:
                        rate = 160;
                        lifetime = 9000;
                        break;
                    case 5:
                        rate = 180;
                        lifetime = 10000;
                        break;
                }

                lifetime = SkillHandler.Instance.AdditionApply(sActor, dActor, rate, lifetime, SkillHandler.异常状态.混乱);
                if (lifetime > 0)
                {
                    var skill = new Confuse(args.skill, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }

        #endregion
    }
}