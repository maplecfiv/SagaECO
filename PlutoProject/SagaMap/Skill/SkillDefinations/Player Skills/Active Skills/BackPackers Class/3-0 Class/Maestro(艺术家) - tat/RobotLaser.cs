using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Maestro_艺术家____tat
{
    /// <summary>
    ///     レールガン
    /// </summary>
    public class RobotLaser : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (!sActor.Status.Additions.ContainsKey("RobotLaser"))
            {
                if (pet == null) return -54; //需回傳"需裝備寵物"
                if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) return 0;
                return -54; //需回傳"需裝備寵物"
            }

            return -30;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 8f * level;
            var lifetime = 35000 - 5000 * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var skill = new DefaultBuff(args.skill, sActor, "RobotLaser", lifetime);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}