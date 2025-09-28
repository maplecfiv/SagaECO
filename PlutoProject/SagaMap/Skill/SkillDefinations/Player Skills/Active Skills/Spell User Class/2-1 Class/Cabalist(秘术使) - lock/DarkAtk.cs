using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock
{
    /// <summary>
    ///     黑暗刀刃（グリムリーパー）
    /// </summary>
    public class DarkAtk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.9f + 0.3f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Dark, factor);
        }

        #endregion
    }
}