using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     レイビングソード
    /// </summary>
    public class LivingSword : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 3.5f + 2.0f * level;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, SkillHandler.DefType.Def, Elements.Neutral, factor);
        }

        #endregion
    }
}