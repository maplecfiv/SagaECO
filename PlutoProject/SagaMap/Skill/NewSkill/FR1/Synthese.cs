using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class Synthese : SkillEvent
    {
        protected override void RunScript(Parameter para)
        {
            Scripting.SkillEvent.Instance.Synthese((ActorPC)para.dActor, (ushort)para.args.skill.ID, para.level, true);
        }
    }
}