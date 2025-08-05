using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Event
{
    public class MaxiMum : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultBuff(args.skill, sActor, "デカデカ", 600000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var pc = actor as ActorPC;
            if (pc != null)
            {
                pc.Size = 2000;
                actor.e.OnPlayerSizeChange(actor);
            }
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var pc = actor as ActorPC;
            if (pc != null)
            {
                pc.Size = 1000;
                actor.e.OnPlayerSizeChange(actor);
            }
        }

        #endregion
    }
}