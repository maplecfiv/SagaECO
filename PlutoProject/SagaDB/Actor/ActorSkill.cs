namespace SagaDB.Actor
{
    public class ActorSkill : ActorMob
    {
        public ActorSkill(Skill.Skill skill, Actor caster)
        {
            type = ActorType.SKILL;
            Skill = skill;
            Caster = caster;
        }

        public Skill.Skill Skill { get; set; }

        public Actor Caster { get; }

        public bool Stackable { get; set; }
    }
}