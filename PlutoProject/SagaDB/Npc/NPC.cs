namespace SagaDB.Npc
{
    public class NPC
    {
        /// <summary>
        ///     NPC的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     NPC的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     NPC所在地图
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     NPC的X坐标
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        ///     NPC的Y坐标
        /// </summary>
        public byte Y { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}