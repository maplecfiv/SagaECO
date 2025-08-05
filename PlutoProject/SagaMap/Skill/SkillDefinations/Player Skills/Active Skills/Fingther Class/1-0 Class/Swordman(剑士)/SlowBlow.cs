using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Swordman_剑士_
{
    /// <summary>
    ///     致殘攻擊(スロウブロウ)
    /// </summary>
    public class SlowBlow : ISkill
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
            if (level > 1 && (args.flag[0] & AttackFlag.HP_DAMAGE) != 0)
            {
                var rate = 0;
                var lifetime = 0;
                switch (level)
                {
                    case 2:
                        rate = 8;
                        lifetime = 3000;
                        break;
                    case 3:
                        rate = 12;
                        lifetime = 4000;
                        break;
                    case 4:
                        rate = 15;
                        lifetime = 5000;
                        break;
                    case 5:
                        rate = 20;
                        lifetime = 6000;
                        break;
                }

                if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.鈍足, rate))
                {
                    var skill = new MoveSpeedDown(args.skill, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }

        #endregion
    }
}