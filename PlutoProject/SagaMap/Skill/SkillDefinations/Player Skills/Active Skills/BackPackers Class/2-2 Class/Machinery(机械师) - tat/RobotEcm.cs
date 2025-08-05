using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     妨礙電波（ECM）
    /// </summary>
    public class RobotEcm : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) return 0;
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 20 + 5 * level;
            var lifetime = 3000 + 500 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse,
                            rate))
                    {
                        var skill = new Confuse(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
        }

        #endregion
    }
}