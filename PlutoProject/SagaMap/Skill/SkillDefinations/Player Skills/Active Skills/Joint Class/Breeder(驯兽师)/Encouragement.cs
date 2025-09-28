using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Joint_Class.Breeder_驯兽师_
{
    /// <summary>
    ///     激励（激励）
    /// </summary>
    public class Encouragement : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            var skill = new DefaultBuff(args.skill, dActor, "Encouragement", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            var max_atk1_add = 12;
            if (skill.Variable.ContainsKey("Encouragement_max_atk1"))
                skill.Variable.Remove("Encouragement_max_atk1");
            skill.Variable.Add("Encouragement_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = 12;
            if (skill.Variable.ContainsKey("Encouragement_max_atk2"))
                skill.Variable.Remove("Encouragement_max_atk2");
            skill.Variable.Add("Encouragement_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = 12;
            if (skill.Variable.ContainsKey("Encouragement_max_atk3"))
                skill.Variable.Remove("Encouragement_max_atk3");
            skill.Variable.Add("Encouragement_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = 6;
            if (skill.Variable.ContainsKey("Encouragement_min_atk1"))
                skill.Variable.Remove("Encouragement_min_atk1");
            skill.Variable.Add("Encouragement_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = 6;
            if (skill.Variable.ContainsKey("Encouragement_min_atk2"))
                skill.Variable.Remove("Encouragement_min_atk2");
            skill.Variable.Add("Encouragement_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = 6;
            if (skill.Variable.ContainsKey("Encouragement_min_atk3"))
                skill.Variable.Remove("Encouragement_min_atk3");
            skill.Variable.Add("Encouragement_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["Encouragement_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["Encouragement_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["Encouragement_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["Encouragement_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["Encouragement_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["Encouragement_min_atk3"];
        }

        #endregion
    }
}