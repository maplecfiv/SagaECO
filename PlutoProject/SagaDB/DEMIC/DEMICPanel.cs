using System.Collections.Generic;

namespace SagaDB.DEMIC
{
    public class DEMICPanel
    {
        public List<Chip> Chips { get; set; } = new List<Chip>();

        public byte EngageTask1 { get; set; } = 255;

        public byte EngageTask2 { get; set; } = 255;
    }
}