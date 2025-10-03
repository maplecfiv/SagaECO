using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     支付追加費（アディショナルチャージ）[接續技能]
    /// </summary>
    public class PetHumAdditional : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint[] SkillIDs = { 0, 6407, 6408, 6409, 6410, 6411 };
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(SkillIDs[level], level, 0));
        }

        //#endregion
    }
}