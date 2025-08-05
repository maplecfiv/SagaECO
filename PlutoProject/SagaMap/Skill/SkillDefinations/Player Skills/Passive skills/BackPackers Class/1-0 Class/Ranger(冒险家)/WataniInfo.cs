using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Ranger
{
    /// <summary>
    ///     水中生物知識（水中生物知識）
    /// </summary>
    public class WataniInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "WataniInfo", MobType.WATER_ANIMAL, MobType.WATER_ANIMAL_BOSS
                , MobType.WATER_ANIMAL_BOSS_SKILL, MobType.WATER_ANIMAL_LVDIFF
                , MobType.WATER_ANIMAL_NOTOUCH, MobType.WATER_ANIMAL_RIDE
                , MobType.WATER_ANIMAL_SKILL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}