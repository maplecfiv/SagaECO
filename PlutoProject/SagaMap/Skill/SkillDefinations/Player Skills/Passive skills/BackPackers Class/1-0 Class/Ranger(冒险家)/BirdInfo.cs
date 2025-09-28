using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     鳥類知識（鳥知識）
    /// </summary>
    public class BirdInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "BirdInfo", MobType.BIRD, MobType.BIRD_BOSS
                , MobType.BIRD_BOSS_SKILL, MobType.BIRD_NOTOUCH
                , MobType.BIRD_SPBOSS_SKILL, MobType.BIRD_UNITE);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}