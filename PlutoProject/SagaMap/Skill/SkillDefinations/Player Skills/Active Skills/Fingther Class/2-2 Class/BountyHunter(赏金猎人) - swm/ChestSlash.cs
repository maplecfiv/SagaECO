using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     砍擊裝飾品（オーナメントスラッシュ）
    /// </summary>
    public class ChestSlash : Slash, ISkill
    {
        //#region ISkill Members

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillProc(sActor, dActor, args, level, PossessionPosition.NECK);
        }

        //#endregion
    }
}