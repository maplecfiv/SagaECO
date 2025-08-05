namespace SagaDB.DefWar
{
    public class DefWar
    {
        public DefWar(uint id)
        {
            ID = id;
        }

        public DefWar(DefWarData baseData)
        {
            ID = baseData.ID;
        }

        public uint ID { get; set; }

        /// <summary>
        ///     任务序列
        /// </summary>
        public byte Number { set; get; }

        /// <summary>
        ///     任务信息
        /// </summary>
        public DefWarData Data
        {
            get
            {
                DefWarData baseData = null;
                if (baseData == null)
                {
                    if (DefWarFactory.Instance.Items.ContainsKey(ID))
                        baseData = DefWarFactory.Instance.Items[ID];
                    else
                        baseData = new DefWarData();
                }

                return baseData;
            }
        }


        /// <summary>
        ///     结果1
        /// </summary>
        public byte Result1 { set; get; }

        /// <summary>
        ///     结果2
        /// </summary>
        public byte Result2 { set; get; }

        public byte unknown1 { set; get; }
        public int unknown2 { set; get; }
        public int unknown3 { set; get; }
        public int unknown4 { set; get; }

        public class DefWarData
        {
            /// <summary>
            ///     ID
            /// </summary>
            public uint ID { get; set; }

            /// <summary>
            ///     任务标题
            /// </summary>
            public string Title { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}