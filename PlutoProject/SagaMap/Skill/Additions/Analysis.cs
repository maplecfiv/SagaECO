using SagaDB.Actor;

namespace SagaMap.Skill.Additions
{
    public class Analysis : DefaultBuff
    {
        public Analysis(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Analysis", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        public Analysis(SagaDB.Skill.Skill skill, Actor actor)
            : base(skill, actor, "Analysis", int.MaxValue)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
        }
    }
}