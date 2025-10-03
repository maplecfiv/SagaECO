using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     雙絞壓制（羽交い絞め）
    /// </summary>
    public class FullNelson : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "FullNelson", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        //#endregion
    }
}