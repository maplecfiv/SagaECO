using SagaDB.Actor;
using SagaDB.FFGarden;
using SagaDB.Item;
using SagaDB.Server;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Network.Client;
using SagaMap.Packets.Client;
using SagaMap.Packets.Client.FFarden;
using SagaMap.Packets.Server;
using SagaMap.Packets.Server.FFarden;
using SagaMap.Packets.Server.FGarden;

namespace SagaMap.Manager
{
    public class CustomMapManager : Singleton<CustomMapManager>
    {
        private ActorFurniture House;
        public Server ser;

        public CustomMapManager()
        {
            ser = new Server();
            MapServer.charDB.GetSerFFurniture(ser);
        }

        public void CreateFF()
        {
            MapManager.Instance.CreateFFInstanceOfSer();
            var map = MapManager.Instance.GetMap(90001999);
            foreach (var i in ser.Furnitures[FurniturePlace.GARDEN])
            {
                i.e = new NullEventHandler();
                map.RegisterActor(i);
                i.invisble = false;
            }

            foreach (var i in ser.Furnitures[FurniturePlace.HOUSE])
            {
                House = i;
                i.e = new NullEventHandler();
                map.RegisterActor(i);
                i.invisble = false;
            }

            map = MapManager.Instance.GetMap(91000999);
            foreach (var i in ser.Furnitures[FurniturePlace.ROOM])
            {
                i.e = new NullEventHandler();
                map.RegisterActor(i);
                i.invisble = false;
            }
        }

        public void EnterFFOnMapLoaded(MapClient client)
        {
            var sky = ScriptManager.Instance.VariableHolder.AInt["服務器FF背景"];
            var weather = ScriptManager.Instance.VariableHolder.AInt["服務器FF天氣"];
            var p = new SSMG_FG_CHANGE_SKY();
            p.Sky = (byte)sky;
            client.NetIo.SendPacket(p);

            var p2 = new SSMG_FG_CHANGE_WEATHER();
            p2.Weather = (byte)weather;
            client.NetIo.SendPacket(p2);
        }

        public void SerFFRoomEnter(MapClient client)
        {
            var map = MapManager.Instance.GetMap(91000999);
            client.Map.SendActorToMap(client.Character, 91000999, Global.PosX8to16(20, map.Width),
                Global.PosY8to16(37, map.Height), true);
        }

        public void SendGotoSerFFMap(MapClient client)
        {
            var p = new SSMG_FF_ENTER();
            p.MapID = client.Character.MapID;
            p.X = Global.PosX16to8(client.Character.X, client.map.Width);
            p.Y = Global.PosY16to8(client.Character.Y, client.map.Height);
            p.Dir = (byte)(client.Character.Dir / 45);
            p.RingHouseID = 30250001;
            if (client.Character.Account.GMLevel >= 100 && client.Character.Ring != null)
                p.RingID = client.Character.Ring.ID;
            p.HouseX = 0xF6DC;
            p.HouseY = 0xFD34;
            p.HouseDir = 0xB6;
            client.NetIo.SendPacket(p);
        }

        public void SerFFFurnitureCastleSetup(MapClient client, CSMG_FF_CASTLE_SETUP p)
        {
            if (client.Character.Account.GMLevel < 100)
            {
                client.SendSystemMessage("您的權限不足");
            }
            else
            {
                if (ser.Furnitures[FurniturePlace.HOUSE].Count > 0)
                {
                    client.SendSystemMessage("无法重复设置");
                    return;
                }

                var item = client.Character.Inventory.GetItem(p.InventorySlot);
                House = new ActorFurnitureUnit();

                client.DeleteItem(p.InventorySlot, 1, false);

                House.MapID = client.Character.MapID;
                House.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(House.MapID);
                House.X = p.X;
                House.Z = p.Z;
                House.Yaxis = p.Yaxis;
                House.Name = item.BaseData.name;
                House.PictID = item.PictID;
                House.e = new NullEventHandler();
                map.RegisterActor(House);
                House.invisble = false;
                map.OnActorVisibilityChange(House);
                ser.Furnitures[FurniturePlace.HOUSE].Add(House);
                MapServer.charDB.SaveSerFF(ser);
            }
        }

