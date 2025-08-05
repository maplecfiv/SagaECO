using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Merchant
{
    /// <summary>
    ///     應援（応援）
    /// </summary>
    public class AtkUpOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 80000 - 2000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AtkUpOne", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            int[] MaxAtk = { 0, 2, 4, 6, 8, 10 };
            int[] MinAtk = { 0, 5, 8, 12, 17, 23 };
            int[] MaxMAtk = { 0, 1, 2, 3, 4, 5 };
            int[] MinMAtk = { 0, 1, 2, 3, 4, 5 };
            //最大攻擊
            var max_atk1_add = MaxAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_max_atk1"))
                skill.Variable.Remove("AtkUpOne_max_atk1");
            skill.Variable.Add("AtkUpOne_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = MaxAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_max_atk2"))
                skill.Variable.Remove("AtkUpOne_max_atk2");
            skill.Variable.Add("AtkUpOne_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = MaxAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_max_atk3"))
                skill.Variable.Remove("AtkUpOne_max_atk3");
            skill.Variable.Add("AtkUpOne_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = MinAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_min_atk1"))
                skill.Variable.Remove("AtkUpOne_min_atk1");
            skill.Variable.Add("AtkUpOne_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = MinAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_min_atk2"))
                skill.Variable.Remove("AtkUpOne_min_atk2");
            skill.Variable.Add("AtkUpOne_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = MinAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_min_atk3"))
                skill.Variable.Remove("AtkUpOne_min_atk3");
            skill.Variable.Add("AtkUpOne_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;

            //最大魔攻
            var max_matk_add = MinMAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_max_matk"))
                skill.Variable.Remove("AtkUpOne_max_matk");
            skill.Variable.Add("AtkUpOne_max_matk", max_matk_add);
            actor.Status.max_matk_skill += (short)max_matk_add;

            //最小魔攻
            var min_matk_add = MinMAtk[level];
            if (skill.Variable.ContainsKey("AtkUpOne_min_matk"))
                skill.Variable.Remove("AtkUpOne_min_matk");
            skill.Variable.Add("AtkUpOne_min_matk", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["AtkUpOne_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["AtkUpOne_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["AtkUpOne_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["AtkUpOne_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["AtkUpOne_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["AtkUpOne_min_atk3"];

            //最大魔攻
            actor.Status.max_matk_skill -= (short)skill.Variable["AtkUpOne_max_matk"];

            //最小魔攻
            actor.Status.min_matk_skill -= (short)skill.Variable["AtkUpOne_min_matk"];
        }

        #endregion
    }
}