using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_2_Class.
    Necromancer_死灵使____lock
{
    /// <summary>
    ///     提升召喚對象的物理攻擊（召喚対象物理系上昇）
    /// </summary>
    public class LemuresAtkUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "LemuresAtkUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}