namespace SagaDB.Actor
{
    /// <summary>
    ///     家具Actor
    /// </summary>
    public class ActorFurniture : Actor
    {
        public ActorFurniture()
        {
            type = ActorType.FURNITURE;
        }

        /// <summary>
        ///     道具ID
        /// </summary>
        public uint ItemID { get; set; }

        /// <summary>
        ///     图像ID，用于确定怪物雕像的怪物ID
        /// </summary>
        public uint PictID { get; set; }

        /// <summary>
        ///     Z坐标
        /// </summary>
        public short Z { get; set; }

        /// <summary>
        ///     Z轴旋转
        /// </summary>
        public short Xaxis { get; set; }

        /// <summary>
        ///     Z轴旋转
        /// </summary>
        public short Yaxis { get; set; }

        /// <summary>
        ///     Z轴旋转
        /// </summary>
        public short Zaxis { get; set; }


        /// <summary>
        ///     动作
        /// </summary>
        public ushort Motion { get; set; } = 111;
    }
}