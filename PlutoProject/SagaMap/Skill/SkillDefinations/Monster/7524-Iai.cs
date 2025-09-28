using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     魔物用居合
    /// </summary>
    public class Iai : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.type = ATTACK_TYPE.SLASH;
            var factor = 2.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
        }
    }
}