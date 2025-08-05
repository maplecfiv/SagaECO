using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class Identify : SkillEvent
    {
        protected override void RunScript(Parameter para)
        {
            Scripting.SkillEvent.Instance.Identify((ActorPC)para.sActor);
        }
    }
}