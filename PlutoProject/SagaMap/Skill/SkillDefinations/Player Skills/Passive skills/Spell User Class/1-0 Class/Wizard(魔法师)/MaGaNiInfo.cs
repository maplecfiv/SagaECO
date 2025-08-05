using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    /// <summary>
    ///     魔法生物知識（魔法生物知識）
    /// </summary>
    public class MaGaNiInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "MaGaNiInfo", MobType.MAGIC_CREATURE,
                MobType.MAGIC_CREATURE_BOSS
                , MobType.MAGIC_CREATURE_BOSS_SKILL, MobType.MAGIC_CREATURE_BOSS_SKILL_NOTPTDROPRANGE
                , MobType.MAGIC_CREATURE_LVDIFF, MobType.MAGIC_CREATURE_MATERIAL
                , MobType.MAGIC_CREATURE_NOTOUCH, MobType.MAGIC_CREATURE_NOTPTDROPRANGE
                , MobType.MAGIC_CREATURE_RIDE, MobType.MAGIC_CREATURE_SKILL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}