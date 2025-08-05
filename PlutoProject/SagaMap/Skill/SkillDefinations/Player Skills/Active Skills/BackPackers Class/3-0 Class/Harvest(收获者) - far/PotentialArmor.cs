using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Harvest
{
    public class PotentialArmor : ISkill
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