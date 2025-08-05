using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     注射麻醉針
    /// </summary>
    public class MobParalyzeblow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var act = dActor;
            var rate = 8;
            var lifetime = 3000;
            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stun, rate))
            {
                var skill1 = new Stun(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill1);
            }
        }

        #endregion
    }
}