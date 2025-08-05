using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     手提包修練（バッグマスタリー）
    /// </summary>
    public class BagDamUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.LEFT_HANDBAG, ItemType.HANDBAG)) active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "BagDamUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int level = skill.skill.Level;
            int[] ATK = { 0, 5, 10, 15, 20, 30 };
            int[] HIT = { 0, 2, 2, 2, 2, 3 };
            //最大攻擊
            var max_atk1_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_max_atk1"))
                skill.Variable.Remove("BagDamUp_max_atk1");
            skill.Variable.Add("BagDamUp_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_max_atk2"))
                skill.Variable.Remove("BagDamUp_max_atk2");
            skill.Variable.Add("BagDamUp_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_max_atk3"))
                skill.Variable.Remove("BagDamUp_max_atk3");
            skill.Variable.Add("BagDamUp_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_min_atk1"))
                skill.Variable.Remove("BagDamUp_min_atk1");
            skill.Variable.Add("BagDamUp_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_min_atk2"))
                skill.Variable.Remove("BagDamUp_min_atk2");
            skill.Variable.Add("BagDamUp_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = ATK[level];
            if (skill.Variable.ContainsKey("BagDamUp_min_atk3"))
                skill.Variable.Remove("BagDamUp_min_atk3");
            skill.Variable.Add("BagDamUp_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //近命中
            var hit_melee_add = HIT[level];
            if (skill.Variable.ContainsKey("BagDamUp_hit_melee"))
                skill.Variable.Remove("BagDamUp_hit_melee");
            skill.Variable.Add("BagDamUp_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["BagDamUp_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["BagDamUp_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["BagDamUp_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["BagDamUp_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["BagDamUp_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["BagDamUp_min_atk3"];

            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["BagDamUp_hit_melee"];
        }

        #endregion
    }
}