using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     昆蟲知識（昆虫知識）
    /// </summary>
    public class InsectInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "InsectInfo", MobType.INSECT, MobType.INSECT_BOSS
                , MobType.INSECT_BOSS_NOTPTDROPRANGE, MobType.INSECT_BOSS_SKILL
                , MobType.INSECT_NOTOUCH, MobType.INSECT_NOTPTDROPRANGE
                , MobType.INSECT_RIDE, MobType.INSECT_SKILL
                , MobType.INSECT_UNITE);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}