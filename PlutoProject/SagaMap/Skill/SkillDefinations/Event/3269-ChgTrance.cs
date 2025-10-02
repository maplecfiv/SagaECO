using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     蝙蝠變身
    /// </summary>
    public class ChgTrance : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "ChgTrance", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //變身成蝙蝠
            // SkillHandler.Instance.TranceMob()
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            SkillHandler.Instance.TranceMob(actor, 0);
        }

        //#endregion
    }
}