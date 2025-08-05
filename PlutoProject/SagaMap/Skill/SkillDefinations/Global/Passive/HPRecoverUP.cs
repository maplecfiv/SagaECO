using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class HPRecoverUP : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "HPRecoverUP", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value;
            value = skill.skill.Level;
            if (skill.Variable.ContainsKey("HPRecover"))
                skill.Variable.Remove("HPRecover");
            skill.Variable.Add("HPRecover", value);
            actor.Status.hp_recover_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var value = skill.Variable["HPRecover"];
            actor.Status.hp_recover_skill -= (short)value;
        }

        #endregion
    }
}