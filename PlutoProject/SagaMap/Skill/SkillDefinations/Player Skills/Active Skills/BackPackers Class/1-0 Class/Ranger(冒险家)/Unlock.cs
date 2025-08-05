using SagaDB.Actor;
using SagaMap.Skill.SkillDefinations.Global;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    public class Unlock : SkillEvent
    {
        protected override void RunScript(Parameter para)
        {
            Scripting.SkillEvent.Instance.OpenTreasureBox((ActorPC)para.sActor);
        }
    }
}