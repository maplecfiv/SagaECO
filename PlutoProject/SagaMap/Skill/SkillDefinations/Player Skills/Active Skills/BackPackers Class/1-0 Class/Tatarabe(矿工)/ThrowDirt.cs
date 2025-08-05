using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Tatarabe
{
    /// <summary>
    ///     投擲泥土（スローダート）
    /// </summary>
    public class ThrowDirt : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 20 + 5 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "ThrowDirt", rate))
            {
                var lifetime = 35000 - 5000 * level;
                var skill = new DefaultBuff(args.skill, dActor, "ThrowDirt", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //近命中
            var hit_melee_add = 15 + 5 * level;
            if (skill.Variable.ContainsKey("ThrowDirt_hit_melee"))
                skill.Variable.Remove("ThrowDirt_hit_melee");
            skill.Variable.Add("ThrowDirt_hit_melee", hit_melee_add);
            actor.Status.hit_melee_skill += (short)hit_melee_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //近命中
            actor.Status.hit_melee_skill -= (short)skill.Variable["ThrowDirt_hit_melee"];
        }

        #endregion
    }
}