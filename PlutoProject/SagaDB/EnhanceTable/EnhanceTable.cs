namespace SagaDB.EnhanceTable
{
    public class EnhanceTable
    {
        /// <summary>
        ///     次數
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     基本機率
        /// </summary>
        public int BaseRate { get; set; }

        /// <summary>
        ///     強化祭加成
        /// </summary>
        public int Matsuri { get; set; }

        /// <summary>
        ///     回收加成
        /// </summary>
        public int Recycle { get; set; }

        /// <summary>
        ///     防止重設加成
        /// </summary>
        public int ResetProtect { get; set; }

        /// <summary>
        ///     防止損毀加成
        /// </summary>
        public int ExplodeProtect { get; set; }

        /// <summary>
        ///     一般水晶加成
        /// </summary>
        public int Crystal { get; set; }

        /// <summary>
        ///     強化水晶加成
        /// </summary>
        public int EnhanceCrystal { get; set; }

        /// <summary>
        ///     超強化水晶加成
        /// </summary>
        public int SPCrystal { get; set; }

        /// <summary>
        ///     強化王加成
        /// </summary>
        public int KingCrystal { get; set; }

        /// <summary>
        ///     奧義加成
        /// </summary>
        public int Okugi { get; set; }

        /// <summary>
        ///     神髓加成
        /// </summary>
        public int Shinzui { get; set; }
    }
}