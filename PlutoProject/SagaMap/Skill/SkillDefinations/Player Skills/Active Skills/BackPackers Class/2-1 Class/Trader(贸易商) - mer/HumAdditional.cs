namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     支付追加費（アディショナルチャージ）
    /// </summary>
    public class HumAdditional : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return;
            if (SkillHandler.Instance.CheckMobType(pet, "HUMAN"))
            {
                uint[] PastMoney = { 0, 50, 80, 50, 120, 100 };
                var pc = (ActorPC)sActor;
                if (pc.Gold >= PastMoney[level])
                {
                    pc.Gold -= (int)PastMoney[level];
                    var ai = SkillHandler.Instance.GetMobAI(pet);
                    ai.CastSkill(6403, level, dActor);
                }
            }
        }

        #endregion
    }
}