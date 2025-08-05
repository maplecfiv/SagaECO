using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Gladiator
{
    public class KenSei : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(pc, ItemType.AXE, ItemType.SWORD)) return 0;
            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, dActor, "KenSei", true);
            skill.OnAdditionStart += skill_OnAdditionStart;
            skill.OnAdditionEnd += skill_OnAdditionEnd;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void skill_OnAdditionEnd(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void skill_OnAdditionStart(Actor actor, DefaultPassiveSkill skill)
        {
        }
    }
}