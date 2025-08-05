using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.FL2_1
{
    public class FireSlash : ISkill
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

            var args2 = args.Clone();
            args.type = ATTACK_TYPE.SLASH;

            factor = 1.0f + 0.5f * level;

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);


            SkillHandler.Instance.MagicAttack(sActor, dActor, args2, Elements.Fire, factor);
            args.AddSameActor(args2);
        }

        #endregion
    }
}