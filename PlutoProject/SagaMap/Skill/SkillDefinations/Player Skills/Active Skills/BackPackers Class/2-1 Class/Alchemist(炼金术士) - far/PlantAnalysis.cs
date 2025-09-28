using System.Collections.Generic;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     植物分析（植物分析）
    /// </summary>
    public class PlantAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.PLANT);
                types.Add(MobType.PLANT_BOSS);
                types.Add(MobType.PLANT_BOSS_SKILL);
                types.Add(MobType.PLANT_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.PLANT_MARK);
                types.Add(MobType.PLANT_MATERIAL);
                types.Add(MobType.PLANT_MATERIAL_BOSS_MARK);
                types.Add(MobType.PLANT_MATERIAL_EAST);
                types.Add(MobType.PLANT_MATERIAL_EAST_BOSS_SKILL_WALL);
                types.Add(MobType.PLANT_MATERIAL_HETERODOXY);
                types.Add(MobType.PLANT_MATERIAL_NORTH);
                types.Add(MobType.PLANT_MATERIAL_NORTH_BOSS_SKILL_WALL);
                types.Add(MobType.PLANT_MATERIAL_NOTPTDROPRANGE);
                types.Add(MobType.PLANT_MATERIAL_SKILL);
                types.Add(MobType.PLANT_MATERIAL_SOUTH);
                types.Add(MobType.PLANT_MATERIAL_SOUTH_BOSS_SKILL_WALL);
                types.Add(MobType.PLANT_MATERIAL_WEST);
                types.Add(MobType.PLANT_MATERIAL_WEST_BOSS_SKILL_WALL);
                types.Add(MobType.PLANT_NOTOUCH);
                types.Add(MobType.PLANT_NOTPTDROPRANGE);
                types.Add(MobType.PLANT_SKILL);
                types.Add(MobType.PLANT_UNITE);

                var mob = (ActorMob)dActor;
                if (types.Contains(mob.BaseData.mobType)) return 0;
            }

            return -4;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Analysis(args.skill, dActor);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}