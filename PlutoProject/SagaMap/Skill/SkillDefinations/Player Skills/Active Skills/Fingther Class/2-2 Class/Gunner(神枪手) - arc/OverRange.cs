using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Gunner_神枪手____arc
{
    /// <summary>
    ///     射程延長（オーバーレンジ）
    /// </summary>
    public class OverRange : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "OverRange", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //射程上升量為Level
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        //#endregion
    }
}