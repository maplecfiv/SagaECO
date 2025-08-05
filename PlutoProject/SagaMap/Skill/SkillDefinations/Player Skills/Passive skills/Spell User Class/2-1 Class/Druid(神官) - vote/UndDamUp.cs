using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Druid
{
    /// <summary>
    ///     不死系傷害增加
    /// </summary>
    public class UndDamUp : ISkill
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

            var skill = new SomeTypeDamUp(args.skill, dActor, "UndDamUp");
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