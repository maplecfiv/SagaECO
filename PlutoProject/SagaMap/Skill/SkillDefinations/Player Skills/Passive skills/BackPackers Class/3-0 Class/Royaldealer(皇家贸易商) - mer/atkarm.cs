using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._3_0_Class.
    Royaldealer_皇家贸易商____mer
{
    /// <summary>
    ///     ディーラーの手腕
    /// </summary>
    public class AtkArm : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(pc, ItemType.CARD)) return 0;
            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, dActor, "AtkArm", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var MaxAttack = skill.skill.Level == 1 ? 30 : 15;
            var MinAttack = MaxAttack;

            //最大攻擊
            var max_atk1_add = MaxAttack;
            if (skill.Variable.ContainsKey("SwordMaster_max_atk1"))
                skill.Variable.Remove("SwordMaster_max_atk1");
            skill.Variable.Add("SwordMaster_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = MaxAttack;
            if (skill.Variable.ContainsKey("SwordMaster_max_atk2"))
                skill.Variable.Remove("SwordMaster_max_atk2");
            skill.Variable.Add("SwordMaster_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = MaxAttack;
            if (skill.Variable.ContainsKey("SwordMaster_max_atk3"))
                skill.Variable.Remove("SwordMaster_max_atk3");
            skill.Variable.Add("SwordMaster_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = MinAttack;
            if (skill.Variable.ContainsKey("SwordMaster_min_atk1"))
                skill.Variable.Remove("SwordMaster_min_atk1");
            skill.Variable.Add("SwordMaster_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = MinAttack;
            if (skill.Variable.ContainsKey("SwordMaster_min_atk2"))
                skill.Variable.Remove("SwordMaster_min_atk2");
            skill.Variable.Add("SwordMaster_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = MinAttack;
            if (skill.Variable.ContainsKey("SwordMaster_min_atk3"))
                skill.Variable.Remove("SwordMaster_min_atk3");
            skill.Variable.Add("SwordMaster_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //近命中
            var hit_melee_add = skill.skill.Level == 1 ? 16 : 6;
            if (skill.Variable.ContainsKey("SwordMaster_hit_melee"))
                skill.Variable.Remove("SwordMaster_hit_melee");
            skill.Variable.Add("SwordMaster_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;

            //爆擊率
            var cri_add = 3;
            if (skill.Variable.ContainsKey("SwordMaster_cri"))
                skill.Variable.Remove("SwordMaster_cri");
            skill.Variable.Add("SwordMaster_cri", cri_add);
            actor.Status.cri_skill += (short)cri_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["SwordMaster_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["SwordMaster_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["SwordMaster_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["SwordMaster_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["SwordMaster_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["SwordMaster_min_atk3"];

            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["SwordMaster_hit_melee"];

            //爆擊率
            actor.Status.cri_skill -= (short)skill.Variable["SwordMaster_cri"];
        }

        #endregion
    }
}