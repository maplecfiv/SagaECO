using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    public class PartyBivouac : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 300, true);
            var affected = new List<Actor>();
            foreach (var i in actors)
            {
                var skill = new HPRecovery(args.skill, sActor, 300000, 5000);
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        #endregion
    }
}