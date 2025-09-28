using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     打擊防禦術（ディフェンス・ブロウ）
    /// </summary>
    public class AstuteBlow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 7500 + 2500 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AstuteBlow", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.damage_atk1_discount = 0.2f + 0.05f * skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.damage_atk1_discount = 0;
        }

        #endregion
    }
}
/*
  BLOW,
  SLASH,
  STAB,
*/