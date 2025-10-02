using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     移動技能
    /// </summary>
    public class EventMoveSkill : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint TargetMobID = 14110000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            foreach (var act in map.Actors)
                if (act.Value.type == ActorType.MOB)
                {
                    var m = (ActorMob)act.Value;
                    if (m.BaseData.id == TargetMobID)
                    {
                        SkillHandler.Instance.AttractMob(sActor, m);
                        MapClientManager.Instance.FindClient((ActorPC)sActor).map.SendActorToMap(m,
                            MapManager.Instance.GetMap(sActor.MapID), sActor.X, sActor.Y);
                        m.invisble = false;
                        MapClientManager.Instance.FindClient((ActorPC)sActor).map.OnActorVisibilityChange(m);
                        return;
                    }
                }
        }

        //#endregion
    }
}