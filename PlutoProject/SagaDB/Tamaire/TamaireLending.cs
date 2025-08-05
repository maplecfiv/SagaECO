using System;
using System.Collections.Generic;

namespace SagaDB.Tamaire
{
    public class TamaireLending
    {
        public byte maxLendings = 4;

        public uint Lender { get; set; }

        public byte Baselv { get; set; }

        public byte JobType { get; set; }

        public string Comment { get; set; }

        public List<uint> Renters { get; set; } = new List<uint>();

        public DateTime PostDue { get; set; }

        /// <summary>
        ///     玩家最大可借出的"心"的數量
        /// </summary>
        public byte MaxLendings
        {
            get => maxLendings;
            set => maxLendings = value;
        }
    }
}