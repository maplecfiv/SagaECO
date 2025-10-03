using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     猪鹿蝶（猪鹿蝶）
    /// </summary>
    public class FlowerCardSEQ2 : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.0f + 0.5f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}