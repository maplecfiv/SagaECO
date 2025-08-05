using SagaDB.Actor;
using SagaMap.Skill.SkillDefinations.Global;

namespace SagaMap.Skill.NewSkill.FR1
{
    public class Synthese : SkillEvent
    {
        protected override void RunScript(Parameter para)
        {
            Scripting.SkillEvent.Instance.Synthese((ActorPC)para.dActor, (ushort)para.args.skill.ID, para.level, true);
        }
    }
}