using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Wizard
{
    public class EnergyExcess : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultPassiveSkill(args.skill, sActor, "EnergyExcess", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.TInt["EnergyExcess"] = skill.skill.Level;
            }
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.TInt["EnergyExcess"] = 0;
                pc.TInt.Remove("EnergyExcess");
            }
        }

        #endregion
    }
}