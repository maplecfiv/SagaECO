using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Gambler_赌徒____mer
{
    /// <summary>
    ///     一擲千金（コインシュート）
    /// </summary>
    public class CoinShot : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = new[] { 0, 2.5f, 3.5f, 4.5f, 5.5f, 7.5f }[level];
            var pc = (ActorPC)sActor;
            uint[] gold = { 0, 500, 700, 900, 1100, 1500 };
            if (pc.Gold > gold[level])
            {
                pc.Gold -= (int)gold[level];
                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            }
        }

        //#endregion
    }
}