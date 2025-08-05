using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaDB.FFarden
{
    public enum FFardenSlot
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
        ROOM,
        FARM,
        FISHERY,
        HOUSE
    }

    public class FFarden
    {
        /// <summary>
        ///     飞空城的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     飞空城的简介
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     飞空城是否需要密码
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        ///     飞空城所属工会
        /// </summary>
        public uint RingID { get; set; }

        /// <summary>
        ///     飞空城的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     飞空城的主人
        /// </summary>
        public ActorPC Owner { get; set; }

        /// <summary>
        ///     飞空城地图ID
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     飞空城状态(00 = 无入手   01 = 作出可能  03 = 已入手)
        /// </summary>
        public byte ObMode { get; set; }

        /// <summary>
        ///     飞空城健康(00 = 正常 01 = 停滞状态 02 = 扣押状态 03 = 维持不能)
        /// </summary>
        public byte HealthMode { get; set; }

        /// <summary>
        ///     飞空城的所持材料数(所持マテリアルポイント)
        /// </summary>
        public uint MaterialPoint { get; set; }

        /// <summary>
        ///     飞空城等级
        /// </summary>
        public uint Level { get; set; }

        /// <summary>
        ///     飞空城等级的经验值
        /// </summary>
        public uint FFexp { get; set; }

        /// <summary>
        ///     飞空城F系等级
        /// </summary>
        public uint FLevel { get; set; }

        /// <summary>
        ///     飞空城F系经验值
        /// </summary>
        public uint FFFexp { get; set; }

        /// <summary>
        ///     飞空城SU系等级
        /// </summary>
        public uint SULevel { get; set; }

        /// <summary>
        ///     飞空城SU系经验值
        /// </summary>
        public uint FFSUexp { get; set; }

        /// <summary>
        ///     飞空城BP系等级
        /// </summary>
        public uint BPLevel { get; set; }

        /// <summary>
        ///     飞空城BP系经验值
        /// </summary>
        public uint FFBPexp { get; set; }

        /// <summary>
        ///     飞空城DEM系等级
        /// </summary>
        public uint DEMLevel { get; set; }

        /// <summary>
        ///     飞空城DEM系经验值
        /// </summary>
        public uint FFDEMexp { get; set; }

        /// <summary>
        ///     飞空城的材料点数消耗(マテリアルコスト)
        /// </summary>
        public uint MaterialConsume { get; set; }

        /// <summary>
        ///     飞空城房间地图ID
        /// </summary>
        public uint RoomMapID { get; set; }

        /// <summary>
        ///     飞空城的装备
        /// </summary>
        public Dictionary<FFardenSlot, uint> FFardenEquipments { get; } = new Dictionary<FFardenSlot, uint>();

        /// <summary>
        ///     飞空城的家具
        /// </summary>
        public Dictionary<FurniturePlace, List<ActorFurniture>> Furnitures { get; } =
            new Dictionary<FurniturePlace, List<ActorFurniture>>();
    }
}