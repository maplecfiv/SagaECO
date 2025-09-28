using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Warlock_暗术使_
{
    public class BlackWidow : ISkill
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
            factor = 1.0f + 0.2f * level;
            if (dActor.Darks != 1)
            {
                MapManager.Instance.GetMap(sActor.MapID).SendEffect(dActor, 5081);
                dActor.Darks = 1;
            }

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Dark, factor);
        }

        #endregion
    }
}