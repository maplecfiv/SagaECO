using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Machinery_机械师____tat
{
    /// <summary>
    ///     機械知識（機械知識）
    /// </summary>
    public class MachineInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "MachineInfo", MobType.MACHINE, MobType.MACHINE_BOSS
                , MobType.MACHINE_BOSS_CHAMP, MobType.MACHINE_BOSS_SKILL
                , MobType.MACHINE_MATERIAL, MobType.MACHINE_NOTOUCH
                , MobType.MACHINE_NOTPTDROPRANGE, MobType.MACHINE_RIDE
                , MobType.MACHINE_RIDE_ROBOT, MobType.MACHINE_SKILL
                , MobType.MACHINE_SKILL_BOSS, MobType.MACHINE_SMARK_BOSS_SKILL_HETERODOXY_NONBLAST);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}