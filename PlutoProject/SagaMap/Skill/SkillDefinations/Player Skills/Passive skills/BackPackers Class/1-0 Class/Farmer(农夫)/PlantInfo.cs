using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Farmer_农夫_
{
    /// <summary>
    ///     植物知識（植物知識）
    /// </summary>
    public class PlantInfo : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "PlantInfo", MobType.PLANT, MobType.PLANT_BOSS
                , MobType.PLANT_BOSS_SKILL, MobType.PLANT_BOSS_SKILL_NOTPTDROPRANGE
                , MobType.PLANT_MARK, MobType.PLANT_MATERIAL
                , MobType.PLANT_MATERIAL_BOSS_MARK, MobType.PLANT_MATERIAL_EAST
                , MobType.PLANT_MATERIAL_EAST_BOSS_SKILL_WALL, MobType.PLANT_MATERIAL_HETERODOXY
                , MobType.PLANT_MATERIAL_NORTH, MobType.PLANT_MATERIAL_NORTH_BOSS_SKILL_WALL
                , MobType.PLANT_MATERIAL_NOTPTDROPRANGE, MobType.PLANT_MATERIAL_SKILL
                , MobType.PLANT_MATERIAL_SOUTH, MobType.PLANT_MATERIAL_SOUTH_BOSS_SKILL_WALL
                , MobType.PLANT_MATERIAL_WEST, MobType.PLANT_MATERIAL_WEST_BOSS_SKILL_WALL
                , MobType.PLANT_NOTOUCH, MobType.PLANT_NOTPTDROPRANGE
                , MobType.PLANT_SKILL, MobType.PLANT_UNITE);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        //#endregion
    }
}