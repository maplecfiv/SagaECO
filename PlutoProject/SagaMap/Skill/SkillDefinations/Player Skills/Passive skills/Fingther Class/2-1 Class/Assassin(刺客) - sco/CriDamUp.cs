using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     增強致命攻擊（クリティカルダメージ上昇）
    /// </summary>
    public class CriDamUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "CriDamUp", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = 10 + 2 * skill.skill.Level;
            if (skill.Variable.ContainsKey("CriDamUp"))
                skill.Variable.Remove("CriDamUp");
            skill.Variable.Add("CriDamUp", value);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (skill.Variable.ContainsKey("CriDamUp"))
                skill.Variable.Remove("CriDamUp");
        }

        #endregion
    }
}