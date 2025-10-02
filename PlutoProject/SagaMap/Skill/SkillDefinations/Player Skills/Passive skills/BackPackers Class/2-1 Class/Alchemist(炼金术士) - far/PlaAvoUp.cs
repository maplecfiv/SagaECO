using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     植物迴避
    /// </summary>
    public class PlaAvoUp : ISkill
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

            var skill = new SomeTypeAvoUp(args.skill, dActor, "PlaAvoUp");
            skill.AddMobType(MobType.PLANT, value);
            skill.AddMobType(MobType.PLANT_BOSS, value);
            skill.AddMobType(MobType.PLANT_BOSS_SKILL, value);
            skill.AddMobType(MobType.PLANT_BOSS_SKILL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.PLANT_MARK, value);
            skill.AddMobType(MobType.PLANT_MATERIAL, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_BOSS_MARK, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_EAST, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_EAST_BOSS_SKILL_WALL, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_HETERODOXY, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_NORTH, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_NORTH_BOSS_SKILL_WALL, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_SKILL, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_SOUTH, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_SOUTH_BOSS_SKILL_WALL, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_WEST, value);
            skill.AddMobType(MobType.PLANT_MATERIAL_WEST_BOSS_SKILL_WALL, value);
            skill.AddMobType(MobType.PLANT_NOTOUCH, value);
            skill.AddMobType(MobType.PLANT_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.PLANT_SKILL, value);
            skill.AddMobType(MobType.PLANT_UNITE, value);
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