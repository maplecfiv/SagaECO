using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock
{
    /// <summary>
    ///     聖曜光輝（ファナティシズム）
    /// </summary>
    public class Fanaticism : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 20 + 10 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "Berserk", rate))
            {
                var skill = new Berserk(args.skill, dActor, 30000);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        //#endregion
    }
}