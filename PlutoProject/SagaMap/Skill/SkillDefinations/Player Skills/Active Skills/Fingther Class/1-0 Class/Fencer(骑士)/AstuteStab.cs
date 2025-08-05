using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Fencer_骑士_
{
    /// <summary>
    ///     刺擊防禦術（ディフェンス・スタブ）
    /// </summary>
    public class AstuteStab : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 7500 + 2500 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AstuteStab", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.damage_atk3_discount = 0.2f + 0.05f * skill.skill.Level;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.damage_atk3_discount = 0;
        }

        #endregion
    }
}
/*
  BLOW,
  SLASH,
  STAB,
*/