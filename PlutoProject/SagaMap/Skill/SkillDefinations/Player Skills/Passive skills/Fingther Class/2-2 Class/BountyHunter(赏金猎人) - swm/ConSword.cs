using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.BountyHunter
{
    /// <summary>
    ///     2段砍擊（二段斬り）
    /// </summary>
    public class ConSword : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.SWORD, ItemType.SHORT_SWORD,
                        ItemType.RAPIER)) active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "ConSword", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill += (short)(2 * level);
            actor.Status.combo_skill = 2;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            actor.Status.combo_rate_skill -= (short)(2 * level);
            actor.Status.combo_skill = 1;
        }

        #endregion
    }
}