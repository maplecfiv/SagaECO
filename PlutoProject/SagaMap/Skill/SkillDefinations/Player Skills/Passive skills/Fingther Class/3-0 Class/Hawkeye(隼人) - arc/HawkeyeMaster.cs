using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    /// <summary>
    ///     シューティングマスタリー
    /// </summary>
    public class HawkeyeMaster : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(pc, ItemType.BOW, ItemType.GUN, ItemType.EXGUN,
                    ItemType.DUALGUN)) return 0;
            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, dActor, "HawkeyeMaster", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var atk_add = 15 + 15 * skill.skill.Level;
            var hit_add = 10 + 6 * skill.skill.Level;
            var cri_add = 3 * skill.skill.Level;

            if (skill.Variable.ContainsKey("HawkeyeMaster_atk"))
                skill.Variable.Remove("HawkeyeMaster_atk");
            skill.Variable.Add("HawkeyeMaster_atk", atk_add);
            actor.Status.max_atk1_skill += (short)atk_add;
            actor.Status.max_atk2_skill += (short)atk_add;
            actor.Status.max_atk3_skill += (short)atk_add;
            actor.Status.min_atk1_skill += (short)atk_add;
            actor.Status.min_atk2_skill += (short)atk_add;
            actor.Status.min_atk3_skill += (short)atk_add;

            if (skill.Variable.ContainsKey("HawkeyeMaster_hit"))
                skill.Variable.Remove("HawkeyeMaster_hit");
            skill.Variable.Add("HawkeyeMaster_hit", hit_add);
            actor.Status.hit_melee_skill += (short)hit_add;

            if (skill.Variable.ContainsKey("HawkeyeMaster_cri"))
                skill.Variable.Remove("HawkeyeMaster_cri");
            skill.Variable.Add("HawkeyeMaster_cri", cri_add);
            actor.Status.cri_skill += (short)cri_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.max_atk1_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.max_atk2_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.max_atk3_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.min_atk1_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.min_atk2_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.min_atk3_skill -= (short)skill.Variable["HawkeyeMaster_atk"];
            actor.Status.hit_melee_skill -= (short)skill.Variable["HawkeyeMaster_hit"];
            actor.Status.cri_skill -= (short)skill.Variable["HawkeyeMaster_cri"];
        }

        #endregion
    }
}