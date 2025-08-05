using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.BountyHunter
{
    /// <summary>
    ///     砍擊盔甲（ストレートスラッシュ）
    /// </summary>
    public class BodySlash : Slash, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillProc(sActor, dActor, args, level, PossessionPosition.CHEST);
        }

        #endregion
    }
}