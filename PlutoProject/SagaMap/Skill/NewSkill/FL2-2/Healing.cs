using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_2
{
    public class Healing : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Status.Additions.ContainsKey("Spell")) return -7;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var HP_ADD = (uint)(dActor.MaxHP * 0.08f * level);
            SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Holy, -HP_ADD);
        }

        //#endregion
    }
}