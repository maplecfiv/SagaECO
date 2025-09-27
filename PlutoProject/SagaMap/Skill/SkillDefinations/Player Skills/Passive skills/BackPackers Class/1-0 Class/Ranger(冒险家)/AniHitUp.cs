using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     動物命中
    /// </summary>
    public class AniHitUp : ISkill
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

            var skill = new SomeTypeHitUp(args.skill, dActor, "AniHitUp");
            skill.AddMobType(MobType.ANIMAL, value);
            skill.AddMobType(MobType.ANIMAL_BOMB_SKILL, value);
            skill.AddMobType(MobType.ANIMAL_BOSS, value);
            skill.AddMobType(MobType.ANIMAL_BOSS_SKILL, value);
            skill.AddMobType(MobType.ANIMAL_BOSS_SKILL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.ANIMAL_NOTOUCH, value);
            skill.AddMobType(MobType.ANIMAL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.ANIMAL_RIDE, value);
            skill.AddMobType(MobType.ANIMAL_RIDE_BREEDER, value);
            skill.AddMobType(MobType.ANIMAL_SKILL, value);
            skill.AddMobType(MobType.ANIMAL_SPBOSS_SKILL, value);
            skill.AddMobType(MobType.ANIMAL_SPBOSS_SKILL_NOTPTDROPRANGE, value);
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