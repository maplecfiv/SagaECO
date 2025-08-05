using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     攻擊！（ゴー！）
    /// </summary>
    public class PetAttack : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var ai = SkillHandler.Instance.GetMobAI(sActor);
            if (ai == null) return;
            ai.AttackActor(dActor.ActorID);
        }

        #endregion
    }
}