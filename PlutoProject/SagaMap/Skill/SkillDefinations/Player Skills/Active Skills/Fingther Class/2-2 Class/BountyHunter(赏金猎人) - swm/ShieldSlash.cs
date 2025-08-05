using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.BountyHunter
{
    /// <summary>
    ///     砍擊盾（シールドスラッシュ）
    /// </summary>
    public class ShieldSlash : Slash, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillProc(sActor, dActor, args, level, PossessionPosition.LEFT_HAND);
        }

        #endregion
    }
}