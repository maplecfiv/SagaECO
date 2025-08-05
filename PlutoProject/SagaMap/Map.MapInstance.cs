using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Ring;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Dungeon;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Packets.Client;

namespace SagaMap
{
    public partial class Map
    {
        public bool IsMapInstance { get; set; }

        public bool IsDungeon { get; set; }

        public DungeonMap DungeonMap { get; set; }

        public uint ClientExitMap { get; set; }

        public byte ClientExitX { get; set; }

        public byte ClientExitY { get; set; }

        public bool AutoDispose { get; set; }

        public ActorPC Creator { get; set; }

        public Ring Ring { get; set; }

        public uint ResurrectionLimit { get; set; }

        public void OnDestrory()
        {
            var pcs = new List<Actor>();
            var items = new List<Actor>();
            var other = new List<Actor>();
            if (MobSpawnManager.Instance.Spawns.ContainsKey(ID))
            {
                foreach (var mob in MobSpawnManager.Instance.Spawns[ID])
                {
                    var eh = (MobEventHandler)mob.e;
                    AIThread.Instance.RemoveAI(eh.AI);
                    //Mob.NewAIThread.Instance.RemoveAI(eh.NewAI);
                    foreach (var i in mob.Tasks.Values) i.Deactivate();
                    mob.Tasks.Clear();
                }

                MobSpawnManager.Instance.Spawns[ID].Clear();
                MobSpawnManager.Instance.Spawns.Remove(ID);
            }

            foreach (var mob in Actors.Values)
                if (mob.type == ActorType.MOB)
                {
                    var eh = (MobEventHandler)mob.e;
                    AIThread.Instance.RemoveAI(eh.AI);
                    foreach (var i in mob.Tasks.Values) i.Deactivate();
                    mob.Tasks.Clear();
                }

            foreach (var i in Actors.Values)
                if (i.type == ActorType.PC)
                    pcs.Add(i);
                else if (i.type == ActorType.ITEM)
                    items.Add(i);
                else if (i.type == ActorType.GOLEM)
                    try
                    {
                        var golem = (ActorGolem)i;
                        MapServer.charDB.SaveChar(golem.Owner, false);
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }
                else
                    other.Add(i);

            foreach (var i in other)
            {
                i.ClearTaskAddition();
                DeleteActor(i);
            }

            var map = MapManager.Instance.GetMap(ClientExitMap);
            foreach (var i in items)
            {
                var item = (ActorItem)i;
                if (item.Item.PossessionedActor != null)
                {
                    var p = new CSMG_POSSESSION_CANCEL();
                    p.PossessionPosition = PossessionPosition.NONE;
                    var eh = (PCEventHandler)item.Item.PossessionedActor.e;
                    var posActor = item.Item.PossessionedActor;
                    if (posActor.Online)
                        eh.Client.OnPossessionCancel(p);
                    else
                        posActor.PossessionTarget = 0;
                    eh.Client.map.SendActorToMap(posActor, ClientExitMap, Global.PosX8to16(ClientExitX, map.Width),
                        Global.PosY8to16(ClientExitY, map.Height));
                    if (!posActor.Online)
                    {
                        MapServer.charDB.SaveChar(posActor, false, false);
                        MapServer.accountDB.WriteUser(posActor.Account);
                        MapManager.Instance.GetMap(posActor.MapID).DeleteActor(posActor);
                        MapClient.FromActorPC(posActor).DisposeActor();
                        posActor.Account = null;
                    }

                    if (pcs.Contains(posActor))
                        pcs.Remove(posActor);
                }

                i.Speed = Configuration.Instance.Speed;
                i.ClearTaskAddition();
                DeleteActor(i);
            }

            foreach (var i in pcs)
            {
                i.Speed = Configuration.Instance.Speed;
                try
                {
                    SendActorToMap(i, ClientExitMap, Global.PosX8to16(ClientExitX, map.Width),
                        Global.PosY8to16(ClientExitY, map.Height));
                }
                catch
                {
                }
            }
        }
    }
}