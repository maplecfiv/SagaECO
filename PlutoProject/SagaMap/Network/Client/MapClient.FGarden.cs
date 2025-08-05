using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaDB.FGGarden;
using SagaDB.Furniture;
using SagaDB.Item;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnFGardenFurnitureUse(CSMG_FGARDEN_FURNITURE_USE p)
        {
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);

            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;

            //this.Character.NowUseFurnitureID = p.ActorID;

            //Item item = ItemFactory.Instance.GetItem(furniture.ItemID);
            var f = FurnitureFactory.Instance.GetFurniture(furniture.ItemID);

            if (f.Motion.Count() <= 0)
            {
                EventActivate(31080000);
                return;
            }

            if (f.Motion.Count() > 1)
            {
                int res;
                //多選
                var ps = new SSMG_NPC_SELECT();
                ps.SetSelect("要做什麼？", "", f.Motion.Select(x => x.ToString()).ToArray(), false);

                npcSelectResult = -1;
                netIO.SendPacket(ps);

                var blocked = ClientManager.Blocked;
                if (blocked)
                    ClientManager.LeaveCriticalArea();
                while (npcSelectResult == -1) Thread.Sleep(500);
                if (blocked)
                    ClientManager.EnterCriticalArea();
                var ps2 = new SSMG_NPC_SELECT_RESULT();
                netIO.SendPacket(ps2);
                res = npcSelectResult;

                SendSystemMessage("家具Select: " + res);
                SendSystemMessage("家具Set motion: " + f.Motion[res - 1]);

                /*
                if (res > 0)
                {
                    furniture.Motion = f.Motion[(res-1)];
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
                */
            }
            else
            {
                //單選
                if (furniture.Motion != f.DefaultMotion)
                {
                    furniture.Motion = f.DefaultMotion;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
                else
                {
                    furniture.Motion = f.Motion[0];
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);
                }
            }


            EventActivate(31000000);
        }

        public void OnFGardenFurnitureReconfig(CSMG_FGARDEN_FURNITURE_RECONFIG p)
        {
            if (Character.FGarden == null)
                return;
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            if (Character.MapID != Character.FGarden.MapID && Character.MapID != Character.FGarden.RoomMapID)
            {
                var p1 = new SSMG_FG_FURNITURE_RECONFIG();
                p1.ActorID = actor.ActorID;
                p1.X = actor.X;
                p1.Y = actor.Y;
                p1.Z = ((ActorFurniture)actor).Z;
                p1.Dir = actor.Dir;
                netIO.SendPacket(p1);
                return;
            }

            map.MoveActor(Map.MOVE_TYPE.START, actor, new[] { p.X, p.Y, p.Z }, p.Dir, 200);
        }

        public void OnFGardenFurnitureRemove(CSMG_FGARDEN_FURNITURE_REMOVE p)
        {
            if (Character.FGarden == null)
                return;
            if (Character.MapID != Character.FGarden.MapID && Character.MapID != Character.FGarden.RoomMapID)
                return;
            Map map = null;
            map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            map.DeleteActor(actor);
            var item = ItemFactory.Instance.GetItem(furniture.ItemID);
            item.PictID = furniture.PictID;
            if (Character.MapID == Character.FGarden.MapID)
                Character.FGarden.Furnitures[FurniturePlace.GARDEN].Remove(furniture);
            else
                Character.FGarden.Furnitures[FurniturePlace.ROOM].Remove(furniture);
            AddItem(item, false);
            SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_REMOVE, furniture.Name,
                Character.FGarden.Furnitures[FurniturePlace.GARDEN].Count +
                Character.FGarden.Furnitures[FurniturePlace.ROOM].Count, Configuration.Instance.MaxFurnitureCount));
        }

        public void OnFGardenFurnitureSetup(CSMG_FGARDEN_FURNITURE_SETUP p)
        {
            if (Character.FGarden == null)
                return;
            if (Character.MapID != Character.FGarden.MapID && Character.MapID != Character.FGarden.RoomMapID)
                return;
            if (Character.FGarden.Furnitures[FurniturePlace.GARDEN].Count +
                Character.FGarden.Furnitures[FurniturePlace.ROOM].Count < Configuration.Instance.MaxFurnitureCount)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                var actor = new ActorFurniture();

                DeleteItem(p.InventorySlot, 1, false);

                actor.MapID = Character.MapID;
                actor.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.X = p.X;
                actor.Y = p.Y;
                actor.Z = p.Z;
                //actor.Dir = p.Dir;
                actor.Xaxis = p.AxleX;
                actor.Yaxis = p.AxleY;
                actor.Zaxis = p.AxleZ;
                actor.Name = item.BaseData.name;
                actor.PictID = item.PictID;
                actor.e = new NullEventHandler();
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);

                if (Character.MapID == Character.FGarden.MapID)
                    Character.FGarden.Furnitures[FurniturePlace.GARDEN].Add(actor);
                else
                    Character.FGarden.Furnitures[FurniturePlace.ROOM].Add(actor);
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_SETUP, actor.Name,
                    Character.FGarden.Furnitures[FurniturePlace.GARDEN].Count +
                    Character.FGarden.Furnitures[FurniturePlace.ROOM].Count, Configuration.Instance.MaxFurnitureCount));
            }
            else
            {
                SendSystemMessage(LocalManager.Instance.Strings.FG_FUTNITURE_MAX);
            }
        }

        public void OnFGardenEquipt(CSMG_FGARDEN_EQUIPT p)
        {
            if (Character.FGarden == null)
                return;
            if (Character.MapID != Character.FGarden.MapID && Character.MapID != Character.FGarden.RoomMapID)
                return;
            if (p.InventorySlot != 0xFFFFFFFF)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                if (item == null)
                    return;
                if (Character.FGarden.FGardenEquipments[p.Place] != 0)
                {
                    var itemID = Character.FGarden.FGardenEquipments[p.Place];
                    AddItem(ItemFactory.Instance.GetItem(itemID, true), false);
                    var p1 = new SSMG_FG_EQUIPT();
                    p1.ItemID = 0;
                    p1.Place = p.Place;
                    netIO.SendPacket(p1);
                }

                if (p.Place == FGardenSlot.GARDEN_MODELHOUSE &&
                    Character.FGarden.FGardenEquipments[FGardenSlot.GARDEN_MODELHOUSE] == 0)
                {
                    var p1 = new SSMG_NPC_SET_EVENT_AREA();
                    p1.EventID = 10000315;
                    p1.StartX = 6;
                    p1.StartY = 7;
                    p1.EndX = 6;
                    p1.EndY = 7;
                    netIO.SendPacket(p1);
                }

                Character.FGarden.FGardenEquipments[p.Place] = item.ItemID;
                var p2 = new SSMG_FG_EQUIPT();
                p2.ItemID = item.ItemID;
                p2.Place = p.Place;
                netIO.SendPacket(p2);
                DeleteItem(p.InventorySlot, 1, false);
            }
            else
            {
                var itemID = Character.FGarden.FGardenEquipments[p.Place];
                if (itemID != 0)
                    AddItem(ItemFactory.Instance.GetItem(itemID, true), false);
                Character.FGarden.FGardenEquipments[p.Place] = 0;
                var p1 = new SSMG_FG_EQUIPT();
                p1.ItemID = 0;
                p1.Place = p.Place;
                netIO.SendPacket(p1);
                if (p.Place == FGardenSlot.GARDEN_MODELHOUSE)
                {
                    var p2 = new SSMG_NPC_CANCEL_EVENT_AREA();
                    p2.StartX = 6;
                    p2.StartY = 7;
                    p2.EndX = 6;
                    p2.EndY = 7;
                    netIO.SendPacket(p2);
                }
            }
        }
    }
}