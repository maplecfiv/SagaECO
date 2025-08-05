using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     提升迴避率
    /// </summary>
    public class MobAvoupSelf : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 90000;
            var skill = new DefaultBuff(args.skill, dActor, "MobAvoupSelf", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //近戰迴避
            actor.Status.avoid_melee_skill += 20;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近戰迴避
            actor.Status.avoid_melee_skill -= 20;
        }

        #endregion
    }
}