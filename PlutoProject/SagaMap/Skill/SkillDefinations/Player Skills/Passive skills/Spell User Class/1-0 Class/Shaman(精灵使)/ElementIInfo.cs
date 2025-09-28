using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    /// <summary>
    ///     精靈知識（精霊知識）
    /// </summary>
    public class ElementIInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "ElementIInfo", MobType.ELEMENT, MobType.ELEMENT_BOSS_SKILL
                , MobType.ELEMENT_MATERIAL_NOTOUCH_SKILL, MobType.ELEMENT_NOTOUCH
                , MobType.ELEMENT_NOTOUCH_SKILL, MobType.ELEMENT_NOTPTDROPRANGE
                , MobType.ELEMENT_SKILL, MobType.ELEMENT_SKILL_BOSS);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}