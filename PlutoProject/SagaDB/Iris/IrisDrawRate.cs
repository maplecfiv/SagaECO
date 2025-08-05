using System;

namespace SagaDB.Iris
{
    /// <summary>
    ///     IRIS抽卡
    /// </summary>
    [Serializable]
    public class IrisDrawRate
    {
        public uint SessionID { get; set; }
        public uint PayFlag { get; set; }
        public uint ItemID { get; set; }
        public int CommonRate { get; set; }
        public int UnCommonRate { get; set; }
        public int RatityRate { get; set; }
        public int SuperRatityRate { get; set; }
    }
}