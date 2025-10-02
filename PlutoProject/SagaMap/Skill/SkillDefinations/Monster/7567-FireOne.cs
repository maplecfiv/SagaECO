using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    public class FireOne : ISkill
    {
        //#region

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.05f;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Fire, factor);
        }

        //#endregion
    }
}