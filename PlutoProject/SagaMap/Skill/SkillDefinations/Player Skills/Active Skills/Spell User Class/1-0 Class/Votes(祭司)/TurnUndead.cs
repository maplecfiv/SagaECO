using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    /// <summary>
    ///     聖光審判術（ターンアンデッド）
    /// </summary>
    public class TurnUndead : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factors = new[] { 0.0f, 2.4f, 2.9f, 3.5f, 4.2f, 5.0f };
            var factor = factors[level] - sActor.Status.Cardinal_Rank;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
        }

        #endregion
    }
}