using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Blacksmith
{
    /// <summary>
    ///     岩石命中
    /// </summary>
    public class StoHitUp : ISkill
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

            var skill = new SomeTypeHitUp(args.skill, dActor, "StoHitUp");
            skill.AddMobType(MobType.ROCK, value);
            skill.AddMobType(MobType.ROCK_BOSS_SKILL_NOTPTDROPRANGE, value);
            skill.AddMobType(MobType.ROCK_MATERIAL, value);
            skill.AddMobType(MobType.ROCK_MATERIAL_NORTH_NOTOUCH, value);
            skill.AddMobType(MobType.ROCK_MATERIAL_SKILL, value);
            skill.AddMobType(MobType.ROCK_MATERIAL_SOUTH_NOTOUCH, value);
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