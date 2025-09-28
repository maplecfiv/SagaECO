using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     不死系迴避
    /// </summary>
    public class UndAvoUp : ISkill
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

            var skill = new SomeTypeAvoUp(args.skill, dActor, "UndAvoUp");
            skill.AddMobType(MobType.UNDEAD, value);
            skill.AddMobType(MobType.UNDEAD_BOSS, value);
            skill.AddMobType(MobType.UNDEAD_BOSS_BOMB_SKILL, value);
            skill.AddMobType(MobType.UNDEAD_BOSS_CHAMP_BOMB_SKILL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.UNDEAD_BOSS_SKILL, value);
            skill.AddMobType(MobType.UNDEAD_BOSS_SKILL_CHAMP, value);
            skill.AddMobType(MobType.UNDEAD_BOSS_SKILL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.UNDEAD_NOTOUCH, value);
            skill.AddMobType(MobType.UNDEAD_SKILL, value);
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