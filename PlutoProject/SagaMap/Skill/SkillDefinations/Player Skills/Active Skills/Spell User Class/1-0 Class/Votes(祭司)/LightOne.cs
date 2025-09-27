using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    public class LightOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 1.2f;
                    break;
                case 2:
                    factor = 1.45f;
                    break;
                case 3:
                    factor = 1.7f;
                    break;
                case 4:
                    factor = 1.95f;
                    break;
                case 5:
                    factor = 2.2f;
                    break;
                case 6:
                    factor = 20f;
                    break;
            }

            if (level == 6)
                SkillHandler.Instance.Seals(sActor, dActor, 5);
            else
                SkillHandler.Instance.Seals(sActor, dActor);
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
        }

        #endregion
    }
}