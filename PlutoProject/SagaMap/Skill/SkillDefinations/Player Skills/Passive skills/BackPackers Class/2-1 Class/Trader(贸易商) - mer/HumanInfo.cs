using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Trader_贸易商____mer
{
    /// <summary>
    ///     人類知識（人間知識）
    /// </summary>
    public class HumanInfo : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new Knowledge(args.skill, sActor, "HumanInfo", MobType.HUMAN, MobType.HUMAN_BOSS
                , MobType.HUMAN_BOSS_CHAMP, MobType.HUMAN_BOSS_SKILL
                , MobType.HUMAN_CHAMP, MobType.HUMAN_NOTOUCH
                , MobType.HUMAN_RIDE, MobType.HUMAN_SKILL
                , MobType.HUMAN_SKILL_BOSS_CHAMP, MobType.HUMAN_SKILL_CHAMP
                , MobType.HUMAN_SMARK_BOSS_HETERODOXY, MobType.HUMAN_SMARK_HETERODOXY);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}