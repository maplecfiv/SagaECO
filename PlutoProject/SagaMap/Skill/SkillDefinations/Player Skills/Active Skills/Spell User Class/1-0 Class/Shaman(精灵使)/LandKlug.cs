using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    public class LandKlug : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0, 1.15f, 1.35f, 1.55f, 1.75f, 2.0f };
            var factor = factors[level];
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Earth, factor);
            if (level == 6)
            {
                factor = 5f;
                var skill = new Stone(args.skill, dActor, 2500);
                SkillHandler.ApplyAddition(dActor, skill);
                SkillHandler.Instance.ShowEffect((ActorPC)sActor, dActor, 4203);
            }
        }

        #endregion
    }
}