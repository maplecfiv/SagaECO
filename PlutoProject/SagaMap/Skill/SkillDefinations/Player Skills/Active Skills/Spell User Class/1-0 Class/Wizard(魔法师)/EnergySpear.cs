using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    public class EnergySpear : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 1.6f;
                    break;
                case 2:
                    factor = 2.0f;
                    break;
                case 3:
                    factor = 2.4f;
                    break;
                case 4:
                    factor = 2.8f;
                    break;
                case 5:
                    factor = 3.2f;
                    break;
                case 6:
                    factor = 10f;
                    break;
            }

            if (level <= 5)
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
            else
                SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor, 50);
        }

        #endregion
    }
}