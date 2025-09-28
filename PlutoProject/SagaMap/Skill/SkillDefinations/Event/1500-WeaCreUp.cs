using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     提升會心一擊機率（クリティカル率アップ）
    /// </summary>
    public class WeaCreUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "WeaCreUp", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.cri_skill += 15;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.cri_skill -= 15;
        }

        #endregion
    }
}