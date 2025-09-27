using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Druid_神官____vote
{
    /// <summary>
    ///     魔法封印（封魔）
    /// </summary>
    public class SealMagic : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3000 + 1000 * level;
            var skill = new MoveSpeedDown(args.skill, dActor, lifetime);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}