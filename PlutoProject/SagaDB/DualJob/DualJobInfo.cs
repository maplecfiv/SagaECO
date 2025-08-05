using System;

namespace SagaDB.DualJob
{
    /// <summary>
    ///     副职业基本信息
    /// </summary>
    [Serializable]
    public class DualJobInfo
    {
        /// <summary>
        ///     基本职业ID
        /// </summary>
        public byte BaseJobID = 0;

        /// <summary>
        ///     3转职业ID
        /// </summary>
        public byte ChronicleJobID = 0;

        /// <summary>
        ///     副职描述
        /// </summary>
        public string Description = "";

        /// <summary>
        ///     副职ID
        /// </summary>
        public byte DualJobID = 0;

        /// <summary>
        ///     副职名称
        /// </summary>
        public string DualJobName = "";

        /// <summary>
        ///     2-1职业ID
        /// </summary>
        public byte ExperJobID = 0;

        /// <summary>
        ///     2-2职业ID
        /// </summary>
        public byte TechnicalJobID = 0;
    }
}