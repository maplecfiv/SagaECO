using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     沖擊之箭
    /// </summary>
    public class MobChargeArrow : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f;
            var lifetime = 1500;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Neutral, factor);
            SkillHandler.Instance.PushBack(sActor, dActor, 3);
            var skill = new Stiff(args.skill, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        //#endregion
    }
}