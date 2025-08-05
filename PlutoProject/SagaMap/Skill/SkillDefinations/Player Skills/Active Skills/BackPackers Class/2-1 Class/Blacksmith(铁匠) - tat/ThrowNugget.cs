using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     投擲礦物（ナゲット投げ）
    /// </summary>
    public class ThrowNugget : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
            {
                if (sActor.type == ActorType.PC)
                {
                    var pc = sActor;
                    uint[] itemIDs = { 0, 10015708, 10015700, 10015600, 10015800, 10015707 };
                    var itemID = itemIDs[args.skill.Level];
                    if (SkillHandler.Instance.CountItem(pc, itemID) > 0) return 0;
                    return -2;
                }

                return 0;
            }

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.0f + 1.0f * level;
            //用等級判斷取走的道具
            uint[] itemIDs = { 0, 10015708, 10015700, 10015600, 10015800, 10015707 };
            var itemID = itemIDs[level];
            var pc = (ActorPC)sActor;
            if (SkillHandler.Instance.CountItem(pc, itemID) > 0)
            {
                SkillHandler.Instance.TakeItem(pc, itemID, 1);
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
        }

        #endregion
    }
}