using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Map;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Scripting;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     露营
    /// </summary>
    public class Camp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var tent = false;
            var item = sActor.Inventory.GetItem(10037900, Inventory.SearchType.ITEM_ID);
            if (item != null)
                tent = false;
            else
                tent = true;

            if (tent) return -37;

            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.Info.walkable[args.x, args.y] != 0 && !map.Info.Flag.Test(MapFlags.Healing) &&
                sActor.TenkActor == null)
            {
                item.Durability -= 1;
                return 0;
            }

            if (sActor.TenkActor != null)
                //SagaMap.Network.Client.MapClient.FromActorPC(sActor).SendSystemMessage("已搭建有帐篷");
                return -18;

            //SagaMap.Network.Client.MapClient.FromActorPC(sActor).SendSystemMessage("该地方无法搭建帐篷");
            return -6;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pc = (ActorPC)sActor;
            var actor = new ActorEvent(pc);
            var map = MapManager.Instance.GetMap(pc.MapID);

            actor.MapID = pc.MapID;
            actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            var eventID = ScriptManager.Instance.GetFreeIDSince(0xF0001233, 2000);
            actor.EventID = eventID;
            pc.TenkActor = actor;
            if (ScriptManager.Instance.Events.ContainsKey(0xF0001233))
            {
                var pattern = (EventActor)ScriptManager.Instance.Events[0xF0001233];
                var newEvent = pattern.Clone();
                newEvent.Actor = actor;
                newEvent.EventID = actor.EventID;
                ScriptManager.Instance.Events.Add(newEvent.EventID, newEvent);
            }

            actor.Type = ActorEventTypes.TENT;
            actor.Title = string.Format(pc.Name + "的帐篷");
            actor.e = new ItemEventHandler(actor);
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
        }

        #endregion
    }
}