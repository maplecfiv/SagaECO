using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     礦石分析（鉱石分析）
    /// </summary>
    public class RockAnalysis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var types = new List<MobType>();
                types.Add(MobType.ROCK);
                types.Add(MobType.ROCK_BOMB_SKILL);
                types.Add(MobType.ROCK_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.ROCK_BOSS_SKILL_WALL);
                types.Add(MobType.ROCK_MATERIAL);
                types.Add(MobType.ROCK_MATERIAL_BOSS_NOTPTDROPRANGE);
                types.Add(MobType.ROCK_MATERIAL_BOSS_SKILL_NOTPTDROPRANGE);
                types.Add(MobType.ROCK_MATERIAL_EAST_NOTOUCH);
                types.Add(MobType.ROCK_MATERIAL_NORTH_NOTOUCH);
                types.Add(MobType.ROCK_MATERIAL_SKILL);
                types.Add(MobType.ROCK_MATERIAL_SOUTH_NOTOUCH);
                types.Add(MobType.ROCK_MATERIAL_WEST_NOTOUCH);
                types.Add(MobType.ROCK_NOTPTDROPRANGE);
                types.Add(MobType.ROCK_SKILL);

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