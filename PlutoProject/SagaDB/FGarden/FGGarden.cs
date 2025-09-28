using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SagaDB.Actor;
using SagaLib;

namespace SagaDB.FlyingGarden
{
    public enum FlyingGardenSlot
    {
        FLYING_BASE,
        FLYING_SAIL,
        GARDEN_FLOOR,
        GARDEN_MODELHOUSE,
        HouseOutSideWall,
        HouseRoof,
        ROOM_FLOOR,
        ROOM_WALL,
    }

    public enum FurniturePlace
    {
        GARDEN,
        ROOM,
    }

    public class FlyingGarden
    {
        ActorPC owner;
        ActorEvent ropeActor;
        uint mapID;
        uint roomMapID;
        uint id;

        Dictionary<FlyingGardenSlot, uint> equips = new Dictionary<FlyingGardenSlot, uint>();
        Dictionary<FurniturePlace, List<Actor.ActorFurniture>> furnitures = new Dictionary<FurniturePlace, List<ActorFurniture>>();

        public FlyingGarden(ActorPC pc)
        {
            this.owner = pc;
            equips.Add(FlyingGardenSlot.FLYING_BASE, 0);
            equips.Add(FlyingGardenSlot.FLYING_SAIL, 0);
            equips.Add(FlyingGardenSlot.GARDEN_FLOOR, 0);
            equips.Add(FlyingGardenSlot.GARDEN_MODELHOUSE, 0);
            equips.Add(FlyingGardenSlot.HouseOutSideWall, 0);
            equips.Add(FlyingGardenSlot.HouseRoof, 0);
            equips.Add(FlyingGardenSlot.ROOM_FLOOR, 0);
            equips.Add(FlyingGardenSlot.ROOM_WALL, 0);
            furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
            furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
        }

        /// <summary>
        /// 飞空庭的ID
        /// </summary>
        public uint ID { get { return this.id; } set { this.id = value; } }

        /// <summary>
        /// 飞空庭的主人
        /// </summary>
        public ActorPC Owner { get { return this.owner; } set { this.owner = value; } }

        /// <summary>
        /// 飞空庭绳子的Actor
        /// </summary>
        public ActorEvent RopeActor { get { return this.ropeActor; } set { this.ropeActor = value; } }

        /// <summary>
        /// 飞空庭地图ID
        /// </summary>
        public uint MapID { get { return this.mapID; } set { this.mapID = value; } }

        /// <summary>
        /// 飞空庭房间地图ID
        /// </summary>
        public uint RoomMapID { get { return this.roomMapID; } set { this.roomMapID = value; } }

        /// <summary>
        /// 飞空庭的装备
        /// </summary>
        public Dictionary<FlyingGardenSlot, uint> FlyingGardenEquipments { get { return this.equips; } }

        /// <summary>
        /// 飞空庭的家具
        /// </summary>
        public Dictionary<FurniturePlace, List<Actor.ActorFurniture>> Furnitures { get { return this.furnitures; } }

        /// <summary>
        ///     飞空庭的燃料
        /// </summary>
        public uint Fuel { get; set; }
    }
}
