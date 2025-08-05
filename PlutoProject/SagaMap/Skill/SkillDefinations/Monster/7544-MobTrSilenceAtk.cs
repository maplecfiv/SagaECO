using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     大家安靜點
    /// </summary>
    public class MobTrSilenceAtk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 20;
            var lifetime = 30000;
            if (dActor.type == ActorType.PC)
            {
                var pc = (ActorPC)dActor;
                foreach (Actor act in pc.PossesionedActors)
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Silence,
                            rate))
                    {
                        var skill = new Silence(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
            }
        }

        #endregion
    }
}