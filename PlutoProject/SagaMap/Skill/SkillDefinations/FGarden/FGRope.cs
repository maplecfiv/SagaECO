using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Scripting;

namespace SagaMap.Skill.SkillDefinations.FGarden
{
    public class FGRope : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.FGarden != null)
                {
                    if (pc.FGarden.RopeActor == null)
                    {
                        var map = MapManager.Instance.GetMap(pc.MapID);
                        if (map.Info.Flag.Test(MapFlags.FGarden))
                            createNewRope(args, pc);
                        else
                            MapClient.FromActorPC(pc).SendSystemMessage(LocalManager.Instance.Strings.FG_CANNOT);
                    }
                    else
                    {
                        if (!ScriptManager.Instance.Events.ContainsKey(pc.FGarden.RopeActor.EventID))
                        {
                            var map = MapManager.Instance.GetMap(pc.MapID);
                            if (map.Info.Flag.Test(MapFlags.FGarden))
                            {
                                map = MapManager.Instance.GetMap(pc.FGarden.RopeActor.MapID);
                                map.DeleteActor(pc.FGarden.RopeActor);
                                createNewRope(args, pc);
                            }
                            else
                            {
                                MapClient.FromActorPC(pc).SendSystemMessage(LocalManager.Instance.Strings.FG_CANNOT);
                            }
                        }
                        else
                        {
                            MapClient.FromActorPC(pc)
                                .SendSystemMessage(LocalManager.Instance.Strings.FG_ALREADY_CALLED);
                        }
                    }
                }
                else
                {
                    MapClient.FromActorPC(pc).SendSystemMessage(LocalManager.Instance.Strings.FG_NOT_FOUND);
                }
            }
        }

        private void createNewRope(SkillArg args, ActorPC pc)
        {
            var actor = new ActorEvent(pc);
            var map = MapManager.Instance.GetMap(pc.MapID);

            actor.MapID = pc.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            var eventID = ScriptManager.Instance.GetFreeIDSince(0xF0000100, 1000);
            actor.EventID = eventID;
            pc.FGarden.RopeActor = actor;
            if (ScriptManager.Instance.Events.ContainsKey(0xF0000100))
            {
                var pattern = (EventActor)ScriptManager.Instance.Events[0xF0000100];
                var newEvent = pattern.Clone();
                newEvent.Actor = actor;
                newEvent.EventID = actor.EventID;
                ScriptManager.Instance.Events.Add(newEvent.EventID, newEvent);
            }

            actor.Type = ActorEventTypes.ROPE;
            actor.Title = string.Format(LocalManager.Instance.Strings.FG_NAME, pc.Name);
            actor.e = new ItemEventHandler(actor);
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
        }

        #endregion
    }
}