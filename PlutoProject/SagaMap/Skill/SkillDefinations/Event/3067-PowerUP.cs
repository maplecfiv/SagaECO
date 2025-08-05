using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     力量提升
    /// </summary>
    public class PowerUP : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                var skill = new DefaultBuff(args.skill, dActor, "PowerUP", 60000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var atkadd = (short)skill.skill.Level;
            actor.Status.str_skill += atkadd;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var atkadd = (short)skill.skill.Level;
            actor.Status.str_skill -= atkadd;
        }

        #endregion
    }
}