using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Farmer_农夫_
{
    /// <summary>
    ///     支配（栽培）
    /// </summary>
    public class Cultivation : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //需確認有哪些可以招換
        }

        #endregion
    }
}