using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     冰晶激光（チルレーザー）
    /// </summary>
    public class RobotChillLaser : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) return 0;
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.95f + 0.2f * level;
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, Elements.Water, factor);
            var rate = 20 + 5 * level;
            var lifetime = 20000 + 5000 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Frosen, rate))
            {
                var skill = new Freeze(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        //#endregion
    }
}