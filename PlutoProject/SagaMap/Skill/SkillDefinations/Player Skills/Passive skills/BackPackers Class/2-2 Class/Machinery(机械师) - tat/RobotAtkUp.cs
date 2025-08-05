using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     提升機器人的攻擊力（ロボット攻撃力上昇）
    /// </summary>
    public class RobotAtkUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet != null)
            {
                if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "RobotAtkUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            //最大攻擊
            var max_atk1_add = (int)(actor.Status.max_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_max_atk1"))
                skill.Variable.Remove("RobotAtkUp_max_atk1");
            skill.Variable.Add("RobotAtkUp_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = (int)(actor.Status.max_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_max_atk2"))
                skill.Variable.Remove("RobotAtkUp_max_atk2");
            skill.Variable.Add("RobotAtkUp_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = (int)(actor.Status.max_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_max_atk3"))
                skill.Variable.Remove("RobotAtkUp_max_atk3");
            skill.Variable.Add("RobotAtkUp_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = (int)(actor.Status.min_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_min_atk1"))
                skill.Variable.Remove("RobotAtkUp_min_atk1");
            skill.Variable.Add("RobotAtkUp_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = (int)(actor.Status.min_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_min_atk2"))
                skill.Variable.Remove("RobotAtkUp_min_atk2");
            skill.Variable.Add("RobotAtkUp_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = (int)(actor.Status.min_atk_bs * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_min_atk3"))
                skill.Variable.Remove("RobotAtkUp_min_atk3");
            skill.Variable.Add("RobotAtkUp_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //最大魔攻
            var max_matk_add = (int)(actor.Status.max_matk * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_max_matk"))
                skill.Variable.Remove("RobotAtkUp_max_matk");
            skill.Variable.Add("RobotAtkUp_max_matk", max_matk_add);
            actor.Status.max_matk_skill += (short)max_matk_add;

            //最小魔攻
            var min_matk_add = (int)(actor.Status.min_matk * (0.08f + 0.02f * level));
            if (skill.Variable.ContainsKey("RobotAtkUp_min_matk"))
                skill.Variable.Remove("RobotAtkUp_min_matk");
            skill.Variable.Add("RobotAtkUp_min_matk", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["RobotAtkUp_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["RobotAtkUp_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["RobotAtkUp_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["RobotAtkUp_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["RobotAtkUp_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["RobotAtkUp_min_atk3"];

            //最大魔攻
            actor.Status.max_matk_skill -= (short)skill.Variable["RobotAtkUp_max_matk"];

            //最小魔攻
            actor.Status.min_matk_skill -= (short)skill.Variable["RobotAtkUp_min_matk"];
        }

        #endregion
    }
}