using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     攻擊預測（スキルブレイク）
    /// </summary>
    public class SkillBreak : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, 1f);
        }

        //#endregion
    }
}