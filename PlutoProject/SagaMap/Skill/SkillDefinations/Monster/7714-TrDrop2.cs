using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     解除憑依
    /// </summary>
    public class TrDrop2 : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //List<Actor> affected = map.GetActorsArea(sActor, 150, false);
            var affected = map.GetActorsArea(sActor, 750, false);
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                    SkillHandler.Instance.PossessionCancel((ActorPC)act, PossessionPosition.NONE);
        }

        //#endregion
    }
}