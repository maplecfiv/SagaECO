using System;

namespace SagaDB.Iris
{
    /// <summary>
    ///     伊利斯扭蛋
    /// </summary>
    [Serializable]
    public class IrisGacha
    {
        public uint Count;
        public uint ItemID;
        public uint PageID;
        public uint PayFlag;
        public uint SessionID;
        public string SessionName;
    }
}