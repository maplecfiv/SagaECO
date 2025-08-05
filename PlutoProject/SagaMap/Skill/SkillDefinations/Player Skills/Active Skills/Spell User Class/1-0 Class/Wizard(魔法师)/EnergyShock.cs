using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    public class EnergyShock : ISkill
    {
        private readonly bool MobUse;

        public EnergyShock()
        {
            MobUse = false;
        }

        public EnergyShock(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 1.1f;
                    break;
                case 2:
                    factor = 1.3f;
                    break;
                case 3:
                    factor = 1.5f;
                    break;
                case 4:
                    factor = 1.7f;
                    break;
                case 5:
                    factor = 1.9f;
                    break;
            }

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Neutral, factor);
        }

        #endregion
    }
}