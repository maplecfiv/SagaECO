using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.BountyHunter
{
    /// <summary>
    ///     砍擊武器（アームスラッシュ）
    /// </summary>
    public class ArmSlash : Slash, ISkill
    {
        #region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillProc(sActor, dActor, args, level, PossessionPosition.RIGHT_HAND);
        }

        #endregion
    }
}