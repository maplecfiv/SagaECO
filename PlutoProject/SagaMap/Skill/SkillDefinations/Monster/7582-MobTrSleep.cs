using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     為憑依者的催眠曲
    /// </summary>
    public class MobTrSleep : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 99;
            var lifetime = 30000;
            if (dActor.type == ActorType.PC)
            {
                var pc = (ActorPC)dActor;
                foreach (Actor act in pc.PossesionedActors)
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Sleep, rate))
                    {
                        var skill = new Sleep(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
            }
        }

        #endregion
    }
}