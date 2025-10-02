using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     水中命中
    /// </summary>
    public class WanHitUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] Values = { 0, 3, 6, 9, 12, 15 }; //%

            var value = Values[level];

            var skill = new SomeTypeHitUp(args.skill, dActor, "WanHitUp");
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

        //#endregion
    }
}