using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.FFGarden;

namespace SagaDB.Server
{
    public class Server
    {
        /// <summary>
        ///     飞空城的装备
        /// </summary>
        public Dictionary<FFardenSlot, uint> FFardenEquipments { get; } = new Dictionary<FFardenSlot, uint>();

        /// <summary>
        ///     飞空城的家具
        /// </summary>
        public Dictionary<FurniturePlace, List<ActorFurniture>> Furnitures { get; } =
            new Dictionary<FurniturePlace, List<ActorFurniture>>();

        /// <summary>
        ///     飞空庭的家具
        /// </summary>
        public Dictionary<FurniturePlace, List<ActorFurniture>> FurnituresofFG { get; } =
            new Dictionary<FurniturePlace, List<ActorFurniture>>();
    }
}