using SagaDB.Actor;
using SagaMap.Skill.SkillDefinations.Global;

namespace SagaMap.Skill.SkillDefinations.Ranger
{
    public class Unlock : SkillEvent
    {
        protected override void RunScript(Parameter para)
        {
            Scripting.SkillEvent.Instance.OpenTreasureBox((ActorPC)para.sActor);
        }
    }
}