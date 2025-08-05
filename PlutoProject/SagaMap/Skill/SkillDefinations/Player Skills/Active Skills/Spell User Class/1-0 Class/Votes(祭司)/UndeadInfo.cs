using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    /// <summary>
    ///     邪靈知識（死霊知識）圣印术
    /// </summary>
    public class UndeadInfo : ISkill
    {
        private void sss(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //创建一个默认被动技能处理对象
            var skill2 = new DefaultPassiveSkill(args.skill, sActor, "Seals", true);
            skill2.OnAdditionEnd += sss;
            skill2.OnAdditionStart += sss;
            //对指定Actor附加技能效果
            SkillHandler.ApplyAddition(sActor, skill2);

            var skill = new Knowledge(args.skill, sActor, "UndeadInfo", MobType.UNDEAD, MobType.UNDEAD_BOSS
                , MobType.UNDEAD_BOSS_BOMB_SKILL, MobType.UNDEAD_BOSS_CHAMP_BOMB_SKILL_NOTPTDROPRANGE
                , MobType.UNDEAD_BOSS_SKILL, MobType.UNDEAD_BOSS_SKILL_CHAMP
                , MobType.UNDEAD_BOSS_SKILL_NOTPTDROPRANGE, MobType.UNDEAD_NOTOUCH
                , MobType.UNDEAD_SKILL);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}