using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     速度提升
    /// </summary>
    public class SpeedUP : ISkill
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
                var skill = new DefaultBuff(args.skill, dActor, "SpeedUP", 60000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var atkadd = (int)skill.skill.Level;
            actor.Status.speed_skill += atkadd;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var atkadd = (int)skill.skill.Level;
            actor.Status.speed_skill -= atkadd;
        }

        #endregion
    }
}