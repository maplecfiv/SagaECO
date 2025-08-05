using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.FFarden;
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
        public void OnFFardenFurnitureUse(CSMG_FF_FURNITURE_USE p)
        {
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            if (furniture.Motion == 111)
                furniture.Motion = 622;
            else furniture.Motion = 111;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, null, furniture, false);

            /*if(furniture.ItemID == 31164100)
           {
               byte level = 1;
               if (Character.CInt["料理EXP"] > 5000)
                   level = 2;
               if (Character.CInt["料理EXP"] > 30000)
                   level =3;
               if (Character.CInt["料理EXP"] > 150000)
                   level = 4;
               if (Character.CInt["料理EXP"] > 500000)
                   level = 5;
               Scripting.SkillEvent.Instance.Synthese(Character, 2009, level);
               SendSystemMessage("当前料理经验：" + Character.CInt["料理EXP"].ToString() + " 等级：" + level.ToString());

           }
           //Item item = ItemFactory.Instance.GetItem(furniture.ItemID);
          if (item.BaseData.eventID != 0)
           {
               EventActivate(item.BaseData.eventID);
           }*/
        }

        public void OnFFardenOtherJoin(CSMG_FFGARDEN_JOIN_OTHER p)
        {
            var ringID = MapServer.charDB.GetFFRindID(p.ff_id);
            var ring = RingManager.Instance.GetRing(ringID);
            if (ring == null || ring.FFarden == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            OnFFardenJoin(ringID);
        }

        public void OnFFardenJoin(CSMG_FFGARDEN_JOIN p)
        {
            if (Character.Ring == null || Character.Ring.FFarden == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            OnFFardenJoin(Character.Ring.ID);
        }

        public void OnFFardenJoin(uint ringid)
        {
            var ring = RingManager.Instance.GetRing(ringid);
            if (ring == null || ring.FFarden == null)
            {
                SendSystemMessage("错误，当前工会没有飞空城！");
                return;
            }

            MapServer.charDB.GetFFurniture(ring);
            if (ring.FFarden.MapID == 0)
            {
                var map = MapManager.Instance.GetMap(Character.MapID);
                ring.FFarden.MapID = MapManager.Instance.CreateMapInstance(ring, 90001000, Character.MapID,
                    Global.PosX16to8(Character.X, map.Width), Global.PosY16to8(Character.Y, map.Height), false);
                map = MapManager.Instance.GetMap(ring.FFarden.MapID);
                var ffmap = MapManager.Instance.GetMap(ring.FFarden.MapID);
                var aa = new List<uint>();
                foreach (var y in ffmap.Actors)
                    if (y.Value.type == ActorType.FURNITURE)
                        aa.Add(y.Key);
                foreach (var i in aa) ffmap.DeleteActor(ffmap.Actors[i]);
                foreach (var i in ring.FFarden.Furnitures[FurniturePlace.GARDEN])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FFarden.Furnitures[FurniturePlace.FARM])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FFarden.Furnitures[FurniturePlace.FISHERY])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }

                foreach (var i in ring.FFarden.Furnitures[FurniturePlace.HOUSE])
                {
                    i.e = new NullEventHandler();
                    map.RegisterActor(i);
                    i.invisble = false;
                    ;
                }
            }

            Character.BattleStatus = 0;
            Character.Speed = 200;
            FromActorPC(Character).SendChangeStatus();

            if (Configuration.Instance.HostedMaps.Contains(Character.MapID))
            {
                var newMap = MapManager.Instance.GetMap(Character.MapID);
                if (Character.Marionette != null)
                    MarionetteDeactivate();
                Map.SendActorToMap(Character, ring.FFarden.MapID, 20, 20, true);
            }

            /*Packet p = new Packet();
            string args = "20 44 00 00 00 01 00 00 00 00 F6 31 FF 94 00 64";
            byte[] buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            this.netIO.SendPacket(p);*/
        }

        public void OnFFurnitureSetup(CSMG_FF_FURNITURE_SETUP p)
        {
            if (Character.MapID == 90001999 || Character.MapID == 91000999) // 家具設定
            {
                if (Character.Account.GMLevel < 250)
                {
                    SendSystemMessage("哎哟——？");
                    return;
                }

                CustomMapManager.Instance.SerFFofFurnitureSetup(this, p);
            }

            if (Character.Ring == null || Character.Ring.FFarden == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FFarden.MapID && Character.MapID != ring.FFarden.RoomMapID)
                return;
            if (ring.FFarden.Furnitures[FurniturePlace.GARDEN].Count +
                ring.FFarden.Furnitures[FurniturePlace.ROOM].Count < Configuration.Instance.MaxFurnitureCount)
            {
                var item = Character.Inventory.GetItem(p.InventorySlot);
                var actor = new ActorFurniture();

                if (Character.Account.GMLevel < 100)
                    DeleteItem(p.InventorySlot, 1, false);

                actor.MapID = Character.MapID;
                actor.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.X = p.X;
                actor.Y = p.Y;
                actor.Z = p.Z;
                actor.Xaxis = p.Xaxis;
                actor.Yaxis = p.Yaxis;
                actor.Zaxis = p.Zaxis;
                actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
                actor.PictID = item.PictID;
                actor.e = new NullEventHandler();
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);

                if (Character.MapID == Character.Ring.FFarden.MapID)
                    ring.FFarden.Furnitures[FurniturePlace.GARDEN].Add(actor);
                else
                    ring.FFarden.Furnitures[FurniturePlace.ROOM].Add(actor);
                SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_SETUP, actor.Name,
                    ring.FFarden.Furnitures[FurniturePlace.GARDEN].Count +
                    ring.FFarden.Furnitures[FurniturePlace.ROOM].Count, Configuration.Instance.MaxFurnitureCount));
                MapServer.charDB.SaveFF(ring);
            }
            else
            {
                SendSystemMessage(LocalManager.Instance.Strings.FG_FUTNITURE_MAX);
            }
        }

        public void OnFFFurnitureRemoveCastle(CSMG_FF_FURNITURE_REMOVE_CASTLE p)
        {
            if (Character.MapID == 90001999) CustomMapManager.Instance.RemoveFurnitureCastle(this, p);
        }

        public void OnFFFurnitureReset(CSMG_FF_FURNITURE_RESET p)
        {
            var actorID = p.ActorID;
            var map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;
            var p2 = new SSMG_FF_FURNITURE_RESET();
            p2.AID = 1;
            p2.ActorID = furniture.ActorID;
            p2.RindID = Character.ActorID;
            netIO.SendPacket(p2);
        }

        public void OnFFFurnitureRemove(CSMG_FF_FURNITURE_REMOVE p)
        {
            if (Character.MapID == 90001999 || Character.MapID == 91000999)
            {
                CustomMapManager.Instance.RemoveFurniture(this, p);
                return;
            }

            if (Character.Ring.FFarden == null)
                return;
            if (Character.MapID != Character.Ring.FFarden.MapID && Character.MapID != Character.Ring.FFarden.RoomMapID)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            Map map = null;
            map = MapManager.Instance.GetMap(Character.MapID);
            var actor = map.GetActor(p.ActorID);
            if (actor == null)
                return;
            if (actor.type != ActorType.FURNITURE)
                return;
            var furniture = (ActorFurniture)actor;

            if (Character.MapID == Character.Ring.FFarden.MapID)
                ring.FFarden.Furnitures[FurniturePlace.GARDEN].Remove(furniture);
            else
                ring.FFarden.Furnitures[FurniturePlace.ROOM].Remove(furniture);
            map.DeleteActor(actor);
            var item = ItemFactory.Instance.GetItem(furniture.ItemID);
            item.PictID = furniture.PictID;
            MapServer.charDB.SaveFF(ring);
            AddItem(item, false);
            SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_REMOVE, furniture.Name,
                ring.FFarden.Furnitures[FurniturePlace.GARDEN].Count +
                ring.FFarden.Furnitures[FurniturePlace.ROOM].Count, Configuration.Instance.MaxFurnitureCount));
        }

        public void OnFFFurnitureRoomAppear()
        {
            var p = new Packet();
            var args = "20 44 00 00 00 01 00 00 00 00 F6 31 FF 94 00 64";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            netIO.SendPacket(p);
        }

        public void OnFFurnitureCastleSetup(CSMG_FF_CASTLE_SETUP p)
        {
            if (Character.MapID == 90001999)
            {
                CustomMapManager.Instance.SerFFFurnitureCastleSetup(this, p);
                return;
            }

            if (Character.Ring == null || Character.Ring.FFarden == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FFarden.MapID && Character.MapID != ring.FFarden.RoomMapID)
                return;
            if (ring.FFarden.Furnitures[FurniturePlace.HOUSE].Count > 0)
            {
                SendSystemMessage("无法重复设置");
                return;
            }

            var item = Character.Inventory.GetItem(p.InventorySlot);
            var actor = new ActorFurnitureUnit();

            DeleteItem(p.InventorySlot, 1, false);

            actor.MapID = Character.MapID;
            actor.ItemID = item.ItemID;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.X = p.X;
            actor.Z = p.Z;
            actor.Yaxis = p.Yaxis;
            actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
            actor.PictID = item.PictID;
            actor.e = new NullEventHandler();
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            if (Character.MapID == Character.Ring.FFarden.MapID)
                ring.FFarden.Furnitures[FurniturePlace.HOUSE].Add(actor);
            MapServer.charDB.SaveFF(ring);
        }

        public void OnFFFurnitureRoomEnter(CSMG_FF_FURNITURE_ROOM_ENTER p)
        {
            if (p.data == 1)
            {
                if (this.map.ID == 90001999)
                {
                    CustomMapManager.Instance.SerFFRoomEnter(this);
                    return;
                }

                var ring = RingManager.Instance.GetRing(Character.Ring.ID);

                if (ring == null)
                    return;
                if (ring.FFarden.RoomMapID == 0)
                {
                    ring.FFarden.RoomMapID =
                        MapManager.Instance.CreateMapInstance(ring, 91000000, ring.FFarden.MapID, 6, 7, false);
                    //spawn furnitures
                    var map = MapManager.Instance.GetMap(ring.FFarden.RoomMapID);
                    foreach (var i in ring.FFarden.Furnitures[FurniturePlace.ROOM])
                    {
                        i.e = new NullEventHandler();
                        map.RegisterActor(i);
                        i.invisble = false;
                    }
                }

                Map.SendActorToMap(Character, ring.FFarden.RoomMapID, 20, 36, true);
            }
        }

        public void OnFFurnitureUnitSetup(CSMG_FF_UNIT_SETUP p)
        {
            if (Character.Ring == null || Character.Ring.FFarden == null)
                return;
            var ring = RingManager.Instance.GetRing(Character.Ring.ID);
            if (Character.MapID != ring.FFarden.MapID && Character.MapID != ring.FFarden.RoomMapID)
                return;
            var item = Character.Inventory.GetItem(p.InventorySlot);
            var actor = new ActorFurnitureUnit();

            DeleteItem(p.InventorySlot, 1, false);
            actor.MapID = Character.MapID;
            actor.ItemID = item.ItemID;
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.X = p.X;
            actor.Y = 0;
            actor.Z = p.Z;
            actor.Xaxis = 0;
            actor.Yaxis = p.Yaxis;
            actor.Zaxis = 0;
            actor.Name = "【" + Character.Name + "】" + item.BaseData.name;
            actor.PictID = item.PictID;
            actor.e = new NullEventHandler();


            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            if (Character.MapID == Character.Ring.FFarden.MapID)
            {
                if (item.ItemID == 30300000)
                    ring.FFarden.Furnitures[FurniturePlace.FISHERY].Add(actor);
                else if (item.ItemID == 30260000)
                    ring.FFarden.Furnitures[FurniturePlace.FARM].Add(actor);
            }

            MapServer.charDB.SaveFF(ring);
        }
    }
}