        public void SerFFofFurnitureSetup(MapClient client, CSMG_FF_FURNITURE_SETUP p)
        {
            if (client.Character.Account.GMLevel < 100)
            {
                client.SendSystemMessage("您的權限不足");
            }
            else
            {
                var item = client.Character.Inventory.GetItem(p.InventorySlot);
                var actor = new ActorFurniture();
                if (client.Character.Account.GMLevel < 100)
                    client.DeleteItem(p.InventorySlot, 1, false);
                actor.MapID = client.Character.MapID;
                actor.ItemID = item.ItemID;
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.X = p.X;
                actor.Y = p.Y;
                actor.Z = p.Z;
                actor.Xaxis = p.Xaxis;
                actor.Yaxis = p.Yaxis;
                actor.Zaxis = p.Zaxis;
                actor.Name = item.BaseData.name;
                actor.PictID = item.PictID;
                actor.e = new NullEventHandler();
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);

                if (client.Character.MapID == 90001999)
                    ser.Furnitures[FurniturePlace.GARDEN].Add(actor);
                else if (client.Character.MapID == 91000999)
                    ser.Furnitures[FurniturePlace.ROOM].Add(actor);
                client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_SETUP, actor.Name,
                    ser.Furnitures[FurniturePlace.GARDEN].Count +
                    ser.Furnitures[FurniturePlace.ROOM].Count, Configuration.Configuration.Instance.MaxFurnitureCount));

                MapServer.charDB.SaveSerFF(ser);
            }
        }

        public void RemoveFurnitureCastle(MapClient client, CSMG_FF_FURNITURE_REMOVE_CASTLE p)
        {
            if (client.Character.Account.GMLevel < 100)
            {
                client.SendSystemMessage("您的權限不足");
            }
            else
            {
                Map map = null;
                map = MapManager.Instance.GetMap(90001999);
                ser.Furnitures[FurniturePlace.HOUSE].Clear();
                map.DeleteActor(House);
                MapServer.charDB.SaveSerFF(ser);
                var item = ItemFactory.Instance.GetItem(p.ItemID);
                client.AddItem(item, false);
            }
        }

        public void RemoveFurniture(MapClient client, CSMG_FF_FURNITURE_REMOVE p)
        {
            if (client.Character.Account.GMLevel < 250)
            {
                client.SendSystemMessage("哎哟——！");
            }
            else
            {
                Map map = null;
                map = MapManager.Instance.GetMap(90001999);
                if (client.Character.MapID == 91000999)
                    map = MapManager.Instance.GetMap(91000999);
                var actor = map.GetActor(p.ActorID);
                if (actor == null)
                    return;
                if (actor.type != ActorType.FURNITURE)
                    return;
                var furniture = (ActorFurniture)actor;

                if (client.Character.MapID == 90001999)
                    ser.Furnitures[FurniturePlace.GARDEN].Remove(furniture);
                else if (client.Character.MapID == 91000999)
                    ser.Furnitures[FurniturePlace.ROOM].Remove(furniture);
                map.DeleteActor(actor);
                var item = ItemFactory.Instance.GetItem(furniture.ItemID);
                item.PictID = furniture.PictID;
                MapServer.charDB.SaveSerFF(ser);
                client.AddItem(item, false);
                client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.FG_FUTNITURE_REMOVE,
                    furniture.Name, ser.Furnitures[FurniturePlace.GARDEN].Count +
                                    ser.Furnitures[FurniturePlace.ROOM].Count,
                    Configuration.Configuration.Instance.MaxFurnitureCount));
            }
        }
    }
}