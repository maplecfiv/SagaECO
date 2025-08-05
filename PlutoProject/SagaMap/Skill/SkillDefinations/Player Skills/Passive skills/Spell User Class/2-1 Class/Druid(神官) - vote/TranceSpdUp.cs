using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Druid
{
    /// <summary>
    ///     提升憑依速度（憑依速度上昇）
    /// </summary>
    public class TranceSpdUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "TranceSpdUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["TranceSpdUp"] = skill.skill.Level * 3;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            skill["TranceSpdUp"] = 0;
        }

        #endregion
    }
}