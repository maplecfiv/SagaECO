namespace SagaMap.Scripting
{
    public enum FGardenParts
    {
        /// <summary>
        ///     飛空庭の土台
        /// </summary>
        Foundation = 0x1,

        /// <summary>
        ///     飛空庭エンジン
        /// </summary>
        Engine = 0x2,

        /// <summary>
        ///     飛空庭の回転帆　1枚
        /// </summary>
        Sail1 = 0x4,

        /// <summary>
        ///     飛空庭の回転帆　2枚
        /// </summary>
        Sail2 = 0x8,

        /// <summary>
        ///     飛空庭の回転帆　3枚
        /// </summary>
        Sail3 = 0x10,

        /// <summary>
        ///     飛空庭の回転帆　4枚
        /// </summary>
        Sail4 = 0x20,

        /// <summary>
        ///     飛空庭の回転帆　5枚
        /// </summary>
        Sail5 = 0x40,

        /// <summary>
        ///     飛空庭の揃った回転帆
        /// </summary>
        SailComplete = 0x80,

        /// <summary>
        ///     飛空庭のろくろ
        /// </summary>
        Wheel = 0x100,

        /// <summary>
        ///     操舵輪
        /// </summary>
        Steer = 0x200,

        /// <summary>
        ///     触媒
        /// </summary>
        Catalyst = 0x400
    }
}