using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Explorer_探险家____rag
{
    /// <summary>
    ///     蟲族迴避
    /// </summary>
    public class InsAvoUp : ISkill
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

            var skill = new SomeTypeAvoUp(args.skill, dActor, "InsAvoUp");
            skill.AddMobType(MobType.INSECT, value);
            skill.AddMobType(MobType.INSECT_BOSS, value);
            skill.AddMobType(MobType.INSECT_BOSS_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.INSECT_BOSS_SKILL, value);
            skill.AddMobType(MobType.INSECT_NOTOUCH, value);
            skill.AddMobType(MobType.INSECT_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.INSECT_RIDE, value);
            skill.AddMobType(MobType.INSECT_SKILL, value);
            skill.AddMobType(MobType.INSECT_UNITE, value);
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