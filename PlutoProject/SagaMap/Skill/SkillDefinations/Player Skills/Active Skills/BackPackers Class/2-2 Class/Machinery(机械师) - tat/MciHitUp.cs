using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     機械命中
    /// </summary>
    public class MciHitUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] Values = { 0, 3, 6, 9, 12, 15 }; //%

            var value = Values[level];

            var skill = new SomeTypeHitUp(args.skill, dActor, "MciHitUp");
            skill.AddMobType(MobType.MACHINE, value);
            skill.AddMobType(MobType.MACHINE_BOSS, value);
            skill.AddMobType(MobType.MACHINE_BOSS_CHAMP, value);
            skill.AddMobType(MobType.MACHINE_BOSS_SKILL, value);
            skill.AddMobType(MobType.MACHINE_MATERIAL, value);
            skill.AddMobType(MobType.MACHINE_NOTOUCH, value);
            skill.AddMobType(MobType.MACHINE_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.MACHINE_RIDE, value);
            skill.AddMobType(MobType.MACHINE_RIDE_ROBOT, value);
            skill.AddMobType(MobType.MACHINE_SKILL, value);
            skill.AddMobType(MobType.MACHINE_SKILL_BOSS, value);
            skill.AddMobType(MobType.MACHINE_SMARK_BOSS_SKILL_HETERODOXY_NONBLAST, value);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}