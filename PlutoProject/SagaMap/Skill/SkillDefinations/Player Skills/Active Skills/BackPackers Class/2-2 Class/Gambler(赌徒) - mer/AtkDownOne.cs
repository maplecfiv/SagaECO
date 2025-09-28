using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     女子是撒嬌/男子是魄力（女は愛嬌、男は度胸）
    /// </summary>
    public class AtkDownOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 70000 - 10000 * level;
            var rate = 20 + 10 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill = new DefaultBuff(args.skill, dActor, "AtkDownOne", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //最大攻擊
            var max_atk1_add = -(int)(actor.Status.max_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_max_atk1"))
                skill.Variable.Remove("AtkDownOne_max_atk1");
            skill.Variable.Add("AtkDownOne_max_atk1", max_atk1_add);
            actor.Status.max_atk1_skill += (short)max_atk1_add;

            //最大攻擊
            var max_atk2_add = -(int)(actor.Status.max_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_max_atk2"))
                skill.Variable.Remove("AtkDownOne_max_atk2");
            skill.Variable.Add("AtkDownOne_max_atk2", max_atk2_add);
            actor.Status.max_atk2_skill += (short)max_atk2_add;

            //最大攻擊
            var max_atk3_add = -(int)(actor.Status.max_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_max_atk3"))
                skill.Variable.Remove("AtkDownOne_max_atk3");
            skill.Variable.Add("AtkDownOne_max_atk3", max_atk3_add);
            actor.Status.max_atk3_skill += (short)max_atk3_add;

            //最小攻擊
            var min_atk1_add = -(int)(actor.Status.min_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_min_atk1"))
                skill.Variable.Remove("AtkDownOne_min_atk1");
            skill.Variable.Add("AtkDownOne_min_atk1", min_atk1_add);
            actor.Status.min_atk1_skill += (short)min_atk1_add;

            //最小攻擊
            var min_atk2_add = -(int)(actor.Status.min_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_min_atk2"))
                skill.Variable.Remove("AtkDownOne_min_atk2");
            skill.Variable.Add("AtkDownOne_min_atk2", min_atk2_add);
            actor.Status.min_atk2_skill += (short)min_atk2_add;

            //最小攻擊
            var min_atk3_add = -(int)(actor.Status.min_atk_bs * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkDownOne_min_atk3"))
                skill.Variable.Remove("AtkDownOne_min_atk3");
            skill.Variable.Add("AtkDownOne_min_atk3", min_atk3_add);
            actor.Status.min_atk3_skill += (short)min_atk3_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最大攻擊
            actor.Status.max_atk1_skill -= (short)skill.Variable["AtkDownOne_max_atk1"];

            //最大攻擊
            actor.Status.max_atk2_skill -= (short)skill.Variable["AtkDownOne_max_atk2"];

            //最大攻擊
            actor.Status.max_atk3_skill -= (short)skill.Variable["AtkDownOne_max_atk3"];

            //最小攻擊
            actor.Status.min_atk1_skill -= (short)skill.Variable["AtkDownOne_min_atk1"];

            //最小攻擊
            actor.Status.min_atk2_skill -= (short)skill.Variable["AtkDownOne_min_atk2"];

            //最小攻擊
            actor.Status.min_atk3_skill -= (short)skill.Variable["AtkDownOne_min_atk3"];
        }

        #endregion
    }
}