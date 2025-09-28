using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     流金岁月
    /// </summary>
    public class MoneyTime : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Gold >= 2000) return 0;

            return -2;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                pc.Gold -= 2000;
                var skill = new DefaultBuff(args.skill, dActor, "MoneyTime", 60000);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.speed_skill += 120;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.speed_skill -= 120;
        }

        #endregion
    }
}