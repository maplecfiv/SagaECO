using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Assassin
{
    /// <summary>
    ///     提升抗毒性（毒耐性上昇）
    /// </summary>
    public class PoisonRegiUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            //對「硬化毒、豪腕毒、狂毒氣」三種狀態無效
            var skill = new DefaultPassiveSkill(args.skill, sActor, "PoisonRegiUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}