using System.Collections.Generic;

namespace SagaDB.DEMIC
{
    public class Model
    {
        public uint ID { get; set; }

        public byte CenterX { get; set; }

        public byte CenterY { get; set; }

        public List<byte[]> Cells { get; } = new List<byte[]>();
    }
}