using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class DarkBallad : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.1f;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Dark, factor);
        }
    }
}