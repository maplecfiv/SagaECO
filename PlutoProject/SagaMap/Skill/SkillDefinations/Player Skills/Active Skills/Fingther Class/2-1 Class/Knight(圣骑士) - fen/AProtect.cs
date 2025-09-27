using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Knight_圣骑士____fen
{
    /// <summary>
    ///     聖騎士誓約（プロテクト）
    /// </summary>
    public class AProtect : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.PossessionTarget == 0) return 0;

            return -25;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 24000 + 3000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "AProtect", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.possessionTakeOver = 100;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.possessionTakeOver = 0;
        }

        #endregion
    }
}