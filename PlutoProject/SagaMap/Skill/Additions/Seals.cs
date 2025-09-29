using SagaDB.Actor;

namespace SagaMap.Skill.Additions
{
    public class Seals : DefaultBuff
    {
        public Seals(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
            : base(skill, actor, "Seals", lifetime)
        {
            OnAdditionStart += StartEvent;
            OnAdditionEnd += EndEvent;
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            if (actor.Seals < 5)
                actor.Seals += 1;
            actor.IsSeals = 0;
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            if (actor.IsSeals == 0)
                actor.Seals = 0;
        }
    }
}