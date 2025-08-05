using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Trader
{
    /// <summary>
    ///     賞金（チップ）
    /// </summary>
    public class HumCustomary : ISkill
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
                var PastMoney = (uint)(30 * level);
                var pc = (ActorPC)sActor;
                if (pc.Gold >= PastMoney)
                {
                    pc.Gold -= (int)PastMoney;
                    var ai = SkillHandler.Instance.GetMobAI(pet);
                    ai.CastSkill(6401, level, dActor);
                }
            }
        }

        #endregion
    }
}