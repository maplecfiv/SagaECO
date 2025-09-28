using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Passive
{
    public class SPRecoverUP : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "SPRecoverUP", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value;
            value = skill.skill.Level;
            if (skill.Variable.ContainsKey("SPRecover"))
                skill.Variable.Remove("SPRecover");
            skill.Variable.Add("SPRecover", value);
            actor.Status.sp_recover_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["SPRecover"];
            actor.Status.sp_recover_skill -= (short)value;
        }

        #endregion
    }
}