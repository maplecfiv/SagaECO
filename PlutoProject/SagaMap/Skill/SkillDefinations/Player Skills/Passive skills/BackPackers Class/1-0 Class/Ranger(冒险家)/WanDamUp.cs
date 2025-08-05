using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Ranger
{
    /// <summary>
    ///     水中傷害增加
    /// </summary>
    public class WanDamUp : ISkill
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

            var skill = new SomeTypeDamUp(args.skill, dActor, "WanDamUp");
            skill.AddMobType(MobType.WATER_ANIMAL, value);
            skill.AddMobType(MobType.WATER_ANIMAL_BOSS, value);
            skill.AddMobType(MobType.WATER_ANIMAL_BOSS_SKILL, value);
            skill.AddMobType(MobType.WATER_ANIMAL_LVDIFF, value);
            skill.AddMobType(MobType.WATER_ANIMAL_NOTOUCH, value);
            skill.AddMobType(MobType.WATER_ANIMAL_RIDE, value);
            skill.AddMobType(MobType.WATER_ANIMAL_SKILL, value);
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