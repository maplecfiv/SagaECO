using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     烈炎射擊（チャージショット）
    /// </summary>
    public class ChargeShot : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return SkillHandler.Instance.CheckPcGunAndBullet(sActor);
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.PcBulletDown(sActor);
            var factor = 1.1f + 0.1f * level;
            args.argType = SkillArg.ArgType.Attack;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            SkillHandler.Instance.PushBack(sActor, dActor, 3);
            var skill1 = new Stiff(args.skill, dActor, 3000);
            SkillHandler.ApplyAddition(dActor, skill1);
        }

        #endregion
    }
}