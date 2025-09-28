using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Tatarabe_矿工_
{
    /// <summary>
    ///     鐵礦知識（鉱石知識）
    /// </summary>
    public class RockInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "RockInfo", MobType.ROCK, MobType.ROCK_BOMB_SKILL
                , MobType.ROCK_BOSS_SKILL_NOTPTDROPRANGE, MobType.ROCK_BOSS_SKILL_WALL
                , MobType.ROCK_MATERIAL, MobType.ROCK_MATERIAL_BOSS_NOTPTDROPRANGE
                , MobType.ROCK_MATERIAL_BOSS_SKILL_NOTPTDROPRANGE, MobType.ROCK_MATERIAL_EAST_NOTOUCH
                , MobType.ROCK_MATERIAL_NORTH_NOTOUCH, MobType.ROCK_MATERIAL_SKILL
                , MobType.ROCK_MATERIAL_SOUTH_NOTOUCH, MobType.ROCK_MATERIAL_WEST_NOTOUCH
                , MobType.ROCK_NOTPTDROPRANGE, MobType.ROCK_SKILL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}