using System.Collections.Generic;

namespace SagaDB.Furniture
{
    public class Furniture
    {
        //ushort motion1,motion2, motion3, motion4, motion5, motion6, motion7;

        public Furniture()
        {
        }

        public Furniture(Furniture baseData)
        {
            ItemID = baseData.ItemID;
        }


        /// <summary>
        ///     Furniture的ID
        /// </summary>
        public uint ItemID { get; set; }

        /// <summary>
        ///     Furniture的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Furniture的PictID
        /// </summary>
        public uint PictID { get; set; }

        /// <summary>
        ///     Furniture的EventID
        /// </summary>
        public uint EventID { get; set; }


        /// <summary>
        ///     預設動作ID
        /// </summary>
        public ushort DefaultMotion { get; set; }


        /// <summary>
        ///     動作ID
        /// </summary>
        public List<ushort> Motion { get; set; } = new List<ushort>();

        /// <summary>
        ///     類別
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        ///     飛空倉庫容量
        /// </summary>
        public ushort Capacity { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}