using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     動物知識（動物知識）
    /// </summary>
    public class AnimalInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "AnimalInfo", MobType.ANIMAL, MobType.ANIMAL_BOMB_SKILL
                , MobType.ANIMAL_BOSS, MobType.ANIMAL_BOSS_SKILL
                , MobType.ANIMAL_BOSS_SKILL_NOTPTDROPRANGE, MobType.ANIMAL_NOTOUCH
                , MobType.ANIMAL_NOTPTDROPRANGE, MobType.ANIMAL_RIDE
                , MobType.ANIMAL_RIDE_BREEDER, MobType.ANIMAL_SKILL
                , MobType.ANIMAL_SPBOSS_SKILL, MobType.ANIMAL_SPBOSS_SKILL_NOTPTDROPRANGE);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}