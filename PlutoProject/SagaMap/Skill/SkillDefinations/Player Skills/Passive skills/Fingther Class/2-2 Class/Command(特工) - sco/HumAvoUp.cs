using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class MaxHealMpForWeapon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ushort[] Values = { 0, 3, 6, 9, 12, 15 }; //%
            var value = Values[level];

            var skill = new SomeTypeAvoUp(args.skill, dActor, "HumAvoidUp");
            skill.AddMobType(MobType.HUMAN, value);
            skill.AddMobType(MobType.HUMAN_BOSS, value);
            skill.AddMobType(MobType.HUMAN_BOSS_CHAMP, value);
            skill.AddMobType(MobType.HUMAN_BOSS_SKILL, value);
            skill.AddMobType(MobType.HUMAN_CHAMP, value);
            skill.AddMobType(MobType.HUMAN_NOTOUCH, value);
            skill.AddMobType(MobType.HUMAN_RIDE, value);
            skill.AddMobType(MobType.HUMAN_SKILL, value);
            skill.AddMobType(MobType.HUMAN_SKILL_BOSS_CHAMP, value);
            skill.AddMobType(MobType.HUMAN_SKILL_CHAMP, value);
            skill.AddMobType(MobType.HUMAN_SMARK_BOSS_HETERODOXY, value);
            skill.AddMobType(MobType.HUMAN_SMARK_HETERODOXY, value);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}