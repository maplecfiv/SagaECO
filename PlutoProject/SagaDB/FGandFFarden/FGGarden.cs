using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaDB.FGarden
{
    public enum FGardenSlot
    {
        FLYING_BASE,
        FLYING_SAIL,
        GARDEN_FLOOR,
        GARDEN_MODELHOUSE,
        HouseOutSideWall,
        HouseRoof,
        ROOM_FLOOR,
        ROOM_WALL
    }

    public enum FurniturePlace
    {
        GARDEN,
        ROOM
    }

    public class FGarden
    {
        public FGarden(ActorPC pc)
        {
            Owner = pc;
            FGardenEquipments.Add(FGardenSlot.FLYING_BASE, 0);
            FGardenEquipments.Add(FGardenSlot.FLYING_SAIL, 0);
            FGardenEquipments.Add(FGardenSlot.GARDEN_FLOOR, 0);
            FGardenEquipments.Add(FGardenSlot.GARDEN_MODELHOUSE, 0);
            FGardenEquipments.Add(FGardenSlot.HouseOutSideWall, 0);
            FGardenEquipments.Add(FGardenSlot.HouseRoof, 0);
            FGardenEquipments.Add(FGardenSlot.ROOM_FLOOR, 0);
            FGardenEquipments.Add(FGardenSlot.ROOM_WALL, 0);
            Furnitures.Add(FurniturePlace.GARDEN, new List<ActorFurniture>());
            Furnitures.Add(FurniturePlace.ROOM, new List<ActorFurniture>());
        }

        /// <summary>
        ///     飞空庭的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     飞空庭的主人
        /// </summary>
        public ActorPC Owner { get; set; }

        /// <summary>
        ///     飞空庭绳子的Actor
        /// </summary>
        public ActorEvent RopeActor { get; set; }

        /// <summary>
        ///     飞空庭地图ID
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     飞空庭房间地图ID
        /// </summary>
        public uint RoomMapID { get; set; }

        /// <summary>
        ///     飞空庭的装备
        /// </summary>
        public Dictionary<FGardenSlot, uint> FGardenEquipments { get; } = new Dictionary<FGardenSlot, uint>();

        /// <summary>
        ///     飞空庭的家具
        /// </summary>
        public Dictionary<FurniturePlace, List<ActorFurniture>> Furnitures { get; } =
            new Dictionary<FurniturePlace, List<ActorFurniture>>();

        /// <summary>
        ///     飞空庭的燃料
        /// </summary>
        public uint Fuel { get; set; }
    }
}