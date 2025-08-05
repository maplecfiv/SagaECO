using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Merchant_商人_
{
    /// <summary>
    ///     高聲放歌（ファシーボイス）
    /// </summary>
    public class Magrow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 70000 - 10000 * level;
            var rate = 10 + 10 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "Magrow", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "Magrow", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            //最小魔攻
            var min_matk_add = (int)(actor.Status.min_matk * (0.1f + 0.1f * level));
            if (skill.Variable.ContainsKey("AtkUpOne_min_matk"))
                skill.Variable.Remove("AtkUpOne_min_matk");
            skill.Variable.Add("AtkUpOne_min_matk", min_matk_add);
            actor.Status.min_matk_skill += (short)min_matk_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //最小魔攻
            actor.Status.min_matk_skill -= (short)skill.Variable["AtkUpOne_min_matk"];
        }

        #endregion
    }
}