using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Maestro
{
    public class GreatMaster : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet != null)
                if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                {
                    //创建一个默认被动技能处理对象
                    var skill = new DefaultPassiveSkill(args.skill, sActor, "GreatMaster", true);
                    //设置OnAdditionStart事件处理过程
                    skill.OnAdditionStart += StartEventHandler;
                    //设置OnAdditionEnd事件处理过程
                    skill.OnAdditionEnd += EndEventHandler;
                    //对指定Actor附加技能效果
                    SkillHandler.ApplyAddition(sActor, skill);
                }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //actor.MaxHP += (uint)(actor.MaxHP * 1f+0.05f*skill.skill.Level);

            //actor.Status.max_atk1_skill += (short)(actor.Status.max_atk1 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.max_atk2_skill += (short)(actor.Status.max_atk2 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.max_atk3_skill += (short)(actor.Status.max_atk3 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk1_skill += (short)(actor.Status.min_atk1 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk2_skill += (short)(actor.Status.min_atk2 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk3_skill += (short)(actor.Status.min_atk3 * 1f + 0.05f * skill.skill.Level);

            //actor.Status.max_matk_skill += (short)(actor.Status.max_matk * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_matk_skill += (short)(actor.Status.min_matk * 1f + 0.05f * skill.skill.Level);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //actor.MaxHP -= (uint)(actor.MaxHP * 1f + 0.05f * skill.skill.Level);

            //actor.Status.max_atk1_skill -= (short)(actor.Status.max_atk1 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.max_atk2_skill -= (short)(actor.Status.max_atk2 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.max_atk3_skill -= (short)(actor.Status.max_atk3 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk1_skill -= (short)(actor.Status.min_atk1 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk2_skill -= (short)(actor.Status.min_atk2 * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_atk3_skill -= (short)(actor.Status.min_atk3 * 1f + 0.05f * skill.skill.Level);

            //actor.Status.max_matk_skill -= (short)(actor.Status.max_matk * 1f + 0.05f * skill.skill.Level);
            //actor.Status.min_matk_skill -= (short)(actor.Status.min_matk * 1f + 0.05f * skill.skill.Level);
        }

        #endregion
    }
}