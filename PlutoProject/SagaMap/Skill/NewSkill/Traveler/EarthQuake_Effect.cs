using SagaDB.Actor;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.Traveler
{
    public class EarthQuake_Effect : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }
    }
}