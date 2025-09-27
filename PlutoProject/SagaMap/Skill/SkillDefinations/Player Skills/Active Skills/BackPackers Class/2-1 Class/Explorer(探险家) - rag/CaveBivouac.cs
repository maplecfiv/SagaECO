using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     洞穴野營（ケイブビバーク）
    /// </summary>
    public class CaveBivouac : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new HPRecovery(args.skill, sActor, 300000, 5000);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}